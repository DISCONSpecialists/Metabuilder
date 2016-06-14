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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ModelSort))]
    [Serializable()]
    public class Model : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public Attachment fileattachment;
        
        public string number;
        
        public string documented;
        
        public OrganizationalUnit businessarea;
        
        public Employee modelowner;
        
        public Employee modeluser;
        
        public Employee modelsuperuser;
        
        public Model() {
        }
        
        [DescriptionAttribute("The Name of the Model")]
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
        
        [DescriptionAttribute("The attached model")]
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
        
        [DescriptionAttribute("Model Number")]
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
        [DescriptionAttribute("Documented?")]
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
        
        [DescriptionAttribute("BusinessArea")]
        [CategoryAttribute("General")]
        public virtual OrganizationalUnit BusinessArea {
            get {
                return this.businessarea;
            }
            set {
                this.businessarea = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Model Owner")]
        [CategoryAttribute("General")]
        public virtual Employee ModelOwner {
            get {
                return this.modelowner;
            }
            set {
                this.modelowner = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Model User")]
        [CategoryAttribute("General")]
        public virtual Employee ModelUser {
            get {
                return this.modeluser;
            }
            set {
                this.modeluser = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Model SuperUser")]
        [CategoryAttribute("General")]
        public virtual Employee ModelSuperUser {
            get {
                return this.modelsuperuser;
            }
            set {
                this.modelsuperuser = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
