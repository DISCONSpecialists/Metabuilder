using System;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.General;
using Northwoods.Go;
using Northwoods.Go.Xml;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Links
{


    public class NonPrintQuickPortTransformer : BaseGoObjectTransformer
    {

		#region Constructors (1) 

        public NonPrintQuickPortTransformer()
            : base()
        {
            this.TransformerType = typeof(NonPrintingQuickPort);
            this.ElementName = "nonprintqprt";
            IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            NonPrintingQuickPort qp = new NonPrintingQuickPort();
            return qp;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            NonPrintingQuickPort port = obj as NonPrintingQuickPort;
            port.Name = StringAttr("prtName", "");
            port.IncomingLinksDirection = float.Parse(StringAttr("I", "0"), System.Globalization.CultureInfo.InvariantCulture);
            port.OutgoingLinksDirection = float.Parse(StringAttr("O", "0"), System.Globalization.CultureInfo.InvariantCulture);
            port.IsValidSelfNode = BooleanAttr("Self", false);
            string style = StringAttr("St", "");
            if (style != string.Empty)
            {
                Type t = typeof(GoPortStyle);
                port.Style = (GoPortStyle)Enum.Parse(t, style);
            }

            if (IsAttrPresent("id"))
                this.Reader.MakeShared(StringAttr("id", null), obj);
            port.Printable = false;

        }

        public override void GenerateAttributes(Object obj)
        {
            this.Writer.MakeShared(obj);
            //base.GenerateAttributes(obj);
            GenerateAttributes(obj, true);
            
            NonPrintingQuickPort qp = obj as NonPrintingQuickPort;
            WriteAttrVal("prtName", (qp.Parent as GoGroup).FindName(qp));
           

        }


		#endregion Methods 

    }


}
