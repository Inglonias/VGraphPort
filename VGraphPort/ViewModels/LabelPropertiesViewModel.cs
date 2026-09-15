using Avalonia.Media;
using Avalonia.Media.Fonts;
using Avalonia.Skia;
using CommunityToolkit.Mvvm.ComponentModel;
using VGraphPort.Config;
using VGraphPort.Objects;

namespace VGraphPort.ViewModels;

public partial class LabelPropertiesViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial IFontCollection InstalledFonts { get; set; }
    [ObservableProperty] public partial TextLabel TargetLabel { get; set; }

    public Color ChosenColor
    {
        get { return _chosenColor; }
        set { _chosenColor = SetChosenColor(value); }
        
    }
    private Color _chosenColor;
    
    public LabelPropertiesViewModel(TextLabel targetLabel)
    {
        TargetLabel = targetLabel;
        InstalledFonts = FontManager.Current.SystemFonts;
        _chosenColor = Color.FromArgb(PageData.Instance.CurrentLabelColor.Alpha,
            PageData.Instance.CurrentLabelColor.Red, 
            PageData.Instance.CurrentLabelColor.Green,
            PageData.Instance.CurrentLabelColor.Blue);
    }
    
    public Color SetChosenColor(Color newColor)
    {
        _chosenColor = newColor;
        PageData.Instance.CurrentLabelColor = newColor.ToSKColor();
        return _chosenColor;
    }
}