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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.DatedResponsibilitySort))]
    [Serializable()]
    public class DatedResponsibility : MetaBuilder.Meta.MetaBase {
        
        public Responsibility assignedresponsibility;
        
        public System.DateTime datecreated;
        
        public DatedResponsibility() {
        }
        
        [DescriptionAttribute("AR")]
        [CategoryAttribute("General")]
        public virtual Responsibility AssignedResponsibility {
            get {
                return this.assignedresponsibility;
            }
            set {
                this.assignedresponsibility = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("DT")]
        [CategoryAttribute("General")]
        public virtual System.DateTime DateCreated {
            get {
                return this.datecreated;
            }
            set {
                this.datecreated = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return (AssignedResponsibility!=null)?AssignedResponsibility.ToString() + " " + DateCreated.ToShortDateString():"Dated Responsibility";
        }
    }
}
