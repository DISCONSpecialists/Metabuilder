using System.Drawing;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Utilities;
using Northwoods.Go;
using mutil = MetaBuilder.Graphing.Utilities;

namespace ShapeBuilding
{
    public class NodeDropper
    {
        public void DropNodes(GoView view)
        {
            string SymbolPath = @"e:\Function.sym";
            GraphNode node = StorageManipulator.FileSystemManipulator.LoadSymbol(SymbolPath) as GraphNode;
            node.EditMode = false;
            view.Document.Add(node);
            node.Position = new PointF(0, 0);
            node = StorageManipulator.FileSystemManipulator.LoadSymbol(SymbolPath) as GraphNode;
            view.Document.Add(node);
            node.EditMode = false;
            node.Position = new PointF(100, 100);
            node = StorageManipulator.FileSystemManipulator.LoadSymbol(SymbolPath) as GraphNode;
            view.Document.Add(node);
            node.EditMode = false;
            node.Position = new PointF(200, 200);
        }
    }
}
