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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ScoreOutOfTenSort))]
    [Serializable()]
    public class ScoreOutOfTen : MetaBuilder.Meta.MetaBase {
        
        public string score;
        
        public ScoreOutOfTen() {
        }
        
        [DomainAttribute("RatingOutOfTen")]
        [DescriptionAttribute("Rating out of 10")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("RatingOutOfTen")]
        [ValueMember("PossibleValue")]
        [DisplayMember("Description")]
        public virtual string Score {
            get {
                return this.score;
            }
            set {
                this.score = value;
            }
        }
        
        public override string ToString() {
return Score;
        }
    }
}
