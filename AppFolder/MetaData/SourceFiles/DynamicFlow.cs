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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.DynamicFlowSort))]
    [Serializable()]
    public class DynamicFlow : MetaBuilder.Meta.MetaBase {
        
        public string sequence;
        
        public string process;
        
        public string conditions;
        
        public string comments;
        
        public string @class;
        
        public string designmappingname;
        
        public string designmappingversion;
        
        public string designmappingid;
        
        public string transactionvolume;
        
        public string transactionfrequency;
        
        public string rateofvolumechange;
        
        public string transactionduration;
        
        public DynamicFlow() {
        }
        
        [DescriptionAttribute("Sequence")]
        [CategoryAttribute("Process")]
        public virtual string Sequence {
            get {
                return this.sequence;
            }
            set {
                this.sequence = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Description of process")]
        [CategoryAttribute("Process")]
        public virtual string Process {
            get {
                return this.process;
            }
            set {
                this.process = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Relevant conditions")]
        [CategoryAttribute("Process")]
        public virtual string Conditions {
            get {
                return this.conditions;
            }
            set {
                this.conditions = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Relevant comments")]
        [CategoryAttribute("Process")]
        public virtual string Comments {
            get {
                return this.comments;
            }
            set {
                this.comments = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Class")]
        [CategoryAttribute("Process")]
        public virtual string Class {
            get {
                return this.@class;
            }
            set {
                this.@class = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Design Mapping Name")]
        [CategoryAttribute("Design Mapping")]
        public virtual string DesignMappingName {
            get {
                return this.designmappingname;
            }
            set {
                this.designmappingname = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Design Mapping Version")]
        [CategoryAttribute("Design Mapping")]
        public virtual string DesignMappingVersion {
            get {
                return this.designmappingversion;
            }
            set {
                this.designmappingversion = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Design Mapping ID")]
        [CategoryAttribute("Design Mapping")]
        public virtual string DesignMappingID {
            get {
                return this.designmappingid;
            }
            set {
                this.designmappingid = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Transaction Volume")]
        [CategoryAttribute("Transaction")]
        public virtual string TransactionVolume {
            get {
                return this.transactionvolume;
            }
            set {
                this.transactionvolume = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Transaction Frequency")]
        [CategoryAttribute("Transaction")]
        public virtual string TransactionFrequency {
            get {
                return this.transactionfrequency;
            }
            set {
                this.transactionfrequency = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Rate of Volume Change")]
        [CategoryAttribute("Transaction")]
        public virtual string RateOfVolumeChange {
            get {
                return this.rateofvolumechange;
            }
            set {
                this.rateofvolumechange = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Transaction Duration")]
        [CategoryAttribute("Transaction")]
        public virtual string TransactionDuration {
            get {
                return this.transactionduration;
            }
            set {
                this.transactionduration = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Sequence + " " + Process;
        }
    }
}
