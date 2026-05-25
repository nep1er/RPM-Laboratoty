using System.Windows.Controls;
using System.Windows.Input;

namespace PhoneBook.Views
{
    /// <summary>
    /// Логика взаимодействия для ContactsListView.xaml
    /// </summary>
    public partial class ContactsListView : UserControl
    {
        public ContactsListView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик двойного клика по строке DataGrid для быстрого редактирования.
        /// </summary>
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Находим DataContext (это ContactsListViewModel)
            if (DataContext is ViewModels.ContactsListViewModel vm)
            {
                // Проверяем, что есть выбранный контакт
                if (vm.SelectedContact != null && vm.EditContactCommand.CanExecute(null))
                {
                    vm.EditContactCommand.Execute(vm.SelectedContact);
                }
            }
        }
    }
}