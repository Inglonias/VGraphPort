using Avalonia;
using Avalonia.Media;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using System;
using System.Collections.Generic;
using VGraphPort.Config;

namespace VGraphPort.DataLayers
{
    public class LayerDrawOperation : ICustomDrawOperation
    {
        public List<IDataLayer> Layers { get; set; } = new List<IDataLayer>();
        public Rect Bounds => GetBounds();

        public void Dispose()
        {
            
        }

        public bool Equals(ICustomDrawOperation? other)
        {
            throw new NotImplementedException();
        }

        public bool HitTest(Point p)
        {
            return Bounds.Contains(p);
        }

        private Rect GetBounds()
        {
            return new Rect(new Point(0,0), new Point(PageData.Instance.GetTotalWidth(), PageData.Instance.GetTotalHeight()));
        }

        //CAUTION: CoPilot helped to generate this method.
        public void Render(ImmediateDrawingContext context)
        {
            var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
            if (leaseFeature == null)
                return;

            using var lease = leaseFeature.Lease();

            foreach (var layer in Layers)
            {
                var renderPoint = layer.GetRenderPoint();
                if (layer.GenerateLayerImage() != null)
                {
                    lease.SkCanvas.DrawImage(layer.GenerateLayerImage(), renderPoint.X, renderPoint.Y);
                }
            }
        }
    }
}
