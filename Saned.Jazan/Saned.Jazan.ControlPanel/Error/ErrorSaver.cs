using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Saned.Jazan.ControlPanel.Error
{
    public class ErrorSaver
    {
        public static void SaveError(Exception ex)
        {


            string filePath = HttpContext.Current.Server.MapPath("~/Error/") + "Error.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }


        }


    }
}