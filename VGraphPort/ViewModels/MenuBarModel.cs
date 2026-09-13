using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VGraphPort.Config;
using VGraphPort.DataLayers;
using VGraphPort.Objects;


namespace VGraphPort.ViewModels;

public partial class MenuBarModel : ViewModelBase
{
    [ObservableProperty] public partial bool UndoEnabled { get; set; } = false;
    [ObservableProperty] public partial bool RedoEnabled { get; set; } = false;
    [ObservableProperty] public partial bool CenterLinesEnabled { get; set; }
    public Func<Task<bool>>? RequestUnsavedChangesConfirmation { get; set; }

    public event EventHandler<bool>? ShowNewGridWindow;
    
    //Commands
    public ICommand CreateNewGridCommand { get; }
    public ICommand EditExistingGridCommand { get; }

    public MenuBarModel()
    {
        CreateNewGridCommand = new RelayCommand(CreateNewGrid);
        EditExistingGridCommand = new RelayCommand(EditExistingGrid);
    }

    private void CreateNewGrid()
    {
        _ = CreateNewGridAsync();
    }
    
    private async Task CreateNewGridAsync()
    {
        if (await CheckUnsavedChangesAsync())
        {
            ShowNewGridWindow?.Invoke(this, true);
        }
    }

    private void EditExistingGrid()
    {
        ShowNewGridWindow?.Invoke(this, false);
    }

    public void SelectTool(string tool)
    {
        LineLayer lineLayer = (LineLayer)PageData.Instance.GetDataLayer(PageData.LINE_LAYER);
        TextLayer textLayer = (TextLayer)PageData.Instance.GetDataLayer(PageData.TEXT_LAYER);
        lineLayer.SelectTool(tool);
        textLayer.SelectTool(tool);
    }

    public void ToggleOddMode(bool value)
    {
        PreviewLayer previewLayer = (PreviewLayer)PageData.Instance.GetDataLayer(PageData.PREVIEW_LAYER); 
        previewLayer.OddMode = value;
    }
    
    //Returns true if there are no unsaved changes, and/or the user wants to continue. False otherwise.
    //CAUTION: Method written by CoPilot
    private async Task<bool> CheckUnsavedChangesAsync()
    {
        if (!PageData.Instance.IsCanvasDirty)
        {
            return true;
        }

        if (RequestUnsavedChangesConfirmation is null)
        {
            return true; // fallback if no UI is attached
        }

        return await RequestUnsavedChangesConfirmation.Invoke();
    }

    public void CheckEditButtonValidity()
    {
        UndoEnabled = PageHistory.Instance.CanUndo();
        RedoEnabled = PageHistory.Instance.CanRedo();
    }

    public void OpenVgpFile(string filePath)
    {
        PageData.Instance.FileOpen(filePath);
    }

    public bool SaveCurrentVgp()
    {
        if (string.IsNullOrEmpty(PageData.Instance.LastSavePath))
        {
            return false;
        }
        PageData.Instance.FileSave();
        return true;
    }

    public void SaveNewVgp(string filePath)
    {
        PageData.Instance.FileSave(filePath);
    }

    public void ImportVgp(string filePath)
    {
        PageData.Instance.FileImport(filePath);
    }

    public void ExportVgp(string filePath)
    {
        PageData.Instance.FileExport(filePath);
    }
}