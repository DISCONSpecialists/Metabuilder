using System;

namespace MetaBuilder.Graphing.Shapes.Behaviours.Observers
{
    [Serializable]
    public enum PositionLockLocation
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        MiddleLeft,
        MiddleRight,
        BottomCenter,
        TopCenter,
        MiddleCenter
    }

    [Serializable]
    public enum AutoSizeType
    {
        Width,
        Height,
        Both
    }

    [Serializable]
    public enum ContainerPaddingType
    {
        Height,
        Width
    }
}