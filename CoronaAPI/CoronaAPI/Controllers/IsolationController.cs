using CoronaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;

namespace CoronaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IsolationController : ControllerBase
    {
        private readonly ILogger<IsolationController> _logger;
        private MySqlConnection _connection;

        public IsolationController(ILogger<IsolationController> logger)
        {
            _logger = logger;
            _connection = new MySqlConnection("server=localhost;userid=root;database=corona_base");
        }

        [Route("All")]
        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public ActionResult<List<Isolation>> GetAllIsolations()
        {
            List<Isolation> isolations = new List<Isolation>();
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = "SELECT * FROM `isolations`";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                DateTime date = DateTime.Parse(sqlResult.GetString(2));
                                Isolation isolation = new Isolation(sqlResult.GetInt32(0), sqlResult.GetString(1), date.ToShortDateString(), sqlResult.GetInt32(3), sqlResult.GetInt32(4), sqlResult.GetString(5));
                                isolations.Add(isolation);
                            }
                            sqlResult.Close();
                        }
                        return isolations;
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
        public ActionResult<string> CreateIsolation(Isolation isolation)
        {
            try
            {
                using (_connection)
                {
                    if (!DateTime.TryParse(isolation.StartDate, out DateTime date))
                    {
                        return StatusCode(400, "Invalid Start Date");
                    }
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = $"SELECT * FROM `pacients` WHERE id = '{isolation.Pacient}'";

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
                            return StatusCode(404, $"Pacient ({isolation.Pacient}) does not exist");
                        }

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = $"SELECT * FROM `isolations` WHERE pacient = '{isolation.Pacient}'";

                        sqlResult = cmd.ExecuteReader();
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                int amountOfDays = sqlResult.GetInt32(3);
                                DateTime leftdate = DateTime.Parse(sqlResult.GetString(2));
                                //DateTime rightDate = leftdate.AddDays(amountOfDays);
                                //DateTime rightOption = date.AddDays((double)isolation.AmountOfDays);
                                //if (date <= rightDate && leftdate <= rightOption)
                                //{
                                //    return StatusCode(400, $"Pacient ({isolation.Pacient}) does already has isolation in that period");
                                //}
                            }
                            sqlResult.Close();
                        }

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = $"INSERT INTO `isolations`(`cause`, `start_date`, `amount_of_days`, `pacient`, `code`) " +
                            $"VALUES ('{isolation.Cause}','{isolation.StartDate}','{isolation.AmountOfDays}','{isolation.Pacient}','{Math.Abs(DateTime.Now.Ticks.GetHashCode())}')";

                        cmd.ExecuteReader();
                        return $"Isolation created for Pacient ({isolation.Pacient})";
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1452:
                        return StatusCode(404, $"Pacient ({isolation.Pacient}) does not exist");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }

        [Route("{id}")]
        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public ActionResult<Isolation> GetIsolation(int id)
        {
            Isolation isolation = new Isolation();
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = $"SELECT * FROM `isolations` WHERE id = '{id}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        int rowsAffected = 0;
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                DateTime date = DateTime.Parse(sqlResult.GetString(2));
                                isolation = new Isolation(sqlResult.GetInt32(0), sqlResult.GetString(1), date.ToShortDateString(), sqlResult.GetInt32(3), sqlResult.GetInt32(4), sqlResult.GetString(5));
                                rowsAffected++;
                            }
                            sqlResult.Close();
                        }
                        return rowsAffected > 0 ? isolation : StatusCode(404, $"Isolation ({id}) does not exist");
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
        public ActionResult<string> UpdateIsolation(int id, IsolationForUpdate isolation)
        {
            try
            {
                using (_connection)
                {
                    if (isolation.StartDate != null && !DateTime.TryParse(isolation.StartDate, out DateTime date))
                    {
                        return StatusCode(400, "Invalid Start Date");
                    }
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        List<string> values = new List<string>
                        {
                            $"{(isolation.Cause != null ? $"`cause`='{isolation.Cause}'" : "")}",
                            $"{(isolation.StartDate != null ? $"`start_date`='{isolation.StartDate}'" : "")}",
                            $"{(isolation.AmountOfDays != null ? $"`amount_of_days`='{isolation.AmountOfDays}'" : "")}",
                            $"{(isolation.Pacient != null ? $"`pacient`='{isolation.Pacient}'" : "")}"
                        };

                        if (!values.Any(x => !string.IsNullOrEmpty(x)))
                        {
                            return StatusCode(304);
                        }
                        cmd.CommandText = $"UPDATE `isolations` SET {string.Join(",", values.Where(x => !string.IsNullOrEmpty(x)))} WHERE id = '{id}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Isolation ({id}) updated" : StatusCode(404, $"Isolation ({id}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1452:
                        return StatusCode(404, $"Pacient ({isolation.Pacient}) does not exist");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }

        [Route("{id}")]
        [HttpDelete]
        [Authorize(Roles = "Doctor")]
        public ActionResult<string> DeleteIsolation(int id)
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
                        cmd.CommandText = $"DELETE FROM `isolations` WHERE id = '{id}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Isolation ({id}) deleted" : StatusCode(404, $"Isolation ({id}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1451:
                        return StatusCode(400, $"Isolation has Tests so it can not be deleted");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }
        [Route("{id}/Tests")]
        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public ActionResult<List<Test>> GetAllTests(int id)
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
                        cmd.CommandText = $"SELECT * FROM `isolations` WHERE id = '{id}'";

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
                            return StatusCode(404, $"Isolation ({id}) does not exist");
                        }

                        cmd.CommandText = $"SELECT * FROM `tests` WHERE isolation = '{id}'";

                        sqlResult = cmd.ExecuteReader();
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

        [Route("Check/{code}")]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<string> GetIsolationByCode(string code)
        {
            string response = string.Empty;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = $"SELECT `cause`, `start_date`, `amount_of_days` FROM `isolations` WHERE code = '{code}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        int rowsAffected = 0;
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                DateTime date = DateTime.Parse(sqlResult.GetString(1));
                                var data = new
                                {
                                    Cause = sqlResult.GetString(0),
                                    StartDate = date.ToShortDateString(),
                                    AmountOfDays = sqlResult.GetInt32(2)
                                };
                                
                                response = JsonConvert.SerializeObject(data,Formatting.Indented);
                                rowsAffected++;
                            }
                            sqlResult.Close();
                        }
                        return rowsAffected > 0 ? response : StatusCode(404, $"Isolation ({code}) does not exist");
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