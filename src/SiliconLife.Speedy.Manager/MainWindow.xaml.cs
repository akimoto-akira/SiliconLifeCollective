using System.Windows;
using SiliconLife.Speedy.Manager.ViewModels;
using SiliconLife.Speedy.Manager.ViewModels.Dialogs;
using SiliconLife.Speedy.Manager.Views.Dialogs;

namespace SiliconLife.Speedy.Manager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainViewModel? _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        // Unsubscribe from old ViewModel
        if (_viewModel != null)
        {
            _viewModel.ShowNewEntryDialogRequested -= OnShowNewEntryDialogRequested;
            _viewModel.ShowImportDialogRequested -= OnShowImportDialogRequested;
            _viewModel.ShowFileInfoDialogRequested -= OnShowFileInfoDialogRequested;
        }

        _viewModel = e.NewValue as MainViewModel;

        // Subscribe to new ViewModel
        if (_viewModel != null)
        {
            _viewModel.ShowNewEntryDialogRequested += OnShowNewEntryDialogRequested;
            _viewModel.ShowImportDialogRequested += OnShowImportDialogRequested;
            _viewModel.ShowFileInfoDialogRequested += OnShowFileInfoDialogRequested;
        }
    }

    private void OnShowNewEntryDialogRequested(NewEntryDialogViewModel vm)
    {
        var dialog = new NewEntryDialog
        {
            Owner = this,
            DataContext = vm
        };

        vm.DialogCloseRequested += result =>
        {
            dialog.DialogResult = result;
            dialog.Close();
        };

        dialog.ShowDialog();
    }

    private void OnShowImportDialogRequested(ImportDialogViewModel vm)
    {
        var dialog = new ImportDialog
        {
            Owner = this,
            DataContext = vm
        };

        vm.DialogCloseRequested += result =>
        {
            dialog.DialogResult = result;
            dialog.Close();
        };

        dialog.ShowDialog();
    }

    private void OnShowFileInfoDialogRequested(FileInfoDialogViewModel vm)
    {
        var dialog = new FileInfoDialog
        {
            Owner = this,
            DataContext = vm
        };

        vm.DialogCloseRequested += () =>
        {
            dialog.Close();
        };

        dialog.ShowDialog();
    }

    private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}
