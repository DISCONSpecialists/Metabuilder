namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class FlowDescriptionSort : BaseSorter {
        
        public FlowDescriptionSort() {
            SortOrder = new string[] {
                    "Sequence",
                    "Description",
                    "Condition"};
        }
    }
}
