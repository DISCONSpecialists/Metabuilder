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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.GoToSort))]
    [Serializable()]
    public class GoTo : MetaBuilder.Meta.MetaBase {
        
        public string gototarget;
        
        public GoTo() {
        }
        
        [DescriptionAttribute("The Target of the GoTo Operation")]
        [CategoryAttribute("General")]
        public virtual string GoToTarget {
            get {
                return this.gototarget;
            }
            set {
                this.gototarget = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return GoToTarget;
        }
    }
}
