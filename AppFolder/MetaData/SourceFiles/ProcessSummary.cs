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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.ProcessSummarySort))]
    [Serializable()]
    public class ProcessSummary : MetaBuilder.Meta.MetaBase {
        
        public string summary;
        
        public ProcessSummary() {
        }
        
        [DescriptionAttribute("The summary of the process")]
        [CategoryAttribute("General")]
        public virtual string Summary {
            get {
                return this.summary;
            }
            set {
                this.summary = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Summary;
        }
    }
}
