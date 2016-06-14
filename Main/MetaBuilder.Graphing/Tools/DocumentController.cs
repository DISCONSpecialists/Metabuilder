using System;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using Northwoods.Go;
using System.Drawing;

namespace MetaBuilder.Graphing.Tools
{
    public class DocumentController
    {
        #region Fields (1)

        [NonSerialized]
        private GoDocument document;

        #endregion Fields

        #region Constructors (1)

        public DocumentController(GoDocument document)
        {
            this.document = document;
        }

        #endregion Constructors

        #region Properties (1)

        public GoDocument Document
        {
            get { return document; }
            set { document = value; }
        }

        #endregion Properties

        #region Other Methods - Most of these are KAK methods

        public void ApplyBrushes()
        {
            ApplyBrushesForCollection(document);
        }
        public void RemoveBrushes()
        {
            RemoveBrushesForCollection(document);
        }

        private void RemoveBrushesForCollection(IGoCollection coll)
        {
            // HACK - following line replaces behaviours with new copies of the behaviours
            CreateNewInstancesOfBehaviours(coll);
            foreach (GoObject o in coll)
            {
                //if (o is BoundLabel)
                //{
                //    (o as BoundLabel).SetColor();// .ColorBeforeBlackAndWhitePrint = (o as BoundLabel).TextColor;
                //    (o as BoundLabel).TextColor = System.Drawing.Color.Black;
                //}
                //else if (o is GoText)
                //{
                //    //image nodes text will not be coloured back
                //    (o as GoText).TextColor = System.Drawing.Color.Black;
                //}

                if (o is GoCollapsibleHandle)
                    (o as GoCollapsibleHandle).Brush = Brushes.White;//new System.Drawing.SolidBrush(System.Drawing.Color.White);

                if (o is QLink)
                {
                    System.Drawing.Pen p = new System.Drawing.Pen(System.Drawing.Color.Black);
                    p.DashStyle = (o as QLink).Pen.DashStyle;
                    (o as QLink).Pen = p;
                }
                else if (o is FishLink)
                {
                    (o as FishLink).Pen = new System.Drawing.Pen(System.Drawing.Color.Black);
                    (o as FishLink).Brush = Brushes.Black;//new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                }

                if (o is IBehaviourShape)
                {
                    GradientBehaviour gradBehaviour = (o as IBehaviourShape).Manager.GetExistingBehaviour(typeof(GradientBehaviour)) as GradientBehaviour;
                    if (gradBehaviour != null)
                        gradBehaviour.Disable(o);
                }
                if (o is IGoCollection)
                {
                    RemoveBrushesForCollection(o as IGoCollection);
                }
                if (o is IMetaNode)
                {
                    try
                    {
                        foreach (GoObject ochild in o as GoGroup)
                        {
                            if (ochild is GoImage)
                            {
                                //set to black and white
                                (ochild as GoImage).Image = GrayScale((Bitmap)(ochild as GoImage).Image);//.Palette.ToString();
                            }
                            ochild.Selectable = false;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public Bitmap GrayScale(Bitmap Bmp)
        {
            int rgb;
            Color c;

            for (int y = 0; y < Bmp.Height; y++)
                for (int x = 0; x < Bmp.Width; x++)
                {
                    c = Bmp.GetPixel(x, y);
                    rgb = (int)((c.R + c.G + c.B) / 3);
                    Bmp.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
            return Bmp;
        }

        private void ApplyBrushesForCollection(IGoCollection coll)
        {
            // HACK - following line replaces behaviours with new copies of the behaviours
            CreateNewInstancesOfBehaviours(coll);
            foreach (GoObject o in coll)
            {
                //if (o is BoundLabel)
                //{
                //    Color c = (o as BoundLabel).GetColor();
                //    if (c != Color.Empty)//.IsEmpty
                //    {
                //        if ((o as BoundLabel).Name == "cls_id")
                //        {
                //            (o as BoundLabel).TextColor = Color.Gray;
                //        }
                //        else
                //        {
                //            (o as BoundLabel).TextColor = c;// (o as BoundLabel).ColorBeforeBlackAndWhitePrint;
                //        }
                //    }

                //}
                //else if (o is GoText)
                //{
                //    (o as GoText).TextColor = System.Drawing.Color.Black;
                //}

                if (o is GoCollapsibleHandle)
                {
                    (o as GoCollapsibleHandle).Brush = Brushes.Yellow;// new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
                }
                if (o is QLink)
                {
                    System.Drawing.Pen p = new System.Drawing.Pen((o as QLink).PenColorBeforeCompare);
                    p.DashStyle = (o as QLink).Pen.DashStyle;
                    (o as QLink).Pen = p;
                }
                else if (o is FishLink)
                {
                    (o as FishLink).Pen = Pens.LightBlue;// new System.Drawing.Pen(System.Drawing.Color.LightBlue);
                    (o as FishLink).Brush = Brushes.LightBlue; //new System.Drawing.SolidBrush(System.Drawing.Color.LightBlue);
                }

                if (o is IBehaviourShape)
                {
                    GradientBehaviour gradBehaviour = (o as IBehaviourShape).Manager.GetExistingBehaviour(typeof(GradientBehaviour)) as GradientBehaviour;
                    if (gradBehaviour != null)
                        gradBehaviour.Update(o);
                }
                if (o is IGoCollection)
                {
                    ApplyBrushesForCollection(o as IGoCollection);
                }
                if (o is IMetaNode)
                {
                    try
                    {
                        foreach (GoObject ochild in o as GoGroup)
                        {
                            if (ochild is GoImage)
                            {
                                //set back to color by resetting image
                                if (o is GraphNode)
                                    (o as GraphNode).loadedImage = false;
                                (o as IMetaNode).BindMetaObjectImage();
                            }
                            ochild.Selectable = false;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void CreateNewInstancesOfBehaviours(IGoCollection coll)
        {
            foreach (GoObject o in coll)
            {
                if (o is GraphNode)
                {
                    GraphNode n = o as GraphNode;
                    foreach (GoObject oChild in n)
                    {
                        if (oChild is IBehaviourShape)
                        {
                            IBehaviourShape ibehaviourShape = oChild as IBehaviourShape;
                            //ibehaviourShape.Manager = ibehaviourShape.Manager.Copy(oChild);
                            GradientBehaviour gbeh =
                                ibehaviourShape.Manager.GetExistingBehaviour(typeof(GradientBehaviour)) as
                                GradientBehaviour;
                            if (gbeh != null)
                            {
                                gbeh.Owner = oChild;
                                gbeh.Update(oChild);
                            }
                        }
                    }
                }
            }
        }

        public virtual int NumLinksBetween(IGoPort a, IGoPort b)
        {
            int count = 0;
            foreach (IGoLink l in a.DestinationLinks)
            {
                if (l.ToPort == b)
                    count++;
            }
            return count;
        }

        #endregion
    }
}