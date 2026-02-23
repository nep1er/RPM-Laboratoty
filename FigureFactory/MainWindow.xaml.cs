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
        private CircleCreator _currentCircleCreator;
        private SquareCreator _currentSquareCreator;
        private TriangleCreator _currentTriangleCreator;

        public MainWindow()
        {
            InitializeComponent();
            ColorComboBox.SelectionChanged += ColorComboBox_SelectionChanged;


            UpdateCreatorsBasedOnColor();

        }


        private void UpdateCreatorsBasedOnColor()
        {
            var selectedItem = ColorComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem == null) return;

            string color = selectedItem.Tag.ToString();

            switch (color)
            {
                case "Red":
                    _currentCircleCreator = new RedCircleCreator();
                    _currentSquareCreator = new RedSquareCreator();
                    _currentTriangleCreator = new RedTriangleCreator();

                    break;

                case "Blue":
                    _currentCircleCreator = new BlueCircleCreator();
                    _currentSquareCreator = new BlueSquareCreator();
                    _currentTriangleCreator = new BlueTriangleCreator();

                    break;

                case "Green":
                    _currentCircleCreator = new GreenCircleCreator();
                    _currentSquareCreator = new GreenSquareCreator();
                    _currentTriangleCreator = new GreenTriangleCreator();

                    break;

                default: return;
            }
        }

        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCreatorsBasedOnColor();
            ClearAllFigures();
        }

        private void AddCircleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCircleCreator == null) return;

            Circle circle = _currentCircleCreator.CreateCircle();
            FiguresPanel.Children.Add(circle.CreateUIElement());
        }

        private void AddSquareButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSquareCreator == null) return;

            Square square = _currentSquareCreator.CreateSquare();
            FiguresPanel.Children.Add(square.CreateUIElement());
        }

        private void AddTriangleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTriangleCreator == null) return;

            Triangle triangle = _currentTriangleCreator.CreateTriangle();
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