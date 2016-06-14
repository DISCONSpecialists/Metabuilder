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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.DataTableSort))]
    [Serializable()]
    public class DataTable : MetaBuilder.Meta.MetaBase {
        
        public string name;
        
        public int initialpopulation;
        
        public string growthrateovertime;
        
        public string recordsize;
        
        public DataTable() {
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
        
        [DescriptionAttribute("InitialPopulation")]
        [CategoryAttribute("General")]
        public virtual int InitialPopulation {
            get {
                return this.initialpopulation;
            }
            set {
                this.initialpopulation = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("Growth Rate over Time")]
        [CategoryAttribute("General")]
        public virtual string GrowthRateOverTime {
            get {
                return this.growthrateovertime;
            }
            set {
                this.growthrateovertime = value;
                this.PropertyChanged();
            }
        }
        
        [DescriptionAttribute("The Size in Bytes of one record")]
        [CategoryAttribute("General")]
        public virtual string RecordSize {
            get {
                return this.recordsize;
            }
            set {
                this.recordsize = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Name;
        }
    }
}
