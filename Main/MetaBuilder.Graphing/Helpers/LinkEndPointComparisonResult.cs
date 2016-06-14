namespace MetaBuilder.Graphing.Helpers
{
    /// <summary>
    /// This enum is used to see if all the parent classes 
    /// in a selection are of the same type. 
    /// 
    /// Used when "Select All Items Of This Type" is clicked.
    /// </summary>
    public enum LinkEndPointComparisonResult
    {
        ParentsEQ,
        BothEQ,
        ChildrenEQ,
        Neither
    }
}