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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ProcessStepSort))]
    [Serializable()]
    public class ProcessStep : MetaBuilder.Meta.MetaBase {
        
        public string description;
        
        public ProcessStep() {
        }
        
        [DescriptionAttribute("Describe the Process Step")]
        [CategoryAttribute("General")]
        public virtual string Description {
            get {
                return this.description;
            }
            set {
                this.description = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Description;
        }
    }
}
