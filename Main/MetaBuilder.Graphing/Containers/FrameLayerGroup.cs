#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Modification and/or tampering with the code is also strictly prohibited, and
// punishable by law.
//
// Filename: FrameLayerGroup.cs
// Author: Deon Fourie
// Last Modified: 2007-15-26
//

#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using MetaBuilder.Core;
using MetaBuilder.ResourceManagement;
using Northwoods.Go;
using System.ComponentModel;

namespace MetaBuilder.Graphing.Containers
{

    public class DocumentInfoSort : MetaBuilder.Meta.BaseSorter
    {
        public DocumentInfoSort()
        {
            SortOrder = new string[] {
                   "Name",
                   "Description",
                   "OrganisationUnit",
                   "Author",
                   "Version",
                   "Date",
                   "Filename",
            };
        }
    }
    [TypeConverter(typeof(DocumentInfoSort))]
    public class DocumentInfo
    {
        #region Fields (7)

        private string id;
        private string authors;
        private string companyName;
        private string description;
        private DateTime docDate;
        private string filename;
        private Guid originalFileUniqueID;
        private string version;
        private string docType; //ALA Title

        #endregion Fields

        #region Properties (7)

        [CategoryAttribute("General")]
        [BrowsableAttribute(true)]
        public string AuthorID
        {
            get { return id; }
            set
            {
                id = value;
                if (group != null)
                    group.Update(this);
            }
        }
        //[DescriptionAttribute("The name of the object (class instance) that uniquely identifies the object.")]
        [CategoryAttribute("General")]
        [BrowsableAttribute(true)]
        public string Author
        {
            get { return authors; }
            set
            {
                authors = value;
                if (group != null)
                    group.Update(this);
            }
        }

        [CategoryAttribute("General")]
        [BrowsableAttribute(true)]
        public string OrganisationUnit
        {
            get { return companyName; }
            set
            {
                companyName = value;
                if (group != null)
                    group.Update(this);
            }
        }

        [CategoryAttribute("General")]
        [BrowsableAttribute(true)]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                if (group != null)
                    group.Update(this);
            }
        }

        [CategoryAttribute("General")]
        [BrowsableAttribute(true)]
        [ReadOnly(true)]
        public DateTime Date
        {
            get { return docDate; }
            set { docDate = value; }
        }

        [CategoryAttribute("General")]
        [BrowsableAttribute(true)]
        [ReadOnly(true)]
        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }

        [CategoryAttribute("General")]
        [BrowsableAttribute(false)]
        public Guid OriginalFileUniqueID
        {
            get { return originalFileUniqueID; }
            set { originalFileUniqueID = value; }
        }

        [CategoryAttribute("General")]
        [BrowsableAttribute(true)]
        public string Version
        {
            get { return version; }
            set
            {
                version = value;
                if (group != null)
                    group.Update(this);
            }
        }

        [CategoryAttribute("General")]
        [BrowsableAttribute(true)]
        public string Name
        {
            get { return docType; }
            set
            {
                docType = value;
                if (group != null)
                    group.Update(this);
            }
        }

        #endregion Properties

        public void ForceVersion(int major, int minor)
        {
            Version = "V " + major + "." + minor;
        }

        public FrameLayerGroup group;
    }


    [Serializable]
    public class FrameLayerGroup : GoGroup
    {
        #region Fields (14)

        private const float FRAMELOGOMARGINRIGHT = 3f;
        private const float FRAMELOGOMARGINTOP = 3f;
        //private float ALLOWDESCRIPTIONBLEED = 80;
        private FrameLayerRect frame;
        private GoImage logo;
        private GoRectangle rectTop;

        private GoText lblID;
        private GoText lblAuthors;
        private GoText lblDate;
        private GoText lblDocumentDescription;
        private GoText lblDocumentType;
        private GoText lblMyCompanyName;
        private GoText lblVersion;
        private GoText lblFileName;

        private GoText txtID;
        private GoText txtAuthors;
        private GoText txtDate;
        private GoText txtDocumentDescription;
        private GoText txtDocumentType;
        private GoText txtMyCompanyName;
        private GoText txtVersion;
        public GoText txtFileName;

        #endregion Fields

        #region Constructors (1)

        public FrameLayerGroup()
        {
            Copyable = false;
            SkipsUndoManager = true;
        }

        #endregion Constructors

        #region Properties (2)

        public FrameLayerRect Frame
        {
            get { return frame; }
            set { frame = value; }
        }

        public float TopHeight
        {
            get { return logo.Height + 2; }
        }

        #endregion Properties

        #region Methods (14)

        // Public Methods (12) 

        public override bool CanCopy()
        {
            return false; // return base.CanCopy();
        }

        public override void Changed(int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            base.Changed(subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
            switch (subhint)
            {
                case 1001:
                    try
                    {
                        Reposition();
                    }
                    catch
                    {
                    }
                    break;
            }
        }

        public DocumentInfo GetDocumentInfo()
        {
            DocumentInfo retval = new DocumentInfo();
            retval.Author = txtAuthors.Text;
            retval.AuthorID = txtID.Text;
            retval.Description = txtDocumentDescription.Text;
            retval.OrganisationUnit = txtMyCompanyName.Text;
            retval.Date = GlobalParser.ParseGlobalisedDateString(txtDate.Text);
            retval.Version = txtVersion.Text;
            retval.FileName = txtFileName.Text;
            retval.Name = txtDocumentType.Text;
            return retval;
        }

        private bool init = true;
        public void InitTextBoxes(RectangleF bounds)
        {
            if (!init)
                return;
            Clear();

            frame = new FrameLayerRect();
            frame.Width = bounds.Size.Width - 2f;
            frame.Height = bounds.Size.Height - 2f;
            frame.Position = new PointF(bounds.X, bounds.Y);
            Pen p = new Pen(Color.Gray, 1f);
            frame.Pen = p;
            /*frame.Selectable = true;
           frame.Resizable = true;
           frame.Reshapable = true;*/
            Add(frame);
            DisableFrameObjectForEditing(frame);
            frame.Reshapable = true;

            LoadLogo();
            // Add frame borders (left top)
            rectTop = new GoRectangle();
            rectTop.Brush = Brushes.Snow;
            rectTop.Pen = new Pen(Color.Silver);
            rectTop.AutoRescales = false;
            rectTop.Selectable = false;
            Add(rectTop);
            // Add frame borders (right top)
            DisableFrameObjectForEditing(rectTop);

            // Now add the image (So it appears above the toprectangle)
            Add(logo);

            // Title
            lblDocumentType = new GoText();
            lblDocumentType.FontSize = 10;
            lblDocumentType.Bold = true;
            lblDocumentType.Editable = false;
            DisableFrameObjectForEditing(lblDocumentType);
            Add(lblDocumentType);

            txtDocumentType = new GoText();
            txtDocumentType.FontSize = 10;
            txtDocumentType.Editable = true;
            txtDocumentType.Selectable = false;
            Add(txtDocumentType);
            DisableFrameObjectForEditing(txtDocumentType);

            // Description
            lblDocumentDescription = new GoText();
            lblDocumentDescription.FontSize = 10;
            lblDocumentDescription.Bold = true;
            DisableFrameObjectForEditing(lblDocumentDescription);
            Add(lblDocumentDescription);

            txtDocumentDescription = new GoText();
            txtDocumentDescription.FontSize = 10;
            txtDocumentDescription.Selectable = false;
            txtDocumentDescription.Editable = true;
            DisableFrameObjectForEditing(txtDocumentDescription);
            Add(txtDocumentDescription);

            // Company Name
            lblMyCompanyName = new GoText();
            lblMyCompanyName.FontSize = 10;
            lblMyCompanyName.Bold = true;
            lblMyCompanyName.Editable = false;
            DisableFrameObjectForEditing(lblMyCompanyName);
            Add(lblMyCompanyName);

            txtMyCompanyName = new GoText();
            txtMyCompanyName.FontSize = 10;
            txtMyCompanyName.Editable = true;
            DisableFrameObjectForEditing(txtMyCompanyName);
            Add(txtMyCompanyName);

            // Filename
            lblFileName = new GoText();
            lblFileName.FontSize = 10;
            lblFileName.Bold = true;
            lblFileName.Editable = false;
            DisableFrameObjectForEditing(lblFileName);
            Add(lblFileName);

            txtFileName = new GoText();
            txtFileName.FontSize = 10;
            txtFileName.Editable = false;
            DisableFrameObjectForEditing(txtFileName);
            Add(txtFileName);

            // Version
            lblVersion = new GoText();
            lblVersion.FontSize = 10;
            lblVersion.Bold = true;
            lblVersion.Editable = false;
            DisableFrameObjectForEditing(lblVersion);
            Add(lblVersion);

            txtVersion = new GoText();
            txtVersion.FontSize = 10;
            txtVersion.Visible = true;
            txtVersion.Editable = true;
            txtVersion.Printable = false;
            txtVersion.Selectable = false;
            DisableFrameObjectForEditing(txtVersion);
            Add(txtVersion);

            // Date
            lblDate = new GoText();
            lblDate.FontSize = 10;
            lblDate.Bold = true;
            lblDate.Editable = false;
            DisableFrameObjectForEditing(lblDate);
            Add(lblDate);

            txtDate = new GoText();
            txtDate.FontSize = 10;
            txtDate.Selectable = false;
            txtDate.Selectable = false;
            DisableFrameObjectForEditing(txtDate);
            Add(txtDate);

            // Authors
            lblAuthors = new GoText();
            lblAuthors.FontSize = 10;
            lblAuthors.Bold = true;
            lblAuthors.Editable = false;
            DisableFrameObjectForEditing(lblAuthors);
            Add(lblAuthors);

            txtAuthors = new GoText();
            txtAuthors.FontSize = 10;
            txtAuthors.Selectable = false;
            txtAuthors.Editable = true;
            DisableFrameObjectForEditing(txtAuthors);
            Add(txtAuthors);

            // Authors
            lblID = new GoText();
            lblID.FontSize = 10;
            lblID.Bold = true;
            lblID.Editable = false;
            DisableFrameObjectForEditing(lblID);
            Add(lblID);

            //ID
            txtID = new GoText();
            txtID.FontSize = 10;
            txtID.Selectable = false;
            txtID.Editable = true;
            DisableFrameObjectForEditing(txtID);
            Add(txtID);

            txtID.FamilyName = "Verdana";
            txtAuthors.FamilyName = "Verdana";
            txtDocumentType.FamilyName = "Verdana";
            txtDate.FamilyName = "Verdana";
            txtDocumentDescription.FamilyName = "Verdana";
            txtFileName.FamilyName = "Verdana";
            txtMyCompanyName.FamilyName = "Verdana";
            txtVersion.FamilyName = "Verdana";

            lblID.FamilyName = "Verdana";
            lblAuthors.FamilyName = "Verdana";
            lblDocumentType.FamilyName = "Verdana";
            lblDate.FamilyName = "Verdana";
            lblDocumentDescription.FamilyName = "Verdana";
            lblFileName.FamilyName = "Verdana";
            lblMyCompanyName.FamilyName = "Verdana";
            lblVersion.FamilyName = "Verdana";

            Reposition();
            SetDefaults();

            init = false;
        }

        public override void LayoutChildren(GoObject childchanged)
        {
            if (!Initializing)
            {
                base.LayoutChildren(childchanged);
                Reposition();
                Initializing = false;
            }
        }

        public void LoadLogo()
        {
            // DISCON Logo
            logo = new GoImage();
            logo.AutoRescales = false;
            logo.Printable = false;
            logo.Image = Reader.PoweredByDisconSpecialistsImage;
            logo.Selectable = false;
            logo.Visible = true;
            DisableFrameObjectForEditing(logo);
        }

        public void Reposition()
        {
            if (logo != null && txtDocumentType != null && txtDocumentDescription != null)
            {
                PointF origin = frame.Position;
                rectTop.Position = origin;
                rectTop.Width = frame.Width - logo.Width - FRAMELOGOMARGINRIGHT - FRAMELOGOMARGINRIGHT;
                int verticalspacing = 15;
                int horizontalSpacing = 10;
                float xPosition = frame.Right - logo.Width - FRAMELOGOMARGINRIGHT;
                float yPosition = origin.Y + FRAMELOGOMARGINTOP;

                logo.Position = new PointF(xPosition, yPosition);
                logo.Printable = false;

                xPosition = origin.X + 3;
                yPosition = origin.Y + 3; //Must all be in same 'line'

                rectTop.Position = new PointF(xPosition, yPosition);
                //rectTop.Height = lblDocumentType.Height + 5;

                xPosition += 2;
                int lines = 1;
                if (xPosition + horizontalSpacing + lblDocumentType.Width + horizontalSpacing + txtDocumentType.Width > rectTop.Right)
                {
                    yPosition += verticalspacing;
                    xPosition = origin.X + 5;
                    lines += 1;
                }
                lblDocumentType.Position = new PointF(xPosition, yPosition);
                xPosition += horizontalSpacing + lblDocumentType.Width;
                txtDocumentType.Position = new PointF(xPosition, yPosition);

                xPosition += horizontalSpacing + txtDocumentType.Width;
                if (xPosition + horizontalSpacing + lblDocumentDescription.Width + horizontalSpacing + txtDocumentDescription.Width > rectTop.Right)
                {
                    yPosition += verticalspacing;
                    xPosition = origin.X + 5;
                    lines += 1;
                }
                lblDocumentDescription.Position = new PointF(xPosition, yPosition);
                xPosition += horizontalSpacing + lblDocumentDescription.Width;
                txtDocumentDescription.Position = new PointF(xPosition, yPosition);

                xPosition += horizontalSpacing + txtDocumentDescription.Width;
                if (xPosition + horizontalSpacing + lblMyCompanyName.Width + horizontalSpacing + txtMyCompanyName.Width > rectTop.Right)
                {
                    yPosition += verticalspacing;
                    xPosition = origin.X + 5;
                    lines += 1;
                }
                lblMyCompanyName.Position = new PointF(xPosition, yPosition);
                xPosition += horizontalSpacing + lblMyCompanyName.Width;
                txtMyCompanyName.Position = new PointF(xPosition, yPosition);

                xPosition += horizontalSpacing + txtMyCompanyName.Width;
                if (xPosition + horizontalSpacing + lblFileName.Width + horizontalSpacing + txtFileName.Width > rectTop.Right)
                {
                    yPosition += verticalspacing;
                    xPosition = origin.X + 5;
                    lines += 1;
                }
                lblFileName.Position = new PointF(xPosition, yPosition);
                xPosition += horizontalSpacing + lblFileName.Width;
                txtFileName.Position = new PointF(xPosition, yPosition);

                xPosition += horizontalSpacing + txtFileName.Width;
                if (xPosition + horizontalSpacing + lblAuthors.Width + horizontalSpacing + txtAuthors.Width > rectTop.Right)
                {
                    yPosition += verticalspacing;
                    xPosition = origin.X + 5;
                    lines += 1;
                }
                lblAuthors.Position = new PointF(xPosition, yPosition);
                xPosition += horizontalSpacing + lblAuthors.Width;
                txtAuthors.Position = new PointF(xPosition, yPosition);

                xPosition += horizontalSpacing + txtAuthors.Width;
                if (xPosition + horizontalSpacing + lblDate.Width + horizontalSpacing + txtDate.Width > rectTop.Right)
                {
                    yPosition += verticalspacing;
                    xPosition = origin.X + 5;
                    lines += 1;
                }
                lblDate.Position = new PointF(xPosition, yPosition);
                xPosition += horizontalSpacing + lblDate.Width;
                txtDate.Position = new PointF(xPosition, yPosition);

                xPosition += horizontalSpacing + txtDate.Width;
                if (xPosition + horizontalSpacing + lblVersion.Width + horizontalSpacing + txtVersion.Width > rectTop.Right)
                {
                    yPosition += verticalspacing;
                    xPosition = origin.X + 5;
                    lines += 1;
                }
                lblVersion.Position = new PointF(xPosition, yPosition);
                xPosition += horizontalSpacing + lblVersion.Width;
                txtVersion.Position = new PointF(xPosition, yPosition);

                xPosition += horizontalSpacing + txtVersion.Width;
                if (xPosition + horizontalSpacing + lblID.Width + horizontalSpacing + lblID.Width > rectTop.Right)
                {
                    yPosition += verticalspacing;
                    xPosition = origin.X + 5;
                    lines += 1;
                }
                lblID.Position = new PointF(xPosition, yPosition);
                xPosition += horizontalSpacing + lblID.Width;
                txtID.Position = new PointF(xPosition, yPosition);

                rectTop.Height = (lblDocumentType.Height + 1) * lines;
            }
        }

        public void SetDefaults()
        {
            lblDocumentType.Text = "Name :";
            lblDocumentDescription.Text = "Description :";
            lblID.Text = "Author ID :";
            lblAuthors.Text = "Author :";
            lblMyCompanyName.Text = "OrganisationUnit :";
            lblDate.Text = "Date :";
            lblVersion.Text = "Version :";
            lblFileName.Text = "Filename :";

            txtDocumentType.Text = "Name";
            txtDocumentDescription.Text = "Description";
            txtMyCompanyName.Text = Variables.Instance.CompanyName;
            txtDate.Text = DateTime.Now.ToShortDateString(); // CurrentVersion.ModifiedDate.ToShortDateString();
            txtID.Text = "Author ID"; // "Author Name";
            txtAuthors.Text = Variables.Instance.UserFullName; // "Author Name";
            txtFileName.Text = "New Diagram";
            txtVersion.Text = "Version";
            foreach (GoObject o in this)
            {
                o.Copyable = false; // DisableFrameObjectForEditing(o, true); <-wtf?
            }
        }

        public void Setup(RectangleF bounds)
        {
            Initializing = true;
            // Clear any existing items
            //Clear();

            if (frame == null)
            {
                InitTextBoxes(bounds);
            }
            Reposition();
            Initializing = false;
        }

        public void Update(DocumentInfo info)
        {
            txtAuthors.Text = info.Author;
            txtID.Text = info.AuthorID;
            txtDate.Text = info.Date.ToShortDateString();
            txtMyCompanyName.Text = info.OrganisationUnit;
            if (info.Description != null)
                txtDocumentDescription.Text = info.Description.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Trim();
            if (info.Name != null)
                txtDocumentType.Text = info.Name.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Trim();
            txtFileName.Text = info.FileName;
            if (info.Version != null)
                txtVersion.Text = info.Version;
        }

        public void Update(string Name, List<DocumentVersion> versions)
        {
            DocumentVersion CurrentVersion = versions[versions.Count - 1];
            if (CurrentVersion != null)
            {
                DocumentInfo dInfo = GetDocumentInfo();
                dInfo.Version = CurrentVersion.AppVersion;
                dInfo.Date = CurrentVersion.ModifiedDate;
                Update(dInfo);
            }
        }

        public void UpdateDate()
        {
            txtDate.Text = DateTime.Now.ToShortDateString();
        }

        // Protected Methods (1) 

        protected override void OnBoundsChanged(RectangleF old)
        {
            Reposition();
            base.OnBoundsChanged(old);
        }

        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj)
        {
            RemoveSelectionHandles(sel);
            return;

            this.Selectable = false;
            if (!CanResize())
            {
                sel.CreateBoundingHandle(this, selectedObj);
                return;
            }

            RectangleF rect = Bounds;

            float x2 = rect.X + (rect.Width / 2);
            float x3 = rect.X + rect.Width;

            float y2 = rect.Y + (rect.Height / 2);
            float y3 = rect.Y + rect.Height;

            // create the handles
            //sel.CreateResizeHandle(this, selectedObject, new PointF(x1, y1), TopLeft, true);
            //sel.CreateResizeHandle(this, selectedObject, new PointF(x3, y1), TopRight, true);
            sel.CreateResizeHandle(this, selectedObj, new PointF(x3, y3), BottomRight, true);
            //sel.CreateResizeHandle(this, selectedObject, new PointF(x1, y3), BottomLeft, true);
            if (CanReshape())
            {
                //sel.CreateResizeHandle(this, selectedObject, new PointF(x2, y1), MiddleTop, true);
                sel.CreateResizeHandle(this, selectedObj, new PointF(x3, y2), MiddleRight, true);
                sel.CreateResizeHandle(this, selectedObj, new PointF(x2, y3), MiddleBottom, true);
                //sel.CreateResizeHandle(this, selectedObject, new PointF(x1, y2), MiddleLeft, true);
            }
        }

        // Private Methods (1) 

        private static void DisableFrameObjectForEditing(GoObject obj)
        {
            obj.Reshapable = false;
            obj.Copyable = false;
            obj.Deletable = false;
            obj.Movable = false;
            obj.Printable = true;
        }

        #endregion Methods
    }
}