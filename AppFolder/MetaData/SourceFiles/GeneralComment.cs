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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.GeneralCommentSort))]
    [Serializable()]
    public class GeneralComment : MetaBuilder.Meta.MetaBase {
        
        public string comment;
        
        public GeneralComment() {
        }
        
        [DescriptionAttribute("Textual Commentary")]
        [CategoryAttribute("General")]
        public virtual string Comment {
            get {
                return this.comment;
            }
            set {
                this.comment = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Comment;
        }
    }
}
