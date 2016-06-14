namespace MetaBuilder.Graphing.Layout
{
    partial class LayerDigraphLayoutDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
       

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.left = new System.Windows.Forms.RadioButton();
            this.down = new System.Windows.Forms.RadioButton();
            this.aggressive = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.up = new System.Windows.Forms.RadioButton();
            this.dfsout = new System.Windows.Forms.RadioButton();
            this.right = new System.Windows.Forms.RadioButton();
            this.dfsin = new System.Windows.Forms.RadioButton();
            this.naive = new System.Windows.Forms.RadioButton();
            this.greedy = new System.Windows.Forms.RadioButton();
            this.layer = new System.Windows.Forms.TextBox();
            this.sink = new System.Windows.Forms.RadioButton();
            this.iter = new System.Windows.Forms.TextBox();
            this.source = new System.Windows.Forms.RadioButton();
            this.column = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.depthFirst = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.length = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // left
            // 
            this.left.Location = new System.Drawing.Point(104, 24);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(64, 24);
            this.left.TabIndex = 2;
            this.left.Text = "&Left";
            // 
            // down
            // 
            this.down.Location = new System.Drawing.Point(24, 48);
            this.down.Name = "down";
            this.down.Size = new System.Drawing.Size(56, 24);
            this.down.TabIndex = 1;
            this.down.Text = "&Down";
            // 
            // aggressive
            // 
            this.aggressive.Location = new System.Drawing.Point(16, 48);
            this.aggressive.Name = "aggressive";
            this.aggressive.Size = new System.Drawing.Size(88, 24);
            this.aggressive.TabIndex = 2;
            this.aggressive.Text = "Aggressive";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(360, 304);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(72, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            // 
            // label1
            // 
            this.label1.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Iterations";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "layerSpacing";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "columnSpacing";
            // 
            // up
            // 
            this.up.Checked = true;
            this.up.Location = new System.Drawing.Point(24, 24);
            this.up.Name = "up";
            this.up.Size = new System.Drawing.Size(56, 24);
            this.up.TabIndex = 0;
            this.up.TabStop = true;
            this.up.Text = "&Up";
            // 
            // dfsout
            // 
            this.dfsout.Checked = true;
            this.dfsout.Location = new System.Drawing.Point(16, 40);
            this.dfsout.Name = "dfsout";
            this.dfsout.Size = new System.Drawing.Size(168, 24);
            this.dfsout.TabIndex = 1;
            this.dfsout.TabStop = true;
            this.dfsout.Text = "Depth First Search Outward";
            // 
            // right
            // 
            this.right.Location = new System.Drawing.Point(104, 48);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(64, 24);
            this.right.TabIndex = 3;
            this.right.Text = "&Right";
            // 
            // dfsin
            // 
            this.dfsin.Location = new System.Drawing.Point(16, 64);
            this.dfsin.Name = "dfsin";
            this.dfsin.Size = new System.Drawing.Size(168, 24);
            this.dfsin.TabIndex = 2;
            this.dfsin.Text = "Depth First Search Inward";
            // 
            // naive
            // 
            this.naive.Location = new System.Drawing.Point(16, 16);
            this.naive.Name = "naive";
            this.naive.Size = new System.Drawing.Size(168, 24);
            this.naive.TabIndex = 0;
            this.naive.Text = "Naive";
            // 
            // greedy
            // 
            this.greedy.Location = new System.Drawing.Point(16, 24);
            this.greedy.Name = "greedy";
            this.greedy.Size = new System.Drawing.Size(112, 24);
            this.greedy.TabIndex = 0;
            this.greedy.Text = "Greedy";
            // 
            // layer
            // 
            this.layer.Location = new System.Drawing.Point(104, 24);
            this.layer.Name = "layer";
            this.layer.Size = new System.Drawing.Size(72, 20);
            this.layer.TabIndex = 2;
            this.layer.Text = "20";
            this.layer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // sink
            // 
            this.sink.Location = new System.Drawing.Point(16, 16);
            this.sink.Name = "sink";
            this.sink.Size = new System.Drawing.Size(128, 24);
            this.sink.TabIndex = 0;
            this.sink.Text = "Longest Path Sink";
            // 
            // iter
            // 
            this.iter.Location = new System.Drawing.Point(112, 24);
            this.iter.Name = "iter";
            this.iter.Size = new System.Drawing.Size(72, 20);
            this.iter.TabIndex = 0;
            this.iter.Text = "4";
            this.iter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // source
            // 
            this.source.Location = new System.Drawing.Point(16, 40);
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(128, 24);
            this.source.TabIndex = 1;
            this.source.Text = "Longest Path Source";
            // 
            // column
            // 
            this.column.Location = new System.Drawing.Point(104, 48);
            this.column.Name = "column";
            this.column.Size = new System.Drawing.Size(72, 20);
            this.column.TabIndex = 2;
            this.column.Text = "20";
            this.column.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(16, 304);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(72, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "&OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // depthFirst
            // 
            this.depthFirst.Checked = true;
            this.depthFirst.Location = new System.Drawing.Point(16, 48);
            this.depthFirst.Name = "depthFirst";
            this.depthFirst.Size = new System.Drawing.Size(120, 24);
            this.depthFirst.TabIndex = 1;
            this.depthFirst.TabStop = true;
            this.depthFirst.Text = "Depth First Search";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                            this.depthFirst,
                                                                            this.greedy});
            this.groupBox1.Location = new System.Drawing.Point(16, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cycle Remove Options";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                            this.aggressive,
                                                                            this.label1,
                                                                            this.iter});
            this.groupBox2.Location = new System.Drawing.Point(232, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 80);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Crossing Reduction Options";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                            this.length,
                                                                            this.sink,
                                                                            this.source});
            this.groupBox3.Location = new System.Drawing.Point(16, 96);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 96);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Layering Options";
            // 
            // length
            // 
            this.length.Checked = true;
            this.length.Location = new System.Drawing.Point(16, 64);
            this.length.Name = "length";
            this.length.Size = new System.Drawing.Size(128, 24);
            this.length.TabIndex = 2;
            this.length.TabStop = true;
            this.length.Text = "Optimal Link Length";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                            this.dfsout,
                                                                            this.dfsin,
                                                                            this.naive});
            this.groupBox4.Location = new System.Drawing.Point(16, 200);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 96);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Initialize Options";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                            this.groupBox7,
                                                                            this.groupBox6});
            this.groupBox5.Location = new System.Drawing.Point(232, 96);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 200);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Layout Options";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                            this.right,
                                                                            this.left,
                                                                            this.down,
                                                                            this.up});
            this.groupBox7.Location = new System.Drawing.Point(8, 112);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(184, 80);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Direction";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                            this.column,
                                                                            this.layer,
                                                                            this.label3,
                                                                            this.label2});
            this.groupBox6.Location = new System.Drawing.Point(8, 16);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(184, 80);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Spacing";
            // 
            // LayerDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(448, 333);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                  this.cancelButton,
                                                                  this.okButton,
                                                                  this.groupBox5,
                                                                  this.groupBox4,
                                                                  this.groupBox3,
                                                                  this.groupBox2,
                                                                  this.groupBox1});
            this.Name = "LayerDialog";
            this.Text = "Layered-Digraph Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

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


    }
}