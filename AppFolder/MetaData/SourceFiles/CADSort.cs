namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class CADSort : BaseSorter {
        
        public CADSort() {
            SortOrder = new string[] {
                    "Name",
                    "ControlOwner",
                    "PreparedBy",
                    "ReviewedBy",
                    "ControlDate",
                    "FileAttachment"};
        }
    }
}
