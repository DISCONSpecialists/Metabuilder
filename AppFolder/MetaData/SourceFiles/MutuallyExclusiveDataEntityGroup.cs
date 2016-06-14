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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.MutuallyExclusiveDataEntityGroupSort))]
    [Serializable()]
    public class MutuallyExclusiveDataEntityGroup : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public MutuallyExclusiveDataEntityGroup() {
        }
        
        [DescriptionAttribute("Mutually Exclusive Data Entity Group Name")]
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
