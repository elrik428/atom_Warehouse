using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Threading;
using Ini;
using SMTPClass;

namespace PinExceptionCons
{
   public partial class PinException
    {
        bool run;
       static string toEmailList, ccEmailList, bccEmailList;
       static string inputDirectory, outputDirectory, exceptionDaysDirectory;
       static ParticipantInfo[] Participants;
       static DateTime mailDate;
       //static private string path;
       int execHour, execMinute;
       //static private string path2 = AppDomain.CurrentDomain.BaseDirectory;
       static private string path = Directory.GetCurrentDirectory();
       static private string path_2 = System.Reflection.Assembly.GetEntryAssembly().Location;
       int enable;
      
     
       static void ReadSetup(string path)
        {
           
            IniFile ini = new IniFile(path);
            inputDirectory = ini.IniReadValue("Directories", "InputDirectory");
            outputDirectory = ini.IniReadValue("Directories", "OutputDirectory");
            exceptionDaysDirectory = ini.IniReadValue("Directories", "ExceptionsDirectory");
            toEmailList = ini.IniReadValue("Email", "To Email List");
            ccEmailList = ini.IniReadValue("Email", "CC Email List");
            bccEmailList = ini.IniReadValue("Email", "BCC Email List");

            int count = 1;
            string pData;
            while ((pData = ini.IniReadValue("Participants", "Participant" + count.ToString())) != "")
            {
                string[] partInfo = pData.Split(new char[] { ';' });

                Array.Resize(ref Participants, count);
                Participants[count - 1] = new ParticipantInfo();
                Participants[count - 1].ParticipantFile = partInfo[0];
                Participants[count - 1].ParticipantName = partInfo[1];
                Participants[count - 1].ParticipantSubject = partInfo[2];
                Participants[count - 1].ParticipantExtention = partInfo[3];
                Participants[count - 1].ParticipantExceptionDays = GetExceptionDays(partInfo[4]);
                count++;
            }
         }

       static void Main()
        {
            ReadSetup(path + @"\PinExceptions.ini");

            FileIO fio = new FileIO();

            int i;

            string year, month, day;

            if (DateTime.Now.DayOfWeek != DayOfWeek.Monday)
                mailDate = DateTime.Now.AddDays(-1.0);
            else
                mailDate = DateTime.Now.AddDays(-3.0);

            string dtstamp = DateTime.Now.Ticks.ToString();
            for (i = 0; i < Participants.Length; i++)
            {
                string[] strArrExDays = Participants[i].ParticipantExceptionDays;

                if (Array.Exists(strArrExDays, GetCurrDate))
                    continue;

                while (Array.Exists(strArrExDays, ExDays) ||
                       mailDate.DayOfWeek == DayOfWeek.Saturday || mailDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    mailDate = mailDate.AddDays(-1.0);
                }

                if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                {
                    year = string.Format("{0:D2}", mailDate.Year).Substring(2);
                    month = string.Format("{0:D2}", mailDate.Month);
                    day = string.Format("{0:D2}", mailDate.Day);

                    Participants[i].ParticipantFile = Participants[i].ParticipantFile +
                        year + month + day + Participants[i].ParticipantExtention;

                    if (!fio.FileExists(inputDirectory + Participants[i].ParticipantFile))
                    {
                        if (fio.Open(inputDirectory + Participants[i].ParticipantFile, FileIO.GENERIC_WRITE, FileIO.CREATE_ALWAYS))
                            fio.Close();
                    }

                    Thread.Sleep(5000);

                    MailClass.SendMail(path, Participants[i].ParticipantSubject,
                                       inputDirectory + Participants[i].ParticipantFile,
                                       toEmailList, ccEmailList, bccEmailList);
                    Console.WriteLine("File has been processed and mail is sent");
                    Thread.Sleep(10000);


                    FileIO.MoveFile(inputDirectory + Participants[i].ParticipantFile, outputDirectory + "Proccessed_" +
                                    Participants[i].ParticipantFile + "_" + dtstamp);
                    Console.WriteLine("File has been moved to OUT directory");
                }
            }
        }

      static  private string[] GetExceptionDays(string exDaysFile)
        {
            FileIO fio = new FileIO();
            byte[] buffer = new byte[1024];
            ASCIIEncoding Encoding = new ASCIIEncoding();
            string content = "";

            fio.Open(exceptionDaysDirectory + exDaysFile, FileIO.GENERIC_READ, FileIO.OPEN_EXISTING);

            int bytesRead;
            do
            {
                bytesRead = fio.Read(buffer, 0, buffer.Length);
                content += Encoding.GetString(buffer, 0, bytesRead);
            }
            while (bytesRead > 0);

            fio.Close();

            return (content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
        }

      static  public bool ExDays(string s)
        {
            if (s == mailDate.ToShortDateString())
                return true;
            else
                return false;
        }

      static  public bool GetCurrDate(string s)
        {
            if (s == DateTime.Now.ToShortDateString())
                return true;
            else
                return false;
        }


    }
}
