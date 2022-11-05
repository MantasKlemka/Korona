using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoronaAPI.Models
{
    public class Test
    {
        public int? Id { get; set; }
        [Required]
        public string? Date { get; set; }
        [Required]
        public string? Type { get; set; }
        [Required]
        public string? Result { get; set; }
        [Required]
        public int? Isolation { get; set; }

        public Test(string date, string type, string result, int isolation)
        {
            Date = date;
            Type = type;
            Result = result;
            Isolation = isolation;
        }

        public Test(int id, string date, string type, string result, int isolation)
        {
            Id = id;
            Date = date;
            Type = type;
            Result = result;
            Isolation = isolation;
        }


        public Test()
        {

        }
    }

    public class TestForUpdate
    {
        public string? Date { get; set; }
        public string? Type { get; set; }
        public string? Result { get; set; }
        public int? Isolation { get; set; }
        public TestForUpdate(string date, string type, string result, int isolation)
        {
            Date = date;
            Type = type;
            Result = result;
            Isolation = isolation;
        }

        public TestForUpdate()
        {
        }
    }
}
