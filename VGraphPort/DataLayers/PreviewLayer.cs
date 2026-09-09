using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGraphPort.Config;
using VGraphPort.Objects;

namespace VGraphPort.DataLayers
{
	public class PreviewLayer : IDataLayer
	{
		public SKPointI PreviewPoint { get; set; }
		public SKPointI PreviewGridPoint { get; set; }
		private LineSegment[] PreviewLines;
		public bool OddMode { get; set; }
		private SKImage _lastImage;
		SKImage IDataLayer.LastImage => _lastImage;
		bool IDataLayer.DrawInExport => false;
		private bool PreviewPointActive = false;
		private bool RedrawOverride = false;
		public PreviewLayer()
		{
			OddMode = false;
		}

		public void ForceRedraw()
		{
			RedrawOverride = true;
		}

		public SKPointI GetRenderPoint()
		{
			int drawRadius = Math.Max(0, PageData.Instance.SquareSize / 6);
			int minX = PageData.Instance.GetTotalWidth();
			int minY = PageData.Instance.GetTotalHeight();

			if (PreviewLines == null)
			{
				return new SKPointI(0, 0);
			}
			foreach (LineSegment l in PreviewLines)
			{
				foreach (SKPointI p in l.GetCanvasPoints())
				{
					if (p.X < minX)
					{
						minX = p.X;
					}
					if (p.Y < minY)
					{
						minY = p.Y;
					}
				}
			}

			return new SKPointI(minX - drawRadius, minY - drawRadius);
		}

		private SKRectI GetLayerSize()
		{
			int drawRadius = Math.Max(0, PageData.Instance.SquareSize / 6);
			int minX = PageData.Instance.GetTotalWidth();
			int minY = PageData.Instance.GetTotalHeight();
			int maxX = 0;
			int maxY = 0;
			if (PreviewLines == null || PreviewLines.Length == 0)
			{
				return new SKRectI(0, 0, 1, 1);
			}
			foreach (LineSegment l in PreviewLines)
			{
				foreach (SKPointI p in l.GetCanvasPoints())
				{
					if (p.X < minX)
					{
						minX = p.X;
					}
					if (p.Y < minY)
					{
						minY = p.Y;
					}
					if (p.X > maxX)
					{
						maxX = p.X;
					}
					if (p.Y > maxY)
					{
						maxY = p.Y;
					}
				}
			}
			return new SKRectI(minX - 100, minY - 100, maxX + 100, maxY + 100);
		}

		public bool IsRedrawRequired()
		{
			return PreviewPointActive || RedrawOverride;
		}

		public void HandleCreationClick(SKPointI point, SKPointI gridPoint)
		{
			LineLayer lLines = (LineLayer)PageData.Instance.GetDataLayer(PageData.LINE_LAYER);

			if (lLines.SelectedTool == null)
			{
				return;
			}

			if (PreviewPointActive)
			{
				PreviewPointActive = false;
				LineSegment[] lines;
				if (!OddMode)
				{
					lines = lLines.SelectedTool.DrawWithTool(PreviewGridPoint, gridPoint);
				}
				else
				{
					lines = lLines.SelectedTool.DrawWithToolOdd(PreviewGridPoint, gridPoint);
				}
				if (lines != null)
				{
					PageHistory.Instance.CreateUndoPoint(lLines.LineList, null, true);
					lLines.AddNewLines(lines);
					PageData.Instance.MakeCanvasDirty();
					ForceRedraw();
				}
			}
			else
			{
				PreviewPointActive = true;
				PreviewPoint = point;
				PreviewGridPoint = gridPoint;
			}
		}

		public string GetStatusText()
		{
			if (!PreviewPointActive)
			{
				return "";
			}
			LineLayer lLines = (LineLayer)PageData.Instance.GetDataLayer(PageData.LINE_LAYER);
			SKPointI cursorGridPoint = ((CursorLayer)PageData.Instance.GetDataLayer(PageData.CURSOR_LAYER)).GetCursorGridPoints();
			string rVal = lLines.SelectedTool.GenerateStatusText(PreviewGridPoint, cursorGridPoint);

			return rVal;
		}

		public SKImage GenerateLayerImage()
		{
			int drawRadius = Math.Max(0, PageData.Instance.SquareSize / 6);
			LineLayer lLines = (LineLayer)PageData.Instance.GetDataLayer(PageData.LINE_LAYER);

			if (_lastImage == null || IsRedrawRequired())
			{
				RedrawOverride = false;
				int canvasWidth = GetLayerSize().Width;
				int canvasHeight = GetLayerSize().Height;
				if (canvasWidth < 1 || canvasHeight < 1)
				{
					return null;
				}

				//Disposables
				SKSurface drawingSurface = SKSurface.Create(new SKImageInfo(canvasWidth, canvasHeight));
				SKCanvas drawingCanvas = drawingSurface.Canvas;
				SKPaint previewBrush = new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = drawRadius, Color = PageData.Instance.CurrentLineColor.WithAlpha(86), IsAntialias = true };

				if (PreviewPointActive && lLines.SelectedTool != null)
				{
					SKPointI cursorGridPoint = ((CursorLayer)PageData.Instance.GetDataLayer(PageData.CURSOR_LAYER)).GetCursorGridPoints();
					if (!OddMode)
					{
						PreviewLines = lLines.SelectedTool.DrawWithTool(PreviewGridPoint, cursorGridPoint);
					}
					else
					{
						PreviewLines = lLines.SelectedTool.DrawWithToolOdd(PreviewGridPoint, cursorGridPoint);
					}
					if (PreviewLines != null)
					{
						foreach (LineSegment line in PreviewLines)
						{
							SKPointI[] canvasPoints = line.GetCanvasPoints();
							SKPointI topLeft = GetRenderPoint();
							canvasPoints[LineSegment.START].X -= topLeft.X;
							canvasPoints[LineSegment.START].Y -= topLeft.Y;
							canvasPoints[LineSegment.END].X -= topLeft.X;
							canvasPoints[LineSegment.END].Y -= topLeft.Y;
							drawingCanvas.DrawLine(canvasPoints[LineSegment.START], canvasPoints[LineSegment.END], previewBrush);
						}
					}
				}
				else
				{
					PreviewLines = null;
				}

				//Dispose of them.
				drawingSurface.Dispose();
				previewBrush.Dispose();

				if (_lastImage != null)
				{
					_lastImage.Dispose();
				}
				_lastImage = drawingSurface.Snapshot();
			}

			return _lastImage;
		}
	}
}
