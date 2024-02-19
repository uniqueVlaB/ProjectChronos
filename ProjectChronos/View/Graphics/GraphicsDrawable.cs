using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectChronos.View.Graphics
{
    public class GraphicsDrawable: IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Teal;
            canvas.StrokeSize = 4;
            canvas.DrawArc(10, 10, 100, 100, 0, 30, false, false);
        }
    }
}
