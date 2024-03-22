using ContactsManager.Core.DTOs;
using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using ServiceContracts;

namespace CRUDExample.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;

        public PersonController(IPersonService personService, ICountriesService countriesService)
        {
            _personService = personService;
            _countriesService = countriesService;
        }

        [Route("/")]
        [Route("/persons/index")]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrderOptions = SortOrderOptions.ASC)
        {
            // Searching
            ViewData["SearchFields"] = new Dictionary<string, string>()
            {
                {
                    nameof(PersonResponse.PersonName), "Person Name"
                },
				{
					nameof(PersonResponse.Email), "Email"
				},
				{
					nameof(PersonResponse.DateOfBirth), "Date of Birth"
				},
				{
					nameof(PersonResponse.Gender), "Gender"
				},
				{
					nameof(PersonResponse.CountryName), "Country"
				},
				{
					nameof(PersonResponse.Address), "Address"
				},
			};

            List<PersonResponse> persons = await _personService.GetFilterPersons(searchBy, searchString);

            ViewData["CurrentSearchBy"] = searchBy;
			ViewData["CurrentSearchString"] = searchString;

            // Sorting
            List<PersonResponse> sortedPersons = await _personService.GetSortedPersons(persons, sortBy, sortOrderOptions);

            ViewData["CurrentSortBy"] = sortBy;
            ViewData["CurrentSortOrder"] = sortOrderOptions;

			return View(sortedPersons);
        }

        [HttpGet]
        [Route("persons/create")]    
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewData["Countries"] = countries;

			return View();
        }

        [HttpPost]
		[Route("persons/create")]
		public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
			/*
             * Code này được comment vì đã thêm Client-side Validation rồi => Giảm tải tài nguyên Server
             * Server-side Validation
            if (!ModelState.IsValid)
            {
				List<CountryResponse> countries = _countriesService.GetAllCountries();
				ViewData["Countries"] = countries;

				ViewData["Errors"] = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                // SelectMany khác Select

                return View();
			}
            */

			await _personService.AddPerson(personAddRequest);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/persons/edit/{personId}")]
        public async Task<IActionResult> Edit(Guid personId)
        {

			List<CountryResponse> countries = await _countriesService.GetAllCountries();
			ViewData["Countries"] = countries;

			PersonResponse personResponse = (await _personService.GetPersonById(personId))!;

            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest()
            {
                PersonId = personResponse.PersonId,
                PersonName = personResponse.PersonName,
                Email = personResponse.Email,
                DateOfBirth = personResponse.DateOfBirth,
                Gender = personResponse.Gender == null ? null : (GenderOptions)Enum.Parse(typeof(GenderOptions), personResponse.Gender),
                CountryId = personResponse.CountryId,
                Address = personResponse.Address,
                ReceiveNewsLetters = personResponse.ReceiveNewsLetters,
			};

			return View(personUpdateRequest);
        }

		[HttpPost]
		[Route("/persons/edit/{personId}")]
		public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
		{
            await _personService.UpdatePerson(personUpdateRequest);
			return RedirectToAction("Index");
		}

		[HttpGet]
		[Route("/persons/delete/{personId}")]
		public async Task<IActionResult> DeleteView(Guid personId)
		{
			PersonResponse personResponse = (await _personService.GetPersonById(personId))!;
			return View("Delete", personResponse);
		}

        [HttpPost]
        [Route("/persons/delete/{personId}")]
        public async Task<IActionResult> Delete(Guid personId)
        {
            await _personService.DeletePerson(personId);
            return RedirectToAction("Index");
        }

        [Route("/persons/personsPDF")]
        public async Task<IActionResult> PersonsPDF()
        {
            List<PersonResponse> persons = await _personService.GetAllPersons();
            return new ViewAsPdf(persons, ViewData)
            {
				// Nếu gán thuộc tính FileName thì khi được kích hoạt, nó sẽ tải file PDF về mới tên file là "FileName"
                // Còn nếu không gán thuộc tính FileName, thì nó sẽ mở ra 1 trang PDF
				// FileName = "PersonData",
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 20,
                    Left = 20,
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            };
        }

        [Route("/persons/personsCSV")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personService.GetPersonsMemoryStreamCSV();
            return File(memoryStream, "application/octet-stream", "persons.csv");
        }

		[Route("/persons/personsExcel")]
		public async Task<IActionResult> PersonsExcel()
		{
			MemoryStream memoryStream = await _personService.GetPersonsMemoryStreamExcel();
			return File(memoryStream, "application/vnd.ms-excel", "persons.xlsx");
		}
	}
}
