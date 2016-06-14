namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class OrganizationalUnitSort : BaseSorter {
        
        public OrganizationalUnitSort() {
            SortOrder = new string[] {
                    "Name",
                    "Type"};
        }
    }
}
