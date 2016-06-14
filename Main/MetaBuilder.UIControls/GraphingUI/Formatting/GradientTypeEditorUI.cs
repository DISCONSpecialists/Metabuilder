#region - Terms of Usage -
/*
 * Copyright 2006 Sameera Perera
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 */
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.Graphing.Formatting;

namespace MetaBuilder.UIControls.GraphingUI.Formatting
{
    public partial class GradientTypeEditorUI : UserControl
    {

		#region Fields (1) 

        private Marker marker;

		#endregion Fields 

		#region Constructors (1) 

        public GradientTypeEditorUI()
        {
            InitializeComponent();
            colorEditor.EditedType = typeof (Color);
            colorEditor.Value = Color.White;
            Font editorFont = new Font(Font.FontFamily, 7.0f);
            colorEditor.Font = editorFont;
        }

		#endregion Constructors 

		#region Properties (2) 

        [Browsable(false), DefaultValue(null)]
        public Gradient Gradient
        {
            get { return gradientBuilder.Gradient; }
            set { gradientBuilder.Gradient = value; }
        }

        // <summary>
        /// Gets or sets whether the GradientChanged event is fired when a marker
        /// is dragged.
        /// </summary>
        [DefaultValue(false)]
        public bool SilentMarkers
        {
            get { return gradientBuilder.SilentMarkers; }
            set { gradientBuilder.SilentMarkers = value; }
        }

		#endregion Properties 

		#region Delegates and Events (1) 


		// Events (1) 

        public event EventHandler GradientChanged;


		#endregion Delegates and Events 

		#region Methods (5) 


		// Public Methods (1) 

        public Marker GetMarker(int index)
        {
            return gradientBuilder.GetMarker(index);
        }



		// Private Methods (4) 

        private void colorMixerFieldValueChanged(object sender, EventArgs e)
        {
            if (marker != null)
                UpdateMarkerColor();
        }

        private void gradientBuilder_GradientChanged(object sender, EventArgs e)
        {
            if (GradientChanged != null)
                GradientChanged(this, EventArgs.Empty);
        }

        private void gradientBuilder_MarkerSelected(object sender, EventArgs e)
        {
            Marker tempMarker = sender as Marker;
            marker = null; // Freeze all other event handlers
            if (tempMarker != null)
            {
                colorEditor.Value = tempMarker.Color;
                opacityBox.Value = Convert.ToDecimal(tempMarker.Color.A*(100.0f/255));
                grpEditor.Enabled = true;
            }
            else
                grpEditor.Enabled = false;
            marker = tempMarker;
        }

        private void UpdateMarkerColor()
        {
            Color color = (Color) colorEditor.Value;
            int a = Convert.ToInt32((float) opacityBox.Value*(255.0f/100));
            marker.Color = Color.FromArgb(a, color);
        }


		#endregion Methods 

    }
}