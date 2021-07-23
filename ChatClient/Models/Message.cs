using System;

namespace ChatClient.Models
{
    public class Message
    {
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime Time { get; set; }
        public bool Self { get; set; }
    }
}
