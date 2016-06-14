using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.Meta
{
    public class Tuple
    {
        private object first;
        public object First { get { return first; } private set { first = value; } }
        private object second;
        public object Second { get { return second; } private set { second = value; } }
        public Tuple(object f, object s)
        {
            First = f;
            Second = s;
        }
    }

}
