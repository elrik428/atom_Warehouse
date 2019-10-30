using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using System.Net;

namespace TestWebServiceStatus
{
    class Program
    {
        static void Main(string[] args)
        {

            var url = "http://grat1-dev-ap2t.dir.eeft.com:80/ReportServer/ReportExecution2005.asmx";

            try
            {
                var myRequest = (HttpWebRequest)WebRequest.Create(url);

                var response = (HttpWebResponse)myRequest.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //  it's at least in some way responsive
                    //  but may be internally broken
                    //  as you could find out if you called one of the methods for real
                    Console.WriteLine(string.Format("{0} Available", url));
                    //Debug.Write(string.Format("{0} Available", url));
                }
                else
                {
                    //  well, at least it returned...
                    Console.WriteLine(string.Format("{0} Returned, but with status: {1}",
                        url, response.StatusDescription));
                    ////Debug.Write(string.Format("{0} Returned, but with status: {1}",
                    //    url, response.StatusDescription));
                }
            }
            catch (Exception ex)
            {
                //  not available at all, for some reason
                Console.WriteLine(string.Format("{0} unavailable: {1}", url, ex.Message));
                //Debug.Write(string.Format("{0} unavailable: {1}", url, ex.Message));
            }
        }
    }
}
