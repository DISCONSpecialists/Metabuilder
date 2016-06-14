namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class DataViewSort : BaseSorter {
        
        public DataViewSort() {
            SortOrder = new string[] {
                    "Name",
                    "DataViewType"};
        }
    }
}
