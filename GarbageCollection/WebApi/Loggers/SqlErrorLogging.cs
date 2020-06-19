using DataAccess.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GarbageCollection.WebApi.Loggers
{
    public class SqlErrorLogging
    {
        public void InsertErrorLog(API_Error apiError)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
                {
                    sqlConnection.Open();
                    var cmd =
                        new SqlCommand("API_ErrorLogging", connection: sqlConnection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                    cmd.Parameters.AddWithValue("@TimeUtc", apiError.TimeUtc);
                    cmd.Parameters.AddWithValue("@RequestUri", apiError.RequestUri);
                    cmd.Parameters.AddWithValue("@Message", apiError.Message);
                    cmd.Parameters.AddWithValue("@RequestMethod", apiError.RequestMethod);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}