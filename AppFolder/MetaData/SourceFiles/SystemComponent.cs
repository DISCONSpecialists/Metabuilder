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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.SystemComponentSort))]
    [Serializable()]
    public class SystemComponent : MetaBuilder.Meta.MetaBase {
        
        public string type;
        
        public string name;
        
        public string description;
        
        public string severityrating;
        
        public LongText configuration;
        
        public string macaddress;
        
        public string staticip;
        
        public string networkaddress3;
        
        public string networkaddress4;
        
        public string networkaddress5;
        
        public string make;
        
        public string model;
        
        public string serialnumber;
        
        public string assetnumber;
        
        public string isdns;
        
        public string isdhcp;
        
        public string capacity;
        
        public string number_of_disks;
        
        public string cpu_type;
        
        public string cpu_speed;
        
        public string monitor;
        
        public string video_card;
        
        public string mem_total;
        
        public string datepurchased;
        
        public string underwarranty;
        
        public string domain;
        
        public SystemComponent() {
        }
        
        [DomainAttribute("SystemCompType")]
        [DescriptionAttribute("Type")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("SystemCompType")]
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
        
        [DescriptionAttribute("MAC Address")]
        [CategoryAttribute("General")]
        public virtual string MACAddress {
            get {
                return this.macaddress;
            }
            set {
                this.macaddress = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Static IP Address")]
        [CategoryAttribute("General")]
        public virtual string StaticIP {
            get {
                return this.staticip;
            }
            set {
                this.staticip = value;
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
        
        [DescriptionAttribute("Serial Number")]
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
        
        [DescriptionAttribute("Asset Number")]
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
        
        [DescriptionAttribute("Capacity")]
        [CategoryAttribute("General")]
        public virtual string Capacity {
            get {
                return this.capacity;
            }
            set {
                this.capacity = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Number of Disks")]
        [CategoryAttribute("General")]
        public virtual string Number_Of_Disks {
            get {
                return this.number_of_disks;
            }
            set {
                this.number_of_disks = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("CPU Type")]
        [CategoryAttribute("General")]
        public virtual string CPU_Type {
            get {
                return this.cpu_type;
            }
            set {
                this.cpu_type = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("CPU Speed")]
        [CategoryAttribute("General")]
        public virtual string CPU_Speed {
            get {
                return this.cpu_speed;
            }
            set {
                this.cpu_speed = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Monitor")]
        [CategoryAttribute("General")]
        public virtual string Monitor {
            get {
                return this.monitor;
            }
            set {
                this.monitor = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Video Card")]
        [CategoryAttribute("General")]
        public virtual string Video_Card {
            get {
                return this.video_card;
            }
            set {
                this.video_card = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Memory Total")]
        [CategoryAttribute("General")]
        public virtual string Mem_Total {
            get {
                return this.mem_total;
            }
            set {
                this.mem_total = value;
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
        
        [DescriptionAttribute("Under Warranty")]
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
        
        [DescriptionAttribute("Domain")]
        [CategoryAttribute("General")]
        public virtual string Domain {
            get {
                return this.domain;
            }
            set {
                this.domain = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
