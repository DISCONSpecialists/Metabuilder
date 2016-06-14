using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace MetaBuilder.UIControls.GraphingUI.Tools.Allocation
{
    public partial class Allocation : Form
    {

        private Collection<MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem> items;
        public Collection<MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem> Items
        {
            get
            {
                if (items == null)
                    items = new Collection<MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem>();
                return items;
            }
            set { items = value; }
        }

        private Collection<MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem> newItems;
        public Collection<MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem> NewItems
        {
            get
            {
                if (newItems == null)
                    newItems = new Collection<MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem>();
                return newItems;
            }
            set { newItems = value; }
        }

        public Allocation(Collection<MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem> i)
        {
            InitializeComponent();
            Items = i;
            foreach (MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem item in Items)
            {
                NewItems.Add(item);
                listBoxAllocation.Items.Add(item);
            }
            panelPathType.Visible = false;
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            //browse path
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "All Files (*.*)|*.*|MetaBuilder Diagram (*.mdgm)|*.mdgm;*.dgm";
            ofd.FilterIndex = 0;
            ofd.Title = "Select a file to allocate";
            ofd.Multiselect = true;

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    if (file.Length == 0)
                        continue;

                    MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem item = new MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem(file, "");
                    NewItems.Add(item);
                    listBoxAllocation.Items.Add(item);
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listBoxAllocation.SelectedItem != null)
            {
                NewItems.Remove(listBoxAllocation.SelectedItem as MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem);
                listBoxAllocation.Items.Remove(listBoxAllocation.SelectedItem);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void listBoxAllocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelPathType.Visible = false;
            return;
            if (listBoxAllocation.SelectedItem is MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem)
            {
                //(listBoxAllocation.SelectedItem as MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem).IsRelative = radioButtonRelative.Checked;
                panelPathType.Visible = true;
            }
        }

        private void radioButtonRelative_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxAllocation.SelectedItem is MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem)
            {
                //(listBoxAllocation.SelectedItem as MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem).IsRelative = radioButtonRelative.Checked;
            }
        }

        private void radioButtonAbsolute_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxAllocation.SelectedItem is MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem)
            {
                //(listBoxAllocation.SelectedItem as MetaBuilder.Graphing.Shapes.AllocationHandle.AllocationItem).IsRelative = radioButtonRelative.Checked;
            }
        }
    }
}