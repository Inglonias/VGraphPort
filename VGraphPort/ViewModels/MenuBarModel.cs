using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using VGraphPort.Config;
using VGraphPort.DataLayers;
using VGraphPort.Objects;
using VGraphPort.Views;

namespace VGraphPort.ViewModels;

public class MenuBarModel : ViewModelBase
{
    public async void CreateNewGrid(bool deleteLines)
    {
        if (deleteLines)
        {
            if (await CheckUnsavedChanges())
            {
                return;
            }
        }
        NewGridWindow ngw = new NewGridWindow
        {
            DeleteLines = deleteLines
        };
        ngw.OkPressed += (_, _) => NewGridOkPressed?.Invoke(this, EventArgs.Empty);
        ngw.Show();
    }
    
    private async Task<bool> CheckUnsavedChanges()
    {
        if (PageData.Instance.IsCanvasDirty)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Warning - Unsaved changes",
                "You have unsaved changes. Are you sure you want to continue?", ButtonEnum.YesNo);
            var result = await box.ShowAsync();
            return result.HasFlag(ButtonResult.Yes);
        }
        return false;
    }
    
    public void CheckEditButtonValidity()
    {
        LineLayer lineLayer = (LineLayer)PageData.Instance.GetDataLayer(PageData.LINE_LAYER);
        UndoButton.IsEnabled = PageHistory.Instance.CanUndo();
        RedoButton.IsEnabled = PageHistory.Instance.CanRedo();
    }
}