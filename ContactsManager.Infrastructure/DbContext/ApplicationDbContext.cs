using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
	{
		public virtual DbSet<Country> Countries { get; set; }
		public virtual DbSet<Person> Persons { get; set; }

		public ApplicationDbContext(DbContextOptions options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Chưa rõ tác dụng của 2 dòng dưới này,
			// đã test khi xóa 2 dòng dưới này, và chạy migration. => vẫn hoạt động bình thường
			modelBuilder.Entity<Country>().ToTable("Countries");
			modelBuilder.Entity<Person>().ToTable("Persons");

			// Seed to Countries
			string countriesJson = System.IO.File.ReadAllText("countries.json");
			List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson)!;

			foreach (Country country in countries)
			{
				modelBuilder.Entity<Country>().HasData(country);
			}

			//Seed to Persons
			string personsJson = System.IO.File.ReadAllText("persons.json");
			List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson)!;

			foreach (Person person in persons)
				modelBuilder.Entity<Person>().HasData(person);
		}
	}
}
