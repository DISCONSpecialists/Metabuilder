namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.ComponentModel;
    using System.Drawing.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    
    
    // This source code file was generated using the DISCON Specialists MetaBuilder engine. Copyright © 2006
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.MissionSort))]
    [Serializable()]
    public class Mission : MetaBuilder.Meta.MetaBase {
        
        public string statement;
        
        public Mission() {
        }
        
        [DescriptionAttribute("The Mission Statement")]
        [CategoryAttribute("General")]
        public virtual string Statement {
            get {
                return this.statement;
            }
            set {
                this.statement = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Statement;
        }
    }
}
