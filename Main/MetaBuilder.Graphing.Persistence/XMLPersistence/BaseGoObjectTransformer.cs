using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Meta;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence
{
    public class BaseGoObjectTransformer : BaseTransformer
    {
        public BaseGoObjectTransformer()
            : base()
        {
            this.BodyConsumesChildElements = false;
            this.TransformerType = typeof(GoObject);
            this.IdAttributeUsedForSharedObjects = true;
        }

        public void GenerateAttributes(object obj, bool IncludeBasicAttributes)
        {
            if (IdAttributeUsedForSharedObjects)
                this.Writer.MakeShared(obj);

            if (IncludeBasicAttributes)
            {
                /* if (obj is GraphNode)
                 {
                     System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true); // true means get line numbers.
                     foreach (System.Diagnostics.StackFrame f in st.GetFrames())
                     {
                         Console.Write(f);
                     }
                 }*/

                GoObject goObj = obj as GoObject;
                if (obj is GraphNodeGrid)
                {
                    //      WriteAttrVal("Move", goObj.Parent.Movable);
                    //    WriteAttrVal("Reshape", false);
                    //  WriteAttrVal("Resize", false);
                }
                else
                {
                    base.GenerateAttributes(obj);

                    if (goObj.Editable)
                        WriteAttrVal("Edit", goObj.Editable);

                    if (!(goObj.DragsNode))
                        WriteAttrVal("DragsNode", goObj.DragsNode);

                    if (goObj.Printable)
                        WriteAttrVal("Print", goObj.Printable);

                    WriteAttrVal("Reshape", goObj.Reshapable);
                    WriteAttrVal("Resize", goObj.Resizable);
                    WriteAttrVal("Move", goObj.Movable);

                    if (goObj.Selectable)
                        WriteAttrVal("Select", goObj.Selectable);

                    if (goObj.Shadowed)
                        WriteAttrVal("Shadow", goObj.Shadowed);

                    if (!goObj.Visible)
                        WriteAttrVal("Visible", false);

                    if (goObj.Deletable)
                        WriteAttrVal("Deletable", goObj.Deletable);
                }

                WriteAttrVal("xy", goObj.Bounds);
                if (obj is GoShape)
                {
                    GenerateBasicShapeAttributes(obj);
                }
            }

            IBehaviourShape shape = obj as IBehaviourShape;
            if (shape != null)
            {
                AnchorPositionBehaviour anchorBehaviour = shape.Manager.GetExistingBehaviour(typeof(MetaBuilder.Graphing.Shapes.Behaviours.Observers.AnchorPositionBehaviour)) as AnchorPositionBehaviour;

                if (anchorBehaviour != null)
                {
                    WriteAttrVal("AnchorPosition", anchorBehaviour.LockLocation.ToString());
                    WriteAttrRef("AnchoredToObj", anchorBehaviour.MyObserved);
                }
            }
        }

        public override void GenerateAttributes(object obj)
        {
            //GenerateAttributes(obj,true);
        }

        private void GenerateBasicShapeAttributes(object obj)
        {
            //if (obj is QLink || obj is QuickPort)
            //    return;
            GoShape shape = obj as GoShape;
            try
            {
                if (shape.Brush is SolidBrush)
                {
                    SolidBrush brush = shape.Brush as SolidBrush;
                    WriteAttrVal("solidbrush", brush.Color);
                }
                if (shape.Pen != null && !(shape is QuickPort))
                {
                    //lock (shape)
                    {
                        //lock (shape.Pen)
                        {
                            try
                            {
                                WriteAttrVal("PenWidth", shape.Pen.Width);
                                WriteAttrVal("PenColor", shape.Pen.Color);
                                WriteAttrVal("PenDashStyle", shape.Pen.DashStyle.ToString());
                                WriteAttrVal("PenDashCap", shape.Pen.DashCap.ToString());
                            }
                            catch (InvalidOperationException objectInUseElsewhereException)
                            {
                                Form f = new Form();
                                Graphics g = f.CreateGraphics();
                                g.ReleaseHdc();
                                Pen shapePen = shape.Pen;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private string AnchorLock;
        private string AnchorObjID;
        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            GoObject goObj = obj as GoObject;
            goObj.Editable = BooleanAttr("Edit", false);
            goObj.Movable = BooleanAttr("Move", true);
            goObj.DragsNode = BooleanAttr("DragsNode", true);
            goObj.Printable = BooleanAttr("Print", true);
            goObj.Reshapable = BooleanAttr("Reshape", false);
            goObj.Resizable = BooleanAttr("Resize", false);
            goObj.Selectable = BooleanAttr("Select", false);
            goObj.Shadowed = BooleanAttr("Shadow", false);
            goObj.Visible = BooleanAttr("Visible", true);
            if (goObj is GraphNode)
            {
                GraphNode gnode = goObj as GraphNode;
                gnode.EditMode = false;
            }

            RectangleF bounds = RectangleFAttr("xy", new RectangleF());

            goObj.Width = bounds.Width;
            goObj.Height = bounds.Height;
            goObj.Position = new PointF(bounds.X, bounds.Y);

            if (goObj is Shapes.Nodes.Containers.MappingCell)
            {
                (goObj as Shapes.Nodes.Containers.MappingCell).BackGround.Width = bounds.Width;
                (goObj as Shapes.Nodes.Containers.MappingCell).BackGround.Height = bounds.Height;
                (goObj as Shapes.Nodes.Containers.MappingCell).BackGround.Position = new PointF(bounds.X, bounds.Y);
            }

            goObj.Deletable = BooleanAttr("Deletable", true);

            // Consume Basic Shape Attributes if necessary
            if (obj is GoShape)
            {
                ConsumeBasicShapeAttributes(obj);
            }

            AnchorLock = StringAttr("AnchorPosition", "");
            AnchorObjID = StringAttr("AnchoredToObj", "");

            if (AnchorLock != "")
            {
                Reader.AddDelayedRef(obj, "AnchorPosition", AnchorLock);
                Reader.AddDelayedRef(obj, "AnchoredObject", AnchorObjID);
            }

            if (IsAttrPresent("id"))
                this.Reader.MakeShared(StringAttr("id", null), obj);
        }

        public override void UpdateReference(object obj, string prop, object referred)
        {
            if (prop == "AnchoredObject")
            {
                GoObject anchoredTo = referred as GoObject;
                if (anchoredTo != null && AnchorLock != "")
                {
                    Type tAnchorLock = typeof(PositionLockLocation);
                    IIdentifiable ident = anchoredTo as IIdentifiable;
                    if (ident.Name == Guid.Empty.ToString() || ident.Name == "")
                        ident.Name = Guid.NewGuid().ToString();

                    PositionLockLocation position = (PositionLockLocation)Enum.Parse(tAnchorLock, AnchorLock);

                    AnchorPositionBehaviour apb = new AnchorPositionBehaviour(ident, obj as GoObject, position);

                    IBehaviourShape behShape = obj as IBehaviourShape;
                    behShape.Manager.AddBehaviour(apb);
                    behShape.Manager.Enabled = true;
                    anchoredTo.AddObserver(obj as GoObject);
                    /*GoObject o = obj as GoObject;
                    if (position == PositionLockLocation.BottomCenter 
                        ||
                        position == PositionLockLocation.BottomLeft 
                        || position == PositionLockLocation.BottomRight)
                    o.Position = new PointF(o.Position.X, anchoredTo.Bottom);*/
                    if (anchoredTo is IBehaviourShape)
                    {
                        IBehaviourShape ibShape = anchoredTo as IBehaviourShape;
                        ibShape.Manager.Enabled = true;
                    }
                }
            }
            if (prop == "AnchorPosition")
            {
                AnchorLock = referred.ToString();
            }

            /*   if (AnchorLock != "")
               {

                 //  Reader.AddDelayedRef(obj, "AnchoredObject", AnchorObjID);
                   //Reader.AddDelayedRef(obj, "AnchorPosition", AnchorLock);

                   GoObject anchoredTo = Reader.FindShared(AnchorObjID) as GoObject;
                   if (anchoredTo != null)
                   {
                       Type tAnchorLock = typeof(PositionLockLocation);

                       IIdentifiable ident = anchoredTo as IIdentifiable;
                       if (ident.Name == Guid.Empty.ToString() || ident.Name == "")
                           ident.Name = Guid.NewGuid().ToString();


                       PositionLockLocation position = (PositionLockLocation)Enum.Parse(tAnchorLock, AnchorLock);

                       AnchorPositionBehaviour apb = new AnchorPositionBehaviour(ident, anchoredTo, position);

                       IBehaviourShape behShape = obj as IBehaviourShape;
                       behShape.Manager.AddBehaviour(apb);
                   }
               } */
        }

        private void ConsumeBasicShapeAttributes(object obj)
        {
            GoShape shape = obj as GoShape;
            Color brushesForSolids = ColorAttr("solidbrush", Color.White);

            if (brushesForSolids != Color.Transparent)
            {
                shape.Brush = new SolidBrush(brushesForSolids);
            }
            else
            {
                shape.Brush = Brushes.White;//new SolidBrush(Color.White);
            }

            Color penColor = ColorAttr("PenColor", Color.Black);
            float penWidth = FloatAttr("PenWidth", 1f);

            if (!(shape is QuickPort)) // prevent thick borders on ports
            {
                Pen pen = new Pen(penColor, penWidth);
                if (shape is QuickStroke)
                {
                    shape.Width = pen.Width;
                }
                Type tPenDashStyle = typeof(DashStyle);
                Type tPenDashCap = typeof(DashCap);

                if (!(shape is IBehaviourShape)) //Force all shapes whose parents are nodes to ALWAYS have solid borders
                {
                    pen.DashStyle = (DashStyle)Enum.Parse(tPenDashStyle, StringAttr("PenDashStyle", DashStyle.Solid.ToString()));
                    pen.DashCap = (DashCap)Enum.Parse(tPenDashCap, StringAttr("PenDashCap", DashCap.Flat.ToString()));
                }

                shape.Pen = pen;
            }
            else
            {
                shape.Pen = new Pen(Color.Gray, 1f);
            }
        }
    }
}