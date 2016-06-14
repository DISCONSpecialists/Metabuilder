namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    public class ScenarioSort : BaseSorter {
        
        public ScenarioSort() {
            SortOrder = new string[] {
                    "Name",
                    "Acc_Prob_of_Real",
                    "Start_Date",
                    "End_Date"};
        }
    }
}
