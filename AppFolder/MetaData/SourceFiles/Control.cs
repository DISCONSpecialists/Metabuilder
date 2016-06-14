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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ControlSort))]
    [Serializable()]
    public class Control : MetaBuilder.Meta.MetaBase {
        
        public string controlname;
        
        public string controltype;
        
        public string frequency;
        
        public bool isexpected;
        
        public string iscurrent;
        
        public System.DateTime startdate;
        
        public System.DateTime enddate;
        
        public Control() {
        }
        
        [DescriptionAttribute("The Control\'s Name")]
        [CategoryAttribute("General")]
        public virtual string ControlName {
            get {
                return this.controlname;
            }
            set {
                this.controlname = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Please specify: Complementary / Application / Manual / Compensating / IT General " +
            "/ Redundant")]
        [CategoryAttribute("General")]
        public virtual string ControlType {
            get {
                return this.controltype;
            }
            set {
                this.controltype = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Control Frequency")]
        [CategoryAttribute("General")]
        public virtual string Frequency {
            get {
                return this.frequency;
            }
            set {
                this.frequency = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Is this an expected control?")]
        [CategoryAttribute("General")]
        public virtual bool IsExpected {
            get {
                return this.isexpected;
            }
            set {
                this.isexpected = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Is this a current control?")]
        [CategoryAttribute("General")]
        public virtual string IsCurrent {
            get {
                return this.iscurrent;
            }
            set {
                this.iscurrent = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("The start date of this control")]
        [CategoryAttribute("General")]
        public virtual System.DateTime StartDate {
            get {
                return this.startdate;
            }
            set {
                this.startdate = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("The end date of this control")]
        [CategoryAttribute("General")]
        public virtual System.DateTime EndDate {
            get {
                return this.enddate;
            }
            set {
                this.enddate = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return ControlName;
        }
    }
}
