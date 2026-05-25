using System.Windows;

namespace PhoneBook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml (Shell)
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // DataContext устанавливается через DI в App.xaml.cs
        }
    }
}