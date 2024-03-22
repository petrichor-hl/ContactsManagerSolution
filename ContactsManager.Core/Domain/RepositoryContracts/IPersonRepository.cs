using Entities;
using System.Linq.Expressions;

namespace RepositoryContracts
{
	public interface IPersonRepository
	{
		Task<Person> AddPerson(Person person);

		Task<List<Person>> GetAllPersons();

		Task<Person?> GetPersonByPersonID(Guid personID);

		Task<Person> UpdatePerson(Person person);

		Task<bool> DeletePersonById(Guid personID);
	}
}
