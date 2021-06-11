using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace JAGER
{
    public class GUI
    {
        ContentManager content;

        SpriteFont font;
        SpriteFont scoreFont;
        Texture2D guiOverlay;
        Texture2D Heart;
        Player player;
        


        public GUI(ContentManager content, Player player)
        {
            this.content = content;
            font = this.content.Load<SpriteFont>("Fonts/Debug");
            scoreFont= this.content.Load<SpriteFont>("Fonts/scorefont");
            Heart = this.content.Load<Texture2D>("Sprites/GUI/Heart");
            guiOverlay = this.content.Load<Texture2D>("Sprites/GUI/GUIOverlay");

            this.player = player;

        }

        public void Draw(SpriteBatch spriteBatch, int score)
        {
            spriteBatch.Draw(guiOverlay, new Rectangle(0, 0, 1920, 1080), Color.White);

            spriteBatch.DrawString(scoreFont, "SCORE:"+score, new Vector2(104, 75), Color.White);
            spriteBatch.DrawString(scoreFont, "HEALTH", new Vector2(104, 495), Color.White);
            for (int i = 0; i < player.Health; i++)
            {
                spriteBatch.Draw(Heart, new Rectangle(100 + ((Heart.Width) * i), 520, (Heart.Width), (Heart.Height)), Color.White);
            }

            //spriteBatch.DrawString(font, "Current Player State:" + player.PlayerState.ToString(), new Vector2(100, 50), Color.White);
            //spriteBatch.DrawString(font, "Current Player pos X:" + player.Position.X.ToString(), new Vector2(100, 70), Color.White);
            //spriteBatch.DrawString(font, "Current Player pos Y:" + player.Position.Y.ToString(), new Vector2(100, 90), Color.White);
            //spriteBatch.DrawString(font, "Bullet Count:" + player.BulletList.Count.ToString(), new Vector2(100, 230), Color.White);
        }

    }
}
