using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
	public class PersonRepository : IPersonRepository
	{
		private readonly ApplicationDbContext _db;

		public PersonRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<Person> AddPerson(Person person)
		{
			_db.Persons.Add(person);
			await _db.SaveChangesAsync();

			return person;
		}

		public async Task<List<Person>> GetAllPersons()
		{
			return await _db.Persons.Include("Country").ToListAsync();
		}

		public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
		{
			return await _db.Persons.Include("Country")
			.Where(predicate)
			.ToListAsync();
		}

		public async Task<Person?> GetPersonByPersonID(Guid personID)
		{
			return await _db.Persons.Include("Country")
				.FirstOrDefaultAsync(temp => temp.PersonId == personID);
		}

		public async Task<Person> UpdatePerson(Person person)
		{
			Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == person.PersonId);

			if (matchingPerson == null)
				return person;

			matchingPerson.PersonName = person.PersonName;
			matchingPerson.Email = person.Email;
			matchingPerson.DateOfBirth = person.DateOfBirth;
			matchingPerson.Gender = person.Gender;
			matchingPerson.CountryId = person.CountryId;
			matchingPerson.Address = person.Address;
			matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

			int countUpdated = await _db.SaveChangesAsync();

			return matchingPerson;
		}

		public async Task<bool> DeletePersonById(Guid personID)
		{
			_db.Persons.RemoveRange(_db.Persons.Where(temp => temp.PersonId == personID));
			int rowsDeleted = await _db.SaveChangesAsync();

			return rowsDeleted > 0;
		}
	}
}
