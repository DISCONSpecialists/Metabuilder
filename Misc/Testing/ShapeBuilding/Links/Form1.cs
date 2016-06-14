using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MetaBuilder.Graphing;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Meta;


namespace ShapeBuilding.Links
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            
            float Y = 0;
            foreach (LinkAssociationType linkType in Enum.GetValues(typeof(LinkAssociationType)))
            {
                float X = 175;
                Y += 50;

                QNode n1 = new QNode();
                n1.Text = linkType.ToString();
                goDrawView1.Document.Add(n1);
                n1.Location = new PointF(0,Y);

                QNode n2 = new QNode();
                n2.Text = linkType.ToString();
                goDrawView1.Document.Add(n2);
                n2.Location = new PointF(X, Y);

                QLink lnk = new QLink();
                lnk.AssociationType = linkType;
                lnk.FromPort = n1.Port;
                lnk.ToPort = n2.Port;
                goDrawView1.Document.Add(lnk);
            }






        }

        private void goDrawView1_SelectionFinished(object sender, EventArgs e)
        {
            if (goDrawView1.Selection.Primary!=null)
                propertyGrid1.SelectedObject = goDrawView1.Selection.Primary;
        }

        private void goDrawView1_ObjectGotSelection(object sender, Northwoods.Go.GoSelectionEventArgs e)
        {
            if (goDrawView1.Selection.Primary != null)
                propertyGrid1.SelectedObject = goDrawView1.Selection.Primary;
        }
    }
}