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

namespace Xbox360Game1
{
    public class Bullet
    {
        Vector2 position;
        Vector2 size;
        public Player player;
        Vector2 direction;
        float speed;

        Texture2D objectTexture;
        Color objectColor = Color.White;

        SpriteBatch spriteBatch;

        Random r = new Random();

        GraphicsDeviceManager graphics;

        public float GetY
        {
            get { return position.Y; }
            //set { position.Y = value; }
        }

        public float GetX
        {
            get { return position.X; }
            //set { position.X = value; }
        }

        public float GetHeight
        {
            get { return size.X; }
        }

        public float GetWidth
        {
            get { return size.Y; }
        }

        public Bullet(GraphicsDeviceManager graphics, float x, float y, Player player, SpriteBatch spriteBatch, Vector2 direction)
        {
            position = new Vector2(x, y);
            size = new Vector2(10, 10);
            objectTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            objectTexture.SetData<Color>(new[] { player.playerColor });
            this.spriteBatch = spriteBatch;
            this.player = player;
            this.graphics = graphics;
            this.direction = direction;
        }
        public void Initialize()
        {

        }

        public void Update(GameTime gameTime)
        {
            direction.Normalize();
            position += direction * 10;
        }

        public void Draw()
        {
            spriteBatch.Draw(objectTexture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), player.playerColor);
        }
    }
}
