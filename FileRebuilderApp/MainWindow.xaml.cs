using FileRebuilderApp.Data;
using FileRebuilderApp.Models;
using FileRebuilderApp.Services;
using Microsoft.Win32;
using System.Windows;
using FileRebuilderApp.Helpers;

namespace FileRebuilderApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly FileService _fileService = new();
    private readonly RestoreService _restoreService = new();
    private readonly DatabaseService _databaseService = new();
    private readonly CommonHelpers _commonHelpers = new();
    private List<FileRecord> _allFiles = [];

    public MainWindow()
    {
        InitializeComponent();
        LoadFiles();
    }

    private void Browse_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();

        if (dialog.ShowDialog() == true)
        {
            FilePathTextBox.Text = dialog.FileName;
        }
    }

    private void Backup_Click(object sender, RoutedEventArgs e)
    {
        var path = FilePathTextBox.Text;

        if (!string.IsNullOrEmpty(path))
        {
            try
            {
                var result = _fileService.BackupFile(path);

                string message = $"File backed up successfully!\n" +
                                 $"Original Size: {_commonHelpers.FormatSize(result.OriginalSize)} bytes\n" +
                                 $"Compressed Size: {_commonHelpers.FormatSize(result.CompressedSize)} bytes\n";

                if (result.UsedExistingContent)
                    message += "\nNote: This file's content was already backed up, so it was not stored again.";

                MessageBox.Show(message);

                LoadFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }

    private void Restore_Click(object sender, RoutedEventArgs e)
    {
        if (FilesListBox.SelectedItem is not FileRecord selectedFile)
        {
            MessageBox.Show("Please select a file.");
            return;
        }

        var dialog = new SaveFileDialog
        {
            FileName = selectedFile.FileName
        };

        if (dialog.ShowDialog() == true)
        {
            _restoreService.RestoreFile(selectedFile.Id, dialog.FileName);
            MessageBox.Show("File restored successfully!");
        }
    }

    private void LoadFiles()
    {
        _allFiles = _databaseService.GetAllFiles();
        FilesListBox.ItemsSource = _allFiles;
    }

    private void Search_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        var query = SearchTextBox.Text.ToLower();

        var filtered = _allFiles
            .Where(f => f.FileName.ToLower().Contains(query))
            .ToList();

        FilesListBox.ItemsSource = filtered;
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as FrameworkElement)?.DataContext is not FileRecord file)
            return;

        var result = MessageBox.Show($"Are you sure you want to delete '{file.FileName}'?", "Confirm Delete", MessageBoxButton.YesNo);

        if (result == MessageBoxResult.Yes)
        {
            _databaseService.DeleteFile(file.Id);
            LoadFiles();
        }
    }
}