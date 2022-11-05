using CoronaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoronaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainController : ControllerBase
    {
        private MySqlConnection _connection;
        private IConfiguration _config;

        public MainController(IConfiguration config)
        {
            _config = config;
            _connection = new MySqlConnection("server=localhost;userid=root;database=corona_base");
        }

        private string GenerateToken(string primary, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                role.Equals("Doctor") ? new Claim(JwtRegisteredClaimNames.Email, primary) : new Claim(JwtRegisteredClaimNames.Sub, primary),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Route("Login/Doctor")]
        [HttpPost]
        public ActionResult<string> LoginDoctor(DoctorAccount account)
        {
            bool isAuthorized = false;
            bool isActivated = false;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = $"SELECT * FROM `doctors` WHERE `email` = '{account.Email}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        int rowsAffected = 0;
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                if (sqlResult.GetString(4).Equals(account.Password)) isAuthorized = true;
                                if (isAuthorized && sqlResult.GetInt16(5) == 1) isActivated = true;
                                rowsAffected++;
                            }
                            sqlResult.Close();
                        }
                        if(rowsAffected > 0)
                        {
                            if (isAuthorized)
                            {
                                return isActivated ? GenerateToken(account.Email, "Doctor") : StatusCode(400, $"Account is not activated");
                            }
                            else
                            {
                                return StatusCode(400, $"Incorrect password");
                            }
                        }
                        else
                        {
                            return StatusCode(400, $"Account does not exist");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [Route("Login/Admin")]
        [HttpPost]
        public ActionResult<string> LoginAdmin(AdminAccount account)
        {
            bool isAuthorized = false;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand cmd = _connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.CommandText = $"SELECT * FROM `administrators` WHERE `username` = '{account.Username}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        int rowsAffected = 0;
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                if (sqlResult.GetString(2).Equals(account.Password)) isAuthorized = true;
                                rowsAffected++;
                            }
                            sqlResult.Close();
                        }
                        if (rowsAffected > 0)
                        {
                            return isAuthorized ? GenerateToken(account.Username, "Administrator") : StatusCode(400, $"Incorrect password");
                        }
                        else
                        {
                            return StatusCode(400, $"Account does not exist");
                        }
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