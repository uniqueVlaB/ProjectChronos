using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectChronos.Graphics
{
    public class CompleteCircleDrawing: IDrawable
    {
        public double NumOfCompletedTasks;
        public double TotalTasks;
        
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            double part = NumOfCompletedTasks / TotalTasks;
        //    PathF path = new PathF();
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 4;
            canvas.DrawArc(dirtyRect.Width - 40, 2, 20, 20, 90, 360, false, false);
            // canvas.DrawCircle(dirtyRect.Width-30,dirtyRect.Height-30,20);
            canvas.StrokeColor = Colors.Green;
            canvas.StrokeSize = 4;
            canvas.DrawArc(dirtyRect.Width - 40, 2, 20, 20, 90, (float)((360*part)+90), false, false);
            canvas.FontSize = 12;
            canvas.FontColor = Colors.White;
            canvas.DrawString($"{(short)(part*100)}%", dirtyRect.Width - 29, 10, HorizontalAlignment.Left);


        }
    }
}
