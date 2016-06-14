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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.RelevanceSort))]
    [Serializable()]
    public class Relevance : MetaBuilder.Meta.MetaBase {
        
        public string relevant;
        
        public Relevance() {
        }
        
        [DomainAttribute("YesNo")]
        [DescriptionAttribute("Relevance between mapped objects")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("YesNo")]
        [ValueMember("PossibleValue")]
        [DisplayMember("Description")]
        public virtual string Relevant {
            get {
                return this.relevant;
            }
            set {
                this.relevant = value;
            }
        }
        
        public override string ToString() {
return Relevant;
        }
    }
}
