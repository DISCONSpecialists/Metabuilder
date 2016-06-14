using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.CommonControls.Tree;
using MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation;
using Northwoods.Go;
using MetaBuilder.Docking;
namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.UI
{
    public partial class ValidationResultForm : DockContent
    {

        #region Constructors (1)

        public ValidationResultForm()
        {
            InitializeComponent();
        }

        #endregion Constructors

        void btnFEBTSID_Click(object sender, EventArgs e)
        {
            FEBT(false);
        }
        void btnFEBTVisual_Click(object sender, EventArgs e)
        {
            FEBT(true);
        }

        List<string> usedColours;

        Random r = new Random();
        private Color GetNewColour()
        {
            byte red = (byte)r.Next(0, 255);
            byte green = (byte)r.Next(0, 255);
            byte blue = (byte)r.Next(0, 255);
            string colorKey = red.ToString() + green.ToString() + blue.ToString();
            if (usedColours.Contains(colorKey))
                return GetNewColour();

            usedColours.Add(colorKey);
            return Color.FromArgb(255, red, green, blue);
        }
        private List<Color> GetListOfColors(int count)
        {
            usedColours = new List<string>();
            List<Color> listColors = new List<Color>();

            for (int i = 0; i < count; i++)
            {
                listColors.Add(GetNewColour());
            }

            //listColors.Add(Color.Aqua);
            //listColors.Add(Color.Azure);
            //listColors.Add(Color.Beige);
            //listColors.Add(Color.Bisque);
            //listColors.Add(Color.Blue);
            //listColors.Add(Color.BlanchedAlmond);
            //listColors.Add(Color.Brown);
            //listColors.Add(Color.CadetBlue);
            //listColors.Add(Color.Chartreuse);
            //listColors.Add(Color.Coral);
            //listColors.Add(Color.CornflowerBlue);
            //listColors.Add(Color.Crimson);
            //listColors.Add(Color.DarkBlue);
            //listColors.Add(Color.DarkGray);
            //listColors.Add(Color.DarkOliveGreen);
            //listColors.Add(Color.DarkOrange);
            //listColors.Add(Color.DarkSlateGray);
            //listColors.Add(Color.DeepPink);
            //listColors.Add(Color.IndianRed);
            //listColors.Add(Color.Lavender);
            //listColors.Add(Color.OliveDrab);
            //listColors.Add(Color.LightGray);
            //listColors.Add(Color.Peru);
            //listColors.Add(Color.Violet);
            //listColors.Add(Color.Tomato);
            //listColors.Add(Color.Yellow);
            //listColors.Add(Color.SeaGreen);
            //listColors.Add(Color.Thistle);
            //listColors.Add(Color.MediumOrchid);
            //listColors.Add(Color.LightSteelBlue);
            //listColors.Add(Color.DodgerBlue);
            //listColors.Add(Color.DarkSlateBlue);
            //listColors.Add(Color.ForestGreen);
            //listColors.Add(Color.NavajoWhite);
            //listColors.Add(Color.MistyRose);
            //listColors.Add(Color.SaddleBrown);

            //listColors.Add(Color.BlueViolet);
            //listColors.Add(Color.Orange);
            //listColors.Add(Color.BurlyWood);
            //listColors.Add(Color.Chocolate);
            //listColors.Add(Color.DarkGoldenrod);
            //listColors.Add(Color.DeepSkyBlue);
            //listColors.Add(Color.Gold);
            //listColors.Add(Color.ForestGreen);
            //listColors.Add(Color.Green);
            //listColors.Add(Color.AliceBlue);
            //listColors.Add(Color.Blue);
            //listColors.Add(Color.Purple);
            //listColors.Add(Color.Pink);
            //listColors.Add(Color.SpringGreen);
            //listColors.Add(Color.YellowGreen);
            //listColors.Add(Color.Turquoise);

            //listColors.Add(Color.Wheat);
            //listColors.Add(Color.White);
            //listColors.Add(Color.WhiteSmoke);
            //listColors.Add(Color.Tan);
            //listColors.Add(Color.Sienna);
            //listColors.Add(Color.SandyBrown);
            //listColors.Add(Color.Plum);
            //listColors.Add(Color.Silver);
            //listColors.Add(Color.PeachPuff);
            //listColors.Add(Color.PowderBlue);

            return listColors;
        }

        public InferenceRulesOptions GetOptions()
        {
            InferenceRulesOptions options = new InferenceRulesOptions();
            options.ColorAugmentive = pboxAugmentive.BackColor;
            options.ColorTransitive = pboxTransitive.BackColor;
            options.ColorReflexive = pictureBox1.BackColor;
            options.IndicateAugmentive = cbAug.Checked;
            options.IndicateTransitive = cbTrans.Checked;
            options.IndicateReflexive = cbIndicateReflexive.Checked;
            return options;
        }

        private void FEBT(bool visual)
        {
            model = new TreeModel();

            MyEngine.Diagram.StartTransaction();

            MyEngine = new DataModel.Engine(myEngine.MyView, MyEngine.Diagram);
            MyEngine.Options = GetOptions();
            MyEngine.ApplyInferenceRules(); //Options
            //CompileResults(MyEngine.inferenceRules);

#if DEBUG
            //MyEngine.Diagram.UndoManager.FinishTransaction("Clustering");
            //return;
#endif

            FEBT.PatternFinder pf = new MetaBuilder.UIControls.GraphingUI.Tools.DataModel.FEBT.PatternFinder(MyEngine, MyEngine.MyView);
            if (visual)
            {
                #region Color Nodes by Cluster
                List<Color> colors = GetListOfColors((pf.FoundClusters.Count + 2));
                for (int i = 0; i < pf.FoundClusters.Count; i++)
                {
                    FEBT.Cluster c = pf.FoundClusters[i];
                    GoSelection sel = new GoSelection(null);
                    foreach (ADDNode node in c.Nodes)
                    {
                        foreach (GoObject gobject in node.MyGoObjects)
                        {
                            if (gobject is MetaBuilder.Graphing.Shapes.GraphNode)
                            {
                                MetaBuilder.Graphing.Shapes.GraphNode n = gobject as MetaBuilder.Graphing.Shapes.GraphNode;
                                sel.Add(n);
                            }
                        }
                    }

                    MetaBuilder.Graphing.Formatting.FormattingManipulator fman = new MetaBuilder.Graphing.Formatting.FormattingManipulator(sel);

                    fman.NewSettings.IsGradient = true;

                    fman.NewSettings.GradientStartColour = colors[i];
                    fman.NewSettings.GradientEndColour = Color.White;
                    fman.NewSettings.GradientType = MetaBuilder.Graphing.Formatting.GradientType.Horizontal;

                    fman.NewSettings.TextColour = Color.Black;
                    fman.NewSettings.Font = null;

                    fman.ApplyFormatSettings();

                    //GoCollection col = new GoCollection();
                }

                #endregion
            }
            else
            {
                FEBT.SIDBuilder sidBuilder = new MetaBuilder.UIControls.GraphingUI.Tools.DataModel.FEBT.SIDBuilder();
                //sidBuilder.BuildSID(pf);
                sidBuilder.createDiagram(pf);
            }
            MyEngine.Diagram.FinishTransaction("Clustering Indicators");
        }

        //private Engine2015 myEngine;
        //public Engine2015 MyEngine
        //{
        //    get { return myEngine; }
        //    set { myEngine = value; }
        //}

        private Engine myEngine;
        public Engine MyEngine
        {
            get { return myEngine; }
            set { myEngine = value; }
        }

        #region Methods (10)

        // Public Methods (1) 

        // Private Methods (9) 

        private TreeModel model;

        bool errorsFound = false;

        private void CompileResults(List<BaseRule> items)
        {
            model = new TreeModel();
            this.treeViewAdv1.Model = model;

            bool warnings = false;
            bool errorsFoundBefore = errorsFound;
            errorsFound = false;
            MyValidationTreeNode gotoItem = null;
            foreach (BaseRule rule in items)
            {
                MyValidationTreeNode item = new MyValidationTreeNode();
                item.Tag = rule;
                if (rule.AllItems != null && rule.AllItems.Count > 0)
                    model.Nodes.Add(item);
                else
                    continue;

                //foreach (ValidatedItem valitem in rule.AllItems)
                foreach (ValidatedItem valitem in rule.AllItems)
                {
                    MyValidationTreeNode subitem = new MyValidationTreeNode();
                    subitem.Tag = valitem;
                    item.Nodes.Add(subitem);
                }
                if (!errorsFound)
                {
                    gotoItem = item;
                    errorsFound = (rule.OverallValidationResult == ValidationResult.Error);
                }
                else
                    if (gotoItem != null)
                        gotoItem = item;

                if (!warnings)
                {
                    gotoItem = item;
                    warnings = (rule.OverallValidationResult == ValidationResult.Warning);
                }
                else
                    if (gotoItem != null)
                        gotoItem = item;
            }

            if (errorsFound)// && !errorsFoundBefore)
            {
                MessageBox.Show(this,"Errors are indicated in the Validation Result window", "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //btnGo.Enabled = true;
            }
            else if (warnings)
            {
                MessageBox.Show(this,"Warnings are indicated in the Validation Result window", "Validation unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //btnGo.Enabled = true;
            }
        }
        private void CompileResults(List<BaseRule2015> items)
        {
            model = new TreeModel();
            this.treeViewAdv1.Model = model;

            bool warnings = false;
            bool errorsFoundBefore = errorsFound;
            errorsFound = false;
            MyValidationTreeNode gotoItem = null;
            foreach (BaseRule2015 rule in items)
            {
                MyValidationTreeNode item = new MyValidationTreeNode();
                item.Tag = rule;
                if (rule.AllItems != null && rule.AllItems.Count > 0)
                    model.Nodes.Add(item);
                else
                    continue;

                //foreach (ValidatedItem valitem in rule.AllItems)
                foreach (ValidationItem valitem in rule.AllItems)
                {
                    MyValidationTreeNode subitem = new MyValidationTreeNode();
                    subitem.Tag = valitem;
                    item.Nodes.Add(subitem);
                }
                if (!errorsFound)
                {
                    gotoItem = item;
                    errorsFound = (rule.OverallValidationResult == ValidationResult.Error);
                }
                else
                    if (gotoItem != null)
                        gotoItem = item;

                if (!warnings)
                {
                    gotoItem = item;
                    warnings = (rule.OverallValidationResult == ValidationResult.Warning);
                }
                else
                    if (gotoItem != null)
                        gotoItem = item;
            }

            if (errorsFound)// && !errorsFoundBefore)
            {
                MessageBox.Show(this,"Errors are indicated in the Validation Result window", "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //btnGo.Enabled = true;
            }
            else if (warnings)
            {
                MessageBox.Show(this,"Warnings are indicated in the Validation Result window", "Validation unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //btnGo.Enabled = true;
            }
        }
        private void CompileResults(List<BaseInferenceRule> items)
        {
            if (items == null || items.Count == 0)
                return;

            //model = new TreeModel();
            this.treeViewAdv1.Model = model;

            bool warnings = false;
            bool errorsFoundBefore = errorsFound;
            errorsFound = false;
            MyValidationTreeNode gotoItem = null;
            foreach (BaseInferenceRule rule in items)
            {
                MyValidationTreeNode item = new MyValidationTreeNode();
                item.Tag = rule;
                if (rule.AllItems != null && rule.AllItems.Count > 0)
                    model.Nodes.Add(item);
                else
                    continue;

                //foreach (ValidatedItem valitem in rule.AllItems)
                foreach (ValidationItem valitem in rule.AllItems)
                {
                    MyValidationTreeNode subitem = new MyValidationTreeNode();
                    subitem.Tag = valitem;
                    item.Nodes.Add(subitem);
                }
                if (!errorsFound)
                {
                    gotoItem = item;
                    errorsFound = (rule.OverallValidationResult == ValidationResult.Error);
                }
                else
                    if (gotoItem != null)
                        gotoItem = item;

                if (!warnings)
                {
                    gotoItem = item;
                    warnings = (rule.OverallValidationResult == ValidationResult.Warning);
                }
                else
                    if (gotoItem != null)
                        gotoItem = item;
            }
            if (errorsFound)// && !errorsFoundBefore)
            {
                MessageBox.Show(this,"Errors are indicated in the Validation Result window", "Inference Rule Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //btnGo.Enabled = true;
            }
            else if (warnings)
            {
                MessageBox.Show(this,"Warnings are indicated in the Validation Result window", "Inference Rule Validation unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //btnGo.Enabled = true;
            }
        }

        public void ShowResults()
        {
            //DockingForm.DockForm.ValidationResultForm.Show();
            DockingForm.DockForm.ValidationResultForm.Show(DockingForm.DockForm.dockPanel1, DockState.DockBottom);

            if (MyEngine == null)
            {
                //btnGo.Enabled = true;
                return;
            }

            //this.treeListView2.Items.Clear();

            CompileResults(MyEngine.validationRules);
            //CompileResults(MyEngine.inferenceRules);
            //foreach (Validation.BaseRule rule in MyEngine.validationRules)
            //foreach (BaseRule2015 rule in MyEngine.validationRules)
            //{
            //    MyValidationTreeNode item = new MyValidationTreeNode();
            //    item.Tag = rule;
            //    model.Nodes.Add(item);

            //    //foreach (ValidatedItem valitem in rule.AllItems)
            //    foreach (ValidationItem valitem in rule.AllItems)
            //    {
            //        MyValidationTreeNode subitem = new MyValidationTreeNode();
            //        subitem.Tag = valitem;
            //        item.Nodes.Add(subitem);
            //    }
            //    if (!errorsFound)
            //    {
            //        gotoItem = item;
            //        errorsFound = (rule.OverallValidationResult == ValidationResult.Error);
            //    }
            //    else
            //        if (gotoItem != null)
            //            gotoItem = item;

            //    if (!warnings)
            //    {
            //        gotoItem = item;
            //        warnings = (rule.OverallValidationResult == ValidationResult.Warning);
            //    }
            //    else
            //        if (gotoItem != null)
            //            gotoItem = item;
            //}

            //if (errorsFound)// && !errorsFoundBefore)
            //{
            //    MessageBox.Show(this,"Errors are indicated in the Validation Result window", "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    //btnGo.Enabled = true;
            //}
            //else if (warnings)
            //{
            //    MessageBox.Show(this,"Warnings are indicated in the Validation Result window", "Validation unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    //btnGo.Enabled = true;
            //}
            //else
            //{
            //    //MessageBox.Show(this,"Validation successful – no errors found. Refer to the Validation log for criteria tested", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    //btnGo.Enabled = true;
            //}

            //if (gotoItem != null)
            //{
            //}
        }

        public class MyValidationTreeNode : MetaBuilder.CommonControls.Tree.Node
        {
            public string DisplayName
            {
                get
                {
                    if (Tag is BaseRule)
                    {
                        BaseRule r = Tag as BaseRule;
                        return r.Name;
                    }
                    else if (Tag is BaseRule2015)
                    {
                        BaseRule2015 r = Tag as BaseRule2015;
                        return r.Name;
                    }
                    else if (Tag is BaseInferenceRule)
                    {
                        BaseInferenceRule r = Tag as BaseInferenceRule;
                        return r.Name;
                    }
                    else
                    {
                        ValidatedItem vitem = tag as ValidatedItem;
                        //ValidationItem vitem = tag as ValidationItem;
                        return vitem.Name;
                    }
                }
            }
            public string DisplayResult
            {
                get
                {
                    if (Tag is BaseRule)
                    {
                        BaseRule r = Tag as BaseRule;
                        return r.Value;
                    }
                    else if (Tag is BaseRule2015)
                    {
                        BaseRule2015 r = Tag as BaseRule2015;
                        return r.Value;
                    }
                    else if (Tag is BaseInferenceRule)
                    {
                        BaseInferenceRule r = Tag as BaseInferenceRule;
                        return r.Value;
                    }
                    else
                    {
                        ValidatedItem vitem = tag as ValidatedItem;
                        //ValidationItem vitem = tag as ValidationItem;
                        return vitem.Result.ToString();
                    }
                }
            }

            private object tag;
            public object Tag
            {
                get { return tag; }
                set { tag = value; }
            }
        }

        #endregion Methods

        private void treeViewAdv1_NodeMouseDoubleClick(object sender, TreeNodeAdvMouseEventArgs e)
        {
            MyValidationTreeNode myvalnode = e.Node.Tag as MyValidationTreeNode;
            if (myvalnode.Tag is ValidatedItem)
            {
                ValidatedItem item = myvalnode.Tag as ValidatedItem;
                GoObject diagramObject = item.MyNode.MyGoObjects[0];
                DockingForm.DockForm.GetCurrentGraphView().ScrollRectangleToVisible(diagramObject.Bounds);
                DockingForm.DockForm.GetCurrentGraphView().Selection.Clear();
                DockingForm.DockForm.GetCurrentGraphView().Selection.Add(diagramObject);
            }
            else if (myvalnode.Tag is ValidationItem)
            {
                ValidationItem item = myvalnode.Tag as ValidationItem;
                GoObject diagramObject = item.MyGoObject;
                DockingForm.DockForm.GetCurrentGraphView().ScrollRectangleToVisible(diagramObject.Bounds);
                DockingForm.DockForm.GetCurrentGraphView().Selection.Clear();
                DockingForm.DockForm.GetCurrentGraphView().Selection.Add(diagramObject);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            MyEngine.Diagram.UndoManager.Undo();
        }
        private void pboxTransitive_Click(object sender, EventArgs e)
        {
            DockingForm.DockForm.ShowColorDialog(pboxTransitive);

            //colorDialog1 = new ColorDialog();
            //colorDialog1.FullOpen = true;
            //colorDialog1.Color = pboxTransitive.BackColor;
            //colorDialog1.SolidColorOnly = true;
            //colorDialog1.ShowDialog(this);
            //pboxTransitive.BackColor = colorDialog1.Color;
        }

        private void pboxAugmentive_Click(object sender, EventArgs e)
        {
            DockingForm.DockForm.ShowColorDialog(pboxAugmentive);
            //colorDialog1 = new ColorDialog();
            //colorDialog1.FullOpen = true;
            //colorDialog1.Color = pboxAugmentive.BackColor;
            //colorDialog1.SolidColorOnly = true;
            //colorDialog1.SolidColorOnly = true;
            //colorDialog1.ShowDialog(this);
            //pboxAugmentive.BackColor = colorDialog1.Color;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DockingForm.DockForm.ShowColorDialog(pictureBox1);
            //colorDialog1 = new ColorDialog();
            //colorDialog1.FullOpen = true;
            //colorDialog1.Color = pictureBox1.BackColor;
            //colorDialog1.SolidColorOnly = true;
            //colorDialog1.ShowDialog(this);
            //pictureBox1.BackColor = colorDialog1.Color;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            DockingForm.DockForm.GetCurrentGraphViewContainer().ValidateModel(this);

            //beautiful?
            if (errorsFound)
            {
                btnGo.Enabled = true;
                //DockingForm.DockForm.DisplayTip("Validation failed" + Environment.NewLine + "Errors are indicated in the Validation Result window", comboBox1.Text, ToolTipIcon.Error);
                return;
            }

            switch (comboBox1.Text)
            {
                case "FEBT - Visual":
                    FEBT(true);
                    break;
                case "FEBT - SID":
                    FEBT(false);
                    break;
                case "Synthesis - DSD":
                    MyEngine.Diagram.StartTransaction();
                    DSD.DSDBuilder dbuilder = new MetaBuilder.UIControls.GraphingUI.Tools.DataModel.DSD.DSDBuilder();
                    FEBT(true);
                    dbuilder.MyEngine = MyEngine;
                    //dbuilder.SaveToDB();
                    dbuilder.CreateDiagram();
                    MyEngine.Diagram.FinishTransaction("DSD");
                    break;
            }
        }
    }
}