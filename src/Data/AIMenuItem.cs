namespace BlazorRTEWithOpenAI.Data
{
    public class AIMenuItem
    {
        public List<AIMenuItem> Items { get; set; }
        public string Content { get; set; }
        public string Prompt { get; set; }
        public string Id { get; set; }
        public bool Separator { get; set; }
    }
}
