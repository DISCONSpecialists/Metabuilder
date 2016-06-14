namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class DatedResponsibilitySort : BaseSorter {
        
        public DatedResponsibilitySort() {
            SortOrder = new string[] {
                    "AssignedResponsibility",
                    "DateCreated"};
        }
    }
}
