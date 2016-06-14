namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    
    
    public class ControlSort : BaseSorter {
        
        public ControlSort() {
            SortOrder = new string[] {
                    "ControlName",
                    "ControlType",
                    "Frequency",
                    "IsExpected",
                    "IsCurrent",
                    "StartDate",
                    "EndDate"};
        }
    }
}
