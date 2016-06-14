using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections.ObjectModel;

namespace MetaBuilder.Core
{
    public static class Xmler
    {
        public static Dictionary<string, string> GetRemappedClasses()
        {
            //Old Class, New Class
            Dictionary<string, string> r = new Dictionary<string, string>();

            XmlDocument doc = new XmlDocument();
            if (!(System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "RemappedClasses.xml")))
                return r;
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "RemappedClasses.xml");
            foreach (XmlNode node in doc.ChildNodes[0].ChildNodes)
                if (node.Name == "Class") //Combined Node Check
                    r.Add(node.Attributes["Old"].Value.ToString(), node.Attributes["New"].Value.ToString());

            return r;
        }
        public static void SetRemappedClass(string oldClass, string newClass)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "RemappedClasses.xml");

            //XmlNode node = doc.CreateNode(XmlNodeType.Element, "Class", null);
            ////node.Name = "Class";

            //XmlAttribute attrO = node.app new XmlAttribute();
            //attrO.Name = "Old";
            //attrO.Value = oldClass;
            //node.Attributes.Append(attrO);
            //XmlAttribute attrN = new XmlAttribute();
            //attrN.Name = "New";
            //attrN.Value = newClass;
            //node.Attributes.Append(attrN);

            //doc.ChildNodes[0].AppendChild(node);

            //doc.Save();
        }
        public static Collection<string> GetRemappedFields()
        {
            //when the OLDclass is function get the OLDfield value and set it for the NEWfield value
            //"OLDclass:OLDfield:NEWfield" });
            Collection<string> r = new Collection<string>(new string[] { });

            XmlDocument doc = new XmlDocument();
            if (!(System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "RemappedFields.xml")))
                return r;
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "RemappedFields.xml");
            foreach (XmlNode node in doc.ChildNodes[0].ChildNodes)
            {
                if (node.Name == "Field") //Combined Node Check
                {
                    try
                    {
                        r.Add(node.Attributes["Class"].Value.ToString() + ":" + node.Attributes["Old"].Value.ToString() + ":" + node.Attributes["New"].Value.ToString() + ":" + node.Attributes["NewClass"].Value.ToString());
                    }
                    catch
                    {
                        //ignore this field remapping because it is missing an attribute
                    }
                }
            }

            return r;
        }
        public static void SetRemappedField(string Class, string oldField, string newField, string newClass)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "RemappedFields.xml");

            //XmlNode node = new XmlNode();
            //node.Name = "Field";

            //XmlAttribute attrC = new XmlAttribute();
            //attrC.Name = "Class";
            //attrC.Value = Class;
            //node.Attributes.Append(attrC);
            //XmlAttribute attrO = new XmlAttribute();
            //attrO.Name = "Old";
            //attrO.Value = oldField;
            //node.Attributes.Append(attrO);
            //XmlAttribute attrN = new XmlAttribute();
            //attrN.Name = "New";
            //attrN.Value = newField;
            //node.Attributes.Append(attrN);
            //XmlAttribute attrNC = new XmlAttribute();
            //attrNC.Name = "NewClass";
            //attrNC.Value = newClass;
            //node.Attributes.Append(attrNC);

            //doc.ChildNodes[0].AppendChild(node);

            //doc.Save();
        }

        public static Collection<string> GetRemappedClassToFields()
        {
            //when the OLDclass is function get the OLDfield value and set it for the NEWfield value
            //"objectType:OLDclass:OLDfield:NEWclass:NEWfield" });
            Collection<string> r = new Collection<string>(new string[] { });

            XmlDocument doc = new XmlDocument();
            if (!(System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "RemappedClasses.xml")))
                return r;
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "RemappedClasses.xml");
            foreach (XmlNode node in doc.ChildNodes[0].ChildNodes)
            {
                if (node.Name == "ClassToField") //Combined Node Check
                {
                    try
                    {
                        r.Add(node.Attributes["ObjectType"].Value.ToString() + ":" + node.Attributes["OldClass"].Value.ToString() + ":" + node.Attributes["OldField"].Value.ToString() + ":" + node.Attributes["NewClass"].Value.ToString() + ":" + node.Attributes["NewField"].Value.ToString());
                    }
                    catch
                    {
                        //ignore this remapping because it is missing an attribute
                    }
                }
            }

            return r;
        }

        public static Collection<string> GetRemappedAssociationTypes()
        {

            return null;
        }

    }
}
