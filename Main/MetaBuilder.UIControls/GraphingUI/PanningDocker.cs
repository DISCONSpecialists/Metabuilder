using System.Windows.Forms;
using MetaBuilder.Docking;
using MetaBuilder.Graphing.Containers;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class PanningDocker : DockContent
    {

		#region Constructors (1) 

        public PanningDocker()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Methods (1) 

		// Public Methods (1) 

        public void SetCoordinates(MouseEventArgs evtArgs)
        {
            // lblXValue.Text = evtArgs.X.ToString();
            //lblYValue.Text = evtArgs.Y.ToString();
        }

		#endregion Methods 

    }
}