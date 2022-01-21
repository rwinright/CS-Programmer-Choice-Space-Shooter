using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_Shooter.Engine
{
    public class UIText
    {
        public Vector2 Position;
        public string Text;
        public Font DrawFont;
        public SolidBrush DrawBrush = new SolidBrush(Color.White);
        public StringFormat drawFormat = new StringFormat();

        public UIText(string text, Vector2 position)
        {
            Text = text;
            Position = position;
            DrawFont = new Font("Arial", 16);
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
        }
        public UIText(string text, Vector2 position, int fontSize, Color fontColor)
        {
            this.Text = text;
            this.Position = position;
            DrawFont = new Font("Arial", fontSize);
            DrawBrush = new SolidBrush(fontColor);
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            Engine.RegisterText(this);
        }
        public void DestroySelf()
        {
            Engine.RemoveText(this);
        }
    }
}
