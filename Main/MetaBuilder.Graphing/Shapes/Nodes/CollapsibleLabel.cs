using System;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class CollapsibleLabel : BoundLabel, IGoCollapsible
    {
        #region Properties (2) 

        public override bool Wrapping
        {
            get { return true; }
            set
            {
                //;
            }
        }

        public override float WrappingWidth
        {
            get { return Width - 2; }
            set
            {
                //
            }
        }

        public override bool AutoResizes
        {
            get { return true; }
            set
            {
                //base.AutoRescales = value;
            }
        }

        public bool Collapsible
        {
            get { return Visible; }
            set { }
        }

        public bool IsExpanded
        {
            get { return Visible; }
        }

        #endregion Properties 

        #region Methods (2) 

        // Public Methods (2) 

        public void Collapse()
        {
        }

        public void Expand()
        {
        }

        #endregion Methods 
    }
}