namespace MicroInventory.Person.Api.Application.Dtos
{
    public class PersonDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }  // Opsiyonel

        public string PhoneNumber { get; set; }  // Opsiyonel

        public string Department { get; set; }  // Örn: IT, Muhasebe
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
