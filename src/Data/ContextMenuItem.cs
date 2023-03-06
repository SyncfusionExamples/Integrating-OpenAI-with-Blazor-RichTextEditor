namespace BlazorRTEWithOpenAI.Data
{
    public class ContextMenuItem
    {
        public List<ContextMenuItem> Items { get; set; }
        public string Content { get; set; }
        public string Prompt { get; set; }
        public string Id { get; set; }
        public bool Separator { get; set; }
    }
}
