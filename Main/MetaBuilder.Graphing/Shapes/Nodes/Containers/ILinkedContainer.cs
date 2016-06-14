using System.Collections.Generic;
using System.Drawing;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Meta;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;

namespace MetaBuilder.Graphing.Shapes.Nodes.Containers
{
    public interface ILinkedContainer
    {
        //List<IMetaNode> LinkedObjects{get;set;}
        List<EmbeddedRelationship> ObjectRelationships { get; set; }
        RectangleF Bounds { get; set; }
        MetaBase MetaObject { get; set; }
        Dictionary<string, ClassAssociation> DefaultClassBindings { get; set; }
        bool Locked { get; }
        string LabelText { get; }
        void RemoveByRelationship(EmbeddedRelationship rel);
        void Remove(GoObject o);
        bool ObjectInAcceptedRegion(GoObject o);
        void PerformAddCollection(GoView view, GoCollection objectCollection);
        bool Contains(GoObject o);

        bool ParentIsILinkedContainer { get;set;}
    }
}