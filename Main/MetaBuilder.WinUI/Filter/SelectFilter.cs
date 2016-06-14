using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.Meta;

namespace MetaBuilder.WinUI.Filter
{
    public partial class SelectFilter : Form
    {
        public SelectFilter(List<string> filters)
        {
            InitializeComponent();
            FilterItem disconFilter = null;
            foreach (string s in filters)
            {
                FilterItem i = new FilterItem(s);
                if (i.ToString() == "DISCON")
                    disconFilter = i;
                if (!comboBoxFilter.Items.Contains(i))
                    comboBoxFilter.Items.Add(i);
            }
            comboBoxFilter.SelectedItem = disconFilter;
            comboBoxFilter.Refresh();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Core.FilterVariables.filterName = (comboBoxFilter.SelectedItem as FilterItem).Filename;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }

}