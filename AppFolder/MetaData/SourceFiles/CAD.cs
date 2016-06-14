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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.CADSort))]
    [Serializable()]
    public class CAD : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public Employee controlowner;
        
        public Employee preparedby;
        
        public Employee reviewedby;
        
        public System.DateTime controldate;
        
        public Attachment fileattachment;
        
        public CAD() {
        }
        
        [DescriptionAttribute("CAD Name")]
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
        
        [DescriptionAttribute("COwner")]
        [CategoryAttribute("General")]
        public virtual Employee ControlOwner {
            get {
                return this.controlowner;
            }
            set {
                this.controlowner = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("CPreparedBy")]
        [CategoryAttribute("General")]
        public virtual Employee PreparedBy {
            get {
                return this.preparedby;
            }
            set {
                this.preparedby = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("CReviewer")]
        [CategoryAttribute("General")]
        public virtual Employee ReviewedBy {
            get {
                return this.reviewedby;
            }
            set {
                this.reviewedby = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Date")]
        [CategoryAttribute("General")]
        public virtual System.DateTime ControlDate {
            get {
                return this.controldate;
            }
            set {
                this.controldate = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Fileattachment")]
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
