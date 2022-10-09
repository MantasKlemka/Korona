namespace CoronaAPI.Models
{
    public class Isolation
    {
        public string IsolationID { get; set; }
        public string Cause { get; set; }
        public string StartDate { get; set; }
        public string AmountOfDays { get; set; }
        public string Pacient { get; set; }

        public Isolation(string isolationID, string cause, string startDate, string amountOfDays, string pacient)
        {
            IsolationID = isolationID;
            Cause = cause;
            StartDate = startDate;
            AmountOfDays = amountOfDays;
            Pacient = pacient;
        }

        public Isolation()
        {

        }
    }
}
