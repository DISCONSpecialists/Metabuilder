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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.DataStoreSort))]
    [Serializable()]
    public class DataStore : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public List<DataStoreType> @class;
        
        public string id;
        
        public DataStore() {
        }
        
        [DescriptionAttribute("The datastore\'s name")]
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
        
        [DescriptionAttribute("The type of datastore")]
        [CategoryAttribute("General")]
        public virtual List<DataStoreType> Class {
            get {
                return this.@class;
            }
            set {
                this.@class = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("The datastore\'s ID")]
        [CategoryAttribute("General")]
        public virtual string ID {
            get {
                return this.id;
            }
            set {
                this.id = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
