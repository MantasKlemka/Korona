namespace CoronaAPI.Models
{
    public class DoctorAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DoctorAccount(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
    public class AdminAccount
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public AdminAccount(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
