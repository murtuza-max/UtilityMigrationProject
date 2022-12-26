
namespace Utility
{
    public class UserData
    {
        public static int startindex, endindex, batchsize = 100;
        public static void FetchUserInputData()
        {
            bool validrange = true;

            Console.WriteLine("Please Enter ID between 1 to 1 Million");
            do
            {
                Console.WriteLine("User Enter Starting Source ID for Migration:");
                while (!(Int32.TryParse(Console.ReadLine(), out startindex)))
                {
                    Console.WriteLine("Invalid type please enter Integer!!");
                }
                Console.WriteLine("User Enter Ending Source ID for Migration:");
                while (!(Int32.TryParse(Console.ReadLine(), out endindex)))
                {
                    Console.WriteLine("Invalid type please enter Integer!!");
                }

                if (endindex < startindex || (endindex - startindex) > 1000000)
                {
                    Console.WriteLine("Invalid Range !! Please Enter EndIndex greater then StartIndex !! Range greater then 1 Million");
                    validrange = true;
                }
                else
                {
                    validrange = false;
                }
            } while (validrange);
        }
    }
}
