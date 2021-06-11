using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace JAGER
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameState gameState;
        private GameState previousGameState;
        private KeyboardState previousKeyboardState;
        private GamePadState previousGamePadState;



        private TutorialLevel tutorialLevel;
        private Level1 level1;
        private Level2 level2;
        private Level3 level3;

        public int globalScore;

        private Texture2D misato;//test texture
        private Texture2D mainMenuBG;
        private Texture2D pauseMenuBG;
        private Texture2D CreditsBG;
        private Texture2D gameOverBG;

        private Video intro;
        private VideoPlayer videoPlayer;
        private bool videoStart;

        private SpriteFont font;
        private SpriteFont largeFont;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = true;
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 1920;//game screen width
            graphics.PreferredBackBufferHeight = 1080;// game screen height
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameState = GameState.MainMenu;
            
            InitialiseLevels();
            misato = Content.Load<Texture2D>("Sprites/game/misato");
            mainMenuBG = Content.Load<Texture2D>("Sprites/GUI/MainMenuScreen");
            pauseMenuBG = Content.Load<Texture2D>("Sprites/GUI/PauseScreen");
            CreditsBG = Content.Load<Texture2D>("Sprites/GUI/CreditsScreenBG");
            gameOverBG= Content.Load<Texture2D>("Sprites/GUI/GameOver");

            font = Content.Load<SpriteFont>("fonts/Debug");
            largeFont= Content.Load<SpriteFont>("fonts/FinalScoreFont");


            intro = Content.Load<Video>("video/intro");
            videoPlayer = new VideoPlayer();
            videoPlayer.IsLooped = false;
            videoStart = true;


            previousKeyboardState = Keyboard.GetState();
            previousGamePadState = GamePad.GetState(PlayerIndex.One);


        }

        protected override void Update(GameTime gameTime)
        {
            Window.Title = gameState.ToString();
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamepadState= GamePad.GetState(PlayerIndex.One);
            switch (gameState)
            {
                case GameState.MainMenu:
                    InitialiseLevels();
                    if ((Keyboard.GetState().IsKeyDown(Keys.Enter) & keyboardState != previousKeyboardState)
                       ||(GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A)& gamepadState!=previousGamePadState))
                    {
                        gameState = GameState.VideoPlayer;
                    }
                    break;
                case GameState.VideoPlayer:
                    if (videoStart)
                    {   
                        videoPlayer.Play(intro);
                        videoStart = false;
                    } 
                   if ((Keyboard.GetState().IsKeyDown(Keys.Enter) &keyboardState != previousKeyboardState)
                     ||(GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) & gamepadState != previousGamePadState)
                     || videoPlayer.State==MediaState.Stopped)
                    {
                        videoPlayer.Stop();
                        gameState = GameState.Tutorial;
                    }
                    break;
                case GameState.Paused:
                    if ((Keyboard.GetState().IsKeyDown(Keys.Escape) & keyboardState != previousKeyboardState)
                     || (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start)&& gamepadState != previousGamePadState))
                    {
                        gameState = previousGameState;
                        MediaPlayer.Resume();
                    }
                    else if ((Keyboard.GetState().IsKeyDown(Keys.K) & keyboardState != previousKeyboardState)
                    || (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Y) && gamepadState != previousGamePadState))
                    {
                        gameState = GameState.MainMenu;
                    }
                    break;
                case GameState.Tutorial:
                    tutorialLevel.Update(gameTime, ref gameState, ref globalScore);
                    if ((Keyboard.GetState().IsKeyDown(Keys.Escape)                   & keyboardState != previousKeyboardState)
                    ||  (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start)& gamepadState  != previousGamePadState))
                    {
                        gameState = GameState.Paused;
                        previousGameState = GameState.Tutorial;
                        MediaPlayer.Pause();
                    }
                    break;
                case GameState.Level1:
                    level1.Update(gameTime, ref gameState,ref globalScore);
                    if ((Keyboard.GetState().IsKeyDown(Keys.Escape) & keyboardState != previousKeyboardState)
                      ||(GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) & gamepadState != previousGamePadState))
                    {
                        gameState = GameState.Paused;
                        previousGameState = GameState.Level1;
                        MediaPlayer.Pause();
                    }
                    break;
                case GameState.Level2:
                    level2.Update(gameTime, ref gameState, ref globalScore);
                    if ((Keyboard.GetState().IsKeyDown(Keys.Escape)                    & keyboardState!= previousKeyboardState)
                     || (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) & gamepadState != previousGamePadState))
                    {
                        gameState = GameState.Paused;
                        previousGameState = GameState.Level2;
                        MediaPlayer.Pause();
                    }
                    break;
                case GameState.Level3:
                    level3.Update(gameTime, ref gameState, ref globalScore);
                    if ((Keyboard.GetState().IsKeyDown(Keys.Escape)                    & keyboardState!= previousKeyboardState)
                     || (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) & gamepadState != previousGamePadState))
                    {
                        gameState = GameState.Paused;
                        previousGameState = GameState.Level3;
                        MediaPlayer.Pause();
                    }
                    break;

                case GameState.Credits:
                    if ((Keyboard.GetState().IsKeyDown(Keys.Escape)                    & keyboardState!= previousKeyboardState)
                     || (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) & gamepadState != previousGamePadState))
                    {
                        Exit();
                    }
                    if ((Keyboard.GetState().IsKeyDown(Keys.J)                    & keyboardState!= previousKeyboardState)
                    || (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) & gamepadState != previousGamePadState))
                    {
                        gameState = GameState.MainMenu;
                    }
                    break;
                case GameState.GameOver:
                    if ((Keyboard.GetState().IsKeyDown(Keys.Enter)                     & keyboardState!= previousKeyboardState)
                     || (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) & gamepadState != previousGamePadState))
                    {
                        gameState = GameState.MainMenu;
                    }
                    break;
                default:
                    gameState = GameState.MainMenu;
                    break;
            }
            previousKeyboardState = keyboardState;
            previousGamePadState = gamepadState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred,
                              BlendState.AlphaBlend,
                              SamplerState.PointClamp,
                              DepthStencilState.Default,
                              RasterizerState.CullCounterClockwise);

            switch (gameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(mainMenuBG,
                    new Rectangle(0, 0,
                    graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight),
                    Color.White);
                    break;
                
                case GameState.VideoPlayer:
                    Texture2D videoTexture = null;
                    if (videoPlayer.State != MediaState.Stopped)
                        videoTexture = videoPlayer.GetTexture();
                    if (videoTexture != null)
                        spriteBatch.Draw(videoTexture, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.DrawString(font, "Press Enter or GamePad A to skip", new Vector2(1350, 1000), Color.White);
                    break;
                
                case GameState.Tutorial:
                    tutorialLevel.Draw(spriteBatch);
                    break;
                
                case GameState.Level1:
                    level1.Draw(spriteBatch);
                    break;
                
                case GameState.Level2:
                    level2.Draw(spriteBatch);
                    break;
                
                case GameState.Level3:
                    level3.Draw(spriteBatch);
                    break;

                case GameState.GameOver:
                    spriteBatch.Draw(gameOverBG,
                    new Rectangle(0, 0,
                    graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight),
                    Color.White);
                    break;

                case GameState.Paused:
                    spriteBatch.Draw(pauseMenuBG,
                    new Rectangle(0, 0,
                    graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight),
                    Color.White);
                    spriteBatch.DrawString(font, "Press keyboard K or Gamepad Y to return to Main Menu", new Vector2(550, 900), Color.White);
                    break;
                
                case GameState.Credits:
                    spriteBatch.Draw(CreditsBG, 
                    new Rectangle(0, 0, 
                    graphics.PreferredBackBufferWidth, 
                    graphics.PreferredBackBufferHeight), 
                    Color.White);
                    spriteBatch.DrawString(largeFont, "YOUR SCORE: "+globalScore, new Vector2(600, 300), Color.White);
                    break;
                
                default:
                    gameState = GameState.MainMenu;
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public void InitialiseLevels()
        {
            globalScore = 0;
            tutorialLevel = new TutorialLevel(Content, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 3);//set this to the main menu so a new game starts each time
            level1 = new Level1(Content, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 3);
            level2 = new Level2(Content, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 3);
            level3=new Level3(Content, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 3);
        }

    }
}
