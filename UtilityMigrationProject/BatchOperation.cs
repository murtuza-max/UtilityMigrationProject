
using System.Data.SqlClient;
using System.Data;

namespace Utility
{
    public class BatchOperation
    {
        public static DataTable? SumDataTable;
        public static SqlConnection? InsertData;
        public static bool completed = false;
        private const string CONNECTION_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Utility;Integrated Security=True";
        public static async Task ExecuteBatch(Dictionary<int, (int, int)> d1)
        {
            await Task.Run(async () =>
            {
                await using SqlConnection InsertData = new SqlConnection(CONNECTION_STRING);

                Console.WriteLine("**********************  Data Insertion started  ****************************");
                Console.WriteLine("------------------------------------------------------------");
               
                DataMigration.cancellationToken.Register(InsertData.Close); 
                await InsertData.OpenAsync(DataMigration.cancellationToken);

                SumDataTable = new DataTable();
                SumDataTable.Columns.Add(new DataColumn("SourceID", typeof(Int32)));
                SumDataTable.Columns.Add(new DataColumn("Sum", typeof(Int32)));

                // int cnt=0;
                var d2 =  d1.Skip(DataMigration.iterator).Take(100);
                foreach (var row in d2)
                {
                   // cnt++;
                    int id = row.Key;
                    int n1 = row.Value.Item1;
                    int n2 = row.Value.Item2;
                    DataRow dr = SumDataTable.NewRow();
                    dr["SourceID"] = id;
                    dr["Sum"] = n1 + n2;
                    SumDataTable.Rows.Add(dr);
                    //if(cnt == 100)
                    //{
                    //    break;
                    //}
                }
                Console.WriteLine($"Datatable rows count{SumDataTable.Rows.Count}");
            
                SqlBulkCopy objbulk = new SqlBulkCopy(InsertData);
               
                objbulk.DestinationTableName = "DestinatTable";
                objbulk.ColumnMappings.Add("SourceID", "SourceID");
                objbulk.ColumnMappings.Add("Sum", "Sum");

                try
                {
                    await objbulk.WriteToServerAsync(SumDataTable);
                    DataMigration.iterator += SumDataTable.Rows.Count;
                    DataMigration.range -= SumDataTable.Rows.Count;
                    Console.WriteLine($"Table Row Count : {SumDataTable.Rows.Count}");
                    Console.WriteLine("----------------------------------------------------------------------------");
                    Console.WriteLine($"    {DataMigration.iterator} data batch is inserted into DestinationTable");
                    Console.WriteLine("----------------------------------------------------------------------------");
                    Thread.Sleep(5000);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                }
                finally
                {
                    InsertData.Close();
                }
            }, DataMigration.cancellationToken);
        }
    }
    
}
