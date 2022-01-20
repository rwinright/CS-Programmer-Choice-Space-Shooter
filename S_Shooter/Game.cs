using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using S_Shooter.Engine;
using System.Windows.Forms;

namespace S_Shooter
{
    class Game:Engine.Engine
    {
        Sprite2D player;
        Sprite2D bb;
        Sprite2D brandon;
        Sprite2D alan;

        Collision coll;

  

        List<Shape2D> bullets = new List<Shape2D>();

        bool left, right, up, down, shooting;
        public Game() : base(new Vector2(600, 500), "Space Shooter")
        {
            
        }

        public override void OnLoad()
        {
            BgColor = Color.Black;

            //Player sprites
            player = new Sprite2D(new Vector2(10, 10), new Vector2(32, 32), "Ships/ship", "player");
            //Enemy sprites
            bb = new Sprite2D(new Vector2(10, player.Position.Y + 32), new Vector2(32, 32), "Enemies/bb", "enemy-0");
            brandon = new Sprite2D(new Vector2(10, bb.Position.Y + 32), new Vector2(32, 32), "Enemies/brandon", "enemy-1");
            alan = new Sprite2D(new Vector2(10, brandon.Position.Y + 32), new Vector2(32, 32), "Enemies/alan", "enemy-2");

            coll = new Collision(); 
        }

        public override void OnDraw()
        {

        }
        
        //Calculate time between bullets
        int bulletFrames = 0;
        public override void OnUpdate()
        {

            int moveVertical = (this.down ? 1 : 0) - (this.up ? 1 : 0);
            int moveHorizontal = (this.right ? 1 : 0) - (this.left ? 1 : 0);

            this.player.Position.X += moveHorizontal * 10;
            this.player.Position.Y += moveVertical * 10;
            
            if(this.shooting)
            {
                //Increment while shooting button is held down.
                this.bulletFrames++;
                if(this.bulletFrames > 10 && bullets.Count < 5)
                {
                    this.Shoot();
                    //reset to 0 to start the count over again
                    this.bulletFrames = 0;
                }
            }
            foreach(Shape2D bullet in this.bullets)
            {
                bullet.Position.Y -= 4;
                if (bullet.Position.Y < 0 || coll.Collides(bullet.Position, alan.Position, bullet.Scale, alan.Scale))
                {
                    bullet.DestroySelf();
                    bullets.Remove(bullet);
                }
            }
        }

        public void Shoot()
        {
            //Instantiate bullet to bullets array.
            //Projectiles
            Shape2D bullet = new Shape2D(
                    new Vector2((player.Position.X + player.Scale.X / 2) + 4, player.Position.Y + player.Scale.Y),
                    new Vector2(8, 8),
                    "player-bullet"
                );
            bullets.Add(bullet);
        }

        public override void GetKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up) this.up = true;
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left) this.left = true;
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right) this.right = true;
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down) this.down = true;
            if (e.KeyCode == Keys.Space) this.shooting = true;
        }

        public override void GetKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up) this.up = false;
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left) this.left = false;
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right) this.right = false;
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down) this.down = false;
            if (e.KeyCode == Keys.Space) this.shooting = false;
        }


    }
}
