using Avalonia.Media;
using Avalonia.Media.Fonts;
using Avalonia.Skia;
using CommunityToolkit.Mvvm.ComponentModel;
using VGraphPort.Config;
using VGraphPort.Objects;

namespace VGraphPort.ViewModels;

public partial class LabelPropertiesViewModel : ViewModelBase
{
    [ObservableProperty] public partial IFontCollection InstalledFonts { get; set; }
    [ObservableProperty] public partial TextLabel TargetLabel { get; set; }
    [ObservableProperty] public partial string LabelTextInUi { get; set; }
    [ObservableProperty] public partial int FontSizeInUi { get; set; }
    [ObservableProperty] public partial FontFamily FontInUi { get; set; }
    [ObservableProperty] public partial int AlignmentIntInUi { get; set; }

    public Color ChosenColorInUi
    {
        get { return _chosenColorInUi; }
        set { _chosenColorInUi = SetChosenColor(value); }

    }
    private Color _chosenColorInUi;

    public LabelPropertiesViewModel(TextLabel targetLabel)
    {
        TargetLabel = targetLabel;
        LabelTextInUi = TargetLabel.LabelText;
        FontSizeInUi = TargetLabel.FontSize;
        InstalledFonts = FontManager.Current.SystemFonts;
        FontInUi = InstalledFonts[0];
        foreach (FontFamily f in InstalledFonts)
        {
            if (f.Name.Equals(TargetLabel.FontFamily))
            {
                FontInUi = f;
                break;
            }
        }
        AlignmentIntInUi = TargetLabel.Alignment;
        _chosenColorInUi = Color.FromArgb(PageData.Instance.CurrentLabelColor.Alpha,
            PageData.Instance.CurrentLabelColor.Red,
            PageData.Instance.CurrentLabelColor.Green,
            PageData.Instance.CurrentLabelColor.Blue);
    }

    public Color SetChosenColor(Color newColor)
    {
        _chosenColorInUi = newColor;
        PageData.Instance.CurrentLabelColor = newColor.ToSKColor();
        return _chosenColorInUi;
    }
}