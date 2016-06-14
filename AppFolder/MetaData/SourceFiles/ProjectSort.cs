namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    
    
    public class ProjectSort : BaseSorter {
        
        public ProjectSort() {
            SortOrder = new string[] {
                    "Name",
                    "StartDate",
                    "EndDate",
                    "Description",
                    "ProjectBrief",
                    "MandatedBy",
                    "MandateDate",
                    "ProjectWillingness",
                    "ProjectMethodAvailable",
                    "BusinessHasUnderstandingOfProblem",
                    "BusinessHasUnderstandingOfSolution"};
        }
    }
}
