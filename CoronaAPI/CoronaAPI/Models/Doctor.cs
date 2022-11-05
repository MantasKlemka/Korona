using System.ComponentModel.DataAnnotations;

namespace CoronaAPI.Models
{
    public class Doctor
    {
        public int? Id { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Surname { get; set; }
        [Required]
        public string? Password { get; set; }
        public bool Activated { get; set; }

        public Doctor(int id, string email, string name, string surname, string password, bool activated)
        {
            Id = id;
            Email = email;
            Name = name;
            Surname = surname;
            Password = password;
            Activated = activated;
        }

        public Doctor(string email, string name, string surname, string password, bool activated)
        {
            Email = email;
            Name = name;
            Surname = surname;
            Password = password;
            Activated = activated;
        }

        public Doctor(string email, string name, string surname, string password)
        {
            Email = email;
            Name = name;
            Surname = surname;
            Password = password;
        }

        public Doctor()
        {

        }
    }
}
