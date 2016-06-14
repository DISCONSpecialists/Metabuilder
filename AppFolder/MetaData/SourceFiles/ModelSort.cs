namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class ModelSort : BaseSorter {
        
        public ModelSort() {
            SortOrder = new string[] {
                    "Name",
                    "FileAttachment",
                    "Number",
                    "Documented",
                    "BusinessArea",
                    "ModelOwner",
                    "ModelUser",
                    "ModelSuperUser"};
        }
    }
}
