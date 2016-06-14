namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class CADRealSort : BaseSorter {
        
        public CADRealSort() {
            SortOrder = new string[] {
                    "Name",
                    "CADDate",
                    "FileAttachment"};
        }
    }
}
