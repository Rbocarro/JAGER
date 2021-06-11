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
    class Level2
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

        private enum Level2States { intro, wave1, wave2, wave3, boss, end };
        private int wave1count = 12;
        private int bossCount = 1;
        private Level2States levelState;

        private int levelScore;

        private Song levelSong;

        public Level2(ContentManager content, int screenWidth, int screenHeight, int speed)
        {
            playerShip = new Player(content, screenWidth, screenHeight);

            this.content = content;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            enemyManager = new EnemyManager(content, playerShip);
            gui = new GUI(content, playerShip);
            r = new Random();
            sg = new ScrollingBackground(content, "Models/LondonBlock");
            levelStartBG = content.Load<Texture2D>("Sprites/GUI/ldn_start");
            levellEndBG = content.Load<Texture2D>("Sprites/GUI/ldn_end");


            font = content.Load<SpriteFont>("Fonts/Debug");
            levelSong = content.Load<Song>("Sound/bgm/bgm2");

            levelState = Level2States.intro;

        }

        public void Update(GameTime gameTime, ref GameState baseGaGameState, ref int globalScore)
        {
            sg.Update();
            playerShip.Update(screenWidth, screenHeight, gameTime);
            enemyManager.Update(gameTime, playerShip, ref globalScore);
            levelScore = globalScore;


            switch (levelState)
            {

                case Level2States.intro:
                    MediaPlayer.Play(levelSong);
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue += mFadeIncrement;
                        if (mAlphaValue >= 1)
                        {
                            levelState = Level2States.wave1;
                            playerShip.PlayerEnter = true;

                        }
                    }
                    break;
                case Level2States.wave1:
                    if ((r.Next(1, 500) <= 1) && playerShip.Health <= 5) { enemyManager.GenerateHealthPickup(1); }
                    if (r.Next(1, 2000) <= 1) { enemyManager.GenerateBonusPickup(1); }
                    if (enemyManager.IntermediateEnemyList.Count <= 0 & Convert.ToBoolean(wave1count))
                    {
                        enemyManager.GenerateIntermediate(5);
                        wave1count--;
                    }
                    if (wave1count == 0 & enemyManager.IntermediateEnemyList.Count == 0)
                    {
                        levelState = Level2States.boss;
                    }
                    break;
                case Level2States.boss:
                    if ((r.Next(1, 500) <= 1) && playerShip.Health <= 5) { enemyManager.GenerateHealthPickup(1); }
                    if (enemyManager.BossList.Count <= 0 & Convert.ToBoolean(bossCount))
                    {
                        enemyManager.GenerateBoss(1);
                        bossCount--;
                    }
                    if (bossCount == 0 & enemyManager.BossList.Count == 0)
                    {
                        levelState = Level2States.end;

                    }

                    break;
                case Level2States.end:
                    playerShip.PlayerExit = true;
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue -= mFadeIncrement;
                        if (mAlphaValue <= 0)
                        {
                            MediaPlayer.Stop();
                            baseGaGameState = GameState.Level3;
                        }
                    }
                    break;

            }
            if (playerShip.Health <= 0)
            {
                MediaPlayer.Stop();
                baseGaGameState = GameState.GameOver;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            sg.Draw();
            playerShip.Draw(spriteBatch);
            gui.Draw(spriteBatch, levelScore);
            enemyManager.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Wave 1 count:" + wave1count, new Vector2(30, 900), Color.Red);
            switch (levelState)
            {

                case Level2States.intro:
                    spriteBatch.Draw(levelStartBG, new Rectangle(0, 0, 1920, 1080), Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;
                case Level2States.wave1:

                    break;
                case Level2States.end:
                    spriteBatch.Draw(levellEndBG,
                                    new Rectangle(0, 0, 1920, 1080),
                                    Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;

            }
        }

    }
}
