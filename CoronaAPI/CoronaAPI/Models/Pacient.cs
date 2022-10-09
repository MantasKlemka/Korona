namespace CoronaAPI.Models
{
    public class Pacient
    {
        public string PersonalCode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Doctor { get; set; }

        public Pacient(string personalCode, string name, string surname, string birthDate, string phoneNumber, string address, string doctor)
        {
            PersonalCode = personalCode;
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
}
