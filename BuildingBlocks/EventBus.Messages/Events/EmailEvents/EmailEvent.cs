namespace EventBus.Messages.Events.EmailEvents
{
    public class EmailEvent
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string HtmlMessage { get; set; }
    }
}
