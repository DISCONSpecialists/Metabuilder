using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Northwoods.Go;
using Northwoods.Go.Layout;
using MetaBuilder.Core.Storage;
using MetaBuilder.Graphing.Persistence;
using MetaBuilder.BusinessLogic;
using System.IO;
using MetaBuilder.Graphing.Shapes.Nodes;
using System.Xml;
using MetaBuilder.UIControls.GraphingUI.Formatting;
using System.Collections.Generic;

namespace MetaBuilder.UIControls.Dialogs.MindMap
{
    public class MindmapForm : System.Windows.Forms.Form
    {
        #region Windows Form Designer generated code

        private Northwoods.Go.GoView goView1;
        private System.Windows.Forms.Label label1;
        private System.Timers.Timer timer1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem printToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem convertToFSDToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem autoLayoutToolStripMenuItem;
        private System.ComponentModel.Container components = null;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem formatToolStripMenuItem;
        private ToolStrip toolStripFormat;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripShapeA;
        private ToolStripButton toolStripShapeB;
        private ToolStripButton toolStripShapeC;
        private ToolStripButton toolStripShapeD;
        private ToolStripButton toolStripShapeE;
        private ToolStripButton toolStripButtonStart;
        private ToolStripButton toolStripButtonEnd;
        private ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ProgressBar progressBarIO;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MindmapForm));
            this.goView1 = new Northwoods.Go.GoView();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Timers.Timer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertToFSDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.progressBarIO = new System.Windows.Forms.ProgressBar();
            this.toolStripFormat = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonEnd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripShapeC = new System.Windows.Forms.ToolStripButton();
            this.toolStripShapeA = new System.Windows.Forms.ToolStripButton();
            this.toolStripShapeB = new System.Windows.Forms.ToolStripButton();
            this.toolStripShapeD = new System.Windows.Forms.ToolStripButton();
            this.toolStripShapeE = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.toolStripFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // goView1
            // 
            this.goView1.ArrowMoveLarge = 10F;
            this.goView1.ArrowMoveSmall = 1F;
            this.goView1.BackColor = System.Drawing.Color.White;
            this.goView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goView1.DragsRealtime = true;
            this.goView1.Location = new System.Drawing.Point(0, 95);
            this.goView1.Name = "goView1";
            this.goView1.PrimarySelectionColor = System.Drawing.Color.Magenta;
            this.goView1.SecondarySelectionColor = System.Drawing.Color.Magenta;
            this.goView1.Size = new System.Drawing.Size(1018, 628);
            this.goView1.TabIndex = 0;
            this.goView1.Text = "goView1";
            this.goView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.goView1_KeyDown);
            this.goView1.ObjectGotSelection += new Northwoods.Go.GoSelectionEventHandler(this.goView1_ObjectGotSelection);
            this.goView1.ObjectContextClicked += new Northwoods.Go.GoObjectEventHandler(this.goView1_ObjectContextClicked);
            this.goView1.LinkCreated += new Northwoods.Go.GoSelectionEventHandler(this.goView1_LinkCreated);
            this.goView1.ClipboardPasted += new System.EventHandler(this.goView1_ClipboardPasted);
            this.goView1.DocumentChanged += new Northwoods.Go.GoChangedEventHandler(this.goView1_DocumentChanged);
            this.goView1.BackgroundDoubleClicked += new Northwoods.Go.GoInputEventHandler(this.goView1_BackgroundDoubleClicked);
            this.goView1.ObjectSingleClicked += new Northwoods.Go.GoObjectEventHandler(this.goView1_ObjectSingleClicked);
            this.goView1.ObjectEdited += new Northwoods.Go.GoSelectionEventHandler(this.goView1_ObjectEdited);
            this.goView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.goView1_KeyUp);
            this.goView1.ObjectDoubleClicked += new Northwoods.Go.GoObjectEventHandler(this.goView1_ObjectDoubleClicked);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1018, 46);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.SynchronizingObject = this;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1018, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripMenuItem1,
            this.printToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(97, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.printToolStripMenuItem.Text = "&Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(97, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.exitToolStripMenuItem.Text = "Clos&e";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
            //,
            //this.toolStripMenuItem4,
            //this.formatToolStripMenuItem
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(105, 6);
            // 
            // formatToolStripMenuItem
            // 
            this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
            this.formatToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.formatToolStripMenuItem.Text = "Format";
            this.formatToolStripMenuItem.Click += new System.EventHandler(this.formatToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoLayoutToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // autoLayoutToolStripMenuItem
            // 
            this.autoLayoutToolStripMenuItem.Checked = true;
            this.autoLayoutToolStripMenuItem.CheckOnClick = true;
            this.autoLayoutToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoLayoutToolStripMenuItem.Name = "autoLayoutToolStripMenuItem";
            this.autoLayoutToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.autoLayoutToolStripMenuItem.Text = "Auto &Layout";
            // 
            // convertToFSDToolStripMenuItem
            // 
            this.convertToFSDToolStripMenuItem.Name = "convertToFSDToolStripMenuItem";
            this.convertToFSDToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.convertToFSDToolStripMenuItem.Text = "&Convert To FSD";
            this.convertToFSDToolStripMenuItem.Click += new System.EventHandler(this.convertToFSDToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(149, 6);
            // 
            // progressBarIO
            // 
            this.progressBarIO.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarIO.Enabled = false;
            this.progressBarIO.Location = new System.Drawing.Point(0, 723);
            this.progressBarIO.Name = "progressBarIO";
            this.progressBarIO.Size = new System.Drawing.Size(1018, 23);
            this.progressBarIO.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarIO.TabIndex = 3;
            this.progressBarIO.Visible = false;
            // 
            // toolStripFormat
            // 
            this.toolStripFormat.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonStart,
            this.toolStripLabel1,
            this.toolStripButtonEnd,
            this.toolStripSeparator1,
            this.toolStripShapeC,
            this.toolStripShapeA,
            this.toolStripShapeB,
            this.toolStripShapeD,
            this.toolStripShapeE});
            this.toolStripFormat.Location = new System.Drawing.Point(0, 70);
            this.toolStripFormat.Name = "toolStripFormat";
            this.toolStripFormat.Size = new System.Drawing.Size(1018, 25);
            this.toolStripFormat.TabIndex = 4;
            this.toolStripFormat.Text = "toolStrip1";
            // 
            // toolStripButtonStart
            // 
            this.toolStripButtonStart.AutoSize = false;
            this.toolStripButtonStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.toolStripButtonStart.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStart.Image")));
            this.toolStripButtonStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStart.Name = "toolStripButtonStart";
            this.toolStripButtonStart.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonStart.BackColorChanged += new System.EventHandler(this.formatControlChanged);
            this.toolStripButtonStart.Click += new System.EventHandler(this.colorPickerButton_Clicked);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(10, 22);
            this.toolStripLabel1.Text = " ";
            // 
            // toolStripButtonEnd
            // 
            this.toolStripButtonEnd.AutoSize = false;
            this.toolStripButtonEnd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.toolStripButtonEnd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEnd.Image")));
            this.toolStripButtonEnd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEnd.Name = "toolStripButtonEnd";
            this.toolStripButtonEnd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonEnd.BackColorChanged += new System.EventHandler(this.formatControlChanged);
            this.toolStripButtonEnd.Click += new System.EventHandler(this.colorPickerButton_Clicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripShapeC
            // 
            this.toolStripShapeC.AutoSize = false;
            this.toolStripShapeC.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripShapeC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripShapeC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripShapeC.Image = global::MetaBuilder.UIControls.Properties.Resources.Rectangle;
            this.toolStripShapeC.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripShapeC.Name = "toolStripShapeC";
            this.toolStripShapeC.Size = new System.Drawing.Size(22, 22);
            this.toolStripShapeC.Text = "Rectangle";
            this.toolStripShapeC.Click += new System.EventHandler(this.ToolStripMenuItemShape_Clicked);
            // 
            // toolStripShapeA
            // 
            this.toolStripShapeA.AutoSize = false;
            this.toolStripShapeA.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripShapeA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripShapeA.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripShapeA.Image = global::MetaBuilder.UIControls.Properties.Resources.RoundedRectangle;
            this.toolStripShapeA.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripShapeA.Name = "toolStripShapeA";
            this.toolStripShapeA.Size = new System.Drawing.Size(22, 22);
            this.toolStripShapeA.Text = "RoundedRectangle";
            this.toolStripShapeA.Click += new System.EventHandler(this.ToolStripMenuItemShape_Clicked);
            // 
            // toolStripShapeB
            // 
            this.toolStripShapeB.AutoSize = false;
            this.toolStripShapeB.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripShapeB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripShapeB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripShapeB.Image = global::MetaBuilder.UIControls.Properties.Resources.Ellipse;
            this.toolStripShapeB.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripShapeB.Name = "toolStripShapeB";
            this.toolStripShapeB.Size = new System.Drawing.Size(22, 22);
            this.toolStripShapeB.Text = "Ellipse";
            this.toolStripShapeB.Click += new System.EventHandler(this.ToolStripMenuItemShape_Clicked);
            // 
            // toolStripShapeD
            // 
            this.toolStripShapeD.AutoSize = false;
            this.toolStripShapeD.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripShapeD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripShapeD.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripShapeD.Image = global::MetaBuilder.UIControls.Properties.Resources.Triangle;
            this.toolStripShapeD.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripShapeD.Name = "toolStripShapeD";
            this.toolStripShapeD.Size = new System.Drawing.Size(22, 22);
            this.toolStripShapeD.Text = "Triangle";
            this.toolStripShapeD.Click += new System.EventHandler(this.ToolStripMenuItemShape_Clicked);
            // 
            // toolStripShapeE
            // 
            this.toolStripShapeE.AutoSize = false;
            this.toolStripShapeE.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripShapeE.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripShapeE.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripShapeE.Image = global::MetaBuilder.UIControls.Properties.Resources.Diamond;
            this.toolStripShapeE.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripShapeE.Name = "toolStripShapeE";
            this.toolStripShapeE.Size = new System.Drawing.Size(22, 22);
            this.toolStripShapeE.Text = "Diamond";
            this.toolStripShapeE.Click += new System.EventHandler(this.ToolStripMenuItemShape_Clicked);
            // 
            // MindmapForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1018, 746);
            this.Controls.Add(this.goView1);
            this.Controls.Add(this.progressBarIO);
            this.Controls.Add(this.toolStripFormat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MindmapForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MetaMapper";
            this.Load += new System.EventHandler(this.MindmapForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MindmapForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripFormat.ResumeLayout(false);
            this.toolStripFormat.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        void toolStripButtonStart_BackColorChanged(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private GoDocument myDocument;
        private string fileName;
        private MindmapLayout myLayout = null;
        private bool myPerformingLayout = false;
        private bool myNeedsLayout = false;
        private bool docChanged;
        private string tempDeleteFile;

        public bool DocumentChanged
        {
            get { return docChanged; }
            set
            {
                docChanged = value;
                //if (docChanged)
                //{
                //    if (!(this.Text.Contains("* ")))
                //        this.Text = "* " + this.Text;
                //}
                //else
                //{
                //    if (this.Text.Contains("* "))
                //        this.Text.Replace("* ", "");
                //}
            }
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
                this.Text = "MetaMapper" + " - " + fileName;
            }
        }
        public GoView MyView
        {
            get { return goView1; }
            set { goView1 = value; }
        }
        //Setting this updates the current goview with it, and binds the layout function to it
        public GoDocument MyDocument
        {
            get
            {
                return myDocument;
            }
            set
            {
                myDocument = value;

                //place all items in this document into the view
                MyView.SuspendLayout();
                MyView.BeginUpdate();
                MyView.Document = myDocument;
                MyView.EndUpdate();
                MyView.ResumeLayout();

                MyView.Document.UndoManager = new GoUndoManager();
                myLayout = new MindmapLayout();
                myLayout.MaxIterations = 2;
                myLayout.Document = goView1.Document;

                NeedLayout();
            }
        }

        public MindmapForm()
        {
            InitializeComponent();
            // Customize the standard kind of link that is drawn.
            goView1.NewGoLink.Pen = new Pen(Color.Red, 2);
            goView1.NewGoLink.Brush = Brushes.Black;
            goView1.NewGoLink.ToArrow = true;
            // enable undo and redo
            goView1.Document.UndoManager = new GoUndoManager();

            myLayout = new MindmapLayout();
            myLayout.MaxIterations = 2;
            myLayout.Document = goView1.Document;
        }
        public MindmapForm(string filename)
        {
            InitializeComponent();

            // Customize the standard kind of link that is drawn.
            goView1.NewGoLink.Pen = new Pen(Color.Red, 2);
            goView1.NewGoLink.Brush = Brushes.Black;
            goView1.NewGoLink.ToArrow = true;
            MyDocument = OpenMindMapFile(filename);

            Text = "MetaMapper - " + fileName;
        }

        void goView1_ClipboardPasted(object sender, EventArgs e)
        {
            foreach (GoObject o in MyView.Selection)
                o.Location = new PointF(o.Location.X + 15, o.Location.Y + 15);
        }

        private void MindmapForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            if (DocumentChanged)
            {
                DialogResult res = MessageBox.Show(this, "This Meta Map has not been saved. If you close this window all changes will be lost. Would you like to save?", "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (res)
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    case DialogResult.No:
                        DocumentChanged = false;
                        Close();
                        break;
                    case DialogResult.Yes:
                        saveToolStripMenuItem_Click(sender, e);
                        break;
                    default:
                        break;
                }
            }
            timer1.Enabled = false;
        }

        MindMapNode visibilizingNode;
        private void goView1_ObjectSingleClicked(object sender, GoObjectEventArgs e)
        {
            if (e.Shift && e.Control && e.GoObject.ParentNode is MindMapNode)
            //if (e.GoObject is GoRectangle && e.GoObject.ParentNode is MindMapNode)
            {
                MindMapNode n = e.GoObject.ParentNode as MindMapNode;
                visibilizingNode = n;
                n.Expanded = !n.Expanded;
                actedUpon = new List<MindMapNode>();
                visibilize(n, n.Expanded);
                visibilizingNode = null;
                actedUpon = new List<MindMapNode>();
                MyView.Selection.Clear();
                return;
            }
        }
        List<MindMapNode> actedUpon;
        private void visibilize(MindMapNode node, bool visible)
        {
            if (actedUpon.Contains(node))
                return;
            actedUpon.Add(node);

            foreach (GoObject o in node.DestinationLinks)
            {
                o.Visible = visible;
            }
            foreach (GoObject o in node.Destinations)
            {
                if (o is MindMapNode && o != visibilizingNode)
                {
                    o.Visible = visible;
                    visibilize((o as MindMapNode), visible);
                }
            }
            if (node != visibilizingNode)
                foreach (GoObject o in node.Links)
                {
                    o.Visible = visible;
                }
        }

        private void goView1_ObjectEdited(object sender, GoSelectionEventArgs e)
        {

        }
        private void goView1_ObjectContextClicked(object sender, GoObjectEventArgs e)
        {
            if (e.Control)
            {
                if (e.GoObject is MindMapNode)
                {
                    (e.GoObject as MindMapNode).IsFixedLocation = !(e.GoObject as MindMapNode).IsFixedLocation;
                    return;
                }
                if (e.GoObject.ParentNode is MindMapNode)
                {
                    (e.GoObject.ParentNode as MindMapNode).IsFixedLocation = !(e.GoObject.ParentNode as MindMapNode).IsFixedLocation;
                    return;
                }
            }
        }

        private void goView1_ObjectDoubleClicked(object sender, GoObjectEventArgs e)
        {
            if (e.Control)
            {
                MindMapNode node = insertAndLinkNode(e.GoObject);
                if (node != null)
                    node.DoBeginEdit(MyView);
            }
            else if (e.Shift)
            {
                insertAndLinkNode(e.GoObject);
                insertAndLinkNode(e.GoObject);
                insertAndLinkNode(e.GoObject);
                insertAndLinkNode(e.GoObject);
                insertAndLinkNode(e.GoObject);
            }
            else
                e.GoObject.DoBeginEdit(MyView);
        }
        private void goView1_BackgroundDoubleClicked(object sender, GoInputEventArgs e)
        {
            MindMapNode node = InsertNode(e.DocPoint);
            node.DoBeginEdit(MyView);
        }
        private void goView1_LinkCreated(object sender, Northwoods.Go.GoSelectionEventArgs e)
        {
            GoLink link = e.GoObject as GoLink;
            // Multi
            if (MyView.Selection.Count > 1)
            {
                foreach (GoObject o in MyView.Selection)
                {
                    if (!(o is MindMapNode)) continue; //Only nodes
                    MindMapNode n = o as MindMapNode;

                    if (n == link.ToNode) continue; //Cant link to itself
                    GoLink multiLink = new GoLink();

                    multiLink.FromPort = n.Port;
                    multiLink.ToPort = link.ToPort;

                    multiLink.Pen = new Pen(Color.Black, 2);
                    multiLink.Brush = Brushes.Green;// new SolidBrush(Color.Green);
                    multiLink.ToArrow = true;

                    MyView.Document.Add(multiLink);
                }
            }
            else
            {
                if (link != null)
                {
                    link.Pen = new Pen(Color.Black, 2);
                    link.Brush = Brushes.Green;//new SolidBrush(Color.Green);
                    link.ToArrow = true;
                }
            }
            DocumentChanged = true;
        }
        private void goView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                timer1.Enabled = false;
            }
        }
        private void goView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Oemtilde)
                showFormatForm();
        }

        // assume that selected objects may be moved, in which case we do not want the autolayout pushing/pulling them around
        private bool isSelecting;
        private void goView1_ObjectGotSelection(object sender, Northwoods.Go.GoSelectionEventArgs e)
        {
            isSelecting = true;
            if (myLayout.Network != null)
            {
                GoLayoutForceDirectedNode ln = myLayout.Network.FindNode(e.GoObject);
                if (ln != null)
                    ln.IsFixed = true;
            }
            if (e.GoObject is MindMapNode)
            {
                toolStripButtonStart.BackColor = (e.GoObject as MindMapNode).ShapeBrush.InnerColor;
                toolStripButtonEnd.BackColor = (e.GoObject as MindMapNode).ShapeBrush.OuterColor;
            }
            isSelecting = false;
        }
        private void goView1_ObjectLostSelection(object sender, Northwoods.Go.GoSelectionEventArgs e)
        {
            if (myLayout.Network != null)
            {
                GoLayoutForceDirectedNode ln = myLayout.Network.FindNode(e.GoObject);
                if (ln != null)
                    ln.IsFixed = false;
            }
        }
        private void goView1_DocumentChanged(object sender, Northwoods.Go.GoChangedEventArgs e)
        {
            if (e.Hint == GoLayer.ChangedObject)
            {
                // if some code other than PerformLayout is moving a node,
                if (!myPerformingLayout && e.SubHint == GoObject.ChangedBounds && e.GoObject is MindMapNode)
                {
                    // make sure we do a layout later
                    NeedLayout();
                    // and update the GoLayoutForceDirectedNode's position in the Network
                    if (myLayout.Network != null)
                    {
                        GoLayoutForceDirectedNode ln = myLayout.Network.FindNode(e.GoObject);
                        if (ln != null)
                        {
                            ln.IsFixed = true;
                            ln.Center = e.GoObject.SelectionObject.Center;
                        }
                    }
                }
            }
            else if (e.Hint == GoLayer.InsertedObject)
            {
                myLayout.Network = null;
                // don't do a layout when inserting nodes, just when inserting links --
                // that makes constructing graphs easier
                if (e.GoObject is IGoLink)
                {
                    NeedLayout();
                }
            }
            else if (e.Hint == GoLayer.RemovedObject)
            {
                myLayout.Network = null;
                NeedLayout();
            }
        }

        public GoDocument OpenMindMapFile(string filename)
        {
            progress(true);
            GoDocument rDOC = new GoDocument();
            string tempFile = filename + ".xml";
            bool unzipped = Graphing.Persistence.ZipUtil.UnzipFile(filename, tempFile);

            if (unzipped)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(tempFile);
                tempDeleteFile = tempFile;

                XmlNodeList nodes = doc.GetElementsByTagName("mmbasicnode");
                XmlNodeList links = doc.GetElementsByTagName("mmgolink");

                Hashtable addedNodes = new Hashtable();

                foreach (XmlNode n in nodes)
                {
                    MindMapNode node = null;

                    try
                    {
                        node = new MindMapNode(true);
                        node.MyShapeType = (MindMapNode.ShapeType)Enum.Parse(typeof(MindMapNode.ShapeType), (n.Attributes["shapeType"].ToString()));
                        node.SetShapeColor(Color.FromArgb(int.Parse(n.Attributes["brushFrom"].Value.ToString())), Color.FromArgb(int.Parse(n.Attributes["brushTo"].Value.ToString())));
                    }
                    catch
                    {
                        node = new MindMapNode(true);
                    }

                    //get labels text
                    node.GUID = n.Attributes["guid"].Value.ToString();
                    node.Text = n.Attributes["txt"].Value.ToString();
                    node.Label.FontSize = float.Parse(n.Attributes["fontSize"].Value.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    node.Label.FamilyName = n.Attributes["fontFamilyName"].Value.ToString();
                    try
                    {
                        node.Label.WrappingWidth = float.Parse(n.Attributes["wrappingWidth"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        node.Label.WrappingWidth = 120f;
                    }
                    node.Location = new PointF(float.Parse(n.Attributes["locX"].Value.ToString(), System.Globalization.CultureInfo.InvariantCulture), float.Parse(n.Attributes["locY"].Value.ToString(), System.Globalization.CultureInfo.InvariantCulture));
                    try
                    {
                        node.IsFixedLocation = bool.Parse(n.Attributes["isFixed"].ToString());
                    }
                    catch
                    {
                    }
                    //add to document
                    if (!addedNodes.ContainsKey(node.GUID))
                    {
                        addedNodes.Add(node.GUID, node);
                        rDOC.Add(node);
                    }
                    else
                    {
                        //This is a shallow copy type object!
                        //rDOC.Add(node);
                    }
                }
                foreach (XmlNode l in links)
                {
                    GoLink link = new GoLink();
                    //set link properties
                    try
                    {
                        link.FromPort = (addedNodes[l.Attributes["from"].Value.ToString()] as MindMapNode).Port;
                        link.ToPort = (addedNodes[l.Attributes["to"].Value.ToString()] as MindMapNode).Port;

                        link.Pen = new Pen(Color.Black, 2);
                        link.Brush = Brushes.Green;//new SolidBrush(Color.Green);
                        link.ToArrow = true;
                        //add to document
                        if (!rDOC.Contains(link))
                            rDOC.Add(link);
                    }
                    catch
                    {
                        //Cannot link
                    }
                }
            }
            //Delete temporary file
            System.Threading.ThreadStart ts = new System.Threading.ThreadStart(deleteTempFile);
            System.Threading.Thread t = new System.Threading.Thread(ts);
            t.Start();

            FileName = filename;
            progress(false);
            return rDOC;
        }
        private void deleteTempFile()
        {
            bool success = false;
            while (!success)
            {
                try
                {
                    //System.Threading.Thread.Sleep(2000);
                    System.IO.File.Delete(tempDeleteFile);
                    success = true;
                    tempDeleteFile = null;
                }
                catch
                {
                }
            }
        }

        private MindMapNode InsertNode(PointF pt)
        {
            GoDocument doc = goView1.Document;
            doc.StartTransaction();
            MindMapNode n = new MindMapNode(true);
            // specify the position and colors
            n.Location = pt;
            doc.Add(n);
            doc.FinishTransaction("Inserted node");

            DocumentChanged = true;

            return n;
        }
        private MindMapNode insertAndLinkNode()
        {
            if (goView1.Selection.First is MindMapNode)
            {
                MyView.Document.StartTransaction();

                MindMapNode selectednode = goView1.Selection.First as MindMapNode;
                MindMapNode addedNode = InsertNode(new PointF(selectednode.Location.X + 15, selectednode.Location.X + 15));

                GoLink l = new GoLink();
                l.FromPort = selectednode.Port;
                l.ToPort = addedNode.Port;
                l.Pen = new Pen(Color.Black, 2);
                l.Brush = Brushes.Green;//new SolidBrush(Color.Green);
                l.ToArrow = true;

                MyView.Document.Add(l);
                MyView.Document.FinishTransaction("Node added and linked");
                DocumentChanged = true;
                return addedNode;
            }
            return null;
        }
        private MindMapNode insertAndLinkNode(GoObject parent)
        {
            MindMapNode selectednode = null;
            if (parent is GoText)
                selectednode = (parent as GoText).ParentNode as MindMapNode;
            else
                selectednode = parent as MindMapNode;

            if (selectednode == null)
            {
                return insertAndLinkNode();
            }

            MyView.Document.StartTransaction();
            MindMapNode addedNode = InsertNode(new PointF(selectednode.Location.X + 15, selectednode.Location.X + 15));
            GoLink l = new GoLink();
            l.FromPort = selectednode.Port;
            l.ToPort = addedNode.Port;
            l.Pen = new Pen(Color.Black, 2);
            l.Brush = Brushes.Green;//new SolidBrush(Color.Green);
            l.ToArrow = true;

            MyView.Document.Add(l);
            MyView.Document.FinishTransaction("Node added and linked");
            DocumentChanged = true;
            return addedNode;
        }

        private bool determineFileName()
        {
            if (fileName.Length > 0)
            {
                if (Directory.Exists(Core.strings.GetPath(FileName)))
                {
                    //Let them know of our return?
                    return true;
                }
            }

            FileDialogSpecification dialogSpecification = FilePathManager.Instance.GetSpecification(FileTypeList.MindMap);
            SaveFileDialog sfdialog = new SaveFileDialog();
            sfdialog.SupportMultiDottedExtensions = true;
            sfdialog.Filter = dialogSpecification.Filter;
            sfdialog.Title = "Please enter the name for this Meta Map file";
            sfdialog.InitialDirectory = Core.Variables.Instance.DiagramPath;
            if (sfdialog.ShowDialog(this) == DialogResult.OK)
            {
                FileName = sfdialog.FileName;
            }

            return fileName.Length > 0;
        }

        private void NeedLayout()
        {
            myNeedsLayout = true;
            timer1.Enabled = true;
        }
        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (autoLayoutToolStripMenuItem.CheckState != CheckState.Checked) return;
            // if not everything is selected, don't perform a layout
            if (myNeedsLayout && goView1.Selection.Count != goView1.Document.Count)
            {
                myPerformingLayout = true;
                DocumentChanged = true;
                // don't record for undo/redo the movement of nodes and links due to autolayout
                MyView.Document.SkipsUndoManager = true;
                myLayout.PerformLayout();
                MyView.Document.SkipsUndoManager = false;

                myNeedsLayout = myLayout.CurrentIteration >= myLayout.MaxIterations && myLayout.MaxIterations > 1;
                myPerformingLayout = false;
            }
        }

        private void progress(bool on)
        {
            progressBarIO.Visible = on;
            progressBarIO.Enabled = on;
        }
        private void showFormatForm()
        {
            MyView.Document.UndoManager.StartTransaction();
            if (MyView.Selection.Count > 0)
            {
                Main mFormatting = new Main();
                MyView.Document.UndoManager.CurrentEdit = new GoUndoManagerCompoundEdit();
                mFormatting.MyView = MyView;
                mFormatting.ObjectCollection = MyView.Selection;
                mFormatting.SetPropertyGridObject(MyView.Selection.Primary);
                mFormatting.Init();
                mFormatting.GoToPage(1);
                mFormatting.ShowDialog(this);
            }
            MyView.Document.FinishTransaction("ShowFormatForm");

            MyView.Document.UndoManager = new GoUndoManager();
            myLayout = new MindmapLayout();
            myLayout.MaxIterations = 2;
            myLayout.Document = goView1.Document;

            NeedLayout();
        }

        #region Menu

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!determineFileName())
            {
                FileName = null;
                return;
            }

            timer1.Enabled = false;
            progress(true);
            FileUtil fUtil = new FileUtil();
            fUtil.Save(MyView.Document, FileName);
            DocumentChanged = false;
            progress(false);
            //MessageBox.Show(this,"Diagram was saved completely", "Save Complete", MessageBoxButtons.OK);
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DocumentChanged)
            {
                DialogResult res = MessageBox.Show(this, "This Meta Map has not been saved. If you open another Meta Map all changes will be lost. Would you like to save?", "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (res)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.No:
                        break;
                    case DialogResult.Yes:
                        saveToolStripMenuItem_Click(sender, e);
                        break;
                    default:
                        break;
                }
            }

            FileDialogSpecification dialogSpecification = FilePathManager.Instance.GetSpecification(FileTypeList.MindMap);
            OpenFileDialog ofdialog = new OpenFileDialog();
            ofdialog.SupportMultiDottedExtensions = true;
            ofdialog.Filter = dialogSpecification.Filter;
            ofdialog.Title = "Please select a Meta Map file to open";
            ofdialog.InitialDirectory = Core.Variables.Instance.DiagramPath;
            if (ofdialog.ShowDialog(this) == DialogResult.OK)
            {
                if (ofdialog.FileName.Length > 0)
                    MyDocument = OpenMindMapFile(ofdialog.FileName);
            }
        }
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UIControls.GraphingUI.CustomPrinting.MyPrintDialog prntD = new MetaBuilder.UIControls.GraphingUI.CustomPrinting.MyPrintDialog(false);
            prntD.Document = MyView.Document;
            prntD.ShowDialog(this);
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
            //timer1.Enabled = false;
            //if (DocumentChanged)
            //{
            //    DialogResult res = MessageBox.Show(this,"This Meta Map has not been saved. If you close this window all changes will be lost. Would you like to save?", "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //    switch (res)
            //    {
            //        case DialogResult.Cancel:
            //            break;
            //        case DialogResult.No:
            //            Close();
            //            break;
            //        case DialogResult.Yes:
            //            saveToolStripMenuItem_Click(sender, e);
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //timer1.Enabled = true;
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            goView1.EditCut();
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            goView1.EditCopy();
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            goView1.EditPaste();
        }
        private void formatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showFormatForm();
        }

        private void convertToFSDToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        private void MindmapForm_Load(object sender, EventArgs e)
        {
            //toolStripFormat.Visible = Core.Variables.Instance.IsDeveloperEdition;
        }

        #region Threaded Coloring

        //Add a thread that runs while this form is open
        //that checks over the diagram for levels of nodes
        //in order to change level colors based on predefined
        //colors set in options (UNLSS MANUALLY SPECIFIED)

        #endregion

        private void ToolStripMenuItemShape_Clicked(object sender, EventArgs e)
        {
            if (sender is ToolStripItem)
            {
                ToolStripItem item = sender as ToolStripItem;
                MindMapNode.ShapeType type = (MindMapNode.ShapeType)Enum.Parse(typeof(MindMapNode.ShapeType), item.Text);
                switchShape(type);
            }
        }
        //This method will switch all selected shapes to the newShape
        private void switchShape(MindMapNode.ShapeType newShape)
        {
            foreach (GoObject o in MyView.Selection)
            {
                if (!(o is MindMapNode)) continue;
                MindMapNode node = o as MindMapNode;
                node.MyShapeType = newShape;
            }
        }

        private void colorPickerButton_Clicked(object sender, EventArgs e)
        {
            MetaBuilder.UIControls.GraphingUI.DockingForm.DockForm.ShowColorDialog(sender);
        }
        private void formatControlChanged(object sender, EventArgs e)
        {
            if (isSelecting)
                return;
            foreach (GoObject o in MyView.Selection)
            {
                if (!(o is MindMapNode)) continue;
                MindMapNode node = o as MindMapNode;
                node.SetShapeColor(toolStripButtonStart.BackColor, toolStripButtonEnd.BackColor);
            }
        }
    }
}