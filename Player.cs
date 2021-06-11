using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace JAGER
{
    public class Player
    {
        private Texture2D texture;          //player ship texture
        private Rectangle destRect;         //the actual rectangle the ship is drawn to
        private Rectangle sourceRect;       //this is basically that cycles through each animation frame of the player
        private Rectangle collisionRect;
        private Rectangle GameBoundsRect;
        public Vector2 centerPosition;

        private ContentManager content;

        private State playerState;
        private bool playerControllable;
        private bool playerEnter;
        private bool playerExit;

        private int playerSpeed = 9;
        private int playerHealth;


        private float elapsed;
        private float delay = 60f;
        int frames = 0;

        int playerscale = 4;

        private KeyboardState previousKeyboardState;
        private GamePadState previousGamepadState;



        private List<Bullet> bulletList;
        private int bulletDelay;

        private SoundEffect laserSound;

        private Texture2D Hitboxtex;

        public Vector2 CenterPosition
        {
            get { return centerPosition; }
        }
        public Rectangle Position
        {
            get { return destRect; }
        }
        public int Health
        {
            get { return playerHealth; }
            set { playerHealth = value; }
        }

        public List<Bullet> BulletList
        {
            get { return bulletList; }
            set { bulletList = value; }
        }

        public State PlayerState
        {
            get { return playerState; }
        }
        public bool PlayerEnter
        {
            get { return playerEnter; }
            set { playerEnter = value; }
        }
        public bool PlayerExit
        {
            get { return playerExit; }
            set { playerExit = value; }
        }

        public Rectangle Hitbox
        {
            get { return collisionRect; }
        }


        public Player(ContentManager con, int screenWidth, int screenHeight)
        {
            GameBoundsRect = new Rectangle(555, 0, 810, screenHeight);
            content = con;
            texture = content.Load<Texture2D>("Sprites/game/spritesheetblue");
            laserSound = content.Load<SoundEffect>("Sound/soundeffects/bullet_shot");
            Hitboxtex = content.Load<Texture2D>("Sprites/game/1x1WhitePixel");
            centerPosition = new Vector2(screenWidth / 2, 1200);
            destRect = new Rectangle((int)centerPosition.X - ((350 / playerscale) / 2),
                                     (int)centerPosition.Y - ((450 / playerscale) / 2),
                                     350 / playerscale,
                                     450 / playerscale);
            collisionRect = new Rectangle((int)centerPosition.X - 25, (int)centerPosition.Y - 25, 50, 50);
            previousKeyboardState = Keyboard.GetState();
            previousGamepadState = GamePad.GetState(PlayerIndex.One);
            playerControllable = false;
            playerEnter = false;
            playerExit = false;
            playerState = State.Blue;
            playerHealth = 10;
            bulletDelay = 10;
            bulletList = new List<Bullet>();

        }
        public void Initiaize()
        {
            previousKeyboardState = Keyboard.GetState();

        }

        public void Update(int screenWidth, int screenHeight, GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            GamePadState gpadstate = GamePad.GetState(PlayerIndex.One);
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
            sourceRect = new Rectangle(frames * 350, 0, 350, 450);

            if (playerEnter)
            {
                PlayerEnterScreen(screenWidth);
            }
            if (playerExit)
            {
                PlayerExitScreen(screenWidth);
            }

            if (playerControllable)
            {
                if (((state.IsKeyDown(Keys.Space) & state != previousKeyboardState) || (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) & gpadstate != previousGamepadState))
                    & playerState == State.Blue )
                {
                    playerState = State.Red;
                    texture = content.Load<Texture2D>("Sprites/game/spritesheetred");
                }
                else if (((state.IsKeyDown(Keys.Space) & state != previousKeyboardState) || (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) & gpadstate != previousGamepadState))
                    & playerState == State.Red )
                {
                    playerState = State.Blue;
                    texture = content.Load<Texture2D>("Sprites/game/spritesheetblue");

                }

                if (Keyboard.GetState().IsKeyDown(Keys.D)||GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickRight))
                {
                    centerPosition.X += playerSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickLeft))
                {
                    centerPosition.X -= playerSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickUp))
                {
                    centerPosition.Y -= playerSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown))
                {
                    centerPosition.Y += playerSpeed;
                }


                if ((Keyboard.GetState().IsKeyDown(Keys.J)|| GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightTrigger) )&& 
                    (playerState == State.Red || playerState == State.Blue))
                {

                    FireBullet();
                }
                ClearBulletsOffScreen();





                if (centerPosition.X <= GameBoundsRect.X + ((350 / playerscale) / 2))
                {
                    centerPosition.X = GameBoundsRect.X + ((350 / playerscale) / 2);
                }
                if (centerPosition.X >= (GameBoundsRect.X + GameBoundsRect.Width) - (350 / playerscale) / 2)
                {
                    centerPosition.X = (GameBoundsRect.X + GameBoundsRect.Width) - (350 / playerscale) / 2;
                }
                if (centerPosition.Y <= ((450 / playerscale) / 2))
                {
                    centerPosition.Y = (450 / playerscale) / 2;
                }
                if (centerPosition.Y >= screenHeight - ((450 / playerscale) / 2))
                {
                    centerPosition.Y = screenHeight - ((450 / playerscale) / 2);
                }
            }
            foreach (Bullet b in bulletList)
            {
                b.Update(gameTime);
            }
            destRect = new Rectangle((int)centerPosition.X - ((350 / playerscale) / 2),
                                     (int)centerPosition.Y - ((450 / playerscale) / 2),
                                      350 / playerscale,
                                      450 / playerscale);
            collisionRect = new Rectangle((int)centerPosition.X - 13, (int)centerPosition.Y - 30, 26, 40);
            previousGamepadState = gpadstate;
            previousKeyboardState = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
            //spriteBatch.Draw(Hitboxtex, collisionRect, Color.Red);//uncomment this line to see the playership hitbox


            foreach (Bullet b in bulletList)
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
                bulletList.Add(new Bullet(playerState, content, this.centerPosition));
                laserSound.Play();
            }
            if (bulletDelay == 0)
                bulletDelay = 10;
        }

        public void ClearBulletsOffScreen()
        {
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (bulletList[i].YPos <= 0)
                {
                    bulletList.RemoveAt(i);
                }
            }
        }


        public void PlayerEnterScreen(int screenWidth)
        {

            //centerPosition = new Vector2(screenWidth / 2, 300);
            Vector2 TargetPos = new Vector2(screenWidth / 2, 900);
            if (Vector2.Distance(centerPosition, TargetPos) >= 1f + playerSpeed)
            {
                Vector2 posDelta = TargetPos - centerPosition;
                posDelta.Normalize();
                posDelta *= playerSpeed;
                centerPosition += posDelta;

            }
            else
            {
                playerControllable = true;
                playerEnter = false;
            }
        }
        public void PlayerExitScreen(int screenWidth)
        {
            playerControllable = false;
            Vector2 TargetPos = new Vector2(screenWidth / 2, -300);
            if (Vector2.Distance(centerPosition, TargetPos) >= 1f + playerSpeed)
            {
                Vector2 posDelta = TargetPos - centerPosition;
                posDelta.Normalize();
                posDelta *= playerSpeed;
                centerPosition += posDelta;

            }

        }


    }
}
