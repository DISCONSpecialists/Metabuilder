using System;
using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Primitives
{
    [Serializable]
    public class GradientValueChainStep : GradientPolygon
    {
        private float _nose;
        private float _tail;

        public GradientValueChainStep(float nose, float tail, float width, float height, bool reshapable)
            : base(GoPolygonStyle.Line)
        {
            _nose = nose;
            _tail = tail;
            DrawStep(width, height, reshapable);
        }

        public float Nose
        {
            get { return _nose; }
            set
            {
                _nose = value;
                DoRedraw();
            }
        }

        public float Tail
        {
            get { return _tail; }
            set
            {
                _tail = value;
                DoRedraw();
            }
        }

        private void DrawStep(float width, float height, bool reshapable)
        {
            ClearPoints();
            AddPoint(Position);
            AddPoint(Position.X + width, Position.Y);
            AddPoint(Position.X + width + _nose, Position.Y + height/2);
            AddPoint(Position.X + width, Position.Y + height);
            AddPoint(Position.X, Position.Y + height);
            AddPoint(Position.X + _tail, Position.Y + height/2);
            Brush = Brushes.White;
            Reshapable = reshapable;
        }

        private void DoRedraw()
        {
            DrawStep(Width, Height, Reshapable);
        }
    }
}