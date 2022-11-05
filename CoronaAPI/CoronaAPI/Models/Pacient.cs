using System.ComponentModel.DataAnnotations;

namespace CoronaAPI.Models
{
    public class Pacient
    {
        public int? Id { get; set; }
        [Required]
        public string? IdentificationCode { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Surname { get; set; }
        [Required]
        public string? BirthDate { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public int? Doctor { get; set; }

        public Pacient(int id, string identificationCode, string name, string surname, string birthDate, string phoneNumber, string address, int doctor)
        {
            Id = id;
            IdentificationCode = identificationCode;
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            Address = address;
            Doctor = doctor;
        }

        public Pacient(string identificationCode, string name, string surname, string birthDate, string phoneNumber, string address, int doctor)
        {
            IdentificationCode = identificationCode;
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            Address = address;
            Doctor = doctor;
        }

        public Pacient()
        {

        }

    }

    public class PacientForUpdate
    {
        public string? IdentificationCode { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int? Doctor { get; set; }

        public PacientForUpdate(string identificationCode, string name, string surname, string birthDate, string phoneNumber, string address, int doctor)
        {
            IdentificationCode = identificationCode;
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            Address = address;
            Doctor = doctor;
        }

        public PacientForUpdate()
        {

        }

    }
}
