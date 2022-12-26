
namespace Utility
{
    public class DataMigration
    {
        public static bool completed;
        public static int iterator,range;
        public static CancellationTokenSource? tokenSource;
        public static CancellationToken cancellationToken;
       
        public static void StartDataMigration()
        {
            UserData.FetchUserInputData();

            tokenSource = new CancellationTokenSource();
            cancellationToken = tokenSource.Token;
            completed = false;
            iterator = UserData.startindex - 1;
           
            Migration();
        
            while (true)
            {
                if (Console.ReadLine()!.ToUpper() == "CANCEL")
                {
                    StatusSummary("CANCEL");
                    tokenSource.Cancel();
                    break;
                }
                else if(Console.ReadLine()!.ToUpper() == "STATUS")
                {
                    StatusSummary("STATUS");
                }
                else if (completed)
                {
                    StatusSummary("completed");
                    break;
                }
            }
        }
        public static async Task Migration()
        {
            Console.WriteLine(" ************   Migration started  *******************");
            Console.WriteLine("------------------------------------------------------");

            range = UserData.endindex - UserData.startindex + 1;
            var datasource = await DataFetch.getDataFromSource();

            while (range > 0)
            {
                 await BatchOperation.ExecuteBatch(datasource);  
            }
            Console.WriteLine($"Migration completed!! Press Enter to show Details");
            completed = true;
        }
        public static void StatusSummary(string operation)
        {
            Console.WriteLine(".....  STATUS OF DATA SUMMATION  ........");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($" DATA SUM MIGRATED SUCCESSFULLY :: {iterator - UserData.startindex + 1} ");
            if (operation == "STATUS" || operation == "completed")
                Console.WriteLine($" Ongoing DATA SUM MIGRATION  :: {UserData.endindex - iterator} ");
            else
                Console.WriteLine($" CANCELED DATA SUM MIGRATION  :: {UserData.endindex - iterator} ");
        }
    }
}
