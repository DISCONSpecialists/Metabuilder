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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ModelRealSort))]
    [Serializable()]
    public class ModelReal : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string number;
        
        public string documented;
        
        public string businessarea;
        
        public Attachment fileattachment;
        
        public ModelReal() {
        }
        
        [DescriptionAttribute("The Model\'s Name")]
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
        
        [DescriptionAttribute("The Model\'s Number")]
        [CategoryAttribute("General")]
        public virtual string Number {
            get {
                return this.number;
            }
            set {
                this.number = value;
                this.PropertyChanged();
            }
        }
        
        [DomainAttribute("YesNo")]
        [DescriptionAttribute("Has the model been documented?")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("YesNo")]
        [ValueMember("PossibleValue")]
        [DisplayMember("Description")]
        public virtual string Documented {
            get {
                return this.documented;
            }
            set {
                this.documented = value;
            }
        }
        
        [DescriptionAttribute("The Business Area where this model fits in")]
        [CategoryAttribute("General")]
        public virtual string BusinessArea {
            get {
                return this.businessarea;
            }
            set {
                this.businessarea = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("File Attachment")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.FileEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public virtual Attachment FileAttachment {
            get {
                return this.fileattachment;
            }
            set {
                this.fileattachment = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
