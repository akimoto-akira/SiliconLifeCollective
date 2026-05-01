using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using SiliconLife.Speedy.Manager.Models;
using SiliconLife.Speedy.Manager.ViewModels;

namespace SiliconLife.Speedy.Manager.Views.Controls;

/// <summary>
/// Interaction logic for DirectoryTreeView.xaml
/// Wraps the WPF TreeView for displaying the Pack file's virtual directory structure.
/// All business logic is handled by <see cref="DirectoryTreeViewModel"/>.
/// </summary>
public partial class DirectoryTreeView : UserControl
{
    public DirectoryTreeView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    // ─── DataContext wiring ───────────────────────────────────────────────────

    private DirectoryTreeViewModel? _viewModel;

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        // Detach from old ViewModel
        if (_viewModel?.RootNodes is INotifyCollectionChanged oldCollection)
            oldCollection.CollectionChanged -= OnRootNodesChanged;

        _viewModel = e.NewValue as DirectoryTreeViewModel;

        // Attach to new ViewModel
        if (_viewModel?.RootNodes is INotifyCollectionChanged newCollection)
            newCollection.CollectionChanged += OnRootNodesChanged;

        UpdateEmptyStateHint();
    }

    // ─── Empty-state hint ────────────────────────────────────────────────────

    private void OnRootNodesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        => UpdateEmptyStateHint();

    private void UpdateEmptyStateHint()
    {
        bool isEmpty = _viewModel == null || _viewModel.RootNodes.Count == 0;
        EmptyStateHint.Visibility = isEmpty ? Visibility.Visible : Visibility.Collapsed;
    }

    // ─── TreeView event handlers ──────────────────────────────────────────────

    /// <summary>
    /// Fires when a TreeViewItem is expanded.
    /// Calls <see cref="DirectoryTreeViewModel.ExpandNodeCommand"/> with the expanded node.
    /// </summary>
    private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
    {
        if (_viewModel == null) return;
        if (sender is TreeViewItem { DataContext: PackEntryNode node })
        {
            if (_viewModel.ExpandNodeCommand.CanExecute(node))
                _viewModel.ExpandNodeCommand.Execute(node);
        }
        // Prevent the event from bubbling up to parent TreeViewItems
        e.Handled = true;
    }

    /// <summary>
    /// Fires when the selected item in the TreeView changes.
    /// Calls <see cref="DirectoryTreeViewModel.SelectNodeCommand"/> with the newly selected node.
    /// </summary>
    private void DirectoryTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (_viewModel == null) return;
        if (e.NewValue is PackEntryNode node)
        {
            if (_viewModel.SelectNodeCommand.CanExecute(node))
                _viewModel.SelectNodeCommand.Execute(node);
        }
    }
}
