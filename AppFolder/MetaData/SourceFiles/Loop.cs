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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.LoopSort))]
    [Serializable()]
    public class Loop : MetaBuilder.Meta.MetaBase {
        
        public string condition;
        
        public string type;
        
        public Loop() {
        }
        
        [DescriptionAttribute("The loop\'s condition")]
        [CategoryAttribute("General")]
        public virtual string Condition {
            get {
                return this.condition;
            }
            set {
                this.condition = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("The loop type (while, do while, for, foreach)")]
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
        
        public override string ToString() {
return Condition + " <" + Type + ">";
        }
    }
}
