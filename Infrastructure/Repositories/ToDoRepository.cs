using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using ToDo_App.Models;

namespace ToDo_App.Infrastructure.Repositories
{
    public class ToDoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ClaimsPrincipal _claimPrincipal;
        public ToDoRepository(IConfiguration configuration, ClaimsPrincipal claimPrincipal)
        {
            _configuration = configuration;
            _claimPrincipal = claimPrincipal;
        }

        public IEnumerable<ToDoModel> GetToDos()
        {
            try
            {
                // get connection string from configuration file
                string connectionString = _configuration["ConnectionStrings:DefaultConnection"];

                // get connection timeout from the configuration file
                int connectionTimeOut = int.Parse(_configuration["ConnectionStrings:ConnectionTimeout"]);

                // open the sql connection using connection string
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // set sql command parameters
                    SqlCommand sqlCommand = new SqlCommand("SELECT * FROM ToDo", connection)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = connectionTimeOut
                    };

                    using (SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        // Currency symbols are present
                        if (reader != null && reader.HasRows)
                        {
                            // init dictionary variable 
                            List<ToDoModel> toDos = new List<ToDoModel>();

                            // get ordinals
                            int idOrdinal = reader.GetOrdinal("Id");
                            int titleOrdinal = reader.GetOrdinal("Title");
                            int descriptionOrdinal = reader.GetOrdinal("Title");

                            // fill the property data in property list
                            while (reader.Read())
                            {
                                toDos.Add(new ToDoModel()
                                {
                                    Id = reader.GetInt32(idOrdinal),
                                    Title = reader.GetString(titleOrdinal),
                                    Description = reader.GetString(descriptionOrdinal)
                                });
                            }

                            reader.Close();
                            connection.Close();

                            // return currency symbol list
                            return toDos;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            // if nothing found return null
            return null;
        }

        public bool Insert(ToDoModel toDo)
        {
            try
            {
                // get connection string from configuration file
                string connectionString = _configuration["ConnectionStrings:DefaultConnection"];

                // get connection timeout from the configuration file
                int connectionTimeOut = int.Parse(_configuration["ConnectionStrings:ConnectionTimeout"]);

                // open the sql connection using connection string
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // set sql command parameters
                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO ToDo(Title, Description) VALUES(@title, @description)", connection)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = connectionTimeOut,
                    };

                    sqlCommand.Parameters.Add(new SqlParameter("@title", toDo.Title));
                    sqlCommand.Parameters.Add(new SqlParameter("@description", toDo.Description));

                    int result = sqlCommand.ExecuteNonQuery();

                    connection.Close();

                    return result > 0;
                }
            }
            catch (Exception)
            {

            }

            return false;
        }
    }
}
