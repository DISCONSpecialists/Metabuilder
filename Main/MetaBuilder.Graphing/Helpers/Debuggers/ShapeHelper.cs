using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using MetaBuilder.Core;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Helpers.Debuggers
{
    public class ShapeHelper
    {
        #region Methods (6) 

        // Public Methods (6) 

        public static void DebugObjectToXml(IGoCollection collection, string targetFile)
        {
            string filename = targetFile;
            XmlWriter writer = XmlWriter.Create(filename);


            writer.WriteStartDocument();
            writer.WriteDocType("ObjectDebug", null, null, null);
            writer.WriteStartElement("ObjectDebugXML");

            foreach (GoObject o in collection)
            {
                DebugToWriter(writer, o);
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        public static void DebugShape(GoObject o)
        {
#if DEBUG
            Debug.WriteLine("");
            Debug.WriteLine("Debuggin GoObject (" + o + ")");
            Debug.Indent();
            Debug.WriteLine("DraggingObject:" + o.DraggingObject);
            Debug.WriteLine("DragsNode:" + o.DragsNode);
            Debug.WriteLine("Resizable:" + o.Resizable);
            Debug.WriteLine("Reshapeable:" + o.Reshapable);
            if (o is GoGroup)
            {
                Debug.WriteLine("GoGroup... decomposing:");
                GoGroup grp = o as GoGroup;
                Debug.Indent();
                foreach (GoObject oChild in grp)
                {
                    DebugShape(oChild);
                }
                Debug.Unindent();
            }

            if (o is GraphNode)
            {
                Debug.WriteLine("GraphNode... decomposing:");
                GraphNode grp = o as GraphNode;
                Debug.Indent();
                foreach (GoObject oChild in grp)
                {
                    DebugShape(oChild);
                }
                Debug.Unindent();
            }
            Debug.WriteLine("");
            Debug.Unindent();
#endif
        }

        public static void DebugToWriter(XmlWriter writer, GoObject o)
        {
            writer.WriteStartElement(o.ToString());
            writer.WriteAttributeString("ObjectType", o.GetType().ToString());
            writer.WriteStartElement("Behaviour");
            writer.WriteElementString("Resizable", o.Resizable.ToString());
            writer.WriteElementString("Reshapable", o.Reshapable.ToString());
            writer.WriteElementString("DragsNode", o.DragsNode.ToString());
            writer.WriteElementString("Selectable", o.Selectable.ToString());
            writer.WriteElementString("Movable", o.Movable.ToString());
            writer.WriteElementString("Editable", o.Editable.ToString());
            writer.WriteElementString("Visible", o.Visible.ToString());
            writer.WriteEndElement();

            if (o is GoGroup)
            {
                GoGroup grp = o as GoGroup;
                GoGroupEnumerator colEnum = grp.GetEnumerator();
                //if (colEnum.Count > 0)
                {
                    writer.WriteStartElement("Children");
                    while (colEnum.MoveNext())
                    {
                        DebugToWriter(writer, colEnum.Current);
                    }
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }

        public static string DebugToXML(GoObject o, string Tag)
        {
            if (o != null)
            {
                string minutestring = (DateTime.Now.Minute.ToString().Length == 1)
                                          ? "0" + DateTime.Now.Minute.ToString()
                                          : DateTime.Now.Minute.ToString();
                string hourstring = (DateTime.Now.Hour.ToString().Length == 1)
                                        ? "0" + DateTime.Now.Hour.ToString()
                                        : DateTime.Now.Hour.ToString();
                string filestamp = " @ " + hourstring + "." + minutestring;
                filestamp += " - " + DateTime.Now.Day.ToString() + " " +
                             DateTimeFormatInfo.CurrentInfo.MonthNames[DateTime.Now.Month - 1].Substring(0, 3) + " " +
                             DateTime.Now.Year.ToString();
                string objstring = o.ToString().Substring(o.ToString().LastIndexOf(".") + 1,
                                                          o.ToString().Length - o.ToString().LastIndexOf(".") - 1);
                string filename = (Tag != null)
                                      ? Tag + " " + objstring + filestamp + ".xml"
                                      : objstring + filestamp + ".xml";
                XmlWriter writer = XmlWriter.Create(Variables.Instance.ExportsPath + "\\" + filename);
                writer.WriteStartDocument();
                writer.WriteDocType("ShapeDebug", null, null, null);
                writer.WriteStartElement(o.ToString());
                if (o is GraphNode)
                {
                    GraphNode g = o as GraphNode;
                    WriteShapeContainerValues(writer, g);
                }
                writer.WriteStartElement("Behaviour");
                writer.WriteElementString("Resizable", o.Resizable.ToString());
                writer.WriteElementString("Reshapable", o.Reshapable.ToString());
                writer.WriteElementString("DragsNode", o.DragsNode.ToString());
                writer.WriteElementString("Selectable", o.Selectable.ToString());
                writer.WriteElementString("Movable", o.Movable.ToString());
                writer.WriteElementString("Editable", o.Editable.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
                return Variables.Instance.ExportsPath + filename;
            }
            return "Object was null";
        }

        public static string DebugToXML(GoObject o)
        {
            return DebugToXML(o, null);
        }

        public static void WriteShapeContainerValues(XmlWriter writer, GraphNode g)
        {
            if (g.MetaObject != null)
            {
                writer.WriteAttributeString("MetaObjectID", g.MetaObject.pkid.ToString());
            }
        }

        #endregion Methods 
    }
}