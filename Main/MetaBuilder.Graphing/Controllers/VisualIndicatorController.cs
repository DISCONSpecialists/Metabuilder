using System.Drawing;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Controllers
{
    public class VisualIndicatorController
    {
        //DATA STATE
        public void AddChangedIndicator(string s, Color c, GoGroup node)
        {
            if (node.Document != null)
            {
                node.Document.SkipsUndoManager = true;
                node.Document.StartTransaction();
            }
            // Find a normal indicator (if necessary)
            float OffSet = 0;
            foreach (GoObject o in node)
            {
                if (o is IndicatorLabel && (!(o is ChangedIndicatorLabel)))
                {
                    OffSet = o.Bottom;
                }
            }
            RemoveChangedIndicators(node, true);
            ChangedIndicatorLabel txt = new ChangedIndicatorLabel(s, c);

            PointF labelPoint;
            if (node is CollapsibleNode)
            {
                labelPoint = new PointF(node.Center.X - txt.Width / 2f, node.Top + txt.Height + 25);
            }
            else
            {
                labelPoint = new PointF(node.Center.X - txt.Width / 2f, node.Center.Y - txt.Height / 2f);
            }
            if (OffSet != 0)
            {
                labelPoint.Y = OffSet + 5;
            }
            txt.Position = labelPoint;
            node.Add(txt);

            if (node.Document != null)
            {
                node.Document.FinishTransaction("Adding Change Indicator");
                node.Document.SkipsUndoManager = false;
            }
        }

        //VC STATE
        public void AddIndicator(string s, Color c, GoGroup node)
        {
            if (node.Document != null)
            {
                node.Document.SkipsUndoManager = true;
                node.Document.StartTransaction();
            }

            RemoveIndicators(node);
            IndicatorLabel txt = new IndicatorLabel(s, c);

            if (node is CollapsibleNode)
            {
                txt.Position = new PointF(node.Center.X - txt.Width / 2f, node.Top + txt.Height + 25);
            }
            else
            {
                txt.Position = new PointF(node.Center.X - txt.Width / 2f, node.Center.Y - txt.Height / 2f);
            }

            foreach (GoObject o in node)
            {
                if (o is ChangedIndicatorLabel)
                {
                    o.Position = new PointF(node.Center.X - o.Width / 2f, txt.Bottom + 5);
                }
            }
            node.Add(txt);
            if (node.Document != null)
            {
                node.Document.FinishTransaction("Adding Indicator");
                node.Document.SkipsUndoManager = false;
            }
        }

        public void RemoveChangedIndicators(IGoCollection collection, bool overrideRules)
        {
            foreach (GoObject o in collection)
            {
                if (o is ChangedIndicatorLabel)
                {
                    if (overrideRules)
                    {
                        o.Remove();
                        continue;
                    }
                    if ((o as ChangedIndicatorLabel).Text != "Marked For Delete" && (o as ChangedIndicatorLabel).Text != "Not in Database") //7 January 2013
                        o.Remove();
                }

                if (o is IGoCollection)
                    RemoveChangedIndicators(o as IGoCollection, overrideRules);
            }
        }

        //on save we must not remove MFD, because items remain MFD after save
        public void RemoveSavedChangedIndicators(IGoCollection collection, bool overrideRules)
        {
            foreach (GoObject o in collection)
            {
                if (o is ChangedIndicatorLabel)
                {
                    if ((o as ChangedIndicatorLabel).Text != "Marked For Delete")
                        o.Remove();
                }

                if (o is IGoCollection)
                    RemoveChangedIndicators(o as IGoCollection, overrideRules);
            }
        }

        public void RemoveIndicators(IGoCollection node)
        {
            // Removes VC Indicators Only
            foreach (GoObject o in node)
            {
                if ((o is IndicatorLabel) && (!(o is ChangedIndicatorLabel)) && (!o.GetType().ToString().Contains("NumberingText")))
                {
                    IndicatorLabel lbl = o as IndicatorLabel;
                    if (!(IsChangedIndicator(lbl.Text)))
                        if (IsVCStatusIndicator(lbl.Text))
                            o.Remove();

                    if (o is IGoCollection)
                    {
                        RemoveIndicators(o as IGoCollection);
                    }
                }
            }
        }

        private bool IsChangedIndicator(string text)
        {
            return (text == "Differs from DB" || text == "MarkedForDelete" || text == "Marked For Delete" || text == "Not in Database");
        }

        private bool IsVCStatusIndicator(string text)
        {
            return (text == "Locked" || text == "CheckedOutRead" || text == "ReadOnly" || text == "CheckedIn" || text == "Obsolete");
        }

    }
}