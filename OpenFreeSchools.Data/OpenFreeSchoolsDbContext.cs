using Ardalis.GuardClauses;
using OpenFreeSchools.Data.Auditing;
using OpenFreeSchools.Data.Conventions;
using OpenFreeSchools.Data.Models;
using OpenFreeSchools.UserContext;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Transactions;
using OpenFreeSchools.Data.Models.Projects;

namespace OpenFreeSchools.Data
{
	public partial class OpenFreeSchoolsDbContext : DbContext
	{
		private readonly IServerUserInfoService _userInfoService;

		public OpenFreeSchoolsDbContext()
		{
		}

		public OpenFreeSchoolsDbContext(DbContextOptions<OpenFreeSchoolsDbContext> options, IServerUserInfoService userInfoService)
			: base(options)
		{
			Guard.Against.Null(userInfoService);
			_userInfoService = userInfoService;
		}

		public virtual DbSet<Audit> Audits { get; set; }
		public virtual DbSet<Project> Projects { get; set; }

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			var userName = GetCurrentUsername();
			var auditedEntities = GetAuditsNeeded();
			int auditsWritten = 0;

			using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
			{
				// commit changes to get IDs for new records. Then commit audits as they refer to those IDs. Use transaction to make the two commits atomic.
				var entriesWritten = base.SaveChanges(acceptAllChangesOnSuccess);
				InsertAuditsNeeded(userName, auditedEntities);

				if (auditedEntities.Count > 0)
				{
					auditsWritten = base.SaveChanges(acceptAllChangesOnSuccess);
				}
				scope.Complete();

				return entriesWritten + auditsWritten;
			}
		}

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			var userName = GetCurrentUsername();
			var auditedEntities = GetAuditsNeeded();
			int auditsWritten = 0;

			using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
			{
				// commit changes to get IDs for new records. Then commit audits as they refer to those IDs. Use transaction to make the two commits atomic.
				var entriesWritten = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(true);

				if (auditedEntities.Count > 0)
				{
					await InsertAuditsNeededAsync(userName, auditedEntities, cancellationToken);
					auditsWritten = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(true);
				}

				scope.Complete();

				return entriesWritten + auditsWritten;
			}
		}

		protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
		{
			configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseConcernsSqlServer("Data Source=127.0.0.1;Initial Catalog=local_trams_test_db;persist security info=True;User id=sa; Password=StrongPassword905");
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

			base.OnModelCreating(modelBuilder);
		}


		/// <summary>
		/// Takes an IAuditable and coverts it into an Audit entity that can be added to the db context.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="userName"></param>
		/// <param name="changeType"></param>
		/// <returns></returns>
		private Audit BuildAudit(IAuditable entity, string userName, AuditChangeType changeType) => new Audit(entity.GetType().Name, userName, DateTimeOffset.Now, changeType, Serialise(entity));


		/// <summary>
		/// Returns the audits needed - audit type and the entity being audited
		/// </summary>
		/// <returns></returns>
		private List<(AuditChangeType AuditType, object Entity)> GetAuditsNeeded()
		{
			this.ChangeTracker.DetectChanges();

			var auditsNeeded = this.ChangeTracker.Entries()
					.Where(t => EntityStateToAuditChangeTypeMap.IsSupported(t.State) && t.Entity is IAuditable)
					.Select(t => (EntityStateToAuditChangeTypeMap.Map(t.State), t.Entity))
					.ToList();
			return auditsNeeded;
		}

		private string GetCurrentUsername()
		{
			if (string.IsNullOrWhiteSpace(_userInfoService.UserInfo?.Name) && Debugger.IsAttached)
			{
			//	Debugger.Break();
			}
			return _userInfoService.UserInfo?.Name ?? "Unknown";
		}

		/// <summary>
		/// Takes the list of audit entries needed and writes them to the dbset, ready to be saved.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="auditedEntities"></param>
		private void InsertAuditsNeeded(string userName, List<(AuditChangeType AuditType, object Entity)> auditedEntities)
		{
			var audits = auditedEntities.Select(x => BuildAudit((IAuditable)x.Entity, userName, x.AuditType)).ToArray();
			this.Audits.AddRange(audits);
		}

		private async Task InsertAuditsNeededAsync(string userName, List<(AuditChangeType AuditType, object Entity)> auditedEntities, CancellationToken cancellationToken)
		{
			var audits = auditedEntities.Select(x => BuildAudit((IAuditable)x.Entity, userName, x.AuditType)).ToArray();
			await this.Audits.AddRangeAsync(audits, cancellationToken);
		}

		private string Serialise(object obj) => JsonSerializer.Serialize(obj, new JsonSerializerOptions()
		{
			ReferenceHandler = ReferenceHandler.IgnoreCycles,
			WriteIndented = false,
			AllowTrailingCommas = true,
		});
	}
}