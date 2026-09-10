using Avalonia.Controls;
using VGraphPort.Config;

namespace VGraphPort.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
		PrimaryDrawingPanel.LineCreated += (_, _) =>
		{
			MainMenuBar.CheckEditButtonValidity();
		};
		PrimaryDrawingPanel.EyedropperUsed += (_, _) =>
		{
			PageData.Instance.IsEyedropperActive = false;
			MainMenuBar.Eyedropper_Tool.IsChecked = false;
			MainMenuBar.InvalidateVisual();
			//MainMenuBar.ColorSwatch.InvalidateVisual();
		};
	}
}