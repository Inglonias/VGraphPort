using System.Collections.Generic;
using VGraphPort.objects;

namespace VGraphPort.Config
{
    //Class used to serialize a canvas for saving and loading files.
    internal class VgpFile
    {
        public int SquaresWide { get; set; }
        public int SquaresTall { get; set; }
        public int SquareSize { get; set; }
        public int MarginX { get; set; }
        public int MarginY { get; set; }
        public string BackgroundImagePath { get; set; }

        public List<LineSegment> Lines { get; set; }
        public List<TextLabel> Labels { get; set; }
    }
}
