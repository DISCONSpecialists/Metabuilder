using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing;
using MetaBuilder.Graphing.Shapes;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Inference
{
    internal class Transitivity : BaseRule
    {
        public Transitivity()
        {
            Description = "If X determines Y and Y determines Z, then X determines Z by definition";
            //Description = "If X functionally determines Y, and if Z is a true super-set or equal to K, and if XZ and YK exist together, then XZ functionally determines YK, by definition";
        }

        private Relation GetExistingLink(Entity e1, Entity e2, Engine e)
        {
            foreach (Relation r in e.GetRelations())
            {
                if (r.From == e1 && r.To == e2)
                {
                    return r;
                }
            }
            return null;
        }

        //List<Attr> resolvedAttributes;
        #region Implementation of Base Method
        List<Entity> processed;
        public override void Apply(Engine e)
        {
            /* foreach grandparent 
             * if it is a parent, then mark as Transitive 
             * if link doesnt exist, add link and mark as transitive
             * */
            List<Relation> relationsToAdd = new List<Relation>();

            foreach (ADDNode addnode in e.addnodes)
            {

                if (addnode is Entity)
                {
                    processed = new List<Entity>();
                    Entity ent = addnode as Entity;
                    List<Entity> parents = ent.GetParentEntities(true);
                    foreach (Entity entParent in parents)
                    {
                        if (!processed.Contains(entParent))
                        {
                            if (entParent != ent)
                            {
                                List<Entity> grandParents = new List<Entity>();
                                entParent.GetAllParentEntities(grandParents);
                                foreach (Entity grandParent in grandParents)
                                {
                                    if (ent != grandParent)
                                    {
                                        QLink lnkTrans = null;
                                        Relation r = GetExistingLink(ent, grandParent, e);
                                        bool isConcrete = false;
                                        if (r != null)
                                        {
                                            if (!r.IsAbstract)
                                            {
                                                if (r.InferenceType != DependencyType.Reflexive)
                                                {
                                                    isConcrete = true;

                                                    r.InferenceType = DependencyType.Transitive;
                                                    lnkTrans = r.MyGoObject as QLink;
                                                    if (e.Options.IndicateTransitive)
                                                        lnkTrans.Pen = new System.Drawing.Pen(System.Drawing.Color.Red, 3);
                                                    else
                                                        if (lnkTrans.AutomatedAddition)
                                                            lnkTrans.Remove();
                                                        else
                                                            lnkTrans.Pen = new System.Drawing.Pen(lnkTrans.PenColorBeforeCompare, 3);
                                                }
                                            }
                                        }
                                        if (r == null)
                                        {
                                            if (!isConcrete)
                                            {

                                                Relation rTrans = new Relation(ent, grandParent, MetaBuilder.Meta.LinkAssociationType.Dependency, lnkTrans, false);
                                                rTrans.InferenceType = DependencyType.Transitive;
                                                rTrans.IsAbstract = true;
                                                relationsToAdd.Add(rTrans);
                                            }
                                        }
                                    }
                                }
                            }
                            processed.Add(entParent);
                        }
                    }
                }
            }

            foreach (Relation r in relationsToAdd)
            {
                e.SafelyAddRelation(r);
                r.AddToCollections();
            }
        }

        public bool TryToFindParentEntity(ADDNode e, Entity target)
        {
            List<Entity> parents = (e as Entity).GetAllParents();
            return parents.Contains(target);

            /*bool found = false;
            if (e == target)
                return false;
            foreach (Relation r in e.DependentRelations)
            {
                if (r.To == target)
                {
                    return true;
                }
                found = TryToFindParentEntity(r.To, target);
                if (found)
                {
                    return true;
                }
            }*/
            return false;
        }



        #endregion
    }
}
