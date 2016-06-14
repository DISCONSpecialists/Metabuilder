/*
 *  Copyright © 2007 - DISCON Specialists
 *
 *
 *  
 *  
 *  
 *  
 */
using System;
using System.ComponentModel;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI
{
    /// <summary>
    /// Summary description for Props.
    /// </summary>
    public class PropsForm : UserControl
    {

		#region Fields (4) 

        private ComboBox comboBox1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private GraphView myView;
        private PropertyGrid propertyGrid1;

		#endregion Fields 

		#region Constructors (1) 

        public PropsForm()
        {
            InitializeComponent();
            comboBox1.Items.Add("Primary Selection");
            comboBox1.SelectedIndex = 0;
            comboBox1.Items.Add("Layer");
            comboBox1.Items.Add("Document");
            comboBox1.Items.Add("View");
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
        }

		#endregion Constructors 

		#region Properties (3) 

        public Object Object
        {
            get { return PropertyGrid.SelectedObject; }
            set
            {
                if (PropertyGrid.SelectedObject == value) return;
                if (value is GoObject)
                    comboBox1.SelectedIndex = 0;
                else if (value is GoDocument)
                    comboBox1.SelectedIndex = 2;
                else if (value is GraphView)
                    comboBox1.SelectedIndex = 3;
                PropertyGrid.SelectedObject = value;
            }
        }

        public PropertyGrid PropertyGrid
        {
            get { return propertyGrid1; }
        }

        public GraphView View
        {
            get { return myView; }
            set
            {
                Object = null;
                myView = value;
            }
        }

		#endregion Properties 

		#region Methods (5) 


		// Protected Methods (5) 

        protected void comboBox1_SelectedIndexChanged(Object sender, EventArgs evt)
        {
            switch (comboBox1.SelectedIndex)
            {
                default:
                case 0:
                    Object = View.Selection.Primary;
                    break;
                case 1:
                    {
                        GoObject obj = View.Selection.Primary;
                        if (obj != null)
                            Object = obj.Layer;
                        else
                            comboBox1.SelectedIndex = 0;
                        break;
                    }
                case 2:
                    Object = View.Document;
                    break;
                case 3:
                    Object = View;
                    break;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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

        protected void OnActivated(EventArgs evt)
        {
            if (View != null)
            {
                //View.StartTransaction();
            }
            //base.OnActivated(evt);
        }

        protected void OnClosed(EventArgs evt)
        {
            //base.OnClosed(evt);
            if (View != null)
            {
                //this.View.FinishTransaction("Property Form Changes");
            }
            View = null;
        }

        protected void OnDeactivate(EventArgs evt)
        {
            //base.OnDeactivate(evt);
            if (View != null)
            {
                //this.View.FinishTransaction("Property Form Changes");
            }
        }


		#endregion Methods 


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = (((System.Windows.Forms.AnchorStyles.Top |
                                           System.Windows.Forms.AnchorStyles.Bottom)
                                          | System.Windows.Forms.AnchorStyles.Left)
                                         | System.Windows.Forms.AnchorStyles.Right);
            this.propertyGrid1.CommandsVisibleIfAvailable = true;
            this.propertyGrid1.LargeButtons = false;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 16);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(292, 544);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.Text = "";
            this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
            this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                     | System.Windows.Forms.AnchorStyles.Right);
            this.comboBox1.DropDownWidth = 296;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(296, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.Text = "";
            // 
            // PropsForm
            // 
            //// this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 557);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
                                       {
                                           this.comboBox1,
                                           this.propertyGrid1
                                       });
            this.Name = "PropsForm";
            //// this.ShowInTaskbar = true;
            this.Text = "Properties";
            this.ResumeLayout(false);
        }
        #endregion
    }
}