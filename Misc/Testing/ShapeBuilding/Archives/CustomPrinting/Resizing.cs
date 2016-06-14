using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Northwoods.Go;
using MetaBuilder.Graphing.Containers;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
namespace ShapeBuilding.CustomPrinting
{
    [TestFixture]
    public class Resizing
    {
        public Resizing(PrintableView view)
        {
            this.view = view;

        }

        PrintableView view;
        NormalDiagram diagram;

        /// <summary>
        /// This method sets up the view with a default size diagram and two nodes
        /// </summary>
        [SetUp]
        public void DoSetup()
        {
            diagram = new NormalDiagram();
            diagram.Initialise();
            //diagram.SetupFrameLayer();
            diagram.DocumentFrame.Size = new SizeF(2306, 3342);
            view.Document = diagram;
            view.Sheet.Size = new SizeF(2506, 3542);


            diagram.DocumentFrame.Position = view.SheetCorner;
            // node 1 is top left corner
            System.Drawing.Printing.PaperSize psize = new PaperSize("A4", 2100, 2920);

           // InitializeSheet( psize);

            GoBasicNode node1 = new GoBasicNode();
            node1.Text = "Basic Node 1";
            node1.Movable = true;
            diagram.Add(node1);
            node1.Size = new SizeF(200, 200);
            node1.Position = new PointF(view.Sheet.MarginBounds.Left, view.Sheet.MarginBounds.Top);
            
            // node 2 is bottom right corner
            GoBasicNode node2 = new GoBasicNode();
            diagram.Add(node2); 
            node2.Text = "Basic Node 2";
            node2.Size = new SizeF(200, 200);
            node2.Position = new PointF(view.Sheet.MarginBounds.Right - node2.Width, view.Sheet.MarginBounds.Bottom - node2.Height);
            node2.Movable = true;

        }
        private void InitializeSheet(PaperSize psize)
        {
            view.Sheet.Size = new SizeF(psize.Width, psize.Height);
        }
        [Test]
        public void Test800x600to800x600()
        {
            diagram.DocumentFrame.Width = 600;
            diagram.DocumentFrame.Height = 800;
            PrintingVariables pv = new PrintingVariables();
            pv.DocumentSize = diagram.DocumentFrame.Size;
            pv.TargetPageInner = new SizeF(600,800);
        }



    }
}
