using System;
using Avalonia.Controls;
using VGraphPort.Objects;
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

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is MainViewModel vm)
        {
            vm.EditTextLabelEvent += (_, targetLabel) => EditLabel(targetLabel);
        }
    }
    private void PrimaryDrawingPanel_OnRenderEvent(object? sender, EventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.SetWindowTitle();
        }
    }

    private void EditLabel(TextLabel target)
    {
        LabelPropertiesWindow lpw = new LabelPropertiesWindow
        {
            ParentWindow = this,
            DataContext = new LabelPropertiesViewModel(target)
        };
        lpw.Show();
    }
}