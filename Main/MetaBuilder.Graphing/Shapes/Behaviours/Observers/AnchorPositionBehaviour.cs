using System;
using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Behaviours.Observers
{
    [Serializable]
    public class AnchorPositionBehaviour : BaseObserverBehaviour
    {
        #region Fields (2)

        private SizeF _initialOffset;
        private PositionLockLocation lockLocation;

        #endregion Fields

        #region Constructors (1)

        public AnchorPositionBehaviour(IIdentifiable observed, GoObject observer, PositionLockLocation location)
        {
            LockLocation = location;
            observer.Movable = false;
            SetObserver(observed, observer);
        }

        #endregion Constructors

        #region Properties (2)

        public SizeF InitialOffset
        {
            get { return _initialOffset; }
            set { _initialOffset = value; }
        }

        public PositionLockLocation LockLocation
        {
            get { return lockLocation; }
            set { lockLocation = value; }
        }

        #endregion Properties

        #region Methods (4)

        // Public Methods (3) 

        public override IBehaviour Copy(GoObject newObserver)
        {
            GraphNode nodeParent = newObserver.ParentNode as GraphNode;
            if (nodeParent != null && MyObserved != null)
            {
                IIdentifiable identifiable = nodeParent.FindByName(MyObserved.Name);
                if (identifiable != null)
                    return new AnchorPositionBehaviour(identifiable, newObserver, LockLocation);
                return null;
            }
            return null;
        }

        public override void OnObservedChanged(GoObject observed, int subhint, GoObject RealObserver)
        {
            // Hint = 1001
            if (subhint == 1102 || subhint == 1101 || subhint == 1001)
            {
                if (observed is IIdentifiable)
                {
                    if (MyObserved != null)
                        if (((IIdentifiable)observed).Name == MyObserved.Name)
                        {
                            // need to store the offset
                            PointF anchorspot = GetAnchorSpot(observed);
                            RealObserver.Position = new PointF(anchorspot.X + InitialOffset.Width, anchorspot.Y + InitialOffset.Height);
                            if (observed is RepeaterSection)
                            {
                                RealObserver.Position = new PointF(RealObserver.Position.X, observed.Bottom - RealObserver.Height / 2f);
                            }
                        }
                }
            }
        }

        public override void SetupInitialProperties(GoObject observed, GoObject observer)
        {
            InitialOffset = new SizeF(0f, 0f);
            // AnchorSpot is used for initial offset calculation
            PointF AnchorSpot = GetAnchorSpot(observed);
            InitialOffset = new SizeF(observer.Position.X - AnchorSpot.X, observer.Position.Y - AnchorSpot.Y);
        }

        // Private Methods (1) 

        private PointF GetAnchorSpot(GoObject observed)
        {
            PointF AnchorSpot = new PointF();
            switch (LockLocation)
            {
                case PositionLockLocation.TopLeft:
                    AnchorSpot = new PointF(observed.Position.X, observed.Position.Y);
                    break;
                case PositionLockLocation.TopRight:
                    AnchorSpot = new PointF(observed.Position.X + observed.Width, observed.Position.Y);
                    break;
                case PositionLockLocation.BottomLeft:
                    AnchorSpot = new PointF(observed.Position.X, observed.Position.Y + observed.Height);
                    break;
                case PositionLockLocation.BottomRight:
                    AnchorSpot = new PointF(observed.Position.X + observed.Width, observed.Position.Y + observed.Height);
                    break;
                case PositionLockLocation.MiddleLeft:
                    AnchorSpot = new PointF(observed.Position.X, (observed.Position.Y + observed.Height / 2));
                    break;
                case PositionLockLocation.MiddleRight:
                    AnchorSpot = new PointF(observed.Position.X + observed.Width, observed.Position.Y + (observed.Height / 2));
                    break;
                case PositionLockLocation.BottomCenter:
                    AnchorSpot = new PointF((observed.Position.X + observed.Width / 2), observed.Position.Y + observed.Height);
                    break;
                case PositionLockLocation.TopCenter:
                    AnchorSpot = new PointF(observed.Position.X + (observed.Width / 2), observed.Position.Y);
                    break;
                case PositionLockLocation.MiddleCenter:
                    AnchorSpot = new PointF(observed.Position.X + (observed.Width / 2), observed.Position.Y + (observed.Height / 2));
                    break;
            }
            return AnchorSpot;
        }

        #endregion Methods
    }
}