namespace ShadowrunTracker.Model
{
    public class SaveContext
    {
        public string? Filename { get; set; }
        public bool SaveAs { get; set; }
        public object Object { get; set; }

        public SaveContext(object obj, bool saveAs = true, string? filename = null)
        {
            Object = obj;
            Filename = filename;
            SaveAs = saveAs;
        }
    }
}
