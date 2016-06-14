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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.JobPositionSort))]
    [Serializable()]
    public class JobPosition : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public int totalrequired;
        
        public int totaloccupied;
        
        public int totalavailable;
        
        public System.DateTime timestamp;
        
        public JobPosition() {
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
        
        [DescriptionAttribute("Total Required")]
        [CategoryAttribute("General")]
        public virtual int TotalRequired {
            get {
                return this.totalrequired;
            }
            set {
                this.totalrequired = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Total Occupied")]
        [CategoryAttribute("General")]
        public virtual int TotalOccupied {
            get {
                return this.totaloccupied;
            }
            set {
                this.totaloccupied = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Total Available")]
        [CategoryAttribute("General")]
        public virtual int TotalAvailable {
            get {
                return this.totalavailable;
            }
            set {
                this.totalavailable = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Date and Time of values entered")]
        [CategoryAttribute("General")]
        public virtual System.DateTime TimeStamp {
            get {
                return this.timestamp;
            }
            set {
                this.timestamp = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
