
namespace Utility
{
    public class UtilitMigrate
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("   Console Application For Data Migration Utility that reads data from the Source table,");
            Console.WriteLine("   manipulates records (ADDITION), and stores it in the Destination table.");
            Console.WriteLine("---------------------------------------------------------------------------------------------------");
            string key;

            do
            {
                DataMigration.StartDataMigration();
               
                Console.WriteLine("USER WANT TO CONTINUE DATA MIGRATION? YES/NO");
                key = Console.ReadLine()!;
            } while (key == "YES");
        }

    }
}