using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Text;

namespace JAGER
{
    public static class DrawHelper
    {

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, GraphicsDevice graphics)
        {
            Texture2D t = new Texture2D(graphics, 1, 1);
            t.SetData<Color>(new Color[] { Color.White });

            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =(float)Math.Atan2(edge.Y, edge.X);
            spriteBatch.Draw(t,
                            new Rectangle(// rectangle defines shape of line and position of start of line
                            (int)start.X,
                            (int)start.Y,
                            (int)edge.Length(), //sb will strech the texture to fill this rectangle
                            7), //width of line, change this to make thicker line
                            null,
                            Color.Magenta, //colour of line
                            angle,     //angle of line (calulated above)
                            new Vector2(0, 0), // point in line about which to rotate
                            SpriteEffects.None,
                            0);
        }


    }
}
