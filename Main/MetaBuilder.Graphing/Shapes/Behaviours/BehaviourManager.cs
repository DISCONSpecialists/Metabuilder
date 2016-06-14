using System;
using System.Collections.Generic;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using Northwoods.Go;
using System.Collections.ObjectModel;

namespace MetaBuilder.Graphing.Shapes.Behaviours
{
    [Serializable]
    public class BaseShapeManager
    {
        private bool _enabled;
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public void ClearBehaviours(object sender, EventArgs e)
        {
            behaviours = new List<IBehaviour>();
            Enabled = false;
        }

        public BaseShapeManager Copy(GoObject parent)
        {
            BaseShapeManager bsm = new BaseShapeManager();
            foreach (IBehaviour beh in Behaviours)
            {
                bsm.AddBehaviour(beh.Copy(parent));
            }
            return bsm;
        }

        private List<IBehaviour> behaviours;
        public List<IBehaviour> Behaviours
        {
            get
            {
                if (behaviours == null)
                    behaviours = new List<IBehaviour>();
                return behaviours;
            }
        }

        public bool AddBehaviour(IBehaviour newBehaviour)
        {
            Enabled = true;
            if (newBehaviour != null)
            {
                if (newBehaviour.AllowMultiple == false)
                {
                    if (GetExistingBehaviour(newBehaviour.GetType()) != null)
                    {
                        Behaviours.Remove(GetExistingBehaviour(newBehaviour.GetType()));
                    }
                }
                Behaviours.Add(newBehaviour);
            }
            return true;
        }

        public void RemoveBehaviourOfType(Type t)
        {
            IBehaviour existing = GetExistingBehaviour(t);
            if (existing != null)
                Behaviours.Remove(existing);
        }

        public IBehaviour GetExistingBehaviour(Type t)
        {
            if (Behaviours.Count > 0)
                foreach (IBehaviour existingBehaviour in Behaviours)
                {
                    if (existingBehaviour.GetType().FullName == t.FullName)
                    {
                        return existingBehaviour;
                    }
                }
            return null;
        }

        public void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect, GoObject Observer)
        {
            if (Behaviours.Count > 0)
                foreach (IBehaviour behaviour in Behaviours)
                {
                    if (behaviour is BaseObserverBehaviour)
                    {
                        ((BaseObserverBehaviour)behaviour).OnObservedChanged(observed, subhint, Observer);
                        Changed(subhint, oldI, oldVal, oldRect, newI, newVal, newRect, Observer);
                    }
                }
        }

        public void Changed(int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect, GoObject Observer)
        {
            if (Behaviours.Count > 0)
                foreach (IBehaviour behaviour in Behaviours)
                {
                    if (behaviour is BaseInternalBehavior)
                        ((BaseInternalBehavior)behaviour).Changed(subhint, Observer);
                }
        }
    }
}