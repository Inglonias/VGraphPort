using Avalonia.Controls;
using VGraphPort.Config;
using VGraphPort.ViewModels;

namespace VGraphPort.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainMenuBar.DataContext = new MenuBarModel();
        this.DataContextChanged += (_, _) =>
        {
            if (DataContext is MainViewModel vm)
            {
                vm.UpdateMainCanvasVisual += (_, _) =>
                {
                    PrimaryDrawingPanel.InvalidateVisual();
                };
            }
        };
    }
}