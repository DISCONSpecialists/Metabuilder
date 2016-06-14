using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MetaBuilder.Meta.Editors
{
    public class RichTextBoxEx : RichTextBox
    {

        #region BULLETING

        [StructLayout(LayoutKind.Sequential)]
        private class PARAFORMAT2
        {
            public int cbSize;
            public int dwMask;
            public short wNumbering;
            public short wReserved;
            public int dxStartIndent;
            public int dxRightIndent;
            public int dxOffset;
            public short wAlignment;
            public short cTabCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
            public int[] rgxTabs;

            public int dySpaceBefore; // Vertical spacing before para
            public int dySpaceAfter; // Vertical spacing after para
            public int dyLineSpacing; // Line spacing depending on Rule
            public short sStyle; // Style handle
            public byte bLineSpacingRule; // Rule for line spacing (see tom.doc)
            public byte bOutlineLevel; // Outline Level
            public short wShadingWeight; // Shading in hundredths of a per cent
            public short wShadingStyle; // Byte 0: style, nib 2: cfpat, 3: cbpat
            public short wNumberingStart; // Starting value for numbering
            public short wNumberingStyle; // Alignment, Roman/Arabic, (), ), ., etc.
            public short wNumberingTab; // Space bet 1st indent and 1st-line text
            public short wBorderSpace; // Border-text spaces (nbl/bdr in pts)
            public short wBorderWidth; // Pen widths (nbl/bdr in half twips)
            public short wBorders; // Border styles (nibble/border)

            public PARAFORMAT2()
            {
                this.cbSize = Marshal.SizeOf(typeof(PARAFORMAT2));
            }
        }

        #region PARAFORMAT MASK VALUES
        // PARAFORMAT mask values
        private const uint PFM_STARTINDENT = 0x00000001;
        private const uint PFM_RIGHTINDENT = 0x00000002;
        private const uint PFM_OFFSET = 0x00000004;
        private const uint PFM_ALIGNMENT = 0x00000008;
        private const uint PFM_TABSTOPS = 0x00000010;
        private const uint PFM_NUMBERING = 0x00000020;
        private const uint PFM_OFFSETINDENT = 0x80000000;

        // PARAFORMAT 2.0 masks and effects
        private const uint PFM_SPACEBEFORE = 0x00000040;
        private const uint PFM_SPACEAFTER = 0x00000080;
        private const uint PFM_LINESPACING = 0x00000100;
        private const uint PFM_STYLE = 0x00000400;
        private const uint PFM_BORDER = 0x00000800; // (*)
        private const uint PFM_SHADING = 0x00001000; // (*)
        private const uint PFM_NUMBERINGSTYLE = 0x00002000; // RE 3.0
        private const uint PFM_NUMBERINGTAB = 0x00004000; // RE 3.0
        private const uint PFM_NUMBERINGSTART = 0x00008000; // RE 3.0

        private const uint PFM_RTLPARA = 0x00010000;
        private const uint PFM_KEEP = 0x00020000; // (*)
        private const uint PFM_KEEPNEXT = 0x00040000; // (*)
        private const uint PFM_PAGEBREAKBEFORE = 0x00080000; // (*)
        private const uint PFM_NOLINENUMBER = 0x00100000; // (*)
        private const uint PFM_NOWIDOWCONTROL = 0x00200000; // (*)
        private const uint PFM_DONOTHYPHEN = 0x00400000; // (*)
        private const uint PFM_SIDEBYSIDE = 0x00800000; // (*)
        private const uint PFM_TABLE = 0x40000000; // RE 3.0
        private const uint PFM_TEXTWRAPPINGBREAK = 0x20000000; // RE 3.0
        private const uint PFM_TABLEROWDELIMITER = 0x10000000; // RE 4.0

        // The following three properties are read only
        private const uint PFM_COLLAPSED = 0x01000000; // RE 3.0
        private const uint PFM_OUTLINELEVEL = 0x02000000; // RE 3.0
        private const uint PFM_BOX = 0x04000000; // RE 3.0
        private const uint PFM_RESERVED2 = 0x08000000; // RE 4.0

        public enum AdvRichTextBulletType
        {
            Normal = 1,
            Number = 2,
            LowerCaseLetter = 3,
            UpperCaseLetter = 4,
            LowerCaseRoman = 5,
            UpperCaseRoman = 6
        }

        public enum AdvRichTextBulletStyle
        {
            RightParenthesis = 0x000,
            DoubleParenthesis = 0x100,
            Period = 0x200,
            Plain = 0x300,
            NoNumber = 0x400
        }
        #endregion

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [In, Out, MarshalAs(UnmanagedType.LPStruct)] PARAFORMAT2 lParam);

        private AdvRichTextBulletType _BulletType = AdvRichTextBulletType.Number;
        private AdvRichTextBulletStyle _BulletStyle = AdvRichTextBulletStyle.Period;
        private short _BulletNumberStart = 1;


        public AdvRichTextBulletType BulletType
        {
            get { return _BulletType; }
            set
            {
                _BulletType = value;
                NumberedBullet(true);
            }
        }
        public AdvRichTextBulletStyle BulletStyle
        {
            get { return _BulletStyle; }
            set
            {
                _BulletStyle = value;
                NumberedBullet(true);
            }
        }
        public void NumberedBullet(bool TurnOn)
        {
            PARAFORMAT2 paraformat1 = new PARAFORMAT2();
            paraformat1.dwMask = (int)(PFM_NUMBERING | PFM_OFFSET | PFM_NUMBERINGSTART | PFM_NUMBERINGSTYLE | PFM_NUMBERINGTAB);
            if (!TurnOn)
            {
                paraformat1.wNumbering = 0;
                paraformat1.dxOffset = 0;
            }
            else
            {
                paraformat1.wNumbering = (short)_BulletType;
                paraformat1.dxOffset = this.BulletIndent;
                paraformat1.wNumberingStyle = (short)_BulletStyle;
                paraformat1.wNumberingStart = _BulletNumberStart;
                paraformat1.wNumberingTab = 500;
            }
            SendMessage(new HandleRef(this, this.Handle), 0x447, 0, paraformat1);
        }

        #endregion

        #region Printing
        //Convert the unit used by the .NET framework (1/100 inch) 
        //and the unit used by Win32 API calls (twips 1/1440 inch)
        private const double anInch = 14.4;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CHARRANGE
        {
            public int cpMin;         //First character of range (0 for start of doc)
            public int cpMax;           //Last character of range (-1 for end of doc)
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct FORMATRANGE
        {
            public IntPtr hdc;             //Actual DC to draw on
            public IntPtr hdcTarget;       //Target DC for determining text formatting
            public RECT rc;                //Region of the DC to draw to (in twips)
            public RECT rcPage;            //Region of the whole DC (page size) (in twips)
            public CHARRANGE chrg;         //Range of text to draw (see earlier declaration)
        }

        private const int WM_USER = 0x0400;
        private const int EM_FORMATRANGE = WM_USER + 57;

        [DllImport("USER32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        // Render the contents of the RichTextBox for printing
        //	Return the last character printed + 1 (printing start from this point for next page)
        public int Print(int charFrom, int charTo, PrintPageEventArgs e)
        {
            //Calculate the area to render and print
            RECT rectToPrint;
            rectToPrint.Top = (int)(e.MarginBounds.Top * anInch);
            rectToPrint.Bottom = (int)(e.MarginBounds.Bottom * anInch);
            rectToPrint.Left = (int)(e.MarginBounds.Left * anInch);
            rectToPrint.Right = (int)(e.MarginBounds.Right * anInch);

            //Calculate the size of the page
            RECT rectPage;
            rectPage.Top = (int)(e.PageBounds.Top * anInch);
            rectPage.Bottom = (int)(e.PageBounds.Bottom * anInch);
            rectPage.Left = (int)(e.PageBounds.Left * anInch);
            rectPage.Right = (int)(e.PageBounds.Right * anInch);

            IntPtr hdc = e.Graphics.GetHdc();

            FORMATRANGE fmtRange;
            fmtRange.chrg.cpMax = charTo;				//Indicate character from to character to 
            fmtRange.chrg.cpMin = charFrom;
            fmtRange.hdc = hdc;                    //Use the same DC for measuring and rendering
            fmtRange.hdcTarget = hdc;              //Point at printer hDC
            fmtRange.rc = rectToPrint;             //Indicate the area on page to print
            fmtRange.rcPage = rectPage;            //Indicate size of page

            IntPtr res = IntPtr.Zero;

            IntPtr wparam = IntPtr.Zero;
            wparam = new IntPtr(1);

            //Get the pointer to the FORMATRANGE structure in memory
            IntPtr lparam = IntPtr.Zero;
            lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange));
            Marshal.StructureToPtr(fmtRange, lparam, false);

            //Send the rendered data for printing 
            res = SendMessage(Handle, EM_FORMATRANGE, wparam, lparam);

            //Free the block of memory allocated
            Marshal.FreeCoTaskMem(lparam);

            //Release the device context handle obtained by a previous call
            e.Graphics.ReleaseHdc(hdc);

            //Return last + 1 character printer
            return res.ToInt32();
        }



        private PrintDocument _PrintDocument;

        public PrintDocument PrintDocument
        {
            get { return _PrintDocument; }
            set
            {
                _PrintDocument = value;
                if (_PrintDocument != null)
                {
                    _PrintDocument.BeginPrint += new PrintEventHandler(_PrintDocument_BeginPrint);
                    _PrintDocument.PrintPage += new PrintPageEventHandler(_PrintDocument_PrintPage);
                }
            }
        }

        void _PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Print the content of RichTextBox. Store the last character printed.
            checkPrint = this.Print(checkPrint, this.TextLength, e);

            // Check for more pages
            if (checkPrint < this.TextLength)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
        }

        void _PrintDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            checkPrint = 0;
        }


        int checkPrint = 0;

        #endregion

        #region Search
        int SearchIndex = 0;
        public void Search(string SearchText)
        {
            //MDI form search term always holds precedence than the local search term 
            //as the user might be interested in searchning same term in more than one windows...

            SearchIndex = (SearchIndex >= 0 ? SearchIndex : 0);

            if (SearchIndex >= Text.Length)
                MessageBox.Show(this, "Reached Page End \nUnable to find Specified Text \"" + SearchText + "\"");

            if (this.Text.Length > 0 && SearchText.Length > 0)
            {
                this.Focus();
                SearchIndex = this.Find(SearchText, SearchIndex, RichTextBoxFinds.None);
                if (SearchIndex == -1)
                    MessageBox.Show(this, "Reached Page End \nUnable to find Specified Text \"" + SearchText + "\"");
                else
                    SearchIndex += SearchText.Length;
            }

        }


        public void Lock()
        {
            this.ReadOnly = true;
            this.BackColor = SystemColors.Info;
        }

        public void Unlock()
        {
            this.ReadOnly = false;
            this.BackColor = SystemColors.Window;

        }


        #endregion



    }


}
