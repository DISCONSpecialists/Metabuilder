namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    
    
    public class StartSort : BaseSorter {
        
        public StartSort() {
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
