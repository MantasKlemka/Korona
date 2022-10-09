using CoronaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
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

        [Route("All/{personalCode}")]
        [HttpGet]
        public ActionResult<List<Isolation>> GetAllPacientIsolations(string personalCode)
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
                        cmd.CommandText = $"SELECT * FROM `pacients` WHERE personal_code = '{personalCode}'";

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
                        if(rowsAffected < 1)
                        {
                            return StatusCode(400, $"Pacient with Personal Code ({personalCode}) does not exist");
                        }
                        
                        cmd.CommandText = $"SELECT * FROM `isolations` WHERE `pacient` = '{personalCode}'";

                        sqlResult = cmd.ExecuteReader();
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                Isolation isolation = new Isolation(sqlResult.GetString(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetString(4));
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

        [Route("All")]
        [HttpGet]
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
                                Isolation isolation = new Isolation(sqlResult.GetString(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetString(4));
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
        public ActionResult<string> CreateIsolation(Isolation isolation)
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
                        cmd.CommandText = $"INSERT INTO `isolations`(`id`, `cause`, `start_date`, `amount_of_days`, `pacient`) " +
                            $"VALUES ('{isolation.IsolationID}','{isolation.Cause}','{isolation.StartDate}','{isolation.AmountOfDays}','{isolation.Pacient}')";

                        cmd.ExecuteReader();
                        return $"Isolation ({isolation.IsolationID}) created for Pacient ({isolation.Pacient})";
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1452:
                        return StatusCode(400, $"Pacient with Personal Code ({isolation.Pacient}) does not exist");
                    case 1062:
                        return StatusCode(400, $"Isolation with Isolation ID ({isolation.IsolationID}) already exists");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }

        [Route("{isolationID}")]
        [HttpGet]
        public ActionResult<Isolation> GetIsolation(string isolationID)
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
                        cmd.CommandText = $"SELECT * FROM `isolations` WHERE id = '{isolationID}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        int rowsAffected = 0;
                        if (sqlResult != null)
                        {
                            while (sqlResult.Read())
                            {
                                isolation = new Isolation(sqlResult.GetString(0), sqlResult.GetString(1), sqlResult.GetString(2), sqlResult.GetString(3), sqlResult.GetString(4));
                                rowsAffected++;
                            }
                            sqlResult.Close();
                        }
                        return rowsAffected > 0 ? isolation : StatusCode(400, $"Isolation with Isolation ID ({isolationID}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [Route("{isolationID}")]
        [HttpPut]
        public ActionResult<string> UpdateIsolation(string isolationID, Isolation isolation)
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
                        cmd.CommandText = $"UPDATE `isolations` SET `id`='{isolation.IsolationID}',`cause`='{isolation.Cause}',`start_date`='{isolation.StartDate}'," +
                            $"`amount_of_days`='{isolation.AmountOfDays}',`pacient`='{isolation.Pacient}' WHERE id = '{isolationID}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Isolation ({isolationID}) updated" : StatusCode(400, $"Isolation with Isolation ID ({isolationID}) does not exist");
                    }
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1452:
                        return StatusCode(400, $"Pacient with Personal Code ({isolation.Pacient}) does not exist");
                    case 1062:
                        return StatusCode(400, $"Isolation with Isolation ID ({isolation.IsolationID}) already exists");
                    default:
                        return StatusCode(400, ex.Message);
                }
            }
        }

        [Route("{isolationID}")]
        [HttpDelete]
        public ActionResult<string> DeleteIsolation(string isolationID)
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
                        cmd.CommandText = $"DELETE FROM `isolations` WHERE id = '{isolationID}'";

                        MySqlDataReader sqlResult = cmd.ExecuteReader();
                        return sqlResult.RecordsAffected > 0 ? $"Isolation ({isolationID}) deleted" : StatusCode(400, $"Isolation with Isolation ID ({isolationID}) does not exist");
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
    }
}