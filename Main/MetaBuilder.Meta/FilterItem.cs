using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.Meta
{
    public class FilterItem
    {
        public string Filename;
        public FilterItem(string filename)
        {
            Filename = filename;
        }

        public override string ToString()
        {
            if (Filename == "settings.xml")
                return "DISCON";
            return Filename.Replace(".", "").Replace("xml", "").Replace("settings", "").Replace("-", "");
        }
    }
}
