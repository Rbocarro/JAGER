using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace JAGER
{
    class AdvancedEnemy
    {

        private Texture2D texture;
        private Rectangle destRect;
        private Rectangle sourceRect;
        
        public Vector2 currentEnemyPos;
        public Vector2 nextEnemypos;
        Vector2 TargetPos;

        private float elapsed;
        private float delay = 60f;
        private int frames = 0;

        private ContentManager content;
        private Player player;

        public static Random r;

        public int enemySpeed;

        private State enemyState;

        int scale = 4;

        private List<EnemyBullet> bulletList;
        private int bulletDelay;

        private enum ActionState { enterscreen, stage1, }
        private ActionState enemyActionState;
        private SoundEffect laserSound;

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

        public AdvancedEnemy(ContentManager content, Player player)
        {
            this.content = content;
            this.player = player;
            r = new Random();
            enemyActionState = ActionState.enterscreen;
            enemyState = (r.Next(0, 2) == 1) ? State.Red : State.Blue;
            switch (enemyState)
            {
                case State.Red:
                    texture = this.content.Load<Texture2D>("sprites/game/advanced_red");
                    break;
                case State.Blue:
                    texture = this.content.Load<Texture2D>("sprites/game/advanced_blue");
                    break;
            }
            laserSound = this.content.Load<SoundEffect>("sound/soundeffects/laserSmall_001");
            nextEnemypos = new Vector2(r.Next(556, 1366 - destRect.Width), r.Next(0, 100));
            currentEnemyPos = new Vector2(r.Next(555, 1365 - destRect.Width), r.Next(-texture.Height * 2, -texture.Height));
            TargetPos = new Vector2(r.Next(556, 1366 - destRect.Width), r.Next(0, 100));
            destRect = new Rectangle((int)currentEnemyPos.X, (int)currentEnemyPos.Y, (texture.Width / scale) / 10, (texture.Height / scale));
            
            bulletDelay = 15;
            enemySpeed = r.Next(4, 6);
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
            sourceRect = new Rectangle(frames * (texture.Width / 10), 0, (texture.Width / 10), 315);




            switch(enemyActionState)
            {
                case ActionState.enterscreen:
                    if (Vector2.Distance(currentEnemyPos, TargetPos) >= 1f + enemySpeed)
                    {
                        Vector2 posDelta = TargetPos - currentEnemyPos;
                        posDelta.Normalize();
                        posDelta *= enemySpeed;
                        currentEnemyPos += posDelta;
                    }
                    else
                        enemyActionState= ActionState.stage1;
                    break;
                case ActionState.stage1:
                    if (Vector2.Distance(currentEnemyPos, nextEnemypos) >= ((float)enemySpeed + 1f))
                    {

                        Vector2 posDelta = nextEnemypos - currentEnemyPos;
                        posDelta.Normalize();
                        posDelta *= enemySpeed;
                        currentEnemyPos += posDelta;
                    }

                    else if (Vector2.Distance(currentEnemyPos, nextEnemypos) < ((float)enemySpeed + 1f))
                    {
                        nextEnemypos = new Vector2(r.Next(556, 1366 - destRect.Width-10), r.Next(0, 300));
                    }

                    if (r.Next(0, 30) > 1) { FireBullet(); }
                    break;
            }





            destRect = new Rectangle((int)currentEnemyPos.X, (int)currentEnemyPos.Y, (texture.Width / scale) / 10, (texture.Height / scale));



            



            for (int i = 0; i < bulletList.Count; i++)
            {
                if (bulletList[i].Active == false)
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
                bulletList.Add(new EnemyBullet(content, EnemyState, currentEnemyPos, player, 2, enemySpeed + 5));
                laserSound.Play(1,-1,1);
            }
            if (bulletDelay == 0)
                bulletDelay = 50;
        }
    }
}
