namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    
    
    public class PersonSort : BaseSorter {
        
        public PersonSort() {
            SortOrder = new string[] {
                    "Firstname",
                    "Surname",
                    "IDNumber",
                    "Title"};
        }
    }
}
