using System;
using System.Collections.Generic;
using MetaBuilder.Meta;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class BindingInfo
    {
        #region Fields (4) 

        private string bindingClass;
        private Dictionary<string, string> bindings;
        private int boundObjectID;
        private List<RepeaterBindingInfo> repeaterBindings;

        #endregion Fields 

        #region Constructors (1) 

        #endregion Constructors 

        #region Properties (4) 

        public string BindingClass
        {
            get { return bindingClass; }
            set { bindingClass = value; }
        }

        public Dictionary<string, string> Bindings
        {
            get
            {
                if (bindings == null)
                    bindings = new Dictionary<string, string>();
                return bindings;
            }
            set { bindings = value; }
        }

        public int BoundObjectID
        {
            get { return boundObjectID; }
            set { boundObjectID = value; }
        }

        public List<RepeaterBindingInfo> RepeaterBindings
        {
            get
            {
                if (repeaterBindings == null)
                    repeaterBindings = new List<RepeaterBindingInfo>();
                return repeaterBindings;
            }
            set { repeaterBindings = value; }
        }

        #endregion Properties 

        #region Methods (3) 

        // Public Methods (3) 

        public BindingInfo Copy()
        {
            BindingInfo retval = new BindingInfo();
            retval.Bindings = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> valuepair in Bindings)
            {
                retval.Bindings.Add(valuepair.Key, valuepair.Value);
            }
            if (RepeaterBindings.Count > 0)
            {
                retval.RepeaterBindings = new List<RepeaterBindingInfo>();
                foreach (RepeaterBindingInfo rbinfo in RepeaterBindings)
                {
                    retval.RepeaterBindings.Add(rbinfo.Copy());
                }
            }
            else
            {
                retval.RepeaterBindings = null;
            }
            retval.BindingClass = BindingClass;
            return retval;
        }

        public RepeaterBindingInfo GetRepeaterBindingInfo(string RepeaterName)
        {
            foreach (RepeaterBindingInfo rbinfo in repeaterBindings)
            {
                if (rbinfo.RepeaterName == RepeaterName)
                {
                    return rbinfo;
                }
            }

            foreach (RepeaterBindingInfo rbinfo in repeaterBindings)
            {
                if (rbinfo.RepeaterName == RepeaterName.Replace("Columns", "Attributes"))
                {
                    return rbinfo;
                }
            }

            return null;
        }

        public RepeaterBindingInfo GetRepeaterBindingInfo(Guid repeaterID)
        {
            foreach (RepeaterBindingInfo rbinfo in repeaterBindings)
            {
                if (rbinfo.RepeaterID == repeaterID)
                {
                    return rbinfo;
                }
            }

            return null;
        }

        #endregion Methods 
    }

    [Serializable]
    public class RepeaterBindingInfo
    {
        #region Fields (5) 

        private Association association;
        private string boundProperty;
        private Direction repeaterDirection;
        private Guid repeaterID;
        private string repeaterName;

        #endregion Fields 

        #region Enums (1) 

        public enum Direction
        {
            Forward,
            Reverse
        }

        #endregion Enums 

        #region Properties (5) 

        public Association Association
        {
            get { return association; }
            set { association = value; }
        }

        public string BoundProperty
        {
            get { return boundProperty; }
            set { boundProperty = value; }
        }

        public Direction RepeaterDirection
        {
            get { return repeaterDirection; }
            set { repeaterDirection = value; }
        }

        public Guid RepeaterID
        {
            get { return repeaterID; }
            set { repeaterID = value; }
        }

        public string RepeaterName
        {
            get { return repeaterName; }
            set { repeaterName = value; }
        }

        #endregion Properties 

        #region Methods (1) 

        // Public Methods (1) 

        public RepeaterBindingInfo Copy()
        {
            RepeaterBindingInfo retval = new RepeaterBindingInfo();
            retval.Association = Association.Copy();
            retval.BoundProperty = BoundProperty;
            retval.RepeaterDirection = RepeaterDirection;
            retval.RepeaterID = RepeaterID;
            retval.RepeaterName = RepeaterName;
            return retval;
        }

        #endregion Methods 
    }
}