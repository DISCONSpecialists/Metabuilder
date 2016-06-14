using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes.General;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class PortManipulator : Form
    {
        public QuickPortHelper.QuickPortLocation Position;
        public PortManipulator()
        {
            InitializeComponent();
            bindData();
        }
        private void bindData()
        {
            comboBoxPortPosition.DataSource = Enum.GetValues(typeof(QuickPortHelper.QuickPortLocation));
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void comboBoxPortPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            Position = (QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(QuickPortHelper.QuickPortLocation), comboBoxPortPosition.Text);
        }
    }
}