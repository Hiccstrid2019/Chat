using System;
using System.Windows;
using System.Windows.Navigation;
using ChatClient.ViewModels;

namespace ChatClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new Uri("/Pages/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }
        public MainWindowViewModel ViewModel { get; set; } = new MainWindowViewModel();
        private void frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            UpdateFrameDataContext(sender, e);
        }
        private void UpdateFrameDataContext(object sender, NavigationEventArgs e)
        {
            var content = mainFrame.Content as FrameworkElement;
            if (content == null)
                return;
            content.DataContext = mainFrame.DataContext;
        }
    }
}
