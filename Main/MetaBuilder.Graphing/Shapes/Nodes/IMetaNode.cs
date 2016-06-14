using System;
using MetaBuilder.Meta;
using b =MetaBuilder.BusinessLogic;

namespace MetaBuilder.Graphing.Shapes
{
    public interface IMetaNode
    {
        MetaBase CopiedFrom { get; set; }
        MetaBase MetaObject { get; set; }
        BindingInfo BindingInfo { get; set; }
        EventHandler ContentsChanged { get; set; }
        bool HasBindingInfo { get; }
        bool RequiresAttention { get; set; }
        void CreateMetaObject(object sender, EventArgs e);
        void BindToMetaObjectProperties();
        void LoadMetaObject(int ID, string Machine);
        void FireMetaObjectChanged(object sender, EventArgs e);
        void HookupEvents();
        void OnContentsChanged(object sender, EventArgs e);
        b.TList<b.ObjectAssociation> SaveToDatabase(object sender, EventArgs e); // retval added to facilitate attribute association saving to graphfileassociation (dirty fix)

        bool ParentIsILinkedContainer { get;set;} //This is for objects within ILinkedcontainers, when pressing F8 in order to shallow copy parent(s)
        void BindMetaObjectImage();
    }
}