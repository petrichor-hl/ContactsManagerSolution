using CsvHelper;
using Entities;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ContactsManager.Core.DTOs;
using ContactsManager.Core.Enums;
using Services.Helpers;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Services
{
    public class PersonService : IPersonService
    {
		private readonly IPersonRepository _personsRepository;

		public PersonService(IPersonRepository personsRepository)
		{
			_personsRepository = personsRepository;
		}

		public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
		{
			if (personAddRequest == null)
			{
				throw new ArgumentNullException(nameof(personAddRequest));
			}

			// Model Validation
			ValidationHelper.ModelValidation(personAddRequest);

			Person person = personAddRequest.ToPerson();
			person.PersonId = Guid.NewGuid();

			await _personsRepository.AddPerson(person);

			return person.ToPersonResponse();
		}

		public async Task<List<PersonResponse>> GetAllPersons()
		{
			var persons = await _personsRepository.GetAllPersons();

			return persons
			  .Select(temp => temp.ToPersonResponse()).ToList();
		}

		public async Task<PersonResponse?> GetPersonById(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

			Person? matchingPerson = await _personsRepository.GetPersonByPersonID(personId.Value);

			return matchingPerson?.ToPersonResponse();
		}

        public async Task<List<PersonResponse>> GetFilterPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = await GetAllPersons();

			List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchString))
            {
                return matchingPersons;
            }

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(p => string.IsNullOrEmpty(p.PersonName) || p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(p => string.IsNullOrEmpty(p.Email) || p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(p => p.DateOfBirth == null || p.DateOfBirth.Value.ToString().Contains(searchString)).ToList();
                    break;
                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(p => string.IsNullOrEmpty(p.Gender) || p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                case nameof(PersonResponse.CountryName):
                    matchingPersons = allPersons.Where(p => p.CountryName == null || p.CountryName.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(p => string.IsNullOrEmpty(p.Address) || p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

				default: 
                    matchingPersons = allPersons; 
                    break;
			}

            return matchingPersons;
        }

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPerons, string sortBy, SortOrderOptions sortOrder)
        {
            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                ((nameof(PersonResponse.PersonName)), SortOrderOptions.ASC)
                => allPerons.OrderBy(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                ((nameof(PersonResponse.PersonName)), SortOrderOptions.DESC)
                => allPerons.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                ((nameof(PersonResponse.Email)), SortOrderOptions.ASC)
                => allPerons.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                ((nameof(PersonResponse.Email)), SortOrderOptions.DESC)
                => allPerons.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                ((nameof(PersonResponse.Gender)), SortOrderOptions.ASC)
                => allPerons.OrderBy(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                ((nameof(PersonResponse.Gender)), SortOrderOptions.DESC)
                => allPerons.OrderByDescending(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                ((nameof(PersonResponse.CountryName)), SortOrderOptions.ASC)
                => allPerons.OrderBy(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                ((nameof(PersonResponse.CountryName)), SortOrderOptions.DESC)
                => allPerons.OrderByDescending(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                ((nameof(PersonResponse.DateOfBirth)), SortOrderOptions.ASC)
                => allPerons.OrderBy(p => p.DateOfBirth).ToList(),

                ((nameof(PersonResponse.DateOfBirth)), SortOrderOptions.DESC)
                => allPerons.OrderByDescending(p => p.DateOfBirth).ToList(),

                ((nameof(PersonResponse.Age)), SortOrderOptions.ASC)
                => allPerons.OrderBy(p => p.Age).ToList(),

                ((nameof(PersonResponse.Age)), SortOrderOptions.DESC)
                => allPerons.OrderByDescending(p => p.Age).ToList(),

				((nameof(PersonResponse.Address)), SortOrderOptions.ASC)
				=> allPerons.OrderBy(p => p.Address).ToList(),

				((nameof(PersonResponse.Address)), SortOrderOptions.DESC)
				=> allPerons.OrderByDescending(p => p.Address).ToList(),

				((nameof(PersonResponse.ReceiveNewsLetters)), SortOrderOptions.ASC)
				=> allPerons.OrderBy(p => p.ReceiveNewsLetters).ToList(),

				((nameof(PersonResponse.ReceiveNewsLetters)), SortOrderOptions.DESC)
				=> allPerons.OrderByDescending(p => p.ReceiveNewsLetters).ToList(),

				_ => allPerons
            };

            return await Task.FromResult(sortedPersons);
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) 
            { 
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            ValidationHelper.ModelValidation(personUpdateRequest);

			// Get Matching Person Object
			Person? matchingPerson = await _personsRepository.GetPersonByPersonID(personUpdateRequest.PersonId);

			if (matchingPerson == null)
            {
                throw new ArgumentException();
            }

            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

			await _personsRepository.UpdatePerson(matchingPerson); //UPDATE

			return matchingPerson.ToPersonResponse();
		}

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

			Person? matchingPerson = await _personsRepository.GetPersonByPersonID(personId.Value);

			if ( matchingPerson == null)
            {
                return false;
            }

			await _personsRepository.DeletePersonById(personId.Value);

			return true;
        }

		public async Task<MemoryStream> GetPersonsMemoryStreamCSV()
		{
			MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            // streamWriter writes content into memoryStream

            CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, true);
            csvWriter.WriteHeader<PersonResponse>();
            csvWriter.NextRecord();

            List<PersonResponse> persons = await GetAllPersons();

            await csvWriter.WriteRecordsAsync(persons);

            memoryStream.Position = 0;

            return memoryStream;
		}

		public async Task<MemoryStream> GetPersonsMemoryStreamExcel()
		{
			MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");

				worksheet.Cells["A1"].Value = "Person Name";
				worksheet.Cells["B1"].Value = "Email";
				worksheet.Cells["C1"].Value = "Date of Birth";
				worksheet.Cells["D1"].Value = "Age";
				worksheet.Cells["E1"].Value = "Gender";
				worksheet.Cells["F1"].Value = "Country";
				worksheet.Cells["G1"].Value = "Address";
				worksheet.Cells["H1"].Value = "Receive News Letters";

				using (ExcelRange headerCells = worksheet.Cells["A1:H1"])
				{
					headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
					headerCells.Style.Font.Bold = true;
				}

                // Dòng 1 dùng để chứa Header Column
                // Dữ liệu record sẽ bắt đầu từ dòng 2 trong Excel
                int row = 2;
                List<PersonResponse> persons = await GetAllPersons();

				foreach (PersonResponse person in persons)
				{
					worksheet.Cells[row, 1].Value = person.PersonName;
					worksheet.Cells[row, 2].Value = person.Email;
					worksheet.Cells[row, 3].Value = person.DateOfBirth?.ToString("dd/MM/yyyy");
					worksheet.Cells[row, 4].Value = person.Age;
					worksheet.Cells[row, 5].Value = person.Gender;
					worksheet.Cells[row, 6].Value = person.CountryName;
					worksheet.Cells[row, 7].Value = person.Address;
					worksheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

					row++;
				}

				worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

				await excelPackage.SaveAsync();
			}

			memoryStream.Position = 0;
			return memoryStream;
		}
	}
}
