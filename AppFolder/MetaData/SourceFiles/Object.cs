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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ObjectSort))]
    [Serializable()]
    public class Object : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public Object() {
        }
        
        [DescriptionAttribute("Name of the Object")]
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
        
        public override string ToString() {
return Name;
        }
    }
}
