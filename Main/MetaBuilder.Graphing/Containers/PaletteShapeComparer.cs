using System;
using System.Collections;
using System.Collections.Generic;

namespace MetaBuilder.Graphing.Containers
{
    [Serializable]
    public class PaletteShapeComparer : IComparer
    {
        #region Fields (1) 

        private List<object> objectStack;

        #endregion Fields 

        #region Constructors (1) 

        public PaletteShapeComparer()
        {
            objectStack = new List<object>();
        }

        #endregion Constructors 

        #region Properties (1) 

        public List<Object> ObjectStack
        {
            get { return objectStack; }
            set { objectStack = value; }
        }

        #endregion Properties 

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
        //     Value Condition Less than zero x is less than y. Zero x equals y. Greater
        //     than zero x is greater than y.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     Neither x nor y implements the System.IComparable interface.-or- x and y
        //     are of different types and neither one can handle comparisons with the other.
        public int Compare(object x, object y)
        {
            int indexX = -1;
            int indexY = -1;
            for (int i = 0; i < objectStack.Count; i++)
            {
                if (objectStack[i] == x)
                    indexX = i;
                if (objectStack[i] == y)
                    indexY = i;
            }
            return (indexX - indexY);
        }

        #endregion Methods 
    }
}