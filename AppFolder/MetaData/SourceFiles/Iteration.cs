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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.IterationSort))]
    [Serializable()]
    public class Iteration : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string iterationtype;
        
        public Iteration() {
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
        
        [DomainAttribute("IterationType")]
        [DescriptionAttribute("IterationType")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("IterationType")]
        [ValueMember("PossibleValue")]
        [DisplayMember("Description")]
        public virtual string IterationType {
            get {
                return this.iterationtype;
            }
            set {
                this.iterationtype = value;
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
