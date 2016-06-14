using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class ChooseRepository : Form
    {
        public int Index = -1;
        public ChooseRepository()
        {
            InitializeComponent();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            SelectRepository();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Index = -1;
            Close();
        }

        private void listBoxRepository_SelectedIndexChanged(object sender, EventArgs e)
        {
            Index = listBoxRepository.SelectedIndex;
        }

        private void listBoxRepository_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectRepository();
        }

        private void SelectRepository()
        {
            if (Index != -1)
                Close();
        }

    }
}