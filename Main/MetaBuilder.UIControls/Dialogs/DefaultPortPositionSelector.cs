using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class DefaultPortPositionSelector : Form
    {
        List<Label> FromLabels = new List<Label>();
        List<Label> ToLabels = new List<Label>();

        Label currentFrom = null;
        Label currentTo = null;

        public DefaultPortPositionSelector()
        {
            InitializeComponent();

            FromLabels.Add(labelFromTopLeft);
            FromLabels.Add(labelFromBottomLeft);
            FromLabels.Add(labelFromLeftTop);
            FromLabels.Add(labelFromRightBottom);
            FromLabels.Add(labelFromTopRight);
            FromLabels.Add(labelFromBottomRight);
            FromLabels.Add(labelFromLeftBottom);
            FromLabels.Add(labelFromRightTop);
            FromLabels.Add(labelFromTop);
            FromLabels.Add(labelFromBottom);
            FromLabels.Add(labelFromLeft);
            FromLabels.Add(labelFromRight);

            ToLabels.Add(labelToTopLeft);
            ToLabels.Add(labelToBottomLeft);
            ToLabels.Add(labelToLeftTop);
            ToLabels.Add(labelToRightBottom);
            ToLabels.Add(labelToTopRight);
            ToLabels.Add(labelToBottomRight);
            ToLabels.Add(labelToLeftBottom);
            ToLabels.Add(labelToRightTop);
            ToLabels.Add(labelToTop);
            ToLabels.Add(labelToBottom);
            ToLabels.Add(labelToLeft);
            ToLabels.Add(labelToRight);

            foreach (Label c in FromLabels)
            {
                string name = c.Name.Replace("labelFrom", "");
                if (name == Core.Variables.Instance.DefaultFromPort)
                {
                    c.BackColor = Color.Yellow;
                    currentFrom = c as Label;
                    break;
                }
            }

            foreach (Label c in ToLabels)
            {
                string name = c.Name.Replace("labelTo", "");
                if (name == Core.Variables.Instance.DefaultToPort)
                {
                    c.BackColor = Color.Yellow;
                    currentTo = c as Label;
                    break;
                }
            }
        }

        private void labelFrom_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl == currentFrom)
                return;

            string name = lbl.Name.Replace("labelFrom", "");
            if (currentFrom != null)
                currentFrom.BackColor = (name.Length > 6) ? SystemColors.ButtonShadow : SystemColors.ControlDarkDark;

            Core.Variables.Instance.DefaultFromPort = name;
            lbl.BackColor = Color.Yellow;
            currentFrom = lbl;
        }

        private void labelTo_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl == currentTo)
                return;

            string name = lbl.Name.Replace("labelTo", "");
            if (currentTo != null)
                currentTo.BackColor = (name.Length > 6) ? SystemColors.ButtonShadow : SystemColors.ControlDarkDark;

            Core.Variables.Instance.DefaultToPort = name;
            lbl.BackColor = Color.Yellow;
            currentTo = lbl;
        }

    }
}