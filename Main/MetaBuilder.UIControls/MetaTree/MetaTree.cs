using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.MetaTree
{
    public class MetaTree : MetaTreeBase
    {

		#region Constructors (1) 

        public MetaTree()
        {
            CheckBoxes = true;
        }

		#endregion Constructors 

		#region Methods (2) 


		// Public Methods (1) 

        public void LoadDocumentsInWorkspace()
        {
            Nodes.Clear();
            // Add some documents
            TreeNode tnDocuments = new TreeNode();
            tnDocuments.Text = "Documents in this workspace";
            Nodes.Add(tnDocuments);
            Hashtable addedDocuments = new Hashtable();
            // Load the list of documents into the tree
            // TODO: Hack
            SqlCommand cmd =
                new SqlCommand("HACK_MarkDrawingsActiveFromReportingTool",
                               new SqlConnection(Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            TempFileGraphAdapter tfAdapter = new TempFileGraphAdapter();
            TList<GraphFile> fileList =
                tfAdapter.GetFilesByWorkspaceTypeIdWorkspaceName(Variables.Instance.CurrentWorkspaceTypeId,
                                                                 Variables.Instance.CurrentWorkspaceName,
                                                                 (int) FileTypeList.Diagram,false);
            //.GetByWorkspaceID(Core.Variables.Instance.CurrentWorkspaceID);
            foreach (GraphFile file in fileList)
            {
                //if (file.IsActive)
                {
                    if (!addedDocuments.ContainsKey(file.Name))
                    {
                        DocumentNode tnFile = new DocumentNode();
                        tnFile.FileID = file.pkid;
                        tnFile.GraphFile = file;
                        tnFile.Text = strings.GetFileNameWithoutExtension(file.Name) + " (" +
                                      file.MajorVersion.ToString() + "." + file.MinorVersion.ToString("00") + ")";
                        tnFile.Nodes.Add(new TreeNode("-- loading --"));
                        tnDocuments.Nodes.Add(tnFile);
                        addedDocuments.Add(file.Name, true);
                    }
                }
            }
            HookupEvents();
        }



		// Protected Methods (1) 

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            base.OnBeforeExpand(e);
            if (!e.Node.IsExpanded)
            {
                if (e.Node is MetaTreeNode)
                {
                    (e.Node as MetaTreeNode).LoadChildren();
                }
            }
        }


		#endregion Methods 

    }
}