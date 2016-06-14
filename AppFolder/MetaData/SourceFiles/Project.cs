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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ProjectSort))]
    [Serializable()]
    public class Project : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public System.DateTime startdate;
        
        public System.DateTime enddate;
        
        public string description;
        
        public string projectbrief;
        
        public Person mandatedby;
        
        public System.DateTime mandatedate;
        
        public string projectwillingness;
        
        public string projectmethodavailable;
        
        public string businesshasunderstandingofproblem;
        
        public string businesshasunderstandingofsolution;
        
        public Project() {
        }
        
        [DescriptionAttribute("Project Name")]
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
        
        [DescriptionAttribute("The project\'s start date")]
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
        
        [DescriptionAttribute("The project\'s end date")]
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
        
        [DescriptionAttribute("Description of the project")]
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
        
        [DescriptionAttribute("Project Brief")]
        [CategoryAttribute("General")]
        public virtual string ProjectBrief {
            get {
                return this.projectbrief;
            }
            set {
                this.projectbrief = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("By whom was the project mandated?")]
        [CategoryAttribute("General")]
        public virtual Person MandatedBy {
            get {
                return this.mandatedby;
            }
            set {
                this.mandatedby = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("The date that the project was mandated")]
        [CategoryAttribute("General")]
        public virtual System.DateTime MandateDate {
            get {
                return this.mandatedate;
            }
            set {
                this.mandatedate = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("High, Intermediate, Low")]
        [CategoryAttribute("General")]
        public virtual string ProjectWillingness {
            get {
                return this.projectwillingness;
            }
            set {
                this.projectwillingness = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("High, Intermediate, Low")]
        [CategoryAttribute("General")]
        public virtual string ProjectMethodAvailable {
            get {
                return this.projectmethodavailable;
            }
            set {
                this.projectmethodavailable = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("High, Intermediate, Low")]
        [CategoryAttribute("General")]
        public virtual string BusinessHasUnderstandingOfProblem {
            get {
                return this.businesshasunderstandingofproblem;
            }
            set {
                this.businesshasunderstandingofproblem = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("High, Intermediate, Low")]
        [CategoryAttribute("General")]
        public virtual string BusinessHasUnderstandingOfSolution {
            get {
                return this.businesshasunderstandingofsolution;
            }
            set {
                this.businesshasunderstandingofsolution = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
