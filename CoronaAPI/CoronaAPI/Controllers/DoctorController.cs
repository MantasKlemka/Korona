using CoronaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace CoronaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private MySqlConnection _connection;
        private IConfiguration _config;
        public DoctorController(IConfiguration config)
        {
            _config = config;
            _connection = new MySqlConnection(_config.GetConnectionString("myDB"));
        }

        [Route("All")]
        [HttpGet]
        [Authorize(Roles = "Administrator, Doctor")]
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
                                Doctor doctor = new Doctor(sqlResult.GetInt32(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetString(4), sqlResult.GetBoolean(5));
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

        [Route("Activate/{id}")]
        [HttpPut]
        [Authorize(Roles = "Administrator")]
        public ActionResult<string> ActivateDoctor(int id)
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
                        cmd.CommandText = $"SELECT * FROM `doctors` WHERE id = '{id}'";

                        Doctor doctor = new Doctor();
                        int rowsAffected = 0;
                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                doctor = new Doctor(sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetString(4), sqlResult.GetBoolean(5));
                                rowsAffected++;
                            }
                            sqlResult.Close();
                        }
                        if (rowsAffected < 1)
                        {
                            return StatusCode(404, $"Doctor ({id}) does not exist");
                        }
                        else if (doctor.Activated) return StatusCode(400, $"Doctor ({id}) is already activated");


                        cmd.CommandText = $"UPDATE `doctors` SET `activated`='1' WHERE id = '{id}'";

                        sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Doctor ({id}) activated" : StatusCode(404, $"Doctor ({id}) does not exist");
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
        [AllowAnonymous]
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
                        cmd.CommandText = $"INSERT INTO `doctors`(`email`, `name`, `surname`, `password`, `activated`) VALUES ('{doctor.Email}','{doctor.Name}','{doctor.Surname}','{doctor.Password}','0')";

                        cmd.ExecuteReader();
                        return $"Doctor {doctor.Name} {doctor.Surname} ({doctor.Email}) created";
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        return StatusCode(400, $"Doctor with that Email ({doctor.Email}) already exists");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }

        [Route("{id}")]
        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        public ActionResult<string> DeleteDoctor(int id)
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
                        cmd.CommandText = $"DELETE FROM `doctors` WHERE id = '{id}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Doctor ({id}) deleted" : StatusCode(400, $"Doctor ({id}) does not exist");
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
        [Route("{id}/Pacients")]
        [HttpGet]
        [Authorize(Roles = "Administrator, Doctor")]
        public ActionResult<List<Pacient>> GetAllDoctorPacients(int id)
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
                        cmd.CommandText = $"SELECT * FROM `pacients` WHERE doctor = '{id}'";

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
    }
}