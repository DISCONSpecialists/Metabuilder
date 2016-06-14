using System;
using System.Windows.Forms;
using Northwoods.Go;

namespace ShapeBuilding
{
    public class SubGrapher
    {
        public void MakeSubGraph(Type type, GoSelection sel,GoView view, SubGraphDraggingTool tool)
        {
            // get the selected nodes
            GoCollection coll = new GoCollection();
            // find the common parent, if any, to find new subgraph's parent
            GoObject common = null;
            // also remember the layer
            GoLayer layer = null;
            bool first = true;
            foreach (GoObject obj in sel)
            {
                if (obj is IGoNode)
                {
                    coll.Add(obj);  // remembering selected nodes
                    if (first)
                    {
                        first = false;
                        common = obj.Parent;
                        layer = obj.Layer;
                    }
                    else
                    {
                        common = GoObject.FindCommonParent(common, obj);
                        if (obj.Layer != layer)
                        {
                            MessageBox.Show("Can't mix layers of objects to be grouped into a GoSubGraph");
                            return;
                        }
                    }
                }
            }
            if (coll.IsEmpty)
            {
                MessageBox.Show("Need to select some nodes");
            }
            else
            {
                view.StartTransaction();
                // determine the links that also should be grouped along with those nodes
                GoSubGraph sg = (GoSubGraph)Activator.CreateInstance(type);
                tool = (SubGraphDraggingTool)view.FindMouseTool(typeof(SubGraphDraggingTool),true);

                    GoSelection whole = tool.ComputeEffectiveSelection(coll, true);
                    // create a subgraph and add it to the document
                    
                    sg.Text = (myCounter++).ToString();
                    sg.Position = whole.Primary.Position;
                    // if there is a common parent, and it is a GoSubGraph, add the new one there
                    if (common is GoSubGraph)
                    {
                        ((GoSubGraph)common).Add(sg);
                    }
                    else
                    {  // otherwise just add as a top-level object
                        view.Document.Add(sg);
                    }
                    // move the objects into the subgraph, and
                    // make sure all links belong to the proper subgraph or as a top-level link
                    sg.AddCollection(whole, true);
   
                // afterwards, make the new subgraph the only selected object
                sel.Select(sg);
                view.FinishTransaction("grouped subgraph");
            }
        }
        private int myCounter = 0;
    }
}
