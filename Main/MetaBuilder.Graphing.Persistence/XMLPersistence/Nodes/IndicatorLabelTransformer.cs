using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Primitives;
using Northwoods.Go;
using Northwoods.Go.Xml;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public class IndicatorLabelTransformer:GoXmlTransformer
    {

		#region Constructors (1) 

      public IndicatorLabelTransformer()
            : base()
        {
            this.TransformerType = typeof(IndicatorLabel);
            //ElementName = "indlbl";
            this.IdAttributeUsedForSharedObjects = false;
        }

		#endregion Constructors 

		#region Methods (2) 


		// Public Methods (2) 

        public override object Allocate()
        {
            return null;// new IndicatorLabel();
        }

        public override void GenerateAttributes(object obj)
        {
            //base.GenerateAttributes(obj);
        }


		#endregion Methods 

}

    public class HighlightIndicatorTransformer : GoXmlTransformer
    {

        #region Constructors (1)

        public HighlightIndicatorTransformer()
            : base()
        {
            this.TransformerType = typeof(HighlightIndicator);
            this.IdAttributeUsedForSharedObjects = false;
        }

        #endregion Constructors

        #region Methods (2)


        // Public Methods (2) 

        public override object Allocate()
        {
            return null;
        }

        public override void GenerateAttributes(object obj)
        {

        }


        #endregion Methods

    }
}
