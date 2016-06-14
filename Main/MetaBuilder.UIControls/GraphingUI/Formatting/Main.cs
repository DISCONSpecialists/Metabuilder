using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.UIControls.Common;
using MetaBuilder.UIControls.GraphingUI.Formatting.FormatUndo;
using Northwoods.Go;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using fundo = MetaBuilder.UIControls.GraphingUI.Formatting.FormatUndo;

namespace MetaBuilder.UIControls.GraphingUI.Formatting
{
    partial class Main
    {

        #region Fields (5)

        public FormatEditCommand cmd;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;
        private GoView myView;
        private GoCollection objectCollection;
        private List<GoObject> SelectedObjects;

        #endregion Fields

        #region Properties (2)

        public GoView MyView
        {
            get { return myView; }
            set { myView = value; }
        }

        public GoCollection ObjectCollection
        {
            get { return objectCollection; }
            set { objectCollection = value; }
        }

        #endregion Properties

        #region Methods (10)

        // Protected Methods (1) 

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Private Methods (9) 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cmd.objectHistory = cmd.originalSettings;
            cmd.CancelSettings();
            //   cmd.Undo();
            myView.Document.UndoManager.FinishTransaction("Format");
            //myView.Document.UndoManager.Undo(); // DONT DO THIS BECAUSE IT IS HANDLED BY PROPERTY SETTER
            Close();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            // ApplyFormatSettings();
            // myView.Document.UndoManager.FinishTransaction("Format");     
            myView.Document.UndoManager.FinishTransaction("Format");
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //  ApplyFormatSettings();
            myView.Document.UndoManager.FinishTransaction("Format");
            Close();
        }

        private void colorPickerSolidColour_ColorSelected(object sender, EventArgs e)
        {
            /*if (colorPickerSolidColour.SelectedColor == Color.Transparent)
            {
                MessageBox.Show(this,"There is a problem printing with transparent colours. Please choose another colour.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //colorPickerSolidColour.SelectedColor = Color.White;
                return;
            }*/
            radioGradientFill_CheckedChanged(sender, e);
        }

        private void comboGradientType_SelectedIndexChanged(object sender, EventArgs e)
        {
            radioGradientFill_CheckedChanged(sender, e);
        }

        private void gradEditor_GradientChanged(object sender, EventArgs e)
        {
            pnlCanvas.Invalidate();
            UpdateSettings();
        }

        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Z)
            {
                myView.Document.UndoManager.Undo();
            }
            else if (e.Modifiers == Keys.Control && e.Modifiers == Keys.Shift && e.KeyCode == Keys.Z)
            {
                myView.Document.UndoManager.Redo();
            }
        }

        private void tabPageText_Click(object sender, EventArgs e)
        {
        }

        private void UpdateSettings(object sender, EventArgs e)
        {
            /*
            if (!IsInitialising)
            {
                
                if (angleChooserControl1.Enabled)
                {
                    cmd.PrimaryFormatting.TextSettings.Angle = angleChooserControl1.Value;
                }
                else
                {
                    cmd.PrimaryFormatting.TextSettings.Angle = 0;
                }
               cmd.PrimaryFormatting.GeneralSettings.AutoRescales = cbAutoRescale.Checked;
               cmd.PrimaryFormatting.GeneralSettings.AutoResizes = cbAutoResize.Checked;
               cmd.PrimaryFormatting.GeneralSettings.Corner = new SizeF(float.Parse(txtCornerX.Text), float.Parse(txtCornerY.Text));
               cmd.PrimaryFormatting.GeneralSettings.Font = new Font(((Font)listFonts.SelectedValue).Name, float.Parse(comboFontSize.Text.ToString()), (FontStyle)Enum.Parse(typeof(FontStyle), comboFontStyle.Text.ToString()));
              
                cmd.Settings.TextFormatSettings.Multiline = cbMultiline.Checked;
               cmd.Settings.TextFormatSettings.StrikeThrough = cbStrikeThrough.Checked;
               cmd.Settings.TextFormatSettings.TextColour = colorPickerText.SelectedColor;
               cmd.Settings.TextFormatSettings.Underline = cbUnderline.Checked;
               cmd.Settings.TextFormatSettings.Wrap = cbWrapping.Checked;
               cmd.Settings.TextFormatSettings.WrappingWidth = float.Parse(txtWrapTextAt.Text);


                if (cmd.HasShapes)
                {
                    cmd.Settings.ShapeFormatSettings.GradientStartColour = colorPickerSolidColour.SelectedColor;
                    cmd.Settings.ShapeFormatSettings.IsGradient = this.radioGradientFill.Checked;
                    if (comboGradientType.SelectedItem != null)
                    {
                        cmd.Settings.ShapeFormatSettings.GradientType = (GradientType)Enum.Parse(typeof(GradientType), comboGradientType.SelectedItem.ToString());
                    }
                    if (this.radioGradientFill.Checked)
                    {
                        cmd.Settings.ShapeFormatSettings.GradientStartColour = gradEditor.GetMarker(0).Color;
                        cmd.Settings.ShapeFormatSettings.GradientEndColour = gradEditor.GetMarker(1).Color;
                        cmd.Settings.ShapeFormatSettings.GradientType = (GradientType)Enum.Parse(typeof(GradientType), comboGradientType.SelectedItem.ToString());
                    }
                    else
                    {
                        cmd.Settings.ShapeFormatSettings.FillColour = this.colorPickerSolidColour.SelectedColor;
                        cmd.Settings.ShapeFormatSettings.GradientEndColour = this.colorPickerSolidColour.SelectedColor;
                        cmd.Settings.ShapeFormatSettings.GradientStartColour = this.colorPickerSolidColour.SelectedColor;
                    }

                    cmd.Settings.ShapeFormatSettings.PenColour = colorPickerPen.SelectedColor;
                    try
                    {
                        cmd.Settings.ShapeFormatSettings.PenEndCap = (LineCap)Enum.Parse(typeof(LineCap), comboPenEndCap.Text);
                        cmd.Settings.ShapeFormatSettings.PenStartCap = (LineCap)Enum.Parse(typeof(LineCap), comboPenStartCap.Text);
                        cmd.Settings.ShapeFormatSettings.PenStyle = (DashStyle)Enum.Parse(typeof(DashStyle), comboPenStyle.Text);
                        cmd.Settings.ShapeFormatSettings.PenWidth = float.Parse(comboPenWidth.Text);
                    }
                    catch { }
                }

                cmd.Settings.GeneralFormatSettings.Deletable = !cbLockText.Checked;
                cmd.Settings.GeneralFormatSettings.Printable = cbPrintable.Checked;
                cmd.Settings.GeneralFormatSettings.Resizable = cbResizable.Checked;
                cmd.Settings.GeneralFormatSettings.ResizesRealtime = cbResizesRealtime.Checked;
                cmd.Settings.GeneralFormatSettings.Selectable = cbSelectable.Checked;
                cmd.Settings.GeneralFormatSettings.Visible = cbVisible.Checked;
                cmd.ApplySettings();
               
            }
            else
            {
              
            }*/
        }

        #endregion Methods

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            MetaBuilder.Graphing.Formatting.Gradient gradient1 = new MetaBuilder.Graphing.Formatting.Gradient();
            System.Drawing.Drawing2D.ColorBlend colorBlend1 = new System.Drawing.Drawing2D.ColorBlend();
            this.cbUnderline = new System.Windows.Forms.CheckBox();
            this.cbStrikeThrough = new System.Windows.Forms.CheckBox();
            this.comboFontStyle = new System.Windows.Forms.ComboBox();
            this.colorPickerText = new Syncfusion.Windows.Forms.ColorPickerButton();
            this.cbMultiline = new System.Windows.Forms.CheckBox();
            this.comboFontSize = new System.Windows.Forms.ComboBox();
            this.txtWrapTextAt = new Syncfusion.Windows.Forms.Tools.DoubleTextBox();
            this.cbWrapping = new System.Windows.Forms.CheckBox();
            this.comboPenWidth = new System.Windows.Forms.ComboBox();
            this.comboPenStyle = new System.Windows.Forms.ComboBox();
            this.comboPenStartCap = new System.Windows.Forms.ComboBox();
            this.colorPickerPen = new Syncfusion.Windows.Forms.ColorPickerButton();
            this.comboPenEndCap = new System.Windows.Forms.ComboBox();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.tabControlAdv1 = new Syncfusion.Windows.Forms.Tools.TabControlAdv();
            this.tabPageText = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.listFonts = new System.Windows.Forms.ListBox();
            this.lblAngle = new System.Windows.Forms.Label();
            this.lblTextColour = new System.Windows.Forms.Label();
            this.lblFont = new System.Windows.Forms.Label();
            this.lblTextSize = new System.Windows.Forms.Label();
            this.lblPixels = new System.Windows.Forms.Label();
            this.lblTextStyle = new System.Windows.Forms.Label();
            this.tabPageLine = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.txtCornerY = new Syncfusion.Windows.Forms.Tools.DoubleTextBox();
            this.txtCornerX = new Syncfusion.Windows.Forms.Tools.DoubleTextBox();
            this.lblCornerSize = new System.Windows.Forms.Label();
            this.lblPenColour = new System.Windows.Forms.Label();
            this.lblPenEndCap = new System.Windows.Forms.Label();
            this.lblPenStartCap = new System.Windows.Forms.Label();
            this.lblStyle = new System.Windows.Forms.Label();
            this.lblPenWidth = new System.Windows.Forms.Label();
            this.tabPageFill = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.comboGradientType = new System.Windows.Forms.ComboBox();
            this.gradEditor = new MetaBuilder.UIControls.GraphingUI.Formatting.GradientTypeEditorUI();
            this.radioGradientFill = new System.Windows.Forms.RadioButton();
            this.radioSolidFill = new System.Windows.Forms.RadioButton();
            this.colorPickerSolidColour = new Syncfusion.Windows.Forms.ColorPickerButton();
            this.tabPageBehaviour = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.cbSelectable = new System.Windows.Forms.CheckBox();
            this.cbCannotDelete = new System.Windows.Forms.CheckBox();
            this.cbLockText = new System.Windows.Forms.CheckBox();
            this.cbVisible = new System.Windows.Forms.CheckBox();
            this.cbResizesRealtime = new System.Windows.Forms.CheckBox();
            this.cbPrintable = new System.Windows.Forms.CheckBox();
            this.cbAutoRescale = new System.Windows.Forms.CheckBox();
            this.cbAutoResize = new System.Windows.Forms.CheckBox();
            this.cbDragsParentShape = new System.Windows.Forms.CheckBox();
            this.cbResizable = new System.Windows.Forms.CheckBox();
            this.tabPageAdvanced = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.propertyGrid = new MetaBuilder.MetaControls.MetaPropertyGrid();
            this.btnCancel = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonOK = new Syncfusion.Windows.Forms.ButtonAdv();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.colorPickerButton3 = new Syncfusion.Windows.Forms.ColorPickerButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtWrapTextAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlAdv1)).BeginInit();
            this.tabControlAdv1.SuspendLayout();
            this.tabPageText.SuspendLayout();
            this.tabPageLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCornerY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCornerX)).BeginInit();
            this.tabPageFill.SuspendLayout();
            this.tabPageBehaviour.SuspendLayout();
            this.tabPageAdvanced.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbUnderline
            // 
            this.cbUnderline.AutoSize = true;
            this.cbUnderline.Location = new System.Drawing.Point(52, 281);
            this.cbUnderline.Name = "cbUnderline";
            this.cbUnderline.Size = new System.Drawing.Size(71, 17);
            this.cbUnderline.TabIndex = 5;
            this.cbUnderline.Text = "Underline";
            this.cbUnderline.UseVisualStyleBackColor = true;
            this.cbUnderline.CheckedChanged += new System.EventHandler(this.fontControls_Changed);
            // 
            // cbStrikeThrough
            // 
            this.cbStrikeThrough.AutoSize = true;
            this.cbStrikeThrough.Location = new System.Drawing.Point(52, 258);
            this.cbStrikeThrough.Name = "cbStrikeThrough";
            this.cbStrikeThrough.Size = new System.Drawing.Size(89, 17);
            this.cbStrikeThrough.TabIndex = 4;
            this.cbStrikeThrough.Text = "Strikethrough";
            this.cbStrikeThrough.UseVisualStyleBackColor = true;
            this.cbStrikeThrough.CheckedChanged += new System.EventHandler(this.fontControls_Changed);
            // 
            // comboFontStyle
            // 
            this.comboFontStyle.FormattingEnabled = true;
            this.comboFontStyle.Items.AddRange(new object[] {
            "Regular",
            "Italic",
            "Bold",
            "Italic Bold"});
            this.comboFontStyle.Location = new System.Drawing.Point(52, 125);
            this.comboFontStyle.Name = "comboFontStyle";
            this.comboFontStyle.Size = new System.Drawing.Size(121, 21);
            this.comboFontStyle.TabIndex = 2;
            this.comboFontStyle.Text = "Regular";
            this.comboFontStyle.SelectedIndexChanged += new System.EventHandler(this.fontControls_Changed);
            // 
            // colorPickerText
            // 
            this.colorPickerText.BackColor = System.Drawing.Color.Black;
            this.colorPickerText.ColorUISize = new System.Drawing.Size(208, 230);
            this.colorPickerText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorPickerText.Location = new System.Drawing.Point(52, 186);
            this.colorPickerText.Name = "colorPickerText";
            this.colorPickerText.SelectedAsBackcolor = true;
            this.colorPickerText.SelectedColor = System.Drawing.Color.Black;
            this.colorPickerText.SelectedColorGroup = Syncfusion.Windows.Forms.ColorUISelectedGroup.None;
            this.colorPickerText.Size = new System.Drawing.Size(50, 20);
            this.colorPickerText.TabIndex = 5;
            this.colorPickerText.UseVisualStyleBackColor = false;
            this.colorPickerText.ColorSelected += new System.EventHandler(this.fontControls_Changed);
            // 
            // cbMultiline
            // 
            this.cbMultiline.AutoSize = true;
            this.cbMultiline.Location = new System.Drawing.Point(52, 212);
            this.cbMultiline.Name = "cbMultiline";
            this.cbMultiline.Size = new System.Drawing.Size(64, 17);
            this.cbMultiline.TabIndex = 0;
            this.cbMultiline.Text = "Multiline";
            this.cbMultiline.UseVisualStyleBackColor = true;
            this.cbMultiline.CheckedChanged += new System.EventHandler(this.fontControls_Changed);
            // 
            // comboFontSize
            // 
            this.comboFontSize.FormattingEnabled = true;
            this.comboFontSize.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "12",
            "14",
            "16",
            "18",
            "24",
            "30",
            "36",
            "48",
            "72"});
            this.comboFontSize.Location = new System.Drawing.Point(52, 158);
            this.comboFontSize.Name = "comboFontSize";
            this.comboFontSize.Size = new System.Drawing.Size(121, 21);
            this.comboFontSize.TabIndex = 4;
            this.comboFontSize.Text = "10";
            this.comboFontSize.SelectedIndexChanged += new System.EventHandler(this.fontControls_Changed);
            // 
            // txtWrapTextAt
            // 
            this.txtWrapTextAt.DoubleValue = 0;
            this.txtWrapTextAt.Location = new System.Drawing.Point(146, 232);
            this.txtWrapTextAt.Name = "txtWrapTextAt";
            this.txtWrapTextAt.NegativeInputPendingOnSelectAll = false;
            this.txtWrapTextAt.NullString = "0.0";
            this.txtWrapTextAt.Size = new System.Drawing.Size(65, 20);
            this.txtWrapTextAt.TabIndex = 2;
            this.txtWrapTextAt.TextChanged += new System.EventHandler(this.fontControls_Changed);
            // 
            // cbWrapping
            // 
            this.cbWrapping.AutoSize = true;
            this.cbWrapping.Location = new System.Drawing.Point(52, 235);
            this.cbWrapping.Name = "cbWrapping";
            this.cbWrapping.Size = new System.Drawing.Size(88, 17);
            this.cbWrapping.TabIndex = 1;
            this.cbWrapping.Text = "Wrap Text at";
            this.cbWrapping.UseVisualStyleBackColor = true;
            this.cbWrapping.CheckedChanged += new System.EventHandler(this.fontControls_Changed);
            // 
            // comboPenWidth
            // 
            this.comboPenWidth.FormattingEnabled = true;
            this.comboPenWidth.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "10",
            "15",
            "20",
            "25",
            "40",
            "60",
            "100"});
            this.comboPenWidth.Location = new System.Drawing.Point(79, 13);
            this.comboPenWidth.Name = "comboPenWidth";
            this.comboPenWidth.Size = new System.Drawing.Size(121, 21);
            this.comboPenWidth.TabIndex = 8;
            this.comboPenWidth.Text = "1";
            // 
            // comboPenStyle
            // 
            this.comboPenStyle.FormattingEnabled = true;
            this.comboPenStyle.Items.AddRange(new object[] {
            "Dash",
            "DashDot",
            "DashDotDot",
            "Dot",
            "Solid"});
            this.comboPenStyle.Location = new System.Drawing.Point(79, 36);
            this.comboPenStyle.Name = "comboPenStyle";
            this.comboPenStyle.Size = new System.Drawing.Size(121, 21);
            this.comboPenStyle.TabIndex = 7;
            this.comboPenStyle.Text = "Solid";
            // 
            // comboPenStartCap
            // 
            this.comboPenStartCap.FormattingEnabled = true;
            this.comboPenStartCap.Items.AddRange(new object[] {
            "ArrowAnchor",
            "DiamondAnchor",
            "Flat",
            "NoAnchor",
            "Round",
            "RoundAnchor",
            "Square",
            "SquareAnchor",
            "Triangle"});
            this.comboPenStartCap.Location = new System.Drawing.Point(79, 63);
            this.comboPenStartCap.Name = "comboPenStartCap";
            this.comboPenStartCap.Size = new System.Drawing.Size(121, 21);
            this.comboPenStartCap.TabIndex = 3;
            this.comboPenStartCap.Text = "Square";
            // 
            // colorPickerPen
            // 
            this.colorPickerPen.BackColor = System.Drawing.Color.Black;
            this.colorPickerPen.ColorUISize = new System.Drawing.Size(208, 230);
            this.colorPickerPen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorPickerPen.Location = new System.Drawing.Point(79, 117);
            this.colorPickerPen.Name = "colorPickerPen";
            this.colorPickerPen.SelectedAsBackcolor = true;
            this.colorPickerPen.SelectedColor = System.Drawing.Color.Black;
            this.colorPickerPen.SelectedColorGroup = Syncfusion.Windows.Forms.ColorUISelectedGroup.None;
            this.colorPickerPen.Size = new System.Drawing.Size(44, 20);
            this.colorPickerPen.TabIndex = 0;
            this.colorPickerPen.UseVisualStyleBackColor = false;
            // 
            // comboPenEndCap
            // 
            this.comboPenEndCap.FormattingEnabled = true;
            this.comboPenEndCap.Items.AddRange(new object[] {
            "ArrowAnchor",
            "DiamondAnchor",
            "Flat",
            "NoAnchor",
            "Round",
            "RoundAnchor",
            "Square",
            "SquareAnchor",
            "Triangle"});
            this.comboPenEndCap.Location = new System.Drawing.Point(79, 90);
            this.comboPenEndCap.Name = "comboPenEndCap";
            this.comboPenEndCap.Size = new System.Drawing.Size(121, 21);
            this.comboPenEndCap.TabIndex = 4;
            this.comboPenEndCap.Text = "Square";
            // 
            // pnlCanvas
            // 
            this.pnlCanvas.Location = new System.Drawing.Point(39, 66);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(234, 109);
            this.pnlCanvas.TabIndex = 1;
            // 
            // tabControlAdv1
            // 
            this.tabControlAdv1.Controls.Add(this.tabPageText);
            this.tabControlAdv1.Controls.Add(this.tabPageLine);
            this.tabControlAdv1.Controls.Add(this.tabPageFill);
            this.tabControlAdv1.Controls.Add(this.tabPageBehaviour);
            this.tabControlAdv1.Controls.Add(this.tabPageAdvanced);
            this.tabControlAdv1.Location = new System.Drawing.Point(1, 2);
            this.tabControlAdv1.Name = "tabControlAdv1";
            this.tabControlAdv1.ShowScroll = false;
            this.tabControlAdv1.Size = new System.Drawing.Size(403, 365);
            this.tabControlAdv1.TabIndex = 5;
            // 
            // tabPageText
            // 
            this.tabPageText.Controls.Add(this.listFonts);
            this.tabPageText.Controls.Add(this.lblAngle);
            this.tabPageText.Controls.Add(this.cbUnderline);
            this.tabPageText.Controls.Add(this.lblTextColour);
            this.tabPageText.Controls.Add(this.lblFont);
            this.tabPageText.Controls.Add(this.cbStrikeThrough);
            this.tabPageText.Controls.Add(this.comboFontStyle);
            this.tabPageText.Controls.Add(this.colorPickerText);
            this.tabPageText.Controls.Add(this.lblTextSize);
            this.tabPageText.Controls.Add(this.lblPixels);
            this.tabPageText.Controls.Add(this.cbMultiline);
            this.tabPageText.Controls.Add(this.comboFontSize);
            this.tabPageText.Controls.Add(this.txtWrapTextAt);
            this.tabPageText.Controls.Add(this.cbWrapping);
            this.tabPageText.Controls.Add(this.lblTextStyle);
            this.tabPageText.Location = new System.Drawing.Point(1, 29);
            this.tabPageText.Name = "tabPageText";
            this.tabPageText.Size = new System.Drawing.Size(400, 334);
            this.tabPageText.TabIndex = 0;
            this.tabPageText.Text = "Text";
            this.tabPageText.Click += new System.EventHandler(this.tabPageText_Click);
            // 
            // listFonts
            // 
            this.listFonts.FormattingEnabled = true;
            this.listFonts.Location = new System.Drawing.Point(52, 11);
            this.listFonts.Name = "listFonts";
            this.listFonts.Size = new System.Drawing.Size(275, 95);
            this.listFonts.TabIndex = 9;
            this.listFonts.SelectedIndexChanged += new System.EventHandler(this.fontControls_Changed);
            // 
            // lblAngle
            // 
            this.lblAngle.AutoSize = true;
            this.lblAngle.Location = new System.Drawing.Point(227, 158);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(34, 13);
            this.lblAngle.TabIndex = 8;
            this.lblAngle.Text = "Angle";
            this.lblAngle.Visible = false;

            // 
            // lblTextColour
            // 
            this.lblTextColour.AutoSize = true;
            this.lblTextColour.Location = new System.Drawing.Point(9, 191);
            this.lblTextColour.Name = "lblTextColour";
            this.lblTextColour.Size = new System.Drawing.Size(37, 13);
            this.lblTextColour.TabIndex = 6;
            this.lblTextColour.Text = "Colour";
            // 
            // lblFont
            // 
            this.lblFont.AutoSize = true;
            this.lblFont.Location = new System.Drawing.Point(18, 11);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(28, 13);
            this.lblFont.TabIndex = 1;
            this.lblFont.Text = "Font";
            // 
            // lblTextSize
            // 
            this.lblTextSize.AutoSize = true;
            this.lblTextSize.Location = new System.Drawing.Point(19, 161);
            this.lblTextSize.Name = "lblTextSize";
            this.lblTextSize.Size = new System.Drawing.Size(27, 13);
            this.lblTextSize.TabIndex = 3;
            this.lblTextSize.Text = "Size";
            // 
            // lblPixels
            // 
            this.lblPixels.AutoSize = true;
            this.lblPixels.Location = new System.Drawing.Point(217, 236);
            this.lblPixels.Name = "lblPixels";
            this.lblPixels.Size = new System.Drawing.Size(34, 13);
            this.lblPixels.TabIndex = 3;
            this.lblPixels.Text = "Pixels";
            // 
            // lblTextStyle
            // 
            this.lblTextStyle.AutoSize = true;
            this.lblTextStyle.Location = new System.Drawing.Point(16, 128);
            this.lblTextStyle.Name = "lblTextStyle";
            this.lblTextStyle.Size = new System.Drawing.Size(30, 13);
            this.lblTextStyle.TabIndex = 1;
            this.lblTextStyle.Text = "Style";
            // 
            // tabPageLine
            // 
            this.tabPageLine.Controls.Add(this.txtCornerY);
            this.tabPageLine.Controls.Add(this.txtCornerX);
            this.tabPageLine.Controls.Add(this.lblCornerSize);
            this.tabPageLine.Controls.Add(this.lblPenColour);
            this.tabPageLine.Controls.Add(this.comboPenWidth);
            this.tabPageLine.Controls.Add(this.comboPenStyle);
            this.tabPageLine.Controls.Add(this.lblPenEndCap);
            this.tabPageLine.Controls.Add(this.lblPenStartCap);
            this.tabPageLine.Controls.Add(this.comboPenEndCap);
            this.tabPageLine.Controls.Add(this.comboPenStartCap);
            this.tabPageLine.Controls.Add(this.lblStyle);
            this.tabPageLine.Controls.Add(this.lblPenWidth);
            this.tabPageLine.Controls.Add(this.colorPickerPen);
            this.tabPageLine.Location = new System.Drawing.Point(1, 29);
            this.tabPageLine.Name = "tabPageLine";
            this.tabPageLine.Size = new System.Drawing.Size(400, 334);
            this.tabPageLine.TabIndex = 3;
            this.tabPageLine.TabVisible = false;
            this.tabPageLine.Text = "Line / Border";
            // 
            // txtCornerY
            // 
            this.txtCornerY.DoubleValue = 0;
            this.txtCornerY.Location = new System.Drawing.Point(112, 144);
            this.txtCornerY.MinValue = 0;
            this.txtCornerY.Name = "txtCornerY";
            this.txtCornerY.NegativeInputPendingOnSelectAll = false;
            this.txtCornerY.NullString = "0.0";
            this.txtCornerY.NumberDecimalDigits = 1;
            this.txtCornerY.Size = new System.Drawing.Size(31, 20);
            this.txtCornerY.TabIndex = 13;
            // 
            // txtCornerX
            // 
            this.txtCornerX.DoubleValue = 0;
            this.txtCornerX.Location = new System.Drawing.Point(79, 144);
            this.txtCornerX.Name = "txtCornerX";
            this.txtCornerX.NegativeInputPendingOnSelectAll = false;
            this.txtCornerX.NullString = "0.0";
            this.txtCornerX.NumberDecimalDigits = 1;
            this.txtCornerX.Size = new System.Drawing.Size(27, 20);
            this.txtCornerX.TabIndex = 12;
            // 
            // lblCornerSize
            // 
            this.lblCornerSize.AutoSize = true;
            this.lblCornerSize.Location = new System.Drawing.Point(14, 146);
            this.lblCornerSize.Name = "lblCornerSize";
            this.lblCornerSize.Size = new System.Drawing.Size(61, 13);
            this.lblCornerSize.TabIndex = 11;
            this.lblCornerSize.Text = "Corner Size";
            // 
            // lblPenColour
            // 
            this.lblPenColour.AutoSize = true;
            this.lblPenColour.Location = new System.Drawing.Point(36, 117);
            this.lblPenColour.Name = "lblPenColour";
            this.lblPenColour.Size = new System.Drawing.Size(37, 13);
            this.lblPenColour.TabIndex = 9;
            this.lblPenColour.Text = "Colour";
            // 
            // lblPenEndCap
            // 
            this.lblPenEndCap.AutoSize = true;
            this.lblPenEndCap.Location = new System.Drawing.Point(24, 93);
            this.lblPenEndCap.Name = "lblPenEndCap";
            this.lblPenEndCap.Size = new System.Drawing.Size(49, 13);
            this.lblPenEndCap.TabIndex = 6;
            this.lblPenEndCap.Text = "Line End";
            // 
            // lblPenStartCap
            // 
            this.lblPenStartCap.AutoSize = true;
            this.lblPenStartCap.Location = new System.Drawing.Point(21, 66);
            this.lblPenStartCap.Name = "lblPenStartCap";
            this.lblPenStartCap.Size = new System.Drawing.Size(52, 13);
            this.lblPenStartCap.TabIndex = 5;
            this.lblPenStartCap.Text = "Line Start";
            // 
            // lblStyle
            // 
            this.lblStyle.AutoSize = true;
            this.lblStyle.Location = new System.Drawing.Point(43, 39);
            this.lblStyle.Name = "lblStyle";
            this.lblStyle.Size = new System.Drawing.Size(30, 13);
            this.lblStyle.TabIndex = 2;
            this.lblStyle.Text = "Style";
            // 
            // lblPenWidth
            // 
            this.lblPenWidth.AutoSize = true;
            this.lblPenWidth.Location = new System.Drawing.Point(38, 16);
            this.lblPenWidth.Name = "lblPenWidth";
            this.lblPenWidth.Size = new System.Drawing.Size(35, 13);
            this.lblPenWidth.TabIndex = 1;
            this.lblPenWidth.Text = "Width";
            // 
            // tabPageFill
            // 
            this.tabPageFill.Controls.Add(this.comboGradientType);
            this.tabPageFill.Controls.Add(this.gradEditor);
            this.tabPageFill.Controls.Add(this.radioGradientFill);
            this.tabPageFill.Controls.Add(this.radioSolidFill);
            this.tabPageFill.Controls.Add(this.colorPickerSolidColour);
            this.tabPageFill.Controls.Add(this.pnlCanvas);
            this.tabPageFill.Location = new System.Drawing.Point(1, 1);
            this.tabPageFill.Name = "tabPageFill";
            this.tabPageFill.Size = new System.Drawing.Size(400, 362);
            this.tabPageFill.TabIndex = 1;
            this.tabPageFill.Text = "Fill";
            // 
            // comboGradientType
            // 
            this.comboGradientType.FormattingEnabled = true;
            this.comboGradientType.Items.AddRange(new object[] {
            "Horizontal",
            "Vertical",
            "ForwardDiagonal",
            "BackwardDiagonal",
            "Ellipse"});
            this.comboGradientType.Location = new System.Drawing.Point(94, 40);
            this.comboGradientType.Name = "comboGradientType";
            this.comboGradientType.Size = new System.Drawing.Size(121, 21);
            this.comboGradientType.TabIndex = 9;
            this.comboGradientType.Text = "Horizontal";
            this.comboGradientType.SelectedIndexChanged += new System.EventHandler(this.comboGradientType_SelectedIndexChanged);
            // 
            // gradEditor
            // 
            colorBlend1.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.White,
        System.Drawing.Color.White};
            colorBlend1.Positions = new float[] {
        0F,
        1F};
            gradient1.ColorBlend = colorBlend1;
            gradient1.GradientDirection = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.gradEditor.Gradient = gradient1;
            this.gradEditor.Location = new System.Drawing.Point(22, 181);
            this.gradEditor.Name = "gradEditor";
            this.gradEditor.Size = new System.Drawing.Size(349, 94);
            this.gradEditor.TabIndex = 0;
            // 
            // radioGradientFill
            // 
            this.radioGradientFill.AutoSize = true;
            this.radioGradientFill.Location = new System.Drawing.Point(22, 40);
            this.radioGradientFill.Name = "radioGradientFill";
            this.radioGradientFill.Size = new System.Drawing.Size(65, 17);
            this.radioGradientFill.TabIndex = 8;
            this.radioGradientFill.Text = "Gradient";
            this.radioGradientFill.UseVisualStyleBackColor = true;
            // 
            // radioSolidFill
            // 
            this.radioSolidFill.AutoSize = true;
            this.radioSolidFill.Checked = true;
            this.radioSolidFill.Location = new System.Drawing.Point(22, 14);
            this.radioSolidFill.Name = "radioSolidFill";
            this.radioSolidFill.Size = new System.Drawing.Size(48, 17);
            this.radioSolidFill.TabIndex = 7;
            this.radioSolidFill.TabStop = true;
            this.radioSolidFill.Text = "Solid";
            this.radioSolidFill.UseVisualStyleBackColor = true;
            // 
            // colorPickerSolidColour
            // 
            this.colorPickerSolidColour.BackColor = System.Drawing.Color.Transparent;
            this.colorPickerSolidColour.ColorUISize = new System.Drawing.Size(208, 230);
            this.colorPickerSolidColour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorPickerSolidColour.Location = new System.Drawing.Point(86, 14);
            this.colorPickerSolidColour.Name = "colorPickerSolidColour";
            this.colorPickerSolidColour.SelectedAsBackcolor = true;
            this.colorPickerSolidColour.SelectedColor = System.Drawing.Color.Transparent;
            this.colorPickerSolidColour.SelectedColorGroup = Syncfusion.Windows.Forms.ColorUISelectedGroup.None;
            this.colorPickerSolidColour.Size = new System.Drawing.Size(50, 20);
            this.colorPickerSolidColour.TabIndex = 6;
            this.colorPickerSolidColour.UseVisualStyleBackColor = false;
            this.colorPickerSolidColour.ColorSelected += new System.EventHandler(this.colorPickerSolidColour_ColorSelected);
            // 
            // tabPageBehaviour
            // 
            this.tabPageBehaviour.Controls.Add(this.cbSelectable);
            this.tabPageBehaviour.Controls.Add(this.cbCannotDelete);
            this.tabPageBehaviour.Controls.Add(this.cbLockText);
            this.tabPageBehaviour.Controls.Add(this.cbVisible);
            this.tabPageBehaviour.Controls.Add(this.cbResizesRealtime);
            this.tabPageBehaviour.Controls.Add(this.cbPrintable);
            this.tabPageBehaviour.Controls.Add(this.cbAutoRescale);
            this.tabPageBehaviour.Controls.Add(this.cbAutoResize);
            this.tabPageBehaviour.Controls.Add(this.cbDragsParentShape);
            this.tabPageBehaviour.Controls.Add(this.cbResizable);
            this.tabPageBehaviour.Location = new System.Drawing.Point(1, 1);
            this.tabPageBehaviour.Name = "tabPageBehaviour";
            this.tabPageBehaviour.Size = new System.Drawing.Size(400, 362);
            this.tabPageBehaviour.TabIndex = 2;
            this.tabPageBehaviour.TabVisible = false;
            this.tabPageBehaviour.Text = "Behaviour";
            // 
            // cbSelectable
            // 
            this.cbSelectable.AutoSize = true;
            this.cbSelectable.Location = new System.Drawing.Point(10, 13);
            this.cbSelectable.Name = "cbSelectable";
            this.cbSelectable.Size = new System.Drawing.Size(76, 17);
            this.cbSelectable.TabIndex = 13;
            this.cbSelectable.Text = "Selectable";
            this.cbSelectable.UseVisualStyleBackColor = true;
            // 
            // cbCannotDelete
            // 
            this.cbCannotDelete.AutoSize = true;
            this.cbCannotDelete.Location = new System.Drawing.Point(10, 216);
            this.cbCannotDelete.Name = "cbCannotDelete";
            this.cbCannotDelete.Size = new System.Drawing.Size(125, 17);
            this.cbCannotDelete.TabIndex = 12;
            this.cbCannotDelete.Text = "Protect from Deletion";
            this.cbCannotDelete.UseVisualStyleBackColor = true;
            // 
            // cbLockText
            // 
            this.cbLockText.AutoSize = true;
            this.cbLockText.Location = new System.Drawing.Point(10, 193);
            this.cbLockText.Name = "cbLockText";
            this.cbLockText.Size = new System.Drawing.Size(74, 17);
            this.cbLockText.TabIndex = 11;
            this.cbLockText.Text = "Lock Text";
            this.cbLockText.UseVisualStyleBackColor = true;
            // 
            // cbVisible
            // 
            this.cbVisible.AutoSize = true;
            this.cbVisible.Location = new System.Drawing.Point(10, 170);
            this.cbVisible.Name = "cbVisible";
            this.cbVisible.Size = new System.Drawing.Size(56, 17);
            this.cbVisible.TabIndex = 9;
            this.cbVisible.Text = "Visible";
            this.cbVisible.UseVisualStyleBackColor = true;
            // 
            // cbResizesRealtime
            // 
            this.cbResizesRealtime.AutoSize = true;
            this.cbResizesRealtime.Location = new System.Drawing.Point(10, 55);
            this.cbResizesRealtime.Name = "cbResizesRealtime";
            this.cbResizesRealtime.Size = new System.Drawing.Size(107, 17);
            this.cbResizesRealtime.TabIndex = 8;
            this.cbResizesRealtime.Text = "Resizes Realtime";
            this.cbResizesRealtime.UseVisualStyleBackColor = true;
            // 
            // cbPrintable
            // 
            this.cbPrintable.AutoSize = true;
            this.cbPrintable.Location = new System.Drawing.Point(10, 147);
            this.cbPrintable.Name = "cbPrintable";
            this.cbPrintable.Size = new System.Drawing.Size(67, 17);
            this.cbPrintable.TabIndex = 7;
            this.cbPrintable.Text = "Printable";
            this.cbPrintable.UseVisualStyleBackColor = true;
            // 
            // cbAutoRescale
            // 
            this.cbAutoRescale.AutoSize = true;
            this.cbAutoRescale.Location = new System.Drawing.Point(10, 124);
            this.cbAutoRescale.Name = "cbAutoRescale";
            this.cbAutoRescale.Size = new System.Drawing.Size(90, 17);
            this.cbAutoRescale.TabIndex = 6;
            this.cbAutoRescale.Text = "Auto Rescale";
            this.cbAutoRescale.UseVisualStyleBackColor = true;
            // 
            // cbAutoResize
            // 
            this.cbAutoResize.AutoSize = true;
            this.cbAutoResize.Location = new System.Drawing.Point(10, 78);
            this.cbAutoResize.Name = "cbAutoResize";
            this.cbAutoResize.Size = new System.Drawing.Size(83, 17);
            this.cbAutoResize.TabIndex = 5;
            this.cbAutoResize.Text = "Auto Resize";
            this.cbAutoResize.UseVisualStyleBackColor = true;
            // 
            // cbDragsParentShape
            // 
            this.cbDragsParentShape.AutoSize = true;
            this.cbDragsParentShape.Location = new System.Drawing.Point(10, 101);
            this.cbDragsParentShape.Name = "cbDragsParentShape";
            this.cbDragsParentShape.Size = new System.Drawing.Size(122, 17);
            this.cbDragsParentShape.TabIndex = 4;
            this.cbDragsParentShape.Text = "Drags Parent Shape";
            this.cbDragsParentShape.UseVisualStyleBackColor = true;
            // 
            // cbResizable
            // 
            this.cbResizable.AutoSize = true;
            this.cbResizable.Location = new System.Drawing.Point(10, 32);
            this.cbResizable.Name = "cbResizable";
            this.cbResizable.Size = new System.Drawing.Size(72, 17);
            this.cbResizable.TabIndex = 3;
            this.cbResizable.Text = "Resizable";
            this.cbResizable.UseVisualStyleBackColor = true;
            // 
            // tabPageAdvanced
            // 
            this.tabPageAdvanced.Controls.Add(this.propertyGrid);
            this.tabPageAdvanced.Location = new System.Drawing.Point(1, 1);
            this.tabPageAdvanced.Name = "tabPageAdvanced";
            this.tabPageAdvanced.Size = new System.Drawing.Size(400, 362);
            this.tabPageAdvanced.TabIndex = 5;
            this.tabPageAdvanced.TabVisible = false;
            this.tabPageAdvanced.Text = "Advanced";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(400, 362);
            this.propertyGrid.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Classic;
            this.btnCancel.ComboEditBackColor = System.Drawing.Color.Empty;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.IsMouseDown = false;
            this.btnCancel.Location = new System.Drawing.Point(327, 373);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyle = true;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Classic;
            this.buttonOK.ComboEditBackColor = System.Drawing.Color.Empty;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.buttonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonOK.IsMouseDown = false;
            this.buttonOK.Location = new System.Drawing.Point(246, 373);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyle = true;
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(6, 65);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(89, 17);
            this.checkBox3.TabIndex = 4;
            this.checkBox3.Text = "Strikethrough";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(64, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Multiline";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(6, 42);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(88, 17);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Wrap Text at";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Solid Colour:";
            // 
            // colorPickerButton3
            // 
            this.colorPickerButton3.BackColor = System.Drawing.Color.Black;
            this.colorPickerButton3.ColorUISize = new System.Drawing.Size(208, 230);
            this.colorPickerButton3.Location = new System.Drawing.Point(110, 108);
            this.colorPickerButton3.Name = "colorPickerButton3";
            this.colorPickerButton3.SelectedAsBackcolor = true;
            this.colorPickerButton3.SelectedColor = System.Drawing.Color.Black;
            this.colorPickerButton3.SelectedColorGroup = Syncfusion.Windows.Forms.ColorUISelectedGroup.None;
            this.colorPickerButton3.Size = new System.Drawing.Size(121, 23);
            this.colorPickerButton3.TabIndex = 6;
            this.colorPickerButton3.UseVisualStyleBackColor = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(406, 399);
            this.ControlBox = false;
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControlAdv1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Formatting";
            ////Attempted showontop fix
            //this.TopMost = true;
            //this.Focus();
            //this.BringToFront();
            //this.TopMost = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Main_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.txtWrapTextAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlAdv1)).EndInit();
            this.tabControlAdv1.ResumeLayout(false);
            this.tabPageText.ResumeLayout(false);
            this.tabPageText.PerformLayout();
            this.tabPageLine.ResumeLayout(false);
            this.tabPageLine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCornerY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCornerX)).EndInit();
            this.tabPageFill.ResumeLayout(false);
            this.tabPageFill.PerformLayout();
            this.tabPageBehaviour.ResumeLayout(false);
            this.tabPageBehaviour.PerformLayout();
            this.tabPageAdvanced.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //myView.Document.UndoManager.FinishTransaction("Format");
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void fontControls_Changed(object sender, EventArgs e)
        {
            MarkFontDirty(sender, e);
        }

        private void fillControls_Changed(object sender, EventArgs e)
        {
            MarkFillDirty(sender, e);
        }

        private void MarkFontDirty(object sender, EventArgs e)
        {
            if (cmd != null)
            {
                if (!IsInitialising)
                    UpdateSettings(sender, e);
            }
        }

        private void MarkFillDirty(object sender, EventArgs e)
        {
            if (cmd != null)
            {
                if (!IsInitialising)
                    UpdateSettings(sender, e);
            }
        }
        #endregion

        #region Controls
        private GradientTypeEditorUI gradEditor;
        private ComboBox comboPenEndCap;
        private ComboBox comboPenStyle;
        private Label lblPenEndCap;
        private Label lblPenStartCap;
        private ComboBox comboPenWidth;
        private Label lblPenColour;
        private CheckBox cbVisible;
        private CheckBox cbResizesRealtime;
        private CheckBox cbPrintable;
        private CheckBox cbAutoRescale;
        private CheckBox cbAutoResize;
        private CheckBox cbDragsParentShape;
        private CheckBox cbResizable;
        private MetaBuilder.MetaControls.MetaPropertyGrid propertyGrid;
        private CheckBox cbUnderline;
        private Label lblTextColour;
        private Label lblFont;
        private CheckBox cbStrikeThrough;
        private ComboBox comboFontStyle;
        private ColorPickerButton colorPickerText;
        private Label lblTextSize;
        private Label lblPixels;
        private CheckBox cbMultiline;
        private ComboBox comboFontSize;
        private DoubleTextBox txtWrapTextAt;
        private CheckBox cbWrapping;
        private Label lblTextStyle;
        private CheckBox cbSelectable;
        private CheckBox cbCannotDelete;
        private CheckBox cbLockText;
        private Panel pnlCanvas;
        private TabControlAdv tabControlAdv1;
        private TabPageAdv tabPageText;
        private TabPageAdv tabPageFill;
        private TabPageAdv tabPageBehaviour;
        private TabPageAdv tabPageLine;
        private ButtonAdv btnCancel;
        private ButtonAdv buttonOK;
        private TabPageAdv tabPageAdvanced;
        private ColorPickerButton colorPickerPen;
        private CheckBox checkBox3;
        private CheckBox checkBox1;
        private ColorPickerButton colorPickerSolidColour;
        private RadioButton radioGradientFill;
        private RadioButton radioSolidFill;
        private Label label2;
        private ColorPickerButton colorPickerButton3;
        private ComboBox comboPenStartCap;
        private Label lblStyle;
        private Label lblPenWidth;
        private DoubleTextBox txtCornerY;
        private DoubleTextBox txtCornerX;
        private Label lblCornerSize;
        private ComboBox comboGradientType;
        private Label lblAngle;
        private ListBox listFonts;
        private CheckBox checkBox2;
        #endregion
    }

    public partial class Main : Form
    {

        #region Fields (4)

        private bool hasShapes;
        private bool hasText;
        private bool IsInitialising;
        private FormattingManipulator manipulator;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            InitializeComponent();
            IsInitialising = true;
            // These are our final data sources: two ArrayList objects.
            ArrayList fontObjList = new ArrayList();
            InstalledFontCollection InstalledFonts = new InstalledFontCollection();
            foreach (FontFamily family in InstalledFonts.Families)
            {
                try
                {
                    if (family.IsStyleAvailable(FontStyle.Regular))
                        fontObjList.Add(new Font(family, 12));
                }
                catch
                {
                    // We end up here if the font could not be created
                    // with the default style.
                }
            }
            listFonts.DataSource = fontObjList;
            listFonts.DisplayMember = "Name";
            gradEditor.SilentMarkers = true;
            gradEditor.Gradient = new Gradient(Color.SteelBlue, Color.White);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            StartPosition = FormStartPosition.CenterScreen;
        }

        #endregion Constructors

        #region Properties (3)

        /// <summary>
        /// Gets or sets a value indicating whether this instance has shapes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has shapes; otherwise, <c>false</c>.
        /// </value>
        public bool HasShapes
        {
            get { return hasShapes; }
            set { hasShapes = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has text.
        /// </summary>
        /// <value><c>true</c> if this instance has text; otherwise, <c>false</c>.</value>
        public bool HasText
        {
            get { return hasText; }
            set { hasText = value; }
        }

        /// <summary>
        /// Gets or sets the manipulator.
        /// </summary>
        /// <value>The manipulator.</value>
        public FormattingManipulator Manipulator
        {
            get { return manipulator; }
            set { manipulator = value; }
        }

        #endregion Properties

        #region Methods (11)

        // Public Methods (3) 

        /// <summary>
        /// Goes to the specified tabpage.
        /// </summary>
        /// <param name="PageNumber">The page number.</param>
        public void GoToPage(int PageNumber)
        {
            tabControlAdv1.SelectedIndex = PageNumber;
        }

        // COPYFORMATTING 
        /// <summary>
        /// Inits this instance.
        /// </summary>
        public void Init()
        {
            SelectedObjects = new List<GoObject>();
            GoCollectionEnumerator colEnum = ObjectCollection.GetEnumerator();
            StoreSettingsForCollection(colEnum);
            StoreProperties(SelectedObjects);
            cmd = new FormatEditCommand();
            cmd.Collection = SelectedObjects;
            cmd.StoreSettings(cmd.originalSettings);

            if (myView.Document.UndoManager == null) //Readonly diagrams do not have an undo manager!
                Close();

            if (myView.Document.UndoManager.CurrentEdit == null)
                myView.Document.UndoManager.CurrentEdit = new GoUndoManagerCompoundEdit();
            myView.Document.UndoManager.CurrentEdit.AddEdit(cmd);
            BindInterface();
            SetupMethods();
        }

        /// <summary>
        /// Sets the property grid object.
        /// </summary>
        /// <param name="o">The o.</param>
        public void SetPropertyGridObject(object o)
        {
            IsInitialising = true;
            propertyGrid.SelectedObject = o;
            isSubgraphFormatting = (o is MindMapNode) || (o is SubgraphNode);
            IsInitialising = false;
        }

        private bool isSubgraphFormatting;

        // Private Methods (8) 

        private void AddSelectedObject(GoObject o)
        {
            if (!SelectedObjects.Contains(o))
                SelectedObjects.Add(o);
        }

        // COPYFORMATTING 
        /// <summary>
        /// Applies the format settings.
        /// </summary>
        private void ApplyFormatSettings()
        {
            GoUndoManager umanager = myView.Document.UndoManager;
            if (umanager != null)
            {
                umanager.StartTransaction();
            }

            manipulator.ApplyFormatSettings();
            manipulator.IsComplete = true;
            if (umanager != null)
            {
                umanager.CurrentEdit = new GoUndoManagerCompoundEdit();
                umanager.CurrentEdit.AddEdit(manipulator);
                umanager.FinishTransaction("Apply Formatting");
            }
        }

        /// <summary>
        /// Binds the interface.
        /// </summary>
        private void BindInterface()
        {
            IsInitialising = true;
            // Setup the combos
            Array caps = Enum.GetValues(typeof(DashCap));
            comboPenEndCap.DataSource = caps;
            comboPenStartCap.DataSource = caps;
            comboPenStyle.DataSource = Enum.GetValues(typeof(DashStyle));
            comboPenEndCap.SelectedIndex = 0;
            comboPenStartCap.SelectedIndex = 0;
            comboPenStyle.SelectedIndex = 0;
            if (cmd != null)
            {
                if (cmd.PrimaryFormatting.ShapeSettings.PenStartCap.HasValue)
                    comboPenStartCap.Text = cmd.PrimaryFormatting.ShapeSettings.PenStartCap.Value.ToString();
                if (cmd.PrimaryFormatting.ShapeSettings.PenEndCap.HasValue)
                    comboPenEndCap.Text = cmd.PrimaryFormatting.ShapeSettings.PenEndCap.Value.ToString();
                if (cmd.PrimaryFormatting.ShapeSettings.DashStyle.HasValue)
                    comboPenStyle.Text = cmd.PrimaryFormatting.ShapeSettings.DashStyle.Value.ToString();
                // font style might be a combination of bold/italic, so need to add items manually
                // rather than binding to enumcollection
                comboFontStyle.Items.Clear();
                foreach (FontStyle style in Enum.GetValues(typeof(FontStyle)))
                {
                    if (style != FontStyle.Strikeout && style != FontStyle.Underline)
                        // these are controlled via checkboxes
                        comboFontStyle.Items.Add(style);
                }
                comboFontStyle.Items.Add(FontStyle.Italic | FontStyle.Bold);
                if (cmd.HasText && cmd.PrimaryFormatting.TextSettings.Font != null)// && !isSubgraphFormatting
                {
                    listFonts.SelectedIndex = listFonts.FindStringExact(cmd.PrimaryFormatting.TextSettings.Font.Name);
                    colorPickerText.SelectedColor = cmd.PrimaryFormatting.TextSettings.TextColour.Value;
                    comboFontSize.SelectedIndex = comboFontSize.FindStringExact(cmd.PrimaryFormatting.TextSettings.Font.Size.ToString().Replace(".0", ""));

                    // Setup the font style - it might be a combination
                    bool IsItalic = cmd.PrimaryFormatting.TextSettings.Italic.Value;
                    bool IsBold = cmd.PrimaryFormatting.TextSettings.Bold.Value;
                    if (IsItalic && IsBold)
                    {
                        comboFontStyle.SelectedIndex = comboFontStyle.FindStringExact("Bold, Italic");
                    }
                    if (IsItalic && !IsBold)
                        comboFontStyle.SelectedIndex = comboFontStyle.FindStringExact("Italic");
                    if (!IsItalic && IsBold)
                        comboFontStyle.SelectedIndex = comboFontStyle.FindStringExact("Bold");
                    if (!IsItalic && !IsBold)
                        comboFontStyle.SelectedIndex = comboFontStyle.FindStringExact("Regular");
                    cbMultiline.Checked = cmd.PrimaryFormatting.TextSettings.Multiline.Value;
                    cbStrikeThrough.Checked = cmd.PrimaryFormatting.TextSettings.StrikeThrough.Value;
                    cbUnderline.Checked = cmd.PrimaryFormatting.TextSettings.Underline.Value;
                    txtWrapTextAt.Text = cmd.PrimaryFormatting.TextSettings.WrappingWidth.ToString();
                    colorPickerText.SelectedColor = cmd.PrimaryFormatting.TextSettings.TextColour.Value;
                    cbWrapping.Checked = cmd.PrimaryFormatting.TextSettings.Wrap.Value;
                }

                // text specific controls
                listFonts.Enabled = cmd.HasText;
                colorPickerText.Enabled = cmd.HasText;
                comboFontSize.Enabled = cmd.HasText;
                comboFontStyle.Enabled = cmd.HasText;
                cbMultiline.Enabled = cmd.HasText;
                cbStrikeThrough.Enabled = cmd.HasText;
                cbUnderline.Enabled = cmd.HasText;
                txtWrapTextAt.Enabled = cmd.HasText;
                colorPickerText.Enabled = cmd.HasText;
                cbWrapping.Enabled = cmd.HasText;
                // shape specific controls
                comboPenEndCap.Enabled = cmd.HasShapes;
                comboPenStartCap.Enabled = cmd.HasShapes;
                comboPenStyle.Enabled = cmd.HasShapes;
                comboPenWidth.Enabled = cmd.HasShapes;
                colorPickerPen.Enabled = cmd.HasShapes;

                if (cmd.PrimaryFormatting.ShapeSettings.PenWidth.HasValue)
                    colorPickerPen.SelectedColor = cmd.PrimaryFormatting.ShapeSettings.PenColour.Value;
                // applicable to all controls
                if (cmd.PrimaryFormatting.GeneralSettings != null)
                {
                    if (cmd.PrimaryFormatting.GeneralSettings.AutoRescales.HasValue)
                        cbAutoRescale.Checked = cmd.PrimaryFormatting.GeneralSettings.AutoRescales.Value;
                    if (cmd.PrimaryFormatting.TextSettings.AutoResizes.HasValue)
                        cbAutoResize.Checked = cmd.PrimaryFormatting.TextSettings.AutoResizes.Value;
                    if (cmd.PrimaryFormatting.GeneralSettings.Deletable.HasValue)
                        cbCannotDelete.Checked = !cmd.PrimaryFormatting.GeneralSettings.Deletable.Value;
                    if (cmd.PrimaryFormatting.GeneralSettings.Printable.HasValue)
                        cbPrintable.Checked = cmd.PrimaryFormatting.GeneralSettings.Printable.Value;
                    if (cmd.PrimaryFormatting.GeneralSettings.Resizable.HasValue)
                        cbResizable.Checked = cmd.PrimaryFormatting.GeneralSettings.Resizable.Value;
                    if (cmd.PrimaryFormatting.GeneralSettings.ResizesRealtime.HasValue)
                        cbResizesRealtime.Checked = cmd.PrimaryFormatting.GeneralSettings.ResizesRealtime.Value;
                    if (cmd.PrimaryFormatting.GeneralSettings.Selectable.HasValue)
                        cbSelectable.Checked = cmd.PrimaryFormatting.GeneralSettings.Selectable.Value;
                    if (cmd.PrimaryFormatting.GeneralSettings.Visible.HasValue)
                        cbVisible.Checked = cmd.PrimaryFormatting.GeneralSettings.Visible.Value;
                }

                if (cmd.PrimaryFormatting.ShapeSettings.Corner.HasValue)
                {
                    txtCornerX.Text = cmd.PrimaryFormatting.ShapeSettings.Corner.Value.Width.ToString();
                    txtCornerY.Text = cmd.PrimaryFormatting.ShapeSettings.Corner.Value.Height.ToString();
                }
                // Shape specific controls
                //angleChooserControl1.Value = cmd.PrimaryFormatting.TextSettings.Angle.Value;
                radioGradientFill.Enabled = cmd.HasShapes;
                radioSolidFill.Enabled = cmd.HasShapes;
                comboGradientType.Enabled = cmd.HasShapes;
                gradEditor.Enabled = cmd.HasShapes;
                colorPickerSolidColour.Enabled = cmd.HasShapes;
                if (cmd.HasShapes && !isSubgraphFormatting)
                {
                    try
                    {
                        if (cmd.PrimaryFormatting.ShapeSettings.PenEndCap.HasValue)
                        {
                            comboPenEndCap.SelectedIndex = comboPenEndCap.FindStringExact(cmd.PrimaryFormatting.ShapeSettings.PenEndCap.Value.ToString());
                        }
                        if (cmd.PrimaryFormatting.ShapeSettings.PenStartCap.HasValue)
                        {
                            comboPenStartCap.SelectedIndex = comboPenStartCap.FindStringExact(cmd.PrimaryFormatting.ShapeSettings.PenStartCap.Value.ToString());
                        }
                        if (cmd.PrimaryFormatting.ShapeSettings.DashStyle.HasValue)
                        {
                            comboPenStyle.SelectedIndex = comboPenStyle.FindStringExact(cmd.PrimaryFormatting.ShapeSettings.DashStyle.Value.ToString());
                        }
                        if (cmd.PrimaryFormatting.ShapeSettings.PenWidth.HasValue)
                        {
                            comboPenWidth.SelectedIndex = comboPenWidth.FindStringExact(cmd.PrimaryFormatting.ShapeSettings.PenWidth.Value.ToString().Replace(".0", ""));
                        }
                        if (cmd.PrimaryFormatting.ShapeSettings.IsGradient.HasValue)
                        {
                            radioSolidFill.Checked = false;
                            radioGradientFill.Checked = true;

                            gradEditor.GetMarker(0).Color = gradEditor.Gradient.ColorBlend.Colors[0] = cmd.PrimaryFormatting.ShapeSettings.GradientStartColour.Value;
                            gradEditor.GetMarker(1).Color = gradEditor.Gradient.ColorBlend.Colors[1] = cmd.PrimaryFormatting.ShapeSettings.GradientEndColour.Value;
                            // cmd.PrimaryFormatting.ShapeSettings.GradientStartColour.Value;
                            // cmd.PrimaryFormatting.ShapeSettings.GradientEndColour.Value;

                            if (cmd.PrimaryFormatting.ShapeSettings.GradientType.HasValue)
                            {
                                if (cmd.PrimaryFormatting.ShapeSettings.GradientType.Value != GradientType.Ellipse)
                                {
                                    gradEditor.Gradient.GradientDirection = (LinearGradientMode)Enum.Parse(typeof(LinearGradientMode), cmd.PrimaryFormatting.ShapeSettings.GradientType.Value.ToString());
                                }

                                comboGradientType.SelectedItem = ((GradientType)cmd.PrimaryFormatting.ShapeSettings.GradientType).ToString();
                            }
                        }
                        else
                        {
                            if (cmd.PrimaryFormatting.ShapeSettings.FillColour.HasValue)
                            {
                                radioSolidFill.Checked = true;
                                colorPickerSolidColour.SelectedColor = cmd.PrimaryFormatting.ShapeSettings.FillColour.Value;
                            }
                        }
                        int count = 0;
                        ShapeFormatting sformatting = null;
                        foreach (KeyValuePair<GoObject, FormattingContainer> kvp in cmd.originalSettings)
                        {
                            if (kvp.Key is GoShape && (!(kvp.Key is GraphNodeGrid)))
                            {
                                count++;
                                sformatting = kvp.Value.ShapeSettings;
                            }
                        }
                        if (count == 1 && sformatting != null)
                        {
                            if (sformatting.IsGradient.HasValue)
                            {
                                radioSolidFill.Checked = false;
                                radioGradientFill.Checked = true;
                                gradEditor.Gradient.ColorBlend.Colors[0] = sformatting.GradientStartColour.Value;
                                gradEditor.Gradient.ColorBlend.Colors[1] = sformatting.GradientEndColour.Value;
                                gradEditor.GetMarker(0).Color = sformatting.GradientStartColour.Value;
                                gradEditor.GetMarker(1).Color = sformatting.GradientEndColour.Value;
                            }
                            else
                            {
                                radioSolidFill.Checked = true;
                                radioGradientFill.Checked = false;
                            }
                        }
                    }
                    catch
                    {
                        radioSolidFill.Checked = true;
                        radioGradientFill.Checked = false;
                    }
                    /*radioGradientFill.Checked = cmd.PrimaryFormatting.ShapeSettings.IsGradient.Value;
                    if (cmd.PrimaryFormatting.ShapeSettings.FillColour.HasValue)
                    {
                        radioSolidFill.Checked = true;
                        radioGradientFill.Checked = false;
                    }*/
                }
                if (propertyGrid.SelectedObject is GoShape && (propertyGrid.SelectedObject as GoObject).Parent != null)
                {
                    if ((propertyGrid.SelectedObject as GoShape).Parent is LegendItem)
                    {
                        radioSolidFill.Visible = false; //legend items must have gradients because I am too lazy to code solid brushes into legends
                        colorPickerSolidColour.Visible = false;
                    }
                    else if ((propertyGrid.SelectedObject as GoShape).Parent.Parent is LegendItem)
                    {
                        radioSolidFill.Visible = false; //legend items must have gradients because I am too lazy to code solid brushes into legends
                        colorPickerSolidColour.Visible = false;
                    }
                }
                if (propertyGrid.SelectedObject is GoGroup)
                    if ((propertyGrid.SelectedObject as GoGroup).Parent is LegendItem)
                    {
                        radioSolidFill.Visible = false; //legend items must have gradients because I am too lazy to code solid brushes into legends
                        colorPickerSolidColour.Visible = false;
                    }

                if (isSubgraphFormatting)
                {
                    radioGradientFill.Checked = false;
                    radioGradientFill.Enabled = false;
                    comboGradientType.Enabled = false;
                    gradEditor.Enabled = false;
                    radioSolidFill.Checked = true;
                    radioSolidFill.Enabled = true;

                    if (propertyGrid.SelectedObject is SubgraphNode)
                        colorPickerSolidColour.SelectedColor = (propertyGrid.SelectedObject as SubgraphNode).BackgroundColor;
                    if (propertyGrid.SelectedObject is MindMapNode)
                    {
                        MindMapNode mmNode = (propertyGrid.SelectedObject as MindMapNode);

                        listFonts.SelectedIndex = listFonts.FindStringExact(mmNode.Label.FamilyName);
                        colorPickerText.SelectedColor = mmNode.Label.TextColor;
                        comboFontSize.SelectedIndex = comboFontSize.FindStringExact(mmNode.Label.FontSize.ToString().Replace(".0", ""));

                        bool IsItalic = mmNode.Label.Italic;
                        bool IsBold = mmNode.Label.Bold;
                        if (IsItalic && IsBold)
                        {
                            comboFontStyle.SelectedIndex = comboFontStyle.FindStringExact("Bold, Italic");
                        }
                        if (IsItalic && !IsBold)
                            comboFontStyle.SelectedIndex = comboFontStyle.FindStringExact("Italic");
                        if (!IsItalic && IsBold)
                            comboFontStyle.SelectedIndex = comboFontStyle.FindStringExact("Bold");
                        if (!IsItalic && !IsBold)
                            comboFontStyle.SelectedIndex = comboFontStyle.FindStringExact("Regular");

                        cbMultiline.Checked = mmNode.Label.Multiline;
                        cbStrikeThrough.Checked = mmNode.Label.StrikeThrough;
                        cbUnderline.Checked = mmNode.Label.Underline;

                        txtWrapTextAt.Text = mmNode.Label.WrappingWidth.ToString();
                        txtWrapTextAt.Enabled = true;
                        cbWrapping.Checked = mmNode.Label.Wrapping;
                        cbWrapping.Enabled = false;

                        if (mmNode.MyShape == null || mmNode.Shape == null)
                            colorPickerSolidColour.SelectedColor = (mmNode.Brush as SolidBrush).Color;
                        else
                        {
                            radioGradientFill.Checked = true;
                            radioGradientFill.Enabled = false;
                            comboGradientType.SelectedItem = "Horizontal";
                            comboGradientType.Enabled = false;
                            gradEditor.Enabled = true;
                            radioSolidFill.Checked = false;
                            radioSolidFill.Enabled = false;

                            gradEditor.Gradient.ColorBlend.Colors[0] = mmNode.ShapeBrush.InnerColor;
                            gradEditor.Gradient.ColorBlend.Colors[1] = mmNode.ShapeBrush.OuterColor;
                            gradEditor.GetMarker(0).Color = mmNode.ShapeBrush.InnerColor;
                            gradEditor.GetMarker(1).Color = mmNode.ShapeBrush.OuterColor;
                        }
                    }
                    colorPickerSolidColour.Enabled = true;
                }
            }
            IsInitialising = false;
        }

        /// <summary>
        /// Handles the Paint event of the pnlCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        private void pnlCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(50, 50, 100, 100);
            gradEditor.Gradient.FillRectangle(e.Graphics, rect);
            GraphicsPath gp = new GraphicsPath();
            gp.StartFigure();
            gp.AddEllipse(new Rectangle(250, 50, 100, 100));
            gp.CloseAllFigures();
            gradEditor.Gradient.FillPath(e.Graphics, gp);
        }

        private void StoreItem(GoObject obj)
        {
#if DEBUG
            // Console.WriteLine(obj.ToString());
#endif
            if (obj is GoText)
            {
                GoText txt = obj as GoText;
                HasText = true;
                bool added = false;

                if (txt is BoundLabel)
                {
                    BoundLabel lbl = txt as BoundLabel;
                    if (lbl.originalEditable.HasValue)
                    {
                        if ((lbl.originalEditable.Value && txt.EditorStyle == GoTextEditorStyle.TextBox) || (txt.ParentNode is ArtefactNode) || (txt.Parent is SubgraphNode) || txt.Parent is ValueChain)
                        {
                            AddSelectedObject(obj);
                            added = true;
                        }
                    }
                }

                if (!added)
                {
                    if ((txt.Editable && txt.EditorStyle == GoTextEditorStyle.TextBox) || (txt.ParentNode is ArtefactNode) || (txt.Parent is SubgraphNode) || txt.Parent is ValueChain)
                    {
                        AddSelectedObject(obj);
                    }
                }
            }
            else
            {
                if (obj is GoShape)
                {
                    if (obj is IBehaviourShape)
                    {
                        IBehaviourShape shape = obj as IBehaviourShape;
                        GradientBehaviour gbehaviour =
                            shape.Manager.GetExistingBehaviour(typeof(GradientBehaviour)) as GradientBehaviour;
                        if (gbehaviour != null || (!(obj is GradientRoundedRectangle)))
                            AddSelectedObject(obj);
                    }
                }
                else
                {
                    if (!(obj is GoPort))
                        AddSelectedObject(obj);
                }
                if (obj is GoGroup)
                {
                    bool skipNodes = (obj is ILinkedContainer) || (obj is TreeSubGraph);
                    foreach (GoObject o in obj as GoGroup)
                    {
                        if (!(o is GoPort))
                        {
                            if (o is GoNode || o is ValueChain)
                            {
                                if (skipNodes)
                                {
                                    // do nothing
                                }
                                else
                                {
                                    StoreItem(o);
                                }
                            }
                            else
                            {
                                StoreItem(o);
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Stores the properties.
        /// </summary>
        /// <param name="collection">The collection.</param>
        private void StoreProperties(List<GoObject> collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                GoObject o = collection[i];
                if (o is GoText)
                {
                    HasText = true;
                }
                if (o is GoShape)
                {
                    hasShapes = true;
                }
                if (o is GoGroup)
                {
                    bool skipNodes = (o is ILinkedContainer || o is TreeSubGraph);
                    List<GoObject> children = new List<GoObject>();
                    GoGroupEnumerator groupEnum = (o as GoGroup).GetEnumerator();
                    while (groupEnum.MoveNext())
                    {
                        if (!(groupEnum.Current is GoPort))
                        {
                            //    if (groupEnum.Current is GoNode || groupEnum.Current is ValueChain)
                            {
                                if (skipNodes)
                                {
                                    if (groupEnum.Current is GoText && groupEnum.Current.ParentNode is MappingCell)
                                    {
                                        children.Add(groupEnum.Current);
                                        AddSelectedObject(groupEnum.Current);
                                    }
                                    // do nothing
                                }
                                else
                                {
                                    children.Add(groupEnum.Current);
                                    AddSelectedObject(groupEnum.Current);
                                }
                            }

                        }
                    }
                    StoreProperties(children);
                }
            }
        }

        // COPYFORMATTING 
        private void StoreSettingsForCollection(GoCollectionEnumerator colEnum)
        {
            while (colEnum.MoveNext())
            {
                if (!(colEnum.Current is GoPort))
                {
                    StoreItem(colEnum.Current);

                }
            }
        }

        /// <summary>
        /// Updates the settings.
        /// </summary>
        private void UpdateSettings()
        {
            // do nothing
        }

        #endregion Methods

    }
}