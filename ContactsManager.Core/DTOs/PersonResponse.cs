using Entities;

namespace ContactsManager.Core.DTOs
{
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse otherPersonResponse = (PersonResponse)obj;

            return PersonId == otherPersonResponse.PersonId && PersonName == otherPersonResponse.PersonName && Email == otherPersonResponse.Email && DateOfBirth == otherPersonResponse.DateOfBirth && Gender == otherPersonResponse.Gender && CountryId == otherPersonResponse.CountryId && CountryName == otherPersonResponse.CountryName && Address == otherPersonResponse.Address && ReceiveNewsLetters == otherPersonResponse.ReceiveNewsLetters;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"PersonId: {PersonId}, Person Name: {PersonName}, Email: {Email}, DOB: {DateOfBirth}, Gender: {Gender}, CountryId: {CountryId}, CountryName: {CountryName}, Address: {Address}, Receive News Letters: {ReceiveNewsLetters}.";
        }


    }

    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                CountryName = person.Country?.CountryName,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth).Value.TotalDays / 365.25) : null,
            };
        }
    }
}
