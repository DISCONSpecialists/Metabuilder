using System;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class ShapeGroup : GoGroup
    {
        #region Constructors (1) 

        public ShapeGroup()
        {
            Resizable = false;
            Reshapable = false;
            this.PickableBackground = false;
        }

        #endregion Constructors 
    }
}