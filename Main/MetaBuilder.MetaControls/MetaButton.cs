using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.MetaControls
{
    public class MetaButton : Ascend.Windows.Forms.GradientNavigationButton
    {
        public MetaButton()
        {
            //normal
            this.GradientHighColor = System.Drawing.SystemColors.ButtonHighlight;
            this.GradientLowColor = System.Drawing.Color.DarkGray;

            //hover
            //this.HighlightGradientHighColor = System.Drawing.SystemColors.MenuHighlight;
            //this.HighlightGradientLowColor = System.Drawing.Color.DarkGray;

            //click
            //this.ActiveGradientHighColor = System.Drawing.SystemColors.MenuHighlight;
            //this.ActiveGradientLowColor = System.Drawing.Color.DarkGray;

            this.ActiveOnClick = true;
            this.StayActiveAfterClick = false;

            //this.AntiAlias = true;
            //this.RenderMode = Ascend.Windows.Forms.RenderMode.Glass;
            this.RenderMode = Ascend.Windows.Forms.RenderMode.Gradient;

            this.CornerRadius = new Ascend.CornerRadius(2);

            this.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        }
    }
}
