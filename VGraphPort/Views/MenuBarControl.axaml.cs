using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System.Collections.Generic;
using Avalonia.Platform.Storage;
using VGraphPort.Config;
using VGraphPort.DataLayers;
using VGraphPort.ViewModels;

namespace VGraphPort.Views
{
    public partial class MenuBarControl : UserControl
    {
        public required MainWindow ParentWindow { get; set; }
        //Commands
        public MenuBarControl()
        {
            InitializeComponent();
        }
        
        private void ToolMenu_OnChecked(object sender, RoutedEventArgs e)
        {
            ToggleButton toolClicked = (ToggleButton)sender;
            if (toolClicked.Name != null)
            {
                string targetTool = toolClicked.Name;
                SelectTool(targetTool);
            }

            InvalidateVisual();
        }
        
        private void SelectTool(string tool)
        {
            List<ToggleButton> toolMenuItems =
            [
                LineTool,
                TriTool,
                BoxTool,
                CircleTool,
                BoxyCircleTool,
                EllipseTool,
                TextTool,
            ];

            foreach (ToggleButton m in toolMenuItems)
            {
                m.IsChecked = m.Name != null && m.Name.Equals(tool);
            }
            LineLayer lineLayer = (LineLayer)PageData.Instance.GetDataLayer(PageData.LINE_LAYER);
            TextLayer textLayer = (TextLayer)PageData.Instance.GetDataLayer(PageData.TEXT_LAYER);
            lineLayer.SelectTool(tool);
            textLayer.SelectTool(tool);
        }

        private void OddMode_OnClick(object sender, RoutedEventArgs e)
        {
            PreviewLayer previewLayer = (PreviewLayer)PageData.Instance.GetDataLayer(PageData.PREVIEW_LAYER);
            previewLayer.OddMode = OddModeCheckbox.IsChecked ?? false;
        }


        private void NewGridButton_OnClick(object? sender, RoutedEventArgs e)
        {
            OpenNewGridWindow(true);
        }

        private void ResizeButton_OnClick(object? sender, RoutedEventArgs e)
        {
            OpenNewGridWindow(false);
        }

        private async void OpenButton_OnClick(object? sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var file = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open VGP File",
                AllowMultiple = false,
                FileTypeFilter = [Vgp]
            });
            if (file.Count > 0)
            {
                PageData.Instance.FileOpen(file[0].Path.ToString().Substring(7)); //Remove the preceding "file//")
                ParentWindow.PrimaryDrawingPanel.InvalidateVisual();
                ParentWindow.PrimaryDrawingPanel.InvalidateMeasure();
            }
        }

        private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PageData.Instance.LastSavePath))
            {
                PageData.Instance.FileSave(PageData.Instance.LastSavePath);
            }
            else
            {
                SaveAsButton_OnClick(sender, e);
            }
        }
        
        private async void SaveAsButton_OnClick(object? sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var file = await topLevel!.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save VGP File",
                DefaultExtension = ".vgp",
                SuggestedFileType = Vgp
            });
            if (file is not null)
            {
                PageData.Instance.FileOpen(file.Path.ToString().Substring(7)); //Remove the preceding "file//")
            }
        }
        
        private async void ImportButton_OnClick(object? sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var file = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Import VGP File",
                AllowMultiple = false,
                FileTypeFilter = [Vgp]
            });
            if (file.Count > 0)
            {
                PageData.Instance.FileOpen(file[0].Path.ToString().Substring(7)); //Remove the preceding "file//")
                ParentWindow.PrimaryDrawingPanel.InvalidateVisual();
                ParentWindow.PrimaryDrawingPanel.InvalidateMeasure();
            }
        }
        
        private void ExportButton_OnClick(object? sender, RoutedEventArgs e)
        {
            
        }
        
        private void OpenNewGridWindow(bool deleteLines)
        {
            NewGridWindow ngw = new NewGridWindow();
            var ngwm = new NewGridWindowModel
            {
                DeleteLines = deleteLines
            };
            ngw.DataContext = ngwm;
            ngw.NewGridWindowComplete += (_, _) =>
            {
                ParentWindow.PrimaryDrawingPanel.InvalidateVisual();
                ParentWindow.PrimaryDrawingPanel.InvalidateMeasure();
            };
            ngw.Show();
        }
        
        private static FilePickerFileType Vgp { get; } = new FilePickerFileType("VGP File")
        {
            Patterns = (IReadOnlyList<string>) new string[1]
            {
                "*.vgp"
            },
            AppleUniformTypeIdentifiers = (IReadOnlyList<string>) new string[1]
            {
                "public.json"
            },
            MimeTypes = (IReadOnlyList<string>) new string[1]
            {
                "application/json"
            }
        };
    }
}