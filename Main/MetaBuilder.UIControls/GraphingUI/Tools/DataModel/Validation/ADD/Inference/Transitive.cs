using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;
using MetaBuilder.Meta;
using System.Drawing;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD.Inference
{
    class Transitive : BaseInferenceRule
    {
        public Transitive(string name, Engine2015 engine)
            : base(name, engine)
        {
            Description = "Transitivity";
        }
        #region Implementation of Base Method
        /// <summary>
        /// if x-->y-->z then x-->z
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

                foreach (IGoLink lnk in FromNode.DestinationLinks)
                {
                    if (Engine.ErrorLinks.Contains(lnk as QLink))
                        continue;

                    if ((lnk as QLink).AssociationType != LinkAssociationType.Dependency)
                        continue;

                    if ((lnk as QLink).InferenceType == InferenceType.Error)
                        continue;

                    foreach (IGoLink link in lnk.ToNode.DestinationLinks)
                    {
                        if (Engine.ErrorLinks.Contains(link as QLink))
                            continue;

                        if ((link as QLink).AssociationType != LinkAssociationType.Dependency)
                            continue;

                        if ((link as QLink).InferenceType == InferenceType.Error)
                            continue;

                        ValidationItem i = new ValidationItem();
                        i.ValidationType = "Transitive";

                        if (FromNode == link.ToNode)
                            continue;

                        QLink existingLink = GetLink(FromNode, link.ToNode as GoNode, LinkAssociationType.Dependency) as QLink;
                        if (existingLink == null)
                        {
                            QLink transitiveLink = QLink.CreateLink(FromNode, link.ToNode as GoNode, (int)LinkAssociationType.Dependency, -1);
                            transitiveLink.Pen = new Pen(Engine.Options.ColorTransitive, 2);
                            transitiveLink.AutomatedAddition = true;
                            i.MyGoObject = transitiveLink;
                            (i.MyGoObject as QLink).InferenceType = InferenceType.Transitivity;
                            i.AddObject = true;
                            i.Result = ValidationResult.Warning;

                            if (Engine.Options.IndicateTransitive)
                            {
                                if (!(Engine.TransitiveLinks.Contains(existingLink)))
                                {
                                    Engine.Diagram.Add(transitiveLink);
                                    Engine.TransitiveLinks.Add(transitiveLink);
                                    AddArtefact("DependencyDescription", "Transitive", "-1", i.MyGoObject as QLink);
                                }
                            }
                            else
                            {
                                transitiveLink.Remove();
                            }
                        }
                        else
                        {
                            if (Engine.ReflexiveLinks.Contains(existingLink))
                                continue;
                            if (!(Engine.TransitiveLinks.Contains(existingLink)))
                                Engine.TransitiveLinks.Add(existingLink);

                            i.MyGoObject = existingLink;
                            (i.MyGoObject as QLink).InferenceType = InferenceType.Transitivity;
                            i.AddObject = false;
                            i.Result = ValidationResult.OK;

                            if (Engine.Options.IndicateTransitive)
                            {
                                existingLink.Pen = new Pen(Engine.Options.ColorTransitive, 2);
                                AddArtefact("DependencyDescription", "Transitive", "-1", i.MyGoObject as QLink);
                            }
                            else
                            {
                                existingLink.Pen = new Pen(existingLink.PenColorBeforeCompare, 1);
                                if (existingLink.AutomatedAddition)
                                    existingLink.Remove();
                            }
                        }
                        bool actedOn = false;
                        foreach (ValidationItem it in AllItems)
                        {
                            if (i.MyGoObject == it.MyGoObject)
                            {
                                actedOn = true;
                                break;
                            }
                        }

                        if (!actedOn)
                            AllItems.Add(i);
                    }
                }
            }
        }

        #endregion
    }
}
