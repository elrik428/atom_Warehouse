﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;

namespace routingrules_loadbalances
{
    public class TID_BankConn : Form1
    {
        public static void TID_Check(string flgTM_id, string id_Parm, int[] cnt_Bank)
        {
            int[] arrChkExist = new int[7];
            arrChkExist[0] = 1;
            arrChkExist[1] = 6;
            arrChkExist[2] = 201;
            arrChkExist[3] = 205;
            arrChkExist[4] = 7;
            arrChkExist[5] = 302;
            arrChkExist[5] = 989;

            if (flgTM_id == "M")
            {
                for (int i = 0; i < 7; i++)
                {
                    // Check records in backup file and original the same
                    OdbcCommand comCount_1 = new OdbcCommand("select count(*) from MERCHANTS_test where mid = ? and tid not in " +
                                                              "(select tid from MERCHANTS_test where mid = ? and uploadhostid = " + arrChkExist[i] + ")", conn);
                    comCount_1.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = id_Parm;
                    comCount_1.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = id_Parm;
                    cnt_Bank[i] = Convert.ToInt32(comCount_1.ExecuteScalar());
                }
            }
            else if (flgTM_id == "T")
            {
                for (int i = 0; i < 7; i++)
                {
                    OdbcCommand comCount_1 = new OdbcCommand("select count(*) from MERCHANTS_test where tid = ? and tid not in " +
                                                                         "(select tid from MERCHANTS_test where tid = ? and uploadhostid = " + arrChkExist[i] + ")", conn);
                    comCount_1.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = id_Parm;
                    comCount_1.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = id_Parm;
                    cnt_Bank[i] = Convert.ToInt32(comCount_1.ExecuteScalar());
                }

            }

        }
    }
}
