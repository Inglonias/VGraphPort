using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using SkiaSharp;
using VGraphPort.DataLayers;

namespace VGraphPort.Views;

public class MainCanvas : UserControl
{
	public override void Render(DrawingContext context)
	{
		base.Render(context);

		IDataLayer gridBackgroundLayer = new GridBackgroundLayer();
		
		//CAUTION: Code below was AI-generated. It converts an SKBitmap to something Avalonia can draw.
		SKBitmap backgroundBitmap = gridBackgroundLayer.GenerateLayerBitmap();

		using (var stream = new MemoryStream())
		{
			backgroundBitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
			stream.Position = 0;
			var backgroundImage = new Bitmap(stream);
    
			context.DrawImage(backgroundImage, new Rect(0, 0, backgroundBitmap.Width, backgroundBitmap.Height));
		}
	}
}
