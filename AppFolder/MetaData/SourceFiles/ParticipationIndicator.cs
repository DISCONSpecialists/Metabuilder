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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ParticipationIndicatorSort))]
    [Serializable()]
    public class ParticipationIndicator : MetaBuilder.Meta.MetaBase {
        
        public string participation;
        
        public ParticipationIndicator() {
        }
        
        [DomainAttribute("YesNo")]
        [DescriptionAttribute("Participation Indicator")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("YesNo")]
        [ValueMember("PossibleValue")]
        [DisplayMember("Description")]
        public virtual string Participation {
            get {
                return this.participation;
            }
            set {
                this.participation = value;
            }
        }
        
        public override string ToString() {
return Participation.ToString();
        }
    }
}
