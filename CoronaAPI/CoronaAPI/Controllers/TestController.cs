using CoronaAPI.Models;
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

        [Route("All/{isolationID}")]
        [HttpGet]
        public ActionResult<List<Test>> GetAllTests(string isolationID)
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
                        cmd.CommandText = $"SELECT * FROM `isolations` WHERE id = '{isolationID}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        int rowsAffected = 0;
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                rowsAffected++;
                            }
                            sqlResult.Close();
                        }
                        if (rowsAffected < 1)
                        {
                            return StatusCode(400, $"Isolation with Isolation ID ({isolationID}) does not exist");
                        }

                        cmd.CommandText = $"SELECT * FROM `tests` WHERE isolation = '{isolationID}'";

                        sqlResult = cmd.ExecuteReader();
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                Test test = new Test(sqlResult.GetString(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetString(4));
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

        [Route("All")]
        [HttpGet]
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
                                Test test = new Test(sqlResult.GetString(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetString(4));
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
        public ActionResult<string> CreateTest(Test test)
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
                        cmd.CommandText = $"INSERT INTO `tests`(`id`, `date`, `type`, `result`, `isolation`) " +
                            $"VALUES ('{test.TestID}','{test.Date}','{test.Type}','{test.Result}','{test.Isolation}')";

                        cmd.ExecuteReader();
                        return $"Test ({test.TestID}) created for Isolation ({test.Isolation})";
                    }
                }
            }
            catch(MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1452:
                        return StatusCode(400, $"Isolation with Isolation ID ({test.Isolation}) does not exist");
                    case 1062:
                        return StatusCode(400, $"Test with Test ID ({test.TestID}) already exists");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }

        [Route("{testID}")]
        [HttpGet]
        public ActionResult<Test> GetTest(string testID)
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
                        cmd.CommandText = $"SELECT * FROM `tests` WHERE id = '{testID}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        int rowsAffected = 0;
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                test = new Test(sqlResult.GetString(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetString(4));
                                rowsAffected++;
                            }
                            sqlResult.Close();
                        }
                        return rowsAffected > 0 ? test : StatusCode(400, $"Test with Test ID ({testID}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [Route("{testID}")]
        [HttpPut]
        public ActionResult<string> UpdateTest(string testID, Test test)
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
                        cmd.CommandText = $"UPDATE `tests` SET `id`='{test.TestID}',`date`='{test.Date}',`type`='{test.Type}'," +
                            $"`result`='{test.Result}',`isolation`='{test.Isolation}' WHERE id = '{testID}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Test ({testID}) updated" : StatusCode(400, $"Test with Test ID ({testID}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1452:
                        return StatusCode(400, $"Isolation with Isolation ID ({test.Isolation}) does not exist");
                    case 1062:
                        return StatusCode(400, $"Test with Test ID ({test.TestID}) already exists");
                    default:
                        return StatusCode(400, ex.Message);

                }
            }
        }

        [Route("{testID}")]
        [HttpDelete]
        public ActionResult<string> DeleteTest(string testID)
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
                        cmd.CommandText = $"DELETE FROM `tests` WHERE id = '{testID}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Test ({testID}) deleted" : StatusCode(400, $"Test with Test ID ({testID}) does not exist");
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