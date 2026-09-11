using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VGraphPort.Config;
using VGraphPort.DataLayers;
using VGraphPort.Views;

namespace VGraphPort.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public ICommand MoveThingsUpCommand { get; }
    public ICommand MoveThingsLeftCommand { get; }
    public ICommand MoveThingsDownCommand { get; }
    public ICommand MoveThingsRightCommand { get; }
    public ICommand DeleteThingsCommand { get; }
    public event EventHandler? UpdateMainCanvasVisual;

    public MainViewModel()
    {
        MoveThingsUpCommand = new RelayCommand(MoveThingsUp);
        MoveThingsLeftCommand = new RelayCommand(MoveThingsLeft);
        MoveThingsDownCommand = new RelayCommand(MoveThingsDown);
        MoveThingsRightCommand = new RelayCommand(MoveThingsRight);
        DeleteThingsCommand = new RelayCommand(DeleteThings);
    }

    private void MoveThings(int x, int y)
    {
        LineLayer lineLayer = (LineLayer)PageData.Instance.GetDataLayer(PageData.LINE_LAYER);
        TextLayer textLayer = (TextLayer)PageData.Instance.GetDataLayer(PageData.TEXT_LAYER);
        lineLayer.MoveSelectedLines(x, y);
        textLayer.MoveSelectedLabels(x, y);
        UpdateMainCanvasVisual?.Invoke(this, EventArgs.Empty);
    }
    
    private void MoveThingsUp()
    {
        MoveThings(0, -1);
    }

    private void MoveThingsLeft()
    {
        MoveThings(-1, 0);
    }
    private void MoveThingsDown()
    {
        MoveThings(0, 1);
    }
    private void MoveThingsRight()
    {
        MoveThings(1, 0);
    }

    private void DeleteThings()
    {
        LineLayer lineLayer = (LineLayer)PageData.Instance.GetDataLayer(PageData.LINE_LAYER);
        TextLayer textLayer = (TextLayer)PageData.Instance.GetDataLayer(PageData.TEXT_LAYER);
        lineLayer.DeleteSelectedLines();
        textLayer.DeleteSelectedLabels();
        UpdateMainCanvasVisual?.Invoke(this, EventArgs.Empty);
    }
    
}