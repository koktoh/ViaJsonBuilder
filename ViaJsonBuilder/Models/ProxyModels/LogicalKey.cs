namespace ViaJsonBuilder.Models.ProxyModels
{
    public class LogicalKey
    {
        public string Tag { get; }
        public int Col { get; set; }
        public int Row { get; set; }

        public LogicalKey(string tag)
        {
            this.Tag = tag;
        }
    }
}
