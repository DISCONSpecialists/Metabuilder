using System;
using MetaBuilder.Graphing.Formatting;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Behaviours.Internal
{
    [Serializable]
    public class GradientBehaviour : BaseInternalBehavior
    {
        #region Fields (1)

        private ShapeGradientBrush myBrush;

        #endregion Fields

        #region Properties (1)

        public ShapeGradientBrush MyBrush
        {
            get
            {
                return myBrush;
            }
            set { myBrush = value; }
        }
        #endregion Properties

        #region Methods (4)

        // Public Methods (4) 

        public override void Changed(int subhint, GoObject owner)
        {
            base.Changed(subhint, owner);
            if (myBrush != null && owner.Initializing == false)
            {
                owner.Initializing = true;
                myBrush.Apply(owner as GoShape);
                owner.Initializing = false;
            }
        }

        public override IBehaviour Copy(GoObject observer)
        {
            GradientBehaviour retval = new GradientBehaviour();
            retval.AllowMultiple = AllowMultiple;
            retval.MyBrush = new ShapeGradientBrush();
            retval.MyBrush.BorderColor = MyBrush.BorderColor;
            retval.MyBrush.GradientType = myBrush.GradientType;
            retval.myBrush.IsLinear = myBrush.IsLinear;
            retval.myBrush.LinearEnd = myBrush.LinearEnd;
            retval.myBrush.LinearStart = myBrush.LinearStart;
            retval.myBrush.OuterColor = myBrush.OuterColor;
            retval.myBrush.InnerColor = myBrush.InnerColor;
            retval.Owner = observer;
            retval.MyBrush.Apply(observer as GoShape);
            return retval;
        }

        public void Disable(GoObject owner)
        {
            if (myBrush != null)
            {
                myBrush.Disable(owner as GoShape);
            }
        }

        public override void Update(GoObject owner)
        {
            /* if (myBrush != null)
            {
                if (owner.ParentNode != null)
                {
                    if (owner.ParentNode is CollapsibleNode)
                    {
                        if (owner.ParentNode is IMetaNode)
                        {
                            IMetaNode node = owner.ParentNode as IMetaNode;
                            if (node.BindingInfo.BindingClass == "DataView")
                            {
                                if (owner is GoRoundedRectangle)
                                {
                                    GoRoundedRectangle rr = owner as GoRoundedRectangle;
                                    if (rr.Right < rr.ParentNode.Right-50)
                                    {
                                        myBrush.Apply(owner as GoShape);
                                    }
                                    else
                                    {
                                        
                                    }
                                }
                            }
                        }
                    }
                }
                else
                myBrush.Apply(owner as GoShape);
            }*/
            if (owner != null && myBrush != null)
                myBrush.Apply(owner as GoShape);
        }

        #endregion Methods
    }
}