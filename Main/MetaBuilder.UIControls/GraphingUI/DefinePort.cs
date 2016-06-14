using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Controllers;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class DefinePort : Form
    {

		#region Constructors (1) 

        public DefinePort()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Methods (10) 


		// Public Methods (1) 

        public void IndicateAndBind()
        {
            if (currentShapeIndex == -1)
            {
                ports = new List<QuickPort>();
                foreach (GoObject o in view.Document)
                {
                    if (o is QuickPort)
                        ports.Add(o as QuickPort);
                    if (o is GraphNode)
                        foreach (GoObject objChild in o as GraphNode)
                        {
                            if (objChild is QuickPort)
                                ports.Add(objChild as QuickPort);
                        }
                }
                btnNextPort_Click(this, EventArgs.Empty);
            }
            if (SelectedPort != null)
            {
                Highlight();
                BindControlsToPort();
                EnableDisablePrevNextObjectButtons();
            }
        }



		// Private Methods (9) 

        private void BindControlsToPort()
        {
            txtHeight.Value = decimal.Parse(SelectedPort.Height.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            txtWidth.Value = decimal.Parse(SelectedPort.Width.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            cbAllowIncoming.Checked = SelectedPort.IsValidTo;
            cbAllowOutgoing.Checked = SelectedPort.IsValidFrom;
            cbBehaviourAllowMultipleLinks.Checked = SelectedPort.IsValidDuplicateLinks;
            cbValidSelfLinks.Checked = SelectedPort.IsValidSelfNode;
            SolidBrush sbrush = SelectedPort.Brush as SolidBrush;
            if (sbrush != null)
                uiColorButton1.SelectedColor = ((SolidBrush) SelectedPort.Brush).Color;
            else
            {
                try
                {
                    uiColorButton1.SelectedColor = ((LinearGradientBrush) SelectedPort.Brush).LinearColors[0];
                }
                catch
                {
                }
            }
            if (SelectedPort.Pen != null)
                uiColorButton2.SelectedColor = SelectedPort.Pen.Color;
            else
                comboStyle.SelectedValue = SelectedPort.Style;
        }

        private void btnActivatePortMovingTool_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapePrototyping = new ShapeDesignController(view);
            shapePrototyping.StartToolPortMover();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNextPort_Click(object sender, EventArgs e)
        {
            currentShapeIndex++;
            IndicateAndBind();
        }

        private void btnPreviousPort_Click(object sender, EventArgs e)
        {
            currentShapeIndex--;
            IndicateAndBind();
        }

        private void ChangePortDirection(LinkType linkType, int Spot, float Direction)
        {
            //// Console.WriteLine(linkType.ToString() + " Spot:" + Spot + " Direction:" + Direction);
            SelectedPort.Changing(100000);
            switch (linkType)
            {
                case LinkType.Incoming:
                    SelectedPort.ToSpot = Spot;
                    SelectedPort.IncomingLinksDirection = Direction;
                    break;
                case LinkType.Outgoing:
                    SelectedPort.FromSpot = Spot;
                    SelectedPort.OutgoingLinksDirection = Direction;
                    break;
            }
        }

        private void DefinePort_Leave(object sender, EventArgs e)
        {
        }

        private void EnableDisablePrevNextObjectButtons()
        {
            btnNextPort.Enabled = currentShapeIndex < Ports.Count - 1;
            btnPreviousPort.Enabled = currentShapeIndex > 0;
        }

        private void Highlight()
        {
            View.Selection.Clear();
            View.Selection.Add(SelectedPort);
        }


		#endregion Methods 


        #region Properties
        public QuickPort SelectedPort
        {
            get
            {
                if (Ports.Count > currentShapeIndex)
                    return Ports[currentShapeIndex];
                return null;
            }
        }

        private List<QuickPort> ports;

        public List<QuickPort> Ports
        {
            get { return ports; }
            set { ports = value; }
        }

        private GraphView view;

        public GraphView View
        {
            get { return view; }
            set { view = value; }
        }

        private int currentShapeIndex = -1;
        #endregion

        #region Incoming Direction
        private void btnIncomingUp_Click(object sender, EventArgs e)
        {
            ChangePortDirection(LinkType.Incoming, GoObject.MiddleTop, 270);
        }

        private void btnIncomingRight_Click(object sender, EventArgs e)
        {
            ChangePortDirection(LinkType.Incoming, GoObject.MiddleRight, 0);
        }

        private void btnIncomingDown_Click(object sender, EventArgs e)
        {
            ChangePortDirection(LinkType.Incoming, GoObject.MiddleBottom, 90);
        }

        private void btnIncomingLeft_Click(object sender, EventArgs e)
        {
            ChangePortDirection(LinkType.Incoming, GoObject.MiddleLeft, 180);
        }

        private void btnIncomingAuto_Click(object sender, EventArgs e)
        {
            ChangePortDirection(LinkType.Incoming, GoObject.NoSpot, -1);
        }
        #endregion

        #region Outgoing Direction
        private void btnOutgoingUp_Click(object sender, EventArgs e)
        {
            ChangePortDirection(LinkType.Outgoing, GoObject.MiddleTop, 270);
        }

        private void btnOutgoingRight_Click(object sender, EventArgs e)
        {
            ChangePortDirection(LinkType.Outgoing, GoObject.MiddleRight, 0);
        }

        private void btnOutgoingAuto_Click(object sender, EventArgs e)
        {
            ChangePortDirection(LinkType.Outgoing, GoObject.NoSpot, -1);
        }

        private void btnOutgoingDown_Click(object sender, EventArgs e)
        {
            ChangePortDirection(LinkType.Outgoing, GoObject.BottomCenter, 90);
        }

        private void btnOutgoingLeft_Click(object sender, EventArgs e)
        {
            ChangePortDirection(LinkType.Outgoing, GoObject.MiddleLeft, 180);
        }
        #endregion

        #region Checkboxes - Behaviour
        private void cbBehaviourAllowMultipleLinks_CheckedChanged(object sender, EventArgs e)
        {
            SelectedPort.IsValidDuplicateLinks = cbBehaviourAllowMultipleLinks.Checked;
        }

        private void cbValidSelfLinks_CheckedChanged(object sender, EventArgs e)
        {
            SelectedPort.IsValidSelfNode = cbValidSelfLinks.Checked;
        }

        private void cbAllowOutgoing_CheckedChanged(object sender, EventArgs e)
        {
            SelectedPort.IsValidFrom = cbAllowOutgoing.Checked;
        }

        private void cbAllowIncoming_CheckedChanged(object sender, EventArgs e)
        {
            SelectedPort.IsValidTo = cbAllowIncoming.Checked;
        }
        #endregion

        #region Appearance
        private void uiColorButton1_SelectedColorChanged(object sender, EventArgs e)
        {
            SelectedPort.Brush = new SolidBrush(uiColorButton1.SelectedColor);
        }

        private void uiColorButton2_SelectedColorChanged(object sender, EventArgs e)
        {
            SelectedPort.Pen = new Pen(new SolidBrush(uiColorButton2.SelectedColor));
        }

        private void comboStyle_SelectedValueChanged(object sender, EventArgs e)
        {
            SelectedPort.Style = (GoPortStyle) Enum.Parse(typeof (GoPortStyle), comboStyle.SelectedItem.ToString());
        }

        private void txtHeight_ValueChanged(object sender, EventArgs e)
        {
            SelectedPort.Height = float.Parse(txtHeight.Value.ToString(), System.Globalization.CultureInfo.InvariantCulture);
        }

        private void txtWidth_ValueChanged(object sender, EventArgs e)
        {
            SelectedPort.Width = float.Parse(txtWidth.Value.ToString(), System.Globalization.CultureInfo.InvariantCulture);
        }
        #endregion
    }

    public enum LinkType
    {
        Incoming,
        Outgoing
    }
}