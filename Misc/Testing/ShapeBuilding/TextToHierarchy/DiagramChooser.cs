using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;

namespace ShapeBuilding.TextToHierarchy
{
    public partial class DiagramChooser : Form
    {
        public DiagramChooser()
        {
            InitializeComponent();
            LoadDiagramList();
        }
        b.TList<b.GraphFile> files;

        private void LoadDiagramList()
        {

            listView1.Items.Clear();

            TempFileGraphAdapter tfga = new TempFileGraphAdapter();
            files = tfga.GetAllFilesByTypeID((int)MetaBuilder.BusinessLogic.FileTypeList.Diagram, false);
            foreach (b.GraphFile file in files)
            {
                if (file.IsActive)
                {
                    ListViewItem item = GetListViewItem(file);
                    listView1.Items.Add(item);

                }
            }
        }
        public ListViewItem GetListViewItem(b.GraphFile file)
        {
            string name = file.Name;
            if (file.Name.ToLower().Contains(".mdgm"))
            {
                name = MetaBuilder.Core.strings.GetFileNameWithoutExtension(file.Name);
            }
            ListViewItem item = new ListViewItem(name);
            item.SubItems.Add(file.MajorVersion.ToString() + "." + file.MinorVersion.ToString());
            item.SubItems.Add(file.ModifiedDate.ToString("D", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
            item.SubItems.Add(file.ModifiedDate.ToShortTimeString());
            item.ImageIndex = 0;
            item.Tag = file;
            return item;
        }
        public b.GraphFile SelectedFile;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                SelectedFile = listView1.SelectedItems[0].Tag as b.GraphFile;
                this.DialogResult = DialogResult.OK;
                Close();
            }


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


    }
}