
namespace Kiosk_Logging_CSharp;
class KioskLogging
{
    // PROGRAM CREATES TRANSACTIONS LOG
    static void Main(string[] args)
    {

        string year = DateTime.Now.Year.ToString();
        string month = getAbbreviatedName(DateTime.Now.Month);
        string day = DateTime.Now.Day.ToString();

        string fileName = month + "-" + day + "-" + year + "-" + "Transactions";
        string path = $@"C:\Users\POBOYINSAMSARA\source\repos\Kiosk_console_CSharp\Kiosk_Logging_CSharp\Kiosk_Logging_CSharp\LOG\{fileName}";

        try
        {

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
    // FUNCTIONS RETURNS MONTH NAME IN ABBREVIATED FORM, EX. AUG
    static string getAbbreviatedName(int month)
    {
        DateTime date = new DateTime(2023, month, 1);

        return date.ToString("MMM");
    }
}



