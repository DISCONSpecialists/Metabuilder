using System.Collections.Generic;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.RepositoryTree
{
    public class ClassNode : ItemNode
    {

        #region Fields (2)

        private bool autoLoadChildren;
        private Class myClass;
        //private bool serverWorkspaceTransferObject = false;
        private bool workspaceTransfer = false;

        #endregion Fields

        #region Constructors (1)

        public ClassNode(Class cls, bool Transfer)
        {
            workspaceTransfer = Transfer;
            Items = new List<IRepositoryItem>();
            MyClass = cls;
            Text = cls.Name;
            TargetPanel = TargetPanelType.Objects;
        }

        #endregion Constructors

        #region Properties (2)

        public bool AutoLoadChildren
        {
            get { return autoLoadChildren; }
            set { autoLoadChildren = value; }
        }

        public Class MyClass
        {
            get { return myClass; }
            set { myClass = value; }
        }

        #endregion Properties

        #region Methods (1)


        // Public Methods (1) 

        public override void LoadChildren()
        {
            Items.Clear();
            Nodes.Clear();
            ///TODO: This can be optimised by loading workspace specific items

            TList<MetaObject> objects = DataRepository.Connections[Provider].Provider.MetaObjectProvider.GetByClass(MyClass.Name);
            foreach (MetaObject mo in objects)
            {
                if (workspaceTransfer && Provider == Core.Variables.Instance.ServerProvider)
                {
                    if (mo.VCStatusID == 1)
                    {
                        bool add = true;
                        if (WorkspaceFilter != null)
                            if (mo.WorkspaceName != WorkspaceFilter.Name || mo.WorkspaceTypeId != WorkspaceFilter.WorkspaceTypeId)
                                add = false;

                        if (!add)
                            continue;

                        MetaBase mbase = ObjectHelper.ConvertToMetaBase(mo, (Provider == Core.Variables.Instance.ServerProvider));
                        //if (!VCStatusTool.IsObsoleteOrMarkedForDelete(mbase))
                        if (!VCStatusTool.IsReadOnly(mbase))
                            Items.Add(mbase);
                    }
                }
                else if (workspaceTransfer && Provider == Core.Variables.Instance.ClientProvider)
                {
                    if (mo.VCStatusID == 7)
                    {
                        bool add = true;
                        if (WorkspaceFilter != null)
                            if (mo.WorkspaceName != WorkspaceFilter.Name || mo.WorkspaceTypeId != WorkspaceFilter.WorkspaceTypeId)
                                add = false;

                        if (!add)
                            continue;

                        MetaBase mbase = ObjectHelper.ConvertToMetaBase(mo, (Provider == Core.Variables.Instance.ServerProvider));
                        if (!VCStatusTool.IsObsoleteOrMarkedForDelete(mbase) && !VCStatusTool.IsReadOnly(mbase))
                            Items.Add(mbase);
                    }
                }
                else
                {
                    if (WorkspaceFilter != null)
                    {
                        if (mo.WorkspaceName == WorkspaceFilter.Name && mo.WorkspaceTypeId == WorkspaceFilter.WorkspaceTypeId)
                        {
                            MetaBase mbase = ObjectHelper.ConvertToMetaBase(mo, (Provider == Core.Variables.Instance.ServerProvider));
                            //if (!VCStatusTool.IsObsoleteOrMarkedForDelete(mbase))
                            if (!VCStatusTool.IsReadOnly(mbase))
                                Items.Add(mbase);
                        }
                    }
                    else
                    {
                        MetaBase mbase = ObjectHelper.ConvertToMetaBase(mo, (Provider == Core.Variables.Instance.ServerProvider));
                        if (!VCStatusTool.IsObsoleteOrMarkedForDelete(mbase) && !VCStatusTool.IsReadOnly(mbase))
                            Items.Add(mbase);
                    }
                }
            }
        }


        #endregion Methods

    }
}