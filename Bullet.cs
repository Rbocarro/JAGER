using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JAGER
{
    public class Bullet
    {
        private Texture2D texture;
        private Rectangle destRect;
        private Rectangle sourceRect;


        private float elapsed;
        private float delay = 30f;
        int frames = 0;

        private int speed = 15;


        private State bulletState;

        public int YPos
        {
            get { return destRect.Y; }
        }

        public Rectangle DestRec
        {
            get { return destRect; }
        }

        public State BulletState
        {
            get { return bulletState; }
        }



        public Bullet(State state, ContentManager content, Vector2 playerPosition)//add velocity to curve
        {
            bulletState = state;
            if (state == State.Blue)
            {
                texture = content.Load<Texture2D>("Sprites/game/bluebullet");
            }
            else if (state == State.Red)
            {
                texture = content.Load<Texture2D>("Sprites/game/redbullet");
            }
            destRect = new Rectangle((int)playerPosition.X - ((texture.Width / 10) / 2), (int)playerPosition.Y - 100, (texture.Width / 10), texture.Height);



        }


        public void Update(GameTime gameTime)
        {
            destRect.Y -= speed;
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
                if (elapsed >= delay)
                {
                    if (frames >= 2)
                    {
                        frames = 0;
                    }
                    else
                        frames++;
                    elapsed = 0;
                }
            sourceRect = new Rectangle(frames * (texture.Width / 10), 0, texture.Width / 10, texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
        }
    }
}
