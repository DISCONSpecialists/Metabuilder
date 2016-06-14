using System;
using System.Drawing;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using MetaBuilder.Graphing.Shapes.Primitives;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence
{
    public class GradientShapeTransformer : BaseGoObjectTransformer
    {

        #region Constructors (1)

        public GradientShapeTransformer()
            : base()
        {
            this.TransformerType = typeof(GoShape);
            this.ElementName = "IBehaviourShape";
            this.IdAttributeUsedForSharedObjects = true;
        }

        #endregion Constructors

        #region Methods (3)

        // Public Methods (2) 

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            bool hasGradient = BooleanAttr("grad", false);

            if (obj is IIdentifiable)
            {
                IIdentifiable iidobj = obj as IIdentifiable;
                iidobj.Name = StringAttr("shpName", "");
            }
            Color cborder;
            Color couter;
            Color cinner;
            if (hasGradient)
            {
                cborder = ColorAttr("grad_bordercolor", Color.Black);
                couter = ColorAttr("grad_outercolor", Color.Black);
                cinner = ColorAttr("grad_innercolor", Color.Black);
                CreateFill(obj, ref cborder, ref couter, ref cinner);
            }
            else
            {
                GoShape shape = obj as GoShape;
                if ((shape != null) && (!(shape is QuickStroke)))
                {
                    cborder = Color.Black;
                    cinner = ColorAttr("solidColor", Color.White);
                    couter = cinner;
                    CreateFill(obj, ref cborder, ref couter, ref cinner);
                }
            }
            GoObject objGo = obj as GoObject;
            RectangleF bounds = RectangleFAttr("xy", new RectangleF());
            objGo.Width = bounds.Width;
            objGo.Height = bounds.Height;
            objGo.Position = new PointF(bounds.X, bounds.Y);
            objGo.AutoRescales = false;
        }

        public override void GenerateAttributes(Object obj)
        {
#if DEBUG
            //   TraceTool.TTrace.Debug.Send("generating for obj of type " + obj.GetType().FullName);
#endif
            base.GenerateAttributes(obj, true);

            if (obj is IIdentifiable)
            {
                IIdentifiable iidobj = obj as IIdentifiable;
                WriteAttrVal("shpName", iidobj.Name);
            }
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
                        if (sbrush != null)
                            WriteAttrVal("solidColor", sbrush.Color);
                    }
                }
                //WriteAttrVal("Type", obj.GetType());
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
        }

        // Private Methods (1) 

        private void CreateFill(object obj, ref Color cborder, ref Color couter, ref Color cinner)
        {
            try
            {
                GoShape shape = obj as GoShape;
                IBehaviourShape ibshape = shape as IBehaviourShape;
                GradientBehaviour gbeh = new GradientBehaviour();
                gbeh.MyBrush = new ShapeGradientBrush();
                gbeh.MyBrush.BorderColor = cborder;

                if (couter == Color.Transparent)
                    gbeh.MyBrush.OuterColor = Color.White;
                else
                    gbeh.MyBrush.OuterColor = couter;

                if (cinner == Color.Transparent)
                    gbeh.MyBrush.InnerColor = Color.White;
                else
                    gbeh.MyBrush.InnerColor = cinner;

                string defaultGradient = StringAttr("grad_type", GradientType.ForwardDiagonal.ToString());
                Type tGradType = typeof(GradientType);
                gbeh.MyBrush.GradientType = (GradientType)Enum.Parse(tGradType, defaultGradient);
                ibshape.Manager.AddBehaviour(gbeh);
                gbeh.Update(shape);

            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
        }

        #endregion Methods

    }
}