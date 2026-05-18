# Лабораторная работа 11. Навигация в MVVM-приложениях
Задание:
Выполнить рефакторинг приложения «Телефонная книга» в архитектуру Shell (Оболочка).

Требования к доработке:
1) Создать структуру папок: Разделить Views и ViewModels по папкам (Views и ViewModels).
2) Рефакторинг: переименовать ViewModel в ContactsListViewModel, а MainWindow.xaml — в ContactsListView (UserControl).
3) Создать Shell (Главное окно) с меню и областью контента (ContentControl) и MainWindowViewModel.
4) Разработать интерфейс INavigationService с методом NavigateTo<TViewModel>() и реализовать сервис навигации.
5) Настроить DI: зарегистрировать ViewModels, определить DataTemplate в App.xaml. Создать дополнительный экран «О программе» (AboutViewModel / AboutView).
