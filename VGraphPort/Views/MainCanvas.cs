using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using SkiaSharp;
using VGraphPort.DataLayers;

namespace VGraphPort.Views;

public class MainCanvas : Control
{
	GridBackgroundLayer LGrid = new GridBackgroundLayer();
	CursorLayer LCursor = new CursorLayer();
	LayerDrawOperation DrawOp = new LayerDrawOperation();

	public MainCanvas()
	{
		DrawOp.Layers.Add(LGrid);
		DrawOp.Layers.Add(LCursor);
	}

	public override void Render(DrawingContext context)
	{
		base.Render(context);
		context.Custom(DrawOp);
	}
	protected override void OnPointerMoved(PointerEventArgs e)
	{
		base.OnPointerMoved(e);
		var position = e.GetPosition(this);
		LCursor.MoveCursor(position);
		foreach (var layer in DrawOp.Layers)
		{
			if (layer.IsRedrawRequired())
			{
				InvalidateVisual();
				break;
			}
		}
	}
	protected override Size MeasureOverride(Size availableSize)
	{
		return new Size(DrawOp.Bounds.Width, DrawOp.Bounds.Height);
	}
}
