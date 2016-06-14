using System;
using System.Collections.Generic;
using MetaBuilder.Graphing.Shapes.Behaviours;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes
{
    /// <summary>
    /// Each collection of attributes and of operations are held in one of these,
    /// to support expand and collapse, showing a simple label when collapsed,
    /// or showing a list of ClassDiagramNodeItems when expanded.
    /// </summary>
    /// <remarks>
    /// This group will always have exactly three child objects:
    /// [0]: a GoCollapsibleHandle, to let users collapse/expand this section
    /// [1]: a GoText label to be shown when collapsed
    /// [2]: a GoListGroup holding the items to be shown when expanded
    /// </remarks>
    [Serializable]
    public class RepeaterSection : CollapsingRecordNodeItemList, IGoLabeledNode, IIdentifiable, ISnappableShape
    //IBehaviourShape, 
    {
        private bool childPortsEnabled;
        private Guid repeaterID;

        public bool ChildPortsEnabled
        {
            get { return childPortsEnabled; }
            set { childPortsEnabled = value; }
        }

        public Guid RepeaterID
        {
            get { return repeaterID; }
            set { repeaterID = value; }
        }

        public void ClearChildItems()
        {
            List<GoObject> objectsToRemove = new List<GoObject>();
            foreach (GoObject child in this)
            {
                if (child is CollapsingRecordNodeItem)
                {
                    CollapsingRecordNodeItem childItem = child as CollapsingRecordNodeItem;
                    if (!childItem.IsHeader)
                        if ((childItem.MetaObject != null && childItem.MetaObject.IsInDatabase(Core.Variables.Instance.ClientProvider)))
                            objectsToRemove.Add(childItem);
                }
            }

            for (int i = 0; i < objectsToRemove.Count; i++)
            {
                objectsToRemove[i].Remove();
            }
        }

        public virtual void CreateBody()
        {
            Add(new CollapsingRecordNodeItemList());
            //Grid.Selectable = false;
        }

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            env.Delayeds.Add(this);
            return base.CopyObject(env);
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            base.CopyObjectDelayed(env, newobj);
            RepeaterSection section = newobj as RepeaterSection;
            if (BindingInfo != null)
                section.BindingInfo = BindingInfo.Copy();
            section.Name = Name.Substring(0, Name.Length);
            section.RepeaterID = Guid.NewGuid();
            section.ChildPortsEnabled = ChildPortsEnabled;
        }

        //#region IIdentifiable Implementation

        //private string name;

        //public string Name
        //{
        //    get { return name; }
        //    set { name = value; }
        //}

        //#endregion

        #region IBehaviourShape Implementation

        /*
        private BaseShapeManager manager;
        public BaseShapeManager Manager
        {
            get
            {
                if (manager == null)
                    manager = new BaseShapeManager();
                return manager;
            }
            set
            {
                manager = value;
            }
        }
        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            Manager.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect, this);
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

     */

        #endregion
    }
}