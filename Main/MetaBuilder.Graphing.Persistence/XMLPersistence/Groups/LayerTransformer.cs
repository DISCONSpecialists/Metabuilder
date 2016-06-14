using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using Northwoods.Go.Xml;
namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Groups
{
    public class LayerTransformer: GoXmlTransformer
    {

		#region Constructors (1) 

        public LayerTransformer()
            : base()
        {
            this.TransformerType = typeof(GoLayer);
            this.ElementName = "goLayer";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

    }
}
