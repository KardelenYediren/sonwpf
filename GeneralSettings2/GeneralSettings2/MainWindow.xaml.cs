using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.Linq;
using System;
using System.IO;

namespace GeneralSettings
{
    public class DpiDecorator : Decorator
    {
        public DpiDecorator()
        {
            this.Loaded += (s, e) =>
            {
                System.Windows.Media.Matrix m = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
                ScaleTransform dpiTransform = new ScaleTransform(1 / m.M11, 1 / m.M22);
                if (dpiTransform.CanFreeze)
                    dpiTransform.Freeze();
                this.LayoutTransform = dpiTransform;
            };
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.SizeChanged += Window_SizeChanged;
            MainTabControl.SelectionChanged += MainTabControl_SelectionChanged;
            // Subscribe to the SizeChanged event of each TabItem's content
            foreach (TabItem tabItem in MainTabControl.Items)
            {
                if (tabItem.Content is FrameworkElement content)
                {
                    content.SizeChanged += Content_SizeChanged;
                }
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AdjustColumnWidths();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustColumnWidths();
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AdjustColumnWidths();
        }

        private void Content_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustColumnWidths();
        }

        private void AdjustColumnWidths()
        {
            // Adjust column widths for MainGrid and AnotherGrid
            AdjustMainGridColumnWidth();
            AdjustAnotherGridColumnWidth();
        }

        // Adjust the column width for MainGrid
        private void AdjustMainGridColumnWidth()
        {
            double windowWidthMinusFixed = this.ActualWidth - 178.71;
            double maxTextBoxWidth = GetMaxTextBoxWidth(MainGrid);

            double dynamicColumnWidth = Math.Min(windowWidthMinusFixed, maxTextBoxWidth);
            if (dynamicColumnWidth < 0) dynamicColumnWidth = 0;

            DynamicColumn.Width = new GridLength(dynamicColumnWidth, GridUnitType.Pixel);
        }

        // Adjust the column width for AnotherGrid
        private void AdjustAnotherGridColumnWidth()
        {
            double windowWidthMinusFixed = this.ActualWidth - 260.71;
            double maxTextBoxWidth = GetMaxTextBoxWidth(AnotherGrid);

            double dynamicColumnWidth = Math.Min(windowWidthMinusFixed, maxTextBoxWidth);
            if (dynamicColumnWidth < 0) dynamicColumnWidth = 0;

            DynamicColumn2.Width = new GridLength(dynamicColumnWidth, GridUnitType.Pixel);
        }

        // Get the maximum width of TextBoxes in a specific grid
        private double GetMaxTextBoxWidth(Grid grid)
        {
            double maxWidth = 0;
            foreach (var textBox in grid.Children.OfType<TextBox>())
            {
                textBox.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                if (textBox.DesiredSize.Width > maxWidth) maxWidth = textBox.DesiredSize.Width;
            }
            return maxWidth;
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                Button clickedButton = sender as Button;
                if (clickedButton != null)
                {
                    switch (clickedButton.Name)
                    {
                        case "SelectFileButton0":
                            FilePathTextBox0.Text = openFileDialog.FileName;
                            break;
                        case "SelectFileButton1":
                            FilePathTextBox1.Text = openFileDialog.FileName;
                            break;
                        case "SelectFileButton2":
                            FilePathTextBox2.Text = openFileDialog.FileName;
                            break;
                        case "SelectFileButton3":
                            FilePathTextBox3.Text = openFileDialog.FileName;
                            break;
                        case "SelectFileButton4":
                            FilePathTextBox4.Text = openFileDialog.FileName;
                            break;
                        case "SelectFileButton5":
                            FilePathTextBox5.Text = openFileDialog.FileName;
                            break;
                    }

                    MainGrid.UpdateLayout();
                    AdjustMainGridColumnWidth();
                }
            }
        }

        private void SelectFileButton1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(selectedFilePath);
                FileNameTextBox.Text = fileName;

                AnotherGrid.UpdateLayout();
                AdjustAnotherGridColumnWidth();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void WindowStateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
