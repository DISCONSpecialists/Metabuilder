using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using Northwoods.Go;
using Northwoods.Go.Xml;
using MetaBuilder.Graphing.Shapes.General;

namespace ShapeBuilding.XMLPersistence
{
    public class PortTransformer:GoXmlTransformer
    {
        public PortTransformer()
            : base()
        {
            this.TransformerType = typeof(MetaBuilder.Graphing.Shapes.QuickPort);
            this.ElementName = "quickport"; 
            this.IdAttributeUsedForSharedObjects = true; 
        }

        
        public override void GenerateAttributes(Object obj) 
        { 
            QuickPort qp = obj as QuickPort;
            base.GenerateAttributes(qp); 
            
            // Save Gradient
            IBehaviourShape ibShape = qp as IBehaviourShape;
            GradientBehaviour gbeh =  ibShape.Manager.GetExistingBehaviour(typeof(GradientBehaviour)) as GradientBehaviour;
            if (gbeh!=null)
            {
                WriteAttrVal("grad_bordercolor", gbeh.MyBrush.BorderColor.ToString());
                WriteAttrVal("grad_outercolor", gbeh.MyBrush.OuterColor.ToString());
                WriteAttrVal("grad_innercolor", gbeh.MyBrush.InnerColor.ToString());
                WriteAttrVal("grad_type", gbeh.MyBrush.GradientType.ToString());
            }

            WriteAttrVal("posX", qp.Position.X.ToString());
            WriteAttrVal("posY", qp.Position.X.ToString());
            WriteAttrVal("width", qp.Width.ToString());
            WriteAttrVal("height", qp.Height.ToString());
            
        }

    }
}
