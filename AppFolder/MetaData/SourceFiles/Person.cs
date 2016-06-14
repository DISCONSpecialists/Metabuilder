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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.PersonSort))]
    [Serializable()]
    public class Person : MetaBuilder.Meta.MetaBase {
        
        public string firstname;
        
        public string surname;
        
        public string idnumber;
        
        public string title;
        
        public Person() {
        }
        
        [DescriptionAttribute("The person\'s first name")]
        [CategoryAttribute("General")]
        public virtual string Firstname {
            get {
                return this.firstname;
            }
            set {
                this.firstname = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("The person\'s surname")]
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
        
        [DescriptionAttribute("The person\'s ID number")]
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
        
        [DescriptionAttribute("The person\'s title")]
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
        
        public override string ToString() {
return Firstname + " " + Surname;
        }
    }
}
