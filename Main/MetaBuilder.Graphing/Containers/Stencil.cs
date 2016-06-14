using System;
using MetaBuilder.BusinessLogic;

namespace MetaBuilder.Graphing.Containers
{
    [Serializable]
    public class Stencil : BaseDocument
    {
        #region Constructors (1) 

        public Stencil()
        {
            FileType = FileTypeList.SymbolStore;
        }

        #endregion Constructors 
    }
}