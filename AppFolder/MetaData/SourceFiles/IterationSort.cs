namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class IterationSort : BaseSorter {
        
        public IterationSort() {
            SortOrder = new string[] {
                    "Name",
                    "IterationType"};
        }
    }
}
