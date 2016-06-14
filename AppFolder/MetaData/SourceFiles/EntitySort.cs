namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class EntitySort : BaseSorter {
        
        public EntitySort() {
            SortOrder = new string[] {
                    "Name",
                    "EntityType"};
        }
    }
}
