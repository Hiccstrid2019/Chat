using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChatClient.Models;
using ChatClient.Cmds;
using System.Windows.Input;
using System.Windows;
using ChatClient.Services;

namespace ChatClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ChatService chatService;
        public MainWindowViewModel()
        {
            chatService = new ChatService();
            chatService.NewTextMessageRecived += NewTextMessageRecived;
            chatService.Logged += Logged;
            chatService.ShowAllUsers += ShowAllUsers;
            chatService.NewTextMessageFromGroup += NewTextMessageFromGroup;
        }

        private bool _isLogged;
        public bool IsLogged
        {
            get => _isLogged;
            set
            {
                _isLogged = value;
                OnPropertyChanged();
            }

        }

        private Visibility _addingChat = Visibility.Collapsed;
        public Visibility AddingChat
        {
            get => _addingChat;
            set
            {
                _addingChat = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibilityCreateConfPanel = Visibility.Collapsed;
        public Visibility VisibilityCreateConfPanel
        {
            get => _visibilityCreateConfPanel;
            set
            {
                _visibilityCreateConfPanel = value;
                OnPropertyChanged();
            }
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _textMessage;
        public string TextMessage
        {
            get => _textMessage;
            set
            {
                _textMessage = value;
                OnPropertyChanged();
            }
        }
        private string _confName;
        public string ConfName
        {
            get => _confName;
            set
            {
                _confName = value;
                OnPropertyChanged();
            }
        }
        private Member _selectedMember;
        public Member SelectedMember
        {
            get => _selectedMember;
            set
            {
                _selectedMember = value;
                OnPropertyChanged();
            }
        }
        private string _addSelectedUser;
        public string AddSelectedUser
        {
            get => _addSelectedUser;
            set
            {
                _addSelectedUser = value;

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (value != null)
                    {
                        Members.Add(new Member { Name = value });
                    }
                }));
                AddingChat = Visibility.Collapsed;
                OnPropertyChanged();
            }
        }
        public IList<Member> Members { get; } = new ObservableCollection<Member>();

        public IList<string> ExistsUsers { get; } = new ObservableCollection<string>();

        public IList<UserList> ConfUsers { get; } = new ObservableCollection<UserList>();
        

        private ICommand _loginCommand = null;
        public ICommand LoginCmd
            => _loginCommand ?? (_loginCommand = new RelayCommand(Login, CanLogin));
        private bool CanLogin() => UserName != null && Password != null;
        private void Login()
        {
            // chatService.Connect();
            chatService.LogIn(UserName, Password);
            if (IsLogged)
                (Application.Current.MainWindow as MainWindow)?.mainFrame.Navigate(new Uri("/Pages/MessagesPage.xaml", UriKind.RelativeOrAbsolute));
        }

        public ICommand _addChatCmd = null;
        public ICommand AddChatCmd
            => _addChatCmd ?? (_addChatCmd = new RelayCommand(AddChat, CanAddChat));
        private bool CanAddChat() => AddingChat != Visibility.Visible;
        public void AddChat()
        {
            ExistsUsers.Clear();
            chatService.GetAllUsers();
            AddingChat = Visibility.Visible;
        }

        public ICommand _openConfCmd = null;
        public ICommand OpenConfCmd
            => _openConfCmd ?? (_openConfCmd = new RelayCommand(OpenConf, CanOpenConf));
        private bool CanOpenConf() => VisibilityCreateConfPanel != Visibility.Visible;
        public void OpenConf()
        {
            ConfUsers.Clear();
            chatService.GetAllUsers();
            VisibilityCreateConfPanel = Visibility.Visible;
        }

        private ICommand _closeConfPanelCmd = null;
        public ICommand CloseConfPanelCmd
            => _closeConfPanelCmd ?? (_closeConfPanelCmd = new RelayCommand(CloseConfPanel, CanCloseConfPanel));
        private bool CanCloseConfPanel() => VisibilityCreateConfPanel == Visibility.Visible;
        public void CloseConfPanel()
        {
            VisibilityCreateConfPanel = Visibility.Collapsed;
        }

        private ICommand _createConfCmd = null;
        public ICommand CreateConfCmd
            => _createConfCmd ?? (_createConfCmd = new RelayCommand(CreateConf, CanCreateConf));
        private bool CanCreateConf() => ConfName != null;
        public void CreateConf()
        {
            List<string> ConfUserNames = (from user in ConfUsers where user.IsSelected select user.Name).ToList();
            VisibilityCreateConfPanel = Visibility.Collapsed;
            chatService.CreateGroup(ConfName, ConfUserNames);
            Members.Add(new Member { Name = ConfName });
            ConfName = null;
        }

        private ICommand _closeAddUserCmd = null;
        public ICommand CloseAddUserCmd
            => _closeAddUserCmd ?? (_closeAddUserCmd = new RelayCommand(CloseAddPanel, CanCloseAddPanel));
        private bool CanCloseAddPanel() => AddingChat == Visibility.Visible;
        public void CloseAddPanel()
        {
            AddingChat = Visibility.Collapsed;
        }

        private ICommand _sendCommand = null;
        public ICommand SendCmd
            => _sendCommand ?? (_sendCommand = new RelayCommand(SendTextMessage, CanSendTextMessage));
        private bool CanSendTextMessage() => TextMessage != null && SelectedMember != null;

        private void SendTextMessage()
        {
            string user = SelectedMember.Name;
            chatService.SendTextMessage(user, UserName, _textMessage);
            Message msg = new Message
            {
                Author = UserName,
                Text = _textMessage,
                Time = DateTime.Now,
                Self = true
            };
            SelectedMember.Chat.Add(msg);
            TextMessage = string.Empty;
        }

        private void NewTextMessageRecived(string name, string message)
        {
            if (!(from m in Members where m.Name == name select m).Any())
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Members.Add(new Member { Name = name });
                }));
            }
            Message msg = new Message
            {
                Author = name,
                Text = message,
                Time = DateTime.Now,
                Self = false
            };
            Application.Current.Dispatcher.BeginInvoke(new Action(() => 
            {
                (from m in Members where m.Name == name select m).First().Chat.Add(msg);
            }));
        }

        private void NewTextMessageFromGroup(string groupname, string sender, string message)
        {
            if (!(from m in Members where m.Name == groupname select m).Any())
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Members.Add(new Member { Name = groupname });
                }));
            }
            Message msg = new Message
            {
                Author = sender,
                Text = message,
                Time = DateTime.Now,
                Self = false
            };
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                (from m in Members where m.Name == groupname select m).First().Chat.Add(msg);
            }));
        }
        private void ShowAllUsers(List<string> UserNames)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (var user in UserNames)
                {
                    ConfUsers.Add(new UserList { Name = user, IsSelected = false });
                }
                ConfUsers.Remove(ConfUsers.Where(u => u.Name == UserName).First());
                UserNames.Remove(UserName);
                foreach (var member in Members)
                {
                    if (UserNames.Contains(member.Name))
                    {
                        UserNames.Remove(member.Name);
                    }
                }
                foreach (var name in UserNames)
                {
                    ExistsUsers.Add(name);
                }
            }));
        }
        private void Logged()
        {
            IsLogged = true;
        }
    }
}
