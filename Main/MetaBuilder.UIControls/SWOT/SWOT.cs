using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Meta;
using MetaBuilder.UIControls.GraphingUI;
using ZedGraph;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.SWOT
{
    public partial class SWOT : Form
    {

        #region Fields (3)

        private bool ChangingCheckmarks;
        public string outputPath;
        private SWOTwriter swriter;

        #endregion Fields

        #region Enums (1)

        private enum GraphPurpose
        {
            Display,
            PrintSummary,
            PrintItem
        }

        #endregion Enums

        #region Constructors (1)

        public SWOT()
        {
            InitializeComponent();
            swriter = new SWOTwriter();
            swriter.Details = new List<SWOTwriter.SWOTDetail>();
            outputPath = Application.StartupPath + "\\MetaData\\Export\\";
        }

        #endregion Constructors

        #region Methods (14)

        // Private Methods (14) 

        private static void AddMiddleAxis(GraphPane myPane)
        {
            //#region Setup middle line (Y Axis 3)
            double[] xMiddle = new double[2];
            double[] yMiddle = new double[2];
            xMiddle[0] = 0;
            xMiddle[1] = 0;
            yMiddle[0] = -14.5;
            yMiddle[1] = 14.5;
            PointPairList ppYmiddle = new PointPairList(xMiddle, yMiddle);
            LineItem myCurve2 = myPane.AddCurve("", ppYmiddle, Color.Black, SymbolType.None);
            //#endregion
        }

        private void btnLoadItems_Click(object sender, EventArgs e)
        {
            SetupList();
            CreateGraph(zgc, GraphPurpose.Display);
        }

        private void btnMark_Click(object sender, EventArgs e)
        {
            ChangingCheckmarks = true;
            for (int i = 0; i < lbImplications.Items.Count; i++)
            {
                lbImplications.SetItemChecked(i, true);
            }
            ChangingCheckmarks = false;
            foreach (SWOTwriter.SWOTDetail detail in swriter.Details)
            {
                detail.IsDisplayed = true;
            }
            //ItemCheckEventArgs args = new ItemCheckEventArgs(-1, CheckState.Checked, CheckState.Checked);
            //lbImplications_ItemCheck(lbImplications, args);
            CreateGraph(zgc, GraphPurpose.Display);
            zgc.Refresh();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            CreateGraph(zgc, GraphPurpose.PrintItem);
            CreateGraph(zgc, GraphPurpose.PrintSummary);
            swriter.ExportPath = outputPath;
            swriter.DoWrite();
            MessageBox.Show(this,
                "Report was generated in HTML Format. We recommend that you copy and paste to Word for final changes.");
            Process.Start(outputPath + "SWOTAnalysis.htm");
        }

        private void btnUnmark_Click(object sender, EventArgs e)
        {
            ChangingCheckmarks = true;
            for (int i = 0; i < lbImplications.Items.Count; i++)
            {
                lbImplications.SetItemChecked(i, false);
            }
            ChangingCheckmarks = false;
            foreach (SWOTwriter.SWOTDetail detail in swriter.Details)
            {
                detail.IsDisplayed = false;
            }
            //ItemCheckEventArgs args = new ItemCheckEventArgs(-1, CheckState.Unchecked, CheckState.Unchecked);
            //lbImplications_ItemCheck(lbImplications, args);
            CreateGraph(zgc, GraphPurpose.Display);
            zgc.Refresh();
        }

        /// <summary>
        /// Create a TextLabel for each bar in the GraphPane.
        /// Call this method only after calling AxisChange()
        /// </summary>
        /// <remarks>
        /// This method will go through the bars, create a label that corresponds to the bar value,
        /// and place it on the graph depending on user preferences.  This works for horizontal or
        /// vertical bars in clusters or stacks.</remarks>
        /// <param name="pane">The GraphPane in which to place the text labels.</param>
        /// <param name="isBarCenter">true to center the labels inside the bars, false to
        /// place the labels just above the top of the bar.</param>
        /// <param name="valueFormat">The double.ToString string format to use for creating
        /// the labels
        /// </param>
        private void CreateBarLabels(GraphPane pane, bool showNumber, string valueFormat)
        {
            bool isVertical = false;
            // keep a count of the number of BarItems
            int curveIndex = 0;
            // Get a valuehandler to do some calculations for us
            ValueHandler valueHandler = new ValueHandler(pane, true);
            // Loop through each curve in the list
            Hashtable hashLabels = new Hashtable();
            for (int iC = 0; iC < pane.CurveList.Count - 1; iC++)
            {
                CurveItem curve = pane.CurveList[iC];
                // work with BarItems only
                LineItem bar = curve as LineItem;
                if (bar != null)
                {
                    IPointList points = curve.Points;
                    // Loop through each point in the BarItem
                    for (int i = 0; i < points.Count; i++)
                    {
                        // Get the high, low and base values for the current bar
                        // note that this method will automatically calculate the "effective"
                        // values if the bar is stacked
                        double baseVal, lowVal, hiVal;
                        valueHandler.GetValues(curve, i, out baseVal, out lowVal, out hiVal);
                        // Get the value that corresponds to the center of the bar base
                        // This method figures out how the bars are positioned within a cluster
                        float centerVal = (float)valueHandler.BarCenterValue(bar, bar.GetBarWidth(pane), i, baseVal, curveIndex);
                        // Create a text label -- note that we have to go back to the original point
                        // data for this, since hiVal and lowVal could be "effective" values from a bar stack
                        string key = points[i].X.ToString() + "," + points[i].Y.ToString();
                        if (hashLabels.ContainsKey(key))
                        {
                            TextObj existing = hashLabels[key] as TextObj;
                            if (showNumber)
                                existing.Text += "," + (iC + 1).ToString();
                        }
                        else
                        {
                            string barLabelText = "(" + points[i].X.ToString() + "," + points[i].Y.ToString() + ")";
                            if (showNumber)
                                //barLabelText = (iC + 1).ToString() + " ";
                                if ((curve.Tag as SWOTwriter.SWOTDetail).UniqueReference != null)
                                    barLabelText = (curve.Tag as SWOTwriter.SWOTDetail).UniqueReference + " " + barLabelText;

                            // Calculate the position of the label -- this is either the X or the Y coordinate
                            // depending on whether they are horizontal or vertical bars, respectively
                            float position;
                            position = ((float)hiVal) / 2.0f;
                            // Create the new TextObj
                            TextObj label;
                            label = new TextObj(barLabelText, (float)points[i].X + 0.2f, (float)points[i].Y);
                            //, centerVal);
                            // Configure the TextObj
                            label.Location.CoordinateFrame = CoordType.AxisXYScale;
                            label.FontSpec.Size = (showNumber) ? 12 : 16;
                            label.FontSpec.FontColor = Color.Black;
                            label.FontSpec.Angle = isVertical ? 90 : 0;
                            label.Location.AlignH = AlignH.Left;
                            label.Location.AlignV = AlignV.Center;
                            label.FontSpec.Border.IsVisible = false;
                            label.FontSpec.Fill.IsVisible = false;
                            label.ZOrder = ZOrder.A_InFront;
                            // Add the TextObj to the GraphPane
                            pane.GraphObjList.Add(label);
                            hashLabels.Add(points[i].X.ToString() + "," + points[i].Y.ToString(), label);
                        }
                    }
                }
                curveIndex++;
            }
        }

        // Build the Chart
        private void CreateGraph(ZedGraphControl zgc, GraphPurpose purpose)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            // Set the Titles
            myPane.Title.Text = "SWOT Analysis";
            myPane.XAxis.Title.Text = "Threat/Opportunity";
            myPane.YAxis.Title.Text = "Weakness/Strength";
            myPane.XAxis.MinorGrid.IsVisible = false;
            myPane.YAxis.MinorGrid.IsVisible = false;
            //myPane.YAxis.MinorTic.Color = Color.Transparent;
            myPane.YAxis.Scale.Min = -10.5;
            myPane.YAxis.Scale.Max = 10.5;
            myPane.XAxis.Scale.Min = -10.5;
            myPane.XAxis.Scale.Max = 10.5;
            myPane.YAxis.Cross = 0.0;
            myPane.YAxis.Title.IsTitleAtCross = false;
            myPane.YAxis.CrossAuto = true;
            myPane.XAxis.CrossAuto = true;
            Margin margin = new Margin();
            margin.Right = 120;
            margin.Bottom = 120;
            myPane.Margin = margin;
            myPane.Legend.IsVisible = false;
            myPane.GraphObjList.Clear();
            myPane.CurveList.Clear();
            int itemCount = 1;
            foreach (SWOTwriter.SWOTDetail detail in swriter.Details)
            {
                if (detail.IsDisplayed)
                {
                    if (purpose == GraphPurpose.PrintItem)
                    {
                        myPane.CurveList.Clear();
                        myPane.GraphObjList.Clear();
                    }
                    double[] x = new double[1];
                    double[] y = new double[1];
                    x[0] = detail.OpportunityThreatRating;
                    y[0] = detail.StrengthWeaknessRating;
                    PointPairList pp = new PointPairList(x, y);
                    // Generate a red curve with diamond symbols, and "Gas Data" in the legend
                    LineItem myCurve = myPane.AddCurve("", pp, Color.Red, SymbolType.Diamond);
                    myCurve.Tag = detail;
                    myCurve.Symbol.Size = 10;
                    // Set up a red-blue color gradient to be used for the fill
                    myCurve.Symbol.Fill = new Fill(Color.Red, Color.Blue);
                    // Turn off the symbol borders
                    myCurve.Symbol.Border.IsVisible = false;
                    // Instruct ZedGraph to fill the symbols by selecting a color out of the
                    // red-blue gradient based on the Z value.  A value of 19 or less will be red,
                    // a value of 34 or more will be blue, and values in between will be a
                    // linearly apportioned color between red and blue.
                    myCurve.Symbol.Fill.Type = FillType.GradientByX;
                    myCurve.Symbol.Fill.RangeMin = -20;
                    myCurve.Symbol.Fill.RangeMax = 20;
                    // Turn off the line, so the curve will by symbols only
                    myCurve.Line.IsVisible = false;
                    if (purpose == GraphPurpose.PrintItem)
                    {
                        string name = (detail.ImplicationName.Length > 100) ? detail.ImplicationName.Substring(0, 100) : detail.ImplicationName;
                        myPane.Title.Text = "SWOT Analysis: Implication " + itemCount.ToString();
                        AddMiddleAxis(myPane);
                        CreateBarLabels(myPane, false, "N0");
                        // Do the save
                        zgc.AxisChange();
                        IncreaseSizeAndSave(zgc, outputPath + "SWOTItem" + itemCount.ToString() + ".png");
                        itemCount++;
                    }
                }
            }
            // Setup the legend (Remove)
            //myPane.Legend.Position = LegendPos.BottomCenter;
            // Create TextObj's to provide labels for each bar
            if (purpose == GraphPurpose.PrintSummary)
            {
                AddMiddleAxis(myPane);
                CreateBarLabels(myPane, true, "N0");
                zgc.AxisChange();
                string outputFilename = outputPath + "SWOTSummary.png";
                IncreaseSizeAndSave(zgc, outputFilename);
            }
            if (purpose == GraphPurpose.Display)
            {
                AddMiddleAxis(myPane);
                CreateBarLabels(myPane, true, "N0");
            }
            zgc.AxisChange();
        }

        // Respond to form 'Load' event
        private void Form1_Load(object sender, EventArgs e)
        {
            SetupList();
            // Setup the graph
            CreateGraph(zgc, GraphPurpose.Display);
            // Size the control to fill the form with a margin
            // SetSize();
        }

        // Respond to the form 'Resize' event
        private void Form1_Resize(object sender, EventArgs e)
        {
            //   SetSize();
        }

        private static void IncreaseSizeAndSave(ZedGraphControl zgc, string outputFilename)
        {
            Size oldSize = zgc.Size;
            Size newSize = new Size((int)(oldSize.Width * 1.4d), (int)(oldSize.Height * 1.4d));
            zgc.Size = newSize;
            zgc.Save(outputFilename);
            zgc.Size = oldSize;
        }

        private void lbImplications_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                CheckState state = e.NewValue;
                SWOTwriter.SWOTDetail detail = lbImplications.Items[e.Index] as SWOTwriter.SWOTDetail;
                detail.IsDisplayed = (state == CheckState.Checked);
            }
            catch
            {
            }
            if (!ChangingCheckmarks)
            {
                CreateGraph(zgc, GraphPurpose.Display);
                zgc.Refresh();
            }
        }

        /// <summary>
        /// Normalises the random number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        private int NormaliseRandomNumber(int number)
        {
            if (number != 0 && number.ToString().Length > 1)
            {
                string no = number.ToString();
                string firstChar = no.Substring(0, 1);
                string firstTwoChar = no.Substring(1, 1);
                int first = int.Parse(firstChar);
                int second = int.Parse(firstTwoChar);
                return first - second;
            }
            return number;
        }

        // SetSize() is separate from Resize() so we can 
        // call it independently from the Form1_Load() method
        // This leaves a 10 px margin around the outside of the control
        // Customize this to fit your needs
        private void SetSize()
        {
            zgc.Location = new Point(10, 10);
            // Leave a small margin around the outside of the control
            zgc.Size = new Size(ClientRectangle.Width - 20, ClientRectangle.Height - 20);
        }

        private void SetupList()
        {
            lbImplications.Items.Clear();
            ObjectFinder ofinder = new ObjectFinder(false);
            ofinder.IncludeStatusCombo = true;
            ofinder.ExcludeStatuses.Add(VCStatusList.Obsolete);
            ofinder.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);

            ofinder.LimitToClass = "Implication";
            ofinder.AllowMultipleSelection = true;
            DialogResult res = ofinder.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                if (ofinder.SelectedObjectsList.Count == 0)
                {
                    Close();
                    return;
                }
                List<MetaBase> objs = ofinder.SelectedObjectsList;
                swriter.Details = new List<SWOTwriter.SWOTDetail>();
                int i = 0;
                foreach (MetaBase mb in objs)
                {
                    try
                    {
                        i++;
                        SWOTwriter.SWOTDetail detail = new SWOTwriter.SWOTDetail();
                        detail.IsDisplayed = true;
                        detail.Number = i;
                        detail.ImplicationName = mb.Get("Name").ToString();

                        if (mb.Get("UniqueReference") != null)
                            detail.UniqueReference = mb.Get("UniqueReference").ToString();

                        try
                        {
                            detail.OpportunityThreatRating = int.Parse(mb.Get("ExternalInfluenceIndicator").ToString());
                        }
                        catch
                        {
                        }
                        try
                        {
                            detail.StrengthWeaknessRating = int.Parse(mb.Get("InternalCapabilityIndicator").ToString());
                        }
                        catch
                        {
                        }
                        swriter.Details.Add(detail);
                    }
                    catch
                    {
                    }
                }
                foreach (SWOTwriter.SWOTDetail detail in swriter.Details)
                {
                    lbImplications.Items.Add(detail, true);
                }
            }
            else
            {
                Close();
            }
        }

        #endregion Methods

    }
}