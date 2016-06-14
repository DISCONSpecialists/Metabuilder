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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.LocationSort))]
    [Serializable()]
    public class Location : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public string locationtype;
        
        public Location() {
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
        
        [DomainAttribute("LocationType")]
        [DescriptionAttribute("Location Type")]
        [CategoryAttribute("General")]
        [Editor(typeof(MetaBuilder.Meta.Editors.UniversalDropdownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [SourceCollection("LocationType")]
        [ValueMember("PossibleValue")]
        [DisplayMember("Description")]
        public virtual string LocationType {
            get {
                return this.locationtype;
            }
            set {
                this.locationtype = value;
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
