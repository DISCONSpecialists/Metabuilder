using MetaBuilder.Docking;
using System.Collections.Generic;
using System;

namespace MetaBuilder.UIControls.GraphingUI
{
    public class MetaPalette : MetaBuilder.Graphing.Containers.GraphPalette
    {
        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs evt)
        {
            if (this.LastInput.Control)
                return;
            base.OnMouseWheel(evt);
        }
        protected override void OnGotFocus(System.EventArgs evt)
        {
            base.OnGotFocus(evt);
        }

        private bool loggedPaintProblem = false;
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs evt)
        {
            try
            {
                base.OnPaint(evt);
            }
            catch (Exception ex)
            {
                if (loggedPaintProblem)
                    return;
                Core.Log.WriteLog(ex.ToString());
                loggedPaintProblem = true;
            }
        }

    }

    public partial class PaletteContainer : DockContent
    {

        #region Constructors (1)

        public PaletteContainer()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Overrides on DockContent parent class
        // When setting the text property of the form, also set the tabtext property
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                TabText = value;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (!Palette.IsDisposed)
                Palette.Dispose();
        }

        #endregion

        protected override void OnGotFocus(System.EventArgs e)
        {
            base.OnGotFocus(e);
        }

        public List<string> Classes = new List<string>();
    }
}