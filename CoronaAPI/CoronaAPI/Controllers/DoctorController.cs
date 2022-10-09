using CoronaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace CoronaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly ILogger<DoctorController> _logger;
        private MySqlConnection _connection;

        public DoctorController(ILogger<DoctorController> logger)
        {
            _logger = logger;
            _connection = new MySqlConnection("server=localhost;userid=root;database=corona_base");
        }

        [Route("All")]
        [HttpGet]
        public ActionResult<List<Doctor>> GetAllDoctors()
        {
            List<Doctor> doctors = new List<Doctor>();
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = "SELECT * FROM `doctors`";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                Doctor doctor = new Doctor(sqlResult.GetString(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetBoolean(4));
                                doctors.Add(doctor);
                            }
                            sqlResult.Close();
                        }
                        return doctors;
                    }
                }
            }
            catch (MySqlException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [Route("Activate/{personalCode}")]
        [HttpPut]
        public ActionResult<string> ActivateDoctor(string personalCode)
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
                        cmd.CommandText = $"SELECT * FROM `doctors` WHERE personal_code = '{personalCode}'";

                        Doctor doctor = new Doctor();
                        int rowsAffected = 0;
                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                doctor = new Doctor(sqlResult.GetString(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetBoolean(4));
                                rowsAffected++;
                            }
                            sqlResult.Close();
                        }
                        if (rowsAffected < 1)
                        {
                            return StatusCode(400, $"Doctor with Personal Code ({personalCode}) does not exist");
                        }
                        else if (doctor.Activated) return StatusCode(400, $"Doctor with Personal Code ({personalCode}) is already activated");


                        cmd.CommandText = $"UPDATE `doctors` SET `activated`='1' WHERE personal_code = '{personalCode}'";

                        sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Doctor ({personalCode}) activated" : StatusCode(400, $"Doctor with Personal Code ({personalCode}) does not exist");
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
        public ActionResult<string> CreateDoctor(Doctor doctor)
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
                        cmd.CommandText = $"INSERT INTO `doctors`(`personal_code`, `name`, `surname`, `password`, `activated`) VALUES ('{doctor.PersonalCode}','{doctor.Name}','{doctor.Surname}','{doctor.Password}','0')";

                        cmd.ExecuteReader();
                        return $"Doctor {doctor.Name} {doctor.Surname} ({doctor.PersonalCode}) created";
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        return StatusCode(400, $"Doctor with Personal Code ({doctor.PersonalCode}) already exists");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }

        [Route("{personalCode}")]
        [HttpDelete]
        public ActionResult<string> DeleteDoctor(string personalCode)
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
                        cmd.CommandText = $"DELETE FROM `doctors` WHERE personal_code = '{personalCode}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Doctor ({personalCode}) deleted" : StatusCode(400, $"Doctor with Personal Code ({personalCode}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1451:
                        return StatusCode(400, $"Doctor has Pacients so it can not be deleted");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }
    }
}