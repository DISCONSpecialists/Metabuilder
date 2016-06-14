using System;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Behaviours.Internal
{
    [Serializable]
    public abstract class BaseInternalBehavior : IBehaviour
    {
        #region Fields (2) 

        private bool allowMultiple;
        private GoObject owner;

        #endregion Fields 

        #region Properties (2) 

        public GoObject Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public bool AllowMultiple
        {
            get { return allowMultiple; }
            set { allowMultiple = value; }
        }

        #endregion Properties 

        #region Methods (4) 

        // Public Methods (4) 

        public virtual IBehaviour Copy(GoObject observer)
        {
            throw new Exception("Cannot use the abstract implementation");
        }

        public virtual void Changed(int subhint, GoObject owner)
        {
        }

        public virtual void Changing(int subhint)
        {
        }

        public virtual void Update(GoObject owner)
        {
        }

        #endregion Methods 
    }
}