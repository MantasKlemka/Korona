using CoronaAPI.Models;
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

        [Route("All/{personalCode}")]
        [HttpGet]
        public ActionResult<List<Pacient>> GetAllDoctorPacients(string personalCode)
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
                        cmd.CommandText = $"SELECT * FROM `pacients` WHERE doctor = '{personalCode}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                Pacient pacient = new Pacient(sqlResult.GetString(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetString(4), sqlResult.GetString(5), sqlResult.GetString(6));
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

        [Route("All")]
        [HttpGet]
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
                                Pacient pacient = new Pacient(sqlResult.GetString(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetString(4), sqlResult.GetString(5), sqlResult.GetString(6));
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
        public ActionResult<string> CreatePacient(Pacient pacient)
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
                        cmd.CommandText = $"INSERT INTO `pacients`(`personal_code`, `name`, `surname`, `birthdate`, `phone_number`, `address`, `doctor`) " +
                            $"VALUES ('{pacient.PersonalCode}','{pacient.Name}','{pacient.Surname}','{pacient.BirthDate}','{pacient.PhoneNumber}','{pacient.Address}', '{pacient.Doctor}')";

                        cmd.ExecuteReader();
                        return $"Pacient {pacient.Name} {pacient.Surname} ({pacient.PersonalCode}) created";
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1452:
                        return StatusCode(400, $"Doctor with Doctor ID ({pacient.Doctor}) does not exist");
                    case 1062:
                        return StatusCode(400, $"Pacient with Personal Code ({pacient.PersonalCode}) already exists");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }

        [Route("{personalCode}")]
        [HttpGet]
        public ActionResult<Pacient> GetPacient(string personalCode)
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
                        cmd.CommandText = $"SELECT * FROM `pacients` WHERE personal_code = '{personalCode}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        int rowsAffected = 0;
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                pacient = new Pacient(sqlResult.GetString(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetString(4), sqlResult.GetString(5), sqlResult.GetString(6));
                                rowsAffected++;
                            }
                            sqlResult.Close();
                        }
                        return rowsAffected > 0 ? pacient : StatusCode(400, $"Pacient with Personal Code ({personalCode}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [Route("{personalCode}")]
        [HttpPut]
        public ActionResult<string> UpdatePacient(string personalCode, Pacient pacient)
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
                        cmd.CommandText = $"UPDATE `pacients` SET `personal_code`='{pacient.PersonalCode}',`name`='{pacient.Name}',`surname`='{pacient.Surname}'," +
                            $"`birthdate`='{pacient.BirthDate}',`phone_number`='{pacient.PhoneNumber}',`address`='{pacient.Address}',`doctor`='{pacient.Doctor}' WHERE `personal_code` = '{personalCode}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Pacient ({personalCode}) updated" : StatusCode(400, $"Pacient with Personal Code ({personalCode}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1452:
                        return StatusCode(400, $"Doctor with Doctor ID ({pacient.Doctor}) does not exist");
                    case 1062:
                        return StatusCode(400, $"Pacient with Personal Code ({pacient.PersonalCode}) already exists");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }

        [Route("{personalCode}")]
        [HttpDelete]
        public ActionResult<string> DeletePacient(string personalCode)
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
                        cmd.CommandText = $"DELETE FROM `pacients` WHERE personal_code = '{personalCode}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Pacient ({personalCode}) deleted" : StatusCode(400, $"Pacient with Personal Code ({personalCode}) does not exist");
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
    }
}