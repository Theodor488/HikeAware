using System.Data;
using System.Data.SqlClient;

namespace HikeAware
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hike Aware App 1.0");

            Console.WriteLine("Choose a hike");

            string hikeName = Console.ReadLine();

            string connectionString = "";
            string queryString = "SELECT TOP (10) * FROM [dbo].[Trails]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", hikeName);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(String.Format("{0}, {1}",
                        reader["tPatCulIntPatIDPk"], reader["tPatSFirstname"]));
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }

                connection.Close();
            }

            /*
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(String.Format("{0}, {1}",
                        reader["tPatCulIntPatIDPk"], reader["tPatSFirstname"]));// etc
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }*/


        }
    }
}
