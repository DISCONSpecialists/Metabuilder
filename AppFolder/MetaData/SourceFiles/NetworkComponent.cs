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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.NetworkComponentSort))]
    [Serializable()]
    public class NetworkComponent : MetaBuilder.Meta.MetaBase {
        
        public string type;
        
        public string name;
        
        public string description;
        
        public string severityrating;
        
        public string configuration;
        
        public string macaddress;
        
        public string networkaddress2;
        
        public string networkaddress3;
        
        public string networkaddress4;
        
        public string networkaddress5;
        
        public string make;
        
        public string model;
        
        public string serialnumber;
        
        public string assetnumber;
        
        public string connectionspeed;
        
        public string number_of_ports;
        
        public string number_of_ports_available;
        
        public string range;
        
        public string isdns;
        
        public string isdhcp;
        
        public string ismanaged;
        
        public string mem_total;
        
        public string datepurchased;
        
        public string underwarranty;
        
        public NetworkComponent() {
        }
        
        [DomainAttribute("NetworkCompType")]
        [DescriptionAttribute("Type")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("NetworkCompType")]
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
        public virtual string Configuration {
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
        public virtual string MacAddress {
            get {
                return this.macaddress;
            }
            set {
                this.macaddress = value;
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
        
        [DescriptionAttribute("Connection Speed")]
        [CategoryAttribute("General")]
        public virtual string ConnectionSpeed {
            get {
                return this.connectionspeed;
            }
            set {
                this.connectionspeed = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Number of Ports")]
        [CategoryAttribute("General")]
        public virtual string Number_of_Ports {
            get {
                return this.number_of_ports;
            }
            set {
                this.number_of_ports = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Number of Ports Available")]
        [CategoryAttribute("General")]
        public virtual string Number_of_Ports_Available {
            get {
                return this.number_of_ports_available;
            }
            set {
                this.number_of_ports_available = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Range")]
        [CategoryAttribute("General")]
        public virtual string Range {
            get {
                return this.range;
            }
            set {
                this.range = value;
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
        
        [DescriptionAttribute("isManaged")]
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
        
        public override string ToString() {
return Name;
        }
    }
}
