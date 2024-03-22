using ContactsManager.Core.DTOs;
using ContactsManager.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonService
    {
        public Task<PersonResponse> AddPerson(PersonAddRequest? personResponse);

        public Task<List<PersonResponse>> GetAllPersons();

        public Task<PersonResponse?> GetPersonById(Guid? personId);

        public Task<List<PersonResponse>> GetFilterPersons(string searchBy, string? searchString);

        public Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPerons, string sortBy, SortOrderOptions sortOrder);

        public Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        public Task<bool> DeletePerson(Guid? personId);

        public Task<MemoryStream> GetPersonsMemoryStreamCSV();

        public Task<MemoryStream> GetPersonsMemoryStreamExcel();

	}
}
