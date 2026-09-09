using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using SkiaSharp;
using VGraphPort.Config;
using VGraphPort.DataLayers;

namespace VGraphPort.Views;

public class MainCanvas : Control
{
	GridBackgroundLayer LGrid = new GridBackgroundLayer();
	LineLayer LLines = new LineLayer();
	TextLayer LText = new TextLayer();
	CursorLayer LCursor = new CursorLayer();
	PreviewLayer LPreview = new PreviewLayer();
	LayerDrawOperation DrawOp = new LayerDrawOperation();

	public MainCanvas()
	{
		DrawOp.Layers.Add(LGrid);
		DrawOp.Layers.Add(LLines);
		DrawOp.Layers.Add(LText);
		DrawOp.Layers.Add(LCursor);
		DrawOp.Layers.Add(LPreview);
		AssignPageData();
	}

	private void AssignPageData()
	{
		PageData.Instance.GetDataLayers()[PageData.GRID_LAYER] = LGrid;
		PageData.Instance.GetDataLayers()[PageData.LINE_LAYER] = LLines;
		PageData.Instance.GetDataLayers()[PageData.PREVIEW_LAYER] = LPreview;
		PageData.Instance.GetDataLayers()[PageData.TEXT_LAYER] = LText;
		PageData.Instance.GetDataLayers()[PageData.CURSOR_LAYER] = LCursor;
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
