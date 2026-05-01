using System.Windows.Controls;

namespace SiliconLife.Speedy.Manager.Views.Controls;

/// <summary>
/// ContentViewerPanel.xaml 的交互逻辑。
/// 显示并允许编辑 Pack 条目内容。
/// DataContext: <see cref="SiliconLife.Speedy.Manager.ViewModels.ContentViewerViewModel"/>
/// </summary>
public partial class ContentViewerPanel : UserControl
{
    public ContentViewerPanel()
    {
        InitializeComponent();
    }
}
