using System;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.General;
using Northwoods.Go;
using Northwoods.Go.Xml;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Links
{

    public class QuickPortTransformer : BaseGoObjectTransformer
    {

        #region Constructors (1)

        public QuickPortTransformer()
            : base()
        {
            this.TransformerType = typeof(QuickPort);
            this.ElementName = "qprt";
            IdAttributeUsedForSharedObjects = true;
        }

        #endregion Constructors

        #region Methods (3)

        // Public Methods (3) 

        public override object Allocate()
        {
            QuickPort qp = new QuickPort(false);

            return qp;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            QuickPort port = obj as QuickPort;
            port.IncomingLinksDirection = float.Parse(StringAttr("I", "0"), System.Globalization.CultureInfo.InvariantCulture);
            port.OutgoingLinksDirection = float.Parse(StringAttr("O", "0"), System.Globalization.CultureInfo.InvariantCulture);

            try
            {
                port.PortPosition = (QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(QuickPortHelper.QuickPortLocation), StringAttr("P_L", "Circumferential"), true);
            }
            catch
            {
                port.PortPosition = QuickPortHelper.QuickPortLocation.Circumferential;
            }

            port.IsValidSelfNode = BooleanAttr("Self", false);
            string style = StringAttr("St", "");
            if (style != string.Empty)
            {
                Type t = typeof(GoPortStyle);
                port.Style = (GoPortStyle)Enum.Parse(t, style);
            }

            if (IsAttrPresent("id"))
            {
                port.PartID = Int32Attr("id", -1);
                this.Reader.MakeShared(StringAttr("id", null), obj);
            }
        }

        public override void GenerateAttributes(Object obj)
        {
            this.Writer.MakeShared(obj);
            if (obj.GetType().FullName == typeof(QuickPort).FullName)
                GenerateAttributes(obj, true);

            QuickPort qp = obj as QuickPort;
            if (qp.IsValidSelfNode)
                WriteAttrVal("S", qp.IsValidSelfNode);

            if (qp.Style != GoPortStyle.Rectangle)
                WriteAttrVal("St", qp.Style.ToString());

            if (qp.PortObject != null)
            {
                string portObjectID = Writer.MakeShared(qp.PortObject);
                WriteAttrRef("PortObject", qp.PortObject);
            }

            if (qp.DestinationLinksCount > 0 || qp.SourceLinksCount > 0)
            {
                Writer.MakeShared(obj);
            }
            WriteAttrVal("Self", qp.IsValidSelfNode);
            WriteAttrVal("I", qp.IncomingLinksDirection);
            WriteAttrVal("O", qp.OutgoingLinksDirection);

            WriteAttrVal("P_L", qp.PortPosition.ToString());
        }

        #endregion Methods

    }
}