using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace JAGER
{
    class HealthPickup
    {

        private Texture2D texture;
        private Rectangle destRect;
        private ContentManager content;
        private Random r;
        private int itemSpeed;
        private bool active;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public HealthPickup(ContentManager content)
        {
            this.content = content;
            r = new Random();
            active = true;
            texture= texture = this.content.Load<Texture2D>("Sprites/GUI/Heart");
            destRect = new Rectangle(r.Next(555, 1365 - destRect.Width-50), r.Next(-texture.Height * 2, -texture.Height),texture.Width,texture.Height);
            itemSpeed = 6;
        }

        public void Update(GameTime gameTime, Player playerShip)
        {
            destRect.Y += itemSpeed;

            if(playerShip.Position.Intersects(this.destRect))
            {
                playerShip.Health = 10;
                this.active = false;
            }

            if (destRect.Y > 1200) this.active = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destRect, Color.White);
        }

        
    }
}
