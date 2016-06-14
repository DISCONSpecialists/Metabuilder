namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class ProcessSort : BaseSorter {
        
        public ProcessSort() {
            SortOrder = new string[] {
                    "Name",
                    "ExecutionIndicator",
                    "ContextualIndicator",
                    "SequenceNumber"};
        }
    }
}
