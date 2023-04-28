using OpenFreeSchools.Data;
using OpenFreeSchools.UserContext;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace OpenFreeSchools.API.Tests
{
	public class DbFixture : IDisposable
	{
		private readonly IDbContextTransaction _concernsTransaction;
		public readonly string ConnString;


		public DbFixture()
		{
			var userInfoService = new ServerUserInfoService()
			{
				UserInfo = new UserInfo() { Name = "API.Test.DbFixture@test.gov.uk", Roles = new[] { Claims.CaseWorkerRoleClaim } }
			};

			var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.tests.json");

			var config = new ConfigurationBuilder()
				.AddJsonFile(configPath)
				.Build();

			ConnString = config.GetConnectionString("DefaultConnection");

			var contextBuilder = new DbContextOptionsBuilder<OpenFreeSchoolsDbContext>();

			contextBuilder.UseSqlServer(ConnString);
			OpenFreeSchoolsDbContext concernsDbContext = new(contextBuilder.Options, userInfoService);
			concernsDbContext.Database.Migrate();

			_concernsTransaction = concernsDbContext.Database.BeginTransaction();
		}

		public void Dispose()
		{
			_concernsTransaction.Rollback();
			_concernsTransaction.Dispose();
			GC.SuppressFinalize(this);
		}
	}

	[CollectionDefinition("Database", DisableParallelization = true)]
	public class DatabaseCollection : ICollectionFixture<DbFixture>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}
}
