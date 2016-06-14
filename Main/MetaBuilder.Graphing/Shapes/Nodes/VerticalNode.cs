using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Drawing;
using MetaBuilder.Meta;

namespace MetaBuilder.Graphing.Shapes.Nodes
{
    //a node which has a list of containing categories which have horizontal sections for attribute and value
    public class VerticalNode : GraphNode, IGoCollapsible, IMetaNode, IShallowCopyable
    {
        public VerticalNode()
        {

        }

        #region IGoCollapsible Members

        //public void Collapse()
        //{
        //}

        //public void Expand()
        //{
        //}

        //private bool collapsible;
        //public bool Collapsible
        //{
        //    get { return collapsible; }
        //    set { collapsible = value; }
        //}

        public bool IsExpanded
        {
            get { return true; }
        }

        #endregion

        #region IMetaNode Members

        private bool requiresAttention;
        public bool RequiresAttention
        {
            get { return requiresAttention; }
            set
            {
                if (requiresAttention == value)
                    return;
                requiresAttention = value;
                try
                {
                    Color c = Color.Red;
                    CalculateGridSize();
                    //Grid.Visible = true;
                    Grid.Brush = new SolidBrush(Color.FromArgb(125, c.R, c.G, c.B));
                    Grid.Printable = false;

                    Grid.Visible = value;
                }
                catch
                {
                    //Invalid operationexception
                    //only when you close and choose to save while there are duplicates on the diagram
                }
            }
        }

        #endregion

        #region IShallowCopyable Members

        public bool copyAsShadow;
        public bool CopyAsShadow
        {
            get { return copyAsShadow; }
            set { copyAsShadow = value; }
        }

        public GoObject CopyAsShallow()
        {
            MetaBase mo = MetaObject;
            GraphNode gnode = Copy() as GraphNode;
            gnode.MetaObject = mo;
            return gnode;
        }

        #endregion

        //#region IIdentifiable Implementation

        //private string name;

        //public string Name
        //{
        //    get { return name; }
        //    set { name = value; }
        //}

        //#endregion

    }

    public class VerticalNodeList : GoListGroup
    {

    }

    //public class VerticalNodeListItem : GoTextNode, IMetaNode
    //{

    //}
}