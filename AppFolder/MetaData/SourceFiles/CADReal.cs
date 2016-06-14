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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.CADRealSort))]
    [Serializable()]
    public class CADReal : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public System.DateTime caddate;
        
        public Attachment fileattachment;
        
        public CADReal() {
        }
        
        [DescriptionAttribute("The CAD Name")]
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
        
        [DescriptionAttribute("The CAD Date")]
        [CategoryAttribute("General")]
        public virtual System.DateTime CADDate {
            get {
                return this.caddate;
            }
            set {
                this.caddate = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("CAD File")]
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
