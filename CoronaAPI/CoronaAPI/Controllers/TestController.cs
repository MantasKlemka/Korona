using CoronaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace CoronaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private MySqlConnection _connection;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
            _connection = new MySqlConnection("server=localhost;userid=root;database=corona_base");
        }

        [Route("All")]
        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public ActionResult<List<Test>> GetAllTests()
        {
            List<Test> tests = new List<Test>();
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = "SELECT * FROM `tests`";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                DateTime date = DateTime.Parse(sqlResult.GetString(1));
                                Test test = new Test(sqlResult.GetInt32(0), date.ToShortDateString(), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetInt32(4));
                                tests.Add(test);
                            }
                            sqlResult.Close();
                        }
                        return tests;
                    }
                }
            }
            catch (MySqlException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [Route("")]
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public ActionResult<string> CreateTest(Test test)
        {
            try
            {
                using (_connection)
                {
                    if (!DateTime.TryParse(test.Date, out DateTime date))
                    {
                        return StatusCode(400, "Invalid Date");
                    }
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = $"INSERT INTO `tests`(`date`, `type`, `result`, `isolation`) " +
                            $"VALUES ('{test.Date}','{test.Type}','{test.Result}','{test.Isolation}')";

                        cmd.ExecuteReader();
                        return $"Test created for Isolation ({test.Isolation})";
                    }
                }
            }
            catch(MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1452:
                        return StatusCode(404, $"Isolation ({test.Isolation}) does not exist");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }

        [Route("{id}")]
        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public ActionResult<Test> GetTest(int id)
        {
            Test test = new Test();
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = $"SELECT * FROM `tests` WHERE id = '{id}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        int rowsAffected = 0;
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                DateTime date = DateTime.Parse(sqlResult.GetString(1));
                                test = new Test(sqlResult.GetInt32(0), date.ToShortDateString(), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetInt32(4));
                                rowsAffected++;
                            }
                            sqlResult.Close();
                        }
                        return rowsAffected > 0 ? test : StatusCode(404, $"Test ({id}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [Route("{id}")]
        [HttpPut]
        [Authorize(Roles = "Doctor")]
        public ActionResult<string> UpdateTest(int id, TestForUpdate test)
        {
            try
            {
                using (_connection)
                {
                    if (test.Date != null && !DateTime.TryParse(test.Date, out DateTime date))
                    {
                        return StatusCode(400, "Invalid Date");
                    }
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        List<string> values = new List<string>
                        {
                            $"{(test.Date != null ? $"`date`='{test.Date}'" : "")}",
                            $"{(test.Type != null ? $"`type`='{test.Type}'" : "")}",
                            $"{(test.Result != null ? $"`result`='{test.Result}'" : "")}",
                            $"{(test.Isolation != null ? $"`isolation`='{test.Isolation}'" : "")}"
                        };
                        if (!values.Any(x => !string.IsNullOrEmpty(x)))
                        {
                            return StatusCode(304);
                        }
                        cmd.CommandText = $"UPDATE `tests` SET {string.Join("," , values.Where(x => !string.IsNullOrEmpty(x)))} WHERE id = '{id}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Test ({id}) updated" : StatusCode(404, $"Test ({id}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1452:
                        return StatusCode(404, $"Isolation ({test.Isolation}) does not exist");
                    default:
                        return StatusCode(400, ex.Message);

                }
            }
        }

        [Route("{id}")]
        [HttpDelete]
        [Authorize(Roles = "Doctor")]
        public ActionResult<string> DeleteTest(int id)
        {
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = $"DELETE FROM `tests` WHERE id = '{id}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Test ({id}) deleted" : StatusCode(404, $"Test ({id}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}