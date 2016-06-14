namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class DataTableSort : BaseSorter {
        
        public DataTableSort() {
            SortOrder = new string[] {
                    "Name",
                    "InitialPopulation",
                    "GrowthRateOverTime",
                    "RecordSize"};
        }
    }
}
