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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.SoftwareSort))]
    [Serializable()]
    public class Software : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string type;
        
        public string description;
        
        public string severityrating;
        
        public string version;
        
        public string id;
        
        public string publisher;
        
        public string internalname;
        
        public string language;
        
        public string datecreated;
        
        public string isdns;
        
        public string isdhcp;
        
        public string islicensed;
        
        public string licensenumber;
        
        public string datepurchased;
        
        public string copyright;
        
        public LongText configuration;
        
        public Software() {
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
        
        [DomainAttribute("SoftwareType")]
        [DescriptionAttribute("Software Type")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("SoftwareType")]
        [ValueMember("PossibleValue")]
        [DisplayMember("Description")]
        public virtual string Type {
            get {
                return this.type;
            }
            set {
                this.type = value;
            }
        }
        
        [DescriptionAttribute("Description")]
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
        
        [DescriptionAttribute("Severity Rating")]
        [CategoryAttribute("General")]
        public virtual string SeverityRating {
            get {
                return this.severityrating;
            }
            set {
                this.severityrating = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Version")]
        [CategoryAttribute("General")]
        public virtual string Version {
            get {
                return this.version;
            }
            set {
                this.version = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("ID")]
        [CategoryAttribute("General")]
        public virtual string ID {
            get {
                return this.id;
            }
            set {
                this.id = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Publisher")]
        [CategoryAttribute("General")]
        public virtual string Publisher {
            get {
                return this.publisher;
            }
            set {
                this.publisher = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("InternalName")]
        [CategoryAttribute("General")]
        public virtual string InternalName {
            get {
                return this.internalname;
            }
            set {
                this.internalname = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Language")]
        [CategoryAttribute("General")]
        public virtual string Language {
            get {
                return this.language;
            }
            set {
                this.language = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("DateCreated")]
        [CategoryAttribute("General")]
        public virtual string DateCreated {
            get {
                return this.datecreated;
            }
            set {
                this.datecreated = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("isDNS")]
        [CategoryAttribute("General")]
        public virtual string isDNS {
            get {
                return this.isdns;
            }
            set {
                this.isdns = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("isDHCP")]
        [CategoryAttribute("General")]
        public virtual string isDHCP {
            get {
                return this.isdhcp;
            }
            set {
                this.isdhcp = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("isLicensed")]
        [CategoryAttribute("General")]
        public virtual string isLicensed {
            get {
                return this.islicensed;
            }
            set {
                this.islicensed = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("LicenseNumber")]
        [CategoryAttribute("General")]
        public virtual string LicenseNumber {
            get {
                return this.licensenumber;
            }
            set {
                this.licensenumber = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("DatePurchased")]
        [CategoryAttribute("General")]
        public virtual string DatePurchased {
            get {
                return this.datepurchased;
            }
            set {
                this.datepurchased = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Copyright")]
        [CategoryAttribute("General")]
        public virtual string Copyright {
            get {
                return this.copyright;
            }
            set {
                this.copyright = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Configuration")]
        [CategoryAttribute("General")]
        public virtual LongText Configuration {
            get {
                return this.configuration;
            }
            set {
                this.configuration = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
