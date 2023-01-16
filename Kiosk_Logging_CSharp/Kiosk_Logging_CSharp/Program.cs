
namespace Kiosk_Logging_CSharp 
{
class KioskLogging
    {
        // function to get the abbreviated month name
        static string getAbbreviatedName(int month)
        {
            DateTime date = new DateTime(2020, month, 1);

            return date.ToString("MMM");
        }
        static void Main(string[] args)
        {
            
            string year = DateTime.Now.Year.ToString();
            string month = getAbbreviatedName(DateTime.Now.Month);
            string day = DateTime.Now.Day.ToString();
            
            string fileName = month + "-" + day + "-" + year + "-" + "Transactions";
            string path = $@"C:\Users\natha\source\repos\Kiosk_console_CSharp\Kiosk_Logging_CSharp\Kiosk_Logging_CSharp\LOG\{fileName}";

            try { 

                StreamWriter outfile = new StreamWriter(path, true);

                foreach (string item in args)
                {
                    outfile.Write(item + " ");
                }

                outfile.Write('\n');
                
                outfile.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
    


