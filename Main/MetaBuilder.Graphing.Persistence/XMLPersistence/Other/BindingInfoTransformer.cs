using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Other
{
    public class BindingInfoTransformer : BaseTransformer
    {
        
		#region Constructors (1) 

        public BindingInfoTransformer()
            : base()
        {
            TransformerType = typeof(BindingInfo);
            ElementName = "bindinginfo";
            IdAttributeUsedForSharedObjects = true;
            BodyConsumesChildElements = false;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            return new BindingInfo();
        }
        const char splitchar = '|';
        const char splitcharForEachBinding = ':';
        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            BindingInfo binfo = obj as BindingInfo;

            binfo.BindingClass = StringAttr("BClass", "");
            binfo.Bindings = new Dictionary<string, string>();
            string bindings = StringAttr("Bindings", "");
 
            string[] labelBindings = bindings.Split(splitchar);
            
            for (int i = 0;i<labelBindings.Length;i++)
            {
                string[] lblAndBinding = labelBindings[i].Split(splitcharForEachBinding);
                binfo.Bindings.Add(lblAndBinding[0], lblAndBinding[1]);
            }
        }

        public override void GenerateAttributes(object obj)
        {
            BindingInfo binfo = obj as BindingInfo;
            WriteAttrVal("BClass", binfo.BindingClass);
            StringBuilder sbBindings = new StringBuilder();
            foreach (KeyValuePair<string,string> kvpair in binfo.Bindings)
            {
                sbBindings.Append(kvpair.Key).Append(":").Append(kvpair.Value).Append("|");
            }

            string BindingAttributeValue = sbBindings.ToString();
            if (BindingAttributeValue.EndsWith("|"))
            {
                BindingAttributeValue = BindingAttributeValue.Substring(0, BindingAttributeValue.Length - 1);
            }
            WriteAttrVal("Bindings", BindingAttributeValue);
            
            
        }


		#endregion Methods 
    }
}
