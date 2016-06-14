using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public class LegendNodeTransformer : BaseGoObjectTransformer
    {
        public LegendNodeTransformer()
            : base()
        {
            this.BodyConsumesChildElements = true;
            this.TransformerType = typeof(LegendNode);
            this.ElementName = "legendnode";
            this.IdAttributeUsedForSharedObjects = true;
        }

        public override object Allocate()
        {
            return new LegendNode(LegendType.ColorAndClass);
        }

        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj);
            WriteAttrVal("bounds", (obj as LegendNode).Bounds);
            WriteAttrVal("legendtype", (obj as LegendNode).MyLegendType.ToString());
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            (obj as LegendNode).Bounds = RectangleFAttr("bounds", new RectangleF());
            (obj as LegendNode).MyLegendType = (LegendType)Enum.Parse(typeof(LegendType), StringAttr("legendtype", "Class"));
        }
        public override void ConsumeChild(object parent, object child)
        {
            if (child is LegendItem)
            {
                (parent as LegendNode).Add(child);
                return;
            }
            else if (child is GoText) //Do not consume gotext because it is created each time
            {
                return;
            }

            base.ConsumeChild(parent, child);
        }
    }

    public class LegendItemTransformer : BaseGoObjectTransformer
    {
        public LegendItemTransformer()
            : base()
        {
            this.BodyConsumesChildElements = true;
            this.TransformerType = typeof(LegendItem);
            this.ElementName = "legenditem";
            this.IdAttributeUsedForSharedObjects = true;
        }

        public override object Allocate()
        {
            return new LegendItem();
        }

        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj);
            WriteAttrVal("itemkey", (obj as LegendItem).Key);
            WriteAttrVal("classkey", (obj as LegendItem).ClassKey);
            WriteAttrVal("colorkey", (obj as LegendItem).ColorKey);
            WriteAttrVal("itemvisibility", (obj as LegendItem).Visibility);
        }

        public override void ConsumeChild(object parent, object child)
        {
            if (child is GoObject || child is GoText)
            {
                (parent as LegendItem).Add(child);

                if (child is GoText)
                    (parent as LegendItem).MyLabel = child as GoText;
                if (child is GoObject)
                    (parent as LegendItem).MyObject = child as GoObject;
                return;
            }

            base.ConsumeChild(parent, child);
        }

        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);
            if (StringAttr("itemkey", "").Length > 0) //Legacy
                (obj as LegendItem).ClassKey = StringAttr("itemkey", "Cannot load key");

            (obj as LegendItem).ClassKey = StringAttr("classkey", "");
            string colorKey = StringAttr("colorkey", "");
            int i = 0;

            string firstKey = "";
            string secondKey = "";
            string firstKeyToReplace = "";
            string secondKeyToReplace = "";
            //Color [Lime]=Color [A=255, R=255, G=255, B=255] //Special Case Must convert the WORD to ARGB Component
            foreach (string singleColor in colorKey.Split('['))
            {
                //"Color " First
                //"[Lime]=Color " Second
                //"[A=255, R=255, G=255, B=255]" Third
                //if (i == 0)
                //{
                //}
                //else
                if (i == 1)
                {
                    if (singleColor.Contains(","))
                    {
                        //we dont care
                    }
                    else
                    {
                        //get the key between [ and ] which should be a known color
                        firstKeyToReplace = singleColor.Substring(0, singleColor.LastIndexOf("]"));
                        try
                        {
                            Color theColor = Color.FromName(firstKeyToReplace);
                            firstKey = getARGBString(theColor);
                        }
                        catch
                        {
                            Core.Log.WriteLog("LegendItemTransformer ComsumeObjectFinish " + colorKey + " first color cannot find known color in" + firstKeyToReplace);
                        }
                    }
                }
                else if (i == 2)
                {
                    if (singleColor.Contains(","))
                    {
                        //we dont care
                    }
                    else
                    {
                        //get the key between [ and ] which should be a known color
                        secondKeyToReplace = singleColor.Substring(0, singleColor.LastIndexOf("]"));
                        try
                        {
                            Color theColor = Color.FromName(secondKeyToReplace);
                            secondKey = getARGBString(theColor);
                        }
                        catch
                        {
                            Core.Log.WriteLog("LegendItemTransformer ComsumeObjectFinish " + colorKey + " second color cannot find known color in" + secondKeyToReplace);
                        }
                    }
                }
                i++;
            }

            if (!(string.IsNullOrEmpty(firstKey)))
            {
                colorKey = colorKey.Replace(firstKeyToReplace, firstKey);
            }

            if (!(string.IsNullOrEmpty(secondKey)))
            {
                colorKey = colorKey.Replace(secondKeyToReplace, secondKey);
            }

            (obj as LegendItem).ColorKey = colorKey;
            (obj as LegendItem).Visibility = true;
        }

        private string getARGBString(Color color)
        {
            //A=255, R=255, G=255, B=255
            return "A=" + color.A.ToString() + ", R=" + color.R.ToString() + ", G=" + color.G.ToString() + ", B=" + color.B.ToString();
        }
    }
}