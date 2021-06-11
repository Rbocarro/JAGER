using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace JAGER
{
    public class Explosion
    {
        private float elapsed;
        private float delay = 60f;
        private int frames = 0;

        private int scale;
        private bool active;

        private Rectangle destRect;
        private Rectangle sourceRect;
        private Texture2D texture;
        

        public bool Active
        {
            get { return active; }
        }

        public Explosion(ContentManager content, int scale, Vector2 position)
        {
            texture = content.Load<Texture2D>("sprites/game/explosion_animation");
            
            destRect = new Rectangle((int)position.X,(int)position.Y, (texture.Width / 10 / scale),texture.Height/scale);
            active = true;
        }

        public void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
                if (elapsed >= delay)
                {
                    if (frames >= 9)
                    {
                        active = false;
                        //frames = 0;
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
