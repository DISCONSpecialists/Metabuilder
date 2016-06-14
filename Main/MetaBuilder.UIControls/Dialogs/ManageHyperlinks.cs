using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Northwoods.Go;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class ManageHyperlinks : Form
    {

		#region Fields (2) 

        private List<HyperlinkSpecification> specs;
        private HyperlinkUpdater updater;

		#endregion Fields 

		#region Constructors (1) 

        public ManageHyperlinks()
        {
            InitializeComponent();
           
            
        }

		#endregion Constructors 

		#region Methods (10) 


		// Public Methods (3) 

        public List<HyperlinkSpecification> GetHyperlinks(IGoCollection col)
        {
            List<HyperlinkSpecification> retval = new List<HyperlinkSpecification>();
            foreach (GoObject o in col)
            {
                if (o is MetaBuilder.Graphing.Shapes.Hyperlink)
                {
                    MetaBuilder.Graphing.Shapes.Hyperlink link = o as MetaBuilder.Graphing.Shapes.Hyperlink;
                    HyperlinkSpecification spec = new HyperlinkSpecification();
                    spec.ActualLink = link;
                    if (link.HyperlinkType == MetaBuilder.Graphing.Shapes.HyperlinkType.File || link.HyperlinkType == MetaBuilder.Graphing.Shapes.HyperlinkType.Diagram)
                    {

                        if (link.Arguments.Contains(":\\"))
                        {
                            spec.FileToFind = link.Arguments;
                            retval.Add(spec);
                        }
                    }
                    
                }

                if (o is IGoCollection)
                {
                    retval.AddRange(GetHyperlinks(o as IGoCollection));
                }
            }
            return retval;
        }

        public void Setup(GoDocument doc)
        {
            specs = GetHyperlinks(doc);
            Dictionary<string, HyperlinkSpecification> hyperdict = new Dictionary<string, HyperlinkSpecification>();
            foreach (HyperlinkSpecification spc in specs)
            {
                if (hyperdict.ContainsKey(spc.FileToFind.ToLower()))
                {
                    // do nothing
                }
                else
                {
                    hyperdict.Add(spc.FileToFind.ToLower(), spc);
                }
            }

            specs = new List<HyperlinkSpecification>();
              foreach (KeyValuePair<string,HyperlinkSpecification> kvp in hyperdict)
              {
                  specs.Add(kvp.Value);
              }
            UpdateResults();
            
        }

        public void TestSetup()
        {
            
        }



		// Private Methods (7) 

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdialog = new FolderBrowserDialog();
            DialogResult res = fbdialog.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                if (listFolders.Items.Contains(fbdialog.SelectedPath))
                {
                    // do nothing
                }
                else
                    listFolders.Items.Add(fbdialog.SelectedPath);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            bool errors = false;
            // ensure that only one replacement is selected
            foreach (TreeListViewItem tvi in treeResults.Items)
            {
                int replacementcount = 0;
                foreach (TreeListViewItem tviChild in tvi.Items)
                {
                    if (tviChild.Checked)
                    {
                        replacementcount++;
                    }
                }
                if (replacementcount>1)
                {
                    errors = true;
                }
            }

            if (errors)
            {
                MessageBox.Show(this,"Please select only one replacement for each hyperlink");
                return;
            }
            else
            {
                foreach (TreeListViewItem tvi in treeResults.Items)
                {
                    HyperlinkSpecification spec = tvi.Tag as HyperlinkSpecification;
                    foreach (TreeListViewItem tviChild in tvi.Items)
                    {
                        if (tviChild.Checked)
                        {
                            spec.ActualLink.Arguments = (tviChild.Tag as FileInfo).FullName;
                        }
                    }
                }


            }
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listFolders.SelectedItem!=null)
            {
                listFolders.Items.RemoveAt(listFolders.SelectedIndex);
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            updater = new HyperlinkUpdater();

            updater.DirectoriesToCheck = new List<DirectoryInfo>();
            foreach (string s in listFolders.Items)
            {
                updater.DirectoriesToCheck.Add(new DirectoryInfo(s));
            }

            updater.HyperlinksToUpdate = specs;
            updater.Reset();
            updater.DoSearch();
            UpdateResults();
        }

        private void treeListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(treeResults.SelectedItems.Count>0)
            {
                TreeListViewItem tvli = treeResults.SelectedItems[0];
                if (tvli.Parent == null)
                {
                    HyperlinkSpecification spec = tvli.Tag as HyperlinkSpecification;
                    if (spec.Exists)
                    {
                        toolStripStatusLabel1.Text = "Hyperlink: " + Core.strings.GetFileNameOnly(spec.FileToFind) +
                                                     ", no need to update.";
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Hyperlink: " + Core.strings.GetFileNameOnly(spec.FileToFind) +
                                                     ", Matches: " + tvli.Items.Count.ToString();
                    }
                }
                else
                {
                    int itemIndex = tvli.Parent.Items.GetIndexOf(tvli);
                    HyperlinkSpecification spec = tvli.Parent.Tag as HyperlinkSpecification;
                 
                    StringBuilder sbDetails = new StringBuilder();
                    sbDetails.Append("New reference: " + spec.PossibleMatches[itemIndex].FullName);

                    toolStripStatusLabel1.Text = sbDetails.ToString();
                }
            }
        }

        private void UpdateResults()
        {
          treeResults.Items.Clear();
          foreach (HyperlinkSpecification spec in specs)
          {
                TreeListViewItem item = new TreeListViewItem();
                item.Text = spec.FileToFind.ToString();
                item.Tag = spec;

                foreach (FileInfo finfo in spec.PossibleMatches)
                {
                    TreeListViewItem replitem = new TreeListViewItem();
                    replitem.Text = Core.strings.GetCompactPath(finfo.FullName,100);
                    replitem.Tag = finfo;
                    item.Items.Add(replitem);
                }
                treeResults.Items.Add(item);
            }
        }


		#endregion Methods 

    }

    public class HyperlinkSpecification
    {

		#region Fields (5) 

        private MetaBuilder.Graphing.Shapes.Hyperlink actualLink;
        private bool exists;
        private string fileToFind;
        private List<FileInfo> possibleMatches;
        private FileInfo selectedFileInfo;

		#endregion Fields 

		#region Constructors (1) 

        public HyperlinkSpecification()
        {
            possibleMatches = new List<FileInfo>();
        }

		#endregion Constructors 

		#region Properties (5) 

        public MetaBuilder.Graphing.Shapes.Hyperlink ActualLink
        {
            get { return actualLink; }
            set { actualLink = value; }
        }

        public bool Exists
        {
            get { return exists; }
            set { exists = value; }
        }

        public string FileToFind
        {
            get { return fileToFind; }
            set { fileToFind = value; }
        }

        public List<FileInfo> PossibleMatches
        {
            get { return possibleMatches; }
            set { possibleMatches = value; }
        }

        public FileInfo SelectedFileInfo
        {
            get { return SelectedFileInfo; }
            set { selectedFileInfo = value; }
        }

		#endregion Properties 

		#region Methods (1) 


		// Public Methods (1) 

        public string GetFileName()
        {
            return MetaBuilder.Core.strings.GetFileNameOnly(FileToFind);
        }


		#endregion Methods 

    }

    public class HyperlinkUpdater
    {

		#region Fields (2) 

        private List<DirectoryInfo> directoriesToCheck;
        private List<HyperlinkSpecification> hyperLinkToUpdate;

		#endregion Fields 

		#region Constructors (1) 

        public HyperlinkUpdater()
        {
            HyperlinksToUpdate = new List<HyperlinkSpecification>();
            DirectoriesToCheck = new List<DirectoryInfo>();
        }

		#endregion Constructors 

		#region Properties (2) 

        public List<DirectoryInfo> DirectoriesToCheck
        {
            get { return directoriesToCheck; }
            set { directoriesToCheck = value; }
        }

        public List<HyperlinkSpecification> HyperlinksToUpdate
        {
            get { return hyperLinkToUpdate; }
            set { hyperLinkToUpdate = value; }
        }

		#endregion Properties 

		#region Methods (3) 


		// Public Methods (2) 

        public void DoSearch()
        {
            foreach (DirectoryInfo dirinfo in DirectoriesToCheck)
                WalkDirectory(dirinfo);
        }

        public void Reset()
        {
            foreach (HyperlinkSpecification hyperlink in HyperlinksToUpdate)
            {
                hyperlink.Exists = false;
                hyperlink.PossibleMatches = new List<FileInfo>();
            }
        }



		// Private Methods (1) 

        private void WalkDirectory(DirectoryInfo directory)
        {
            // Scan all files in the current path
            foreach (HyperlinkSpecification hyperlink in HyperlinksToUpdate)
            {
                if (System.IO.File.Exists(hyperlink.FileToFind))
                {
                    // do nothing
                    hyperlink.Exists = true;
                }
                else
                    foreach (FileInfo file in directory.GetFiles(hyperlink.GetFileName()))
                    {
                        // Do something with each file.
                        hyperlink.PossibleMatches.Add(file);
                    }
            }


            DirectoryInfo[] subDirectories = directory.GetDirectories();

            // Scan the directories in the current directory and call this method 

            // again to go one level into the directory tree

            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                WalkDirectory(subDirectory);
            }
        }


		#endregion Methods 

    }
}