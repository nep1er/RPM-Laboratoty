using FigureFactory.Factories;
using FigureFactory.Figures;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FigureFactory
{
    public partial class MainWindow : Window
    {
        private IFigureFactory _currentFactory;

        public MainWindow()
        {
            InitializeComponent();
            ColorComboBox.SelectionChanged += ColorComboBox_SelectionChanged;

            _currentFactory = new RedFactory();
        }

        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ColorComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem == null) return;

            string color = selectedItem.Tag.ToString();

            switch (color)
            {
                case "Red":
                    _currentFactory = new RedFactory();
                    break;

                case "Blue":
                    _currentFactory = new BlueFactory();
                    break;

                case "Green":
                    _currentFactory = new GreenFactory();
                    break;

                default: return;
            }

            ClearAllFigures();
        }

        private void AddCircleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFactory == null) return;

            Circle circle = _currentFactory.CreateCircle();
            FiguresPanel.Children.Add(circle.CreateUIElement());
        }

        private void AddSquareButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFactory == null) return;

            Square square = _currentFactory.CreateSquare();
            FiguresPanel.Children.Add(square.CreateUIElement());
        }

        private void AddTriangleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFactory == null) return;

            Triangle triangle = _currentFactory.CreateTriangle();
            FiguresPanel.Children.Add(triangle.CreateUIElement());
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearAllFigures();
        }

        private void ClearAllFigures()
        {
            FiguresPanel.Children.Clear();
        }
    }
}