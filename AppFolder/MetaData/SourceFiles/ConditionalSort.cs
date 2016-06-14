namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class ConditionalSort : BaseSorter {
        
        public ConditionalSort() {
            SortOrder = new string[] {
                    "Name",
                    "ConditionalType"};
        }
    }
}
