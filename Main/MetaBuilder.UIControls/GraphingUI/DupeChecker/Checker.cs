using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.DupeChecker
{
    delegate List<MetaBase> MyDelegate(ref MetaBase metaBase, ref List<MetaBase> foundObjectIDs, FindExistingObject.BackgroundSearchType searchType, string fileID);
    delegate void ObjectsFoundEventHandler(object sender, List<MetaBase> list);
    delegate void NoDuplicatesFoundEventHandler(object sender);
    class Checker
    {

        private GoView myView;
        public GoView MyView
        {
            get { return myView; }
            set { myView = value; }
        }

        #region Fields (5)

        bool aborted = false;
        MetaBase createdObject;
        FindExistingObject fexistingobject;
        private IMetaNode nodeThatIsBeingChecked;
        MyDelegate X;

        #endregion Fields

        #region Properties (2)

        public MetaBase CreatedObject
        {
            get { return createdObject; }
        }

        public IMetaNode NodeThatIsBeingChecked
        {
            get { return nodeThatIsBeingChecked; }
            set
            {
                nodeThatIsBeingChecked = value;
                FileUniqueID = "";
            }
        }

        private string fileUniqueID;
        public string FileUniqueID
        {
            get { return fileUniqueID; }
            set { fileUniqueID = value; }
        }

        #endregion Properties

        #region Delegates and Events (2)

        // Events (2) 

        public event ObjectsFoundEventHandler DuplicatesFound;

        public event NoDuplicatesFoundEventHandler NoDuplicatesFound;

        #endregion Delegates and Events

        #region Methods (6)

        // Public Methods (2) 

        public void Abort()
        {
            fexistingobject.Abort();
            aborted = true;
        }

        public void TestBackgroundFinder(MetaBase mbase)
        {
            createdObject = mbase;
            //Control.CheckForIllegalCrossThreadCalls = true;
            X = new MyDelegate(DoSomething);
            List<MetaBase> objects = new List<MetaBase>();
            AsyncCallback cb = new AsyncCallback(FinishedLooking);
            IAsyncResult ar = X.BeginInvoke(ref createdObject, ref objects, FindExistingObject.BackgroundSearchType.Across, FileUniqueID, cb, null);
        }

        // Protected Methods (2) 

        protected void OnDuplicatesFound(object sender, List<MetaBase> list)
        {
            if (DuplicatesFound != null)
                DuplicatesFound(sender, list);
        }

        protected void OnNoDuplicatesFound(object sender)
        {
            if (NoDuplicatesFound != null)
                NoDuplicatesFound(sender);
        }

        // Private Methods (2) 

        //My Async Method
        List<MetaBase> DoSomething(ref MetaBase metaBase, ref List<MetaBase> a, FindExistingObject.BackgroundSearchType searchType, string fileID)
        {
            try
            {
                fexistingobject = new FindExistingObject();
                fexistingobject.MetaBaseToFind = metaBase;
                fexistingobject.FindObject(searchType, fileID);
                a = fexistingobject.MatchedObjects;

                foreach (GoObject o in MyView.Document)
                {
                    if (o is IMetaNode)
                    {
                        IMetaNode imn = o as IMetaNode;
                        if (imn.MetaObject != metaBase && imn.MetaObject.Class == metaBase.Class && imn.MetaObject.pkid == 0 && !a.Contains(imn.MetaObject))
                        {
                            if (imn.MetaObject.ToString() == metaBase.ToString())
                            {
                                a.Add(imn.MetaObject);
                            }
                        }
                    }
                }
            }
            catch (Exception x)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.Message = x.ToString();
                logEntry.Message += x.StackTrace;
                Logger.Write(logEntry);
                a = new List<MetaBase>();
            }
            return a;
        }

        //Mycallback method when finished running DoSomehting
        void FinishedLooking(IAsyncResult ar)
        {
            if (!aborted)
            {
                List<MetaBase> a = new List<MetaBase>();
                //Get the delegate
                MyDelegate X = (MyDelegate)((AsyncResult)ar).AsyncDelegate;

                //get results
                X.EndInvoke(ref createdObject, ref a, ar);
                //// Console.WriteLine(a);
                if (a.Count > 0)
                {
                    OnDuplicatesFound(this, a);
                }
                else
                {
                    OnNoDuplicatesFound(this);
                }
            }
        }

        #endregion Methods

    }
    public enum BackgroundSearchType
    {
        Across, Current
    }
}