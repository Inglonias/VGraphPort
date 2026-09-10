using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Threading;
using SkiaSharp;
using System;
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
	private bool _redrawPending = true;
	public event EventHandler? EyedropperUsed;
	public event EventHandler? LineCreated;

	public MainCanvas()
	{
		DrawOp.Layers.Add(LGrid);
		DrawOp.Layers.Add(LLines);
		DrawOp.Layers.Add(LText);
		DrawOp.Layers.Add(LCursor);
		DrawOp.Layers.Add(LPreview);
		AssignPageData();
		DispatcherTimer timer = new()
		{
			Interval = TimeSpan.FromMilliseconds(16.67) // 60 FPS
		};

		timer.Tick += (_, _) =>
		{
			if (_redrawPending)
			{
				_redrawPending = false;
				foreach (var layer in DrawOp.Layers)
				{
					if (layer.IsRedrawRequired())
					{
						InvalidateVisual();
						break;
					}
				}
			}
		};

		timer.Start();
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
		_redrawPending = true;
	}
	protected override void OnPointerPressed(PointerPressedEventArgs e)
	{
		base.OnPointerPressed(e);
		if (e.Properties.IsRightButtonPressed)
		{
			SKPointI target = LCursor.RoundToNearestIntersection(e.GetPosition(this));
			SKPointI targetGrid = LCursor.GetCursorGridPoints();
			LText.HandleCreationClick(target, targetGrid);
			LPreview.HandleCreationClick(target, targetGrid);
			LineCreated?.Invoke(this, EventArgs.Empty);
		}
		else if (e.Properties.IsLeftButtonPressed)
		{
			bool maintainSelection = e.KeyModifiers.HasFlag(KeyModifiers.Control);
			bool selectionMade = LLines.HandleSelectionClick(e.GetPosition(this), maintainSelection) || LText.HandleSelectionClick(e.GetPosition(this), maintainSelection);
			if (selectionMade && PageData.Instance.IsEyedropperActive)
			{
				EyedropperUsed.Invoke(this, EventArgs.Empty);
			}
		}
		InvalidateVisual();
	}
	protected override Size MeasureOverride(Size availableSize)
	{
		return new Size(DrawOp.Bounds.Width, DrawOp.Bounds.Height);
	}
}
