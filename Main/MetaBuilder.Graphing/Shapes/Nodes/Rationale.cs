using System;
using System.Drawing;
using MetaBuilder.Meta;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;
using System.Collections.Generic;

namespace MetaBuilder.Graphing.Shapes.Nodes
{
    [Serializable]
    public class Rationale : ResizableBalloonComment, IMetaNode, IShallowCopyable
    {
        private bool parentIsILinkedContainer;
        public bool ParentIsILinkedContainer
        {
            get { return parentIsILinkedContainer; }
            set { parentIsILinkedContainer = value; }
        }

        private BindingInfo bindingInfo;
        [NonSerialized]
        private EventHandler contentsChanged;

        [NonSerialized]
        private bool copyAsShadow;
        private string fieldToUse;
        private MetaBase metaObject;

        public override bool Shadowed
        {
            get
            {
                return true;
            }
            set
            {
                base.Shadowed = true;
            }
        }

        public Rationale()
        {
            Text = "Rationale";
            bindingInfo = new BindingInfo();
            bindingInfo.BindingClass = "Rationale";
            fieldToUse = "UniqueRef";
            MyGUID = Guid.NewGuid();
        }
        public Rationale(bool text)
        {
            if (text)
                Text = "Rationale";
            bindingInfo = new BindingInfo();
            bindingInfo.BindingClass = "Rationale";
            fieldToUse = "UniqueRef";
            MyGUID = Guid.NewGuid();
        }
        [NonSerialized]
        private MetaBase copiedFrom;
        public MetaBase CopiedFrom
        {
            get { return copiedFrom; }
            set { copiedFrom = value; }
        }
        public bool CopyAsShadow
        {
            get { return copyAsShadow; }
            set { copyAsShadow = value; }
        }

        #region IMetaNode Members

        public EventHandler ContentsChanged
        {
            get { return contentsChanged; }
            set { contentsChanged = value; }
        }

        public void OnContentsChanged(object sender, EventArgs e)
        {
            BindToMetaObjectProperties();
            if (contentsChanged != null)
                contentsChanged(sender, e);
        }

        public BindingInfo BindingInfo
        {
            get { return bindingInfo; }
            set { bindingInfo = value; }
        }

        public bool RequiresAttention
        {
            get { return false; }
            set { }
        }

        public b.TList<b.ObjectAssociation> SaveToDatabase(object o, EventArgs e)
        {
            if (MetaObject != null)
                MetaObject.Save(Guid.NewGuid());
            if (this.Anchor != null && Core.Variables.Instance.SaveOnCreate)
            {
                if (this.Anchor is IMetaNode)
                {
                    (this.Anchor as IMetaNode).SaveToDatabase(this, e);
                    if (LinkController.GetAssociation(MetaObject, (this.Anchor as IMetaNode).MetaObject, LinkAssociationType.Mapping) == null)
                        LinkController.SaveAssociation(MetaObject, (this.Anchor as IMetaNode).MetaObject, LinkAssociationType.Mapping, Core.Variables.Instance.ClientProvider);
                }
                else if (this.Anchor is QLink)
                {
                    try
                    {
                        MetaBuilder.BusinessLogic.Artifact art = new MetaBuilder.BusinessLogic.Artifact();
                        art.ArtifactObjectID = MetaObject.pkid;
                        art.ArtefactMachine = MetaObject.MachineName;
                        art.ObjectID = (this.Anchor as QLink).DBAssociation.ObjectID;
                        art.ChildObjectID = (this.Anchor as QLink).DBAssociation.ChildObjectID;
                        art.ObjectMachine = (this.Anchor as QLink).DBAssociation.ObjectMachine;
                        art.ChildObjectMachine = (this.Anchor as QLink).DBAssociation.ChildObjectMachine;
                        art.CAid = (this.Anchor as QLink).DBAssociation.CAid;
                        //if saving artifact fails then association is null/exists.

                        //if it exists?
                        MetaBuilder.DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ArtifactProvider.Save(art);
                    }
                    catch (Exception ex)
                    {
                        Core.Log.WriteLog(ex.ToString());
                    }
                }
                else
                {
                    this.Anchor.ToString();
                }
            }

            //List<GoObject> remove = new List<GoObject>();
            //foreach (GoObject or in this)
            //    if (or is IndicatorLabel)
            //        if ((or as IndicatorLabel).TextColor == Color.Red)
            //        {
            //            remove.Add(or);
            //        }
            //foreach (GoObject or in remove)
            //    or.Remove();
            return null;
        }

        public void LoadMetaObject(int pkid, string machine)
        {
            MetaObject = Loader.GetByID("Rationale", pkid, machine);
        }

        public bool HasBindingInfo
        {
            get { return true; }
        }

        private bool hooked;
        public void HookupEvents()
        {
            //if (hooked)
            //    return;
            if (MetaObject != null)
            {
                try
                {
                    MetaObject.Changed -= FireMetaObjectChanged;
                }
                catch
                {
                }
                MetaObject.Changed += FireMetaObjectChanged;
                hooked = true;
            }
            if (Label != null)
            {
                List<GoObject> removeThese = new List<GoObject>();
                System.Collections.IEnumerator enumerator = Label.Observers.GetEnumerator();
                while (enumerator.MoveNext())// (object o in Label.Observers.GetEnumerator())
                    if (enumerator.Current is Rationale)
                        removeThese.Add(enumerator.Current as GoObject);

                foreach (GoObject remove in removeThese)
                    Label.RemoveObserver(remove);
                Label.AddObserver(this);
            }
            else
            {
                //when can the label ever be null?
                this.ToString();
            }
        }

        public MetaBase MetaObject
        {
            get { return metaObject; }
            set { metaObject = value; }
        }

        public void FireMetaObjectChanged(object sender, EventArgs e)
        {
            OnContentsChanged(sender, e);

            if (Core.Variables.Instance.SaveOnCreate)
            {
                SaveToDatabase(sender, e);
            }

        }

        public void CreateMetaObject(object o, EventArgs e)
        {
            MetaObject = Loader.CreateInstance("Rationale");
            MetaObject.Set("AuthorName", Environment.UserName);
            HookupEvents();
        }

        public bool DONOTCHANGETHEMETABASETOBEEQUALETOTEXT;
        bool binding = false;
        public void BindToMetaObjectProperties()
        {
            binding = true;
            try
            {
                if (MetaObject == null)
                    CreateMetaObject(this, EventArgs.Empty);
                if (Label != null)
                    if (MetaObject != null)
                        if (MetaObject.ToString() != null)
                            Label.Text = MetaObject.ToString();
            }
            catch
            {
                Label.Text = "";
            }
            binding = false;
        }
        public void BindMetaObjectImage()
        {
        }
        #endregion

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            env.Delayeds.Add(this);
            GoObject retval = base.CopyObject(env);

            Rationale node = retval as Rationale;
            node.CopiedFrom = this.MetaObject;
            if (CopyAsShadow)
            {
                node.MetaObject = MetaObject;
                node.HookupEvents();
                return node;
            }
            return retval;
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            base.CopyObjectDelayed(env, newobj);
            Rationale node = newobj as Rationale;
            if (node.MetaObject != null)
            {
                node.MetaObject.Changed -= FireMetaObjectChanged;
                node.MetaObject = null;
            }

            base.CopyObjectDelayed(env, node);
            if (bindingInfo != null)
                node.BindingInfo = BindingInfo.Copy();

            //if (MetaObject == null)
            //{
            //    CreateMetaObject(this, null);
            //}

            if (CopyAsShadow)
            {
                node.MetaObject = MetaObject;
            }
            else
            {
                node.CreateMetaObject(null, EventArgs.Empty);
                if (node.MetaObject != null)
                    MetaObject.CopyPropertiesToObject(node.MetaObject);
            }

            node.HookupEvents();
            HookupEvents();
        }

        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            if (binding)
                return;
            if (DONOTCHANGETHEMETABASETOBEEQUALETOTEXT)
                return;
            if (observed == Label)
            {
                if (!(Initializing))
                {
                    Initializing = true;
                    if (MetaObject != null && subhint == 1501)
                    {
                        //if (Label.Text != mbase.Get("UniqueRef")) //This line causes changing ref to override value because value is the field to use when text changes
                        MetaObject.Set(fieldToUse, Label.Text);
                    }
                    Initializing = false;
                }
            }
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

        #region IIShallowCopyable Implementation

        public GoObject CopyAsShallow()
        {
            MetaBase mo = MetaObject;
            Rationale node = Copy() as Rationale;
            node.MetaObject = mo;
            return node;
        }

        #endregion

        #region IIdentifiable Implementation

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

    }
}