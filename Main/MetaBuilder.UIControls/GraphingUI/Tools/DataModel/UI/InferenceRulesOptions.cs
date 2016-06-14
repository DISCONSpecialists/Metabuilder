using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.UI
{
    public class InferenceRulesOptions
    {
        private bool indicateReflexive;
        public bool IndicateReflexive
        {
            get { return indicateReflexive; }
            set { indicateReflexive  = value; }
        }
        private bool indicateTransitive;

        public bool IndicateTransitive
        {
            get { return indicateTransitive; }
            set { indicateTransitive = value; }
        }

        private bool indicateAugmentive;

        public bool IndicateAugmentive
        {
            get { return indicateAugmentive; }
            set { indicateAugmentive = value; }
        }
        private Color colorReflexive;

        public Color ColorReflexive
        {
            get { return colorReflexive; }
            set { colorReflexive = value; }
        }
        private Color colorTransitive;

        public Color ColorTransitive
        {
            get { return colorTransitive; }
            set { colorTransitive = value; }
        }

        private Color colorAugmentive;

        public Color ColorAugmentive
        {
            get { return colorAugmentive; }
            set { colorAugmentive = value; }
        }
	
	
    }
}
