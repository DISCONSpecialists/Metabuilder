namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    
    
    public class IssueSort : BaseSorter {
        
        public IssueSort() {
            SortOrder = new string[] {
                    "Description",
                    "DateRaised",
                    "GrossExposure",
                    "VolumeOfActivity",
                    "LikelihoodOfMisstatement",
                    "DeficiencyType"};
        }
    }
}
