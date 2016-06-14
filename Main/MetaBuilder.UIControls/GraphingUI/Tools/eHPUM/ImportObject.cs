using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.BusinessLogic;

namespace MetaBuilder.UIControls.GraphingUI.Tools.eHPUM
{
    public class ImportObject
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

        private int level;
        public int Level { get { return level; } set { level = value; } }

        private string name;
        public string Name { get { return name; } set { name = value; } } //THIS IS UNIQUE ID
        private string description;
        public string Description { get { return description; } set { description = value; } } //CUSTOM 1
        private string objective;
        public string Objective { get { return objective; } set { objective = value; } } //CUSTOM 2

        //For use when templating(defaults are object and decomposition)
        private string _class = "Function";
        public string _Class
        {
            get { return _class; }
            set { _class = value; }
        }
        private Meta.LinkAssociationType _associationtype = MetaBuilder.Meta.LinkAssociationType.Decomposition;
        public Meta.LinkAssociationType _AssociationType
        {
            get { return _associationtype; }
            set { _associationtype = value; }
        }

        public TList<Field> ClassFields;
    }
}