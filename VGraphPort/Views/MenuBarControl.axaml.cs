using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using VGraphPort.Config;
using VGraphPort.DataLayers;
using VGraphPort.Objects;

namespace VGraphPort.Views
{
    public partial class MenuBarControl : UserControl
    {
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


    }
}