using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using MetaBuilder.Meta;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{

    public class ValidatedItem
    {
        public string Name
        {
            get { return MyNode.ToString(); }
        }
        private ADDNode myNode;
        public ADDNode MyNode
        {
            get { return myNode; }
            set { myNode = value; }
        }

        private ValidationResult result;
        public ValidationResult Result
        {
            get { return result; }
            set { result = value; }
        }

        private GoObject myGoObject;
        public GoObject MyGoObject
        {
            get { return myGoObject; }
            set { myGoObject = value; }
        }

    }
    public abstract class BaseRule
    {
        #region Properties
        private ExecutesOnEnum executesOn;
        public ExecutesOnEnum ExecutesOn
        {
            get { return executesOn; }
            set { executesOn = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<ValidatedItem> OKItems = new List<ValidatedItem>();
        public List<ValidatedItem> ErrorItems = new List<ValidatedItem>();
        public List<ValidatedItem> AllItems;

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Value
        {
            get { return OverallValidationResult.ToString(); }
        }
        public ValidationResult OverallValidationResult
        {
            get
            {

                ValidationResult res = ValidationResult.None;
                foreach (ValidatedItem item in AllItems)
                {
                    if (item.Result == ValidationResult.Error)
                    {
                        return ValidationResult.Error;
                    }

                    if (item.Result == ValidationResult.Warning && res == ValidationResult.None)
                    {
                        return ValidationResult.Warning;
                    }
                }
                return ValidationResult.OK;

            }

        }
        #endregion

        #region Constructor
        public BaseRule(string name, ExecutesOnEnum execsOn)
        {
            this.Name = name;
            this.ExecutesOn = execsOn;
            AllItems = new List<ValidatedItem>();
        }
        #endregion

        #region Implementation
        public abstract void Apply();
        #endregion


        public override string ToString()
        {
            return Name + ": " + OverallValidationResult.ToString();
        }

    }

    /* Synthesis Rules 
     * 
     * XOR
     * 1. Has 1 subset link out of it (no more) - ERROR
     * 2. Has 1 selector attribute on subset link (no more) - ERROR
     * 3. Has Mutually Exclusive Entities - ERROR
     * 4. Only 1 MEE should give warning - WARNING
     * 
     * ENTITY
     * 1. Object or Event - WARNING
     * 2. O/E dictates subset types - ERROR
     * 3. Must always have KA set - ERROR
     */


    public enum ValidationResult
    {
        None, OK, Warning, Error
    }

    public enum ExecutesOnEnum
    {
        Entity, SubSet, SelectorAttribute, EntityAndSubSet, SubSetLink, All
    }

}
