using System;

namespace MetaBuilder.BusinessLogic
{
    /// <summary>
    /// 
    /// </summary>
    /// <remark>this enumeration contains the items contained in the table FileType</remark>
    [Serializable]
    public enum FileTypeList
    {
        ArrowHead = 5,

        Diagram = 1,

        Image = 4,

        Symbol = 2,

        SymbolStore = 3,
        SavedContextView = 6,
        MindMap = 8
    }
}