using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.eHPUM
{
    public class ImportDiagram
    {
        private List<ImportObject> objects;
        public List<ImportObject> Objects
        {
            get
            {
                if (objects == null) objects = new List<ImportObject>();
                return objects;
            }
            set { objects = value; }
        }

        private string name;
        public string Name { get { return name; } set { name = value; } }
    }
}