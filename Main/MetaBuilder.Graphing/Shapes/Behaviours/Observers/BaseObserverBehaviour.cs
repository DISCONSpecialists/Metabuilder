using System;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Behaviours.Observers
{
    [Serializable]
    public abstract class BaseObserverBehaviour : IBehaviour
    {
        #region Fields (2) 

        internal bool allowMultiple;
        internal IIdentifiable myObserved;

        #endregion Fields 

        #region Properties (2) 

        public IIdentifiable MyObserved
        {
            get { return myObserved; }
        }

        public bool AllowMultiple
        {
            get { return allowMultiple; }
            set { allowMultiple = value; }
        }

        #endregion Properties 

        #region Methods (4) 

        // Public Methods (4) 

        public virtual IBehaviour Copy(GoObject o)
        {
            return null;
        }

        public virtual void OnObservedChanged(GoObject observed, int subhint, GoObject observer)
        {
        }

        public void SetObserver(IIdentifiable observed, GoObject observer)
        {
            if (observed is GoObject)
            {
                myObserved = observed;
                SetupInitialProperties(observed as GoObject, observer);
            }
        }

        public virtual void SetupInitialProperties(GoObject observed, GoObject observer)
        {
        }

        #endregion Methods 
    }
}