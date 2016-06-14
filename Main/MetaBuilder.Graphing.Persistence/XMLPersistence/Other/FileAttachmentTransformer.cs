using System;
using System.Text;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes;
using MetaBuilder.Graphing.Shapes.General;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Other
{
    public class FileAttachmentTransformer : BaseGoObjectTransformer 
    {

		#region Constructors (1) 

        public FileAttachmentTransformer():base()
        {
            TransformerType = typeof (SerializableAttachment);
            ElementName = "FileAttachment";
            IdAttributeUsedForSharedObjects = true;
            BodyConsumesChildElements = false;
        }

		#endregion Constructors 

		#region Methods (6) 


		// Public Methods (6) 

        public override object Allocate()
        {
            return new SerializableAttachment();
        }

        public override void ConsumeAttributes(object obj)
        {
            

            SerializableAttachment serAttachment = obj as SerializableAttachment;
            serAttachment.Label.Text = StringAttr("LabelText", "");

            string fileString = StringAttr("FileBytes", "");
            serAttachment.OriginalFileName = StringAttr("OriginalFileName", "");
            

            Byte[] fileData = new Byte[fileString.Length];
            fileData = Convert.FromBase64String(FixBase64ForImage(fileString));
            serAttachment.Bytes = fileData;
           // serAttachment.CRC = (StringAttr("CRCString", serAttachment.CRC));
            base.ConsumeAttributes(obj);
            
        }

        public override void ConsumeChild(object parent, object child)
        {
            if (child is SerializableAttachment )
                base.ConsumeChild(parent, child);
        }

        public string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty);
            sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }

        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj,true);

            SerializableAttachment serAttachment = obj as SerializableAttachment;
            string fileString = Convert.ToBase64String(serAttachment.Bytes);
            WriteAttrVal("FileBytes", fileString);
            WriteAttrVal("OriginalFileName", serAttachment.OriginalFileName);
            WriteAttrVal("LabelText", serAttachment.Label.Text);

            WriteAttrVal("CRCString", serAttachment.CRCString);
            
        }

        public override void GenerateBody(object obj)
        {
            //base.GenerateBody(obj);
        }


		#endregion Methods 

    }
}
