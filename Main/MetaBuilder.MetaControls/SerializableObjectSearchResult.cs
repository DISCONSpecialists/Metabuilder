using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MetaBuilder.BusinessLogic;

namespace MetaBuilder.MetaControls
{
    [Serializable]
    public class SerializableObjectSearchResult
    {

		#region Fields (1) 

        private List<ObjectIDentifier> objectIdentifiers;

		#endregion Fields 

		#region Properties (1) 

        public List<ObjectIDentifier> ObjectIdentifiers
        {
            get { return objectIdentifiers; }
            set { objectIdentifiers = value; }
        }

		#endregion Properties 

		#region Methods (2) 


		// Public Methods (2) 

        public void Open(string file)
        {
            this.ObjectIdentifiers.Clear();
            XmlTextReader reader = new XmlTextReader(file);

            while (reader.Read())
            {
                switch(reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "ObjectIDentifier")
                        {
                            if (reader.HasAttributes)
                            {
                                ObjectIDentifier ObjectIDentifier = new ObjectIDentifier();
                                ObjectIDentifier.Key = new MetaObjectKey();
                                ObjectIDentifier.Key.pkid = int.Parse(reader.GetAttribute("pkid"));
                                ObjectIDentifier.Key.Machine = reader.GetAttribute("machine");
                                ObjectIDentifier.ObjectClass = reader.GetAttribute("class");
                                ObjectIDentifier.StringValue = reader.GetAttribute("stringvalue");
                                ObjectIdentifiers.Add(ObjectIDentifier);
                            }
                        }
                        break;
                    case XmlNodeType.DocumentType:
                        break;
                    case XmlNodeType.Attribute:
                        
                        break;
                }
            }
            
        }

        public void Save(string file)
        {
            FileStream fstream = File.Open(file,FileMode.Create,FileAccess.Write);
            XmlTextWriter myXmlTextWriter = new XmlTextWriter(fstream, null);
            myXmlTextWriter.Formatting = Formatting.Indented;
            myXmlTextWriter.WriteStartDocument(false);
            myXmlTextWriter.WriteDocType("ObjectSelection", null, null, null);
            myXmlTextWriter.WriteComment("This file stores a saved selection of objects for MetaBuilder");
            myXmlTextWriter.WriteStartElement("ObjectIdentifiers");
            foreach (ObjectIDentifier identifier in this.ObjectIdentifiers)
            {
                myXmlTextWriter.WriteStartElement("ObjectIDentifier", null);
                myXmlTextWriter.WriteAttributeString("pkid", identifier.Key.pkid.ToString());
                myXmlTextWriter.WriteAttributeString("machine",identifier.Key.Machine);
                myXmlTextWriter.WriteAttributeString("class", identifier.ObjectClass);
                myXmlTextWriter.WriteAttributeString("stringvalue", identifier.StringValue);
            }
            myXmlTextWriter.WriteEndElement();

            //'Write the XML to file and close the myXmlTextWriter
            myXmlTextWriter.Flush();
            myXmlTextWriter.Close();
            fstream.Close();
        }


		#endregion Methods 

		#region Nested Classes (1) 


        [Serializable]
        public class ObjectIDentifier
        {

		#region Fields (3) 

            private MetaObjectKey key;
            private string objectClass;
	            private string stringValue;

		#endregion Fields 

		#region Properties (3) 

            public MetaObjectKey Key
            {
                get { return key; }
                set { key = value; }
            }

            public string ObjectClass
            {
                get { return objectClass; }
                set { objectClass = value; }
            }

            public string StringValue
            {
                get { return stringValue; }
                set { stringValue = value; }
            }

		#endregion Properties 

	
        }
		#endregion Nested Classes 

    }
}
