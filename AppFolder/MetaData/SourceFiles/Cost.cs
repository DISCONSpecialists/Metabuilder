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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.CostSort))]
    [Serializable()]
    public class Cost : MetaBuilder.Meta.MetaBase {
        
        public int value;
        
        public Cost() {
        }
        
        [DescriptionAttribute("The Cost")]
        [CategoryAttribute("General")]
        public virtual int Value {
            get {
                return this.value;
            }
            set {
                this.value = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return "R" + String.Format("{0:f}",Value);
        }
    }
}
