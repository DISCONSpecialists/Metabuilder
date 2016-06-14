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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.IssueSort))]
    [Serializable()]
    public class Issue : MetaBuilder.Meta.MetaBase {
        
        public string description;
        
        public System.DateTime dateraised;
        
        public string grossexposure;
        
        public string volumeofactivity;
        
        public string likelihoodofmisstatement;
        
        public string deficiencytype;
        
        public Issue() {
        }
        
        [DescriptionAttribute("The Issue\'s Name")]
        [CategoryAttribute("General")]
        public virtual string Description {
            get {
                return this.description;
            }
            set {
                this.description = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("The date the issue was raised (eg. 2006/10/18)")]
        [CategoryAttribute("General")]
        public virtual System.DateTime DateRaised {
            get {
                return this.dateraised;
            }
            set {
                this.dateraised = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("The issue\'s Gross Exposure")]
        [CategoryAttribute("General")]
        public virtual string GrossExposure {
            get {
                return this.grossexposure;
            }
            set {
                this.grossexposure = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("The volume of the activity")]
        [CategoryAttribute("General")]
        public virtual string VolumeOfActivity {
            get {
                return this.volumeofactivity;
            }
            set {
                this.volumeofactivity = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("The likelihood of misstakement")]
        [CategoryAttribute("General")]
        public virtual string LikelihoodOfMisstatement {
            get {
                return this.likelihoodofmisstatement;
            }
            set {
                this.likelihoodofmisstatement = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Design / Operation")]
        [CategoryAttribute("General")]
        public virtual string DeficiencyType {
            get {
                return this.deficiencytype;
            }
            set {
                this.deficiencytype = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Description;
        }
    }
}
