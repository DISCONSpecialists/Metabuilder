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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.StrategicImplicationSort))]
    [Serializable()]
    public class StrategicImplication : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public int externalinfluencerating;
        
        public int internalcapacityrating;
        
        public StrategicImplication() {
        }
        
        [DescriptionAttribute("Strategic Implication Name")]
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
        
        [DescriptionAttribute("External Influence Rating: Opportunity 10 to Threat -10")]
        [CategoryAttribute("General")]
        public virtual int ExternalInfluenceRating {
            get {
                return this.externalinfluencerating;
            }
            set {
                this.externalinfluencerating = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Internal Capacity Rating: Strength 10 to Weakness -10")]
        [CategoryAttribute("General")]
        public virtual int InternalCapacityRating {
            get {
                return this.internalcapacityrating;
            }
            set {
                this.internalcapacityrating = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
