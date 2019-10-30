using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceClient_SSRS.ReportService2010;
//using System.Net;


namespace WebServiceClient_SSRS
{
    class Program
    {
        static void Main(string[] args)
        {
            ReportingService2010 rs = new ReportingService2010();
            //ICredentials credentials_G = new NetworkCredential("epos_support", "1q2w3e4r5t6y!@#");
            //rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            rs.Credentials = new System.Net.NetworkCredential("epos_support", "1q2w3e4r5t6y!@#","\\grat1-dev-ap2t");
            //rs.Credentials = credentials_G;
             rs.Url = "https://grat1-dev-ap2t.dir.eeft.com/ReportServer/ReportService2010.asmx";

            Property name = new Property();
            name.Name = "Name";

            Property description = new Property();
            description.Name = "Description";

            Property[] properties = new Property[2];
            properties[0] = name;
            properties[1] = description;

            try
            {
                Property[] returnproperties = rs.GetProperties("https://grat1-dev-ap2t.dir.eeft.com/Reports/Pages/Report.aspx?ItemPath=%2fTestFolder%2fReport6", properties);

                foreach(Property p in returnproperties)
                {
                    Console.WriteLine(p.Name + ": " + p.Value);
                    Console.ReadLine();
                    
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
