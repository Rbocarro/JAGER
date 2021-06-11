using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JAGER
{
    class ScrollingBackground
    {
        private Model model;
        private Vector3[] modelPosArray;
        private float speed = 8;

        private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 450, -250), new Vector3(0, 0, 0), Vector3.UnitY);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(35), 1920f / 1080f, 0.1f, 5000f);

        public ScrollingBackground(ContentManager content,String filePath)
        {
            model = content.Load<Model>(filePath);
            modelPosArray = new Vector3[4];
            for (int i = 0; i < modelPosArray.Length; i++)
            {
                modelPosArray[i] = new Vector3(0, 0, (i * 400)-400);
            }
        }

        public void Update()
        {
            for (int i = 0; i < modelPosArray.Length; i++)
            {
                modelPosArray[i].Z += -speed;

                if (modelPosArray[i].Z <= -800)
                {
                    modelPosArray[i].Z = 400;
                }

            }



        }

        public void Draw()
        {
            for (int i = 0; i < modelPosArray.Length; i++)
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.World = Matrix.CreateTranslation(modelPosArray[i]);
                        effect.View = view;
                        effect.Projection = projection;
                    }

                    mesh.Draw();
                }

            }
        }

    }
}
