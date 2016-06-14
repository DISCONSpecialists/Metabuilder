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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.DeliverableStatusSort))]
    [Serializable()]
    public class DeliverableStatus : MetaBuilder.Meta.MetaBase {
        
        public Deliverable deliverable;
        
        public System.DateTime date;
        
        public int percentagecomplete;
        
        public DeliverableStatus() {
        }
        
        [DescriptionAttribute("Deliverable")]
        [CategoryAttribute("General")]
        public virtual Deliverable Deliverable {
            get {
                return this.deliverable;
            }
            set {
                this.deliverable = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Date")]
        [CategoryAttribute("General")]
        public virtual System.DateTime Date {
            get {
                return this.date;
            }
            set {
                this.date = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Percentage Complete (Integer)")]
        [CategoryAttribute("General")]
        public virtual int PercentageComplete {
            get {
                return this.percentagecomplete;
            }
            set {
                this.percentagecomplete = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Deliverable.ToString() + " " + Date.ToShortDateString();
        }
    }
}
