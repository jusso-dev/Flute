using Flute.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Flute.Shared.Models
{
	public class UserDbContextContext : DbContext
	{
		public UserDbContextContext(DbContextOptions<UserDbContextContext> options)
			: base(options)
		{ }

		public DbSet<AuthenticatedUserModel> Users { get; set; }
		public DbSet<UsersUploadedModels> UploadedModels { get; set; }
	}
}
