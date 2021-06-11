using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace JAGER
{
    public class TutorialLevel
    {
        private int screenWidth, screenHeight;
        private GUI gui;
        private ScrollingBackground water;
        private Player playerShip;
        private ContentManager content;
        private SpriteFont font;


        private Texture2D tutorialStartBG;
        private Texture2D tutorialEndBG;
        private Texture2D goodJobBG;
        float mAlphaValue = 0;
        float mFadeIncrement = .01f;
        double mFadeDelay = .025;

        private enum TutorialStates { intro, forward,forwardComp, back,backComp, left,leftComp, right,rightComp, fire,fireComp, red,redComp, blue,blueComp, end };
        private TutorialStates tutorialState;
        
        private int levelScore;

        private Song TutorialSong;


        public TutorialLevel(ContentManager content, int screenWidth, int screenHeight, int speed)
        {
            playerShip = new Player(content, screenWidth, screenHeight);

            this.content = content;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            gui = new GUI(content, playerShip);
            water = new ScrollingBackground(content, "Models/WaterBlock");

            tutorialStartBG = content.Load<Texture2D>("Sprites/GUI/tut_start");
            tutorialEndBG = content.Load<Texture2D>("Sprites/GUI/tut_end");
            goodJobBG= content.Load<Texture2D>("Sprites/GUI/gj_bg");


            font = content.Load<SpriteFont>("Fonts/tutorialBlack");
            TutorialSong = content.Load<Song>("Sound/bgm/bgm2");

            tutorialState = TutorialStates.intro;

        }

        public void Update(GameTime gameTime, ref GameState baseGaGameState,ref int globalScore)
        {
            playerShip.Update(screenWidth, screenHeight, gameTime);
            water.Update();
            levelScore = globalScore;
            switch (tutorialState)
            {
                case TutorialStates.intro:
                    MediaPlayer.Play(TutorialSong);
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue += mFadeIncrement;
                        if (mAlphaValue >= 1)
                        {
                            mAlphaValue = 0;
                            mFadeIncrement = .01f;
                            mFadeDelay = .025;
                            tutorialState = TutorialStates.forward;
                            playerShip.PlayerEnter = true;

                        }
                    }
                    break;
                case TutorialStates.forward:
                    if (Keyboard.GetState().IsKeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickUp)) 
                        tutorialState = TutorialStates.forwardComp;
                    break;
                case TutorialStates.forwardComp:
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue += mFadeIncrement+0.03f;
                        if (mAlphaValue >= 1)
                        {
                            mAlphaValue = 0;
                            mFadeIncrement = .01f;
                            mFadeDelay = .025;
                            tutorialState = TutorialStates.back;
                        }
                    }
                    break;
                case TutorialStates.back:
                    if (Keyboard.GetState().IsKeyDown(Keys.S) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown))
                        tutorialState = TutorialStates.backComp;
                    break;
                case TutorialStates.backComp:
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue += mFadeIncrement + 0.03f;
                        if (mAlphaValue >= 1)
                        {
                            mAlphaValue = 0;
                            mFadeIncrement = .01f;
                            mFadeDelay = .025;
                            tutorialState = TutorialStates.left;
                        }
                    }
                    break;
                case TutorialStates.left:
                    if (Keyboard.GetState().IsKeyDown(Keys.A) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickLeft))
                        tutorialState = TutorialStates.leftComp;
                    break;
                case TutorialStates.leftComp:
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue += mFadeIncrement + 0.03f;
                        if (mAlphaValue >= 1)
                        {
                            mAlphaValue = 0;
                            mFadeIncrement = .01f;
                            mFadeDelay = .025;
                            tutorialState = TutorialStates.right;
                        }
                    }
                    break;
                case TutorialStates.right:
                    if (Keyboard.GetState().IsKeyDown(Keys.D) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickRight))
                        tutorialState = TutorialStates.rightComp;
                    break;
                case TutorialStates.rightComp:
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue += mFadeIncrement + 0.03f;
                        if (mAlphaValue >= 1)
                        {
                            mAlphaValue = 0;
                            mFadeIncrement = .01f;
                            mFadeDelay = .025;
                            tutorialState = TutorialStates.blue;
                        }
                    }
                    break;
                case TutorialStates.blue:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
                        tutorialState = TutorialStates.blueComp;
                    break;
                case TutorialStates.blueComp:
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue += mFadeIncrement + 0.03f;
                        if (mAlphaValue >= 1)
                        {
                            mAlphaValue = 0;
                            mFadeIncrement = .01f;
                            mFadeDelay = .025;
                            tutorialState = TutorialStates.red;
                        }
                    }
                    break;
                case TutorialStates.red:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
                        tutorialState = TutorialStates.redComp;
                    break;
                case TutorialStates.redComp:
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue += mFadeIncrement + 0.03f;
                        if (mAlphaValue >= 1)
                        {
                            mAlphaValue = 0;
                            mFadeIncrement = .01f;
                            mFadeDelay = .025;
                            tutorialState = TutorialStates.fire;
                        }
                    }
                    break;
                case TutorialStates.fire:
                    if (playerShip.BulletList.Count>1)
                        tutorialState = TutorialStates.fireComp;
                    break;
                case TutorialStates.fireComp:
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue += mFadeIncrement + 0.03f;
                        if (mAlphaValue >= 1)
                        {
                            mAlphaValue = 1;
                            mFadeIncrement = .01f;
                            mFadeDelay = .025;
                            tutorialState = TutorialStates.end;
                        }
                    }
                    break;
                case TutorialStates.end:
                    playerShip.PlayerExit = true;
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = .025;
                        mAlphaValue -= mFadeIncrement;
                        if (mAlphaValue <= 0)
                        {
                            MediaPlayer.Stop();
                            baseGaGameState = GameState.Level1;
                        }
                    }
                    break;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            water.Draw();
            playerShip.Draw(spriteBatch);
            gui.Draw(spriteBatch,levelScore);

            //spriteBatch.DrawString(font, "Playerpos:" + playerShip.centerPosition, new Vector2(10, 50), Color.White);

            switch (tutorialState)
            {
                case TutorialStates.intro:
                    spriteBatch.Draw(tutorialStartBG, 
                                     new Rectangle(0, 0, 1920, 1080), 
                                     Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;
                case TutorialStates.forward:
                    spriteBatch.DrawString(font, "Press W or Left Joystick UP \n       to move forwards", new Vector2(600, 500), Color.White);
                    
                    break;
                case TutorialStates.forwardComp:
                    spriteBatch.Draw(goodJobBG,
                                     new Rectangle(480, 250, 960, 540),
                                     Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;
                case TutorialStates.back:
                    spriteBatch.DrawString(font, "Press S or Left Joystick DOWN \n         to move backwards", new Vector2(575, 500), Color.White);
                    break;
                case TutorialStates.backComp:
                    spriteBatch.Draw(goodJobBG,
                                     new Rectangle(480, 250, 960, 540),
                                     Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;
                case TutorialStates.left:
                    spriteBatch.DrawString(font, "Press A or Left Joystick LEFT\n               to move left", new Vector2(575, 500), Color.White);
                    break;
                case TutorialStates.leftComp:
                    spriteBatch.Draw(goodJobBG,
                                     new Rectangle(480, 250, 960, 540),
                                     Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;
                case TutorialStates.right:
                    spriteBatch.DrawString(font, "Press D or Left Joystick RIGHT\n              to move right", new Vector2(575, 500), Color.White);
                    break;
                case TutorialStates.rightComp:
                    spriteBatch.Draw(goodJobBG,
                                     new Rectangle(480, 250, 960, 540),
                                     Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;
                case TutorialStates.red:
                    spriteBatch.DrawString(font, "Press Spacebar or Gamepad X to \n        Switch back to blue", new Vector2(555, 500), Color.White);
                    break;
                case TutorialStates.redComp:
                    spriteBatch.Draw(goodJobBG,
                                     new Rectangle(480, 250, 960, 540),
                                     Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;
                case TutorialStates.fire:
                    spriteBatch.DrawString(font, "Press J or Gamepad R2\n    to fire bullets", new Vector2(650, 500), Color.White);
                    break;
                case TutorialStates.fireComp:
                    spriteBatch.Draw(goodJobBG,
                                     new Rectangle(480, 250, 960, 540),
                                     Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;
                case TutorialStates.blue:
                    spriteBatch.DrawString(font, "Press Spacebar or Gamepad X to \n      Switch your state to red", new Vector2(555, 500), Color.White);
                    break;
                case TutorialStates.blueComp:
                    spriteBatch.Draw(goodJobBG,
                                     new Rectangle(480, 250, 960, 540),
                                     Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;
                case TutorialStates.end:
                    spriteBatch.Draw(tutorialEndBG, 
                                     new Rectangle(0, 0, 1920, 1080), 
                                     Color.Lerp(Color.White, Color.Transparent, mAlphaValue));
                    break;

            }

        }



    }
}
