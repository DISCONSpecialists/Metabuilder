using System;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using MetaBuilder.Graphing.Shapes.General;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Graphing.Tools;
using Northwoods.Go;
using MetaBuilder.Meta;

namespace MetaBuilder.Graphing.Controllers
{
    public class ShapeDesignController
    {
        private GoView myView;

        public ShapeDesignController(GoView view)
        {
            myView = view;
        }

        #region GoToolCreating Implementations

        private GoToolCreating ctool;

        private void StartCreateTool(GoObject prototype)
        {
            if (myView.CanInsertObjects())
            {
                ctool = new GoToolCreating(myView);
                if (ctool != null)
                {
                    if (ctool.Active)
                        ctool.Stop();
                }
                ctool.Prototype = prototype;
                ctool.Modal = true;
                myView.Tool = ctool;
            }
        }

        public void AddParallelogram()
        {
            myView.StartTransaction();
            GradientParallelogram para = new GradientParallelogram();
            para.Height = 40F;
            para.Width = 40F;
            SetDefaultGradientBehaviour(para);
            StartCreateTool(para);
            myView.FinishTransaction("Add parallelogram");
        }

        public GoObject AddEllipse()
        {
            myView.StartTransaction();
            GradientEllipse protoEllipse = new GradientEllipse();

            protoEllipse.Height = 40F;
            protoEllipse.Width = 40F;
            SetDefaultGradientBehaviour(protoEllipse);

            StartCreateTool(protoEllipse);
            myView.FinishTransaction("Add ellipse");
            return protoEllipse;
        }

        public GradientRoundedRectangle AddRectangle()
        {
            myView.StartTransaction();
            GradientRoundedRectangle goRect = new GradientRoundedRectangle();
            goRect.Bounds = new RectangleF(0, 0, 100, 100);
            goRect.Pen = new Pen(Color.Black, 1);

            goRect.Editable = true;
            SetDefaultGradientBehaviour(goRect);
            StartCreateTool(goRect);
            myView.FinishTransaction("Add rectangle");
            return goRect;
        }

        public GoObject AddTriangle()
        {
            GradientTriangle ctri = new GradientTriangle();
            SetDefaultGradientBehaviour(ctri);
            ctri.A = new PointF(0, 0);
            ctri.B = new PointF(0, 100);
            ctri.C = new PointF(100, 100);
            StartCreateTool(ctri);

            return ctri;
        }

        #endregion

        #region Add Stuff

        public RepeaterSection AddRepeaterSection()
        {
            int counter = 0;
            foreach (GoObject o in myView.Document)
            {
                if (o is RepeaterSection)
                {
                    counter++;
                }
            }
            RepeaterSection dlg = new RepeaterSection();
            dlg.RepeaterID = Guid.NewGuid();
            dlg.Name = "RepeaterSection " + counter.ToString();
            myView.Document.Add(dlg);
            return dlg;
        }

        public QLink AddConnectedLink()
        {
            QLink clink = new QLink();
            GoBasicNode basicStart = new GoBasicNode();
            basicStart.Brush = Brushes.Green;
            GoBasicNode basicEnd = new GoBasicNode();
            basicEnd.Position = new PointF(100, 100);
            basicEnd.Brush = Brushes.Red;
            clink.FromPort = basicStart.Port;
            clink.ToPort = basicEnd.Port;
            myView.Document.Add(clink);
            return clink;
        }

        public void AddPort()
        {
            myView.StartTransaction();
            Symbol s = myView.Document as Symbol;
            QuickPort p = new QuickPort(false);

            p.Style = GoPortStyle.Rectangle;
            p.Brush = Brushes.LightGray;
            p.Pen = new Pen(Color.Gray);
            p.Size = new SizeF(10, 10);
            p.Position = new PointF(1, 1);
            p.Selectable = true;
            p.AutoRescales = false;
            s.GetShapeContainer().Add(p);

            myView.FinishTransaction("Insert Port");
            StartToolPortMover();
        }

        public void StartToolPortMover()
        {
            myView.Tool = new PortMovingTool(myView);
        }

        public PointF GetCenter()
        {
            PointF pos = myView.DocPosition;
            SizeF siz = myView.DocExtentSize;
            PointF center = new PointF(pos.X + siz.Width / 2, pos.Y + siz.Height / 2);
            return center;
        }

        public ResizableComment AddComment()
        {
            myView.StartTransaction();
            ResizableComment gmc = new ResizableComment();
            gmc.Editable = true;
            gmc.Width = 200;
            gmc.Height = 100;
            gmc.Movable = true;
            gmc.Selectable = true;
            gmc.Resizable = true;
            gmc.Position = myView.LastInput.DocPoint; //GetCenter();
            gmc.Label.Editable = false;
            gmc.Text = "Comment";

            myView.Document.Add(gmc);
            //myView.BeginUpdate();
            //myView.ScrollLine(10, 10);
            //myView.ScrollLine(-10, -10);
            //myView.EndUpdate();
            myView.UpdateView();
            myView.FinishTransaction("Add Comment");
            gmc.DoBeginEdit(myView);
            return gmc;
        }

        public ResizableBalloonComment AddBalloonComment()
        {
            myView.StartTransaction();
            ResizableBalloonComment nballoon = new ResizableBalloonComment();
            nballoon.Editable = false;
            nballoon.Movable = true;
            nballoon.Selectable = true;
            nballoon.Resizable = true;

            nballoon.Text = "Balloon Comment";
            nballoon.Position = myView.LastInput.DocPoint;
            if (myView.Selection.Primary != null && !(myView.Selection.Primary is FrameLayerRect))
            {
                nballoon.Anchor = myView.Selection.Primary;
                nballoon.Position = new PointF(myView.Selection.Primary.Right + 50, myView.Selection.Primary.Position.Y + 50);
            }
            else
            {
                nballoon.Position = myView.LastInput.DocPoint;
            }
            nballoon.Reanchorable = true;
            nballoon.Label.Editable = false;
            myView.Document.Add(nballoon);
            //myView.BeginUpdate();
            //myView.ScrollLine(10, 10);
            //myView.ScrollLine(-10, -10);
            //myView.EndUpdate();
            myView.UpdateView();
            myView.FinishTransaction("Add Comment");
            nballoon.DoBeginEdit(myView);
            return nballoon;
        }

        public Rationale AddAndParentRationale(GoObject parent, MetaBase rationaleBase)
        {
            Rationale nballoon = new Rationale();
            nballoon.Editable = false;
            nballoon.Movable = true;
            nballoon.Selectable = true;
            nballoon.Resizable = true;
            nballoon.MetaObject = rationaleBase;
            nballoon.HookupEvents();
            nballoon.BindToMetaObjectProperties();
            nballoon.Position = myView.LastInput.DocPoint;
            myView.Document.Add(nballoon);
            nballoon.Anchor = parent;
            nballoon.Position = new PointF(parent.Right + 50, parent.Location.Y + 50);
            nballoon.Reanchorable = true;
            myView.UpdateView();
            return nballoon;
        }

        public Rationale AddRationale()
        {
            myView.StartTransaction();
            Rationale nballoon = new Rationale();
            nballoon.Editable = false;
            nballoon.Movable = true;
            nballoon.Selectable = true;
            nballoon.Resizable = true;
            nballoon.CreateMetaObject(this, EventArgs.Empty);
            nballoon.HookupEvents();
            nballoon.BindToMetaObjectProperties();
            nballoon.Position = myView.LastInput.DocPoint;
            myView.Document.Add(nballoon);
            if (myView.Selection.Primary != null)
            {
                nballoon.Anchor = myView.Selection.Primary;
                if (myView.Selection.Primary.TopLevelObject is ArtefactNode)
                {
                    nballoon.Anchor = myView.Selection.Primary.TopLevelObject;
                }
                nballoon.Position = new PointF(myView.Selection.Primary.Right + 50, myView.Selection.Primary.Position.Y + 50);
                myView.Selection.Clear();
                myView.Selection.Add(nballoon);
            }
            nballoon.Reanchorable = true;
            //myView.BeginUpdate();
            //myView.ScrollLine(10, 10);
            //myView.ScrollLine(-10, -10);
            //myView.EndUpdate();
            myView.UpdateView();
            myView.FinishTransaction("Add Rationale");
            nballoon.DoBeginEdit(myView);
            return nballoon;
        }

        public Rationale AddRationale(Meta.MetaBase mbase)
        {
            Rationale nballoon = new Rationale();
            nballoon.Editable = false;
            nballoon.Movable = true;
            nballoon.Selectable = true;
            nballoon.Resizable = true;
            nballoon.Reanchorable = true;

            if (mbase.Class == "Rationale")
                nballoon.CreateMetaObject(this, EventArgs.Empty);
            else
            {
                nballoon.Text = mbase.Class;
                nballoon.BindingInfo.BindingClass = mbase.Class;
            }

            nballoon.MetaObject = mbase;
            nballoon.HookupEvents();
            nballoon.BindToMetaObjectProperties();
            nballoon.Position = myView.LastInput.DocPoint;
            myView.Document.Add(nballoon);
            return nballoon;
        }

        public void AddArrow()
        {
            myView.StartTransaction();
            GradientArrow arrow = new GradientArrow();
            arrow.Position = myView.LastInput.DocPoint;
            SetDefaultGradientBehaviour(arrow);
            myView.Document.Add(arrow);
            myView.FinishTransaction("Add arrow");
        }

        public void AddBlockArrow()
        {
            myView.StartTransaction();
            float width = 200;
            float height = 75;
            float nose = 25;
            float tail = 25;
            GradientValueChainStep gvs = new GradientValueChainStep(nose, tail, width, height, false);

            gvs.Position = myView.LastInput.DocPoint;
            SetDefaultGradientBehaviour(gvs);
            myView.Document.Add(gvs);
            myView.FinishTransaction("Add block arrow");
        }

        public BoundLabel AddLabel()
        {
            return AddLabel(false);
        }

        public void AddRichTextLabel()
        {
            myView.StartTransaction();
            RichText rt = new RichText();
            rt.Rtf =
                @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fcharset0 Arial;}{\f1\fswiss\fcharset0 MS Sans Serif;}{\f2\fmodern\fcharset0 Courier New;}}
{\colortbl ;\red255\green0\blue0;\red0\green0\blue255;\red0\green128\blue0;\red128\green0\blue128;}
\viewkind4\uc1\pard\f0\fs20 RichText\par
}";
            rt.Width = 200;
            rt.Height = 200;
            rt.Position = myView.LastInput.DocPoint;
            myView.Document.Add(rt);
            myView.FinishTransaction("Add Label");
        }

        private int CountLabels()
        {
            int retval = 0;
            try
            {
                foreach (GoObject obj in myView.Document)
                {
                    if (obj is BoundLabel)
                        retval++;
                    if (obj is GraphNode)
                    {
                        foreach (GoObject oo in (obj as GraphNode))
                        {
                            if (oo is BoundLabel)
                                retval++;
                        }
                    }
                }
            }
            catch
            {
            }
            return retval;
        }

        public BoundLabel AddLabel(bool WithRectangle)
        {
            myView.StartTransaction();
            BoundLabel blabel = new BoundLabel();
            blabel.Editable = false;
            blabel.Selectable = true;
            blabel.Text = "New Text Label";
            blabel.Name = blabel.Text;
            blabel.Resizable = true;
            blabel.Multiline = true;
            blabel.AutoRescales = false;
            blabel.Clipping = false;
            blabel.AutoResizes = true;
            blabel.Wrapping = true;
            blabel.ResizesRealtime = true;

            myView.Document.Add(blabel);

            blabel.Position = myView.LastInput.DocPoint;
            myView.FinishTransaction("Add Label");
            return blabel;
        }

        public QuickImage AddImage()
        {
            myView.StartTransaction();
            QuickImage addedImage = new QuickImage();
            OpenFileDialog openImageDialog = new OpenFileDialog();
            openImageDialog.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
            openImageDialog.Filter = "Supported Image Types (jpg, gif, bmp, png, ico)|*.jpg;*.bmp;*.gif;*.png;*.ico";
            DialogResult result = openImageDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                myView.Document.Add(addedImage);
                //addedImage.Image = Image.FromFile(openImageDialog.FileName);
                addedImage.Filename = openImageDialog.FileName;
                addedImage.Reshapable = true;
                addedImage.Resizable = true;
            }
            addedImage.Position = myView.LastInput.DocPoint;
            myView.FinishTransaction("Add Image");
            return addedImage;
        }

        public GradientCube AddCube()
        {
            myView.StartTransaction();
            GradientCube cube = new GradientCube();
            cube.Height = 50;
            cube.Width = 50;
            SetDefaultGradientBehaviour(cube);
            myView.Document.Add(cube);
            cube.Position = myView.LastInput.DocPoint;
            myView.FinishTransaction("Add cube");
            return cube;
        }

        private static void SetDefaultGradientBehaviour(GoShape shape)
        {
            IBehaviourShape ibshape = shape as IBehaviourShape;
            GradientBehaviour gbehaviour = new GradientBehaviour();
            gbehaviour.MyBrush = new ShapeGradientBrush();
            gbehaviour.MyBrush.OuterColor = Color.LightGreen;
            gbehaviour.MyBrush.InnerColor = Color.LightGreen;
            gbehaviour.MyBrush.GradientType = GradientType.ForwardDiagonal;
            ibshape.Manager = new BaseShapeManager();
            gbehaviour.Owner = shape;
            ibshape.Manager.AddBehaviour(gbehaviour);
            gbehaviour.Update(shape);
        }

        public GradientCylinder AddCylinder(Orientation orientation)
        {
            myView.StartTransaction();
            GradientCylinder cylinder = new GradientCylinder();
            cylinder.Height = 50;
            cylinder.Width = 50;
            cylinder.Perspective = GoPerspective.TopLeft;
            cylinder.Orientation = orientation;
            SetDefaultGradientBehaviour(cylinder);
            myView.Document.Add(cylinder);
            cylinder.Position = myView.LastInput.DocPoint;
            myView.FinishTransaction("Add cylinder");
            return cylinder;
        }

        public GradientOctagon AddOctagon()
        {
            myView.StartTransaction();
            GradientOctagon oct = new GradientOctagon();
            oct.Height = 50;
            oct.Width = 50;
            SetDefaultGradientBehaviour(oct);
            myView.Document.Add(oct);
            oct.Position = myView.LastInput.DocPoint;
            myView.FinishTransaction("Add octagon");
            return oct;
        }

        public GradientHexagon AddHexagon()
        {
            myView.StartTransaction();
            GradientHexagon hex = new GradientHexagon();
            hex.Height = 50;
            hex.Width = 50;
            SetDefaultGradientBehaviour(hex);
            myView.Document.Add(hex);
            hex.Position = myView.LastInput.DocPoint;
            myView.FinishTransaction("Add hexagon");
            return hex;
        }

        public HouseShape AddHouseShape()
        {
            myView.StartTransaction();
            HouseShape hshape = new HouseShape();
            hshape.Height = 50;
            hshape.Width = 50;
            hshape.Direction = 32;
            SetDefaultGradientBehaviour(hshape);
            myView.Document.Add(hshape);
            hshape.Position = myView.LastInput.DocPoint;
            myView.FinishTransaction("Add house shape");
            return hshape;
        }

        public GradientDiamond AddDiamond()
        {
            myView.StartTransaction();
            GradientDiamond dmd = new GradientDiamond();
            dmd.Height = 50;
            dmd.Width = 50;
            myView.Document.Add(dmd);
            SetDefaultGradientBehaviour(dmd);
            dmd.Position = myView.LastInput.DocPoint;
            myView.FinishTransaction("Add diamond");
            return dmd;
        }

        public VisualNode AddTextNode(GoNode anchor)
        {
            if (anchor == null)
                return null;
            myView.StartTransaction();
            //add node
            VisualNode node = new VisualNode(anchor);
            MyView.Document.Add(node);
            //link node
            VisualLink link = new VisualLink();
            link.SetStyle(System.Drawing.Drawing2D.DashStyle.DashDot);
            link.ToPort = node.Port as IGoPort;
            //always use right port
            foreach (IGoPort p in anchor.Ports.GetEnumerator())
                if (p != null)
                    if (p is QuickPort)
                        if ((p as QuickPort).PortPosition == QuickPortHelper.QuickPortLocation.Right)
                        {
                            link.FromPort = p as IGoPort;
                            break;
                        }

            if (link.FromPort != null && link.ToPort != null)
                MyView.Document.Add(link);

            MyView.FinishTransaction("AddTextNode");
            return node;
        }

        /// <summary>
        /// Adds the hyperlink.
        /// </summary>
        /// <returns></returns>
        public Hyperlink AddHyperlink()
        {
            myView.StartTransaction();
            Hyperlink txt = new Hyperlink();
            txt.Underline = true;
            txt.Text = "New Hyperlink";
            txt.Arguments = "http://www.discon.co.za";
            myView.Document.Add(txt);
            txt.Position = myView.LastInput.DocPoint;
            myView.FinishTransaction("Add hyperlink");
            return txt;
        }

        public GradientTrapezoid AddTrapezoid()
        {
            myView.StartTransaction();
            GradientTrapezoid trap = new GradientTrapezoid();
            trap.A = new PointF(10, 10);
            trap.B = new PointF(150, 10);
            trap.C = new PointF(10, 110);
            trap.D = new PointF(150, 110);
            SetDefaultGradientBehaviour(trap);
            myView.Document.Add(trap);
            trap.Position = myView.LastInput.DocPoint;
            myView.FinishTransaction("Add trapezoid");
            return trap;
        }

        public void AddAttachment(string filename)
        {
            SerializableAttachment sattachment = new SerializableAttachment();
            sattachment.Load(filename);
            sattachment.Position = myView.LastInput.DocPoint;
            myView.Document.Add(sattachment);
        }

        #endregion

        public GoView MyView
        {
            get { return myView; }
            set { myView = value; }
        }
    }
}