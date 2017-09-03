﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace OptionalProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // game state
        GameState gameState = GameState.Menu;

        // Increment 1: opening screen fields
        const int WindowWidth = 800;
        const int WindowHeight = 600;

        Texture2D openingScreen;
        Rectangle openingDrawRectangle;

        // Increment 2: board field
        NumberBoard numberBoard;

        // Increment 5: random field
        Random random = new Random();

        // Increment 5: new game sound effect field
        SoundEffect newGameSound;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Increment 1: set window resolution and make mouse visible
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            IsMouseVisible = true;
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

            // Increment 1: load opening screen and set opening screen draw rectangle
            openingScreen = Content.Load<Texture2D>(@"bin\graphics\openingscreen");
            openingDrawRectangle = new Rectangle(0, 0, WindowWidth, WindowHeight);
            // Increment 2: create the board object (this will be moved before you're done with the project)
            StartGame();

            // Increment 5: load new game sound effect
            newGameSound = Content.Load<SoundEffect>(@"bin\audio\applause");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Increment 2: change game state if game state is GameState.Menu and user presses Enter
            if (gameState == GameState.Menu && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                gameState = GameState.Play;
            }

            // Increment 4: if we're actually playing, update mouse state and update board
            if (gameState == GameState.Play)
            {
                MouseState mouse = Mouse.GetState();
                numberBoard.Update(gameTime, mouse);

                // Increment 5: check for correct guess
                bool isGameOver = numberBoard.Update(gameTime, mouse);
                if (isGameOver)
                {
                    newGameSound.Play();
                    StartGame();
                }
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Increments 1 and 2: draw appropriate items here
            spriteBatch.Begin();
            if (gameState == GameState.Menu)
            {
                spriteBatch.Draw(openingScreen, openingDrawRectangle, Color.White);
            }
            else
            {
                numberBoard.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Starts a game
        /// </summary>
        void StartGame()
        {
            // Increment 5: randomly generate new number for game
            int correctNumber = random.Next(1,10);

            // Increment 5: create the board object
            numberBoard = new NumberBoard(new ContentManager(Content.ServiceProvider, Content.RootDirectory),
                new Vector2(WindowWidth / 2, WindowHeight / 2),
                WindowHeight / (WindowHeight / 100) * (WindowHeight / 100 - 1),
                correctNumber);
        }
    }
}
