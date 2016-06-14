using System;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Groups
{
    public class CollapsibleHandleTransformer : BaseGoObjectTransformer
    {

        #region Constructors (1)

        public CollapsibleHandleTransformer()
            : base()
        {
            this.TransformerType = typeof(GoCollapsibleHandle);
            this.ElementName = "collapsibleHandle";
        }

        #endregion Constructors

        #region Methods (3)


        // Public Methods (3) 

        public override object Allocate()
        {
            GoCollapsibleHandle retval = new GoCollapsibleHandle();
            return retval;
        }

        public override void ConsumeAttributes(object obj)
        {
            GoCollapsibleHandle handle = obj as GoCollapsibleHandle;
            string handleStyle = StringAttr("HandleStyle", GoCollapsibleHandleStyle.PlusMinus.ToString());
            handle.Style = (GoCollapsibleHandleStyle)Enum.Parse(typeof(GoCollapsibleHandleStyle), handleStyle);
            handle.Printable = false;
            base.ConsumeAttributes(obj);
        }

        public override void GenerateAttributes(Object obj)
        {
            if (!(obj is ShapeCollapsibleHandle) && !(obj is AllocationHandle))
            {
                base.GenerateAttributes(obj, true);
                GoCollapsibleHandle handle = obj as GoCollapsibleHandle;
                WriteAttrVal("HandleStyle", handle.Style.ToString());
            }
        }

        #endregion Methods

    }
}
