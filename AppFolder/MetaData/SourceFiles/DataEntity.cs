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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.DataEntitySort))]
    [Serializable()]
    public class DataEntity : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string objectorevent;
        
        public DataEntity() {
        }
        
        [DescriptionAttribute("Data Entity Name")]
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
        
        [DescriptionAttribute("Is this entity an object or an event?")]
        [CategoryAttribute("General")]
        public virtual string ObjectOrEvent {
            get {
                return this.objectorevent;
            }
            set {
                this.objectorevent = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
