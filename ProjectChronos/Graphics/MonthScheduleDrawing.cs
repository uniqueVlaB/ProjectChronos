using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Google.Crypto.Tink.Signature;


namespace ProjectChronos.Graphics
    {
        public class MonthScheduleDrawing : IDrawable
        {
        public int page;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.White;
            canvas.FillRectangle(dirtyRect);
            canvas.FontColor = Colors.Black;
            canvas.DrawString(page.ToString(),50,50,HorizontalAlignment.Center);
        }
    }
    }

