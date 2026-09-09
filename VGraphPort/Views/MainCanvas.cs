using Avalonia.Controls;
using Avalonia.Media;
using VGraphPort.DataLayers;

namespace VGraphPort.Views;

public class MainCanvas : Control
{
	GridBackgroundLayer LGrid = new GridBackgroundLayer();
	LayerDrawOperation DrawOp = new LayerDrawOperation();

	public override void Render(DrawingContext context)
	{
		base.Render(context);
		LGrid.GenerateLayerImage();
		DrawOp.Layers.Add(LGrid);
		context.Custom(DrawOp);
	}
}
