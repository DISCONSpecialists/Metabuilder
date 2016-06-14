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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.EmployeeSort))]
    [Serializable()]
    public class Employee : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string surname;
        
        public string title;
        
        public string employeenumber;
        
        public string idnumber;
        
        public string email;
        
        public string telephone;
        
        public Employee() {
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
        
        [DescriptionAttribute("Surname")]
        [CategoryAttribute("General")]
        public virtual string Surname {
            get {
                return this.surname;
            }
            set {
                this.surname = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Title")]
        [CategoryAttribute("General")]
        public virtual string Title {
            get {
                return this.title;
            }
            set {
                this.title = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Employee Number")]
        [CategoryAttribute("General")]
        public virtual string EmployeeNumber {
            get {
                return this.employeenumber;
            }
            set {
                this.employeenumber = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("ID Number")]
        [CategoryAttribute("General")]
        public virtual string IDNumber {
            get {
                return this.idnumber;
            }
            set {
                this.idnumber = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Email")]
        [CategoryAttribute("General")]
        public virtual string Email {
            get {
                return this.email;
            }
            set {
                this.email = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Telephone")]
        [CategoryAttribute("General")]
        public virtual string Telephone {
            get {
                return this.telephone;
            }
            set {
                this.telephone = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
