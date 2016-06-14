using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Controllers;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class PortFormatter : Form
    {

		#region Fields (6) 

        private List<GoObject> collectedShapes;
        private int CurrentIndex;
        private GoPort CurrentPort;
        private int currentShapeIndex = -1;
        private bool SkipEvents;
        private GoRectangle spotRectangle;

		#endregion Fields 

		#region Constructors (1) 

        public PortFormatter()
        {
            InitializeComponent();
            // Add Port Styles
            BindPortStyles();
            if (Ports.Count > 0)
            {
                SkipEvents = true;
                CurrentPort = Ports[CurrentIndex];
                BindProperties();
                SkipEvents = false;
            }
            else
            {
                MessageBox.Show(this,"There are no ports on the diagram!");
            }
        }

		#endregion Constructors 

		#region Properties (2) 

        private List<GoPort> Ports
        {
            get
            {
                List<GoPort> portCollection = new List<GoPort>();
                foreach (GoObject obj in view.Document)
                {
                    if (obj is GoPort)
                    {
                        portCollection.Add(obj as GoPort);
                    }
                }
                return portCollection;
            }
        }

        private GraphView view
        {
            get { return DockingForm.DockForm.GetCurrentGraphView(); }
        }

		#endregion Properties 

		#region Methods (18) 


		// Private Methods (18) 

        private void BindPortStyles()
        {
            BindingList<PortStyle> portStyle = new BindingList<PortStyle>();
            portStyle.Add(new PortStyle(GoPortStyle.Object, "Object"));
            portStyle.Add(new PortStyle(GoPortStyle.Ellipse, "Ellipse"));
            portStyle.Add(new PortStyle(GoPortStyle.Triangle, "Triangle"));
            portStyle.Add(new PortStyle(GoPortStyle.Rectangle, "Rectangle"));
            portStyle.Add(new PortStyle(GoPortStyle.Diamond, "Diamond"));
            portStyle.Add(new PortStyle(GoPortStyle.Plus, "Plus"));
            portStyle.Add(new PortStyle(GoPortStyle.Times, "Times"));
            portStyle.Add(new PortStyle(GoPortStyle.PlusTimes, "PlusTimes"));
            comboStyle.ValueMember = "Enumeration";
            comboStyle.DisplayMember = "Display";
            comboStyle.DataSource = portStyle;
        }

        private void BindProperties()
        {
            SkipEvents = true;
            view.Selection.Clear();
            view.Selection.Add(CurrentPort);
            txtHeight.Value = decimal.Parse(CurrentPort.Height.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            txtWidth.Value = decimal.Parse(CurrentPort.Width.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            comboStyle.SelectedValue = CurrentPort.Style;
            cbIn.Checked = CurrentPort.IsValidTo;
            cbIn.Checked = CurrentPort.IsValidFrom;
            colorButtonLine.SelectedColor = CurrentPort.Pen.Color;
            SolidBrush sbrush = CurrentPort.Brush as SolidBrush;
            if (sbrush != null)
                colorButtonFill.SelectedColor = ((SolidBrush) CurrentPort.Brush).Color;
            SkipEvents = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (Ports.Count > 0)
            {
                if (CurrentIndex + 1 < Ports.Count)
                {
                    CurrentIndex++;
                    CurrentPort = Ports[CurrentIndex];
                    BindProperties();
                }
            }
        }

        private void btnNextObject_Click(object sender, EventArgs e)
        {
            currentShapeIndex++;
            DrawTargetRectangle();
            EnableDisablePrevNextObjectButtons();
        }

        private void btnPortMover_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapePrototyping = new ShapeDesignController(view);
            shapePrototyping.StartToolPortMover();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (Ports.Count > 0)
            {
                if (CurrentIndex > 0)
                {
                    CurrentIndex--;
                    CurrentPort = Ports[CurrentIndex];
                    BindProperties();
                }
            }
        }

        private void btnPrevObject_Click(object sender, EventArgs e)
        {
            currentShapeIndex--;
            DrawTargetRectangle();
            EnableDisablePrevNextObjectButtons();
        }

        private void cbIn_CheckedChanged(object sender, EventArgs e)
        {
            CurrentPort.IsValidTo = cbIn.Checked;
        }

        private void cbOut_CheckedChanged(object sender, EventArgs e)
        {
            CurrentPort.IsValidFrom = cbOut.Checked;
        }

        private void colorButtonFill_SelectedColorChanged(object sender, EventArgs e)
        {
            SetProperties();
        }

        private void colorButtonLine_SelectedColorChanged(object sender, EventArgs e)
        {
            SetProperties();
        }

        private void comboStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetProperties();
        }

        private void DrawTargetRectangle()
        {
            if (spotRectangle != null)
                spotRectangle.Remove();
            spotRectangle = new GoRectangle();
            spotRectangle.Brush = Brushes.Transparent;
            spotRectangle.Pen = new Pen(Brushes.Red);
            spotRectangle.Pen.Width = 5;
            spotRectangle.Pen.DashStyle = DashStyle.Dash;
            spotRectangle.Bounds = collectedShapes[currentShapeIndex].Bounds;
            //CurrentPort.Remove();
            CurrentPort.PortObject = collectedShapes[currentShapeIndex];
            CurrentPort.Bounds = collectedShapes[currentShapeIndex].Bounds;
            //collectedShapes[currentShapeIndex].Parent.InsertAfter(collectedShapes[currentShapeIndex],CurrentPort);
            view.Document.Add(spotRectangle);
        }

        private void EnableDisablePrevNextObjectButtons()
        {
            btnNextObject.Enabled = currentShapeIndex < collectedShapes.Count - 1;
            btnPrevObject.Enabled = currentShapeIndex > 0;
        }

        private void PortFormatter_Leave(object sender, EventArgs e)
        {
            if (spotRectangle != null)
                spotRectangle.Remove();
        }

        private void SetProperties()
        {
            if (!SkipEvents)
                if (CurrentPort != null)
                {
                    CurrentPort.Width = float.Parse(txtWidth.Value.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    CurrentPort.Height = float.Parse(txtHeight.Value.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    CurrentPort.Style = (GoPortStyle) comboStyle.SelectedValue;
                    if (CurrentPort.Style == GoPortStyle.Object)
                    {
                        btnNextObject.Visible = true;
                        btnPrevObject.Visible = true;
                        lblSelectObject.Visible = true;
                        // find the object immediately below the port
                        collectedShapes = new List<GoObject>();
                        foreach (GoObject o in view.Document)
                        {
                            if (o is GoPort)
                            {
                                // do nothing
                            }
                            else
                            {
                                if (o is GraphNode)
                                {
                                    foreach (GoObject ochild in (o as GraphNode))
                                    {
                                        collectedShapes.Add(ochild);
                                    }
                                }
                                collectedShapes.Add(o);
                            }
                        }
                        currentShapeIndex = 0;
                        EnableDisablePrevNextObjectButtons();
                        DrawTargetRectangle();
                    }
                    else
                    {
                        btnNextObject.Visible = false;
                        btnPrevObject.Visible = false;
                        lblSelectObject.Visible = false;
                    }
                    if (colorButtonLine.SelectedColor != null)
                        CurrentPort.Pen = new Pen(colorButtonLine.SelectedColor);
                    if (colorButtonFill.SelectedColor != null)
                        CurrentPort.Brush = new SolidBrush(colorButtonFill.SelectedColor);
                }
        }

        private void txtHeight_ValueChanged(object sender, EventArgs e)
        {
            SetProperties();
        }

        private void txtWidth_ValueChanged(object sender, EventArgs e)
        {
            SetProperties();
        }


		#endregion Methods 

		#region Nested Classes (1) 


        private class PortStyle
        {

		#region Fields (2) 

            private string display;
            private Enum enumeration;

		#endregion Fields 

		#region Constructors (1) 

            public PortStyle(Enum e, string Display)
            {
                Enumeration = e;
                this.Display = Display;
            }

		#endregion Constructors 

		#region Properties (2) 

            public string Display
            {
                get { return display; }
                set { display = value; }
            }

            public Enum Enumeration
            {
                get { return enumeration; }
                set { enumeration = value; }
            }

		#endregion Properties 

        }
		#endregion Nested Classes 

    }
}