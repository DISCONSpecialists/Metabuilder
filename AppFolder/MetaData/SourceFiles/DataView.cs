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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.DataViewSort))]
    [Serializable()]
    public class DataView : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string dataviewtype;
        
        public DataView() {
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
        
        [DomainAttribute("DataViewType")]
        [DescriptionAttribute("Data View Type")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("DataViewType")]
        [ValueMember("PossibleValue")]
        [DisplayMember("Description")]
        public virtual string DataViewType {
            get {
                return this.dataviewtype;
            }
            set {
                this.dataviewtype = value;
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
