using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Groups
{

    public class AllocationHandleTransformer : BaseGoObjectTransformer
    {
        #region Constructors (1)

        public AllocationHandleTransformer()
            : base()
        {
            this.TransformerType = typeof(AllocationHandle);
            this.ElementName = "allocationHandle";
        }

        #endregion Constructors

        #region Methods (3)

        // Public Methods (3) 

        public override object Allocate()
        {
            AllocationHandle retval = new AllocationHandle();
            return retval;
        }

        public override void ConsumeAttributes(object obj)
        {
            AllocationHandle handle = obj as AllocationHandle;

            string items = StringAttr("allocations", "");
            if (!string.IsNullOrEmpty(items))
            {
                foreach (string wholeItem in items.Split('|'))
                {
                    if (wholeItem == "")
                        continue;
                    string path = "", unique = "", type = "";
                    int x = 0;
                    foreach (string s in wholeItem.Split(';'))
                    {
                        if (x == 0)
                            unique = s;
                        else if (x == 1)
                            path = s;
                        else if (x == 2)
                            type = s;
                        x++;
                    }
                    AllocationHandle.AllocationItem item = new AllocationHandle.AllocationItem(path, unique);
                    handle.Items.Add(item);
                }
            }
            base.ConsumeAttributes(obj);
        }
        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);
            (obj as AllocationHandle).SetStyle();
        }

        public override void GenerateAttributes(Object obj)
        {
            base.GenerateAttributes(obj, true);
            AllocationHandle handle = obj as AllocationHandle;
            string items = "";
            foreach (AllocationHandle.AllocationItem item in handle.Items)
            {
                items += item.WriteString + "|";
            }
            WriteAttrVal("allocations", items);
        }

        #endregion Methods
    }
}