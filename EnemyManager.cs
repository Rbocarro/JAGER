using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace JAGER
{
    class EnemyManager
    {

        private List<BasicEnemy> basicenemyList;
        private List<IntermediateEnemy> intermediateenemyList;
        private List<AdvancedEnemy> advancedenemyList;
        private List<Boss> bossList;
        private List<HealthPickup> healthPickupList;
        private List<BonusPickup> bonusPickupList;

        private List<Explosion> explosionList;


        private SoundEffect explosionSound;
        private SoundEffect powerupSound;



        private Player player;
        ContentManager content;

        public List<BasicEnemy> BasicEnemyList
        {
            get { return basicenemyList; }
            set { basicenemyList = value; }
        }
        public List<IntermediateEnemy> IntermediateEnemyList
        {
            get { return intermediateenemyList; }
            set { intermediateenemyList = value; }
        }
        public List<AdvancedEnemy> AdvancedenemyList
        {
            get { return advancedenemyList; }
            set { advancedenemyList = value; }
        }

        public List<Boss> BossList
        {
            get { return bossList; }
            set { bossList = value; }
        }

        public List<HealthPickup> HealthPickupList
        {
            get { return healthPickupList; }
            set { healthPickupList = value; }
        }

        public List<BonusPickup> BonusPickupList
        {

            get { return bonusPickupList; }
            set { bonusPickupList = value; }
        }
        public EnemyManager(ContentManager content, Player player)
        {
            this.player = player;
            this.content = content;
            basicenemyList = new List<BasicEnemy>();
            intermediateenemyList = new List<IntermediateEnemy>();
            advancedenemyList = new List<AdvancedEnemy>();
            bossList = new List<Boss>();
            explosionList = new List<Explosion>();
            healthPickupList = new List<HealthPickup>();
            bonusPickupList = new List<BonusPickup>();
            explosionSound = content.Load<SoundEffect>("sound/soundeffects/explosionCrunch_000");
            powerupSound= content.Load<SoundEffect>("sound/soundeffects/poweup");
        }


        public void Update(GameTime gameTime, Player player, ref int globalScore)
        {

            foreach (BasicEnemy e in basicenemyList)
            {
                e.Update(gameTime);
            }

            foreach(IntermediateEnemy e in intermediateenemyList)
            {
                e.Update(gameTime);
            }

            foreach(AdvancedEnemy e in advancedenemyList)
            {
                e.Update(gameTime);
            }

            foreach (Boss b in bossList)
            {
                b.Update(gameTime);
            }

            foreach(HealthPickup h in healthPickupList)
            {
                h.Update(gameTime,player);
            }

            foreach (BonusPickup b in bonusPickupList)
            {
                b.Update(gameTime, player);
            }

            foreach (Explosion e in explosionList)
            {
                e.Update(gameTime);

            }
            for (int i=0;i<explosionList.Count;i++)
            {
                if (explosionList[i].Active == false) 
                    explosionList.RemoveAt(i);
            }

            for (int i = 0; i < healthPickupList.Count; i++)
            {
                if (healthPickupList[i].Active == false)
                    powerupSound.Play();
                    healthPickupList.RemoveAt(i);
            }

            for (int i = 0; i < bonusPickupList.Count; i++)
            {
                if (bonusPickupList[i].Active == false&& bonusPickupList[i].Ypos<1080)
                {
                    globalScore += 5000;
                    powerupSound.Play();
                    bonusPickupList.RemoveAt(i);
                }
                else if(bonusPickupList[i].Active == false)
                {
                    bonusPickupList.RemoveAt(i);
                }

            }

            for (int i = 0; i < player.BulletList.Count; i++)
            {
                for (int j = 0; j < basicenemyList.Count; j++)
                {
                    if (player.BulletList[i].DestRec.Intersects(basicenemyList[j].CollisionRect) &&
                        player.BulletList[i].BulletState != basicenemyList[j].EnemyState)
                    { 
                        explosionList.Add(new Explosion(content,2, basicenemyList[j].Position));
                        explosionSound.Play();
                        basicenemyList.RemoveAt(j);
                        globalScore += 100;
                    }

                }
                for (int j = 0; j < intermediateenemyList.Count; j++)
                {
                    if (player.BulletList[i].DestRec.Intersects(intermediateenemyList[j].CollisionRect) &&
                        player.BulletList[i].BulletState != intermediateenemyList[j].EnemyState)
                    {
                        explosionList.Add(new Explosion(content, 2, intermediateenemyList[j].Position));
                        explosionSound.Play();
                        intermediateenemyList.RemoveAt(j);
                        globalScore += 200;
                    }

                }
                for (int j = 0; j < advancedenemyList.Count; j++)
                {
                    if (player.BulletList[i].DestRec.Intersects(advancedenemyList[j].CollisionRect) &&
                        player.BulletList[i].BulletState != advancedenemyList[j].EnemyState)
                    {
                        explosionList.Add(new Explosion(content, 2, advancedenemyList[j].Position));
                        explosionSound.Play();
                        advancedenemyList.RemoveAt(j);
                        globalScore += 300;
                    }

                }


                for (int k = 0; k < bossList.Count; k++)
                {
                    if (player.BulletList[i].DestRec.Intersects(bossList[k].Position) && player.BulletList[i].BulletState != bossList[k].BossState)
                    {
                        if (bossList[k].Health > 0)
                        {
                            bossList[k].Health--;
                            player.BulletList.RemoveAt(k);
                        }
                        else
                        {
                            explosionList.Add(new Explosion(content, 1, new Vector2(bossList[k].Position.X, bossList[k].Position.Y)));
                            explosionSound.Play(1, -1, 0);
                            BossList.RemoveAt(k);
                            globalScore += 500;
                        }

                    }
                }
            }


            for (int j = 0; j < basicenemyList.Count; j++)
            {
                if (player.Hitbox.Intersects(basicenemyList[j].CollisionRect) && player.PlayerState != basicenemyList[j].EnemyState)
                {
                    --player.Health;
                    explosionList.Add(new Explosion(content, 1, new Vector2(basicenemyList[j].Position.X, basicenemyList[j].Position.Y)));
                    basicenemyList.RemoveAt(j);
                    j = basicenemyList.Count;
                    
                }

            }

            for (int j = 0; j < intermediateenemyList.Count; j++)
            {
                if (player.Hitbox.Intersects(intermediateenemyList[j].CollisionRect) && player.PlayerState != intermediateenemyList[j].EnemyState)
                {
                    --player.Health;
                    explosionList.Add(new Explosion(content, 1, new Vector2(intermediateenemyList[j].Position.X, intermediateenemyList[j].Position.Y)));
                    intermediateenemyList.RemoveAt(j);
                    j = intermediateenemyList.Count;

                }

            }
            for (int j = 0; j < advancedenemyList.Count; j++)
            {
                if (player.Hitbox.Intersects(advancedenemyList[j].CollisionRect) && player.PlayerState != advancedenemyList[j].EnemyState)
                {
                    --player.Health;
                    explosionList.Add(new Explosion(content, 1, new Vector2(advancedenemyList[j].Position.X, advancedenemyList[j].Position.Y)));
                    advancedenemyList.RemoveAt(j);
                    j = advancedenemyList.Count;

                }

            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (BasicEnemy e in basicenemyList)
            {
                e.Draw(spriteBatch);
            }

            foreach(IntermediateEnemy e in intermediateenemyList)
            {
                e.Draw(spriteBatch);
            }

            foreach(AdvancedEnemy e in advancedenemyList)
            {
                e.Draw(spriteBatch);
            }

            foreach (Boss b in bossList)
            {
                b.Draw(spriteBatch);
            }

            foreach(Explosion e in explosionList)
            {
                e.Draw(spriteBatch);
            }

            foreach(HealthPickup h in healthPickupList)
            {
                h.Draw(spriteBatch);
            }

            foreach (BonusPickup b in bonusPickupList)
            {
                b.Draw(spriteBatch);
            }
        }



        public void GenerateBasicEnemies(int noOfEnemies)
        {

            for (int i = 0; i < noOfEnemies; i++)
            {
                basicenemyList.Add(new BasicEnemy(content, player));
            }
        }

        public void GenerateIntermediate(int noOfEnemies)
        {

            for (int i = 0; i < noOfEnemies; i++)
            {
                intermediateenemyList.Add(new IntermediateEnemy(content, player));
            }
        }
        public void GenerateAdvanced(int noOfEnemies)
        {

            for (int i = 0; i < noOfEnemies; i++)
            {
                advancedenemyList.Add(new AdvancedEnemy(content, player));
            }
        }

        public void GenerateBoss(int noOfBosses)
        {
            for (int i = 0; i < noOfBosses; i++)
            {
                bossList.Add(new Boss(content, player));
            }
        }

        public void GenerateHealthPickup(int noOfHealthPickup)
        {
            for (int i = 0; i < noOfHealthPickup; i++)
            {
                healthPickupList.Add(new HealthPickup(content));
            }
        }

        public void GenerateBonusPickup(int noOfBonusPickup)
        {
            for (int i = 0; i < noOfBonusPickup; i++)
            {
                bonusPickupList.Add(new BonusPickup(content));
            }
        }


    }
}
