namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class JobPositionSort : BaseSorter {
        
        public JobPositionSort() {
            SortOrder = new string[] {
                    "Name",
                    "TotalRequired",
                    "TotalOccupied",
                    "TotalAvailable",
                    "TimeStamp"};
        }
    }
}
