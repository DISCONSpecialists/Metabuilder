#region - Terms of Usage -
/*
 * Copyright 2006 Sameera Perera
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using MetaBuilder.Graphing.Formatting;

namespace MetaBuilder.UIControls.GraphingUI.Formatting
{

    #region : The Marker :
    public class Marker
    {
        public event EventHandler MarkerUpdated;
        private RectangleF bounds;

        public RectangleF Bounds
        {
            get { return bounds; }
            set
            {
                if (IsMovable)
                    bounds = value;
            }
        }

        private Color color;

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                if (MarkerUpdated != null)
                    MarkerUpdated(this, EventArgs.Empty);
            }
        }

        private float position;

        public float Position
        {
            get { return position; }
            set
            {
                position = value;
                if (MarkerUpdated != null)
                    MarkerUpdated(this, EventArgs.Empty);
            }
        }

        public readonly bool IsMovable;
        private const int MARKER_SIZE = 10;
        // Set the Position value without causing the MarkerUpdated event to fire
        internal void SetPositionSilently(float position)
        {
            this.position = position;
        }

        // Update the marker's bounds based on the marker strip's bounds.
        public void UpdateBounds(Rectangle markerStripBounds)
        {
            float xOffset = markerStripBounds.Width * Position + markerStripBounds.X;
            float adjustment = MARKER_SIZE / 2;
            if (Position < 1.0) xOffset -= adjustment;
            else xOffset += adjustment;
            bounds = new RectangleF(
                new PointF(xOffset, markerStripBounds.Top),
                new SizeF(MARKER_SIZE, MARKER_SIZE));
        }

        public Marker(float position, Color color)
            : this(position, color, true)
        {
        }

        public Marker(float position)
            : this(position, Color.White, true)
        {
        }

        public Marker(float position, Color color, bool isMovable)
        {
            Position = position;
            Color = color;
            IsMovable = isMovable;
        }
    }
    #endregion

    internal class GradientBuilder : Control
    {
        #region Memeber Variables
        private Rectangle gradBarRect;
        private Rectangle markerStrip;
        private Rectangle compansatedMarkerStrip;
        private List<Marker> markers;
        private const int DEFAULT_PADDING = 10;
        private Marker selectedMarker = null;
        private Marker movingMarker = null;
        private Bitmap gradientBmp = null;
        private bool isBitmapInvalid = true;
        #endregion

        #region Public Memembers
        public event EventHandler MarkerSelected;
        public event EventHandler GradientChanged;
        private Gradient gradient;

        [DefaultValue(null), Browsable(false)]
        public Gradient Gradient
        {
            get { return gradient; }
            set
            {
                gradient = value;
                // Create markers from the gradient
                ColorBlend blend = gradient.ColorBlend;
                markers.Clear();
                Marker marker = new Marker(0.0f, blend.Colors[0], false);
                marker.MarkerUpdated += new EventHandler(marker_MarkerUpdated);
                markers.Add(marker);
                for (int i = 1; i < blend.Colors.Length - 1; i++)
                {
                    marker = new Marker(blend.Positions[i], blend.Colors[i]);
                    marker.MarkerUpdated += new EventHandler(marker_MarkerUpdated);
                    markers.Add(marker);
                }
                marker = new Marker(1.0f, blend.Colors[blend.Colors.Length - 1], false);
                marker.MarkerUpdated += new EventHandler(marker_MarkerUpdated);
                markers.Add(marker);
                Invalidate();
                OnGradientChanged(false);
            }
        }

        private bool silentMarkers = false;

        /// <summary>
        /// Gets or sets whether the GradientChanged event is fired when a marker
        /// is dragged.
        /// </summary>
        [DefaultValue(false)]
        public bool SilentMarkers
        {
            get { return silentMarkers; }
            set { silentMarkers = value; }
        }
        #endregion

        #region Calculating
        private void Recalculate()
        {
            gradBarRect = new Rectangle(DEFAULT_PADDING, DEFAULT_PADDING,
                                        ClientRectangle.Width - DEFAULT_PADDING * 2,
                                        ClientRectangle.Height - DEFAULT_PADDING * 3);
            markerStrip = new Rectangle(DEFAULT_PADDING, gradBarRect.Bottom + 1,
                                        gradBarRect.Width,
                                        DEFAULT_PADDING + 2);
            compansatedMarkerStrip = new Rectangle(
                markerStrip.X, markerStrip.Y,
                markerStrip.Width - DEFAULT_PADDING,
                markerStrip.Height);
            foreach (Marker mrk in markers)
                mrk.UpdateBounds(compansatedMarkerStrip);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Recalculate();
        }
        #endregion

        #region Painting
        protected virtual void OnGradientChanged(bool suppressEvent)
        {
            isBitmapInvalid = true;
            gradient.ColorBlend = GetBlendFromMarkers();
            Invalidate();
            if (!suppressEvent && GradientChanged != null)
                GradientChanged(Parent, EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;
            // Determine what to draw. Don't rebuild the gradient when we have a 
            // cahced bitmap, or when there is a marker being moved.
            bool suppressBmp = (movingMarker != null);
            // Draw the markers
            DrawMarkers(g);
            // Paint the checkerboard style background 
            using (HatchBrush hb = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.White, Color.LightGray))
                g.FillRectangle(hb, gradBarRect);
            // If we are rebuilding the gradient, get the appropriate color blend
            if (!isBitmapInvalid)
                g.DrawImage(gradientBmp, gradBarRect);
            else if (suppressBmp)
            {
                // Simply paint the gradient when a marker is being moved. No use
                // painting it on to a bitmap just yet.
                gradient.PaintGradientWithDirectionOverride(
                    g, gradBarRect, LinearGradientMode.Horizontal);
            }
            else
            {
                if (gradientBmp == null)
                    gradientBmp = new Bitmap(gradBarRect.Width,
                                             gradBarRect.Height,
                                             PixelFormat.Format32bppArgb);
                // Cache the gradient by painting it onto a bitmap
                Graphics gBmp = Graphics.FromImage(gradientBmp);
                Rectangle bmpRct = new Rectangle(0, 0, gradBarRect.Width, gradBarRect.Height);
                gradient.PaintGradientWithDirectionOverride(
                    gBmp, bmpRct,
                    LinearGradientMode.Horizontal);
                g.DrawImage(gradientBmp, gradBarRect);
            }
            // Draw the border around the gradient
            g.DrawRectangle(Pens.Black, gradBarRect);
        }

        private void DrawMarkers(Graphics g)
        {
            foreach (Marker mrk in markers)
            {
                mrk.UpdateBounds(compansatedMarkerStrip); //using ( SolidBrush br = new SolidBrush( mrk.Color ) )
                if (mrk.Equals(selectedMarker))
                    g.FillRectangle(Brushes.Black, mrk.Bounds);
                else
                    g.FillRectangle(Brushes.White, mrk.Bounds);
                g.DrawRectangle(Pens.Black,
                                mrk.Bounds.X,
                                mrk.Bounds.Y,
                                mrk.Bounds.Width,
                                mrk.Bounds.Height);
            }
        }

        private ColorBlend GetBlendFromMarkers()
        {
            Color[] colors = new Color[markers.Count];
            float[] positions = new float[markers.Count];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = markers[i].Color;
                positions[i] = markers[i].Position;
            }
            ColorBlend blend = new ColorBlend(markers.Count);
            blend.Colors = colors;
            blend.Positions = positions;
            return blend;
        }
        #endregion

        #region Marker Management
        protected virtual void OnMarkerSelected(Marker selectedMarker)
        {
            this.selectedMarker = selectedMarker;
            Rectangle invalidRect = markerStrip;
            invalidRect.Inflate((int)selectedMarker.Bounds.Width, 0);
            Invalidate(invalidRect);
            if (MarkerSelected != null)
                MarkerSelected(selectedMarker, EventArgs.Empty);
        }

        #region Marker Movement
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Can a marker (other than the first or the last) receive this 
            // mouse down event?
            for (int i = 1; i < markers.Count - 1; i++)
                if (markers[i].Bounds.Contains(e.X, e.Y))
                {
                    movingMarker = markers[i];
                    return;
                }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Move the marker along the marker strip. If the marker is dragged out
            // of the marker strip then the marker is removed.
            if (markerStrip.Contains(e.X, e.Y) && movingMarker != null)
            {
                float a = markerStrip.Width;
                movingMarker.SetPositionSilently(e.X / a);
                OnGradientChanged(silentMarkers);
            }
            else if (movingMarker != null) // Remove the marker
            {
                markers.Remove(movingMarker);
                movingMarker = null;
                OnGradientChanged(false);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (movingMarker != null) // A movement was done. Resort the array
            {
                markers.Sort(CompareMarkers);
                Invalidate(gradBarRect);
                selectedMarker = movingMarker;
                OnMarkerSelected(movingMarker);
                // Notify that we have a new gradient
                OnGradientChanged(false);
            }
            else if (markers[0].Bounds.Contains(e.X, e.Y))
                OnMarkerSelected(markers[0]);
            else if (markers[markers.Count - 1].Bounds.Contains(e.X, e.Y))
                OnMarkerSelected(markers[markers.Count - 1]);
            movingMarker = null;
        }

        private static int CompareMarkers(Marker a, Marker b)
        {
            return a.Position.CompareTo(b.Position);
        }
        #endregion

        private void marker_MarkerUpdated(object sender, EventArgs e)
        {
            OnGradientChanged(false);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            return;
            // If an location on the marker strip is double clicked where there
            // are no existing markers, a new marker is added.
            /*Point pt = new Point(e.X, e.Y);

            if (markerStrip.Contains(pt))
            {
                int i = 0;
                for (; i < markers.Count; i++)
                    if (markers[i].Bounds.Contains(pt))
                        return;
                    else if (markers[i].Bounds.X > pt.X)
                        break;

                float a, b;
                a = (float)e.X - DEFAULT_PADDING;
                b = (float)markerStrip.Width;

                Marker newMarker = new Marker(a / b);
                newMarker.UpdateBounds(compansatedMarkerStrip);

                // Select the color for the new marker. It should be the current 
                // color of the gradient at the marker point.
                if (this.gradientBmp != null)
                    newMarker.Color = gradientBmp.GetPixel(
                                            (int)newMarker.Bounds.X, 0);

                newMarker.MarkerUpdated += new EventHandler(marker_MarkerUpdated);
                markers.Insert(i, newMarker);

                // Notify that we have a new gradient
                OnGradientChanged(false);
                OnMarkerSelected(newMarker);
            }*/
        }
        #endregion

        #region Construction
        public Marker GetMarker(int index)
        {
            return markers[index];
        }

        public GradientBuilder()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            gradient = new Gradient();
            gradient.GradientDirection = LinearGradientMode.Horizontal;
            markers = new List<Marker>(2);
            Marker startMarker, endMarker;
            startMarker = new Marker(0.0f, Color.Black, false);
            endMarker = new Marker(1.0f, Color.White, false);
            markers.Add(startMarker);
            markers.Add(endMarker);
            startMarker.MarkerUpdated += new EventHandler(marker_MarkerUpdated);
            endMarker.MarkerUpdated += new EventHandler(marker_MarkerUpdated);
        }
        #endregion
    }
}