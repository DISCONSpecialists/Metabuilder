using System;
using System.Collections;
using System.Reflection;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.Meta;

namespace MetaBuilder.BusinessFacade.Imports.FSD
{
    /// <summary>
    /// Summary description for Function.
    /// </summary>
    public class Function
    {
        public int FunctionNo;
        public string VerbQualifiedObject;
        public ArrayList Children;
        private Function parentFunction;

        public Function ParentFunction
        {
            get { return parentFunction; }
            set
            {
                parentFunction = value;
                parentFunction.AddChild(this);
                this.FunctionNo = parentFunction.Children.Count;
            }
        }

        public MetaBase MetaObject;

        public void AddChild(Function child)
        {
            if (Children == null)
            {
                Children = new ArrayList();
            }
            Children.Add(child);

        }

        public Function()
        {
            MetaObject = Loader.CreateInstance("Function");

        }

        public Function(string TextualDescription)
        {
            MetaObject = Loader.CreateInstance("Function");


            if (TextualDescription.IndexOf(" ") > -1)
            {
                VerbQualifiedObject = TextualDescription;//.Substring(0, TextualDescription.IndexOf(" "));
                //QualifiedObject = TextualDescription.Substring(TextualDescription.IndexOf(" ") + 1, (TextualDescription.Length - (Verb.Length + 1)));
            }
            else
            {
                VerbQualifiedObject = TextualDescription;
            }

        }

        public void Save()
        {
            PropertyInfo propVerb = MetaObject.GetType().GetProperty("Name");
            propVerb.SetValue(MetaObject, VerbQualifiedObject, null);
            MetaObject.Save(Guid.NewGuid());
            if (Children != null)
            {
                foreach (Function child in Children)
                {
                    if (!(child.MetaObject.pkid > 0))
                        child.Save();
                    // By default all associations will be decompositions. Any auxiliaries etc will have to be changed manually.
                    bool AddedAssociation = false;
                    if (child.VerbQualifiedObject != null)
                    {
                        if (child.VerbQualifiedObject.ToLower().IndexOf("aux") > -1)
                        {
                            Singletons.GetAssociationHelper().AddQuickAssociation(this.MetaObject.pkid, child.MetaObject.pkid, this.MetaObject.MachineName, child.MetaObject.MachineName, 1);
                            AddedAssociation = true;
                        }

                    }

                    if (child.VerbQualifiedObject != null)
                    {
                        if (child.VerbQualifiedObject.ToLower().IndexOf("aux") > -1)
                        {
                            Singletons.GetAssociationHelper().AddQuickAssociation(this.MetaObject.pkid, child.MetaObject.pkid, this.MetaObject.MachineName, child.MetaObject.MachineName, 1);
                            AddedAssociation = true;
                        }
                    }

                    if (!AddedAssociation)
                    {
                        Singletons.GetAssociationHelper().AddQuickAssociation(this.MetaObject.pkid, child.MetaObject.pkid, this.MetaObject.MachineName, child.MetaObject.MachineName, 3);
                    }
                }
            }
        }

        public void Load(int ObjectID, string Machine)
        {
            MetaObject = Loader.GetByID("Function", ObjectID, Machine);
        }

        public void DebugFunction(int DebugIndent)
        {
            string IndentString = "";
            for (int i = 0; i <= DebugIndent; i++)
            {
                IndentString += "\t";
            }

            //Debug.WriteLine("> " + IndentString + this.Verb + " " + this.QualifiedObject);

            if (this.Children != null)
            {
                DebugIndent++;
                foreach (Function f in this.Children)
                {
                    f.DebugFunction(DebugIndent);
                }
            }


        }

    }
}