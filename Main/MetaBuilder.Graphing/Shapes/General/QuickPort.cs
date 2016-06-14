using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Behaviours;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes.General;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class QuickPort : GoBoxPort, IIdentifiable, IBehaviourShape
    {
        private QuickPortHelper.QuickPortLocation portPosition;
        public QuickPortHelper.QuickPortLocation PortPosition
        {
            get { return portPosition; }
            set
            {
                portPosition = value;
            }
        }

        #region Fields (3)

        private readonly bool _isAutoPort;
        private float _incomingLinksDirection;
        private float _outgoingLinksDirection;

        #endregion Fields

        #region Constructors (2)

        public QuickPort(bool AutoCreate)
        {
            _isAutoPort = AutoCreate;
            manager = new BaseShapeManager();
            AutoRescales = false;
            ToSpot = NoSpot;
            FromSpot = NoSpot;
            Style = GoPortStyle.Rectangle;
            Height = 10;
            Width = 10;
            Brush = Brushes.Silver;
            //this.EndSegmentLength = 20f;
            //Brushes.YellowGreen;
            /* GradientBehaviour gbehaviour = this.Manager.GetExistingBehaviour(typeof(GradientBehaviour)) as GradientBehaviour;
            gbehaviour.MyBrush.InnerColor = Color.Silver;
            gbehaviour.MyBrush.OuterColor = Color.Black;
            gbehaviour.Update(this);*/
            portPosition = QuickPortHelper.QuickPortLocation.Circumferential;
            //SkipsUndoManager = true;
        }

        public QuickPort()
            : this(true)
        {
        }

        #endregion Constructors

        #region Properties (5)

        public override int FromSpot
        {
            get { return MiddleCenter; }
            set { base.FromSpot = value; }
        }

        public float IncomingLinksDirection
        {
            get { return _incomingLinksDirection; }
            set { _incomingLinksDirection = value; }
        }

        public bool IsAutoPort
        {
            get { return _isAutoPort; }
        }

        public float OutgoingLinksDirection
        {
            get { return _outgoingLinksDirection; }
            set { _outgoingLinksDirection = value; }
        }

        public override bool Selectable
        {
            get
            {
                if (Parent is GraphNode)
                {
                    GraphNode p = Parent as GraphNode;
                    if (p.EditMode)
                        return true;
                }
                return false; // return base.Selectable;
            }
            set { base.Selectable = value; }
        }

        #endregion Properties

        #region Methods (8)

        // Public Methods (7) 

        public override bool Printable
        {
            get { return false; }
            set { base.Printable = value; }
        }

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            env.Delayeds.Add(this);
            return base.CopyObject(env);
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            QuickPort port = newobj as QuickPort;
            port.Name = Guid.NewGuid().ToString();
            if (Manager != null)
                port.Manager = Manager.Copy(port);
            base.CopyObjectDelayed(env, newobj);
        }

        public override float GetFromEndSegmentLength(IGoLink link)
        {
            if (link.FromPort != null && link.ToPort != null && link.ToPort == link.FromPort)
            {
                return 25;
            }
            if (link.FromNode != null && link.ToNode != null && link.FromNode == link.ToNode)
            {
                return 50;
            }
            return base.GetFromEndSegmentLength(link);
        }

        public override float EndSegmentLength
        {
            get
            {
                return 25;
                //return base.EndSegmentLength;
            }
            set
            {
                base.EndSegmentLength = value;
                // doesnt really matter
            }
        }

        public override float GetFromLinkDir(IGoLink link)
        {
            if (OutgoingLinksDirection > -1)
                return OutgoingLinksDirection;
            return base.GetFromLinkDir(link);
        }

        public override PointF GetFromLinkPoint(IGoLink link)
        {
            if (OutgoingLinksDirection == 0)
            {
                return new PointF(Right, Position.Y + (Height / 2));
            }
            else if (OutgoingLinksDirection == 90)
            {
                return new PointF(Left + (Width / 2), Bottom);
            }
            else if (OutgoingLinksDirection == 180)
            {
                return new PointF(Left, Position.Y + (Height / 2));
            }
            else if (OutgoingLinksDirection == 270)
            {
                return new PointF(Left + (Width / 2), Top);
            }
            return base.GetToLinkPoint(link);
        }

        public override float GetToLinkDir(IGoLink link)
        {
            if (IncomingLinksDirection > -1)
                return IncomingLinksDirection;
            return base.GetToLinkDir(link);
        }

        public override PointF GetToLinkPoint(IGoLink link)
        {
            if (IncomingLinksDirection == 0)
            {
                return new PointF(Right, Position.Y + (Height / 2));
            }
            else if (IncomingLinksDirection == 90)
            {
                return new PointF(Position.X + Width / 2, Bottom);
            }
            else if (IncomingLinksDirection == 180)
            {
                return new PointF(Left, Position.Y + (Height / 2));
            }
            else if (IncomingLinksDirection == 270)
            {
                return new PointF(Position.X + Width / 2, Top);
            }
            return base.GetToLinkPoint(link);
        }

        public bool IsDefaultFromPort()
        {
            return PortPosition == (QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultFromPort);
        }
        public bool IsDefaultToPort()
        {
            return PortPosition == (QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultToPort);
        }

        public override bool CanLinkTo()
        {
            return base.CanLinkTo();
        }

        // Protected Methods (1) 

        #endregion Methods

        #region IIdentifiable Implementation

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        #region Behaviour Implementation

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

        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            if (Manager.Enabled)
                Manager.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect, this);
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

        public override void Changed(int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            if (subhint == 1001)
                if (Manager.Enabled)
                    Manager.Changed(subhint, oldI, oldVal, oldRect, newI, newVal, newRect, this);

            if ((oldVal != null && newVal != null) || (subhint == 1001))
                base.Changed(subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

        #endregion

        public override bool Shadowed
        {
            get
            {
                if (ParentNode != null && ParentNode is ImageNode)
                    return false;
                return base.Shadowed;
            }
            set
            {
                base.Shadowed = value;
            }
        }

    }
}