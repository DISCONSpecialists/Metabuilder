using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MetaBuilder.Core
{
    public static class Log
    {

        public static void WriteLog(string message)
        {
            if (!Variables.Instance.VerboseLogging) //Dont log stuff we dont need unless specified
                return;

            LogEntry e = new LogEntry();
            e.Title = "Verbose Message";
            e.Severity = System.Diagnostics.TraceEventType.Information;
            e.Message = message;

            //MessageBox.Show(this,e.Message);

            Logger.Write(e);
        }

        public static void WriteLog(string message, string title, System.Diagnostics.TraceEventType severity)
        {
            if (severity == System.Diagnostics.TraceEventType.Information && !Variables.Instance.VerboseLogging)
                return; //Dont log stuff we don need unless specified

            LogEntry e = new LogEntry();
            e.Title = title;
            e.Severity = severity;
            e.Message = message;
            Logger.Write(e);
        }
    }
}