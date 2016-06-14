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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ProjectStatusSort))]
    [Serializable()]
    public class ProjectStatus : MetaBuilder.Meta.MetaBase {
        
        public Project project;
        
        public double percentagecomplete;
        
        public Person projectowner;
        
        public Person projectmanager;
        
        public System.DateTime date;
        
        public ProjectStatus() {
        }
        
        [DescriptionAttribute("Project")]
        [CategoryAttribute("General")]
        public virtual Project Project {
            get {
                return this.project;
            }
            set {
                this.project = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Percentage Complete")]
        [CategoryAttribute("General")]
        public virtual double PercentageComplete {
            get {
                return this.percentagecomplete;
            }
            set {
                this.percentagecomplete = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Project Owner")]
        [CategoryAttribute("General")]
        public virtual Person ProjectOwner {
            get {
                return this.projectowner;
            }
            set {
                this.projectowner = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Project Manager")]
        [CategoryAttribute("General")]
        public virtual Person ProjectManager {
            get {
                return this.projectmanager;
            }
            set {
                this.projectmanager = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Date of Status")]
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
        
        public override string ToString() {
return Project.ToString() + " - " + Date.ToShortDateString() ;
        }
    }
}
