namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class DataSchemaSort : BaseSorter {
        
        public DataSchemaSort() {
            SortOrder = new string[] {
                    "Name",
                    "DataSchemaType"};
        }
    }
}
