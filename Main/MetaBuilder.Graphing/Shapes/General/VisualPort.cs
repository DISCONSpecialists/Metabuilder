using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.General
{
    [Serializable]
    public class VisualPort : GoPort
    {
        public VisualPort()
        {
            IsValidSelfNode = false;
            Width = 5;
            Height = 5;
            Deletable = true;
            Selectable = true;
            Resizable = false;
            DragsNode = true;
            Style = GoPortStyle.Diamond;
        }
    }
}
