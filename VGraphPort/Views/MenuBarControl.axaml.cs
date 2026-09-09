using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using VGraphPort.Config;
using VGraphPort.DataLayers;
using VGraphPort.Objects;

namespace VGraphPort.Views
{
    public partial class MenuBarControl : UserControl
    {
        public MenuBarControl()
        {
            InitializeComponent();
        }

		private void ToolMenu_OnChecked(object sender, RoutedEventArgs e)
		{
			ToggleButton toolClicked = (ToggleButton)sender;
			string targetTool = (string)toolClicked.Name;
			SelectTool(targetTool);
			InvalidateVisual();
		}

		private void SelectTool(string tool)
		{
			List<ToggleButton> toolMenuItems =
			[
				Line_Tool,
				Tri_Tool,
				Box_Tool,
				Circle_Tool,
				Boxy_Circle_Tool,
				Ellipse_Tool,
				Text_Tool,
			];

			foreach (ToggleButton m in toolMenuItems)
			{
				m.IsChecked = m.Name.Equals(tool);
			}
			LineLayer lineLayer = (LineLayer)PageData.Instance.GetDataLayer(PageData.LINE_LAYER);
			TextLayer textLayer = (TextLayer)PageData.Instance.GetDataLayer(PageData.TEXT_LAYER);
			lineLayer.SelectTool(tool);
			textLayer.SelectTool(tool);
		}
	}
}