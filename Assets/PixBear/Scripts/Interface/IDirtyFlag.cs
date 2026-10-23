public interface IDirtyFlag
{
    public bool IsDirty { get; set; }
    public void MarkDirty() => IsDirty = true;
    public void ClearDirty() => IsDirty = false;
}