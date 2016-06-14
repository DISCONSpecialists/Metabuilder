namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class SystemComponentSort : BaseSorter {
        
        public SystemComponentSort() {
            SortOrder = new string[] {
                    "Type",
                    "Name",
                    "Description",
                    "SeverityRating",
                    "Configuration",
                    "MACAddress",
                    "StaticIP",
                    "NetworkAddress3",
                    "NetworkAddress4",
                    "NetworkAddress5",
                    "Make",
                    "Model",
                    "SerialNumber",
                    "AssetNumber",
                    "isDNS",
                    "isDHCP",
                    "Capacity",
                    "Number_Of_Disks",
                    "CPU_Type",
                    "CPU_Speed",
                    "Monitor",
                    "Video_Card",
                    "Mem_Total",
                    "DatePurchased",
                    "UnderWarranty",
                    "Domain"};
        }
    }
}
