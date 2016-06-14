using System;
using System.Drawing;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using MetaBuilder.Graphing.Shapes.Primitives;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class ArrowTransformer : Northwoods.Go.Xml.GoXmlTransformer
    {

		#region Constructors (1) 

        public ArrowTransformer()
            : base()
        {
            this.TransformerType = typeof(GradientArrow);
            this.ElementName = "pmArrow";
            this.IdAttributeUsedForSharedObjects = true;
           
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            GradientArrow gradArrow = new GradientArrow();
            return gradArrow;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            GradientArrow gradArrow = obj as GradientArrow;
            PointF[] pts = PointFArrayAttr("Points", new PointF[] {});
            gradArrow.SetPoints(pts);
            //gradArrow.EndPoint = PointFAttr("EndPt",new PointF(0,0));
          //  gradArrow.StartPoint = PointFAttr("StartPt", new PointF(0, 0));


            bool hasGradient = BooleanAttr("grad", false);
            if (hasGradient)
            {
                GoShape shape = obj as GoShape;
                IBehaviourShape ibshape = shape as IBehaviourShape;
                GradientBehaviour gbeh = new GradientBehaviour();
                gbeh.MyBrush = new ShapeGradientBrush();
                gbeh.MyBrush.BorderColor = ColorAttr("grad_bordercolor", Color.Black);
                gbeh.MyBrush.OuterColor = ColorAttr("grad_outercolor", Color.Black);
                gbeh.MyBrush.InnerColor = ColorAttr("grad_innercolor", Color.Black);
                string defaultGradient = StringAttr("grad_type", GradientType.ForwardDiagonal.ToString());
                Type tGradType = typeof(GradientType);
                gbeh.MyBrush.GradientType =
                    (GradientType)Enum.Parse(tGradType, defaultGradient);
                ibshape.Manager.AddBehaviour(gbeh);
                gbeh.Update(shape);
            }
            else
            {
                GoShape shape = obj as GoShape;
                if (shape != null)
                {
                    shape.Brush = new SolidBrush(ColorAttr("solidColor", Color.White));
                }
            }
        }

        public override void GenerateAttributes(Object obj)
        {
           // base.GenerateAttributes(obj);
            GradientArrow gradArrow = obj as GradientArrow;

            WriteAttrVal("Points", gradArrow.CopyPointsArray());
            WriteAttrVal("StartPt", gradArrow.StartPoint);
            WriteAttrVal("EndPt", gradArrow.EndPoint);

            if (obj is IBehaviourShape)
            {
                IBehaviourShape ibShape = obj as IBehaviourShape;
                GradientBehaviour gbeh = ibShape.Manager.GetExistingBehaviour(typeof(GradientBehaviour)) as GradientBehaviour;
                if (gbeh != null)
                {
                    if (gbeh.MyBrush.OuterColor != gbeh.MyBrush.InnerColor)
                    {

                        WriteAttrVal("grad", true);
                        WriteAttrVal("grad_bordercolor", gbeh.MyBrush.BorderColor);
                        WriteAttrVal("grad_outercolor", gbeh.MyBrush.OuterColor);
                        WriteAttrVal("grad_innercolor", gbeh.MyBrush.InnerColor);
                        WriteAttrVal("grad_type", gbeh.MyBrush.GradientType.ToString());
                    }
                    else
                    {
                        WriteAttrVal("solidColor", gbeh.MyBrush.InnerColor);
                    }
                }
                else
                {
                    GoShape shape = obj as GoShape;
                    if (shape != null)
                    {
                        SolidBrush sbrush = shape.Brush as SolidBrush;
                        WriteAttrVal("solidColor", sbrush.Color);
                    }
                }
                //WriteAttrVal("Type", obj.GetType());
            }

        }


		#endregion Methods 

    }
}
