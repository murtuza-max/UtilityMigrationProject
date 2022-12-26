
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Utility
{
    public class DataFetch
    {
        public static bool completed = false;
        private const string CONNECTION_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Utility;Integrated Security=True";
      
        public static DataTable? SumDataTable;

        public static async Task<Dictionary<int, (int, int)>> getDataFromSource()
        {
            
            Dictionary<int, (int, int)> sourceData = new Dictionary<int, (int, int)>();

            String query = $"SELECT * FROM SourceTable order by ID OFFSET {UserData.startindex-1} rows fetch first {UserData.endindex} rows only";
     
           await Task.Run(async () =>
            {
                await using SqlConnection getData = new SqlConnection(CONNECTION_STRING);
                await using SqlCommand command = new SqlCommand(query, getData);
                DataMigration.cancellationToken.Register(command.Cancel);
                await getData.OpenAsync(DataMigration.cancellationToken);
                await using SqlDataReader reader = await command.ExecuteReaderAsync(DataMigration.cancellationToken);

                while (await reader.ReadAsync())
                {
                    
                    int id = Convert.ToInt32(reader[0]);
                    int n1 = Convert.ToInt32(reader[1]);
                    int n2 = Convert.ToInt32(reader[2]);
                    sourceData[id] = (n1, n2);
                    Console.WriteLine($"ID : {id} Data ");
                }
          
            }, DataMigration.cancellationToken);
            return sourceData;
        }
    }
}