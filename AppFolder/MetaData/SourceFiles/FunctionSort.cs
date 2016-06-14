namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class FunctionSort : BaseSorter {
        
        public FunctionSort() {
            SortOrder = new string[] {
                    "Name",
                    "ContextualIndicator",
                    "FunctionCriticality",
                    "ManagementAbility",
                    "InfoAvailability",
                    "LegalAspects",
                    "Technology",
                    "Budget",
                    "Energy",
                    "RawMaterial",
                    "SkillAvailability",
                    "Manpower",
                    "Efficiency",
                    "Effectiveness"};
        }
    }
}
