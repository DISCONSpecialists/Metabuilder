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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ActivitySort))]
    [Serializable()]
    public class Activity : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string executionindicator;
        
        public string contextualindicator;
        
        public Activity() {
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
        
        [DomainAttribute("ExecutionIndicator")]
        [DescriptionAttribute("Execution Indicator")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("ExecutionIndicator")]
        [ValueMember("PossibleValue")]
        [DisplayMember("Description")]
        public virtual string ExecutionIndicator {
            get {
                return this.executionindicator;
            }
            set {
                this.executionindicator = value;
            }
        }
        
        [DomainAttribute("ContextualIndicator")]
        [DescriptionAttribute("Contextual Indicator")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("ContextualIndicator")]
        [ValueMember("PossibleValue")]
        [DisplayMember("Description")]
        public virtual string ContextualIndicator {
            get {
                return this.contextualindicator;
            }
            set {
                this.contextualindicator = value;
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
