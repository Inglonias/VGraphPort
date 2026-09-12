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
        
    }
}