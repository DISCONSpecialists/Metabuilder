namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class EmployeeSort : BaseSorter {
        
        public EmployeeSort() {
            SortOrder = new string[] {
                    "Name",
                    "Surname",
                    "Title",
                    "EmployeeNumber",
                    "IDNumber",
                    "Email",
                    "Telephone"};
        }
    }
}
