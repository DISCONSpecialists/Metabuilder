using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace MetaBuilder.Core
{
    class EnumDescriptor
    {
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {

                object[] attrs = memInfo[0].GetCustomAttributes(typeof(Description),
                false);

                if (attrs != null && attrs.Length > 0)
                    return ((Description)attrs[0]).Text;
            }
            return en.ToString();
        }
    }
    class Description : Attribute
    {
        public string Text;

        public Description(string text)
        {
            Text = text;
        }
    }

    public class strings
    {
        public static string GetEnumDescription(Enum enumToDescribe)
        {
            return EnumDescriptor.GetDescription(enumToDescribe);

        }
        public static string GetFileNameOnly(string fullPath)
        {
            int lastSlash = fullPath.LastIndexOf('\\');
            return fullPath.Substring(lastSlash + 1, fullPath.Length - (lastSlash + 1));
        }

        public static string GetVCIdentifier()
        {
            return TrimToNumberOfCharacters(Environment.UserName + " @ " + Environment.MachineName, 50);
        }

        public static string TrimToNumberOfCharacters(string text, int limit)
        {
            if (text.Length <= limit)
                return text;
            return text.Substring(0, limit);
        }
        public static string GetFileNameWithoutExtension(string fullPath)
        {
            string filename = GetFileNameOnly(fullPath);
            int lastDot = filename.LastIndexOf('.');
            string filenameonly = filename.Substring(0, lastDot);
            return filenameonly;
        }
        public static string GetDateStampString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.Year.ToString() + "-");
            sb.Append(DateTime.Now.Month.ToString() + "-");
            sb.Append(DateTime.Now.Day.ToString() + " at ");
            sb.Append(DateTime.Now.Hour.ToString() + ".");
            sb.Append(DateTime.Now.Minute.ToString() + ".");
            sb.Append(DateTime.Now.Second.ToString());
            return sb.ToString();
        }

        public static string GetFileExtension(string FullFilePath)
        {
            string filename = GetFileNameOnly(FullFilePath);
            int lastDot = filename.LastIndexOf('.');
            string ext = filename.Substring(lastDot + 1, filename.Length - lastDot - 1);
            return ext;
        }
        public static string GetPath(string FullPath)
        {
            return FullPath.Substring(0, FullPath.LastIndexOf("\\") + 1);
        }

        /// <summary>
        /// Converts a long path to a short representative one using ellipsis if necessary
        /// </summary>
        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern Boolean PathCompactPathEx([MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszOut, [MarshalAs(UnmanagedType.LPTStr)] String pszSource, [MarshalAs(UnmanagedType.U4)] Int32 cchMax, [MarshalAs(UnmanagedType.U4)] Int32 dwReserved);
        public static String GetCompactPath(String path, int Max)
        {
            StringBuilder sb = new StringBuilder(Max);
            PathCompactPathEx(sb, path, Max, 0);
            return sb.ToString();
        }

        public static string replaceApostrophe(string value)
        {
            string returnValue = "";
            string oldValue = value;
            //replace each apostrophe in this string with 2 apostrophes
            for (int p = 0; p < value.Length; p++)
            {
                if (value.Substring(p, 1) == "'")
                    returnValue += "''";
                else
                    returnValue += value.Substring(p, 1);
            }
            if (returnValue.Length == 0 && oldValue.Length > 0)
                return oldValue;
            return returnValue;
        }

        public static string ForceDot(string value)
        {
            return (value == null) ? value : value.Replace(",", ".");
        }

    }
}