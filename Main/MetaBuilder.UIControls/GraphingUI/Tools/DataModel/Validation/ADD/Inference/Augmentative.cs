using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;
using System.Drawing;
using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD.Inference
{
    internal class Augmentative : BaseInferenceRule
    {
        public Augmentative(string name, Engine2015 engine)
            : base(name, engine)
        {
            Description = "Augmentative";
        }
        #region Implementation of Base Method
        /// <summary>
        /// given x-->y
        /// and (x,z) and (y,w)
        /// if (z=w)
        /// then x-->y is aug (0)
        /// </summary>
        public override void Apply()
        {
            foreach (GoObject obj in Engine.Diagram)
            {
                if (!(obj is CollapsibleNode))
                    continue;
                CollapsibleNode FromNode = obj as CollapsibleNode;
                if (!(FromNode.MetaObject.Class.Contains("Entity")))
                    continue;
                if (CountKeyAttributes(FromNode) <= 1)
                    continue;

                foreach (GoObject otherObj in Engine.Diagram)
                {
                    if (obj == otherObj)
                        continue;

                    if (otherObj is CollapsibleNode)
                    {
                        CollapsibleNode ToNode = otherObj as CollapsibleNode;
                        if (!(ToNode.MetaObject.Class.Contains("Entity")))
                            continue;

                        if (CountKeyAttributes(ToNode) <= 1)
                            continue;

                        QLink existingLink = GetLink(FromNode, ToNode, LinkAssociationType.Dependency) as QLink;
                        QLink oppositeLink = GetLink(ToNode, FromNode, LinkAssociationType.Dependency) as QLink;

                        if (IsAugment(FromNode, ToNode) && !Engine.ErrorLinks.Contains(existingLink))
                        {
                            ValidationItem i = new ValidationItem();
                            i.ValidationType = "Augmentation";

                            if (existingLink == null)
                            {
                                i.MyGoObject = QLink.CreateLink(FromNode, ToNode, (int)LinkAssociationType.Dependency, -1);
                                if (Engine.Options.IndicateAugmentive)
                                {
                                    Engine.Diagram.Add(i.MyGoObject);
                                }
                                i.AddObject = true;

                                (i.MyGoObject as QLink).InferenceType = InferenceType.Augmentation;
                                (i.MyGoObject as QLink).AutomatedAddition = true;
                                i.Result = ValidationResult.Warning;
                            }
                            else
                            {
                                //existing link is reflexive or error?
                                if (existingLink.InferenceType != InferenceType.Reflexivity)
                                {
                                    if (existingLink.InferenceType == InferenceType.Error || oppositeLink != null)
                                    {
                                        ValidationItem iO = new ValidationItem();
                                        iO.ValidationType = "Augmentation";
                                        iO.MyGoObject = oppositeLink;

                                        (iO.MyGoObject as QLink).InferenceType = InferenceType.Error;
                                        iO.AddObject = false;
                                        iO.Result = ValidationResult.Error;
                                        iO.AdditionalInformation = "Invalid link";
                                        AllItems.Add(iO);

                                        Engine.ErrorLinks.Add(oppositeLink);
                                        //continue;
                                    }
                                    i.Result = ValidationResult.OK;
                                    i.MyGoObject = existingLink;
                                    (i.MyGoObject as QLink).InferenceType = InferenceType.Augmentation;
                                    if (existingLink.AutomatedAddition == true && existingLink.IsInDocument == false)
                                    {
                                        if (Engine.Options.IndicateAugmentive)
                                        {
                                            Engine.Diagram.Add(i.MyGoObject);
                                        }
                                        i.AddObject = true;
                                    }
                                    else
                                    {
                                        i.AddObject = false;
                                    }
                                }
                            }
                            if (i.MyGoObject == null)
                            {
                                continue;
                            }
                            if (Engine.Options.IndicateAugmentive)
                            {
                                Pen p = new Pen(Engine.Options.ColorAugmentive, 2);
                                (i.MyGoObject as QLink).Pen = p;
                                i.AdditionalInformation = FromNode.MetaObject.ToString() + "--0-->" + ToNode.MetaObject.ToString();
                                AllItems.Add(i);

                                AddArtefact("DependencyDescription", "Augmentative", "0", i.MyGoObject as QLink);
                            }
                            else
                            {
                                Pen p = new Pen((i.MyGoObject as QLink).PenColorBeforeCompare, 1);
                                (i.MyGoObject as QLink).Pen = p;
                                if ((i.MyGoObject as QLink).AutomatedAddition == true)
                                    (i.MyGoObject as QLink).Remove();
                            }
                        }
                        else
                        {
                            if (existingLink != null)
                            {
                                //why is this link here?
                                //existingLink.Pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                                if (existingLink.InferenceType == InferenceType.Error || existingLink.InferenceType != InferenceType.None)
                                    continue;

                                ValidationItem i = new ValidationItem();
                                i.ValidationType = "Augmentation";
                                i.MyGoObject = existingLink;

                                (i.MyGoObject as QLink).InferenceType = InferenceType.Error;
                                i.AddObject = false;
                                i.Result = ValidationResult.Error;
                                i.AdditionalInformation = "Invalid link";
                                AllItems.Add(i);

                                Engine.ErrorLinks.Add(existingLink);
                            }
                        }
                    }

                }
            }
        }

        #endregion

        private int CountKeyAttributes(CollapsibleNode node)
        {
            int count = 0;
            foreach (RepeaterSection rsec in node.RepeaterSections)
            {
                if (!(rsec.Name.Contains("Key")))
                    continue;
                foreach (GoObject obj in rsec)
                {
                    if (!(obj is CollapsingRecordNodeItem) || (obj as CollapsingRecordNodeItem).IsHeader)
                        continue;
                    count += 1;
                }
            }
            return count;
        }

        //is from key set part of to key set?
        private bool IsAugment(CollapsibleNode from, CollapsibleNode to)
        {
            List<string> fromKeys = new List<string>();
            //Dictionary<string, bool> fromKeys = new Dictionary<string, bool>();

            if (GetLink(from, to, LinkAssociationType.SubSetOf) != null)
            {
                return false;
            }

            foreach (RepeaterSection rsecFrom in from.RepeaterSections)
            {
                if (!(rsecFrom.Name.Contains("Key")))
                    continue;
                foreach (GoObject objFrom in rsecFrom)
                {
                    if (!(objFrom is CollapsingRecordNodeItem) || (objFrom as CollapsingRecordNodeItem).IsHeader)
                        continue;
                    fromKeys.Add((objFrom as CollapsingRecordNodeItem).MetaObject.ToString());
                }
            }
            List<string> toKeys = new List<string>();

            foreach (RepeaterSection rsecTo in to.RepeaterSections)
            {
                if (!(rsecTo.Name.Contains("Key")))
                    continue;
                foreach (GoObject objFrom in rsecTo)
                {
                    if (!(objFrom is CollapsingRecordNodeItem) || (objFrom as CollapsingRecordNodeItem).IsHeader)
                        continue;
                    toKeys.Add((objFrom as CollapsingRecordNodeItem).MetaObject.ToString());
                }
            }

            //add keys from transitive entities Dependant on tonode
            addDeterminantEntitiesKeySet(fromKeys, from, to);

            foreach (string s in toKeys)
            {
                bool found = false;
                foreach (string si in fromKeys)
                {
                    found = si == s;
                    if (found)
                        break;
                }
                if (!found)
                    return false;
            }

            return true;
        }

        private List<string> addDeterminantEntitiesKeySet(List<string> fromKeys, CollapsibleNode from, CollapsibleNode to)
        {
            foreach (IGoLink lnk in from.DestinationLinks)
            {
                if (Engine.TransitiveLinks.Contains(lnk as QLink))
                    continue;

                //if (Engine.ErrorLinks.Contains(lnk as QLink))
                //    continue;

                if ((lnk as QLink).AssociationType != LinkAssociationType.Dependency)
                    continue;

                if (!(lnk.ToNode is CollapsibleNode))
                    continue;

                if (lnk.ToNode == to)
                    continue;

                foreach (RepeaterSection rsecFrom in (lnk.ToNode as CollapsibleNode).RepeaterSections)
                {
                    if (!(rsecFrom.Name.Contains("Key")))
                        continue;
                    foreach (GoObject objFrom in rsecFrom)
                    {
                        if (!(objFrom is CollapsingRecordNodeItem) || (objFrom as CollapsingRecordNodeItem).IsHeader)
                            continue;
                        if (fromKeys.Contains((objFrom as CollapsingRecordNodeItem).MetaObject.ToString()))
                            continue;
                        fromKeys.Add((objFrom as CollapsingRecordNodeItem).MetaObject.ToString());
                    }
                }

                foreach (string s in addDeterminantEntitiesKeySet(fromKeys, (lnk.ToNode as CollapsibleNode), to))
                {
                    if (fromKeys.Contains(s))
                        continue;
                    fromKeys.Add(s);
                }
            }

            return fromKeys;
        }

    }
}
