using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using VGraphPort.Config;
using VGraphPort.ViewModels;

namespace VGraphPort.Views;

public partial class NewGridWindow : Window
{

    public NewGridWindow()
    {
        InitializeComponent();
        this.DataContextChanged += (_, _) =>
        {
            if (DataContext is NewGridWindowModel vm)
            {
                vm.NewGridOkPressed += (_, _) =>
                {
                    PageData.Instance.UnlockMainWindow();
                    Close();
                };
            }
        };
        PageData.Instance.LockMainWindow();
        InvalidateVisual();
    }

    private void NewGridWindow_OnCancel(object sender, RoutedEventArgs e)
    {
        PageData.Instance.UnlockMainWindow();
        Close();
    }

    private async void BackgroundImageBrowse_OnClick(object sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var file = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Background Image",
            AllowMultiple = false,
            FileTypeFilter = [FilePickerFileTypes.ImageAll]
        });
        if (file.Count > 0)
        {
            if (DataContext is NewGridWindowModel vm)
            {
                vm.ImagePath = file[0].Path.ToString().Substring(7); //Remove the preceding "file//"
            }
        }
    }
}