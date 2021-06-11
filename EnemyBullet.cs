using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace JAGER
{
    public class EnemyBullet
    {
        private Texture2D texture;
        private State bulletState;
        private Player player;
        private int scale;

        private Rectangle destRect;
        private Rectangle sourceRect;
        private Vector2 bulletPos;
        private Vector2 bulletVelocity;
        private bool bulletActive;
        Vector2 posDelta;

        private float elapsed;
        private float delay = 30f;
        int frames = 0;

        private int speed = 7;

        public Rectangle DestRec
        {
            get { return destRect; }
        }

        public State BulletState
        {
            get { return bulletState; }
        }

        public bool Active
        {
            get { return bulletActive; }
        }

        public EnemyBullet(ContentManager content, State state, Vector2 pos, Player player, int bulletScale)
        {

            bulletState = state;
            this.player = player;
            scale = bulletScale;
            switch (bulletState)
            {
                case State.Blue:
                    texture = content.Load<Texture2D>("Sprites/game/bullet_animation_blue");
                    break;
                case State.Red:
                    texture = content.Load<Texture2D>("Sprites/game/bullet_animation_red");
                    break;
            }
            bulletPos = pos;

            bulletActive = true;
            posDelta = new Vector2();
            bulletVelocity = new Vector2();
            posDelta = player.CenterPosition - bulletPos;
            posDelta.Normalize();
            posDelta *= speed;
            bulletPos += posDelta;
            bulletVelocity = posDelta;
            //destRect = new Rectangle((int)bulletPos.X, (int)bulletPos.Y, texture.Width / 10, texture.Height / 10);
            destRect = new Rectangle((int)bulletPos.X, (int)bulletPos.Y,( texture.Width/6)/scale , texture.Height/scale );
        }

        public EnemyBullet(ContentManager content, State state, Vector2 pos, Player player, int bulletScale,int bulletSpeed)
        {
            
            bulletState = state;
            this.player = player;
            scale = bulletScale;
            switch (bulletState)
            {
                case State.Blue:
                    texture = content.Load<Texture2D>("Sprites/game/bullet_animation_blue");
                    break;
                case State.Red:
                    texture = content.Load<Texture2D>("Sprites/game/bullet_animation_red");
                    break;
            }
            bulletPos = pos;
            speed = bulletSpeed;
            bulletActive = true;
            posDelta = new Vector2();
            bulletVelocity = new Vector2();
            posDelta = player.CenterPosition - bulletPos;
            posDelta.Normalize();
            posDelta *= speed;
            bulletPos += posDelta;
            bulletVelocity = posDelta;
            //destRect = new Rectangle((int)bulletPos.X, (int)bulletPos.Y, texture.Width / 10, texture.Height / 10);
            destRect = new Rectangle((int)bulletPos.X, (int)bulletPos.Y, (texture.Width / 6) / scale, texture.Height / scale);
        }

        public void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
                if (elapsed >= delay)
                {
                    if (frames >= 5)
                    {
                        frames = 0;
                    }
                    else
                        frames++;
                    elapsed = 0;
                }
            sourceRect = new Rectangle(frames * (texture.Width / 6), 0, (texture.Width / 6), texture.Height);

            //Vector2 bulletVelocity = new Vector2();
            //Vector2 posDelta = player.CenterPosition - bulletPos;
            //posDelta.Normalize();
            //posDelta *= speed;

            //if (Vector2.Distance(bulletPos, player.CenterPosition) >= 250 && bulletTracking)
            //{


            //    bulletPos += posDelta;
            //    bulletVelocity = posDelta;


            //}
            //else
            //{
            //    bulletTracking = false;

            //}
            bulletPos += bulletVelocity;

            

            if (destRect.Intersects(player.Hitbox) && bulletState != player.PlayerState)
            {
                player.Health--;
                bulletActive = false;
            }
            destRect = new Rectangle((int)bulletPos.X, (int)bulletPos.Y,( texture.Width / 6)/scale, texture.Height/scale);

            if(destRect.Y>2000|| destRect.Y < -100||destRect.X>2000||destRect.X<-100)
                
            {
                bulletActive = false;
            }

        }

        public void Draw(SpriteBatch sprietBatch)
        {
            sprietBatch.Draw(texture, DestRec,sourceRect, Color.White);
        }
    }
}
