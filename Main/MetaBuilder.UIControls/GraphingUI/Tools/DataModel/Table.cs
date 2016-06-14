using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel
{
    public class Table
    {
        private string name;
        public string Name 
        {
            get { return name; }
            set { name = value;}
        }
        private List<Column> columns;

        public List<Column> Columns 
        {
            get { return columns; }
            set { columns = value; }
        }
    }

    public class Column
    {
        private bool pk;
        public bool PK 
        {
            get { return pk; }
            set { pk = value;}
        }
        private bool fk;
        public bool FK 
        {
            get { return fk; }
            set { fk = value;}
        }
        private string name;
        public string Name 
        {
            get { return name; }
            set { name = value; }
        }
    }
}
