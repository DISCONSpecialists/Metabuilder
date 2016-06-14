using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
namespace MetaBuilder.UIControls.MetaTree
{
    public class WorkspaceNode : MetaTreeNode
    {

		#region Fields (1) 

        private int _workspaceID;

		#endregion Fields 

		#region Properties (1) 

        public int WorkspaceID
        {
            get { return _workspaceID; }
            set { _workspaceID = value; }
        }

		#endregion Properties 

		#region Methods (1) 


		// Public Methods (1) 

        public void LoadDocuments()
        {
            TempFileGraphAdapter tfga = new TempFileGraphAdapter();
            TList<GraphFile> files = tfga.GetFilesByTypeID((int)FileTypeList.Diagram,false);

            foreach (GraphFile file in files)
            {
                if (file.IsActive)
                {
                    DocumentNode docNode = new DocumentNode();
                    docNode.GraphFile = file;
                    docNode.FileID = file.pkid;
                    docNode.Text = strings.GetFileNameOnly(file.Name);
                    docNode.LoadChildren();
                    this.Nodes.Add(docNode);
                }
            }
        }


		#endregion Methods 

    }
}