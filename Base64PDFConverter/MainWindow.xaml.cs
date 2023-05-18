using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Base64PDFConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                sourceTextBox.Text = openFileDialog.FileName;
            }
        }

        private void DestinationBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                destinationTextBox.Text = saveFileDialog.FileName;
            }
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            string direction = ((ComboBoxItem)directionComboBox.SelectedItem).Content.ToString();
            string sourceFilePath = sourceTextBox.Text;
            string destinationFilePath = destinationTextBox.Text;

            if (!File.Exists(sourceFilePath))
            {
                MessageBox.Show("Source file does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (direction == "Base 64 to PDF")
            {
                string base64String = Clipboard.GetText();

                if (string.IsNullOrWhiteSpace(base64String))
                {
                    MessageBox.Show("Base64 string is empty. Please paste the base64-encoded string into the text field.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    byte[] bytes = Convert.FromBase64String(base64String);
                    File.WriteAllBytes(destinationFilePath, bytes);
                    MessageBox.Show("Conversion completed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while converting the base64 string to PDF. Error message: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (direction == "PDF to Base64")
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(sourceFilePath);
                    string base64String = Convert.ToBase64String(bytes);
                    Clipboard.SetText(base64String);
                    MessageBox.Show("Conversion completed successfully. The base64-encoded string has been copied to the clipboard.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while converting the PDF to base64. Error message: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid direction selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }

        private void SourceTextBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    sourceTextBox.Text = files[0];
                }
            }
        }

    }
}
