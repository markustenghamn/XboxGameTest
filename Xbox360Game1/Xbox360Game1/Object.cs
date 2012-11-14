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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Object 
    {
        Vector2 position;
        Vector2 size;
        int type;
        float speed;
        float dir = 5;
        public int life = 10;
        Vector2 dir2;
        public bool split = false;

        Texture2D objectTexture;
        Color objectColor = Color.White;

        SpriteBatch spriteBatch;

        Random r = new Random();

        GraphicsDeviceManager graphics;
        Game1 game;
        ContentManager content;

        SpriteFont lifeFont;

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

        public Object(Game1 game, ContentManager content, GraphicsDeviceManager graphics, float x, float y, int type, SpriteBatch spriteBatch)
        {
            position = new Vector2(x, y);
            if (type == 1 || type == 2)
            {
                size = new Vector2(50, 50);
            }
            else if (type == 3 || type == 4 || type == 5 || type == 6)
            {
                size = new Vector2(25, 25);
            }

            if (type == 3)
            {
                dir2 = new Vector2 (-2f, -2f);
            }
            else if (type == 4) 
            {
                dir2 = new Vector2(-2f, 2f);
            }
            else if (type == 5)
            {
                dir2 = new Vector2(2f, -2f);
            }
            else if (type == 6)
            {
                dir2 = new Vector2(2f, 2f);
            }
            objectTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            objectTexture.SetData<Color>(new[] { objectColor });
            this.spriteBatch = spriteBatch;
            this.type = type;
            this.graphics = graphics;
            this.game = game;
            this.content = content;
            lifeFont = content.Load<SpriteFont>("objectLife");
        }
        public void Initialize()
        {

        }

        public void Update(GameTime gameTime)
        {
            if (type == 1)
            {
                if (life == 0)
                {
                    Split();
                }
                position.Y += dir;
                if (position.Y < 0)
                {
                    dir = r.Next(5, 10);
                }
                if (position.Y > (graphics.PreferredBackBufferHeight - size.Y))
                {
                    dir = r.Next(-10, -5);
                }
            }
            else if (type == 2)
            {
                if (life <= 0)
                {
                    Split();
                }
                position.X += dir;
                if (position.X < 0)
                {
                    dir = r.Next(5, 10);
                }
                if (position.X > (graphics.PreferredBackBufferWidth - size.X))
                {
                    dir = r.Next(-10, -5);
                }
            }
            else if (type == 3 || type == 4 || type == 5 || type == 6)
            {
                if (life == 0)
                {
                    game.objects.Remove(this);
                    split = true;
                }
                if (position.X < 0)
                {
                    dir2.X = r.Next(1, 5);
                }
                if (position.X > (graphics.PreferredBackBufferWidth - size.X))
                {
                    dir2.X = r.Next(-5, -1);
                }
                if (position.Y < 0)
                {
                    dir2.Y = r.Next(1, 5);
                }
                if (position.Y > (graphics.PreferredBackBufferHeight - size.Y))
                {
                    dir2.Y = r.Next(-5, -1);
                }
                position += dir2;

            }
        }

        public void Draw()
        {
            spriteBatch.Draw(objectTexture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), objectColor);
            if (type == 1 || type == 2)
            {
                spriteBatch.DrawString(lifeFont, life.ToString(), new Vector2(position.X - 25f, position.Y - 25f), objectColor);
            }
        }

        public void SwitchColor()
        {
            if (objectColor == Color.Black)
            {
                objectColor = Color.White;
            }
            else
            {
                objectColor = Color.Black;
            }
        }

        public void Split()
        {
            size = new Vector2(25f, 25f);
            type = 3;
            dir2 = new Vector2(-5f, -5f);
            Object ob1 = new Object(game, content, graphics, position.X, position.Y + 25f, 4, spriteBatch);
            Object ob2 = new Object(game, content, graphics, position.X + 25f, position.Y, 5, spriteBatch);
            Object ob3 = new Object(game, content, graphics, position.X + 25f, position.Y + 25f, 6, spriteBatch);
            game.objects.Add(ob1);
            game.objects.Add(ob2);
            game.objects.Add(ob3);
            life = 10;
            split = true;
        }
    }
}
