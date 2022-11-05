using CoronaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace CoronaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientController : ControllerBase
    {
        private readonly ILogger<PacientController> _logger;
        private MySqlConnection _connection;

        public PacientController(ILogger<PacientController> logger)
        {
            _logger = logger;
            _connection = new MySqlConnection("server=localhost;userid=root;database=corona_base");
        }

        [Route("All")]
        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public ActionResult<List<Pacient>> GetAllPacients()
        {
            List<Pacient> pacients = new List<Pacient>();
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = "SELECT * FROM `pacients`";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                DateTime date = DateTime.Parse(sqlResult.GetString(4));
                                Pacient pacient = new Pacient(sqlResult.GetInt32(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), date.ToShortDateString(), sqlResult.GetString(5), sqlResult.GetString(6), sqlResult.GetInt32(7));
                                pacients.Add(pacient);
                            }
                            sqlResult.Close();
                        }
                        return pacients;
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
        public ActionResult<string> CreatePacient(Pacient pacient)
        {
            try
            {
                using (_connection)
                {
                    if (!DateTime.TryParse(pacient.BirthDate, out DateTime date))
                    {
                        return StatusCode(400, "Invalid Birth Date");
                    }
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = $"INSERT INTO `pacients`(`identification_code`, `name`, `surname`, `birthdate`, `phone_number`, `address`, `doctor`) " +
                            $"VALUES ('{pacient.IdentificationCode}','{pacient.Name}','{pacient.Surname}','{date.ToShortDateString()}','{pacient.PhoneNumber}','{pacient.Address}', '{pacient.Doctor}')";

                        cmd.ExecuteReader();
                        return $"Pacient {pacient.Name} {pacient.Surname} ({pacient.IdentificationCode}) created";
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1452:
                        return StatusCode(404, $"Doctor ({pacient.Doctor}) does not exist");
                    case 1062:
                        return StatusCode(400, $"Pacient with Identifiaction Code ({pacient.IdentificationCode}) already exists");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }

        [Route("{id}")]
        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public ActionResult<Pacient> GetPacient(int id)
        {
            Pacient pacient = new Pacient();
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = $"SELECT * FROM `pacients` WHERE id = '{id}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        int rowsAffected = 0;
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                DateTime date = DateTime.Parse(sqlResult.GetString(4));
                                pacient = new Pacient(sqlResult.GetInt32(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), date.ToShortDateString(), sqlResult.GetString(5), sqlResult.GetString(6), sqlResult.GetInt32(7));
                                rowsAffected++;
                            }
                            sqlResult.Close();
                        }
                        return rowsAffected > 0 ? pacient : StatusCode(404, $"Pacient ({id}) does not exist");
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
        public ActionResult<string> UpdatePacient(int id, PacientForUpdate pacient)
        {
            try
            {
                using (_connection)
                {
                    if (pacient.BirthDate != null && !DateTime.TryParse(pacient.BirthDate, out DateTime date))
                    {
                        return StatusCode(400, "Invalid Birth Date");
                    }
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;

                        List<string> values = new List<string>
                        {
                            $"{(pacient.IdentificationCode != null ? $"`identification_code`='{pacient.IdentificationCode}'" : "")}",
                            $"{(pacient.Name != null ? $"`name`='{pacient.Name}'" : "")}",
                            $"{(pacient.Surname != null ? $"`surname`='{pacient.Surname}'" : "")}",
                            $"{(pacient.BirthDate != null ? $"`birthdate`='{pacient.BirthDate}'" : "")}",
                            $"{(pacient.PhoneNumber != null ? $"`phone_number`='{pacient.PhoneNumber}'" : "")}",
                            $"{(pacient.Address != null ? $"`address`='{pacient.Address}'" : "")}",
                            $"{(pacient.Doctor != null ? $"`doctor`='{pacient.Doctor}'" : "")}"
                        };
                        if(!values.Any(x => !string.IsNullOrEmpty(x)))
                        {
                            return StatusCode(304);
                        }
                        cmd.CommandText = $"UPDATE `pacients` SET {string.Join(",", values.Where(x => !string.IsNullOrEmpty(x)))} WHERE id = '{id}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Pacient ({id}) updated" : StatusCode(404, $"Pacient ({id}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1452:
                        return StatusCode(404, $"Doctor ({pacient.Doctor}) does not exist");
                    case 1062:
                        return StatusCode(400, $"Pacient with Identification Code ({pacient.IdentificationCode}) already exists");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }

        [Route("{id}")]
        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        public ActionResult<string> DeletePacient(int id)
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
                        cmd.CommandText = $"DELETE FROM `pacients` WHERE id = '{id}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Pacient ({id}) deleted" : StatusCode(404, $"Pacient ({id}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1451:
                        return StatusCode(400, $"Pacient has Isolations so it can not be deleted");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }

        [Route("{id}/Isolations")]
        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public ActionResult<List<Isolation>> GetAllPacientIsolations(int id)
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
                        cmd.CommandText = $"SELECT * FROM `pacients` WHERE id = '{id}'";

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
                            return StatusCode(404, $"Pacient ({id}) does not exist");
                        }

                        cmd.CommandText = $"SELECT * FROM `isolations` WHERE `pacient` = '{id}'";

                        sqlResult = cmd.ExecuteReader();
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
    }
}