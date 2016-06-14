using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ShapeBuilding.Archives.CustomPrinting
{
    public partial class ChoosePrinter : Form
    {
        public ChoosePrinter()
        {
            InitializeComponent();
        }


        private string selectedPrinter;

        public string SelectedPrinter
        {
            get { return selectedPrinter; }
            set { selectedPrinter = value; }
        }
	
        private List<string> installedPrinters;

        public List<string> InstalledPrinters
        {
            get { return installedPrinters; }
            set 
            { 
                installedPrinters = value;
                listPrinters.Items.Clear();
                foreach (string s in installedPrinters)
                {
                    listPrinters.Items.Add(s);
                }
            }
        }

        private void ChoosePrinter_Load(object sender, EventArgs e)
        {
            if (SelectedPrinter != null)
            {
                int index = -1;
                foreach (object o in listPrinters.Items)
                {
                    if (o.ToString() == SelectedPrinter)
                    {
                        index = listPrinters.Items.IndexOf(o);
                    }
                }
                listPrinters.SelectedIndex = index;
            }
            else
            {
                listPrinters.SelectedIndex = 0;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void listPrinters_SelectedIndexChanged(object sender, EventArgs e)
        {
            object o = listPrinters.Items[listPrinters.SelectedIndex];
            selectedPrinter = o.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        
	
    }
}