using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace JAGER
{
    class BonusPickup
    {
        private Texture2D texture;
        private Rectangle destRect;
        private Rectangle sourceRect;
        private Vector2 currentPos;
        private ContentManager content;
        private Random r;
        private int itemSpeed;
        private bool active;

        private float elapsed;
        private float delay = 60f;
        private int frames = 0;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
        public int Ypos
        {
            get { return (int)currentPos.Y; }
        }

        public BonusPickup(ContentManager content)
        {
            this.content = content;
            r = new Random();
            active = true;
            texture  = this.content.Load<Texture2D>("Sprites/game/bonuspowerup");
            currentPos = new Vector2(r.Next(556, 1365 - destRect.Width - 50), r.Next(-texture.Height * 2, -texture.Height));
            destRect = new Rectangle((int)currentPos.X, (int)currentPos.Y, texture.Width/10, texture.Height);
            itemSpeed = 6;
        }

        public void Update(GameTime gameTime, Player playerShip)
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
            sourceRect = new Rectangle(frames * (texture.Width / 10), 0, (texture.Width / 10), texture.Height);
            currentPos.Y += itemSpeed;
            destRect = new Rectangle((int)currentPos.X, (int)currentPos.Y, texture.Width / 10, texture.Height);

            if (playerShip.Position.Intersects(this.destRect))
            {
                this.active = false;
            }

            if (destRect.Y > 1200) this.active = false;


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destRect,sourceRect, Color.White);
        }

    }
}
