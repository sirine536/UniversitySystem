using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace UniversitySystem
{
    public class DB
    {
        private static string cs =
            ConfigurationManager.ConnectionStrings["UniversityDataBase"].ConnectionString;

        private static SqlConnection GetConnection()
        {
            return new SqlConnection(cs);
        }

        public static DataTable GetData(string query, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    cmd.Dispose();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Database Error: " + ex.Message);
                }
            }

            return dt;
        }

       
        public static int Execute(string query, SqlParameter[] parameters = null)
        {
            int rows = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    rows = cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Database Error: " + ex.Message);
                }
            }

            return rows;
        }

      
        public static object Scalar(string query, SqlParameter[] parameters = null)
        {
            object result = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    result = cmd.ExecuteScalar();

                    cmd.Dispose();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Database Error: " + ex.Message);
                }
            }

            return result;
        }
    }
}