namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class ModelRealSort : BaseSorter {
        
        public ModelRealSort() {
            SortOrder = new string[] {
                    "Name",
                    "Number",
                    "Documented",
                    "BusinessArea",
                    "FileAttachment"};
        }
    }
}
