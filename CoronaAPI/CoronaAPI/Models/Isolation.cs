using System.ComponentModel.DataAnnotations;

namespace CoronaAPI.Models
{
    public class Isolation
    {
        public int? Id { get; set; }
        [Required]
        public string? Cause { get; set; }
        [Required]
        public string? StartDate { get; set; }
        [Required]
        public int? AmountOfDays { get; set; }
        [Required]
        public int? Pacient { get; set; }
        public string? Code { get; set; }

        public Isolation(int id, string cause, string startDate, int amountOfDays, int pacient, string code)
        {
            Id = id;
            Cause = cause;
            StartDate = startDate;
            AmountOfDays = amountOfDays;
            Pacient = pacient;
            Code = code;
        }

        public Isolation(string cause, string startDate, int amountOfDays, int pacient)
        {
            Cause = cause;
            StartDate = startDate;
            AmountOfDays = amountOfDays;
            Pacient = pacient;
        }

        public Isolation()
        {

        }
    }

    public class IsolationForUpdate
    {
        public string? Cause { get; set; }
        public string? StartDate { get; set; }
        public int? AmountOfDays { get; set; }
        public int? Pacient { get; set; }

        public IsolationForUpdate(string cause, string startDate, int amountOfDays, int pacient)
        {
            Cause = cause;
            StartDate = startDate;
            AmountOfDays = amountOfDays;
            Pacient = pacient;
        }

        public IsolationForUpdate()
        {

        }
    }
}
