using System.Collections.Generic;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.RepositoryTree
{
    public class GraphFileCollectionNode : ItemNode
    {

        #region Fields (2)

        private bool autoLoadChildren;
        private bool skipChildren;
        private bool workspaceTransfer = false;

        #endregion Fields

        #region Constructors (1)

        public GraphFileCollectionNode(bool Transfer)
        {
            workspaceTransfer = Transfer;
            Items = new List<IRepositoryItem>();
            TargetPanel = TargetPanelType.Diagrams;
            Text = "Diagrams";
        }

        #endregion Constructors

        #region Properties (2)

        public bool AutoLoadChildren
        {
            get { return autoLoadChildren; }
            set { autoLoadChildren = value; }
        }

        public bool SkipChildren
        {
            get { return skipChildren; }
            set { skipChildren = value; }
        }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        public void RefreshNodes()
        {
            Items.Clear();
            Nodes.Clear();
            TList<GraphFile> files;
            TempFileGraphAdapter tfga = new TempFileGraphAdapter();
            if (WorkspaceFilter != null)
            {
                files = tfga.GetFilesByWorkspaceTypeIdWorkspaceName(WorkspaceFilter.WorkspaceTypeId, WorkspaceFilter.Name, (int)FileTypeList.Diagram, (Provider == Core.Variables.Instance.ServerProvider)); //d.DataRepository.Connections[Provider].Provider.GraphFileProvider.GetByWorkspaceNameWorkspaceTypeId(WorkspaceFilter.Name, WorkspaceFilter.WorkspaceTypeId);//
            }
            else
            {
                files = tfga.GetFilesByTypeID((int)FileTypeList.Diagram, (Provider == Core.Variables.Instance.ServerProvider)); //d.DataRepository.Connections[Provider].Provider.GraphFileProvider.GetAll();//
            }
            foreach (GraphFile file in files)
            {
                if (file.IsActive && file.WorkspaceName == WorkspaceFilter.Name && file.WorkspaceTypeId == WorkspaceFilter.WorkspaceTypeId)
                {
                    if (!SkipChildren)
                    {
                        GraphFileNode filenode = new GraphFileNode(file);
                        filenode.Provider = Provider;
                        filenode.StatusFilter = StatusFilter;
                        Nodes.Add(filenode);
                        Items.Add(file);
                    }
                    else
                    {
                        if (workspaceTransfer && Provider == Core.Variables.Instance.ServerProvider)
                        {
                            if (file.VCStatusID == 1)
                            {
                                Items.Add(file);
                            }
                        }
                        else if (workspaceTransfer && Provider == Core.Variables.Instance.ClientProvider)
                        {
                            if (file.VCStatusID == 7)
                            {
                                Items.Add(file);
                            }
                        }
                        else
                        {
                            GraphFileNode filenode = new GraphFileNode(file);
                            filenode.Provider = Provider;
                            filenode.StatusFilter = StatusFilter;
                            Nodes.Add(filenode);
                            Items.Add(file);
                        }
                    }
                }
            }
            if (Nodes.Count == 0 && !(SkipChildren))
                Text = Text + " (No diagrams)";
        }

        #endregion Methods

    }
}