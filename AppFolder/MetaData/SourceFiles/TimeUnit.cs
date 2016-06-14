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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.TimeUnitSort))]
    [Serializable()]
    public class TimeUnit : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string type;
        
        public string acronym;
        
        public TimeUnit() {
        }
        
        [DescriptionAttribute("Unit Name")]
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
        
        [DescriptionAttribute("Unit Type")]
        [CategoryAttribute("General")]
        public virtual string Type {
            get {
                return this.type;
            }
            set {
                this.type = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Unit Acronym")]
        [CategoryAttribute("General")]
        public virtual string Acronym {
            get {
                return this.acronym;
            }
            set {
                this.acronym = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name + " <" + Type + ">";
        }
    }
}
