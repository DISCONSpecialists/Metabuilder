using System;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Behaviours.Observers
{
    [Serializable]
    public class ContainerPaddingBehaviour : BaseObserverBehaviour
    {
        #region Fields (3) 

        private float _padBottom;
        private float _padRight;
        private ContainerPaddingType options;

        #endregion Fields 

        #region Constructors (1) 

        public ContainerPaddingBehaviour(IIdentifiable observed, GoObject observer, ContainerPaddingType paddingOptions)
        {
            allowMultiple = true;
            Options = paddingOptions;
            myObserved = observed;
            if (observed is GoObject)
                SetupInitialProperties(observed as GoObject, observer);
        }

        #endregion Constructors 

        #region Properties (1) 

        public ContainerPaddingType Options
        {
            get { return options; }
            set { options = value; }
        }

        #endregion Properties 

        #region Methods (4) 

        // Public Methods (2) 

        public override void OnObservedChanged(GoObject observed, int subhint, GoObject RealObserver)
        {
            // Hint = 1001
            switch (subhint)
            {
                case 1001:
                    if (observed is IIdentifiable)
                    {
                        if (((IIdentifiable) observed).Name == MyObserved.Name)
                        {
                            SetChildSize(observed, RealObserver);
                        }
                    }
                    break;
            }
        }

        public override void SetupInitialProperties(GoObject observed, GoObject observer)
        {
            StoreInitialPadding(observed, observer);
        }


        // Private Methods (2) 

        private void SetChildSize(GoObject observed, GoObject observer)
        {
            switch (Options)
            {
                case ContainerPaddingType.Height:
                    if (observed.Height + _padBottom > observer.Height)
                        observer.Height = observed.Height + _padBottom;
                    break;
                case ContainerPaddingType.Width:
                    if (observed.Width + _padRight > observer.Width)
                        observer.Width = observed.Width + _padRight;
                    break;
            }
        }

        private void StoreInitialPadding(GoObject observed, GoObject observer)
        {
            _padBottom = observer.Height - observed.Height;
            _padRight = observer.Width - observed.Width;
        }

        #endregion Methods 
    }
}