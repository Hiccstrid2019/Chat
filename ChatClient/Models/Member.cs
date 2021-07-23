using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace ChatClient.Models
{
    public class Member
    {
        public string Name { get; set; }
        public IList<Message> Chat { get; } = new ObservableCollection<Message>();
    }
}
