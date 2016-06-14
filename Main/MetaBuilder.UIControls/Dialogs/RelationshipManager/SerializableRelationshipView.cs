using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MetaBuilder.BusinessLogic;

namespace MetaBuilder.UIControls.Dialogs.RelationshipManager
{
    [Serializable]
    public class SerializableRelationshipView
    {

		#region Fields (2) 

        private List<ObjectIDentifier> columnObjects;
        private List<ObjectIDentifier> rowObjects;

		#endregion Fields 

		#region Properties (2) 

        public List<ObjectIDentifier> ColumnObjects
        {
            get { return columnObjects; }
            set { columnObjects = value; }
        }

        public List<ObjectIDentifier> RowObjects
        {
            get { return rowObjects; }
            set { rowObjects = value; }
        }

		#endregion Properties 

		#region Methods (3) 


		// Public Methods (2) 

        public void Open(string file)
        {
            ColumnObjects = new List<ObjectIDentifier>();
            RowObjects = new List<ObjectIDentifier>();
            XmlTextReader reader = new XmlTextReader(file);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "ColumnObject")
                        {
                            if (reader.HasAttributes)
                            {
                                ObjectIDentifier ObjectIDentifier = GetObjectIDentifier(reader);
                                ColumnObjects.Add(ObjectIDentifier);
                            }
                        }

                        if (reader.Name == "RowObject")
                        {
                            if (reader.HasAttributes)
                            {
                                ObjectIDentifier ObjectIDentifier = GetObjectIDentifier(reader);
                                RowObjects.Add(ObjectIDentifier);
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
            FileStream fstream = File.Open(file, FileMode.Create, FileAccess.Write);
            XmlTextWriter myXmlTextWriter = new XmlTextWriter(fstream, null);
            myXmlTextWriter.Formatting = Formatting.Indented;
            myXmlTextWriter.WriteStartDocument(false);
            myXmlTextWriter.WriteDocType("RelationshipManagerSelection", null, null, null);
            myXmlTextWriter.WriteComment("This file stores a saved selection of row and column objects for MetaBuilder");
            myXmlTextWriter.WriteStartElement("ColumnObjects");
            foreach (ObjectIDentifier identifier in this.ColumnObjects)
            {
                myXmlTextWriter.WriteStartElement("ColumnObject", null);
                myXmlTextWriter.WriteAttributeString("pkid", identifier.Key.pkid.ToString());
                myXmlTextWriter.WriteAttributeString("machine", identifier.Key.Machine);
                myXmlTextWriter.WriteAttributeString("class", identifier.ObjectClass);
                myXmlTextWriter.WriteAttributeString("stringvalue", identifier.StringValue);
            }
            myXmlTextWriter.WriteEndElement();

            myXmlTextWriter.WriteStartElement("RowObjects");
            foreach (ObjectIDentifier identifier in this.RowObjects)
            {
                myXmlTextWriter.WriteStartElement("RowObject", null);
                myXmlTextWriter.WriteAttributeString("pkid", identifier.Key.pkid.ToString());
                myXmlTextWriter.WriteAttributeString("machine", identifier.Key.Machine);
                myXmlTextWriter.WriteAttributeString("class", identifier.ObjectClass);
                myXmlTextWriter.WriteAttributeString("stringvalue", identifier.StringValue);
            }
            myXmlTextWriter.WriteEndElement();

            //'Write the XML to file and close the myXmlTextWriter
            myXmlTextWriter.Flush();
            myXmlTextWriter.Close();
            fstream.Close();
        }



		// Private Methods (1) 

        private static ObjectIDentifier GetObjectIDentifier(XmlTextReader reader)
        {
            ObjectIDentifier ObjectIDentifier = new ObjectIDentifier();
            ObjectIDentifier.Key = new MetaObjectKey();
            ObjectIDentifier.Key.pkid = int.Parse(reader.GetAttribute("pkid"));
            ObjectIDentifier.Key.Machine = reader.GetAttribute("machine");
            ObjectIDentifier.ObjectClass = reader.GetAttribute("class");
            ObjectIDentifier.StringValue = reader.GetAttribute("stringvalue");
            return ObjectIDentifier;
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
