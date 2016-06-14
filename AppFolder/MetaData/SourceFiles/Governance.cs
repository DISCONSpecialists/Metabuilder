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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.GovernanceSort))]
    [Serializable()]
    public class Governance : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string reference;
        
        public Governance() {
        }
        
        [DescriptionAttribute("The Governance Name")]
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
        
        [DescriptionAttribute("The Reference (if applicable)")]
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
        
        public override string ToString() {
return (!String.IsNullOrEmpty(Reference))?Reference + " " + Name:Name;;
        }
    }
}
