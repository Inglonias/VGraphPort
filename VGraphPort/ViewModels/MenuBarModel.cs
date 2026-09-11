using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using VGraphPort.Config;
using VGraphPort.Objects;
using VGraphPort.Views;

namespace VGraphPort.ViewModels;

public partial class MenuBarModel : ViewModelBase
{
    [ObservableProperty] public partial bool UndoEnabled { get; set; } = false;
    [ObservableProperty] public partial bool RedoEnabled { get; set; } = false;

    //Commands
    public ICommand CreateNewGridCommand { get; }
    public ICommand EditExistingGridCommand { get; }
    

    public MenuBarModel()
    {
        CreateNewGridCommand = new RelayCommand(CreateNewGrid);
        EditExistingGridCommand = new RelayCommand(EditExistingGrid);
    }
    
    private async void CreateNewGrid()
    {
        if (!await CheckUnsavedChanges())
        {
            return;
        }

        ShowNewGridWindow(true);
    }

    private void EditExistingGrid()
    {
        ShowNewGridWindow(false);

    }
    
    private void ShowNewGridWindow(bool deleteLines)
    {
        NewGridWindow ngw = new NewGridWindow();
        var ngwm = new NewGridWindowModel
        {
            DeleteLines = deleteLines
        };
        ngw.DataContext = ngwm;
        ngw.Show();
    }
    
    //Returns true if there are no unsaved changes, and/or the user wants to continue. False otherwise.
    private async Task<bool> CheckUnsavedChanges()
    {
        if (PageData.Instance.IsCanvasDirty)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Warning - Unsaved changes",
                "You have unsaved changes. Are you sure you want to continue?", ButtonEnum.YesNo);
            var result = await box.ShowAsync();
            return result.HasFlag(ButtonResult.Yes);
        }
        return true;
    }
    
    public void CheckEditButtonValidity()
    {
        UndoEnabled = PageHistory.Instance.CanUndo();
        RedoEnabled = PageHistory.Instance.CanRedo();
    }
}