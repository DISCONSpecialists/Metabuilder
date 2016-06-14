namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    
    
    public class InterruptSort : BaseSorter {
        
        public InterruptSort() {
            SortOrder = new string[] {
                    "Sequence",
                    "Process",
                    "Conditions",
                    "Comments",
                    "Class",
                    "DesignMappingName",
                    "DesignMappingVersion",
                    "DesignMappingID",
                    "TransactionVolume",
                    "TransactionFrequency",
                    "RateOfVolumeChange",
                    "TransactionDuration"};
        }
    }
}
