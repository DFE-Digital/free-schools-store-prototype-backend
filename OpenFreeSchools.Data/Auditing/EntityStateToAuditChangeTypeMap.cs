using OpenFreeSchools.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace OpenFreeSchools.Data.Auditing
{
	public static class EntityStateToAuditChangeTypeMap
	{
		public static AuditChangeType Map(EntityState entityState)
		{
			switch (entityState)
			{
				case EntityState.Added:
					return AuditChangeType.INSERT;

				case EntityState.Modified:
					return AuditChangeType.UPDATE;

				case EntityState.Deleted:
					return AuditChangeType.DELETE;

				default: throw new NotSupportedException($"Entity state not supported for auditing: State: {entityState}");
			}
		}

		public static bool IsSupported(EntityState entityState)
		{
			switch (entityState)
			{
				case EntityState.Added:
				case EntityState.Modified:
				case EntityState.Deleted:
					return true;
				default:
					return false;
			}
		}
	}
}
