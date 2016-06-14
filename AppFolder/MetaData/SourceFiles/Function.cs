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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.FunctionSort))]
    [Serializable()]
    public class Function : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string contextualindicator;
        
        public System.Int32? functioncriticality;
        
        public string managementability;
        
        public string infoavailability;
        
        public string legalaspects;
        
        public string technology;
        
        public string budget;
        
        public string energy;
        
        public string rawmaterial;
        
        public string skillavailability;
        
        public string manpower;
        
        public string efficiency;
        
        public string effectiveness;
        
        public Function() {
        }
        
        [DescriptionAttribute("Function Name")]
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
        
        [DomainAttribute("ContextualIndicator")]
        [DescriptionAttribute("Contextual Indicator")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("ContextualIndicator")]
        [ValueMember("PossibleValue")]
        [DisplayMember("Description")]
        public virtual string ContextualIndicator {
            get {
                return this.contextualindicator;
            }
            set {
                this.contextualindicator = value;
            }
        }
        
        [DescriptionAttribute("FunctionCriticality")]
        [CategoryAttribute("General")]
        public virtual System.Int32? FunctionCriticality {
            get {
                return this.functioncriticality;
            }
            set {
                this.functioncriticality = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Management Ability")]
        [CategoryAttribute("General")]
        public virtual string ManagementAbility {
            get {
                return this.managementability;
            }
            set {
                this.managementability = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("InformationAvailability")]
        [CategoryAttribute("General")]
        public virtual string InfoAvailability {
            get {
                return this.infoavailability;
            }
            set {
                this.infoavailability = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Legal Aspects")]
        [CategoryAttribute("General")]
        public virtual string LegalAspects {
            get {
                return this.legalaspects;
            }
            set {
                this.legalaspects = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Technology")]
        [CategoryAttribute("General")]
        public virtual string Technology {
            get {
                return this.technology;
            }
            set {
                this.technology = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Budget")]
        [CategoryAttribute("General")]
        public virtual string Budget {
            get {
                return this.budget;
            }
            set {
                this.budget = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Energy")]
        [CategoryAttribute("General")]
        public virtual string Energy {
            get {
                return this.energy;
            }
            set {
                this.energy = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Raw Material")]
        [CategoryAttribute("General")]
        public virtual string RawMaterial {
            get {
                return this.rawmaterial;
            }
            set {
                this.rawmaterial = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Skill Availability")]
        [CategoryAttribute("General")]
        public virtual string SkillAvailability {
            get {
                return this.skillavailability;
            }
            set {
                this.skillavailability = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Manpower")]
        [CategoryAttribute("General")]
        public virtual string Manpower {
            get {
                return this.manpower;
            }
            set {
                this.manpower = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Efficiency ")]
        [CategoryAttribute("General")]
        public virtual string Efficiency {
            get {
                return this.efficiency;
            }
            set {
                this.efficiency = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Effectiveness ")]
        [CategoryAttribute("General")]
        public virtual string Effectiveness {
            get {
                return this.effectiveness;
            }
            set {
                this.effectiveness = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
