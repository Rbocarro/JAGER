using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;


namespace JAGER
{
    class Level3
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

        private enum Level3States { intro, wave1, wave2, wave3, boss, end };
        private int wave1count = 15;
        private int bossCount = 1;
        private Level3States levelState;

        private int levelScore;

        private Song levelSong;

        public Level3(ContentManager content, int screenWidth, int screenHeight, int speed)
        {
            playerShip = new Player(content, screenWidth, screenHeight);

            this.content = content;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            enemyManager = new EnemyManager(content, playerShip);
            gui = new GUI(content, playerShip);
            r = new Random();
            sg = new ScrollingBackground(content, "Models/TokyoBlock");
            levelStartBG = content.Load<Texture2D>("Sprites/GUI/tokyo_start");
            levellEndBG = content.Load<Texture2D>("Sprites/GUI/tokyo_end");


            font = content.Load<SpriteFont>("Fonts/Debug");
            levelSong = content.Load<Song>("Sound/bgm/bgm2");

            levelState = Level3States.intro;

        }

        public void Update(GameTime gameTime, ref GameState baseGaGameState, ref int globalScore)
        {
            sg.Update();
            playerShip.Update(screenWidth, screenHeight, gameTime);
            enemyManager.Update(gameTime, playerShip, ref globalScore);
            levelScore = globalScore;


            switch (levelState)
            {

                case Level3States.intro:
                    MediaPlayer.Play(levelSong);
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue += mFadeIncrement;
                        if (mAlphaValue >= 1)
                        {
                            levelState = Level3States.wave1;
                            playerShip.PlayerEnter = true;

                        }
                    }
                    break;
                case Level3States.wave1:
                    if((r.Next(1, 500) <= 1) && playerShip.Health <= 5) { enemyManager.GenerateHealthPickup(1); }
                    if (r.Next(1, 2000) <= 1) { enemyManager.GenerateBonusPickup(1); }
                    if (enemyManager.AdvancedenemyList.Count <= 0 & Convert.ToBoolean(wave1count))
                    {
                        enemyManager.GenerateAdvanced(6);
                        wave1count--;
                    }
                    if (wave1count == 0 & enemyManager.AdvancedenemyList.Count == 0)
                    {
                        levelState = Level3States.boss;
                    }
                    break;
                case Level3States.boss:
                    if((r.Next(1, 500) <= 1) && playerShip.Health <= 5) { enemyManager.GenerateHealthPickup(1); }
                    if (enemyManager.BossList.Count <= 0 & Convert.ToBoolean(bossCount))
                    {
                        enemyManager.GenerateBoss(1);
                        bossCount--;
                    }
                    if (bossCount == 0 & enemyManager.BossList.Count == 0)
                    {
                        levelState = Level3States.end;

                    }

                    break;
                case Level3States.end:
                    playerShip.PlayerExit = true;
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue -= mFadeIncrement;
                        if (mAlphaValue <= 0)
                        {
                            MediaPlayer.Stop();
                            baseGaGameState = GameState.Credits;
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

                case Level3States.intro:
                    spriteBatch.Draw(levelStartBG, new Rectangle(0, 0, 1920, 1080), Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;
                case Level3States.wave1:

                    break;
                case Level3States.end:
                    spriteBatch.Draw(levellEndBG,
                                    new Rectangle(0, 0, 1920, 1080),
                                    Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;

            }
        }




    }
}
