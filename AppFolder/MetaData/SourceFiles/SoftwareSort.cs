namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class SoftwareSort : BaseSorter {
        
        public SoftwareSort() {
            SortOrder = new string[] {
                    "Name",
                    "Type",
                    "Description",
                    "SeverityRating",
                    "Version",
                    "ID",
                    "Publisher",
                    "InternalName",
                    "Language",
                    "DateCreated",
                    "isDNS",
                    "isDHCP",
                    "isLicensed",
                    "LicenseNumber",
                    "DatePurchased",
                    "Copyright",
                    "Configuration"};
        }
    }
}
