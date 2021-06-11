using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;


namespace JAGER
{
    public class Level1
    {
        private int screenWidth, screenHeight;
        private GUI gui;
        private Random r;

        private ScrollingBackground sg;

        private Player playerShip;
        private EnemyManager enemyManager;
        private ContentManager content;
        private SpriteFont font;

        private Texture2D levelStartBG;
        private Texture2D levellEndBG;
        float mAlphaValue = 0;
        float mFadeIncrement = .01f;
        double mFadeDelay = .025;

        private enum Level1States { intro, wave1, wave2, wave3, boss, end };
        private int wave1count = 10;
        private int bossCount = 1;
        private Level1States levelState;

        private int levelScore;

        private Song levelSong;

        public Level1(ContentManager content, int screenWidth, int screenHeight, int speed)
        {
            playerShip = new Player(content, screenWidth, screenHeight);

            this.content = content;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            enemyManager = new EnemyManager(content, playerShip);
            gui = new GUI(content, playerShip);
            r = new Random();
            sg = new ScrollingBackground(content,"Models/Ny01CityBlocks");
            levelStartBG = content.Load<Texture2D>("Sprites/GUI/ny_start");
            levellEndBG = content.Load<Texture2D>("Sprites/GUI/ny_end");


            font = content.Load<SpriteFont>("Fonts/Debug");
            levelSong = content.Load<Song>("Sound/bgm/bgm2");

            levelState = Level1States.intro;

        }

        public void Update(GameTime gameTime, ref GameState baseGaGameState, ref int globalScore)
        {
            //bg.Update(gameTime);
            sg.Update();
            playerShip.Update(screenWidth, screenHeight, gameTime);
            enemyManager.Update(gameTime, playerShip, ref globalScore);
            levelScore = globalScore;


            switch (levelState)
            {

                case Level1States.intro:
                    MediaPlayer.Play(levelSong);
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue += mFadeIncrement;
                        if (mAlphaValue >= 1)
                        {
                            levelState = Level1States.wave1;
                            playerShip.PlayerEnter = true;

                        }
                    }
                    break;
                case Level1States.wave1:
                    if((r.Next(1,500)<=1)&&playerShip.Health<=5){ enemyManager.GenerateHealthPickup(1); }///generate new random healthpickup if health is less than
                    if (r.Next(1, 2000)<=1) { enemyManager.GenerateBonusPickup(1); }///generate new bonus pickup
                    if (enemyManager.BasicEnemyList.Count <= 0 & Convert.ToBoolean(wave1count))
                    {
                        enemyManager.GenerateBasicEnemies(4);
                        wave1count--;
                    }
                    if (wave1count == 0 & enemyManager.BasicEnemyList.Count == 0)
                    {
                        levelState = Level1States.boss;
                    }
                    break;
                case Level1States.boss:
                    if ((r.Next(1, 500) <= 1) && playerShip.Health <= 5) { enemyManager.GenerateHealthPickup(1); }
                    if (enemyManager.BossList.Count <= 0 & Convert.ToBoolean(bossCount))
                    {
                        enemyManager.GenerateBoss(1);
                        bossCount--;
                    }
                    if (bossCount == 0 & enemyManager.BossList.Count == 0)
                    {
                        levelState = Level1States.end;
                        
                    }

                    break;
                case Level1States.end:
                    playerShip.PlayerExit = true;
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue -= mFadeIncrement;
                        if (mAlphaValue <= 0)
                        {
                            MediaPlayer.Stop();
                            baseGaGameState = GameState.Level2;
                        }
                    }
                    break;

            }
            if (playerShip.Health <=0)
            {
                MediaPlayer.Stop();
                baseGaGameState = GameState.GameOver;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            //bg.Draw(spriteBatch);
            sg.Draw();
            playerShip.Draw(spriteBatch);
            gui.Draw(spriteBatch,levelScore);
            enemyManager.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Wave 1 count:" + wave1count, new Vector2(30, 900), Color.Red);
            switch (levelState)
            {

                case Level1States.intro:
                    spriteBatch.Draw(levelStartBG, new Rectangle(0, 0, 1920, 1080), Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;
                case Level1States.wave1:

                    break;
                case Level1States.end:
                    spriteBatch.Draw(levellEndBG,
                                    new Rectangle(0, 0, 1920, 1080),
                                    Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;

            }
        }


    }
}
