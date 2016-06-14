// --------------------------------------------------------------------------
// Description : DisconComponents Toolkit library
// Author	   : Serge Weinstock
//
//	You are free to use, distribute or modify this code
//	as long as this header is not removed or modified.
// --------------------------------------------------------------------------
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;

namespace MetaBuilder.UIControls.GraphingUI.ActionMenu.Actions
{
    /// <summary>
    /// A collection that stores Action 
    /// </summary>
    [Editor(typeof (ActionCollectionEditor), typeof (UITypeEditor))]
    public class ActionCollection : CollectionBase
    {

		#region Fields (2) 

        private Action _null = new Action();
        private ActionList _owner;

		#endregion Fields 

		#region Constructors (3) 

        /// <summary>
        ///  Initializes a new instance of ActionCollection.
        /// </summary>
        public ActionCollection(ActionList owner)
        {
            Debug.Assert(owner != null);
            _owner = owner;
            _null._owner = _owner;
        }

        /// <summary>
        /// Initialises a new instance of ActionCollection based on another ActionCollection.
        /// </summary>
        /// <param name='value'>An ActionCollection from which the contents are copied</param>
        public ActionCollection(ActionCollection value)
        {
            AddRange(value);
        }

        /// <summary>
        /// Initialises a new instance of ActionCollection containing any array of 
        /// </summary>
        /// <param name='value'>An array of Actions with which to intialize the collection</param>
        public ActionCollection(Action[] value)
        {
            AddRange(value);
        }

		#endregion Constructors 

		#region Properties (3) 

        /// <summary>
        /// Returns a reference to the "null" action of this collection (needed in design mode)
        /// </summary>
        internal Action Null
        {
            get { return _null; }
        }

        /// <summary>
        /// Returns the ActionList which owns this ActionCollection
        /// </summary>
        public ActionList Parent
        {
            get { return _owner; }
        }

        /// <summary>
        /// Represents the entry at the specified index.
        /// </summary>
        /// <param name='index'>The zero-based index of the entry to locate in the collection.</param>
        /// <returns>The entry at the specified index of the collection.</returns>
        public Action this[int index]
        {
            get { return ((Action) (List[index])); }
            set { List[index] = value; }
        }

		#endregion Properties 

		#region Methods (14) 


		// Public Methods (9) 

        /// <summary>
        /// Adds a Action with the specified value to the ActionCollection.
        /// </summary>
        /// <param name='value'>The Action to add.</param>
        /// <returns>The index at which the new element was inserted.</returns>
        public int Add(Action value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Copies the elements of an array to the end of the ActionCollection.
        /// </summary>
        /// <param name='value'> An array of Actions containing the objects to add to the collection.</param>
        public void AddRange(Action[] value)
        {
            foreach (Action a in value)
            {
                Add(a);
            }
        }

        /// <summary>
        /// Adds the contents of another ActionCollection to the end of the collection.
        /// </summary>
        /// <param name='value'>An ActionCollection containing the objects to add to the collection.</param>
        public void AddRange(ActionCollection value)
        {
            foreach (Action a in value)
            {
                Add(a);
            }
        }

        /// <summary>
        /// Returns true if this ActionCollection contains the specified Action.
        /// </summary>
        /// <param name='value'>The Action to locate.</param>
        public bool Contains(Action value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Copies the ActionCollection values to a one-dimensional Array instance at the  specified index.
        /// </summary>
        /// <param name='array'>The one-dimensional Array that is the destination of the values copied from ActionCollection .</param>
        /// <param name='index'>The index in <paramref name='array'/> where copying begins.</param>
        public void CopyTo(Action[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>Returns an enumerator that can iterate through  the ActionCollection.</summary>
        public new ActionEnumerator GetEnumerator()
        {
            return new ActionEnumerator(this);
        }

        /// <summary>Returns the index of an Action in the ActionCollection.</summary>
        /// <param name='value'>The Action to locate.</param>
        /// <returns>The index of the Action of <paramref name='value'/> in the ActionCollection, if found; otherwise, -1.</returns>
        public int IndexOf(Action value)
        {
            return List.IndexOf(value);
        }

        /// <summary>Inserts a Action into the ActionCollection at the specified index.</summary>
        /// <param name='index'>The zero-based index where <paramref name='value'/> should be inserted.</param>
        /// <param name=' value'>The Action to insert.</param>
        public void Insert(int index, Action value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Removes a specific Action from the ActionCollection.
        /// </summary>
        /// <param name='value'>The Action to remove from the ActionCollection .</param>
        public void Remove(Action value)
        {
            List.Remove(value);
        }



		// Protected Methods (5) 

        protected override void OnClear()
        {
        }

        protected override void OnInsert(int index, object value)
        {
            if (value != null) ((Action) value)._owner = _owner;
        }

        protected override void OnRemove(int index, object value)
        {
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (oldValue != null) ((Action) oldValue)._owner = null;
            if (newValue != null) ((Action) newValue)._owner = _owner;
        }

        protected override void OnValidate(object value)
        {
        }


		#endregion Methods 

		#region Nested Classes (1) 


        /// <summary>
        /// An enumerator for an ActionCollection
        /// </summary>
        public class ActionEnumerator : object, IEnumerator
        {

		#region Fields (2) 

            private IEnumerator _baseEnumerator;
            private IEnumerable _temp;

		#endregion Fields 

		#region Constructors (1) 

            public ActionEnumerator(ActionCollection mappings)
            {
                _temp = ((IEnumerable) (mappings));
                _baseEnumerator = _temp.GetEnumerator();
            }

		#endregion Constructors 

		#region Properties (2) 

            public Action Current
            {
                get { return ((Action) (_baseEnumerator.Current)); }
            }

            object IEnumerator.Current
            {
                get { return _baseEnumerator.Current; }
            }

		#endregion Properties 

		#region Methods (4) 


		// Public Methods (2) 

            public bool MoveNext()
            {
                return _baseEnumerator.MoveNext();
            }

            public void Reset()
            {
                _baseEnumerator.Reset();
            }



		// Private Methods (2) 

            bool IEnumerator.MoveNext()
            {
                return _baseEnumerator.MoveNext();
            }

            void IEnumerator.Reset()
            {
                _baseEnumerator.Reset();
            }


		#endregion Methods 

        }
		#endregion Nested Classes 

    }
}