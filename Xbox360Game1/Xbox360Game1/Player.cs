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
    public class Player
    {
        Vector2 position;
        Vector2 size;

        Texture2D playerTexture;
        public Color playerColor;

        SpriteBatch spriteBatch;

        int score;
        SpriteFont scoreFont;

        PlayerIndex playerIndex;

        public bool alive = true;
        TimeSpan deadTime;
        TimeSpan respawnTime = new TimeSpan(0, 0, 2);

        public List<Bullet> bullets = new List<Bullet>();
        TimeSpan reload = new TimeSpan(2000000);
        TimeSpan lastShot;

        SoundEffect shotFX;
        SoundEffect explodeFX;
        SoundEffect laughFX;
        SoundEffect Hit2FX;

        public int GetScore
        {
            get { return score; }
            set { score = value; }
        }

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

        public PlayerIndex GetIndex {
            get { return playerIndex; }
        }

        public Player(Game game, SpriteBatch spriteBatch, PlayerIndex playerIndex, ContentManager content, GraphicsDeviceManager graphics)
        {
            position = new Vector2(100f, 100f);
            size = new Vector2(25f, 25f);

            if (playerIndex == PlayerIndex.One)
            {
                playerColor = Color.Red;
                position = new Vector2(100f, 100f);
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                playerColor = Color.Blue;
                position = new Vector2(graphics.PreferredBackBufferWidth - 200f, 100f);
            }
            else if (playerIndex == PlayerIndex.Three)
            {
                playerColor = Color.Green;
                position = new Vector2(100f, graphics.PreferredBackBufferHeight - 250f);
            }
            else if (playerIndex == PlayerIndex.Four)
            {
                playerColor = Color.Yellow;
                position = new Vector2(graphics.PreferredBackBufferWidth - 200f, graphics.PreferredBackBufferHeight - 250f);
            }
            playerTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            playerTexture.SetData<Color>(new[] { playerColor } );
            scoreFont = content.Load<SpriteFont>("Score");
            shotFX = content.Load<SoundEffect>("Shot");
            explodeFX = content.Load<SoundEffect>("Explode");
            laughFX = content.Load<SoundEffect>("Laugh");
            Hit2FX = content.Load<SoundEffect>("Hit2");
            this.spriteBatch = spriteBatch;
            this.playerIndex = playerIndex;
            
        }

        public void Initialize()
        {
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            if (!alive)
            {
                if (gameTime.TotalGameTime.CompareTo(deadTime + respawnTime) == 1)
                {
                    alive = true;
                }
            }
            else
            {
                Vector2 leftStick = GamePad.GetState(playerIndex).ThumbSticks.Left;
                Vector2 rightStick = GamePad.GetState(playerIndex).ThumbSticks.Right;
                position += new Vector2(leftStick.X * 5, -leftStick.Y * 5);
                if (rightStick != new Vector2(0,0))
                {
                    if (lastShot == null || gameTime.TotalGameTime.CompareTo(lastShot + reload) == 1)
                    {
                        Bullet b = new Bullet(graphics, position.X, position.Y, this, spriteBatch, rightStick);
                        lastShot = gameTime.TotalGameTime;
                        bullets.Add(b);
                        shotFX.Play();
                    }
                }
            }
            if ((position.Y + size.Y) > graphics.PreferredBackBufferHeight)
            {
                position.Y = (graphics.PreferredBackBufferHeight - size.Y);
            }
            else if (position.Y < 0)
            {
                position.Y = 0;
            }
            if ((position.X + size.X) > graphics.PreferredBackBufferWidth)
            {
                position.X = (graphics.PreferredBackBufferWidth - size.X);
            }
            else if (position.X < 0)
            {
                position.X = 0;
            }
            foreach (Bullet b in bullets)
            {
                if (b.GetX > graphics.PreferredBackBufferWidth || b.GetX < 0 || b.GetY > graphics.PreferredBackBufferHeight || b.GetY < 0)
                {
                    bullets.Remove(b);
                    Hit2FX.Play();
                    break;
                }
            }

            foreach (Bullet b in bullets)
            {
                b.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            if (alive)
            {
                spriteBatch.Draw(playerTexture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), playerColor);
            }
            if (playerIndex == PlayerIndex.One)
            {
                spriteBatch.DrawString(scoreFont, "Score: " + score.ToString(), new Vector2(200f, 25f), playerColor);
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                spriteBatch.DrawString(scoreFont, "Score: " + score.ToString(), new Vector2(graphics.PreferredBackBufferWidth - 200f, 100f), playerColor);
            }
            else if (playerIndex == PlayerIndex.Three)
            {
                spriteBatch.DrawString(scoreFont, "Score: " + score.ToString(), new Vector2(100f, graphics.PreferredBackBufferHeight - 250f), playerColor);
            }
            else if (playerIndex == PlayerIndex.Four)
            {
                spriteBatch.DrawString(scoreFont, "Score: " + score.ToString(), new Vector2(graphics.PreferredBackBufferWidth - 200f, graphics.PreferredBackBufferHeight - 250f), playerColor);
            }
            foreach (Bullet b in bullets)
            {
                b.Draw();
            }


        }



        public void ResetPlayer(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            alive = false;
            laughFX.Play();
            deadTime = gameTime.TotalGameTime;
            if (playerIndex == PlayerIndex.One)
            {
                position = new Vector2(100f, 100f);
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                position = new Vector2(graphics.PreferredBackBufferWidth - 300f, 100f);
            }
            else if (playerIndex == PlayerIndex.Three)
            {
                position = new Vector2(100f, graphics.PreferredBackBufferHeight - 300f);
            }
            else if (playerIndex == PlayerIndex.Four)
            {
                position = new Vector2(graphics.PreferredBackBufferWidth - 300f, graphics.PreferredBackBufferHeight - 300f);
            }
        }
    }
}
