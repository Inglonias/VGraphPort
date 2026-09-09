using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using VGraphPort.DataLayers;

namespace VGraphPort.Views;

public class MainCanvas : Control
{
	GridBackgroundLayer LGrid = new GridBackgroundLayer();
	LayerDrawOperation DrawOp = new LayerDrawOperation();

	public MainCanvas()
	{
		DrawOp.Layers.Add(LGrid);
	}

	public override void Render(DrawingContext context)
	{
		base.Render(context);
		LGrid.GenerateLayerImage();
		context.Custom(DrawOp);
	}

	protected override Size MeasureOverride(Size availableSize)
	{
		return new Size(DrawOp.Bounds.Width, DrawOp.Bounds.Height);
	}
}
