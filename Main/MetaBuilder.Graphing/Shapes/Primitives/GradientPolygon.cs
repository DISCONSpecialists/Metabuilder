using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Primitives
{
    [Serializable]
    public class GradientPolygon : GoPolygon, IIdentifiable, IBehaviourShape, ISnappableShape
    {
        public GradientPolygon(GoPolygonStyle style)
        {
            name = Guid.NewGuid().ToString();
            manager = new BaseShapeManager();
            Style = style;
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

        public BaseInternalBehavior GradientBehaviour
        {
            get { return manager.GetExistingBehaviour(typeof (GradientBehaviour)) as BaseInternalBehavior; }
        }

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            env.Delayeds.Add(this);
            return base.CopyObject(env);
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            base.CopyObjectDelayed(env, newobj);
            GradientPolygon poly = newobj as GradientPolygon;
            poly.Name = Name.Substring(0, Name.Length);
            if (Manager != null)
                poly.Manager = Manager.Copy(poly); // poly wants a crackEr!
        }
    }
}