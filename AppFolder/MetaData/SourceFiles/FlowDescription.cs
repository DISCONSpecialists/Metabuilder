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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.FlowDescriptionSort))]
    [Serializable()]
    public class FlowDescription : MetaBuilder.Meta.MetaBase {
        
        public string sequence;
        
        public string description;
        
        public string condition;
        
        public FlowDescription() {
        }
        
        [DescriptionAttribute("Numbered Sequence in which this flow occurs")]
        [CategoryAttribute("General")]
        public virtual string Sequence {
            get {
                return this.sequence;
            }
            set {
                this.sequence = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("A textual free-form description of this flow")]
        [CategoryAttribute("General")]
        public virtual string Description {
            get {
                return this.description;
            }
            set {
                this.description = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Any pre-existing conditions that must exist")]
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
        
        public override string ToString() {
return (Condition!=null)?((Condition.Length>0)?Sequence + " ?" + Condition + " - " + Description:Sequence + " " + Description):Sequence + " " + Description;
        }
    }
}
