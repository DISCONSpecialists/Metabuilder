using System.Collections.Generic;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using d = MetaBuilder.DataAccessLayer;
using b = MetaBuilder.BusinessLogic;

namespace MetaBuilder.UIControls.GraphingUI.RepositoryTree
{
    public class GraphFileNode : ItemNode
    {

        #region Fields (2)

        private bool autoLoadChildren;
        private GraphFile file;

        #endregion Fields

        #region Constructors (1)

        public GraphFileNode(GraphFile file)
        {
            Items = new List<IRepositoryItem>();
            File = file;
            Text = strings.GetFileNameOnly(file.Name);
            TargetPanel = TargetPanelType.Objects;
        }

        #endregion Constructors

        #region Properties (3)

        public bool AutoLoadChildren
        {
            get { return autoLoadChildren; }
            set { autoLoadChildren = value; }
        }

        public GraphFile File
        {
            get { return file; }
            set { file = value; }
        }

        public GraphFileCollectionNode GraphFileCollectionNode
        {
            get { return Parent as GraphFileCollectionNode; }
        }

        #endregion Properties

        #region Methods (1)

        //2 September 2013 - after first load do not load again
        bool Loaded = false;
        // Public Methods (1) 

        public override void LoadChildren()
        {
            if (Loaded)
                return;

            Items.Clear();
            TList<MetaObject> objects = DataRepository.Connections[Provider].Provider.MetaObjectProvider.GetByGraphFileIDGraphFileMachineFromGraphFileObject(file.pkid, file.Machine);
            foreach (MetaObject mo in objects)
            {
                Items.Add(ObjectHelper.ConvertToMetaBase(mo, (Provider == Core.Variables.Instance.ServerProvider)));
                //Items.Add(mo);
            }
            Loaded = true;
        }

        #endregion Methods

    }
}