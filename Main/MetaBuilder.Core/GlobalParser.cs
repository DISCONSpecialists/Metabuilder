using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MetaBuilder.Core
{
    public static class GlobalParser
    {
        public static DateTime ParseGlobalisedDateString(string date)
        {
            CultureInfo[] cultInfo = CultureInfo.GetCultures(CultureTypes.AllCultures);
            // try using CURRENT culture
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            DateTime retval = DateTime.Now;
            bool parsed = DateTime.TryParse(date, out retval);
            if (parsed)
                return retval;
            
            // first try the major ones (clients)
            IFormatProvider culture = CultureInfo.InvariantCulture;
            parsed = DateTime.TryParse(date,culture,DateTimeStyles.None, out retval);
            if (parsed)
                return retval;

            string[] majorClientCultures = new string[] {"en-ZA", "en-US", "en-GB","af-ZA"};
            if (!parsed)
            {
                for (int i = 0; i < majorClientCultures.Length; i++)
                {
                    if (!parsed)
                    {
                        CultureInfo majorCult = CultureInfo.CreateSpecificCulture(majorClientCultures[i]);
                        parsed = DateTime.TryParse(date, majorCult, DateTimeStyles.None, out retval);
                    }
                }
            }
            if (parsed)
                return retval;

            for (int i = 0; i < cultInfo.Length; i++)
            {
                if (!parsed)
                {
                    parsed = DateTime.TryParse(date, cultInfo[i], DateTimeStyles.None, out retval);
                }
            }
            if (parsed)
                return retval;

            try
            {
                throw new Exception("Cannot parse datetime");
            }
            catch(Exception xxxdateparse)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.Title = "Cannot parse DateTime correctly";
                StringBuilder sbErrorMessage = new StringBuilder();
                sbErrorMessage.Append("String To Parse:" + date + Environment.NewLine);
                sbErrorMessage.Append("Exception:" + xxxdateparse.ToString());
                Logger.Write(logEntry);    
            }
            
            return DateTime.Now;
        }
    }
}
