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
            if (DataContext is ViewModels.ContactsListViewModel vm)
            {
                // ✅ Правильно: передаём SelectedContact в CanExecute
                if (vm.SelectedContact != null && vm.EditCommand.CanExecute(vm.SelectedContact))
                {
                    vm.EditCommand.Execute(vm.SelectedContact);
                }
            }
        }
    }
}