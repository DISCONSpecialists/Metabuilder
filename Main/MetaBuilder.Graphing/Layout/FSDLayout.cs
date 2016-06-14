using System;
using System.Windows.Forms;
using Northwoods.Go;
using Northwoods.Go.Draw;
using Northwoods.Go.Layout;
using MetaBuilder.Graphing.Shapes;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;

namespace MetaBuilder.Graphing.Layout
{
    enum Direction { Left, Right, Up, Down }

    public class FSDLayout : Form
    {
        private GoView MyView;

        private FSDNode RootNode;
        private PointF RootPoint { get { return RootNode.MyLocation; } }

        private string message = "";
        public string Message { get { return message; } set { message = value; } }
        private bool success;
        public bool Success { get { return success; } set { success = value; } }

        //<3
        private int columnSpacing = 50;
        private int levelSpacing = 50;
        //>3
        private int nodeSpacing = 10;
        private int indentSpacing = 50;

        #region Form Init

        BackgroundWorker worker;
        Label lblHead;
        Label lblColumn, lblLevel, lblNodeSpacing, lblIndentSpacing;
        DomainUpDown dupColumn, dupLevel, dupNodeSpacing, dupIndentSpacing;
        ProgressBar progress;
        Button btnAction;

        private void initialize()
        {
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            //label docked top
            lblHead = new Label();
            lblHead.Dock = DockStyle.Top;
            lblHead.Text = "FSD Layout Settings";
            lblHead.AutoSize = true;
            lblHead.TextAlign = ContentAlignment.MiddleCenter;

            //label/textbox
            lblColumn = new Label();
            dupColumn = new DomainUpDown();
            //label/textbox
            lblLevel = new Label();
            dupLevel = new DomainUpDown();
            //label/textbox
            lblNodeSpacing = new Label();
            dupNodeSpacing = new DomainUpDown();
            //label/textbox
            lblIndentSpacing = new Label();
            dupIndentSpacing = new DomainUpDown();

            //progressbar
            progress = new ProgressBar();
            progress.Step = 1;
            progress.Style = ProgressBarStyle.Continuous;
            progress.Width = Width - 50 - 15;
            progress.Height = 25;
            progress.Location = new Point(Left + 5, Bottom - (progress.Height * 2) - 5);
            progress.Maximum = 100;
            progress.Minimum = 0;

            //button
            btnAction = new Button();
            btnAction.Text = "Go";
            btnAction.Width = 50;
            btnAction.Height = 25;
            btnAction.Location = new Point(Right - btnAction.Width - 5, Bottom - (btnAction.Height * 2) - 5);
            btnAction.Click += new EventHandler(btnAction_Click);

            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterScreen;

            Controls.Add(lblHead);
            Controls.Add(btnAction);
            Controls.Add(progress);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }
        private void btnAction_Click(object sender, EventArgs e)
        {
            progress.Maximum = 0;
            //if (RootNode != null)
            //    progress.Maximum = RootNode.TotalNodes();
            //else if (RootNodes.Count > 0)
            //    foreach (FSDNode n in RootNodes)
            //        progress.Maximum += n.TotalNodes();

            btnAction.Enabled = false;
            if (start()) Close();
            //worker.RunWorkerAsync();
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            start();
        }
        private void this_Closing(CancelEventArgs e)
        {
            if (worker.IsBusy)
                if (MessageBox.Show(this,"Pending Layout", "The layout process is currently busy, would you like to cancel it?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    e.Cancel = true;
                else
                    worker.CancelAsync();
        }

        #endregion

        public FSDLayout(GoView view)
        {
            initialize();

            MyView = view;
            manuallySelected = false;
            //Success = start();
        }
        public FSDLayout(GoView view, IMetaNode rootNode)
        {
            initialize();

            MyView = view;
            manuallySelected = true;
            RootNode = new FSDNode(rootNode, 3);//default to 3 because we want to skip the 2 level/row system for whole fsds
            //Success = start();
        }

        private bool start()
        {
            //Start += new StartHandler(FSDLayout_Start);

            if (RootNode == null)
                getRootNode();
            else
            {
                layoutTrees(RootNode);
                return true;
            }

            if (RootNodes.Count > 0)
            {
                foreach (FSDNode n in RootNodes)
                {
                    progress.Maximum += n.TotalNodes();

                    RootNode = n; //each node here deafults to level 1 so we can implement the 2 level/row system for whole fsds, treating each node as a new fsd in this case
                    layoutTrees(RootNode);
                }
            }
            else
            {
                Message = "Cannot find root node";
                return false;
            }

            return true;
        }

        private List<FSDNode> RootNodes;
        private void getRootNode()
        {
            RootNodes = new List<FSDNode>();
            foreach (GoObject o in MyView.Document)
            {
                if (!(o is IMetaNode))
                    continue;
                GoNode n = o as GoNode;
                if (n.SourceLinks.Count > 0 || n.Sources.Count > 0)
                    continue;
                RootNodes.Add(new FSDNode(n as IMetaNode));
            }
        }

        private bool manuallySelected = false;
        private int layoutTrees(FSDNode rootNode)
        {
            float y = 0;
            float x = 0;
            float treeWidth = 0;

            if (rootNode.Level == 1 || !manuallySelected) //First 2 levels and skip if we have manually selected the node ie : via Context menu
            {
                int width = 0;

                foreach (FSDNode cN in rootNode.ChildNodes)
                    width += cN.TreeWidth();

                rootNode.MyLocation = new PointF(width / 2 - rootNode.MyNode.Width / 2 + 100, MyView.DocumentTopLeft.Y + 150);

                //LEVEL 2
                float nodeC = rootNode.ChildNodes.Count / 2;
                float node = 1;
                float extraTreeChildren = 0;
                foreach (FSDNode childNode in rootNode.ChildNodes)
                {
                    y = (rootNode.MyLocation.Y + rootNode.MyNode.Height + levelSpacing);
                    x = rootNode.MyLocation.X + ((rootNode.MyNode.Width + columnSpacing + childNode.TreeWidth()) * (node - nodeC));

                    childNode.MyLocation = new PointF(x, y);

                    extraTreeChildren += layoutTrees(childNode) + 1;
                    treeWidth += 1;
                    node += 1;
                }
                treeWidth += extraTreeChildren;
            }
            else
            {
                float children = 0;
                float extraTreeChildren = 0;
                foreach (FSDNode childNode in rootNode.ChildNodes)
                {
                    y = rootNode.MyLocation.Y + ((nodeSpacing + childNode.MyNode.Height) * (children + 1));
                    x = rootNode.MyLocation.X + (indentSpacing == 0 ? rootNode.MyNode.Width / 2 : indentSpacing);

                    childNode.MyLocation = new PointF(x, y);

                    children += 1;
                    extraTreeChildren += layoutTrees(childNode);
                }
                treeWidth += extraTreeChildren + children + 1;
            }

            return (int)treeWidth;
        }
    }
}