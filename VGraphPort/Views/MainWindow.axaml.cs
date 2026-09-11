using Avalonia.Controls;
using VGraphPort.ViewModels;

namespace VGraphPort.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainMenuBar.DataContext = new MenuBarModel();
        MainMenuBar.ParentWindow = this;
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