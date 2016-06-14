using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;
using Northwoods.Go.Xml;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence
{
    public class BaseTransformer : GoXmlTransformer
    {

        #region Constructors (1)

        public BaseTransformer()
            : base()
        {
            this.IdAttributeUsedForSharedObjects = true;
        }

        #endregion Constructors

        #region Methods (2)

        // Public Methods (2) 

        public float FloatAttr(string s, float defaultvalue)
        {
            string val = Core.strings.ForceDot(StringAttr(s, ""));
            if (!string.IsNullOrEmpty(val))
            {
                return float.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
            }
            return defaultvalue;
        }

        public override void GenerateAttributes(object obj)
        {
            try
            {
                if (!(obj is GraphNodeGrid))
                    if (obj is FishLink)
                    {
                        FishLink flink = obj as FishLink;
                        if (flink.IsValidFishLink())
                            base.GenerateAttributes(obj);
                    }
                    else
                    {
                        if (obj is Meta.MetaBase)
                        {

                            if (IdAttributeUsedForSharedObjects)
                                this.Writer.MakeShared(obj);
                        }
                        base.GenerateAttributes(obj);
                    }
            }
            catch
            {
            }
        }

        #endregion Methods

    }
}
