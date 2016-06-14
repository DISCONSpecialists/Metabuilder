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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ScenarioSort))]
    [Serializable()]
    public class Scenario : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string acc_prob_of_real;
        
        public string start_date;
        
        public string end_date;
        
        public Scenario() {
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
        
        [DescriptionAttribute("Accumulated Probability of Realization")]
        [CategoryAttribute("General")]
        public virtual string Acc_Prob_of_Real {
            get {
                return this.acc_prob_of_real;
            }
            set {
                this.acc_prob_of_real = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Start Date")]
        [CategoryAttribute("General")]
        public virtual string Start_Date {
            get {
                return this.start_date;
            }
            set {
                this.start_date = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("End Date")]
        [CategoryAttribute("General")]
        public virtual string End_Date {
            get {
                return this.end_date;
            }
            set {
                this.end_date = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
