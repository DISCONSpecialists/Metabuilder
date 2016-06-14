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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.CSFSort))]
    [Serializable()]
    public class CSF : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string number;
        
        public CSF() {
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
        
        [DescriptionAttribute("Number")]
        [CategoryAttribute("General")]
        public virtual string Number {
            get {
                return this.number;
            }
            set {
                this.number = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Number;
        }
    }
}
