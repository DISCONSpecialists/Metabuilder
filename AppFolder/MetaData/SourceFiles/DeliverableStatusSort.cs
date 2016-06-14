namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    
    
    public class DeliverableStatusSort : BaseSorter {
        
        public DeliverableStatusSort() {
            SortOrder = new string[] {
                    "Deliverable",
                    "Date",
                    "PercentageComplete"};
        }
    }
}
