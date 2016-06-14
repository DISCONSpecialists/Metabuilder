namespace MetaBuilder.Meta.Generated {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Design;
    using MetaBuilder.Meta;
    using MetaBuilder.Meta.Editors;
    using MetaBuilder.BusinessLogic;
    
    
    // This source code file was generated using the DISCON Specialists MetaBuilder engine. Copyright © 2006
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ImplicationSort))]
    [Serializable()]
    public class Implication : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public int int_capability_indicator;
        
        public int ext_inf_indicator;
        
        public Implication() {
        }
        
        [DescriptionAttribute("Name")]
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
        
        [DescriptionAttribute("Internal Capability Indicator")]
        [CategoryAttribute("General")]
        public virtual int Int_Capability_Indicator {
            get {
                return this.int_capability_indicator;
            }
            set {
                this.int_capability_indicator = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("External Influence Indicator")]
        [CategoryAttribute("General")]
        public virtual int Ext_Inf_Indicator {
            get {
                return this.ext_inf_indicator;
            }
            set {
                this.ext_inf_indicator = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
