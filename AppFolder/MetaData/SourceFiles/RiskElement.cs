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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.RiskElementSort))]
    [Serializable()]
    public class RiskElement : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public int weight;
        
        public RiskElement() {
        }
        
        [DescriptionAttribute("Risk Element Name")]
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
        
        [DescriptionAttribute("Risk Weight (out of 100)")]
        [CategoryAttribute("General")]
        public virtual int Weight {
            get {
                return this.weight;
            }
            set {
                this.weight = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
