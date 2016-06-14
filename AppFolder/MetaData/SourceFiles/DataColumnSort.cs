namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class DataColumnSort : BaseSorter {
        
        public DataColumnSort() {
            SortOrder = new string[] {
                    "Name",
                    "DomainDef"};
        }
    }
}
