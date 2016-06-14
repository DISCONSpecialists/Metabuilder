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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.PeripheralSort))]
    [Serializable()]
    public class Peripheral : MetaBuilder.Meta.MetaBase {
        
        public string type;
        
        public string name;
        
        public string description;
        
        public string severityrating;
        
        public LongText configuration;
        
        public string networkaddress1;
        
        public string networkaddress2;
        
        public string networkaddress3;
        
        public string networkaddress4;
        
        public string networkaddress5;
        
        public string make;
        
        public string model;
        
        public string serialnumber;
        
        public string assetnumber;
        
        public string copy_ppm;
        
        public string print_ppm;
        
        public string iscolor;
        
        public string ismanaged;
        
        public string isnetwork;
        
        public string datepurchased;
        
        public string underwarranty;
        
        public string contract;
        
        public Peripheral() {
        }
        
        [DomainAttribute("PeripheralType")]
        [DescriptionAttribute("Type")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("PeripheralType")]
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
        
        [DescriptionAttribute("Network Address 1")]
        [CategoryAttribute("General")]
        public virtual string NetworkAddress1 {
            get {
                return this.networkaddress1;
            }
            set {
                this.networkaddress1 = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Network Address 2")]
        [CategoryAttribute("General")]
        public virtual string NetworkAddress2 {
            get {
                return this.networkaddress2;
            }
            set {
                this.networkaddress2 = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Network Address 3")]
        [CategoryAttribute("General")]
        public virtual string NetworkAddress3 {
            get {
                return this.networkaddress3;
            }
            set {
                this.networkaddress3 = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Network Address 4")]
        [CategoryAttribute("General")]
        public virtual string NetworkAddress4 {
            get {
                return this.networkaddress4;
            }
            set {
                this.networkaddress4 = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Network Address 5")]
        [CategoryAttribute("General")]
        public virtual string NetworkAddress5 {
            get {
                return this.networkaddress5;
            }
            set {
                this.networkaddress5 = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Make")]
        [CategoryAttribute("General")]
        public virtual string Make {
            get {
                return this.make;
            }
            set {
                this.make = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Model")]
        [CategoryAttribute("General")]
        public virtual string Model {
            get {
                return this.model;
            }
            set {
                this.model = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("SerialNumber")]
        [CategoryAttribute("General")]
        public virtual string SerialNumber {
            get {
                return this.serialnumber;
            }
            set {
                this.serialnumber = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("AssetNumber")]
        [CategoryAttribute("General")]
        public virtual string AssetNumber {
            get {
                return this.assetnumber;
            }
            set {
                this.assetnumber = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Copy PPM")]
        [CategoryAttribute("General")]
        public virtual string Copy_PPM {
            get {
                return this.copy_ppm;
            }
            set {
                this.copy_ppm = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Print PPM")]
        [CategoryAttribute("General")]
        public virtual string Print_PPM {
            get {
                return this.print_ppm;
            }
            set {
                this.print_ppm = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("isColor")]
        [CategoryAttribute("General")]
        public virtual string isColor {
            get {
                return this.iscolor;
            }
            set {
                this.iscolor = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("isMAnaged")]
        [CategoryAttribute("General")]
        public virtual string isManaged {
            get {
                return this.ismanaged;
            }
            set {
                this.ismanaged = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("isNetwork")]
        [CategoryAttribute("General")]
        public virtual string isNetwork {
            get {
                return this.isnetwork;
            }
            set {
                this.isnetwork = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Date Purchased")]
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
        
        [DescriptionAttribute("UnderWarranty")]
        [CategoryAttribute("General")]
        public virtual string UnderWarranty {
            get {
                return this.underwarranty;
            }
            set {
                this.underwarranty = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Contract")]
        [CategoryAttribute("General")]
        public virtual string Contract {
            get {
                return this.contract;
            }
            set {
                this.contract = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
