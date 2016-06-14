using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.UIControls.GraphingUI.Formatting.FormatUndo;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.Formatting
{
    partial class Main
    {
        #region Anonymous Methods
        private void ApplyUndoableSettings(string description, FormattingContainer container)
        {
            FormatEditCommand fec = new FormatEditCommand();
            fec.Collection = cmd.Collection;
            if (myView.Document.UndoManager.CurrentEdit == null)
                myView.Document.UndoManager.CurrentEdit = new GoUndoManagerCompoundEdit();
            myView.Document.UndoManager.CurrentEdit.AddEdit(fec);
            fec.ApplySettings(container);
        }

        private void StoreTextSettings(FormattingContainer formattingContainer)
        {
            formattingContainer.TextSettings.Bold = comboFontStyle.Text.Contains("Bold");
            formattingContainer.TextSettings.Italic = comboFontStyle.Text.Contains("Italic");
            formattingContainer.TextSettings.Underline = cbUnderline.Checked;
            formattingContainer.TextSettings.StrikeThrough = cbStrikeThrough.Checked;
            formattingContainer.TextSettings.TextColour = colorPickerText.SelectedColor;
            formattingContainer.TextSettings.Multiline = cbMultiline.Checked;
            float fontSize = 0;
            bool fontSizeParsed = float.TryParse(comboFontSize.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out fontSize);
            if (fontSizeParsed)
                formattingContainer.TextSettings.FontSize = fontSize;
            formattingContainer.TextSettings.WrappingWidth = float.Parse(txtWrapTextAt.Text, System.Globalization.CultureInfo.InvariantCulture);
            formattingContainer.TextSettings.Editable = !cbLockText.Checked;
            float f = (comboFontSize.SelectedItem != null) ? float.Parse(comboFontSize.Text, System.Globalization.CultureInfo.InvariantCulture) : 12f;
            formattingContainer.TextSettings.Font = new Font(listFonts.Text, f);
        }

        private void SetupMethods()
        {

            cbUnderline.CheckedChanged += delegate(object sender, EventArgs e)
                                              {
                                                  FormattingContainer formattingContainer = new FormattingContainer();
                                                  formattingContainer.TextSettings.Underline = cbUnderline.Checked;
                                                  ApplyUndoableSettings("Formatting: Underline", formattingContainer);
                                              };
            cbStrikeThrough.CheckedChanged += delegate(object sender, EventArgs e)
                                                  {
                                                      FormattingContainer formattingContainer =
                                                          new FormattingContainer();
                                                      StoreTextSettings(formattingContainer);
                                                      ApplyUndoableSettings("Formatting: StrikeThrough",
                                                                            formattingContainer);
                                                  };
            comboFontStyle.SelectedIndexChanged += delegate(object sender, EventArgs e)
                                                       {
                                                           FormattingContainer formattingContainer =
                                                               new FormattingContainer();
                                                           StoreTextSettings(formattingContainer);
                                                           ApplyUndoableSettings("Formatting: Font Style",
                                                                                 formattingContainer);
                                                       };
            comboFontStyle.TextChanged += delegate(object sender, EventArgs e)
                                                       {
                                                           FormattingContainer formattingContainer =
                                                               new FormattingContainer();
                                                           StoreTextSettings(formattingContainer);
                                                           ApplyUndoableSettings("Formatting: Font Style",
                                                                                 formattingContainer);
                                                       };
            colorPickerText.ColorSelected += delegate(object sender, EventArgs e)
                                                 {
                                                     FormattingContainer formattingContainer = new FormattingContainer();
                                                     StoreTextSettings(formattingContainer);
                                                     ApplyUndoableSettings("Formatting: Font Colour", formattingContainer);
                                                 };
            cbMultiline.CheckedChanged += delegate(object sender, EventArgs e)
                                              {
                                                  FormattingContainer formattingContainer = new FormattingContainer();
                                                  StoreTextSettings(formattingContainer);
                                                  ApplyUndoableSettings("Formatting: Multiline", formattingContainer);
                                              };
            comboFontSize.SelectedIndexChanged += delegate(object sender, EventArgs e)
                                                      {
                                                          FormattingContainer formattingContainer =
                                                              new FormattingContainer();
                                                          StoreTextSettings(formattingContainer);
                                                          ApplyUndoableSettings("Formatting: Font Size",
                                                                                formattingContainer);
                                                      };
            txtWrapTextAt.TextChanged += delegate(object sender, EventArgs e)
                                             {
                                                 FormattingContainer formattingContainer = new FormattingContainer();
                                                 StoreTextSettings(formattingContainer);

                                                 ApplyUndoableSettings("Formatting: Wrapping Width", formattingContainer);
                                             };
            cbWrapping.CheckedChanged += delegate(object sender, EventArgs e)
                                             {
                                                 FormattingContainer formattingContainer = new FormattingContainer();
                                                 StoreTextSettings(formattingContainer);
                                                 ApplyUndoableSettings("Formatting: Wrapping", formattingContainer);
                                             };
            comboPenWidth.SelectedIndexChanged += delegate(object sender, EventArgs e)
                                                      {
                                                          FormattingContainer formattingContainer =
                                                              new FormattingContainer();
                                                          StoreTextSettings(formattingContainer);
                                                          ApplyUndoableSettings("Formatting: Pen Width",
                                                                                formattingContainer);
                                                      };
            comboPenStyle.SelectedIndexChanged += delegate(object sender, EventArgs e)
                                                      {
                                                          FormattingContainer formattingContainer =
                                                              new FormattingContainer();
                                                          formattingContainer.ShapeSettings.DashStyle =
                                                              (DashStyle)
                                                              Enum.Parse(typeof(DashStyle), comboPenStyle.Text);
                                                          ApplyUndoableSettings("Formatting: Dash Style",
                                                                                formattingContainer);
                                                      };
            comboPenStartCap.SelectedIndexChanged += delegate(object sender, EventArgs e)
                                                         {
                                                             FormattingContainer formattingContainer =
                                                                 new FormattingContainer();
                                                             formattingContainer.ShapeSettings.PenStartCap =
                                                                 (LineCap)
                                                                 Enum.Parse(typeof(LineCap), comboPenStartCap.Text);
                                                             ApplyUndoableSettings("Formatting: Pen Start Cap",
                                                                                   formattingContainer);
                                                         };
            comboPenEndCap.SelectedIndexChanged += delegate(object sender, EventArgs e)
                                                       {
                                                           FormattingContainer formattingContainer =
                                                               new FormattingContainer();
                                                           formattingContainer.ShapeSettings.PenEndCap =
                                                               (LineCap)
                                                               Enum.Parse(typeof(LineCap), comboPenEndCap.Text);
                                                           ApplyUndoableSettings("Formatting: Pen Start Cap",
                                                                                 formattingContainer);
                                                       };
            colorPickerPen.ColorSelected += delegate(object sender, EventArgs e)
                                                {
                                                    FormattingContainer formattingContainer = new FormattingContainer();
                                                    formattingContainer.ShapeSettings.PenColour =
                                                        colorPickerPen.SelectedColor;
                                                    ApplyUndoableSettings("Formatting: Pen Colour", formattingContainer);
                                                };
            txtCornerY.TextChanged += delegate(object sender, EventArgs e)
                                          {
                                              FormattingContainer formattingContainer = new FormattingContainer();
                                              formattingContainer.ShapeSettings.Corner =
                                                  new SizeF(float.Parse(txtCornerX.Text, System.Globalization.CultureInfo.InvariantCulture), float.Parse(txtCornerY.Text, System.Globalization.CultureInfo.InvariantCulture));
                                              ApplyUndoableSettings("Formatting: Corner Size", formattingContainer);
                                          };
            txtCornerX.TextChanged += delegate(object sender, EventArgs e)
                                          {
                                              FormattingContainer formattingContainer = new FormattingContainer();
                                              formattingContainer.ShapeSettings.Corner =
                                                  new SizeF(float.Parse(txtCornerX.Text, System.Globalization.CultureInfo.InvariantCulture), float.Parse(txtCornerY.Text, System.Globalization.CultureInfo.InvariantCulture));
                                              ApplyUndoableSettings("Formatting: Corner Size", formattingContainer);
                                          };
            comboGradientType.SelectedIndexChanged += delegate(object sender, EventArgs e)
                                                          {
                                                              FormattingContainer formattingContainer =
                                                                  new FormattingContainer();
                                                              formattingContainer.ShapeSettings.GradientType =
                                                                  (GradientType)
                                                                  Enum.Parse(typeof(GradientType),
                                                                             comboGradientType.SelectedItem.ToString());
                                                              ApplyUndoableSettings("Formatting: Gradient Type",
                                                                                    formattingContainer);
                                                          };
            gradEditor.GradientChanged += delegate(object sender, EventArgs e)
                                              {
                                                  pnlCanvas.Invalidate();
                                                  radioGradientFill_CheckedChanged(sender, e);
                                              };
            radioGradientFill.CheckedChanged += new EventHandler(radioGradientFill_CheckedChanged);
            radioSolidFill.CheckedChanged += new EventHandler(radioGradientFill_CheckedChanged);
            colorPickerSolidColour.ColorSelected += new EventHandler(radioGradientFill_CheckedChanged);
            cbSelectable.CheckedChanged += delegate(object sender, EventArgs e)
                                               {
                                                   FormattingContainer formattingContainer = new FormattingContainer();
                                                   formattingContainer.GeneralSettings.Selectable = cbSelectable.Checked;
                                                   ApplyUndoableSettings("Formatting: Selectable", formattingContainer);
                                               };
            cbCannotDelete.CheckedChanged += delegate(object sender, EventArgs e)
                                                 {
                                                     FormattingContainer formattingContainer = new FormattingContainer();
                                                     formattingContainer.GeneralSettings.Deletable =
                                                         !cbCannotDelete.Checked;
                                                     ApplyUndoableSettings("Formatting: Deletable", formattingContainer);
                                                 };
            cbLockText.CheckedChanged += delegate(object sender, EventArgs e)
                                             {
                                                 FormattingContainer formattingContainer = new FormattingContainer();
                                                 StoreTextSettings(formattingContainer);
                                                 ApplyUndoableSettings("Formatting: Editable", formattingContainer);
                                             };
            cbVisible.CheckedChanged += delegate(object sender, EventArgs e)
                                            {
                                                FormattingContainer formattingContainer = new FormattingContainer();
                                                formattingContainer.GeneralSettings.Visible = cbVisible.Checked;
                                                ApplyUndoableSettings("Formatting: Visible", formattingContainer);
                                            };
            cbResizesRealtime.CheckedChanged += delegate(object sender, EventArgs e)
                                                    {
                                                        FormattingContainer formattingContainer =
                                                            new FormattingContainer();
                                                        formattingContainer.GeneralSettings.ResizesRealtime =
                                                            cbResizesRealtime.Checked;
                                                        ApplyUndoableSettings("Formatting: ResizesRealtime",
                                                                              formattingContainer);
                                                    };
            cbPrintable.CheckedChanged += delegate(object sender, EventArgs e)
                                              {
                                                  FormattingContainer formattingContainer = new FormattingContainer();
                                                  formattingContainer.GeneralSettings.Printable = cbPrintable.Checked;
                                                  ApplyUndoableSettings("Formatting: Printable", formattingContainer);
                                              };
            cbAutoRescale.CheckedChanged += delegate(object sender, EventArgs e)
                                                {
                                                    FormattingContainer formattingContainer = new FormattingContainer();
                                                    formattingContainer.GeneralSettings.AutoRescales =
                                                        cbAutoRescale.Checked;
                                                    ApplyUndoableSettings("Formatting: AutoRescales",
                                                                          formattingContainer);
                                                };
            cbResizable.CheckedChanged += delegate(object sender, EventArgs e)
                                              {
                                                  FormattingContainer formattingContainer = new FormattingContainer();
                                                  formattingContainer.GeneralSettings.Resizable = cbResizable.Checked;
                                                  ApplyUndoableSettings("Formatting: Resizable", formattingContainer);
                                              };
            listFonts.SelectedIndexChanged += delegate(object sender, EventArgs e)
                                                  {
                                                      FormattingContainer formattingContainer =
                                                          new FormattingContainer();

                                                      StoreTextSettings(formattingContainer);
                                                      ApplyUndoableSettings("Formatting: Font", formattingContainer);
                                                  };
            comboFontSize.SelectedIndexChanged += delegate(object sender, EventArgs e)
                                                      {
                                                          FormattingContainer formattingContainer =
                                                              new FormattingContainer();
                                                          StoreTextSettings(formattingContainer);

                                                          ApplyUndoableSettings("Formatting: Font", formattingContainer);
                                                      };
        }

        private void radioGradientFill_CheckedChanged(object sender, EventArgs e)
        {
            FormattingContainer formattingContainer = new FormattingContainer();
            if (radioGradientFill.Checked)
            {
                formattingContainer.ShapeSettings.GradientType = (GradientType)Enum.Parse(typeof(GradientType), comboGradientType.SelectedItem.ToString());
                formattingContainer.ShapeSettings.GradientStartColour = gradEditor.GetMarker(0).Color;
                formattingContainer.ShapeSettings.GradientEndColour = gradEditor.GetMarker(1).Color;
                formattingContainer.ShapeSettings.IsGradient = true;
            }
            else
            {
                formattingContainer.ShapeSettings.IsGradient = false;
                formattingContainer.ShapeSettings.FillColour = colorPickerSolidColour.SelectedColor;
            }
            ApplyUndoableSettings("Formatting: Fill", formattingContainer);
        }
        #endregion
    }
}