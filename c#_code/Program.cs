﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Diagnostics;
using SMTPClass;


namespace DailyEPOS_Data
{
    class Program
    {
        public static string OutputDirectory = @"\\10.7.17.11\storage\PDS_TMS_Reports\";
        //public static string OutputDirectory = @"\\GRAT1-FPS-SV2O\Common\Users\PDS_TMS_Reports\";
        public static string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        //public static string timestamp = DateTime.Now.ToShortDateString();
        public static OleDbConnection VCConn = null;
        //public static string DBName="VeriCentre";//Test
        //public static string VCIP="Provider=SQLOLEDB;Data Source=10.1.95.32;Connect Timeout=30;Initial Catalog=VeriCentre";//Test
        public static string DBName = "vc30";//Production
        public static string VCIP = "Provider=SQLOLEDB;Data Source=10.1.45.144;Connect Timeout=30;Initial Catalog=vc30";//Production

        //public static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + "VCreport_" + timestamp + ".xls;Extended Properties=Excel 8.0;";
        public static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + "EPOS_VC.xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES;';";

        public static string mailBody;
        public static string emailTo, emailCC, emailBCC;
        public static string Path;


        static void Main(string[] args)
        {
            string lstDigiTID = " ";

            Path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Path = Path.Substring(0, Path.LastIndexOf('\\') + 1);
            emailTo = "GR-POS-Developers-Dl@euronetworldwide.com";


            connectVC();
            if (VCConn.State != ConnectionState.Open)
            {
                Console.WriteLine("ERROR: Unable to connect to VeriCentre.");
                //ErrorFile += "ERROR: Unable to connect to VeriCentre.\r\n";
                //ErrorCount++;
                return;
            }

            // Delete xls file from loaction
            System.IO.File.Delete(@"\\10.7.17.11\storage\PDS_TMS_Reports\EPOS_VC.xlsx");

            //            Stopwatch swra = new Stopwatch();
            //            swra.Start();
            string sqlCrtQry = "create table  [vc30].[tmp_Tms_EPOS] " +
                                "([TID] [varchar](60),                        " +
                                "[MID] [varchar](60),                         " +
                                "[ΔΙΑΚΡΙΤΙΚΟΣ ΤΙΤΛΟΣ] [varchar](60),          " +
                                "[ΔΙΕΥΘΥΝΣΗ] [varchar](60),                   " +
                                "[ΠΟΛΗ] [varchar](60),	                      " +
                                "[ΤΗΛΕΦΩΝΟ] [varchar](60),	                  " +
                                "[ΔΟΣΕΙΣ] [varchar](60),                      " +
                                "[ΠΛΗΘΟΣ ΔΟΣΕΙΩΝ] [varchar](60),              " +
                                "[ΓΛΩΣΣΑ] [varchar](60),                      " +
                                "[ΠΛΗΚΤΡΟΛΟΓΗΣΗ] [varchar](60),               " +
                                "[CVV] [varchar](60),                         " +
                                "[ΠΡΟΕΓΚΡΙΣΗ] [varchar](60),                  " +
                                "[COMPLETION] [varchar](60),                  " +
                                "[DCC] [varchar](60),                         " +
                                "[CUP] [varchar](60),                         " +
                                "[ΚΑΡΤΑ ΣΥΜΒΟΛΑΙΑΚΗΣ] [varchar](60),          " +
                                "[ΣΥΝΔΕΣΗ]	[varchar](60),                    " +
                                "[ΤΥΠΟΣ ΚΑΡΤΑΣ SIM]	[varchar](60),            " +
                                "[ΑΥΤΟΜΑΤΗ ΑΠΟΣΤΟΛΗ ΠΑΚΕΤΟΥ]	[varchar](60)," +
                                "[CONTACTLESS]	[varchar](60),                " +
                                "[ΦΟΡΟΚΑΡΤΑ]	[varchar](60),                " +
                                "[LAST PARAMETER CALL]	[varchar](60),        " +
                                "[LAST FAILED CALL]	[varchar](60),            " +
                                "[ΣΥΝΔΕΣΗ ΤΑΜΕΙΑΚΗΣ]	[varchar](60),        " +
                                "[TIP]	[varchar](60),                        " +
                                "[ΣΥΝΔΕΣΗ PINPAD]	[varchar](60),            " +
                                "[ΕΚΔΟΣΗ ΕΦΑΡΜΟΓΗΣ]	[varchar](60),            " +
                                "[ΜΟΝΤΕΛΟ ΤΕΡΜΑΤΙΚΟΥ]	[varchar](60),        " +
                                "[ΚΑΤΑΣΚΕΥΑΣΤΗΣ]	[varchar](60),            " +
                                "[LOYALTY]	[varchar](60),                    " +
                                "[BIRTH DATE] [varchar] (60),                 " +
                                "[MCC] [varchar] (60),                        " +
                                "[ETH_DHCP] [varchar] (60),                   " +
                                "[Refund VISA] [varchar] (60),                " +
                                "[Refund MASTERCARD] [varchar] (60),          " +
                                "[Refund MAESTRO] [varchar] (60),             " +
                                "[EASYPAYPOINT] [varchar] (60),               " +
                                "[SERIAL NUMBER] [varchar] (32))               ";

            string sqlDrpQry = "Drop table  [vc30].[tmp_Tms_EPOS]";
            //string sqlDelQry = "delete from [vc30offline].[vc30].[tmp_Tms_EPOS]";

            string sqlSelQry = "Select   [TID]                " +
                                ",[MID]                       " +
                                ",[ΔΙΑΚΡΙΤΙΚΟΣ ΤΙΤΛΟΣ]        " +
                                ",[ΔΙΕΥΘΥΝΣΗ]                 " +
                                ",[ΠΟΛΗ]                      " +
                                ",[ΤΗΛΕΦΩΝΟ]                  " +
                                ",[ΔΟΣΕΙΣ]                    " +
                                ",[ΠΛΗΘΟΣ ΔΟΣΕΙΩΝ]            " +
                                ",[ΓΛΩΣΣΑ]                    " +
                                ",[ΠΛΗΚΤΡΟΛΟΓΗΣΗ]             " +
                                ",[CVV]                       " +
                                ",[ΠΡΟΕΓΚΡΙΣΗ]                " +
                                ",[COMPLETION]                " +
                                ",[DCC]                       " +
                                ",[CUP]                       " +
                                ",[ΚΑΡΤΑ ΣΥΜΒΟΛΑΙΑΚΗΣ]        " +
                                ",[ΣΥΝΔΕΣΗ]                   " +
                                ",[ΤΥΠΟΣ ΚΑΡΤΑΣ SIM]          " +
                                ",[ΑΥΤΟΜΑΤΗ ΑΠΟΣΤΟΛΗ ΠΑΚΕΤΟΥ] " +
                                ",[CONTACTLESS]               " +
                                ",[ΦΟΡΟΚΑΡΤΑ]                 " +
                                ",[LAST PARAMETER CALL]       " +
                                ",[LAST FAILED CALL]          " +
                                ",[ΣΥΝΔΕΣΗ ΤΑΜΕΙΑΚΗΣ]         " +
                                ",[TIP]                       " +
                                ",[ΣΥΝΔΕΣΗ PINPAD]            " +
                                ",[ΕΚΔΟΣΗ ΕΦΑΡΜΟΓΗΣ]          " +
                                ",[ΜΟΝΤΕΛΟ ΤΕΡΜΑΤΙΚΟΥ]        " +
                                ",[ΚΑΤΑΣΚΕΥΑΣΤΗΣ]             " +
                                ",[LOYALTY]                   " +
                                ",[BIRTH DATE]                " +
                                ",[MCC]         " +
                                ",[ETH_DHCP]    " +
                                ",[Refund VISA] " +
                                ",[Refund MASTERCARD]  " +
                                ",[Refund MAESTRO]  " +
                                ",[EASYPAYPOINT]  " +
                                ",[SERIAL NUMBER] " +
                                "from  [vc30].[tmp_Tms_EPOS] " +
                                "where [TID] <> '44444444' ";

            // Check for table [vc30offline].[vc30].[tmp_Tms_EPOS] if exists so to create or not
            bool isExists;
            const string sqlStmnt = @"Select count(*) from  [vc30].[tmp_Tms_EPOS]";

            try
            {
                using (OleDbCommand chkExistance = new OleDbCommand(sqlStmnt, VCConn))
                {
                    chkExistance.ExecuteScalar();
                    isExists = true;
                }
            }
            catch
            {
                isExists = false;
            }



            if (isExists == false)
            {
                OleDbCommand sqlcmdCrt = new OleDbCommand();
                sqlcmdCrt.Connection = VCConn;
                sqlcmdCrt.CommandTimeout = 0;
                sqlcmdCrt.CommandType = CommandType.Text;
                //sqlcmdDel.CommandText = sqlDelQry;
                sqlcmdCrt.CommandText = sqlCrtQry;
                OleDbDataReader rdrDel = sqlcmdCrt.ExecuteReader();
                rdrDel.Close();
                sqlcmdCrt.Dispose();
            }

            for (int i = 0; i < 10; i++)
            {

                lstDigiTID = i.ToString();

                string sqlInsertQry = "insert into  [vc30].[tmp_Tms_EPOS]		                            " +
"select  distinct  cast(vc30.relation.TERMID as varchar(30)) as TID,                                                " +
"cast(a.[value] as varchar(30)) as MID,                                                                             " +
"b.[value] as [ΔΙΑΚΡΙΤΙΚΟΣ ΤΙΤΛΟΣ],                                                                                 " +
"c.[value] as [ΔΙΕΥΘΥΝΣΗ],                                                                                          " +
"d.[value] as [ΠΟΛΗ],                                                                                               " +
"e.[value] as [ΤΗΛΕΦΩΝΟ],                                                                                           " +
"case (f.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [ΔΟΣΕΙΣ],                        " +
"cast(g.[value] as varchar(30)) as [ΠΛΗΘΟΣ ΔΟΣΕΙΩΝ],                                                                " +
"h.[value] as [ΓΛΩΣΣΑ],                                                                                             " +
"i.[value] as [ΠΛΗΚΤΡΟΛΟΓΗΣΗ],                                                                                      " +
"case (j.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [CVV],                           " +
"case (substring(k.[value],12,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end  as [ΠΡΟΕΓΚΡΙΣΗ],           " +
"case (substring(k.[value],20,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end  as [COMPLETION],           " +
"case (l.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [DCC],                           " +
"case (substring(m.[value],10,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end  as [CUP],                  " +
"case (substring(n.[value],6,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end  as [ΚΑΡΤΑ ΣΥΜΒΟΛΑΙΑΚΗΣ],    " +
"o.[value] as [ΣΥΝΔΕΣΗ],                                                                                            " +
"p.[value] as [ΤΥΠΟΣ ΚΑΡΤΑΣ SIM],                                                                                   " +
"case (q.[value]) when '99:99' then 'NO' else q.[value] end  as [ΑΥΤΟΜΑΤΗ ΑΠΟΣΤΟΛΗ ΠΑΚΕΤΟΥ],                        " +
"case (r.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [CONTACTLESS],                   " +
"case (s.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [ΦΟΡΟΚΑΡΤΑ],                     " +
"x.evdate as [LAST PARAMETER CALL],  " +                                                                           // vc30.relation.LASTPARAM_DLD_DATE as [LAST PARAMETER CALL]
"case (t.[value]) when '1' then 'RS232' when '2' then 'TCP' when '3' then 'TCP+RS232' when '4' then 'USB' when '6' then 'TCP+USB' else 'NO CONNECTION' end  as [ΣΥΝΔΕΣΗ ΤΑΜΕΙΑΚΗΣ], " +
"case (u.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [TIP],							" +
"case (v.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [ΣΥΝΔΕΣΗ PINPAD],  " +
"x.late_versio as [ΕΚΔΟΣΗ ΕΦΑΡΜΟΓΗΣ],                                                                 " +   //(Replaced)vc30.relation.appnm as [ΕΚΔΟΣΗ ΕΦΑΡΜΟΓΗΣ]
"xx.evdate as [LAST FAILED CALL],                                                                     " +
"vc30.relation.famnm as [ΜΟΝΤΕΛΟ ΤΕΡΜΑΤΙΚΟΥ],                                                         " +
"'VeriFone' as [ΚΑΤΑΣΚΕΥΑΣΤΗΣ],                                                                       " +
"case (w.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [LOYALTY],         " +
"vc30.TERMINFO.datecreated as [BIRTH DATE] ,                                                          " +
" ' ' ,' ' ,' ' ,' ' ,' ' ,' ' ,                                                                      " +
"vc30.TERMINFO.SERIALNUMBER as [SERIAL NUMBER]                                                           " +
"from (vc30.relation                                                                                  " +
"      left join vc30.PARAMETER a on vc30.relation.TERMID=a.PARTID and a.PARNAMELOC = 'MERCHID'       " +
"      left join vc30.PARAMETER b on vc30.relation.TERMID=b.PARTID and b.PARNAMELOC = 'RECEIPT_LINE_1'" +
"      left join vc30.PARAMETER c on vc30.relation.TERMID=c.PARTID and c.PARNAMELOC = 'RECEIPT_LINE_2'" +
"      left join vc30.PARAMETER d on vc30.relation.TERMID=d.PARTID and d.PARNAMELOC = 'RECEIPT_LINE_3'" +
"      left join vc30.PARAMETER e on vc30.relation.TERMID=e.PARTID and e.PARNAMELOC = 'RECEIPT_LINE_4'" +
"      left join vc30.PARAMETER f on vc30.relation.TERMID=f.PARTID and f.PARNAMELOC = 'INSTALMENT'    " +
"      left join vc30.PARAMETER g on vc30.relation.TERMID=g.PARTID and g.PARNAMELOC = 'MAXINST'       " +
"      left join vc30.PARAMETER h on vc30.relation.TERMID=h.PARTID and h.PARNAMELOC = 'LANGUAGE'      " +
"      left join vc30.PARAMETER i on vc30.relation.TERMID=i.PARTID and i.PARNAMELOC = 'MANUALENTRY'   " +
"      left join vc30.PARAMETER j on vc30.relation.TERMID=j.PARTID and j.PARNAMELOC = 'ASKFORCVV2'    " +
"      left join vc30.PARAMETER k on vc30.relation.TERMID=k.PARTID and k.PARNAMELOC = 'CARD01'        " +
"      left join vc30.PARAMETER l on vc30.relation.TERMID=l.PARTID and l.PARNAMELOC = 'DCCCAPABLE'    " +
"      left join vc30.PARAMETER m on vc30.relation.TERMID=m.PARTID and m.PARNAMELOC = 'CARD08'        " +
"      left join vc30.PARAMETER n on vc30.relation.TERMID=n.PARTID and n.PARNAMELOC = 'CARD07'        " +
"      left join vc30.PARAMETER o on vc30.relation.TERMID=o.PARTID and o.PARNAMELOC = 'MEDIA'         " +
"      left join vc30.PARAMETER p on vc30.relation.TERMID=p.PARTID and p.PARNAMELOC = 'CONFIG_APN'    " +
"      left join vc30.PARAMETER q on vc30.relation.TERMID=q.PARTID and q.PARNAMELOC = 'AUTOCALL'      " +
"      left join vc30.PARAMETER r on vc30.relation.TERMID=r.PARTID and r.PARNAMELOC = 'CONTACTLESS'   " +
"      left join vc30.PARAMETER s on vc30.relation.TERMID=s.PARTID and s.PARNAMELOC = 'FOROKARTA'     " +
"      left join vc30.PARAMETER t on vc30.relation.TERMID=t.PARTID and t.PARNAMELOC = 'DLL_CONN'      " +
"      left join vc30.PARAMETER u on vc30.relation.TERMID=u.PARTID and u.PARNAMELOC = 'TIP_ENABLED'   " +
"      left join vc30.PARAMETER v on vc30.relation.TERMID=v.PARTID and v.PARNAMELOC = 'EXTPINPAD'     " +
"      left join vc30.PARAMETER w on vc30.relation.TERMID=w.PARTID and w.PARNAMELOC = 'LOYALTY_PBG'   " +
"      left join vc30.TERMINFO    on vc30.relation.TERMID=termid_term                                 " +
"      join  (select e.*, " +			
"         case (substring(e.vers_pir,9,1)) when ',' then substring(e.vers_pir,1,8) + 'P' when 'P' then (e.vers_pir) end as late_versio " +                                             
"          from " +                                                                                                                                                                     
"          (select a.evdate, a.termid, a.appnm , substring(a.appnm,charindex('EPOS', a.appnm),9) vers_pir " +                                                                          
"          from vc30.termlog a,(select  max(evdate) date_max, termid  from vc30.termlog where  " +
"          substring(vc30.termlog.termid,1,4) = '7300' and  RIGHT(vc30.termlog.TERMID,1) = '" + lstDigiTID + "'" +
"          and message = 'Download Successful' and status = 'S' " +
"          group by termid) q " +
"          where  a.evdate = q.date_max and a.termid = q.termid and appnm like '%EPOS%' and appnm not in ('EPOS_PIRAEUS','EPOS_PIRAEUS_EPP' ))e) x on vc30.relation.TERMID=x.termid  " +
"          join (select e.*, " +
"          case (substring(e.vers_epos,9,1)) when ',' then substring(e.vers_epos,1,8) + 'P' when 'P' then (e.vers_epos) end as late_versio " +
"          from  " +
"          (select a.evdate, a.termid, a.appnm , substring(a.appnm,charindex('EPOS', a.appnm),9) vers_epos " +
"          from vc30.termlog a,(select  max(evdate) date_max, termid  from vc30.termlog where   " +
"          substring(vc30.termlog.termid,1,4) = '7300'  and  RIGHT(vc30.termlog.TERMID,1) = '" + lstDigiTID + "'" +
"          and message <> 'Download Successful' and status <> 'S' " +
"          group by termid) q  " +
"          where  a.evdate = q.date_max and a.termid = q.termid and appnm like '%EPOS%' and appnm not in ('EPOS_PIRAEUS','EPOS_PIRAEUS_EPP' ))e) xx on vc30.relation.TERMID=xx.termid  " +
"             )  " +
" where substring(cast(vc30.relation.appnm as char(10)),9,1) = ('P')  " +
" and  vc30.relation.CLUSTERID not in ('EPOS_PIRAEUS','EPOS_PIRAEUS_EPP')  " +
" AND substring(vc30.termlog.termid,1,4) = '7300' AND RIGHT(vc30.relation.TERMID,1)  = '" + lstDigiTID + "'" ;


                // Added  by request from y --> ac    PIR2018522-144215
                string updXtraParmQry =
"update   [vc30].[tmp_Tms_EPOS]                                                                           " +
"set [ETH_DHCP] = case ([value]) when '1' then 'YES' when '0' then 'NO' else 'N/A' end                            " +
"from  vc30.tmp_Tms_EPOS                                                                                 " +
"left join vc30.parameter on PARTID = tid                                                                         " +
"where parnameloc = 'ETH.DHCP';                                                                                   " +
"update   [vc30].[tmp_Tms_EPOS]                                                                           " +
"set [Refund VISA] = case (substring([value],10,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end         " +
"from  vc30.tmp_Tms_EPOS                                                                                 " +
"join vc30.parameter on PARTID = tid                                                                              " +
"where parnameloc = 'CARD01';                                                                                     " +
"update   [vc30].[tmp_Tms_EPOS]                                                                           " +
"set [Refund MASTERCARD] = case (substring([value],12,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end   " +
"from   vc30.tmp_Tms_EPOS                                                                                 " +
"join vc30.parameter on PARTID = tid                                                                              " +
"where parnameloc = 'CARD03';                                                                                     " +
"update  [vc30].[tmp_Tms_EPOS]                                                                           " +
"set [Refund MAESTRO] = case (substring([value],13,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end      " +
"from   vc30.tmp_Tms_EPOS                                                                                 " +
"join vc30.parameter on PARTID = tid                                                                              " +
"where parnameloc = 'CARD04';                                                                                     " +
"update   [vc30].[tmp_Tms_EPOS]                                                                           " +
"set [EASYPAYPOINT] = case ([value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end                 " +
"from   vc30.tmp_Tms_EPOS                                                                                 " +
"join vc30.parameter on PARTID = tid                                                                              " +
"where parnameloc = 'SERVICES_ENABLED';                                                                           " +
"update   [vc30].[tmp_Tms_EPOS]                                                                           " +
"set [MCC] = [value]                                                                                              " +
"from   vc30.tmp_Tms_EPOS                                                                                 " +
"join vc30.parameter on PARTID = tid                                                                              " +
"where parnameloc = 'MCC';       																				  " +
"update  [vc30].[tmp_Tms_EPOS]                                                                            " +
"set EASYPAYPOINT = case EASYPAYPOINT when ' '   then 'N/A' else EASYPAYPOINT end                                 ";


                OleDbCommand sqlcmd = new OleDbCommand();
                sqlcmd.Connection = VCConn;
                sqlcmd.CommandTimeout = 0;
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = sqlInsertQry;


                OleDbCommand sqlcmd_Upd = new OleDbCommand();
                sqlcmd_Upd.Connection = VCConn;
                sqlcmd_Upd.CommandTimeout = 0;
                sqlcmd_Upd.CommandType = CommandType.Text;
                sqlcmd_Upd.CommandText = updXtraParmQry;

                // Insert Data
                OleDbDataReader rdrIns = sqlcmd.ExecuteReader();
                rdrIns.Close();
                // Update Parms
                OleDbDataReader rdrUpdParm = sqlcmd_Upd.ExecuteReader();
                rdrUpdParm.Close();

                Console.WriteLine("Import TIDs ending to " + lstDigiTID + "  in  [vc30].[tmp_Tms_EPOS] file");
            }

            //Select records from TMP file so to write to excel
            OleDbCommand sqlcmdSel_Excel = new OleDbCommand();
            sqlcmdSel_Excel.Connection = VCConn;
            sqlcmdSel_Excel.CommandTimeout = 0;
            sqlcmdSel_Excel.CommandType = CommandType.Text;
            sqlcmdSel_Excel.CommandText = sqlSelQry;
            OleDbDataReader rdrSelExcel = sqlcmdSel_Excel.ExecuteReader();

            OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [EPOS_VC Report] ([TID] char(255),[MID] char(255),[ΔΙΑΚΡΙΤΙΚΟΣ ΤΙΤΛΟΣ] char(255),[ΔΙΕΥΘΥΝΣΗ] char(255),[ΠΟΛΗ] char(255),[ΤΗΛΕΦΩΝΟ] char(255), " +
 " [ΔΟΣΕΙΣ] char(255),[ΠΛΗΘΟΣ ΔΟΣΕΩΝ] char(255),[ΓΛΩΣΣΑ] char(255),[ΠΛΗΚΤΡΟΛΟΓΗΣΗ] char(255),[CVV] char(255),[ΠΡΟΕΓΚΡΙΣΗ] char(255),[COMPLETION] char(255), " +
 " [DCC] char(255),[CUP] char(255),[ΚΑΡΤΑ ΣΥΜΒΟΛΑΙΑΚΗΣ] char(255),[ΣΥΝΔΕΣΗ]	char(255),[ΤΥΠΟΣ ΚΑΡΤΑΣ SIM] char(255),[ΑΥΤΟΜΑΤΗ ΑΠΟΣΤΟΛΗ ΠΑΚΕΤΟΥ] char(255),[CONTACTLESS] char(255), " +
 " [ΦΟΡΟΚΑΡΤΑ] char(255),[LAST PARAMETER CALL] char(255),[LAST FAILED CALL] char(255),[ΣΥΝΔΕΣΗ ΤΑΜΕΙΑΚΗΣ] char(255),[TIP] char(255),[ΣΥΝΔΕΣΗ PINPAD] char(255),[ΕΚΔΟΣΗ ΕΦΑΡΜΟΓΗΣ] char(255),[ΜΟΝΤΕΛΟ ΤΕΡΜΑΤΙΚΟΥ] char(255), " +
 " [ΚΑΤΑΣΚΕΥΑΣΤΗΣ] char(255),[LOYALTY] char(255),[BIRTH DATE] char(255),[MCC] char(255),[ETH_DHCP] char(255),[VISA REFUND] char(255),[MASTERCARD REFUND] char(255),[MAESTRO REFUND] char(255),[EASYPAYPOINT] char(255), [SERIAL NUMBER] char(255) )");

            OleDbConnection myConnection = new OleDbConnection(connectionString);
            myConnection.Open();
            dbCmd.Connection = myConnection;
            dbCmd.ExecuteNonQuery();

            while (rdrSelExcel.Read())
            {

                OleDbCommand myCommand = new OleDbCommand("Insert into [EPOS_VC Report] ([TID],[MID],[ΔΙΑΚΡΙΤΙΚΟΣ ΤΙΤΛΟΣ],[ΔΙΕΥΘΥΝΣΗ],[ΠΟΛΗ],[ΤΗΛΕΦΩΝΟ], " +
               " [ΔΟΣΕΙΣ],[ΠΛΗΘΟΣ ΔΟΣΕΩΝ],[ΓΛΩΣΣΑ],[ΠΛΗΚΤΡΟΛΟΓΗΣΗ],[CVV],[ΠΡΟΕΓΚΡΙΣΗ],[COMPLETION],[DCC],[CUP], " +
               " [ΚΑΡΤΑ ΣΥΜΒΟΛΑΙΑΚΗΣ],[ΣΥΝΔΕΣΗ],[ΤΥΠΟΣ ΚΑΡΤΑΣ SIM],[ΑΥΤΟΜΑΤΗ ΑΠΟΣΤΟΛΗ ΠΑΚΕΤΟΥ],[CONTACTLESS],[ΦΟΡΟΚΑΡΤΑ], " +
               " [LAST PARAMETER CALL],[ΣΥΝΔΕΣΗ ΤΑΜΕΙΑΚΗΣ],[TIP],[ΣΥΝΔΕΣΗ PINPAD], " +
               " [ΕΚΔΟΣΗ ΕΦΑΡΜΟΓΗΣ],[ΜΟΝΤΕΛΟ ΤΕΡΜΑΤΙΚΟΥ],[ΚΑΤΑΣΚΕΥΑΣΤΗΣ],[LOYALTY],[BIRTH DATE],[MCC],[ETH_DHCP],[VISA REFUND],[MASTERCARD REFUND],[MAESTRO REFUND],[EASYPAYPOINT],[SERIAL NUMBER] ) " +
               " values ('" + rdrSelExcel.GetValue(0).ToString() + "','" + rdrSelExcel.GetValue(1).ToString() + "','" + rdrSelExcel.GetValue(2).ToString().Replace('\'', ' ') + "','" + rdrSelExcel.GetValue(3).ToString().Replace('\'', ' ') + "','"
                + rdrSelExcel.GetValue(4).ToString().Replace('\'', ' ') + "','" + rdrSelExcel.GetValue(5).ToString().Replace('\'', ' ') + "','" + rdrSelExcel.GetValue(6).ToString() + "','" + rdrSelExcel.GetValue(7).ToString() + "','"
                + rdrSelExcel.GetValue(8).ToString() + "','" + rdrSelExcel.GetValue(9).ToString() + "','" + rdrSelExcel.GetValue(10).ToString() + "','" + rdrSelExcel.GetValue(11).ToString() + "','"
                + rdrSelExcel.GetValue(12).ToString() + "','" + rdrSelExcel.GetValue(13).ToString() + "','" + rdrSelExcel.GetValue(14).ToString() + "','" + rdrSelExcel.GetValue(15).ToString() + "','"
                + rdrSelExcel.GetValue(16).ToString() + "','" + rdrSelExcel.GetValue(17).ToString() + "','" + rdrSelExcel.GetValue(18).ToString() + "','" + rdrSelExcel.GetValue(19).ToString() + "','"
                + rdrSelExcel.GetValue(20).ToString() + "','" + rdrSelExcel.GetValue(21).ToString() + "','" + rdrSelExcel.GetValue(22).ToString() + "','" + rdrSelExcel.GetValue(23).ToString() + "','"
                + rdrSelExcel.GetValue(24).ToString() + "','" + rdrSelExcel.GetValue(25).ToString() + "','" + rdrSelExcel.GetValue(26).ToString() + "','" + rdrSelExcel.GetValue(27).ToString() + "','"
                + rdrSelExcel.GetValue(28).ToString() + "','" + rdrSelExcel.GetValue(29).ToString() + "','" + rdrSelExcel.GetValue(30).ToString() + "','" + rdrSelExcel.GetValue(31).ToString() + "','"
                + rdrSelExcel.GetValue(32).ToString() + "','" + rdrSelExcel.GetValue(33).ToString() + "','" + rdrSelExcel.GetValue(34).ToString() + "','" + rdrSelExcel.GetValue(35).ToString() + "','"
                + rdrSelExcel.GetValue(36).ToString() + "','" + rdrSelExcel.GetValue(37).ToString() + "')");

                myCommand.Connection = myConnection;
                myCommand.ExecuteNonQuery();
            }

            Console.WriteLine("Import finished");
            Console.Write('\r');
            myConnection.Close();
            dbCmd.Dispose();
            rdrSelExcel.Close();

            // Drop temp table
            OleDbCommand sqlcmdDrp = new OleDbCommand();
            sqlcmdDrp.Connection = VCConn;
            sqlcmdDrp.CommandTimeout = 0;
            sqlcmdDrp.CommandType = CommandType.Text;
            //sqlcmdDel.CommandText = sqlDelQry;
            sqlcmdDrp.CommandText = sqlDrpQry;
            OleDbDataReader rdrDrp = sqlcmdDrp.ExecuteReader();
            rdrDrp.Close();
            sqlcmdDrp.Dispose();


            VCConn.Close();

            mailBody += "Weekly VeriCentre TIDs successfully exported";
            //MailClass.SendMail(Path, "VeriCentre Weekly TIDs", mailBody, emailTo, "", "");

        }

        static void connectVC()
        {

            /************CONNECT**************/
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    VCConn = new OleDbConnection(VCIP + ";User Id=vc30;Password=VcEn!@#20130627");//Production
                    //VCConn = new OleDbConnection(VCIP + ";User Id=VeriCentre;Password=VcLe!@#20130423");//Test
                    VCConn.Open();

                    if (VCConn.State == ConnectionState.Open)
                        i = 3;
                }
                catch (Exception exc)
                {


                }

            }

        }
    }
}
