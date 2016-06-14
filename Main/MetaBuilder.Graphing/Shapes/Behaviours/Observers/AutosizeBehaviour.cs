using System;
using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Behaviours.Observers
{
    [Serializable]
    public class AutosizeBehaviour : BaseObserverBehaviour
    {
        #region Fields (2) 

        // Used to store the height/width % of the child
        private SizeF _initialRatio;
        private AutoSizeType autoSizeType;

        #endregion Fields 

        #region Constructors (1) 

        public AutosizeBehaviour(IIdentifiable observed, GoObject observer, AutoSizeType sizeType)
        {
            AutosizeType = sizeType;
            myObserved = observed;
            if (observed is GoObject)
                SetupInitialProperties(observed as GoObject, observer);
            observer.Resizable = false;
        }

        #endregion Constructors 

        #region Properties (2) 

        public AutoSizeType AutosizeType
        {
            get { return autoSizeType; }
            set { autoSizeType = value; }
        }

        public SizeF InitialRatio
        {
            get { return _initialRatio; }
            set { _initialRatio = value; }
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
                            SizeF autosized = GetChildSize(observed, RealObserver);
                            RealObserver.Size = autosized;
                        }
                    }
                    break;
            }
        }

        public override void SetupInitialProperties(GoObject observed, GoObject observer)
        {
            StoreInitialRatio(observed, observer);
            SizeF autosized = GetChildSize(observed, observer);
            observer.Size = autosized;
        }


        // Private Methods (2) 

        private SizeF GetChildSize(GoObject observed, GoObject observer)
        {
            // Observer is always the child
            SizeF NewChildSize = new SizeF(observer.Width, observer.Height);
            switch (AutosizeType)
            {
                case AutoSizeType.Both:
                    NewChildSize.Width = observed.Width*InitialRatio.Width;
                    NewChildSize.Height = observed.Height*InitialRatio.Height;
                    break;
                case AutoSizeType.Height:
                    NewChildSize.Height = observed.Height*InitialRatio.Height;
                    break;
                case AutoSizeType.Width:
                    NewChildSize.Width = observed.Width*InitialRatio.Width;
                    break;
            }
            return NewChildSize;
        }

        private void StoreInitialRatio(GoObject observed, GoObject observer)
        {
            float fHeightDifference = 0;
            float fWidthDifference = 0;
            switch (AutosizeType)
            {
                case AutoSizeType.Both:
                    fHeightDifference = observer.Height/observed.Height;
                    fWidthDifference = observer.Width/observed.Width;
                    break;
                case AutoSizeType.Height:
                    fHeightDifference = observer.Height/observed.Height;
                    fWidthDifference = -1;
                    break;
                case AutoSizeType.Width:
                    fHeightDifference = -1;
                    fWidthDifference = observer.Width/observed.Width;
                    break;
            }
            InitialRatio = new SizeF(fWidthDifference, fHeightDifference);
        }

        #endregion Methods 
    }
}