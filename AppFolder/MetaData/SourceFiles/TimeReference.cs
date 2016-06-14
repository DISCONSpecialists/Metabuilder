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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.TimeReferenceSort))]
    [Serializable()]
    public class TimeReference : MetaBuilder.Meta.MetaBase {
        
        public string reference;
        
        public System.DateTime datevalue;
        
        public TimeReference() {
        }
        
        [DescriptionAttribute("The reference name")]
        [CategoryAttribute("General")]
        public virtual string Reference {
            get {
                return this.reference;
            }
            set {
                this.reference = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("The reference value")]
        [CategoryAttribute("General")]
        public virtual System.DateTime DateValue {
            get {
                return this.datevalue;
            }
            set {
                this.datevalue = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return (DateValue!=null)?Reference + " " + DateValue.ToString():Reference;
        }
    }
}
