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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.MechanismToDeriveMissionStatementSort))]
    [Serializable()]
    public class MechanismToDeriveMissionStatement : MetaBuilder.Meta.MetaBase {
        
        public string mechanism;
        
        public MechanismToDeriveMissionStatement() {
        }
        
        [DescriptionAttribute("Mechanisms to derive mission statement")]
        [CategoryAttribute("General")]
        public virtual string Mechanism {
            get {
                return this.mechanism;
            }
            set {
                this.mechanism = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return Mechanism.ToString();
        }
    }
}
