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

namespace PlanePicking
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class XNAGame : Microsoft.Xna.Framework.Game
    {
        static XNAGame instance = null;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Vector3 pickedPoint = Vector3.Zero;
        bool pickedAPoint;

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }
        
        public static XNAGame Instance
        {
            get { return XNAGame.instance; }
            set { XNAGame.instance = value; }
        }
        GraphicsDeviceManager graphics;

        public GraphicsDeviceManager GraphicsDeviceManager
        {
            get { return graphics; }
            set { graphics = value; }
        }

        RTSCamera camera;
        Ground ground;

        List<GameEntity> children = new List<GameEntity>();

        public RTSCamera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        private Random random = new Random();

        public XNAGame()
        {
            instance = this;
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
            camera = new RTSCamera();            
            camera.Initialize();
            ground = new Ground();
            children.Add(camera);
            children.Add(ground);

            font = Content.Load<SpriteFont>("SpriteFont1");

            foreach (GameEntity gameEntity in children)
            {
                gameEntity.LoadContent();
            }
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Ray pickingRay = Camera.calculatePickingRay();

                pickedAPoint = ground.rayIntersects(pickingRay, out pickedPoint);
            }

            foreach (GameEntity gameEntity in children)
            {
                gameEntity.Update(gameTime);
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
            spriteBatch.Begin();
            foreach (GameEntity gameEntity in children)
            {
                gameEntity.Draw(gameTime);
            }
            if (pickedAPoint)
            {
                spriteBatch.DrawString(font, "" + pickedPoint, new Vector2(10, 10), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
