using System.Data.SqlClient;

namespace HikeAware
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hike Aware App 1.0");

            Console.WriteLine("Choose a hike");

            string hike = Console.ReadLine();

            string queryString = "SELECT TOP (10) * FROM [dbo].[Trails]";
            string connectionString = "";

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
            }


        }
    }
}
