namespace MetaBuilder.UIControls.GraphingUI
{
    partial class PaletteContainer
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
            this.Palette = new MetaPalette();
            this.SuspendLayout();
            // 
            // Palette
            // 
            this.Palette.AllowDelete = false;
            this.Palette.AllowLink = false;
            this.Palette.AllowMove = false;
            this.Palette.AllowReshape = false;
            this.Palette.AllowResize = false;
            this.Palette.ArrowMoveLarge = 10F;
            this.Palette.ArrowMoveSmall = 1F;
            this.Palette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this.Palette.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Palette.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Palette.DocScale = 0.65F;
            this.Palette.GridCellSizeHeight = 60F;
            this.Palette.GridCellSizeWidth = 60F;
            this.Palette.GridOriginX = 20F;
            this.Palette.GridOriginY = 5F;
            this.Palette.IsDirty = true;
            this.Palette.Location = new System.Drawing.Point(0, 0);
            this.Palette.Name = "Palette";
            this.Palette.ShowHorizontalScrollBar = Northwoods.Go.GoViewScrollBarVisibility.Hide;
            this.Palette.ShowsNegativeCoordinates = false;
            this.Palette.Size = new System.Drawing.Size(203, 273);
            this.Palette.TabIndex = 0;
            this.Palette.Text = "Palette";
            // 
            // PaletteContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(203, 273);
            this.Controls.Add(this.Palette);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PaletteContainer";
            this.TabText = "Stencils";
            this.Text = "Stencils";
            this.ResumeLayout(false);

        }

        #endregion

        internal MetaPalette Palette;

    }
}