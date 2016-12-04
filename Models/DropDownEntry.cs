namespace AnalyseVisitorsTool.Models
{
    public class DropDownEntry
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public DropDownEntry(string Text, string Value)
        {
            this.Text = Text;
            this.Value = Value;
        }
    }
}
