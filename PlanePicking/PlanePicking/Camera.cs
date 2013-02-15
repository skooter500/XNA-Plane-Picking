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
    public class RTSCamera : GameEntity
    {
        Texture2D mouseTexture;
        private Matrix projection;

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }
        private Matrix view;

        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }
        private KeyboardState keyboardState;
        private MouseState mouseState;

        public override void Draw(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 position = new Vector2(mouseState.X, mouseState.Y);
            XNAGame.Instance.SpriteBatch.Draw(mouseTexture, position, Color.White);
        }

        public void Initialize()
        {

        }

        public override void LoadContent()
        {
            mouseTexture = XNAGame.Instance.Content.Load<Texture2D>("cur");
        }
        public override void UnloadContent()
        {
        }

        public RTSCamera()
        {
            Position = new Vector3(0, 100, 0);
            Look = new Vector3(1, -1, -1);
            Look.Normalize();
            Up = Vector3.Up;
            Right = Vector3.Cross(Look, Up);
            Right.Normalize();

        }

        public Ray calculatePickingRay()
        {
            MouseState mouseState = Mouse.GetState();
            //  Unproject the screen space mouse coordinate into model space 
            //  coordinates. Because the world space matrix is identity, this 
            //  gives the coordinates in world space.
            Viewport vp = XNAGame.Instance.GraphicsDevice.Viewport;
            //  Note the order of the parameters! Projection first.
            Vector3 pos1 = vp.Unproject(new Vector3(mouseState.X, mouseState.Y, 0), Projection, View, Matrix.Identity);
            Vector3 pos2 = vp.Unproject(new Vector3(mouseState.X, mouseState.Y, 1), Projection, View, Matrix.Identity);
            Vector3 dir = Vector3.Normalize(pos2 - pos1);

            return new Ray(pos1, dir);
        }
        

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
            int width = XNAGame.Instance.GraphicsDeviceManager.PreferredBackBufferWidth;
            int height = XNAGame.Instance.GraphicsDeviceManager.PreferredBackBufferHeight;
            MouseState mouseState = Mouse.GetState();

            float speed = 50.0f;

            if (mouseState.X < 1)
            {
                Mouse.SetPosition(1, mouseState.Y);

                Vector3 direction = Right;
                direction.Y = 0;
                direction.Normalize();
                Position -= direction * speed * timeDelta;
                
            }

            if (mouseState.X > width)
            {
                Mouse.SetPosition(width, mouseState.Y);
                Vector3 direction = Right;
                direction.Y = 0;
                direction.Normalize();
                Position += direction * speed * timeDelta;
            }

            if (mouseState.Y < 0)
            {
                Mouse.SetPosition(mouseState.X, 0);
                Vector3 direction = Look;
                direction.Y = 0;
                direction.Normalize();
                Position += direction * speed * timeDelta;
            }

            if (mouseState.Y > height)
            {
                Mouse.SetPosition(mouseState.X, height);
                Vector3 direction = Look;
                direction.Y = 0;
                direction.Normalize();
                Position -= direction * speed * timeDelta;
            }

            view = Matrix.CreateLookAt(Position, Position + Look, Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), XNAGame.Instance.GraphicsDeviceManager.GraphicsDevice.Viewport.AspectRatio, 1.0f, 10000.0f);
            
        }

        public Matrix getProjection()
        {
            return projection;
        }

        public Matrix getView()
        {
            return view;
        }

        
    }
}
