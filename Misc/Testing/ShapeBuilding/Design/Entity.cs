using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;


namespace ShapeBuilding.Design
{

    /*
  <collapsibleNode 
     *  cls="DataTable" 
     *  lbls="DataTable|Name~txtINIT|InitialPopulation~txtGROWTH|GrowthRateOverTime~txtRecordSize|RecordSize~"
     *  id="118" xy="20 5 180 126" expanded="true" 
     * 
     * --- LIST BINDING
     * Binding="Attribute,Forward,Name,2341,LISTNAME,Key Attributes|Attribute,Forward,Name,2584,LISTNAME2,Descriptive Attributes|
     *          CLASS,?????,PROPERTY,CAID,LISTNAME,CAPTION
     * 
     * ">
    <collapsibleNodeItemList id="67" Print="true" Reshape="true" Resize="false" Move="true" Deletable="true" xy="25 60 170 67" Expanded="true">
      
     <expandableLabelList Name="LISTNAME" HeaderText="Key Columns" Width="169.7" Expanded="false" BoundProperty="Name" ChildClass="Attribute" />
     <expandableLabelList Name="LISTNAME2" HeaderText="Descriptive Columns" Width="169.7" Expanded="false" BoundProperty="Name" ChildClass="Attribute" />

    </collapsibleNodeItemList>
    
  </collapsibleNode>
     * */

    /*
    public class ObjectAssociationList
    {
        public int CAID;
        public IMetaObjectList IList;
    }
    public class ObjectList
    {
        public IMetaObjectList IList;
    }
    public class ContainerNode:GoNode
    {
        public Dictionary<ListBinding, CollapsibleNodeList> ListDictionary;
        public MetaBase MyMB;
        public MetaBuilder.Graphing.Shapes.BindingInfo MyBindingInfo;
        
        public List<BoundList> Lists;
        public ContainerNode()
        {
            MyBindingInfo = new MetaBuilder.Graphing.Shapes.BindingInfo();
            Lists = new List<BoundList>();
        }

        public CollapsibleNodeList GetList(int CAID)
        {
            foreach (BoundList list in Lists)
            {
                if (list.CAID == CAID)
                {
                    return list;
                }
            }
            return null;
        }

        public CollapsibleNodeItem GetList(object sender) // double click
        {

        }

        public 

    }

    public interface IMetaObjectList
    {
        string ListClass;
        string HeaderText;
        string Property;
        string ExtendedProperty;
    }
    public class SimpleList: IMetaObjectList
    {
        public string ListClass;
        public string HeaderText;
        public string Property;
        public string ExtendedProperty;

        public List<CollapsibleNodeItem> Items;
    }

    public class ExpandableList: SimpleList
    {

    }*/


    
    public class MyAutoLinkNode : GoBoxNode
    {
        public MyAutoLinkNode()
        {
            
            MetaBuilder.Graphing.Shapes.Primitives.GradientRoundedRectangle rect = new MetaBuilder.Graphing.Shapes.Primitives.GradientRoundedRectangle();
            rect.Bounds = new System.Drawing.RectangleF(new System.Drawing.PointF(0, 0), new System.Drawing.SizeF(100, 60));


            GoGrid grid = new GoGrid();
            grid.Position = new System.Drawing.PointF(0, 0);
            grid.Size = new System.Drawing.SizeF(100, 60);

            grid.Visible = true;
            this.Body = grid;

            this.PortBorderMargin = new System.Drawing.SizeF(5, 5);

        }

        public GoPort Port;
    }
   
}
