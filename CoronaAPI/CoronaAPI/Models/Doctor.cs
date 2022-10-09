namespace CoronaAPI.Models
{
    public class Doctor
    {
        public string PersonalCode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public bool Activated { get; set; }

        public Doctor(string personalCode, string name, string surname, string password, bool activated)
        {
            PersonalCode = personalCode;
            Name = name;
            Surname = surname;
            Password = password;
            Activated = activated;
        }

        public Doctor(string personalCode, string name, string surname, string password)
        {
            PersonalCode = personalCode;
            Name = name;
            Surname = surname;
            Password = password;
        }

        public Doctor()
        {

        }
    }
}
