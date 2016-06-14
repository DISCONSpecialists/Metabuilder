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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.FilledJobPositionSort))]
    [Serializable()]
    public class FilledJobPosition : MetaBuilder.Meta.MetaBase {
        
        public Person person;
        
        public System.DateTime date;
        
        public JobPosition jobposition;
        
        public FilledJobPosition() {
        }
        
        [DescriptionAttribute("Person in Job Position ")]
        [CategoryAttribute("General")]
        public virtual Person Person {
            get {
                return this.person;
            }
            set {
                this.person = value;
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
        
        [DescriptionAttribute("JobPosition")]
        [CategoryAttribute("General")]
        public virtual JobPosition JobPosition {
            get {
                return this.jobposition;
            }
            set {
                this.jobposition = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Person.ToString();
        }
    }
}
