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

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MetaBuilder.Graphing.Formatting
{
    public class Gradient
    {
        #region Support for Common Fill operations

        /// <summary>
        /// Fills the specified rectangular area on the given graphics context.
        /// </summary>
        public void FillRectangle(Graphics g, Rectangle rect)
        {
            PaintGradientWithDirectionOverride(g, rect, gradientDir);
        }

        /// <summary>
        /// Fills the specified Region area on the given graphics context.
        /// </summary>
        public void FillRegion(Graphics g, Region rgn)
        {
            using (
                LinearGradientBrush brush = new LinearGradientBrush(rgn.GetBounds(g), startColor, endColor, gradientDir)
                )
            {
                brush.InterpolationColors = colorBlend;
                g.FillRegion(brush, rgn);
            }
        }

        /// <summary>
        /// Fills the specified GraphicsPath using a PathGradientBrush.
        /// </summary>
        public void FillPath(Graphics g, GraphicsPath path)
        {
            using (PathGradientBrush pgb = new PathGradientBrush(path))
            {
                pgb.InterpolationColors = colorBlend;
                g.FillPath(pgb, path);
            }
        }

        #endregion

        #region Brush Initializers

        /// <summary>
        /// Returns a LinearGradientBrush object initialized based on the current
        /// gradient configuration.
        /// </summary>
        /// <remarks>
        /// The user is responsible for desposing the returned Brush object.
        /// </remarks>
        public LinearGradientBrush GetLinearGradientBrush(Rectangle rect)
        {
            LinearGradientBrush brush = new LinearGradientBrush(rect, startColor, endColor, gradientDir);
            brush.InterpolationColors = colorBlend;
            return brush;
        }

        /// <summary>
        /// Returns a PathGradientBrush object initialized based on the current
        /// gradient configuration.
        /// </summary>
        /// <remarks>
        /// The user is responsible for desposing the returned Brush object.
        /// </remarks>
        public PathGradientBrush GetPathGradientBrush(GraphicsPath path)
        {
            PathGradientBrush pgb = new PathGradientBrush(path);
            pgb.InterpolationColors = colorBlend;
            return pgb;
        }

        #endregion

        #region Internal Functions

        public void PaintGradientWithDirectionOverride(Graphics g, Rectangle rect, LinearGradientMode mode)
        {
            int colorStepsCount = colorBlend.Colors.Length;
            if ((startColor == endColor) && (colorStepsCount == 2))
                using (SolidBrush solidBrush = new SolidBrush(startColor))
                    g.FillRectangle(solidBrush, rect);
            else
                using (LinearGradientBrush brush = new LinearGradientBrush(rect, startColor, endColor, mode))
                {
                    if (colorBlend != null && colorBlend.Colors.Length > 0)
                    {
                        // Paranoid fix for position values
                        colorBlend.Positions[0] = 0.0f;
                        colorBlend.Positions[colorBlend.Positions.Length - 1] = 1.0f;
                        brush.InterpolationColors = colorBlend;
                    }
                    g.FillRectangle(brush, rect);
                }
        }

        #endregion

        #region Public Properties

        private ColorBlend colorBlend;
        private Color endColor;

        private LinearGradientMode gradientDir = LinearGradientMode.Vertical;
        private Color startColor;

        /// <summary>
        /// Gets or sets the color blend of the gradient.
        /// </summary>
        [DefaultValue(null)]
        public ColorBlend ColorBlend
        {
            get { return colorBlend; }
            set
            {
                colorBlend = value;
                startColor = value.Colors[0];
                endColor = value.Colors[value.Colors.Length - 1];
            }
        }

        [DefaultValue(LinearGradientMode.Vertical)]
        /// <summary>
            /// Gets or sets the gradient direction.
            /// </summary>
            public LinearGradientMode GradientDirection
        {
            get { return gradientDir; }
            set { gradientDir = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a Graient with both start and end colors set to transparent.
        /// </summary>
        public Gradient() : this(Color.Transparent, Color.Transparent)
        {
        }

        /// <summary>
        /// Constructs a new Gradient with the specified start and end colors.
        /// </summary>
        public Gradient(Color startColor, Color endColor)
        {
            this.startColor = startColor;
            this.endColor = endColor;
            ColorBlend blend = new ColorBlend(2);
            blend.Colors[0] = startColor;
            blend.Colors[1] = endColor;
            blend.Positions = new float[] {0.0f, 1.0f};
            colorBlend = blend;
        }

        #endregion
    }
}