using System;
using System.Collections;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Persistence;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Utilities;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.MetaTree
{
    [Serializable]
    public class DocumentNode : MetaTreeNode
    {

		#region Fields (2) 

        private int _fileID;
        private GraphFile _graphFile;

		#endregion Fields 

		#region Properties (2) 

        public int FileID
        {
            get { return _fileID; }
            set { _fileID = value; }
        }

        public GraphFile GraphFile
        {
            get { return _graphFile; }
            set { _graphFile = value; }
        }

		#endregion Properties 

		#region Methods (2) 


		// Public Methods (1) 

        public override void LoadChildren()
        {
            if (GraphFile.Blob.Length == 0)
            {
                Nodes.Clear();
                // Retrieve the file completely now
                GraphFile = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetBypkidMachine(FileID, GraphFile.Machine);
                SetupClassNodes();
            }
        }



		// Private Methods (1) 

        private void SetupClassNodes()
        {
            GraphFileManager graphManager = new GraphFileManager();
            NormalDiagram doc = graphManager.RetrieveGraphDoc(GraphFile);
            Hashtable containedClasses = new Hashtable();
            foreach (GoObject o in doc)
            {
                if (o is GraphNode)
                {
                    GraphNode n = o as GraphNode;
                    if (n.MetaObject != null)
                    {
                        string className = n.MetaObject._ClassName;
                        if (!containedClasses.Contains(className))
                        {
                            GraphNodeCollectionNode classNode = new GraphNodeCollectionNode();
                            classNode.ClassName = className;
                            classNode.Nodes.Add(new EmptyNode());
                            containedClasses.Add(classNode.ClassName, classNode);
                            classNode.GraphNodes.Add(n);
                            Nodes.Add(classNode);
                        }
                        else
                        {
                            GraphNodeCollectionNode existingClassNode =
                                (GraphNodeCollectionNode) containedClasses[className];
                            existingClassNode.GraphNodes.Add(n);
                        }
                    }
                }
            }
        }


		#endregion Methods 

    }
}