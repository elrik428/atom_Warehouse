using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace iTunes_Auto_Reports
{
    public static class Logger
    {
        public static void WriteLog(string message)
        {
            DateTime dt = DateTime.Now;
            string year = string.Format("{0:D2}", dt.Year).Substring(2);
            string month = string.Format("{0:D2}", dt.Month);
            string day = string.Format("{0:D2}", dt.Day);

            string path = Program.Path + "20" + year + month + day + ".log";

            FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write);

            fs.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            fs.Close();
        } 
    }
}
