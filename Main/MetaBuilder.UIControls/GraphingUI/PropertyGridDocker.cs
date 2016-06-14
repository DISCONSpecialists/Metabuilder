using System.ComponentModel;
using System.Windows.Forms;
using MetaBuilder.Docking;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class PropertyGridDocker : DockContent
    {

		#region Constructors (1) 

        public PropertyGridDocker()
        {
            InitializeComponent();
            // Removes the PropertyPages button
            foreach (Control control in propertyGrid1.Controls)
            {
                ToolStrip toolStrip = control as ToolStrip;
                if (toolStrip != null)
                {
                    // Found toolstrip
                    toolStrip.Items[4].Visible = false;
                    break;
                }
            }
        }

		#endregion Constructors 

		#region Properties (1) 

        public object SelectedObject
        {
            get { return propertyGrid1.SelectedObject; }
            set
            {
                if (value== null)
                {
                    UnselectableDiagram usd = new UnselectableDiagram();
                    propertyGrid1.SelectedObject = usd;
                }
                else
                {
                    propertyGrid1.SelectedObject = value;
                }
            }
        }

		#endregion Properties 

		#region Nested Classes (1) 


        public class UnselectableDiagram
        {

		#region Properties (1) 

            [Description("Instructions")]
            [CategoryAttribute("General")]
            public string Instructions
            {
                get { return "Select a shape/node on the diagram to edit the diagramming properties"; }
            }

		#endregion Properties 

        }
		#endregion Nested Classes 

    }
}