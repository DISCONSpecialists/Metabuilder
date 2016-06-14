using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using MetaBuilder.Meta;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Docking;
using TraceTool;
using MetaBuilder.Graphing.Containers;
namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel
{
    public class Tester
    {
        public Tester()
        {
            /*
            Entity eNationality = new Entity("Nationality");
            eNationality.KeyAttributes.Add(new Attr("Nation"));

            Entity eCustomer = new Entity("Customer");
            eCustomer.KeyAttributes.Add(new Attr("CustomerID"));
            eCustomer.Attributes.Add(new Attr("Name"));

            Entity eSalesOrder = new Entity("SalesOrder");
            eSalesOrder.KeyAttributes.Add(new Attr("SalesOrderID"));
            eSalesOrder.Attributes.Add(new Attr("CustomerID"));
            eSalesOrder.Attributes.Add(new Attr("Date"));

            Entity eOrderItem = new Entity("OrderItem");
            eOrderItem.KeyAttributes.Add(new Attr("SalesOrderID"));
            eOrderItem.KeyAttributes.Add(new Attr("ItemID"));
            eOrderItem.Attributes.Add(new Attr("Price"));

            Entity eItem = new Entity("Item");
            eItem.KeyAttributes.Add(new Attr("ItemID"));
            eItem.Attributes.Add(new Attr("Description"));

            Entity eItemPage = new Entity("ItemPage");
            eItemPage.KeyAttributes.Add(new Attr("ItemID"));
            eItemPage.KeyAttributes.Add(new Attr("PageID"));

            Entity ePage = new Entity("Page");
            ePage.KeyAttributes.Add(new Attr("PageID"));

            Relation r0 = new Relation(eSalesOrder, eCustomer, RelationshipTypeEnum.Dependency);
            Relation r4 = new Relation(eItemPage, eItem, RelationshipTypeEnum.Dependency);
            Relation r1 = new Relation(eOrderItem, eSalesOrder, RelationshipTypeEnum.Dependency);
            Relation r2 = new Relation(eOrderItem, eItem, RelationshipTypeEnum.Dependency);
            Relation r3 = new Relation(eCustomer, eNationality, RelationshipTypeEnum.Dependency);
            Relation r5 = new Relation(eItemPage, ePage, RelationshipTypeEnum.Dependency);

            List<Entity> entities = new List<Entity>();

            entities.Add(eItemPage);
            entities.Add(eItem);
            entities.Add(eOrderItem);
            entities.Add(eSalesOrder);
            entities.Add(eCustomer);
            entities.Add(eNationality);

            entities.Add(ePage);

            List<Relation> relationships = new List<Relation>();
            relationships.Add(r0);
            relationships.Add(r4);
            relationships.Add(r5);
            relationships.Add(r1);
            relationships.Add(r2);
            relationships.Add(r3);

            m = new Model(entities, relationships);
             * */

        }

        //Model m;
        public void Test()
        {
            //m.Cycle();
        }

        public void ValidateModel(MetaBuilder.Graphing.Containers.NormalDiagram diagram, MetaBuilder.Graphing.Containers.GraphView view)
        {

            DataModel.Engine engine = new DataModel.Engine(view, diagram);
//#if DEBUG
//            DataModel.Engine2015 e = new DataModel.Engine2015(diagram, DockingForm.DockForm.ValidationResultForm.GetOptions());
//            //DockingForm.DockForm.ValidationResultForm.MyEngine = e;
//            DockingForm.DockForm.ValidationResultForm.MyEngine = engine;
//            DockingForm.DockForm.ValidationResultForm.ShowResults();
//            return;
//#endif
            DockingForm.DockForm.ValidationResultForm.MyEngine = engine;
            DockingForm.DockForm.ValidationResultForm.ShowResults();
        }
        //public void ValidateModel(MetaBuilder.Graphing.Containers.NormalDiagram diagram, MetaBuilder.Graphing.Containers.GraphView view, Tools.DataModel.UI.ValidationResultForm vform)
        //{
        //    DataModel.Engine engine = new DataModel.Engine(view, diagram);
        //    Tools.DataModel.UI.ValidationResultForm vform = new MetaBuilder.UIControls.GraphingUI.Tools.DataModel.UI.ValidationResultForm();
        //    vform.MyEngine = engine;
        //    vform.ShowResults();
        //    vform.Show(DockingForm.DockForm.dockPanel1, DockState.DockBottom);
        //}
    }
    public class EntitySorter : IComparer<Entity>
    {
        #region IComparer<Item> Members

        public int Compare(Entity x, Entity y)
        {

            //return y.Freq - x.Freq; //descending sort
            if (x.CycleID < y.CycleID)
            {
                return -1;
            }
            if (x.CycleIndex < y.CycleIndex && x.CycleID == y.CycleID)
            {
                return -1;
            }
            if (x.CycleID == y.CycleID && x.CycleIndex == y.CycleIndex)
            {
                return -0;
            }
            return 1;
        }

        #endregion
    }
    public class RelationSorter : IComparer<Relation>
    {
        public int Compare(Relation r1, Relation r2)
        {
            int r1CycleIndices = r1.To.CycleIndex + r1.From.CycleIndex;
            int r1CycleIDs = r1.To.CycleID + r1.From.CycleID;

            int r2CycleIndices = r2.To.CycleIndex + r2.From.CycleIndex;
            int r2CycleIDs = r2.To.CycleID + r2.From.CycleID;

            //return y.Freq - x.Freq; //descending sort
            if (r1CycleIDs < r2CycleIDs)
            {
                return -1;
            }
            if (r1CycleIndices < r2CycleIndices && r1CycleIDs == r2CycleIDs)
            {
                return -1;
            }
            if (r1CycleIDs == r2CycleIDs && r1CycleIndices == r2CycleIndices)
            {
                return -0;
            }
            return 1;
        }
    }
}