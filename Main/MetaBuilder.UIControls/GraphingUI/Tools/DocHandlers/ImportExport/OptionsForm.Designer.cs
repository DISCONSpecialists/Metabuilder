namespace MetaBuilder.UIControls.GraphingUI.Tools.DocHandlers.ImportExport
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gradientCaption1 = new Ascend.Windows.Forms.GradientCaption();
            this.gradientPanel1 = new Ascend.Windows.Forms.GradientPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkImportToDatabaseText = new System.Windows.Forms.LinkLabel();
            this.lnkImportToDatabaseExcel = new System.Windows.Forms.LinkLabel();
            this.lnkImportToDiagramText = new System.Windows.Forms.LinkLabel();
            this.gradientCaption2 = new Ascend.Windows.Forms.GradientCaption();
            this.gradientPanel2 = new Ascend.Windows.Forms.GradientPanel();
            this.lnkExportFromDatabaseTextIndented = new System.Windows.Forms.LinkLabel();
            this.lnkExportFromDatabaseTextNumbered = new System.Windows.Forms.LinkLabel();
            this.lnkExportFromDatabaseTextIndentedNumbered = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lnkExportFromDatabaseExcel = new System.Windows.Forms.LinkLabel();
            this.lnkExportFromDiagramTextIndented = new System.Windows.Forms.LinkLabel();
            this.lnkExportFromDiagramTextNumbered = new System.Windows.Forms.LinkLabel();
            this.lnkExportFromDiagramTextIndentedNumbered = new System.Windows.Forms.LinkLabel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.gradientPanel1.SuspendLayout();
            this.gradientPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gradientCaption1
            // 
            this.gradientCaption1.Location = new System.Drawing.Point(0, 1);
            this.gradientCaption1.Name = "gradientCaption1";
            this.gradientCaption1.Size = new System.Drawing.Size(220, 24);
            this.gradientCaption1.TabIndex = 0;
            this.gradientCaption1.Text = "Import...";
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.Controls.Add(this.label2);
            this.gradientPanel1.Controls.Add(this.label1);
            this.gradientPanel1.Controls.Add(this.lnkImportToDatabaseText);
            this.gradientPanel1.Controls.Add(this.lnkImportToDatabaseExcel);
            this.gradientPanel1.Controls.Add(this.lnkImportToDiagramText);
            this.gradientPanel1.Location = new System.Drawing.Point(0, 25);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(220, 139);
            this.gradientPanel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "To New Diagram:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "To Database:";
            // 
            // lnkImportToDatabaseText
            // 
            this.lnkImportToDatabaseText.AutoSize = true;
            this.lnkImportToDatabaseText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkImportToDatabaseText.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkImportToDatabaseText.Location = new System.Drawing.Point(21, 80);
            this.lnkImportToDatabaseText.Name = "lnkImportToDatabaseText";
            this.lnkImportToDatabaseText.Size = new System.Drawing.Size(79, 13);
            this.lnkImportToDatabaseText.TabIndex = 3;
            this.lnkImportToDatabaseText.TabStop = true;
            this.lnkImportToDatabaseText.Text = "Text - Indented";
            this.lnkImportToDatabaseText.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkImportToDatabaseText_LinkClicked);
            // 
            // lnkImportToDatabaseExcel
            // 
            this.lnkImportToDatabaseExcel.AutoSize = true;
            this.lnkImportToDatabaseExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkImportToDatabaseExcel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkImportToDatabaseExcel.Location = new System.Drawing.Point(21, 93);
            this.lnkImportToDatabaseExcel.Name = "lnkImportToDatabaseExcel";
            this.lnkImportToDatabaseExcel.Size = new System.Drawing.Size(96, 13);
            this.lnkImportToDatabaseExcel.TabIndex = 4;
            this.lnkImportToDatabaseExcel.TabStop = true;
            this.lnkImportToDatabaseExcel.Text = "Excel Spreadsheet";
            this.lnkImportToDatabaseExcel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkImportToDatabaseExcel_LinkClicked);
            // 
            // lnkImportToDiagramText
            // 
            this.lnkImportToDiagramText.AutoSize = true;
            this.lnkImportToDiagramText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkImportToDiagramText.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkImportToDiagramText.Location = new System.Drawing.Point(21, 15);
            this.lnkImportToDiagramText.Name = "lnkImportToDiagramText";
            this.lnkImportToDiagramText.Size = new System.Drawing.Size(79, 13);
            this.lnkImportToDiagramText.TabIndex = 3;
            this.lnkImportToDiagramText.TabStop = true;
            this.lnkImportToDiagramText.Text = "Text - Indented";
            this.lnkImportToDiagramText.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkImportToDiagramText_LinkClicked);
            // 
            // gradientCaption2
            // 
            this.gradientCaption2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.gradientCaption2.ForeColor = System.Drawing.Color.Black;
            this.gradientCaption2.GradientHighColor = System.Drawing.Color.Moccasin;
            this.gradientCaption2.GradientLowColor = System.Drawing.Color.DarkOrange;
            this.gradientCaption2.Location = new System.Drawing.Point(226, 1);
            this.gradientCaption2.Name = "gradientCaption2";
            this.gradientCaption2.Size = new System.Drawing.Size(220, 24);
            this.gradientCaption2.TabIndex = 0;
            this.gradientCaption2.Text = "Export...";
            // 
            // gradientPanel2
            // 
            this.gradientPanel2.Controls.Add(this.lnkExportFromDatabaseTextIndented);
            this.gradientPanel2.Controls.Add(this.lnkExportFromDatabaseTextNumbered);
            this.gradientPanel2.Controls.Add(this.lnkExportFromDatabaseTextIndentedNumbered);
            this.gradientPanel2.Controls.Add(this.label4);
            this.gradientPanel2.Controls.Add(this.label3);
            this.gradientPanel2.Controls.Add(this.lnkExportFromDatabaseExcel);
            this.gradientPanel2.Controls.Add(this.lnkExportFromDiagramTextIndented);
            this.gradientPanel2.Controls.Add(this.lnkExportFromDiagramTextNumbered);
            this.gradientPanel2.Controls.Add(this.lnkExportFromDiagramTextIndentedNumbered);
            this.gradientPanel2.Location = new System.Drawing.Point(226, 25);
            this.gradientPanel2.Name = "gradientPanel2";
            this.gradientPanel2.Size = new System.Drawing.Size(220, 139);
            this.gradientPanel2.TabIndex = 1;
            // 
            // lnkExportFromDatabaseTextIndented
            // 
            this.lnkExportFromDatabaseTextIndented.AutoSize = true;
            this.lnkExportFromDatabaseTextIndented.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkExportFromDatabaseTextIndented.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkExportFromDatabaseTextIndented.Location = new System.Drawing.Point(15, 93);
            this.lnkExportFromDatabaseTextIndented.Name = "lnkExportFromDatabaseTextIndented";
            this.lnkExportFromDatabaseTextIndented.Size = new System.Drawing.Size(79, 13);
            this.lnkExportFromDatabaseTextIndented.TabIndex = 9;
            this.lnkExportFromDatabaseTextIndented.TabStop = true;
            this.lnkExportFromDatabaseTextIndented.Text = "Text - Indented";
            this.lnkExportFromDatabaseTextIndented.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExportFromDatabaseTextIndented_LinkClicked);
            // 
            // lnkExportFromDatabaseTextNumbered
            // 
            this.lnkExportFromDatabaseTextNumbered.AutoSize = true;
            this.lnkExportFromDatabaseTextNumbered.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkExportFromDatabaseTextNumbered.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkExportFromDatabaseTextNumbered.Location = new System.Drawing.Point(15, 106);
            this.lnkExportFromDatabaseTextNumbered.Name = "lnkExportFromDatabaseTextNumbered";
            this.lnkExportFromDatabaseTextNumbered.Size = new System.Drawing.Size(86, 13);
            this.lnkExportFromDatabaseTextNumbered.TabIndex = 8;
            this.lnkExportFromDatabaseTextNumbered.TabStop = true;
            this.lnkExportFromDatabaseTextNumbered.Text = "Text - Numbered";
            this.lnkExportFromDatabaseTextNumbered.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExportFromDatabaseTextNumbered_LinkClicked);
            // 
            // lnkExportFromDatabaseTextIndentedNumbered
            // 
            this.lnkExportFromDatabaseTextIndentedNumbered.AutoSize = true;
            this.lnkExportFromDatabaseTextIndentedNumbered.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkExportFromDatabaseTextIndentedNumbered.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkExportFromDatabaseTextIndentedNumbered.Location = new System.Drawing.Point(15, 119);
            this.lnkExportFromDatabaseTextIndentedNumbered.Name = "lnkExportFromDatabaseTextIndentedNumbered";
            this.lnkExportFromDatabaseTextIndentedNumbered.Size = new System.Drawing.Size(140, 13);
            this.lnkExportFromDatabaseTextIndentedNumbered.TabIndex = 10;
            this.lnkExportFromDatabaseTextIndentedNumbered.TabStop = true;
            this.lnkExportFromDatabaseTextIndentedNumbered.Text = "Text - Indented && Numbered";
            this.lnkExportFromDatabaseTextIndentedNumbered.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExportFromDatabaseTextIndentedNumbered_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "From Database:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "From Current Diagram:";
            // 
            // lnkExportFromDatabaseExcel
            // 
            this.lnkExportFromDatabaseExcel.AutoSize = true;
            this.lnkExportFromDatabaseExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkExportFromDatabaseExcel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkExportFromDatabaseExcel.Location = new System.Drawing.Point(15, 80);
            this.lnkExportFromDatabaseExcel.Name = "lnkExportFromDatabaseExcel";
            this.lnkExportFromDatabaseExcel.Size = new System.Drawing.Size(96, 13);
            this.lnkExportFromDatabaseExcel.TabIndex = 4;
            this.lnkExportFromDatabaseExcel.TabStop = true;
            this.lnkExportFromDatabaseExcel.Text = "Excel Spreadsheet";
            this.lnkExportFromDatabaseExcel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExportFromDatabaseExcel_LinkClicked);
            // 
            // lnkExportFromDiagramTextIndented
            // 
            this.lnkExportFromDiagramTextIndented.AutoSize = true;
            this.lnkExportFromDiagramTextIndented.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkExportFromDiagramTextIndented.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkExportFromDiagramTextIndented.Location = new System.Drawing.Point(14, 15);
            this.lnkExportFromDiagramTextIndented.Name = "lnkExportFromDiagramTextIndented";
            this.lnkExportFromDiagramTextIndented.Size = new System.Drawing.Size(79, 13);
            this.lnkExportFromDiagramTextIndented.TabIndex = 3;
            this.lnkExportFromDiagramTextIndented.TabStop = true;
            this.lnkExportFromDiagramTextIndented.Text = "Text - Indented";
            this.lnkExportFromDiagramTextIndented.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExportFromDiagramTextIndented_LinkClicked);
            // 
            // lnkExportFromDiagramTextNumbered
            // 
            this.lnkExportFromDiagramTextNumbered.AutoSize = true;
            this.lnkExportFromDiagramTextNumbered.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkExportFromDiagramTextNumbered.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkExportFromDiagramTextNumbered.Location = new System.Drawing.Point(13, 28);
            this.lnkExportFromDiagramTextNumbered.Name = "lnkExportFromDiagramTextNumbered";
            this.lnkExportFromDiagramTextNumbered.Size = new System.Drawing.Size(86, 13);
            this.lnkExportFromDiagramTextNumbered.TabIndex = 2;
            this.lnkExportFromDiagramTextNumbered.TabStop = true;
            this.lnkExportFromDiagramTextNumbered.Text = "Text - Numbered";
            this.lnkExportFromDiagramTextNumbered.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExportFromDiagramTextNumbered_LinkClicked);
            // 
            // lnkExportFromDiagramTextIndentedNumbered
            // 
            this.lnkExportFromDiagramTextIndentedNumbered.AutoSize = true;
            this.lnkExportFromDiagramTextIndentedNumbered.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkExportFromDiagramTextIndentedNumbered.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkExportFromDiagramTextIndentedNumbered.Location = new System.Drawing.Point(13, 41);
            this.lnkExportFromDiagramTextIndentedNumbered.Name = "lnkExportFromDiagramTextIndentedNumbered";
            this.lnkExportFromDiagramTextIndentedNumbered.Size = new System.Drawing.Size(140, 13);
            this.lnkExportFromDiagramTextIndentedNumbered.TabIndex = 5;
            this.lnkExportFromDiagramTextIndentedNumbered.TabStop = true;
            this.lnkExportFromDiagramTextIndentedNumbered.Text = "Text - Indented && Numbered";
            this.lnkExportFromDiagramTextIndentedNumbered.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExportFromDiagramTextIndentedNumbered_LinkClicked);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(0, 170);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(446, 11);
            this.progressBar1.TabIndex = 2;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 181);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.gradientPanel2);
            this.Controls.Add(this.gradientPanel1);
            this.Controls.Add(this.gradientCaption2);
            this.Controls.Add(this.gradientCaption1);
            this.Name = "OptionsForm";
            this.Text = "Import / Export Options";
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            this.gradientPanel2.ResumeLayout(false);
            this.gradientPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Ascend.Windows.Forms.GradientCaption gradientCaption1;
        private Ascend.Windows.Forms.GradientPanel gradientPanel1;
        private Ascend.Windows.Forms.GradientCaption gradientCaption2;
        private Ascend.Windows.Forms.GradientPanel gradientPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lnkImportToDatabaseText;
        private System.Windows.Forms.LinkLabel lnkImportToDatabaseExcel;
        private System.Windows.Forms.LinkLabel lnkImportToDiagramText;
        private System.Windows.Forms.LinkLabel lnkExportFromDatabaseExcel;
        private System.Windows.Forms.LinkLabel lnkExportFromDiagramTextIndented;
        private System.Windows.Forms.LinkLabel lnkExportFromDiagramTextNumbered;
        private System.Windows.Forms.LinkLabel lnkExportFromDiagramTextIndentedNumbered;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnkExportFromDatabaseTextIndented;
        private System.Windows.Forms.LinkLabel lnkExportFromDatabaseTextNumbered;
        private System.Windows.Forms.LinkLabel lnkExportFromDatabaseTextIndentedNumbered;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;

    }
}