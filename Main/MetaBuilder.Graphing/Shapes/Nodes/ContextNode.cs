using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MetaBuilder.Meta;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class ContextNode : GoBasicNode
    {
        private Guid id = Guid.Empty;

        public Guid ID
        {
            get
            {
                if (id == Guid.Empty)
                    id = Guid.NewGuid();
                return id;
            }
            set { id = value; }
        }

        protected override GoPort CreatePort()
        {
            GoPort p = new GoPort();
            p.PortObject = this;
            if (Label != null)
                p.PortObject = Label;
            p.FromSpot = NoSpot;
            p.ToSpot = NoSpot;
            return p;
        }

        #region Fields (4)

        private IndicatorLabel leftIndicator;
        public IndicatorLabel LeftIndicator
        {
            get { return leftIndicator; }
            set { leftIndicator = value; }
        }

        //private IndicatorLabel rightIndicator;
        //public IndicatorLabel RightIndicator
        //{
        //    get { return rightIndicator; }
        //    set { rightIndicator = value; }
        //}

        private bool isMainItem;
        private MetaBase metaObject;

        #endregion Fields

        #region Constructors (1)

        public ContextNode()
        {
            AddDomainIndicators();

            PathGradientRoundedRectangle rect = new PathGradientRoundedRectangle();
            rect.FringeColor = Color.LightGreen;
            Shape = rect;
            LabelSpot = MiddleCenter;
            Text = "No Value";
            Label.Multiline = true;
            Port.FromSpot = NoSpot;
            Port.ToSpot = NoSpot;
            Shadowed = true;
            Movable = true;

            BuildSmallRectangle();
        }

        private void AddDomainIndicators()
        {
            LeftIndicator = new IndicatorLabel();
            LeftIndicator.Multiline = false;
            LeftIndicator.FontSize = 7f;
            LeftIndicator.DragsNode = true;
            LeftIndicator.Selectable = false;
            LeftIndicator.Deletable = false;
            LeftIndicator.Location = new PointF(Left, Bottom + 10);
            LeftIndicator.Alignment = 0;
            this.Add(LeftIndicator);
        }
        public void BuildSmallRectangle()
        {
            GoRectangle smallRect = new GoRectangle();
            smallRect.Height = 3;
            smallRect.Brush = Brushes.Transparent;//new SolidBrush(Color.Transparent); //Default to transparent
            smallRect.DragsNode = true;
            smallRect.Selectable = false;
            smallRect.Shadowed = false;
            //subtract half the width of the rectangle @10
            //subtract 7 from y to position 'inside' node
            //smallRect.Location = new PointF(Bounds.Right - 15 - (Bounds.Width / 2), Bounds.Top - 7);
            //smallRect.Location = new PointF(Bounds.Right - 15, Bounds.Top - 7);//- (Bounds.Width / 2)

            MySmallRectangle = smallRect;
            Add(smallRect);
        }
        public void reBoundSmallRectangle()
        {
            if (MySmallRectangle != null)
            {
                MySmallRectangle.Width = Width / 2;
                MySmallRectangle.Location = new PointF(Bounds.Right - (Bounds.Width / 2) - (Bounds.Width / 4), Bounds.Top - 1);
            }
        }
        public GoRectangle MySmallRectangle;

        #endregion Constructors

        #region Properties (5)

        //We Set Text=Text to 'Reset' MFD & Diagram Status on explode
        public bool IsMainItem
        {
            get { return isMainItem; }
            set
            {
                isMainItem = value;
                if (value)
                {
                    PathGradientRoundedRectangle rect = Shape as PathGradientRoundedRectangle;
                    rect.FringeColor = Color.Green;
                    Text = Text;
                }
            }
        }
        public bool IsExplodedMainItem
        {
            set
            {
                if (value)
                {
                    PathGradientRoundedRectangle rect = Shape as PathGradientRoundedRectangle;
                    rect.FringeColor = Color.Blue;
                    Text = Text;
                }
            }
        }
        public bool IsExplodedItem
        {
            set
            {
                if (value)
                {
                    PathGradientRoundedRectangle rect = Shape as PathGradientRoundedRectangle;
                    rect.FringeColor = Color.LightBlue;
                    Text = Text;
                }
            }
        }

        private bool isArtifact;
        public bool IsArtifact
        {
            get { return isArtifact; }
            set
            {
                isArtifact = value;

                if (value)
                {
                    PathGradientRoundedRectangle rect = Shape as PathGradientRoundedRectangle;
                    rect.OverrideFocusScale = 0.9f;
                    rect.CenterColor = Color.Snow;
                    Text = Text;
                }
            }
        }

        public MetaBase MetaObject
        {
            get { return metaObject; }
            set
            {
                metaObject = value;
                //set indicator to
                LeftIndicator.Text = "";
                if (value != null)
                {
                    foreach (System.Reflection.PropertyInfo pinfo in metaObject.GetMetaPropertyList(false))
                    {
                        if (pinfo.Name != "Type" && pinfo.Name.Contains("Type"))
                        {
                            if (metaObject.Get(pinfo.Name) != null)
                            {
                                if (metaObject.Get(pinfo.Name) != null && metaObject.Get(pinfo.Name).ToString() != "")
                                {
                                    LeftIndicator.Text = metaObject.Get(pinfo.Name).ToString();
                                }
                            }
                            //stop it from going further than the first 'type'
                            break;
                        }
                    }
                }
            }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                PathGradientRoundedRectangle rect = Shape as PathGradientRoundedRectangle;

                if (metaObject != null)
                {
                    if (VCStatusTool.IsObsoleteOrMarkedForDelete(metaObject))
                        rect.FringeColor = Color.Red;
                }
                else if (value.Contains(".dgm") || value.Contains(".mdgm"))
                {
                    rect.FringeColor = Color.Orange;
                }

                reBoundSmallRectangle();
            }
        }

        #endregion Properties

        private string diagram;
        public string Diagram
        {
            get { return diagram; }
            set { diagram = value; }
        }

    }

    [Serializable]
    public class PathGradientRoundedRectangle : GoRoundedRectangle
    {
        #region Fields (6)

        public const int ChangedCenterColor = 12307;
        public const int ChangedCenterRatio = 12309;
        public const int ChangedFringeColor = 12308;
        [NonSerialized]
        private Brush myBrush;
        private Color myCenterColor = Color.White;
        private Color myFringeColor = Color.Gray;

        #endregion Fields

        #region Constructors (1)

        #endregion Constructors

        #region Properties (4)

        public override RectangleF Bounds
        {
            set
            {
                base.Bounds = value;
                ResetBrush();
            }
        }

        private float dec_overrideFocusScale = 0f;

        public float OverrideFocusScale
        {
            get { return dec_overrideFocusScale; }
            set { dec_overrideFocusScale = value; }
        }

        public override Brush Brush
        {
            get
            {
                if (myBrush == null)
                {
                    //          LinearGradientBrush br = new LinearGradientBrush(GetSpotLocation(TopLeft), GetSpotLocation(BottomRight), this.CenterColor, this.FringeColor);
                    PointF[] myPoints = new PointF[]
                                            {
                                                GetSpotLocation(TopLeft),
                                                GetSpotLocation(TopRight),
                                                GetSpotLocation(BottomRight),
                                                GetSpotLocation(BottomLeft)
                                            };
                    PathGradientBrush br = new PathGradientBrush(myPoints);
                    br.CenterColor = CenterColor;
                    br.SurroundColors = new Color[] { FringeColor, FringeColor, FringeColor, FringeColor };
                    br.FocusScales = new PointF(Math.Max(0.0f, (Width - Corner.Width * (2 - OverrideFocusScale)) / Width),
                                                Math.Max(0.0f, (Height - Corner.Height * (2 - OverrideFocusScale)) / Height));
                    myBrush = br;
                }

                return myBrush;
            }
            set { }
        }

        public Color CenterColor
        {
            get { return myCenterColor; }
            set
            {
                Color old = myCenterColor;
                if (old != value)
                {
                    myCenterColor = value;
                    ResetBrush();
                    Changed(ChangedCenterColor, 0, old, NullRect, 0, value, NullRect);
                }
            }
        }

        public Color FringeColor
        {
            get { return myFringeColor; }
            set
            {
                Color old = myFringeColor;
                if (old != value)
                {
                    myFringeColor = value;
                    ResetBrush();
                    Changed(ChangedFringeColor, 0, old, NullRect, 0, value, NullRect);
                }
            }
        }

        #endregion Properties

        #region Methods (2)

        // Public Methods (1) 

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            switch (e.SubHint)
            {
                case ChangedBounds:
                    base.ChangeValue(e, undo);
                    ResetBrush();
                    break;
                case ChangedCenterColor:
                    CenterColor = (Color)e.GetValue(undo);
                    break;
                case ChangedFringeColor:
                    FringeColor = (Color)e.GetValue(undo);
                    break;
                default:
                    base.ChangeValue(e, undo);
                    break;
            }
        }

        // Private Methods (1) 

        private void ResetBrush()
        {
            myBrush = null;
        }

        #endregion Methods
    }
}