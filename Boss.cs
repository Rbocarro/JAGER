using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace JAGER
{
    class Boss
    {
        private Texture2D texture;
        private Rectangle destRect;

        private Rectangle sourceRect;
        private float elapsed;
        private float delay = 60f;
        private int frames = 0;


        public Vector2 currentEnemyPos;
        public Vector2 nextEnemypos;

        private Player player;

        private int health;
        private int maxHealth;
        private Texture2D HealthbarTexture;
        private Rectangle HealthBarRectangle;

        private int healthBarHeight = 25;
        private int healthBarWidth = 750;

        private List<EnemyBullet> bulletList;
        private int bulletDelay;

        private ContentManager content;//will need to use later on to switch sprites

        private SpriteFont font;

        private SoundEffect laserSound;

        public static Random r = new Random();

        public int enemySpeed = 3;

        private enum BossActionState {enterscreen,stage1,die }
        private BossActionState bossActionState;
        private State enemyState;

        int scale = 1;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Rectangle Position
        {
            get { return destRect; }
        }

        public State BossState
        {
            get { return enemyState; }
        }

        public Boss(ContentManager con, Player player)
        {
            this.content = con;
            this.player = player;
            bulletList = new List<EnemyBullet>();
            enemyState = (r.Next(0, 2) == 1) ? State.Red : State.Blue;
            switch (enemyState)
            {
                case State.Red:
                    texture = this.content.Load<Texture2D>("sprites/game/boss_red");
                    break;
                case State.Blue:
                    texture = this.content.Load<Texture2D>("sprites/game/boss_blue");
                    break;
            }
            maxHealth = health = 200;
            HealthbarTexture = this.content.Load<Texture2D>("Sprites/game/1x1WhitePixel");
            font = this.content.Load<SpriteFont>("Fonts/Debug");
            laserSound = this.content.Load<SoundEffect>("sound/soundeffects/bullet_shot");
            bulletDelay = 18;
            bossActionState = BossActionState.enterscreen;
            HealthBarRectangle = new Rectangle(585, 1000, Convert.ToInt32(healthBarWidth * (health / (float)maxHealth)), healthBarHeight);
            currentEnemyPos = new Vector2(575, -300);
            nextEnemypos = new Vector2(r.Next(556, 1366 - destRect.Width), r.Next(50, 300));
            destRect = new Rectangle((int)currentEnemyPos.X,(int)currentEnemyPos.Y, (texture.Width / 10)*scale, texture.Height *scale);
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
            sourceRect = new Rectangle(frames * (texture.Width / 10), 0, (texture.Width / 10), 269);



            switch(bossActionState)
            {
                case BossActionState.enterscreen:
                        Vector2 TargetPos = new Vector2((1920 / 2)-(destRect.Width/2), 150);
                    if (Vector2.Distance(currentEnemyPos, TargetPos) >= 1f + enemySpeed)
                    {
                        Vector2 posDelta = TargetPos - currentEnemyPos;
                        posDelta.Normalize();
                        posDelta *= enemySpeed;
                        currentEnemyPos += posDelta;
                    }
                    else
                        bossActionState = BossActionState.stage1;
                    break;
                case BossActionState.stage1:
                    if (r.Next(0, 500) <= 4)
                    {
                        int k = r.Next(0, 2);
                        switch (k)
                        {
                            case 0:
                                enemyState = State.Red;
                                break;
                            case 1:
                                enemyState = State.Blue;
                                break;
                        }
                    }
                    if (Vector2.Distance(currentEnemyPos, nextEnemypos) >= ((float)enemySpeed + 1f))
                    {

                        Vector2 posDelta = nextEnemypos - currentEnemyPos;
                        posDelta.Normalize();
                        posDelta *= enemySpeed;
                        currentEnemyPos += posDelta;
                    }

                    else if (Vector2.Distance(currentEnemyPos, nextEnemypos) < ((float)enemySpeed + 1f))
                    {
                        nextEnemypos = new Vector2(r.Next(556, 1366-destRect.Width), r.Next(0, 100));
                    }

                    if(r.Next(0,20)>3)
                    {
                        FireBullet();
                    }

                    for (int i = 0; i < bulletList.Count; i++)
                    {
                        if (bulletList[i].Active == false)
                        {
                            bulletList.RemoveAt(i);
                        }
                    }

                    break;
            }

            switch (enemyState)
            {
                case State.Red:
                    texture = this.content.Load<Texture2D>("sprites/game/boss_red");
                    break;
                case State.Blue:
                    texture = this.content.Load<Texture2D>("sprites/game/boss_blue");
                    break;
            }
            foreach (EnemyBullet b in bulletList)
            {
                b.Update(gameTime);
            }

            HealthBarRectangle = new Rectangle(585, 1000, Convert.ToInt32(healthBarWidth * (health / (float)maxHealth)), healthBarHeight);
            destRect = new Rectangle((int)currentEnemyPos.X, (int)currentEnemyPos.Y, (texture.Width / 10) * scale, texture.Height * scale);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
            spriteBatch.DrawString(font, "Boss", new Vector2(585, 975), Color.White);
            spriteBatch.Draw(HealthbarTexture, HealthBarRectangle, Color.Red);
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
                bulletList.Add(new EnemyBullet(content, enemyState, new Vector2(currentEnemyPos.X+(destRect.Width/2),currentEnemyPos.Y+(destRect.Width-200)), player, 1)); ;
                laserSound.Play(1, 1, 0);
            }
            if (bulletDelay == 0)
                bulletDelay = 18;
        }

    }
}
