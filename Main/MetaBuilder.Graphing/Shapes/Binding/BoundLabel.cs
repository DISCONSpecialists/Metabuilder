using System;
using System.Drawing;
using System.Text;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Binding.Intellisense;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.Graphing.Shapes
{
    /// <summary>
    /// BoundLabel is a normal GoText that binds to a specific property. 
    /// </summary>
    public delegate void TextChangedEventHandler(object sender, string OldText, string NewText);

    [Serializable]
    public class BoundLabel : GoText, IIdentifiable, ISnappableShape //,IBehaviourShape
    {
        public int ddf = 0;

        //public override string ComputeEdit(string oldtext, string newtext)
        //{
        //    Core.Variables.spelling.Suggestions.Clear();
        //    Core.Variables.spelling.Dictionary.Dispose();// = null;
        //    Core.Variables.spelling.Dispose();

        //    return base.ComputeEdit(oldtext, newtext);
        //}

        public bool? originalEditable = true;

        public BoundLabel()
        {
            name = Guid.NewGuid().ToString();
            Alignment = MiddleLeft;
            //manager = new BaseShapeManager();
            //Clipping = true;
            StringTrimming = StringTrimming.Word;
            AutoResizes = true;
            ResizesRealtime = true;
        }

        #region IIdentifiable Implementation

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        public override GoObject SelectionObject
        {
            get
            {
                if (ParentNode is GraphNode)
                {
                    if (IsEditing())
                        return this;
                    if (Editable)
                        return base.SelectionObject;
                }

                return this;
            }
        }

        public override bool Selectable
        {
            get
            {
                if (ParentNode is GraphNode)
                {
                    return IsEditing();
                }
                return base.Selectable;
            }
            set { base.Selectable = value; }
        }

        public override GoControl CreateEditor(GoView view)
        {
            //if (SelectionObject is BoundLabel && (SelectionObject as BoundLabel).Name == "cls_id")
            //{
            //    return null;
            //}
            if (SelectionObject != null && SelectionObject.Editable)
            {
                if (EditorStyle == GoTextEditorStyle.TextBox)
                {
                    GoControl editor = new GoControl();
                    editor.ControlType = typeof(IntellisenseBox);

                    RectangleF r = Bounds;
                    r.Inflate(4, 4); // a bit bigger than the original GoText object
                    editor.Bounds = r;

                    return editor;
                }
                //#if DEBUG
                else if (EditorStyle == GoTextEditorStyle.ComboBox)
                {
                    GoControl editor = new GoControl();
                    editor.ControlType = typeof(CustomComboBox);

                    RectangleF r = Bounds;
                    r.Inflate(4, 4); // a bit bigger than the original GoText object
                    editor.Bounds = r;

                    return editor;
                }
                //#endif
                //GoControl baseEditor = base.CreateEditor(view);
                return base.CreateEditor(view);
            }

            return null;
        }

        public void SetEditable(bool value)
        {
            originalEditable = value;
        }

        public override bool OnSingleClick(GoInputEventArgs evt, GoView view)
        {
            SkipsUndoManager = true;
            if (Parent is ImageNode)
            {
                return false;
            }

            if (Parent is Nodes.Containers.MappingCell)
            {
                return false;
            }

            if (Parent is GraphNode)
            {
                GraphNode n = Parent as GraphNode;
                if (n.EditMode)
                    return base.OnSingleClick(evt, view);
                if (n.MetaObjectLocked)
                    return false;
            }
            if (!originalEditable.HasValue)
                originalEditable = Editable;
            else
                Editable = originalEditable.Value;
            if (Parent is GraphNode)
                Editable = false;

            if (Parent is CollapsingRecordNodeItem)
            {
                IMetaNode imnNode = Parent as IMetaNode;
                if (imnNode.MetaObject != null)
                    if (imnNode.MetaObject.State == MetaBuilder.BusinessLogic.VCStatusList.CheckedIn || imnNode.MetaObject.State == MetaBuilder.BusinessLogic.VCStatusList.CheckedOutRead || imnNode.MetaObject.State == MetaBuilder.BusinessLogic.VCStatusList.Locked || imnNode.MetaObject.State == MetaBuilder.BusinessLogic.VCStatusList.Obsolete)
                    {
                        Editable = false;
                        return false;
                    }
            }
            SkipsUndoManager = false;
            return base.OnSingleClick(evt, view);
        }

        public override bool OnDoubleClick(GoInputEventArgs evt, GoView view)
        {
            if (Parent is ImageNode)
            {
                if (this.Name == "cls_id")
                    return false;

                return true;
            }

            if (Parent is Nodes.Containers.MappingCell)
            {
                return false;
            }

            if (Document is NormalDiagram)
            {
                if (Parent is CollapsingRecordNodeItem)
                {
                    IMetaNode imnNode = Parent as IMetaNode;
                    if (imnNode.MetaObject != null)
                    {
                        if (imnNode.MetaObject.State == MetaBuilder.BusinessLogic.VCStatusList.CheckedIn || imnNode.MetaObject.State == MetaBuilder.BusinessLogic.VCStatusList.CheckedOutRead || imnNode.MetaObject.State == MetaBuilder.BusinessLogic.VCStatusList.Locked || imnNode.MetaObject.State == MetaBuilder.BusinessLogic.VCStatusList.Obsolete)
                        {
                            Core.Log.WriteLog(imnNode.ToString() + " cannot be edited");
                            Editable = false;
                            return false;
                        }
                    }
                }

                if (Parent is GraphNode)
                {
                    GraphNode n = Parent as GraphNode;
                    if (n.MetaObjectLocked)
                    {
                        //Core.Log.WriteLog("MetaObjectLocked - could be null or have invalid state");
                        return false;
                    }
                    // this is a hack. duh.
                    // check if the parent has anything bound to this label. if so, enable editing.
                    if (n.BindingInfo.Bindings.ContainsKey(Name))
                    {
                        Editable = true;
                        originalEditable = true;
                        n.HookupEvents();
                    }
                    if (n.EditMode)
                        return base.OnDoubleClick(evt, view);
                }
                if (originalEditable.HasValue)
                    Editable = originalEditable.Value;
                base.OnSingleClick(evt, view);
            }
            return base.OnDoubleClick(evt, view);
        }

        public bool IsEditing()
        {
            if (ParentNode.Document is Symbol)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Breaks any long text into smaller chunks with newlines in between
        /// preventing the tooltip from spanning across the screen
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public override string GetToolTip(GoView view)
        {
            string TextToProcess = Text;
            if (TextToProcess.Length > 50)
            {
                StringBuilder sbuilder = new StringBuilder();
                while (TextToProcess.Length > 0)
                {
                    if (TextToProcess.Length > 50)
                    {
                        sbuilder.Append(TextToProcess.Substring(0, 50)).Append(Environment.NewLine);
                        TextToProcess = TextToProcess.Substring(50);
                    }
                    else
                    {
                        sbuilder.Append(TextToProcess);
                        TextToProcess = "";
                    }
                }
                return sbuilder.ToString();
            }
            return Text;
            // return base.GetToolTip(view);
        }

        /*#region Behaviour Implementation
        private BaseShapeManager manager;
        public BaseShapeManager Manager
        {
            get
            {
                if (manager == null)
                    manager = new BaseShapeManager();
                return manager;
            }
            set
            {
                manager = value;
            }
        }
        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            Manager.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect, this);
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

        public override void Changed(int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            Manager.Changed(subhint, oldI, oldVal, oldRect, newI, newVal, newRect, this);
            base.Changed(subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }
        #endregion*/

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            env.Delayeds.Add(this);
            return base.CopyObject(env);
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            base.CopyObjectDelayed(env, newobj);
            BoundLabel lbl = newobj as BoundLabel;
            lbl.Name = Name.Substring(0, Name.Length);
        }

        public override void DoEndEdit(GoView view)
        {
            base.DoEndEdit(view);
            if (ParentNode is IMetaNode)
            {
                if ((ParentNode as IMetaNode).MetaObject != null)
                {
                    (ParentNode as IMetaNode).BindToMetaObjectProperties();
                }
            }
            else if (ParentNode is ShapeGroup)
            {
                if (Parent is IMetaNode)
                {
                    if ((Parent as IMetaNode).MetaObject != null)
                    {
                        (Parent as IMetaNode).BindToMetaObjectProperties();
                    }
                }
            }
            else
            {
                //ToString();
            }
        }

    }
}