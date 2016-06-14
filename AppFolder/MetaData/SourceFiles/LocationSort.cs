namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class LocationSort : BaseSorter {
        
        public LocationSort() {
            SortOrder = new string[] {
                    "Name",
                    "LocationType"};
        }
    }
}
