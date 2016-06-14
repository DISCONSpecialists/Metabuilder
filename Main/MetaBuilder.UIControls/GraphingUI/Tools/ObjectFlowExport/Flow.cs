using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.ObjectFlowExport
{
    public class Flow
    {
        public Flow(string n, int p, string m)
        {
            Name = n;
            PKID = p;
            Machine = m;
        }

        private int pkid;
        public int PKID { get { return pkid; } set { pkid = value; } }
        private string machine;
        public string Machine { get { return machine; } set { machine = value; } }
        private string name;
        public string Name { get { return name; } set { name = value; } }
    }
}