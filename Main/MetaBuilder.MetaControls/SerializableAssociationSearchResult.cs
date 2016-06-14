using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MetaBuilder.BusinessLogic;

namespace MetaBuilder.MetaControls
{
    [Serializable]
    public class SerializableAssociationSearchResult
    {

		#region Fields (1) 

        private List<AssociationIdentifier> associationIdentifiers;

		#endregion Fields 

		#region Properties (1) 

        public List<AssociationIdentifier> AssociationIdentifiers
        {
            get { return associationIdentifiers; }
            set { associationIdentifiers = value; }
        }

		#endregion Properties 

		#region Methods (2) 


		// Public Methods (2) 

        public void Open(string file)
        {
            this.AssociationIdentifiers.Clear();
            XmlTextReader reader = new XmlTextReader(file);

            while (reader.Read())
            {
                switch(reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "AssociationIdentifier")
                        {
                            if (reader.HasAttributes)
                            {
                                AssociationIdentifier associationIdentifier = new AssociationIdentifier();
                                associationIdentifier.Key = new ObjectAssociationKey();
                                associationIdentifier.Key.CAid = int.Parse(reader.GetAttribute("CAid"));
                                associationIdentifier.Key.ObjectID = int.Parse(reader.GetAttribute("ObjectID"));
                                associationIdentifier.Key.ObjectMachine = reader.GetAttribute("ObjectMachine");
                                associationIdentifier.Key.ChildObjectID = int.Parse(reader.GetAttribute("ChildObjectID"));
                                associationIdentifier.Key.ChildObjectMachine = reader.GetAttribute("ChildObjectMachine");
                                associationIdentifier.FromObjectString = reader.GetAttribute("FromObjectString");
                                associationIdentifier.ToObjectString = reader.GetAttribute("ToObjectString");
                                associationIdentifier.FromObjectClass = reader.GetAttribute("FromObjectClass");
                                associationIdentifier.ToObjectClass = reader.GetAttribute("ToObjectClass");
                                AssociationIdentifiers.Add(associationIdentifier);
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
            myXmlTextWriter.WriteDocType("AssociationSelection", null, null, null);
            myXmlTextWriter.WriteComment("This file stores a saved selection of associations for MetaBuilder");
            myXmlTextWriter.WriteStartElement("AssociationIdentifiers");
            foreach (AssociationIdentifier identifier in this.AssociationIdentifiers)
            {
                myXmlTextWriter.WriteStartElement("AssociationIdentifier", null);
                myXmlTextWriter.WriteAttributeString("CAid", identifier.Key.CAid.ToString());
                myXmlTextWriter.WriteAttributeString("ObjectID", identifier.Key.ObjectID.ToString());
                myXmlTextWriter.WriteAttributeString("ChildObjectID", identifier.Key.ChildObjectID.ToString());
                myXmlTextWriter.WriteAttributeString("ObjectMachine", identifier.Key.ObjectMachine);
                myXmlTextWriter.WriteAttributeString("ChildObjectMachine", identifier.Key.ChildObjectMachine);
                myXmlTextWriter.WriteAttributeString("FromObjectString", identifier.FromObjectString);
                myXmlTextWriter.WriteAttributeString("ToObjectString", identifier.ToObjectString);
                myXmlTextWriter.WriteAttributeString("FromObjectClass", identifier.FromObjectClass);
                myXmlTextWriter.WriteAttributeString("ToObjectClass", identifier.ToObjectClass);
                myXmlTextWriter.WriteEndElement();

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
        public class AssociationIdentifier
        {

		#region Fields (6) 

                       private string associationDescription;
            private string fromObjectClass;
            private string fromObjectString;
            private ObjectAssociationKey key;
            private string toObjectClass;
            private string toObjectString;

		#endregion Fields 

		#region Properties (6) 

            public string AssociationDescription
            {
                get { return associationDescription; }
                set { associationDescription = value; }
            }

            public string FromObjectClass
            {
                get { return fromObjectClass; }
                set { fromObjectClass = value; }
            }

            public string FromObjectString
            {
                get { return fromObjectString; }
                set { fromObjectString = value; }
            }

            public ObjectAssociationKey Key
            {
                get { return key; }
                set { key = value; }
            }

            public string ToObjectClass
            {
                get { return toObjectClass; }
                set { toObjectClass = value; }
            }

            public string ToObjectString
            {
                get { return toObjectString; }
                set { toObjectString = value; }
            }

		#endregion Properties 

	
	
        }
		#endregion Nested Classes 

    }
}
