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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ValueChainStepSort))]
    [Serializable()]
    public class ValueChainStep : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string formaldefinition;
        
        public string laymansdefinition;
        
        public string startstate;
        
        public string endstate;
        
        public ValueChainStep() {
        }
        
        [DescriptionAttribute("Name of the VCS")]
        [CategoryAttribute("General")]
        public virtual string Name {
            get {
                return this.name;
            }
            set {
                this.name = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Formal Definition of the step")]
        [CategoryAttribute("General")]
        public virtual string FormalDefinition {
            get {
                return this.formaldefinition;
            }
            set {
                this.formaldefinition = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Layman\'s Definition of the step")]
        [CategoryAttribute("General")]
        public virtual string LaymansDefinition {
            get {
                return this.laymansdefinition;
            }
            set {
                this.laymansdefinition = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Start state of this VCS")]
        [CategoryAttribute("State")]
        public virtual string StartState {
            get {
                return this.startstate;
            }
            set {
                this.startstate = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("End state of this VCS")]
        [CategoryAttribute("State")]
        public virtual string EndState {
            get {
                return this.endstate;
            }
            set {
                this.endstate = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
