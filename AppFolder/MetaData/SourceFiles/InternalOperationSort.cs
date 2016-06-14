namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    
    
    public class InternalOperationSort : BaseSorter {
        
        public InternalOperationSort() {
            SortOrder = new string[] {
                    "Sequence",
                    "DescriptionOfProcess",
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
