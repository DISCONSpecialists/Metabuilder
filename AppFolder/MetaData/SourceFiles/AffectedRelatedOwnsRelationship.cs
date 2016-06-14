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
    [TypeConverter(typeof(MetaBuilder.Meta.Generated.AffectedRelatedOwnsRelationshipSort))]
    [Serializable()]
    public class AffectedRelatedOwnsRelationship : MetaBuilder.Meta.MetaBase {
        
        public string relationshiptype;
        
        public AffectedRelatedOwnsRelationship() {
        }
        
        [DescriptionAttribute("None")]
        [CategoryAttribute("General")]
        public virtual string RelationshipType {
            get {
                return this.relationshiptype;
            }
            set {
                this.relationshiptype = value;
                this.PropertyChanged();
            }
        }
        
        public override string ToString() {
return RelationshipType;
        }
    }
}
