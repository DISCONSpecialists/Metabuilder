using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Controllers
{
    /// <summary>
    /// Primarily used to align selections of a goview. Only nodes and shapes can be aligned.
    /// </summary>
    public class AlignmentControl
    {
        #region Alignment etc

        public enum AlignmentType
        {
            Tops,
            Bottoms,
            VerticalCenters,
            HorizontalCenters,
            Lefts,
            Rights
        }

        public static void DoAlign(GoView view, AlignmentType type)
        {
            float var = 0;
            GoObject obj = view.Selection.Primary;
            if (obj != null && !(obj is IGoLink))
            {
                view.StartTransaction();
                switch (type)
                {
                    case AlignmentType.Tops:
                        var = obj.SelectionObject.Top;
                        break;
                    case AlignmentType.Rights:
                        var = obj.SelectionObject.Right;
                        break;
                    case AlignmentType.HorizontalCenters:
                        var = obj.SelectionObject.Center.X;
                        break;
                    case AlignmentType.Lefts:
                        var = obj.SelectionObject.Left;
                        break;
                    case AlignmentType.VerticalCenters:
                        var = obj.SelectionObject.Center.Y;
                        break;
                    case AlignmentType.Bottoms:
                        var = obj.SelectionObject.Bottom;
                        break;
                }
                foreach (GoObject temp in view.Selection)
                {
                    GoObject t = temp.SelectionObject;
                    if (!(t is IGoLink) && (!(t is IGoPort)))
                    {
                        switch (type)
                        {
                            case AlignmentType.Tops:
                                t.Top = var;
                                break;
                            case AlignmentType.Rights:
                                t.Right = var;
                                break;
                            case AlignmentType.HorizontalCenters:
                                t.Center = new PointF(var, t.Center.Y);
                                break;
                            case AlignmentType.Lefts:
                                t.Left = var;
                                break;
                            case AlignmentType.VerticalCenters:
                                t.Center = new PointF(t.Center.X, var);
                                break;
                            case AlignmentType.Bottoms:
                                t.Bottom = var;
                                break;
                        }
                        // ARTEFACTS ONLY
                        if (t.ParentNode != null)
                            if (t.ParentNode is ArtefactNode)
                            {
                                ArtefactNode artnode =
                                    t.ParentNode as ArtefactNode;

                                switch (type)
                                {
                                    case AlignmentType.Tops:
                                        artnode.Label.Alignment = GoObject.TopCenter;
                                        break;
                                    case AlignmentType.Rights:
                                        artnode.Right = view.Selection.Primary.Right;
                                        artnode.Label.Alignment = GoObject.MiddleRight;
                                        break;
                                    case AlignmentType.HorizontalCenters:
                                        artnode.Label.Alignment = GoObject.MiddleCenter;
                                        break;
                                    case AlignmentType.Lefts:
                                        artnode.Left = view.Selection.Primary.Left;
                                        artnode.Label.Alignment = GoObject.MiddleLeft;
                                        break;
                                    case AlignmentType.VerticalCenters:
                                        artnode.Label.Alignment = GoObject.MiddleCenter;
                                        break;
                                    case AlignmentType.Bottoms:
                                        artnode.Label.Alignment = GoObject.MiddleBottom;
                                        break;
                                }
                            }
                    }
                }
                view.FinishTransaction("Align " + type.ToString());
            }
            else
            {
                MessageBox.Show("Alignment failure: Primary Selection is empty or a link instead of a node.", "Failed",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}