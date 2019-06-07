using System.Collections.Generic;

namespace Our.Umbraco.GoldenGate.uSync.Models
{
    public class PreValues
    {
        public string Filter { get; set; }

        public int MaxNumber { get; set; }

        public int MinNumber { get; set; }

        public bool ShowOpen { get; set; }

        public Source TreeSource { get; set; }

        public bool Multiple { get; set; }

        public List<PreValue> Items { get; set; } = new List<PreValue>();
    }
}