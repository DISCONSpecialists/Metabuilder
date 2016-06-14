namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class StorageComponentSort : BaseSorter {
        
        public StorageComponentSort() {
            SortOrder = new string[] {
                    "Type",
                    "Name",
                    "Description",
                    "SeverityIndicator",
                    "Configuration",
                    "NetworkAddress1",
                    "NetworkAddress2",
                    "NetworkAddress3",
                    "NetworkAddress4",
                    "NetworkAddress5",
                    "Make",
                    "Model",
                    "SerialNumber",
                    "AssetNumber",
                    "Capacity",
                    "Number_of_Disks",
                    "DatePurchased",
                    "UnderWarranty",
                    "FileSystem"};
        }
    }
}
