using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace OneSpace
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class OneSpace : Microsoft.Xna.Framework.Game
    {
        public static OneSpace theGame;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        public GamePadState[] OldGamePadState = new GamePadState[4];
        public GamePadState[] NewGamePadState = new GamePadState[4];
        public KeyboardState OldKeyboardState;
        public KeyboardState NewKeyboardState;

        public Level level;

        public List<Ship> AllShips = new List<Ship>();

        public OneSpace()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            theGame = this;

            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteFont = Content.Load<SpriteFont>("Font");

            Player._motherShipTex = Content.Load<Texture2D>("motherShip");
            Ship._smallShipTex = Content.Load<Texture2D>("smallShip");
            Ship._mediumShipTex = Content.Load<Texture2D>("mediumShip");
            Ship._largeShipTex = Content.Load<Texture2D>("largeShip");
            Ship._lightTex = Content.Load<Texture2D>("colouredLight");
            Level.BackgroundTex = Content.Load<Texture2D>("background");
            Laser.Pixel = Content.Load<Texture2D>("whitePix");

            IconList.Icons[0] = Content.Load<Texture2D>("emChg");
            IconList.Icons[1] = Content.Load<Texture2D>("smallIcon");
            IconList.Icons[2] = Content.Load<Texture2D>("mediumIcon");
            IconList.Icons[3] = Content.Load<Texture2D>("largeIcon");
            IconList.Icons[4] = Content.Load<Texture2D>("em");
            IconList.Icons[5] = Content.Load<Texture2D>("pwrPlus");
            IconList.Icons[6] = Content.Load<Texture2D>("spdPlus");


            bool[] h = {true, false, false, false};
            level = new Level(h);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            NewGamePadState[0] = GamePad.GetState(PlayerIndex.One);
            NewGamePadState[1] = GamePad.GetState(PlayerIndex.Two);
            NewGamePadState[2] = GamePad.GetState(PlayerIndex.Three);
            NewGamePadState[3] = GamePad.GetState(PlayerIndex.Four);
            NewKeyboardState = Keyboard.GetState();

            level.Update(gameTime);

            for (int i = 0; i < 4; i++)
                OldGamePadState[i] = NewGamePadState[i];
            OldKeyboardState = NewKeyboardState;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //XNA performs a lot better when the same texture is drawn as a batch.
            spriteBatch.Begin();

            level.Draw(spriteBatch);

            foreach (Player p in level.Players)
                p.Draw(spriteBatch);

            foreach (Ship s in AllShips)
                s.Laser.Draw(spriteBatch);

            foreach (Ship s in AllShips)
                if (s.Type == ShipType.Large) s.DrawSelf(spriteBatch);

            foreach (Ship s in AllShips)
                if (s.Type == ShipType.Medium) s.DrawSelf(spriteBatch);

            foreach (Ship s in AllShips)
                if (s.Type == ShipType.Small) s.DrawSelf(spriteBatch);

            foreach (Player p in level.Players)
            {
                p.HealthBar.Draw(spriteBatch);
                p.ChargeBar.Draw(spriteBatch);
                p.IconBar.Draw(spriteBatch);

                //spriteBatch.DrawString(
                //    spriteFont,
                //    p.ChargeBar.CurrentValue.ToString(),
                //    p.ChargeBar.Position,
                //    Color.LightBlue);
            }



            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
