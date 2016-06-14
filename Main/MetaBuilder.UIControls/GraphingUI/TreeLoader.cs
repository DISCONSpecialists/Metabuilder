using System;
using MetaBuilder.Docking;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class TreeLoader : DockContent
    {

		#region Constructors (1) 

        public TreeLoader()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Methods (1) 


		// Private Methods (1) 

        private void TreeLoader_Load(object sender, EventArgs e)
        {
            treeView1.LoadDocumentsInWorkspace();
        }


		#endregion Methods 

    }
}