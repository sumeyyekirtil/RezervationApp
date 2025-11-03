using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RezervationApp.Entities;

namespace RezervationApp.Data
{
	public class DatabaseContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Slider> Sliders { get; set; }
		public DbSet<Service> Services { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Rezervation> Rezervations { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=ASUS-PRO; database=RezervationAppSH; integrated security=true; TrustServerCertificate=True;").ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasData(
				new User()
				{
					Id = 1,
					CreateDate = DateTime.Now,
					Name = "Admin",
					Surname = "User",
					Email = "test@gmail.com",
					IsActive = true,
					IsAdmin = true,
					Password = "222"
				}
			);
		}
	}
}
