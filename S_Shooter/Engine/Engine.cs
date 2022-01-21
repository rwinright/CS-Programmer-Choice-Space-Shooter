using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace S_Shooter.Engine
{
    class Canvas : Form
    {
        public Canvas()
        {
            this.DoubleBuffered = true;
        }
    }
    public abstract class Engine
    {
        private Vector2 ScreenSize = new Vector2(512, 512);
        private string Title = "Space Shooter";
        private Canvas Window = null;
        private Thread GameLoopThread = null;

        public static bool pauseGame = false;

        private static List<Shape2D> AllShapes = new List<Shape2D>();
        private static List<Sprite2D> AllSprites = new List<Sprite2D>();
        private static List<UIText> AllText = new List<UIText>();

        public Color BgColor = Color.Black;

        public Engine(Vector2 ScreenSize, string Title)
        {
            this.ScreenSize = ScreenSize;
            this.Title = Title;
            Window = new Canvas();
            Window.Size = new Size((int)this.ScreenSize.X, (int)this.ScreenSize.Y);
            Window.Text = this.Title;
            Window.Paint += Renderer;
            Window.KeyUp += Window_KeyUp;
            Window.KeyDown += Window_KeyDown;

            GameLoopThread = new Thread(GameLoop);
            GameLoopThread.Start();
            Application.Run(Window);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            GetKeyDown(e);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            GetKeyUp(e);
        }


        //Register/unregister shapes and sprites to engine
        public static void RegisterShape(Shape2D shape)
        {
            AllShapes.Add(shape);
        }
        public static void RemoveShape(Shape2D shape)
        {
            AllShapes.Remove(shape);
        }
        public static void RegisterSprite(Sprite2D sprite)
        {
            AllSprites.Add(sprite);
        }
        public static void RemoveSprite(Sprite2D sprite)
        {
            AllSprites.Remove(sprite);
        }
        public static void RegisterText(UIText text)
        {
            AllText.Add(text);
        }

        public static void RemoveText(UIText text)
        {
            AllText.Remove(text);
        }

        void GameLoop()
        {
            OnLoad();
            while(GameLoopThread.IsAlive && pauseGame == false)
            {
                try
                {
                    OnDraw();
                    Window.BeginInvoke((MethodInvoker)delegate { Window.Refresh(); });
                    OnUpdate();
                    Thread.Sleep(1);
                }
                catch
                {
                    Log.Error("Window not found, waiting");
                }
            }
        }
        private void Renderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BgColor);
            foreach(Shape2D shape in AllShapes)
            {
                g.FillRectangle(new SolidBrush(Color.Red), shape.Position.X, shape.Position.Y, shape.Scale.X, shape.Scale.Y);
            }

            foreach(Sprite2D sprite in AllSprites)
            {
                if(sprite != null)
                    g.DrawImage(sprite.Sprite, sprite.Position.X, sprite.Position.Y, sprite.Scale.X, sprite.Scale.Y);
            }

            foreach(UIText text in AllText)
            {
                if (!(AllText.Count > 0)) return;
                g.DrawString(text.Text, text.DrawFont, text.DrawBrush, text.Position.X, text.Position.Y, text.drawFormat);
            }
        }

        public abstract void OnLoad();
        public abstract void OnUpdate();
        public abstract void OnDraw();
        public abstract void GetKeyDown(KeyEventArgs e);
        public abstract void GetKeyUp(KeyEventArgs e);
    }
}
