using MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;

namespace MetaBuilder.Graphing.Utilities
{
    internal class NodeManipulator
    {
        #region Methods (1) 

        // Public Methods (1) 

        public static void PrepareObjectForDocumentType(GraphNode node, FileTypeList DocumentType,
                                                        GoDocument callingDocument)
        {
            switch (DocumentType)
            {
                case FileTypeList.Diagram:
                    node.EditMode = false;
                    /*
                    if (node.Document == callingDocument)
                    {
                        node.CreateMetaObject(node.Document, EventArgs.Empty);
                    }*/
                    //node.DisableImageSelection();// workaround for contextclicking
                    break;
                case FileTypeList.SymbolStore:
                    node.EditMode = true;
                    break;
            }
        }

        #endregion Methods 
    }
}