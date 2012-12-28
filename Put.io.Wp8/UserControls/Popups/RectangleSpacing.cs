using System.Windows;

namespace Put.io.Wp8.UserControls.Popups
{
    public class RectangleSpacing
    {
        public int Top { get; private set; }
        public int Bottom { get; private set; }
        public int Left { get; private set; }
        public int Right { get; private set; }
        public double? AdjustedWidth { get; private set; }
        public double? AdjustedHeight { get; private set; }

        public RectangleSpacing(int spacing)
        {
            Top = spacing;
            Bottom = spacing;
            Left = spacing;
            Right = spacing;
        }

        public RectangleSpacing(int spacing, Size size)
            :this(spacing)
        {
            SetupSizes(size);
        }

        public RectangleSpacing(int vertical, int horizontal)
        {
            Top = vertical;
            Bottom = vertical;
            Left = horizontal;
            Right = horizontal;
        }

        public RectangleSpacing(int vertical, int horizontal, Size size)
            :this(vertical, horizontal)
        {
            SetupSizes(size);
        }

        public RectangleSpacing(int top, int bottom, int left, int right)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }

        public RectangleSpacing(int top, int bottom, int left, int right, Size size)
            :this(top, bottom, left, right)
        {
            SetupSizes(size);
        }

        private void SetupSizes(Size size)
        {
            AdjustedWidth = size.Width - (Left + Right);
            AdjustedHeight = size.Height - (Top + Bottom);
        }
    }
}