namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class ImplicationSort : BaseSorter {
        
        public ImplicationSort() {
            SortOrder = new string[] {
                    "Name",
                    "Int_Capability_Indicator",
                    "Ext_Inf_Indicator"};
        }
    }
}
