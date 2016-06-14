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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.EnvironmentSort))]
    [Serializable()]
    public class Environment : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public Environment() {
        }
        
        [DomainAttribute("EnvironmentType")]
        [DescriptionAttribute("Name")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("EnvironmentType")]
        [ValueMember("PossibleValue")]
        [DisplayMember("Description")]
        public virtual string Name {
            get {
                return this.name;
            }
            set {
                this.name = value;
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
