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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.StrategicScenarioSort))]
    [Serializable()]
    public class StrategicScenario : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public System.DateTime datefrom;
        
        public System.DateTime dateto;
        
        public StrategicScenario() {
        }
        
        [DescriptionAttribute("Strategic Scenario Name")]
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
        
        [DescriptionAttribute("Date From")]
        [CategoryAttribute("General")]
        public virtual System.DateTime DateFrom {
            get {
                return this.datefrom;
            }
            set {
                this.datefrom = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Date To")]
        [CategoryAttribute("General")]
        public virtual System.DateTime DateTo {
            get {
                return this.dateto;
            }
            set {
                this.dateto = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
