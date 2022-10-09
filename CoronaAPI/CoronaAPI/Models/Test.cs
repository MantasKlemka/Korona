namespace CoronaAPI.Models
{
    public class Test
    {
        public string TestID { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Result { get; set; }
        public string Isolation { get; set; }

        public Test(string testID, string date, string type, string result, string isolation)
        {
            TestID = testID;
            Date = date;
            Type = type;
            Result = result;
            Isolation = isolation;
        }
        public Test()
        {

        }
    }
}
