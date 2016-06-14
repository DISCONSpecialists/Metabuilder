using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Inference
{
    public abstract class BaseRule
    {
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        #region Implementation
        public abstract void Apply(Engine e);
        #endregion
	
    }
}
