using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Primitives
{
    [Serializable]
    public class GradientRoundedRectangle : GoRoundedRectangle, ISnappableShape, IIdentifiable, IBehaviourShape
    {
        public GradientRoundedRectangle()
        {
            Corner = new SizeF(0, 0);
            name = Guid.NewGuid().ToString();
            manager = new BaseShapeManager();
        }

        #region IIdentifiable Implementation

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        #region IBehaviourShape Implementation

        private BaseShapeManager manager;
        public BaseShapeManager Manager
        {
            get
            {
                if (manager == null)
                    manager = new BaseShapeManager();
                return manager;
            }
            set { manager = value; }
        }

        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal,
                                                  RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            if (Manager.Enabled && subhint == 1001)
                Manager.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect, this);
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

        public override void Changed(int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal,
                                     RectangleF newRect)
        {
            if (subhint == 1001 && Manager.Enabled)
                Manager.Changed(subhint, oldI, oldVal, oldRect, newI, newVal, newRect, this);
            base.Changed(subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

        #endregion

        public GradientBehaviour GradientBehaviour
        {
            get { return manager.GetExistingBehaviour(typeof(GradientBehaviour)) as GradientBehaviour; }
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            base.CopyObjectDelayed(env, newobj);
            GradientRoundedRectangle rect = newobj as GradientRoundedRectangle;
            rect.Name = Name.Substring(0, Name.Length);
            if (Manager != null)
                rect.Manager = Manager.Copy(rect);
        }

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            base.ChangeValue(e, undo);
        }

        public override bool Selectable
        {
            get
            {
                if ((this as GoObject).Parent == null)
                    return base.Selectable;
                else
                    return false; //parented shapes are usually nodes and therefore are also dragsnode = true
            }
            set
            {
                base.Selectable = value;
            }
        }
    }
}