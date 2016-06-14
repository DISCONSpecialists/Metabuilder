namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class ActivitySort : BaseSorter {
        
        public ActivitySort() {
            SortOrder = new string[] {
                    "Name",
                    "ExecutionIndicator",
                    "ContextualIndicator"};
        }
    }
}
