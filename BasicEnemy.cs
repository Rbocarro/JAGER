using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;


namespace JAGER
{
    public class BasicEnemy
    {
        private Texture2D texture;
        private Rectangle destRect;
        private Rectangle sourceRect;
        private Vector2 currentEnemyPos;

        private float elapsed;
        private float delay = 60f;
        private int frames = 0;

        private ContentManager content;
        private Player player;

        public static Random r;

        private SoundEffect laserSound;

        public int enemySpeed;

        private State enemyState;

        int scale = 4;

        private List<EnemyBullet> bulletList;
        private int bulletDelay;

        public State EnemyState
        {
            get { return enemyState; }

        }
        public Rectangle CollisionRect
        {
            get { return destRect; }

        }

        public Vector2 Position
        {
            get { return currentEnemyPos; }
        }

        public BasicEnemy(ContentManager content, Player player)
        {
            this.content = content;
            this.player = player;
            r = new Random();
            enemyState = (r.Next(0, 2) == 1) ? State.Red : State.Blue;
            switch (enemyState)
            {
                case State.Red:
                    texture = this.content.Load<Texture2D>("sprites/game/basic_red");
                    break;
                case State.Blue:
                    texture = this.content.Load<Texture2D>("sprites/game/basic_blue");
                    break;
            }
            laserSound = this.content.Load<SoundEffect>("sound/soundeffects/laserSmall_001");
            destRect = new Rectangle((int)currentEnemyPos.X, (int)currentEnemyPos.Y, (texture.Width / scale) / 10, (texture.Height / scale));
            currentEnemyPos = new Vector2(r.Next(555, 1365 - destRect.Width), r.Next(-texture.Height * 2, -texture.Height));
            bulletDelay = 10;
            enemySpeed = r.Next(5, 8);
            bulletList = new List<EnemyBullet>();
        }

        public void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
                if (elapsed >= delay)
                {
                    if (frames >= 9)
                    {
                        frames = 0;
                    }
                    else
                        frames++;
                    elapsed = 0;
                }
            sourceRect = new Rectangle(frames * (texture.Width/10), 0, (texture.Width / 10), 315);
            currentEnemyPos.Y += enemySpeed;

            switch (enemyState)
            {
                case State.Red:
                    currentEnemyPos.X += (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5) * 3;
                    break;
                case State.Blue:
                    currentEnemyPos.X += (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 5) * 3;
                    break;
            }



            if (currentEnemyPos.X <= 555)
            {
                currentEnemyPos.X = 555;
            }
            if (currentEnemyPos.X + destRect.Width >= 1365)
            {
                currentEnemyPos.X = 1365 - destRect.Width;
            }

            if (currentEnemyPos.Y >= 1080)
            {
                currentEnemyPos = new Vector2(r.Next(555, 1365 - destRect.Width), -texture.Height);
            }

            destRect = new Rectangle((int)currentEnemyPos.X, (int)currentEnemyPos.Y, (texture.Width / scale) / 10, (texture.Height / scale));



            if (currentEnemyPos.Y >= 100 && currentEnemyPos.Y <= 200)
            {
                if (r.Next(1, 11) >= 5) { FireBullet(); }
            }


            for(int i=0;i<bulletList.Count;i++)
            {   if(bulletList[i].Active==false)
                {
                    bulletList.RemoveAt(i);
                } 
            }
            
            foreach (EnemyBullet b in bulletList)
            {
                b.Update(gameTime);
            }



        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);

            foreach (EnemyBullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }

        }

        public void FireBullet()
        {
            if (bulletDelay > 0)
                bulletDelay--;
            if (bulletDelay <= 0)
            {
                bulletList.Add(new EnemyBullet(content, EnemyState, currentEnemyPos, player,2)); ;
                laserSound.Play(1, -1, 0);
            }
            if (bulletDelay == 0)
                bulletDelay = 10;
        }
    }
}
