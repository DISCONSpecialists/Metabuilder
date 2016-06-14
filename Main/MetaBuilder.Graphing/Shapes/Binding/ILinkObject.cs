using System.Drawing;

namespace MetaBuilder.Graphing.Shapes
{
    public interface ILinkObject
    {
        /// <summary>
        /// Gets the point on the parent link where the line from this object connects
        /// with the link stroke.
        /// </summary>
        /// <seealso cref="Offset"/>
        /// <seealso cref="Segment"/>
        /// <seealso cref="SegmentPercentage"/>
        PointF ObjectConnectionPoint { get; }

        /// <summary>
        /// Gets or sets the offset of the center of this object relative to the
        /// <see cref="objectConnectionPoint"/>.
        /// </summary>
        /// <seealso cref="Segment"/>
        /// <seealso cref="SegmentPercentage"/>
        SizeF Offset { get; set; }

        /// <summary>
        /// Gets or sets the index of the segment where the <see cref="objectConnectionPoint"/> should be.
        /// </summary>
        /// <seealso cref="SegmentPercentage"/>
        /// <seealso cref="Offset"/>
        int Segment { get; set; }

        /// <summary>
        /// Gets or sets the percentage along the segment where the <see cref="objectConnectionPoint"/> should be.
        /// </summary>
        /// <seealso cref="Segment"/>
        /// <seealso cref="Offset"/>
        float SegmentPercentage { get; set; }

        Color ConnectionColor { get; set; }
    }
}