namespace TimeTrackingAPI.DTOs
{
    public class TimeLogUpdateDto
    {
        public DateTime NewTime { get; set; }
        public string NewType { get; set; }
        public int? TotalHours { get; set; } // Optional, only needed if updating total hours
    }
}
