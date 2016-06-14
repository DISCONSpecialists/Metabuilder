namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class PeripheralSort : BaseSorter {
        
        public PeripheralSort() {
            SortOrder = new string[] {
                    "Type",
                    "Name",
                    "Description",
                    "SeverityRating",
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
                    "Copy_PPM",
                    "Print_PPM",
                    "isColor",
                    "isManaged",
                    "isNetwork",
                    "DatePurchased",
                    "UnderWarranty",
                    "Contract"};
        }
    }
}
