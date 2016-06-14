using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Meta;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Docking;
using MetaBuilder.MetaControls;
using System.Threading;

namespace MetaBuilder.UIControls.GraphingUI.Tools.HeatMap
{
    public partial class HeatMapMeasure : DockContent
    {
        private string provider;
        public string Provider
        {
            get
            {
                if (provider.Length == 0)
                    provider = Core.Variables.Instance.ClientProvider;
                return provider;
            }
            set { provider = value; }
        }

        private string _class;
        public string _Class
        {
            get { return _class; }
            set { _class = value; }
        }

        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        public HeatMapMeasure(string c)
        {
            InitializeComponent();
            _Class = c;
            //TopMost = true;
            //ShowInTaskbar = false;
            t.Interval = 250;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }

        Thread ApplyCapabilityMapThread = null;
        void t_Tick(object sender, EventArgs e)
        {
            if (ApplyCapabilityMapThread != null)
            {
                if (ApplyCapabilityMapThread.ThreadState == ThreadState.Running)
                {
                    UpdateState(".");
                    buttonApply.Enabled = checkedListBoxMeasurementObjects.Enabled = false;
                }
                else if (ApplyCapabilityMapThread.ThreadState == ThreadState.Stopped)
                {
                    this.Text = "Heat Map";
                    TabText = "Heat Map";
                    buttonApply.Enabled = checkedListBoxMeasurementObjects.Enabled = true;
                    t.Stop();
                }
                else
                {
                    //Console.WriteLine(ApplyCapabilityMapThread.ThreadState.ToString());
                }
            }
        }
        public void UpdateState(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(UpdateState), new object[] { message });
            }
            else
            {
                if (this.Text.EndsWith("..."))
                {
                    this.Text.Replace("...", "");
                    TabText.Replace("...", "");
                }
                this.Text += " " + message;
                TabText += " " + message;
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (ApplyCapabilityMapThread != null && ApplyCapabilityMapThread.ThreadState == ThreadState.Running)
                return;

            if (DockingForm.DockForm.GetCurrentGraphView() == null)
            {
                Hide();
                return;
            }
            try
            {
                t.Start();
                ApplyCapabilityMapThread = new Thread(new ThreadStart(ApplyCapabilityMap));
                ApplyCapabilityMapThread.Start();
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
        }

        private List<MetaBase> measures;
        private ColorFilter BaseColor = new ColorFilter(Color.Red);
        private void HeatMapMeasure_Load(object sender, EventArgs e)
        {
            PopulateMeasures(_Class);
            if (DialogResult == DialogResult.Cancel)
                Hide();
        }
        private void HeatMapMeasure_Click(object sender, EventArgs e)
        {
            checkedListBoxMeasurementObjects.SelectedItem = null;
        }

        public void PopulateMeasures(string className)
        {
            measures = new List<MetaBase>();
            foreach (MetaObject mObj in DataRepository.Connections[Provider].Provider.MetaObjectProvider.GetByClass(className))
            {
                MetaBase mBase = Loader.GetByID(className, mObj.pkid, mObj.Machine);
                measures.Add(mBase);
            }
            if (measures.Count == 0)
            {
                MessageBox.Show(DockingForm.DockForm, "Cannot find any " + className + "'s to apply a heat map for.", "Required Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                foreach (MetaBase mBase in measures)
                {
                    checkedListBoxMeasurementObjects.Items.Add(mBase, CheckState.Unchecked);
                }
            }
        }

        private void checkedListBoxMeasurementObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBoxMeasurementObjects.SelectedItem == null)
            {
                //applyLabelColor(BaseColor);
            }
            else
            {
                MetaBase mBase = measures[checkedListBoxMeasurementObjects.SelectedIndex];
                //find metabase in dictionary and get colorfilter to apply labels

                //applyLabelColor(metabaseColors[mBase]);
            }
        }

        public void ApplyCapabilityMap()
        {
            DockingForm.DockForm.GetCurrentGraphView().ViewController.MyView.StartTransaction();
            try
            {
                List<MetaBase> inputMeasures = new List<MetaBase>();
                foreach (int checkedIndex in checkedListBoxMeasurementObjects.CheckedIndices)
                    inputMeasures.Add(measures[checkedIndex]);

                List<IMetaNode> nodes = DockingForm.DockForm.GetCurrentGraphView().ViewController.GetIMetaNodes();
                List<Capability> topLevelCapabilities = new List<Capability>();
                //define capabilities for top level nodes
                UpdateState("Building capabilities");
                foreach (IMetaNode node in nodes)
                {
                    ////SELECTION ONLY
                    //if (DockingForm.DockForm.GetCurrentGraphView().ViewController.MyView.Selection.Count > 0)
                    //    if (!DockingForm.DockForm.GetCurrentGraphView().ViewController.MyView.Selection.Contains(node as GoObject))
                    //        continue;

                    node.MetaObject.SetWithoutChange("AverageMeasureValue", "");
                    TList<ObjectAssociation> associations = DataRepository.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(node.MetaObject.pkid, node.MetaObject.MachineName);
                    bool ischild = false;
                    foreach (ObjectAssociation association in associations)
                    {
                        if (association.Machine != "DB-TRIGGER")
                        {
                            ischild = true;
                            break;
                        }
                    }
                    if (!ischild)
                        topLevelCapabilities.Add(new Capability(node.MetaObject, inputMeasures));
                }
                UpdateState("Colouring");
                ApplyCapabilityMapColor(topLevelCapabilities);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
            DockingForm.DockForm.GetCurrentGraphView().ViewController.MyView.FinishTransaction("HeatMap");
        }

        private void ApplyCapabilityMapColor(List<Capability> capabilities)
        {
            foreach (Capability cap in capabilities)
            {
                double capTotal = cap.Total;
                Color color = Color.White;
                if (capTotal > 0)
                {
                    if (capTotal < 2)//4
                    {
                        color = labelBottom.BackColor;
                    }
                    else if (capTotal >= 4)//7
                    {
                        color = labelTop.BackColor;
                    }
                    else if (capTotal >= 2)//4
                    {
                        color = labelMid.BackColor;
                    }
                }

                //0, NaN
                if (color != Color.White && checkBoxHueAndSaturation.Checked)
                {
                    double factor = capTotal;// *2;
                    color = hueAndSaturate(color, factor);
                }
                foreach (IMetaNode iNode in DockingForm.DockForm.GetCurrentGraphView().ViewController.GetIMetaNodesBoundToMetaObject(cap.MBase))
                {
                    DockingForm.DockForm.GetCurrentGraphViewContainer().ApplyColorToNode(iNode, color);
                    if (cap.totalCount + cap.totalNumber > 0 && cap.ChildCapabilities.Count > 0)
                        iNode.MetaObject.SetWithoutChange("AverageMeasureValue", Math.Round(cap.totalNumber, 1, MidpointRounding.AwayFromZero).ToString() + " / " + cap.totalCount.ToString() + " = " + Math.Round(capTotal, 1, MidpointRounding.AwayFromZero).ToString());
                    else
                        iNode.MetaObject.SetWithoutChange("AverageMeasureValue", Math.Round(capTotal, 1, MidpointRounding.AwayFromZero).ToString());
                }

                ApplyCapabilityMapColor(cap.ChildCapabilities);
            }
        }
        private Color hueAndSaturate(Color c, double number)
        {
            double factor = number / 10;
            HSB cm = MetaColor.RGBtoHSB(c.R, c.G, c.B);
            cm.Hue += number < 2 ? number + 10 : number < 4 ? number + 5 : number;
            //cm.Brightness -= factor;
            //cm.Brightness += factor;
            RGB rgbM = MetaColor.HSBtoRGB(cm.Hue, cm.Saturation, cm.Brightness);
            c = Color.FromArgb(rgbM.Red, rgbM.Green, rgbM.Blue);
            return c;
        }

        //Thread
        public void ApplyMap()
        {
            if (measures == null || measures.Count == 0)
            {
                return;
            }

            TList<ObjectAssociation> associations = new TList<ObjectAssociation>();
            foreach (ClassAssociation cass in DataRepository.Connections[Provider].Provider.ClassAssociationProvider.GetByChildClass(_Class))
            {
                //filter by association type?
                associations.AddRange(DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetByCAid(cass.CAid));
            }

            List<IMetaNode> nodes = DockingForm.DockForm.GetCurrentGraphView().ViewController.GetIMetaNodes();
            foreach (IMetaNode iNode in nodes)
            {
                if (iNode.MetaObject.Class != "Object")
                    continue;

                iNode.MetaObject.Set("DataSourceID", "");

                //filter out specific types? ie : objectype = cabability

                ColorFilter filter = null;
                //if capability go DOWN
                List<MetaBase> artefacts = new List<MetaBase>();
                foreach (int checkedIndex in checkedListBoxMeasurementObjects.CheckedIndices)
                {
                    //is checked index a child of this node
                    MetaBase checkedItem = measures[checkedIndex];
                    associations.Filter = "ChildObjectID = " + checkedItem.pkid + " AND ChildObjectMachine = " + checkedItem.MachineName + " AND ObjectID = " + iNode.MetaObject.pkid + " AND ObjectMachine = " + iNode.MetaObject.MachineName;
                    foreach (ObjectAssociation association in associations)
                    {
                        //collect artifacts on link
                        foreach (Artifact artifact in DataRepository.Connections[Provider].Provider.ArtifactProvider.GetByObjectIDObjectMachine(association.ObjectID, association.ObjectMachine))
                        {
                            if (artifact.ChildObjectID == association.ChildObjectID && artifact.ChildObjectMachine == association.ChildObjectMachine)
                            {
                                MetaObject obj = DataRepository.Connections[Provider].Provider.MetaObjectProvider.GetBypkidMachine(artifact.ArtifactObjectID, artifact.ArtefactMachine);
                                if (obj != null)
                                    artefacts.Add(Loader.GetByID(obj.Class, obj.pkid, obj.Machine));
                            }
                        }
                        //if (filter != null && filter != metabaseColors[checkedItem])
                        //{
                        //    filter = BaseColor;
                        //    break; //should only be one association!
                        //}
                        //filter = metabaseColors[checkedItem];
                    }
                    //backwards!
                }

                if (filter != null) //is there an association
                {
                    Color color = filter.Middle;
                    if (artefacts.Count == 0)
                    {
                        color = filter.MissingInvalidOrOtherwise; //TOP?
                    }
                    else
                    {
                        bool medium = false;
                        bool low = false;
                        //based on artefact value select 'worst' value in scale high/medium/low/none
                        int total = 0;
                        int count = 0;
                        foreach (MetaBase mBase in artefacts)
                        {
                            int number = 0;
                            int.TryParse(mBase.ToString(), out number);
                            total += number;

                            count += 1;
                            if (mBase.ToString() == "High")
                            {
                                total += 5;
                                color = filter.Top;
                                break;
                            }
                            else if (mBase.ToString() == "Medium")
                            {
                                total += 3;
                                color = filter.Middle;
                                medium = true;
                            }
                            else if (mBase.ToString() == "Low" && !medium)
                            {
                                total += 1;
                                color = filter.Bottom;
                                low = true;
                            }
                            else
                            {
                                if (medium || low)
                                {
                                    continue;
                                }
                                //if no artefact or value is null then it is the WORST -> denotes missing answer/question!
                                color = filter.MissingInvalidOrOtherwise;
                            }
                        }
                        //which color selected based on number(from datatable?)
                        double average = total / count;
                        iNode.MetaObject.Set("DataSourceID", average);
                    }
                    DockingForm.DockForm.GetCurrentGraphViewContainer().ApplyColorToNode(iNode, color);
                }
                else
                {
                    //if (!(iNode is ILinkedContainer) && !(iNode is Rationale))
                    DockingForm.DockForm.GetCurrentGraphViewContainer().ApplyColorToNode(iNode, Color.White);
                }

                //if competency go UP
            }
        }

        private void checkedListBoxMeasurementObjects_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            labelTop.BackColor = Color.Red;
            labelMid.BackColor = Color.Yellow;
            labelBottom.BackColor = Color.Green;
            if (checkedListBoxMeasurementObjects.CheckedItems.Count >= 1 && e.NewValue == CheckState.Checked) //now have more than 1 checked
            {
                labelTop.BackColor = Color.Red;
                labelMid.BackColor = Color.Yellow;
                labelBottom.BackColor = Color.Green;
            }
            else if (checkedListBoxMeasurementObjects.CheckedItems.Count == 2 && e.CurrentValue == CheckState.Checked) //now have only 1 checked
            {
                //get the index of the other checkeditem and check its 'colourcode'
                int otherIndex = -1;
                foreach (int i in checkedListBoxMeasurementObjects.CheckedIndices)
                {
                    if (i != e.Index)
                    {
                        otherIndex = e.Index;
                        break;
                    }
                }
                if (otherIndex == -1)
                {

                    labelTop.BackColor = Color.Red;
                    labelMid.BackColor = Color.Yellow;
                    labelBottom.BackColor = Color.Green;
                }
                else
                {
                    checkboxItemColour(otherIndex);
                }
            }
            else
            {
                if (e.NewValue == CheckState.Checked)
                {
                    checkboxItemColour(e.Index);
                }
            }
        }

        private void checkboxItemColour(int index)
        {
            string s = checkedListBoxMeasurementObjects.Items[index].ToString().ToLower();
            if (s.Contains("strategy") || s.Contains("different"))
            {
                labelTop.BackColor = Color.FromArgb(15, 36, 62);
                labelMid.BackColor = Color.FromArgb(54, 123, 206);
                labelBottom.BackColor = Color.FromArgb(184, 204, 228);
            }
            else if (s.Contains("perf") || s.Contains("org"))
            {
                labelTop.BackColor = Color.FromArgb(99, 37, 35);
                labelMid.BackColor = Color.FromArgb(218, 152, 150);
                labelBottom.BackColor = Color.FromArgb(243, 221, 220);
            }
            else if (s.Contains("proj"))
            {
                labelTop.BackColor = Color.FromArgb(151, 71, 6);
                labelMid.BackColor = Color.FromArgb(250, 192, 146);
                labelBottom.BackColor = Color.FromArgb(253, 234, 219);
            }
            //else if (s.Contains("different"))
            //{
            //    labelTop.BackColor = Color.DarkBlue;
            //    labelMid.BackColor = Color.Blue;
            //    labelBottom.BackColor = ControlPaint.Light(Color.Blue);
            //}
        }

        private void labelTop_Click(object sender, EventArgs e)
        {
            DockingForm.DockForm.ShowColorDialog(sender);
        }

    }

    public class ColorFilter
    {
        public Color Top { get { return ControlPaint.Light(Middle); } }
        public Color Bottom { get { return ControlPaint.Dark(Middle); } }

        private Color middle;
        public Color Middle
        {
            get { return middle; }
            set { middle = value; }
        }

        public ColorFilter(Color midColor)
        {
            Middle = midColor;
        }

        public Color MissingInvalidOrOtherwise { get { return ControlPaint.Light(Top); } }
    }
}