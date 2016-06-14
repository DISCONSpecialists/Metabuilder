namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class NetworkComponentSort : BaseSorter {
        
        public NetworkComponentSort() {
            SortOrder = new string[] {
                    "Type",
                    "Name",
                    "Description",
                    "SeverityRating",
                    "Configuration",
                    "MacAddress",
                    "NetworkAddress2",
                    "NetworkAddress3",
                    "NetworkAddress4",
                    "NetworkAddress5",
                    "Make",
                    "Model",
                    "SerialNumber",
                    "AssetNumber",
                    "ConnectionSpeed",
                    "Number_of_Ports",
                    "Number_of_Ports_Available",
                    "Range",
                    "isDNS",
                    "isDHCP",
                    "isManaged",
                    "Mem_Total",
                    "DatePurchased",
                    "UnderWarranty"};
        }
    }
}
