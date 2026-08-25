namespace LocalAiLibrary.AiLibrary.CategoryClassification.Models
{
    public class ChatDateList
    {
        public required List<DateItem> Dates { get; set; }
    }

    public class DateItem
    {
        public required string DateFilter { get; set; }
    }
}