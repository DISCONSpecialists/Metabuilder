using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Graphing.Persistence;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using MetaBuilder.Graphing.Shapes.Primitives;
using Northwoods.Go;
using Northwoods.Go.Xml;
using MetaBuilder.Graphing.Containers;
namespace ShapeBuilding
{
    public partial class ShapesToXML : Form
    {
        public ShapesToXML()
        {
            InitializeComponent();
        }
        private void ShapesToXML_Load(object sender, EventArgs e)
        {
            NormalDiagram ndiagram = new NormalDiagram();
            graphView1.Document = ndiagram;
            ndiagram.CreateFrameLayer(graphView1);

            AddShapesToBeTested();


            Persist();

        
         

        }
        private class TestShape
        {
            public TestShape(string file, PointF position)
            {
                this.Filename = file;
                this.Position = position;
            }

            private string filename;

            public string Filename
            {
                get { return filename; }
                set { filename = value; }
            }

            private PointF position;

            public PointF Position
            {
                get { return position; }
                set { position = value; }
            }
	
	
        }

        private TestShape tsDT;
        private TestShape tsDE;
        private TestShape tsRP;
        private TestShape tsDV;
        private void AddShapesToBeTested()
        {
            string pathShapes = @"C:\complex shapes\";
            tsDT = new TestShape(pathShapes + "NewDataTable.sym", new PointF(10, 10));
            tsDE = new TestShape(pathShapes + "NewEntity.sym", new PointF(200, 10));
            tsRP = new TestShape(pathShapes + "NewReport.sym", new PointF(10, 200));
            tsDV = new TestShape(pathShapes + "NewDataView.sym", new PointF(200, 200));

            AddItem(tsDT);
            AddItem(tsDE);

            
            AddItem(tsRP);
            AddItem(tsDV);

            
        }

        private void AddItem(TestShape shape)
        {
            GoObject sym =
                MetaBuilder.Graphing.Utilities.StorageManipulator.FileSystemManipulator.LoadSymbol(shape.Filename);
            graphView1.Document.Add(sym);
            
            MetaBuilder.Graphing.Helpers.Debuggers.ShapeHelper.DebugObjectToXml(sym as IGoCollection,"c:\\" + MetaBuilder.Core.strings.GetFileNameOnly(shape.Filename) + ".xml");
            
            if (sym is GoGroup)
            {
                LockItems(sym);
            }

            sym.Position = shape.Position;

        }

        private void LockItems(GoObject sym)
        {
            GoGroup grp = sym as GoGroup;
            GoGroupEnumerator groupEnum = grp.GetEnumerator();
            while (groupEnum.MoveNext())
            {
                groupEnum.Current.Deletable = false;
                groupEnum.Current.Reshapable = false;
                groupEnum.Current.Resizable = false;
                groupEnum.Current.Movable = true;
                groupEnum.Current.DragsNode = true;

                GoObject o = groupEnum.Current;
                if (o is GoGroup)
                    LockItems(o);
            }
        }


        private void btnSerialize_Click(object sender, EventArgs e)
        {
            Persist();
        }

        private void Persist()
        {
            AddIDs(graphView1.Document);
            MetaBuilder.Graphing.Persistence.XmlPersistor pers = new XmlPersistor();
            //pers.PersistDiagram(graphView1.Document as NormalDiagram,"C:\\test.mdgm");
        }

        private void AddIDs(IGoCollection collection)
        {
            foreach (GoObject o in collection)
            {    if (o is IIdentifiable)
                {
                    IIdentifiable idObj = o as IIdentifiable;
                    idObj.Name = Guid.NewGuid().ToString();
                }
                if (o is IGoCollection )
                {
                    AddIDs(o as IGoCollection);
                }
            }
            
        }

        private void btnDeserialize_Click(object sender, EventArgs e)
        {
            Depersist();
            tsDT.Position = new PointF(tsDT.Position.X, tsDT.Position.Y + 400);
            tsRP.Position = new PointF(tsDT.Position.X, tsDT.Position.Y + 400);
            tsDE.Position = new PointF(tsDT.Position.X + 200, tsDT.Position.Y);
            tsDV.Position = new PointF(tsDT.Position.X + 200, tsDT.Position.Y + 200);

            AddItem(tsDT);
            AddItem(tsDE);
            AddItem(tsRP);
            AddItem(tsDV);
        }

        private void Depersist()
        {
          

        
        }
    }
}