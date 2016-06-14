namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    
    
    public class ProjectStatusSort : BaseSorter {
        
        public ProjectStatusSort() {
            SortOrder = new string[] {
                    "Project",
                    "PercentageComplete",
                    "ProjectOwner",
                    "ProjectManager",
                    "Date"};
        }
    }
}
