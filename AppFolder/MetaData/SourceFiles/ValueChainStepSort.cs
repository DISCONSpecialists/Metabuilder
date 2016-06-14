namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    
    
    public class ValueChainStepSort : BaseSorter {
        
        public ValueChainStepSort() {
            SortOrder = new string[] {
                    "Name",
                    "FormalDefinition",
                    "LaymansDefinition",
                    "StartState",
                    "EndState"};
        }
    }
}
