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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<Player> players = new List<Player>();
        public List<Object> objects = new List<Object>();

        Random r = new Random();

        Song bgMusic;

        SoundEffect Hit1FX;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            bgMusic = Content.Load<Song>("AmazingPlan");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(bgMusic);
        }

        protected override void Initialize()
        {

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                Player player = new Player(this, spriteBatch, PlayerIndex.One, Content, graphics);
                players.Add(player);
            }
            if (GamePad.GetState(PlayerIndex.Two).IsConnected)
            {
                Player player = new Player(this, spriteBatch, PlayerIndex.Two, Content, graphics);
                players.Add(player);
            }
            if (GamePad.GetState(PlayerIndex.Three).IsConnected)
            {
                Player player = new Player(this, spriteBatch, PlayerIndex.Three, Content, graphics);
                players.Add(player);
            }
            if (GamePad.GetState(PlayerIndex.Four).IsConnected)
            {
                Player player = new Player(this, spriteBatch, PlayerIndex.Four, Content, graphics);
                players.Add(player);
            }

            int i = 0;
            while (i < 20)
            {
                Object ob;
                if (i <= 10)
                {
                    ob = new Object(this, Content, graphics, 200 * i, r.Next(100, 1000), 1, spriteBatch);
                }
                else
                {
                    ob = new Object(this, Content, graphics, r.Next(100, 1000), 150 * (i - 11), 2, spriteBatch);
                }
                objects.Add(ob);
                i++;
            }
            Hit1FX = Content.Load<SoundEffect>("Hit1"); 

        }
        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            foreach (Player p in players)
            {
                int collisions = 1;
                while (collisions == 1)
                {
                    collisions = Collision(p, gameTime);
                }

                p.Update(gameTime, graphics);

                foreach (Object o in objects)
                {
                    Rectangle r = new Rectangle((int)p.GetX, (int)p.GetY, (int)p.GetHeight, (int)p.GetWidth);
                    if (r.Intersects(new Rectangle((int)o.GetX, (int)o.GetY, (int)o.GetHeight, (int)o.GetWidth)))
                    {
                        if (p.alive)
                        {
                            p.ResetPlayer(gameTime, graphics);
                            foreach (Player p2 in players)
                            {
                                if (p != p2)
                                {
                                    p2.GetScore += 5;
                                }
                            }
                        }
                    }
                }

                if (!GamePad.GetState(p.GetIndex).IsConnected)
                {
                    players.Remove(p);
                    break;
                }
            }



            Connected(PlayerIndex.One);
            Connected(PlayerIndex.Two);
            Connected(PlayerIndex.Three);
            Connected(PlayerIndex.Four);

            while (ObjectUpdate(gameTime))
            {
                //Keep looping!
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            foreach (Player p in players)
            {
                p.Draw(gameTime, graphics);
            }
            foreach (Object o in objects)
            {
                o.Draw();
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }


        private void Connected(PlayerIndex pi)
        {
            if (GamePad.GetState(pi).IsConnected)
            {
                bool found = false;
                foreach (Player p in players)
                {
                    if (p.GetIndex == pi)
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    Player player = new Player(this, spriteBatch, pi, Content, graphics);
                    players.Add(player);
                }

            }
        }

        private int Collision(Player p, GameTime gameTime)
        {
            foreach (Bullet b in p.bullets)
            {
                foreach (Player p3 in players)
                {
                    if (p != p3)
                    {
                        Rectangle r = new Rectangle((int)b.GetX, (int)b.GetY, (int)b.GetHeight, (int)b.GetWidth);
                        if (r.Intersects(new Rectangle((int)p3.GetX, (int)p3.GetY, (int)p3.GetHeight, (int)p3.GetWidth)))
                        {
                            if (p3.alive)
                            {
                                p3.ResetPlayer(gameTime, graphics);
                                p.GetScore += 10;
                                p.bullets.Remove(b);
                                return 1;
                            }
                        }
                    }
                }
                foreach (Object o in objects)
                {
                    Rectangle r = new Rectangle((int)b.GetX, (int)b.GetY, (int)b.GetHeight, (int)b.GetWidth);
                    if (r.Intersects(new Rectangle((int)o.GetX, (int)o.GetY, (int)o.GetHeight, (int)o.GetWidth)))
                    {
                        Hit1FX.Play();
                        p.bullets.Remove(b);
                        b.player.GetScore += 100;
                        o.life--;
                        return 1;
                    }
                }
            }
            return 0;
        }

        private bool ObjectUpdate(GameTime gameTime)
        {
            foreach (Object o in objects)
            {
                o.Update(gameTime);
                if (o.split)
                {
                    o.split = false;
                    return true;
                }
            }
            return false;
        }
    }
}
