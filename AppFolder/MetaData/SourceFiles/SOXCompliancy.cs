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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.SOXCompliancySort))]
    [Serializable()]
    public class SOXCompliancy : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string articlereference;
        
        public SOXCompliancy() {
        }
        
        [DescriptionAttribute("Name of the compliancy requirement")]
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
        
        [DescriptionAttribute("Article Number of the Compliancy Regulation")]
        [CategoryAttribute("General")]
        public virtual string ArticleReference {
            get {
                return this.articlereference;
            }
            set {
                this.articlereference = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
