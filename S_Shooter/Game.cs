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
    class Game : Engine.Engine
    {
        Sprite2D player;
        Sprite2D bb;
        Sprite2D brandon;
        Sprite2D alan;

        Collision2D coll;

        UIText scoreText;

        List<Shape2D> bullets = new List<Shape2D>();
        List<Sprite2D> enemies = new List<Sprite2D>();

        int score = 0;
        bool left, right, up, down, shooting;
        public Game() : base(new Vector2(600, 500), "Space Shooter")
        {

        }

        public override void OnLoad()
        {
            BgColor = Color.Black;

            //Player sprites
            player = new Sprite2D(new Vector2(300 - 32, 400), new Vector2(32, 32), "Ships/ship", "player");
            //Enemy sprites

            coll = new Collision2D();
            scoreText = new UIText($"Score:{score}", new Vector2(100, 100), 16, Color.White);
        }

        public override void OnDraw()
        {
        }

        //Calculate time between bullets
        int bulletFrames = 0;
        int timeBetweenEnemies = 0;
        int frames = 0;
        public override void OnUpdate()
        {
            int moveVertical = (this.down ? 1 : 0) - (this.up ? 1 : 0);
            int moveHorizontal = (this.right ? 1 : 0) - (this.left ? 1 : 0);

            if (this.player.Position.X < 0) this.player.Position.X = 0;
            if (this.player.Position.X > 600 - 48) this.player.Position.X = 600 - 48;
            if (this.player.Position.Y < 0) this.player.Position.Y = 0;
            if (this.player.Position.Y > 500 - 78) this.player.Position.Y = 500 - 78;

            this.player.Position.X += moveHorizontal * 1;
            this.player.Position.Y += moveVertical * 1;

            this.frames++;
            this.timeBetweenEnemies++;

            if (this.shooting)
            {
                //Increment while shooting button is held down.
                this.bulletFrames++;
                if (this.bulletFrames > 250 && bullets.Count < 5 && this.enemies.Count > 0)
                {
                    this.Shoot();
                    //reset to 0 to start the count over again
                    this.bulletFrames = 0;
                }
            }
            foreach (Sprite2D enemy in enemies)
            {
                if (enemy == null) return;
                
                if (enemy.Position.Y >= 500)
                {
                    scoreText.DestroySelf();
                    scoreText = new UIText($"GAME OVER, your score was {score}", new Vector2(500, 10), 24, Color.Red);
                    Engine.Engine.pauseGame = true;
                }

                enemy.Position.Y += 0.5f;
                if (enemy.Tag == "enemy-0")
                {
                    enemy.Position.X += (float)Math.Cos(this.frames / 100) * 0.5f;
                }
            }
            foreach (Shape2D bullet in this.bullets)
            {
                if (bullet == null) return;
                bullet.Position.Y -= 4;
                foreach (Sprite2D enemy in enemies)
                {
                    if (enemy == null) return;
                    if (coll.Collides(bullet.Position, enemy.Position, bullet.Scale, enemy.Scale))
                    {
                        ++score;
                        scoreText.DestroySelf();
                        scoreText = new UIText($"Score:{score}", new Vector2(100, 100), 16, Color.White);

                        bullet.DestroySelf();
                        bullets.Remove(bullet);
                        enemy.DestroySelf();
                        enemies.Remove(enemy);

                    }
                }
                if (bullet.Position.Y < 10)
                {
                    bullets.Remove(bullet);
                    bullet.DestroySelf();
                }
            }

            //Instantiate enemies
            if (timeBetweenEnemies > 1000)
            {
                Random randomSpawnLocation = new Random();

                Random randomEnemy = new Random();
                randomEnemy.Next(1, 3);
                switch (randomEnemy.Next(1, 4))
                {
                    case 1:
                        alan = new Sprite2D(new Vector2(randomSpawnLocation.Next(32, 468), -32), new Vector2(32, 32), "Enemies/alan", "enemy-2");
                        enemies.Add(alan);
                        break;
                    case 2:
                        bb = new Sprite2D(new Vector2(randomSpawnLocation.Next(32, 468), -32), new Vector2(32, 32), "Enemies/bb", "enemy-0");
                        enemies.Add(bb);
                        break;
                    case 3:
                        brandon = new Sprite2D(new Vector2(randomSpawnLocation.Next(32, 468), -32), new Vector2(32, 32), "Enemies/brandon", "enemy-1");
                        enemies.Add(brandon);
                        break;
                }
                timeBetweenEnemies = 0;
            }
        }

        bool leftShoot = false;
        public void Shoot()
        {
            //Instantiate bullet to bullets array.
            //Projectiles
            leftShoot = !leftShoot;
            Shape2D bullet = new Shape2D(
                    new Vector2(((player.Position.X + player.Scale.X / 2) - 4) - (leftShoot ? 8 : -8), player.Position.Y + player.Scale.Y - 8),
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