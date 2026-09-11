using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System.Collections.Generic;
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

        private void OpenButton_OnClick(object? sender, RoutedEventArgs e)
        {
            
        }

        private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
        {
            
        }
        
        private void SaveAsButton_OnClick(object? sender, RoutedEventArgs e)
        {
            
        }
        
        private void ImportButton_OnClick(object? sender, RoutedEventArgs e)
        {
            
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
            };
            ngw.Show();
        }
    }
}