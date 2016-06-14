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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ConnectionSpeedSort))]
    [Serializable()]
    public class ConnectionSpeed : MetaBuilder.Meta.MetaBase {
        
        public string value;
        
        public ConnectionSpeed() {
        }
        
        [DescriptionAttribute("Value")]
        [CategoryAttribute("General")]
        public virtual string Value {
            get {
                return this.value;
            }
            set {
                this.value = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Value;
        }
    }
}
