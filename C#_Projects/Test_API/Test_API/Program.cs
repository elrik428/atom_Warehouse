using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace Test_API
{
    class Program
    {
        static void Main(string[] args)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd");
            int z = 0;
            string[] filePathKots = new string[1];
            string fileKots;


            string path_Srch = @"\\grat1-dev-ap2t\d$\Reporting\ePOSReports\ExportReports\";
            filePathKots = Directory.GetFiles(path_Srch, "KOTSOVOL*", SearchOption.TopDirectoryOnly);
            int resultsKots_Tot = Directory.GetFiles(path_Srch, "KOTSOVOL*", SearchOption.TopDirectoryOnly).Length;
            Console.WriteLine("Finished " + resultsKots_Tot);

            while (z < resultsKots_Tot)
            {
                fileKots = Path.GetFileName(filePathKots[z]);
                System.IO.File.Copy(@"\\grat1-dev-ap2t\d$\Reporting\ePOSReports\ExportReports\" +fileKots, @"\\10.7.17.11\Storage\Euronet2Kotsovolos\" + fileKots, true);
                Console.WriteLine(fileKots + "  OK");
                z++;
            }

            string sourceDirectory = @"\\10.7.17.11\Storage\Euronet2Kotsovolos\";
            
            DirectoryInfo dccSource = new DirectoryInfo(sourceDirectory);
            foreach (FileInfo fi in dccSource.GetFiles())
            {
                //fi.CopyTo(Path.Combine(dccTarget.FullName, fi.Name), true);
                fi.Delete();
                //Console.WriteLine("File {0} succesfully copied. ", fi.Name);

            }


            Console.WriteLine("Done " );

        }
    }
}
