#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: Compiler.cs
//

#endregion

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using MetaBuilder.Core;
using MetaBuilder.Meta.Editors;
using Microsoft.CSharp;

namespace MetaBuilder.Meta.Builder
{
    /// <summary>
    /// Summary description for MyCompiler.
    /// </summary>
    public class Compiler
    {
        public Compiler(int sourcefilecount, int enumfilecount)
        {
            SourceFiles = new string[sourcefilecount];
            SortFiles = new string[sourcefilecount];
            EnumFiles = new string[enumfilecount];
        }

        /// <summary>
        /// Code Provider
        /// </summary>
        private CSharpCodeProvider cscp;

        /// <summary>
        /// Hosts the current class
        /// </summary>
        public CodeTypeDeclaration CurrentClass;

        /// <summary>
        /// Current class' namespace
        /// </summary>
        public CodeNamespace CurrentNameSpace;

        /// <summary>
        /// The sourcefiles used
        /// </summary>
        public string[] SourceFiles;

        /// <summary>
        /// The sortfiles used
        /// </summary>
        public string[] SortFiles;

        /// <summary>
        /// The enumfiles used
        /// </summary>
        public string[] EnumFiles;

        /// <summary>
        /// Indicator of how many classes were generated/compiled
        /// </summary>
        public int ClassCounter;

        /// <summary>
        /// Indicator of how many sortclasses were generated/compiled
        /// </summary>
        public int SortClassCounter;

        /// <summary>
        /// Indicator of how many enums were generated/compiled
        /// </summary>
        public int EnumCounter;

        /// <summary>
        /// Results from compile method. May include errors.
        /// </summary>
        public CompilerResults results;

        /// <summary>
        /// Add the override ToString() method. 
        /// </summary>
        /// <param name="DescriptionCode">What to return when ToString() is called</param>
        public void AddToStringField(string DescriptionCode)
        {
            CodeMemberMethod tostringMethod = new CodeMemberMethod();
            tostringMethod.ReturnType = new CodeTypeReference(typeof(String));
            tostringMethod.Name = "ToString";
            CodeSnippetStatement css = new CodeSnippetStatement("return " + DescriptionCode + ";");

            tostringMethod.Statements.Add(css);
            tostringMethod.Attributes = MemberAttributes.Override | MemberAttributes.Public;
            CurrentClass.Members.Add(tostringMethod);

            //whether we can add a default property or not
            //if (!(DescriptionCode.Contains(" ") || DescriptionCode.Contains(".") || DescriptionCode.Contains("+") || DescriptionCode.Contains("?") || DescriptionCode.Contains(":")))
            //{
            //    CodeAttributeDeclaration cad = new CodeAttributeDeclaration("DefaultPropertyAttribute ", new CodeAttributeArgument(new CodePrimitiveExpression("DescriptionCode")));
            //    CurrentClass.CustomAttributes.Add(cad);
            //}
            //else
            //{
            //    //first 1?
            //    foreach (CodeTypeMember m in CurrentClass.Members.GetEnumerator())
            //    {
            //        if (DescriptionCode.Contains(m.Name))
            //        {
            //            if (m.Name != "Description" || m.Name != "Value" || m.Name != "UniqueRef")
            //                continue;

            //            CodeAttributeDeclaration cad = new CodeAttributeDeclaration("DefaultPropertyAttribute ", new CodeAttributeArgument(new CodePrimitiveExpression(m.Name)));
            //            CurrentClass.CustomAttributes.Add(cad);
            //            break;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Adds a field & property of a certain type with description and category attributes used in propertygrid
        /// </summary>
        /// <param name="FieldName">The name of the field being added</param>
        /// <param name="PropertyName">The name of the property being added</param>
        /// <param name="CodeTypeRef">The type of the field & property</param>
        /// <param name="Description">The description to be used in the propertygrid</param>
        /// <param name="CategoryAttributeValue">The category to be used in the propertygrid</param>
        public void AddAttachmentListField(string FieldName, string PropertyName, string CodeTypeRef, string Description, string CategoryAttributeValue)
        {
            CodeAttributeDeclaration uiEditorDeclaration = null;
            CodeTypeReference codetyperef = new CodeTypeReference(CodeTypeRef);
            CodeMemberField newField = new CodeMemberField(codetyperef, FieldName);
            newField.Attributes = MemberAttributes.Public;
            CurrentClass.Members.Add(newField);

            CodeMemberProperty p = new CodeMemberProperty();
            p.Name = PropertyName;
            p.Attributes = MemberAttributes.Public;
            p.Type = codetyperef;
            p.HasGet = true;
            p.HasSet = true;

            CodeAttributeDeclarationCollection collection = new CodeAttributeDeclarationCollection();
            collection.Add(new CodeAttributeDeclaration("DescriptionAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(Description))));
            collection.Add(new CodeAttributeDeclaration("CategoryAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(CategoryAttributeValue))));
            collection.Add(new CodeAttributeDeclaration("Editor", new CodeAttributeArgument(new CodeTypeOfExpression(typeof(FileCollectionEditor))), new CodeAttributeArgument(new CodeTypeOfExpression(typeof(UITypeEditor)))));

            if (uiEditorDeclaration != null)
            {
                collection.Add(uiEditorDeclaration);
            }

            p.CustomAttributes.AddRange(collection);

            p.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), FieldName)));

            p.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), FieldName), new CodeArgumentReferenceExpression("value")));
            p.SetStatements.Add(new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "PropertyChanged")));
            CurrentClass.Members.Add(p);
        }

        /// <summary>
        /// Adds a field & property of a certain type with description and category attributes used in propertygrid
        /// </summary>
        /// <param name="FieldName">The name of the field being added</param>
        /// <param name="PropertyName">The name of the property being added</param>
        /// <param name="CodeTypeRef">The type of the field & property</param>
        /// <param name="Description">The description to be used in the propertygrid</param>
        /// <param name="CategoryAttributeValue">The category to be used in the propertygrid</param>
        public void AddAttachmentField(string FieldName, string PropertyName, string CodeTypeRef, string Description, string CategoryAttributeValue)
        {
            CodeAttributeDeclaration uiEditorDeclaration = null;
            CodeTypeReference codetyperef = new CodeTypeReference(CodeTypeRef);
            CodeMemberField newField = new CodeMemberField(codetyperef, FieldName);
            newField.Attributes = MemberAttributes.Public;
            CurrentClass.Members.Add(newField);

            CodeMemberProperty p = new CodeMemberProperty();
            p.Name = PropertyName;

            p.Attributes = MemberAttributes.Public;
            p.Type = codetyperef;
            p.HasGet = true;
            p.HasSet = true;

            CodeAttributeDeclarationCollection collection = new CodeAttributeDeclarationCollection();
            collection.Add(new CodeAttributeDeclaration("DescriptionAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(Description))));
            collection.Add(new CodeAttributeDeclaration("CategoryAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(CategoryAttributeValue))));
            collection.Add(new CodeAttributeDeclaration("Editor", new CodeAttributeArgument(new CodeTypeOfExpression(typeof(FileEditor))), new CodeAttributeArgument(new CodeTypeOfExpression(typeof(UITypeEditor)))));

            if (uiEditorDeclaration != null)
            {
                collection.Add(uiEditorDeclaration);
            }

            p.CustomAttributes.AddRange(collection);

            p.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), FieldName)));

            p.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), FieldName), new CodeArgumentReferenceExpression("value")));
            p.SetStatements.Add(new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "PropertyChanged")));
            CurrentClass.Members.Add(p);
        }

        /// <summary>
        /// Adds a field & property of a certain type with description and category attributes used in propertygrid
        /// </summary>
        /// <param name="FieldName">The name of the field being added</param>
        /// <param name="PropertyName">The name of the property being added</param>
        /// <param name="CodeTypeRef">The type of the field & property</param>
        /// <param name="Description">The description to be used in the propertygrid</param>
        /// <param name="CategoryAttributeValue">The category to be used in the propertygrid</param>
        public void AddField(string FieldName, string PropertyName, string CodeTypeRef, string Description, string CategoryAttributeValue, bool IsActive)
        {
            if (!IsActive)
                return;
            CodeAttributeDeclaration uiEditorDeclaration = null;
            CodeTypeReference codetyperef = new CodeTypeReference(CodeTypeRef);
            CodeMemberField newField = new CodeMemberField(codetyperef, FieldName);
            newField.Attributes = MemberAttributes.Public;
            CurrentClass.Members.Add(newField);

            CodeMemberProperty p = new CodeMemberProperty();
            p.Name = PropertyName;

            //  if (IsActive)
            {
                p.Attributes = MemberAttributes.Public;

            }
            //  else
            //    {
            //        p.Attributes = MemberAttributes.Private;

            //  }
            p.Type = codetyperef;
            p.HasGet = true;
            p.HasSet = true;

            CodeAttributeDeclarationCollection collection = new CodeAttributeDeclarationCollection();
            collection.Add(new CodeAttributeDeclaration("DescriptionAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(Description))));
            if (CategoryAttributeValue != "General")
            {
                //"|ValueA|ValueB|"
                collection.Add(new CodeAttributeDeclaration("ObsoleteAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(CategoryAttributeValue))));
                CategoryAttributeValue = "Specific";
            }

            //CodeTypeReferenceExpression refEnum = new CodeTypeReferenceExpression(typeof(RefreshProperties));
            //CodeExpression enumExpr = new CodeFieldReferenceExpression(refEnum, "Repaint");
            //CodeAttributeDeclaration attr = new CodeAttributeDeclaration(new CodeTypeReference(typeof(RefreshPropertiesAttribute)));
            //CodeAttributeArgument attrArg = new CodeAttributeArgument(enumExpr);
            //attr.Arguments.Add(attrArg);
            //collection.Add(attr);

            collection.Add(new CodeAttributeDeclaration("CategoryAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(CategoryAttributeValue))));
            collection.Add(new CodeAttributeDeclaration("BrowsableAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(IsActive))));
            if (p.Name == "Name")
            {
                //[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
                //CodeAttributeArgumentCollection caac = new CodeAttributeArgumentCollection();
                //CodeAttributeArgument caa = new CodeAttributeArgument(new CodeTypeOfExpression("MultilineStringEditor"));
                //CodeAttributeArgument cab = new CodeAttributeArgument(new CodeTypeOfExpression("UITypeEditor"));
                //caac.Add(caa);
                //caac.Add(cab);
                uiEditorDeclaration = new CodeAttributeDeclaration("Editor", new CodeAttributeArgument[] { new CodeAttributeArgument(new CodeTypeOfExpression("MultilineStringEditor")), new CodeAttributeArgument(new CodeTypeOfExpression("UITypeEditor")) });
            }

            if (uiEditorDeclaration != null)
            {
                collection.Add(uiEditorDeclaration);
            }

            p.CustomAttributes.AddRange(collection);

            p.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), FieldName)));
            p.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), FieldName), new CodeArgumentReferenceExpression("value")));
            p.SetStatements.Add(new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "PropertyChanged")));
            CurrentClass.Members.Add(p);
        }

        /// <summary>
        /// Adds a field & property that will point to a domain definition
        /// </summary>
        /// <param name="FieldName">The name of the field being added</param>
        /// <param name="PropertyName">The name of the property being added</param>
        /// <param name="CodeTypeRef">The type of the field & property</param>
        /// <param name="Description">The description to be used in the propertygrid</param>
        /// <param name="CategoryAttributeValue">The category to be used in the propertygrid</param>
        public void AddDomainField(string FieldName, string PropertyName, string CodeTypeRef, string Description, string CategoryAttributeValue, string DomainDefinition, bool IsActive)
        {
            if (!IsActive)
                return;
            CodeAttributeDeclaration uiEditorDeclaration = null;
            CodeTypeReference codetyperef = new CodeTypeReference("System.String");
            CodeMemberField newField = new CodeMemberField(codetyperef, FieldName);
            newField.Attributes = MemberAttributes.Public;
            CurrentClass.Members.Add(newField);

            CodeMemberProperty p = new CodeMemberProperty();
            p.Name = PropertyName;
            //       if (IsActive)
            {
                p.Attributes = MemberAttributes.Public;
            }
            //    else
            //    {
            //        p.Attributes = MemberAttributes.Private;
            // }
            p.Type = codetyperef;
            p.HasGet = true;
            p.HasSet = true;

            CodeAttributeDeclarationCollection collection = new CodeAttributeDeclarationCollection();
            collection.Add(new CodeAttributeDeclaration("DomainAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(DomainDefinition))));
            collection.Add(new CodeAttributeDeclaration("DescriptionAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(Description))));
            collection.Add(new CodeAttributeDeclaration("CategoryAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(CategoryAttributeValue))));
            collection.Add(new CodeAttributeDeclaration("BrowsableAttribute", new CodeAttributeArgument(new CodePrimitiveExpression(IsActive))));
            collection.Add(new CodeAttributeDeclaration("Editor", new CodeAttributeArgument(new CodeTypeOfExpression(typeof(UniversalDropdownEditor))), new CodeAttributeArgument(new CodeTypeOfExpression(typeof(UITypeEditor)))));
            collection.Add(new CodeAttributeDeclaration("SourceCollection", new CodeAttributeArgument(new CodePrimitiveExpression(DomainDefinition))));
            collection.Add(new CodeAttributeDeclaration("ValueMember", new CodeAttributeArgument(new CodePrimitiveExpression("PossibleValue"))));
            collection.Add(new CodeAttributeDeclaration("DisplayMember", new CodeAttributeArgument(new CodePrimitiveExpression("Description"))));

            //[Editor(typeof (RichTextEditor), typeof (UITypeEditor))]

            if (uiEditorDeclaration != null)
            {
                collection.Add(uiEditorDeclaration);
            }

            p.CustomAttributes.AddRange(collection);
            p.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), FieldName)));
            p.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), FieldName), new CodeArgumentReferenceExpression("value")));
            CurrentClass.Members.Add(p);
        }

        private void AddImports(string NameSpace)
        {
            CurrentNameSpace = new CodeNamespace(NameSpace);
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("System"));
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("System.Collections"));
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("System.Diagnostics"));
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("System.ComponentModel"));

            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("System.ComponentModel.Design"));//multilineeditors

            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("System.Drawing.Design"));
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("System.Design"));
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("MetaBuilder.Meta"));
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("MetaBuilder.Meta.Editors"));
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("MetaBuilder.BusinessLogic"));
        }

        /// <summary>
        /// Creates a class within the given namespace
        /// </summary>
        /// <param name="NameSpace">The namespace in which to create this class</param>
        /// <param name="ClassName">The class to create</param>
        public void CreateClass(string NameSpace, string ClassName)
        {
            // Add imports
            AddImports(NameSpace);
            // Create the class
            CurrentClass = new CodeTypeDeclaration(ClassName);
            CurrentNameSpace.Types.Add(CurrentClass);

            // Add default comment to sourcefile
            CodeCommentStatement comment = new CodeCommentStatement("This source code file was generated using the DISCON Specialists MetaBuilder engine. Copyright © 2009");
            CurrentClass.Comments.Add(comment);

            // Add a constructor
            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            CurrentClass.Members.Add(constructor);

            // Derive from MetaBase
            CurrentClass.BaseTypes.Add(typeof(MetaBase));
            /*
            CurrentClass.BaseTypes.Add(typeof(System.Runtime.Serialization.ISerializable));*/

            // Add typeconvertor attribute
            CodeAttributeArgument caa = new CodeAttributeArgument(new CodeTypeOfExpression(Variables.MetaNameSpace + "." + ClassName + "Sort"));
            CodeAttributeDeclaration cad = new CodeAttributeDeclaration("TypeConverter", caa);
            CurrentClass.CustomAttributes.Add(cad);

            CodeAttributeDeclaration serad = new CodeAttributeDeclaration("Serializable");
            CurrentClass.CustomAttributes.Add(serad);

            /*
            // Declare a constructor that takes a string argument and calls the base class constructor with it.
            CodeConstructor baseStringConstructor  = new CodeConstructor();
            baseStringConstructor.Attributes = MemberAttributes.Public;
            // Declares a parameter of type string named "TestStringParameter".    
            baseStringConstructor.Parameters.Add(new CodeParameterDeclarationExpression("System.Runtime.Serialization.SerializationInfo", "info"));
            baseStringConstructor.Parameters.Add(new CodeParameterDeclarationExpression("System.Runtime.Serialization.StreamingContext", "ctxt"));
            // Calls a base class constructor with the TestStringParameter parameter.
            baseStringConstructor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("info"));
            baseStringConstructor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("ctxt"));
            // Adds the constructor to the Members collection of the DerivedType.
            CurrentClass.Members.Add(baseStringConstructor);

            */


            /*   CodeMemberMethod cmm = new CodeMemberMethod();
               CodeEventReferenceExpression cref = new CodeEventReferenceExpression(new CodeThisReferenceExpression(), "ChangeGenerated");
               CodePrimitiveExpression cpe = new CodePrimitiveExpression(null);
               CodeBinaryOperatorExpression cboe = new CodeBinaryOperatorExpression(cref, CodeBinaryOperatorType.IdentityInequality, cpe);
               cmm.Statements.Add(cboe);
               CodeDelegateInvokeExpression cdie = new CodeDelegateInvokeExpression(cref);
               cmm.Statements.Add(cdie);
               cmm.Name = "OnChangedGenerated";
               CurrentClass.Members.Add(cmm);

               CodeMemberEvent cmevt = new CodeMemberEvent();
               cmevt.Name = "ChangeGenerated";
               cmevt.Attributes = MemberAttributes.Public;
               CurrentClass.Members.Add(cmevt);*/
            ClassCounter++;

        }

        /// <summary>
        /// Creates a sortclass.
        /// </summary>
        /// <param name="NameSpace">Target namespace</param>
        /// <param name="ClassName">Target classname (for a class named MyClass, this is usually MyClassSort)</param>
        /// <param name="SortedFields">The fields as they should be sorted</param>
        public void CreateSortClass(string NameSpace, string ClassName, string[] SortedFields)
        {
            // Add imports
            CurrentNameSpace = new CodeNamespace(NameSpace);
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("System"));
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("System.Drawing.Design"));
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("MetaBuilder.Meta"));
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("MetaBuilder.Meta.Editors"));
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("MetaBuilder.BusinessLogic"));

            // Create the class
            CurrentClass = new CodeTypeDeclaration(ClassName);

            // Derive from BaseSorter
            CurrentClass.BaseTypes.Add("BaseSorter");
            CurrentNameSpace.Types.Add(CurrentClass);

            // Add a constructor
            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            CurrentClass.Members.Add(constructor);

            // Add the sorted fields. 
            CodeExpression[] initializers = new CodeExpression[SortedFields.Length];
            for (int i = 0; i < SortedFields.Length; i++)
            {
                initializers[i] = new CodePrimitiveExpression(SortedFields[i]);
            }

            CodeArrayCreateExpression codeArrayCreateExpr = new CodeArrayCreateExpression("System.String", initializers);
            CodeAssignStatement codeAssignStmt = new CodeAssignStatement(new CodeVariableReferenceExpression("SortOrder"), codeArrayCreateExpr);
            constructor.Statements.Add(codeAssignStmt);

            // Increase sortclass counter
            SortClassCounter++;
        }

        /// <summary>
        /// Creates an enum in the given namespace
        /// </summary>
        /// <param name="NameSpace"></param>
        /// <param name="EnumName"></param>
        public void CreateEnum(string NameSpace, string EnumName)
        {
            CurrentNameSpace = new CodeNamespace(NameSpace);
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("System"));
            CurrentNameSpace.Imports.Add(new CodeNamespaceImport("System.ComponentModel"));

            CurrentClass = new CodeTypeDeclaration(EnumName);
            CurrentClass.IsEnum = true;

            CurrentNameSpace.Types.Add(CurrentClass);
            EnumCounter += 1;
        }

        /// <summary>
        /// Adds an enumerated value to the current enum 
        /// </summary>
        /// <param name="Value"></param>
        public void AddEnumValue(string Description, string Value, int Order)
        {
            CodeMemberField fld;
            try
            {
                int result = -999;
                if (int.TryParse(Value, out result))
                {
                    fld = new CodeMemberField("System.Int32", Value);
                }
                else
                {
                    fld = new CodeMemberField("System.String", Value);// + " = " + Order);
                }

            }
            catch
            {
                fld = new CodeMemberField("System.String", Value);// + " = " + Order);
            }

            CodeAttributeDeclarationCollection collection = new CodeAttributeDeclarationCollection();
            collection.Add(new CodeAttributeDeclaration("Description", new CodeAttributeArgument(new CodePrimitiveExpression(Description))));
            //fld.CustomAttributes.AddRange(collection);
            CurrentClass.Members.Add(fld);
        }

        /// <summary>
        /// Generates code for the current enum and adds the output to the enumfiles array (to be compiled as a batch)
        /// </summary>
        public void GenerateEnumCode()
        {
            // Generate code file to output

            CodeDomProvider cshprov = new CSharpCodeProvider();
            string Filename = Variables.Instance.SourceCodePath + "\\" + CurrentClass.Name + ".cs";
            TextWriter tw = new StreamWriter(Filename);
            cshprov.GenerateCodeFromNamespace(CurrentNameSpace, tw, null);

            /*cscp = new CSharpCodeProvider();
            ICodeGenerator gen =cscp.CreateGenerator();
			
            gen.GenerateCodeFromNamespace(CurrentNameSpace, tw, null);*/
            tw.Close();
            EnumFiles[EnumCounter - 1] = Filename;
        }

        /// <summary>
        /// Generates code for the currentclass and adds the output to the sourcefiles array (to be compiled as a batch)
        /// </summary>
        public void GenerateCode()
        {
            // Generate code file to output
            cscp = new CSharpCodeProvider();
            string Filename = Variables.Instance.SourceCodePath + "\\" + CurrentClass.Name + ".cs";
            TextWriter tw = new StreamWriter(Filename);
            cscp.GenerateCodeFromNamespace(CurrentNameSpace, tw, null);
            tw.Close();
            SourceFiles[ClassCounter - 1] = Filename;
        }

        /// <summary>
        /// Generates code for the current sortclass and adds the output to the  sortfiles array (to be compiled as a batch)
        /// </summary>
        public void GenerateSortCode()
        {
            // Generate code file to output
            cscp = new CSharpCodeProvider();

            string Filename = Variables.Instance.SourceCodePath + "\\" + CurrentClass.Name + ".cs";
            TextWriter tw = new StreamWriter(Filename);
            cscp.GenerateCodeFromNamespace(CurrentNameSpace, tw, null);
            tw.Close();
            SortFiles[SortClassCounter - 1] = Filename;
        }

        /// <summary>
        /// Compiles the files in the sourcefiles array
        /// </summary>
        public void CompileCode()
        {
            //CompileEnumCode();
            CSharpCodeProvider cscp = new CSharpCodeProvider();

            string[] dlls = new string[] { "System.Drawing.dll", "System.Design.dll" };
            CompilerParameters parms = new CompilerParameters(dlls);
            if (Core.FilterVariables.filterName != "settings.xml")
            {
                if (File.Exists(Variables.Instance.MetaAssembly))
                {
                    FilterItem i = new FilterItem(Core.FilterVariables.filterName);
                    MainAssemblyMoved = true;
                    parms.OutputAssembly = "C://Metabuilder.Meta.Generated" + i.ToString() + ".dll";
                }
                else
                {
                    parms.OutputAssembly = Variables.Instance.MetaAssembly;
                }
            }
            else
            {
                if (File.Exists(Variables.Instance.MetaAssembly))
                {
                    MainAssemblyMoved = true;
                    parms.OutputAssembly = "C://Metabuilder.Meta.Generated.dll";
                }
                else
                {
                    parms.OutputAssembly = Variables.Instance.MetaAssembly;
                }
            }

            parms.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(Compiler)).Location);
            parms.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(ExpandableObjectConverter)).Location);
            parms.ReferencedAssemblies.Add(Variables.Instance.MetaEnumAssembly);
            parms.ReferencedAssemblies.Add(Variables.Instance.MetaSortAssembly);
            parms.ReferencedAssemblies.Add(Variables.Instance.MetaAssemblyPath + "MetaBuilder.BusinessLogic.dll");
            results = cscp.CompileAssemblyFromFile(parms, SourceFiles);
        }

        /// <summary>
        /// Compiles the files in the sortfiles array
        /// </summary>
        public void CompileSortCode()
        {
            CSharpCodeProvider cscp = new CSharpCodeProvider();

            string[] dlls = new string[] { "System.Drawing.dll", "System.Design.dll" };
            CompilerParameters parms = new CompilerParameters(dlls);
            if (Core.FilterVariables.filterName != "settings.xml")
            {
                if (File.Exists(Variables.Instance.MetaSortAssembly))
                {
                    FilterItem i = new FilterItem(Core.FilterVariables.filterName);
                    SortAssemblyMoved = true;
                    parms.OutputAssembly = "C://MetaBuilder.Meta.Generated" + i.ToString() + ".Sorters.dll";
                }
                else
                {
                    parms.OutputAssembly = Variables.Instance.MetaSortAssembly;
                }
            }
            else
            {
                if (File.Exists(Variables.Instance.MetaSortAssembly))
                {
                    SortAssemblyMoved = true;
                    parms.OutputAssembly = "C://MetaBuilder.Meta.Generated.Sorters.dll";
                }
                else
                {
                    parms.OutputAssembly = Variables.Instance.MetaSortAssembly;
                }
            }
            parms.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(Compiler)).Location);
            parms.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(ExpandableObjectConverter)).Location);
            parms.ReferencedAssemblies.Add(Variables.Instance.MetaAssemblyPath + "MetaBuilder.BusinessLogic.dll");
            results = cscp.CompileAssemblyFromFile(parms, SortFiles);
        }

        /// <summary>
        /// Compiles the files in the sortfiles array
        /// </summary>
        public void CompileEnumCode()
        {
            CSharpCodeProvider cscp = new CSharpCodeProvider();
            string[] dlls = new string[] { "System.dll", "System.Drawing.dll", "System.Design.dll" };
            CompilerParameters parms = new CompilerParameters(dlls);
            //if (Core.FilterVariables.filterName != "settings.xml")
            //{
            //    parms.OutputAssembly = Variables.Instance.MetaEnumAssembly;
            //}
            //else
            //{
            //    //parms.OutputAssembly = Variables.Instance.MetaSortAssembly;
            //    parms.OutputAssembly = Variables.Instance.MetaEnumAssembly;
            //}
            parms.OutputAssembly = Variables.Instance.MetaEnumAssembly;
            results = cscp.CompileAssemblyFromFile(parms, EnumFiles);
        }

        public bool SortAssemblyMoved = false;
        public bool MainAssemblyMoved = false;
    }

}