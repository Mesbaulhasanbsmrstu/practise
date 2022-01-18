using Microsoft.Data.SqlClient;
using practise.Helper;
using practise.Model;
using System;
using System.Data;

namespace practise.Store_Procedures
{
    public class SP_Persons
    {
        public static DataTable getAllPersons(int skip,int next)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(Connection.PRACTISE))
                {
                    string sql = "dbo.SelectAllPerson ";
                    using (SqlCommand sqlCmd = new SqlCommand(sql, connection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@skip", skip);
                        sqlCmd.Parameters.AddWithValue("@next", next);
                        connection.Open();
                        using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                        {
                            sqlAdapter.Fill(dt);
                        }
                        connection.Close();
                    }

                }
                return dt;
                    

            }
            catch(Exception e)
            {
                throw e;
            }
            
        }

        public static string addPerson(Persons person)
        {
            try
            {
                using (var connection = new SqlConnection(Connection.PRACTISE))
                {
                    string sql = "dbo.addPerson";
                    using (SqlCommand sqlCmd = new SqlCommand(sql, connection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@LastName", person.LastName);
                        sqlCmd.Parameters.AddWithValue("@FirstName",person.FirstName);
                        sqlCmd.Parameters.AddWithValue("@Adress",person.Address);
                        sqlCmd.Parameters.AddWithValue("@City",person.City);
                        connection.Open();
 
                        connection.Close();
                    }

                }
                return "OK";


            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
