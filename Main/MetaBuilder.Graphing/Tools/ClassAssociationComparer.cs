using System.Collections.Generic;
using MetaBuilder.BusinessLogic;

namespace MetaBuilder.Graphing.Tools
{
    public class ClassAssociationComparer : IComparer<ClassAssociation>
    {
        #region Methods (1) 

        // Public Methods (1) 

        // Summary:
        //     Compares two objects and returns a value indicating whether one is less than,
        //     equal to, or greater than the other.
        //
        // Parameters:
        //   y:
        //     The second object to compare.
        //
        //   x:
        //     The first object to compare.
        //
        // Returns:
        //     Value Condition Less than zerox is less than y.Zerox equals y.Greater than
        //     zerox is greater than y.
        public int Compare(ClassAssociation x, ClassAssociation y)
        {
            return x.ChildClass.CompareTo(y.ChildClass);
        }

        #endregion Methods 
    }
}