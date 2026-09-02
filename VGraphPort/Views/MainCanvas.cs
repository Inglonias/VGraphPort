using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
namespace VGraphPort.Views;

public class MainCanvas : UserControl
{
	public override void Render(DrawingContext context)
	{
		base.Render(context);

		context.DrawRectangle(
			Brushes.Blue,
			null,
			new Rect(50, 50, 100, 100));
	}
}
