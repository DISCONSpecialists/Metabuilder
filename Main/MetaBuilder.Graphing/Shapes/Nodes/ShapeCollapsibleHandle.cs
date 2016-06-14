using System;
using System.Collections.Generic;
using Northwoods.Go;
using System.Windows.Forms;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class ShapeCollapsibleHandle : GoCollapsibleHandle
    {
        #region Fields (1)

        private List<string> collapsibles;

        #endregion Fields

        #region Constructors (1)

        public ShapeCollapsibleHandle()
        {
            collapsibles = new List<string>();
        }

        #endregion Constructors

        #region Properties (2)

        public List<string> Collapsibles
        {
            get { return collapsibles; }
            set { collapsibles = value; }
        }

        public override bool Printable
        {
            get { return false; }
            set { base.Printable = value; }
        }

        #endregion Properties

        #region Methods (4)

        // Public Methods (4) 

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            env.Delayeds.Add(this);
            return base.CopyObject(env);
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            base.CopyObjectDelayed(env, newobj);
            ShapeCollapsibleHandle newHandle = newobj as ShapeCollapsibleHandle;
            newHandle.Collapsibles = new List<string>();
            foreach (string s in collapsibles)
            {
                newHandle.Collapsibles.Add(s.Substring(0, s.Length));
            }
        }

        public override IGoCollapsible FindCollapsible()
        {
            GraphNode n = ParentNode as GraphNode;
            if (n != null)
                foreach (string o in collapsibles)
                {
                    if (n.FindByName(o) is IGoCollapsible)
                        return n.FindByName(o) as IGoCollapsible;
                }
            return base.FindCollapsible();
        }

        public override bool OnSingleClick(GoInputEventArgs evt, GoView view)
        {
            GraphNode n = ParentNode as GraphNode;
            foreach (string o in collapsibles)
            {
                GoObject obj = n.FindByName(o) as GoObject;
                if (obj != null)
                {
                    obj.Visible = !obj.Visible;
                    obj.Printable = !obj.Printable;
                }
            }

            IGoCollapsible collapsibleNode = ParentNode as IGoCollapsible;
            if (collapsibleNode != null)
            {
                collapsibleNode.Collapsible = !collapsibleNode.Collapsible;
            }
            return base.OnSingleClick(evt, view);
        }

        #endregion Methods
    }

}