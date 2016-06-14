using System;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Groups;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Other
{
    public class FrameLayerTransformer : GoGroupTransformer
    {

        #region Fields (2)

        private FrameLayerGroup frame;
        DocumentInfo info;

        #endregion Fields

        #region Constructors (1)

        public FrameLayerTransformer()
            : base()
        {
            this.TransformerType = typeof(FrameLayerGroup);
            this.ElementName = "frame";

            BodyConsumesChildElements = true;
        }

        #endregion Constructors

        #region Properties (1)

        public FrameLayerGroup Frame
        {
            get { return frame; }
            set { frame = value; }
        }

        #endregion Properties

        #region Methods (5)


        // Public Methods (5) 

        public override object Allocate()
        {
            FrameLayerGroup retval = new FrameLayerGroup();
            return retval;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            FrameLayerGroup flg = obj as FrameLayerGroup;
            info = new DocumentInfo();
            info.Author = StringAttr("Authors", "");
            info.AuthorID = StringAttr("ID", "");
            info.OrganisationUnit = StringAttr("Company", "");
            info.Description = StringAttr("Description", "");
            info.Date = Core.GlobalParser.ParseGlobalisedDateString(StringAttr("DocDate", DateTime.Now.ToShortDateString()));
            info.Version = StringAttr("Version", "");
        }

        public override void ConsumeChild(object parent, object child)
        {
            base.ConsumeChild(parent, child);
        }

        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);

            FrameLayerGroup flg = obj as FrameLayerGroup;
            flg.Initializing = true;
            flg.InitTextBoxes(flg.Bounds);
            flg.Initializing = false;
            flg.Update(info);
            Frame = flg;
        }

        public override void GenerateAttributes(Object obj)
        {
            base.GenerateAttributes(obj);
            FrameLayerGroup flg = obj as FrameLayerGroup;
            DocumentInfo docInfo = flg.GetDocumentInfo();
            WriteAttrVal("Authors", docInfo.Author);
            WriteAttrVal("ID", docInfo.AuthorID);
            WriteAttrVal("Company", docInfo.OrganisationUnit);
            WriteAttrVal("Description", docInfo.Description);
            WriteAttrVal("DocDate", docInfo.Date.ToString());
            WriteAttrVal("Version", docInfo.Version);
        }


        #endregion Methods

    }
}
