﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using xTile.Layers;
using xTile.Tiles;

namespace ProjectGame
{
    class Player
    {
        public Direction playerDirection = Direction.Right;
        public Texture2D PlayerTexture;
        public Point frameSize = new Point(64,64);
        public Point currentFrame = new Point(1,1);
        public Point sheetSize = new Point(5,9);
        public Vector2 Position;
        public int collisionOffset = 10;
        public int speed = 3;
        public int timeSinceLastFrame = 0;
        public int defaultMillisecondsPerFrame = 60;
        public static bool doConversation;
        public static int convset = 0;
        public static int count = 0;
        public static int collision = 0;
        public static int notreaded = 0;
        public ProjectileManager projectileManager;
        public KeyboardState OldKeyState;        
        
        public Player(ProjectileManager projectileManager)
        {
            this.projectileManager = projectileManager;
        }

        public Rectangle playerBounds
        {
            get
            {
                return new Rectangle((int)Position.X + collisionOffset, (int)Position.Y + collisionOffset, frameSize.X - (collisionOffset * 2), frameSize.Y - (collisionOffset * 2));
            }
        }

        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        public void Initalize(Texture2D PlayerTexture, Vector2 Position)
        {
            this.PlayerTexture = PlayerTexture;
            this.Position = Position;
        }

        public void Update(GameTime gameTime, Layer collisionLayer)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                playerDirection = Direction.Left;
                Position.X -= speed;
                if (Collision(Position, collisionLayer))
                {
                    Position.X += speed;
                }
                
                if (timeSinceLastFrame > defaultMillisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    currentFrame.Y = 1;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X-1)
                    {
                        currentFrame.X = 0;
                    }
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                playerDirection = Direction.Right;
                Position.X += speed;
                if (Collision(Position, collisionLayer))
                {
                    Position.X -= speed;
                }
                if (timeSinceLastFrame > defaultMillisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    currentFrame.Y = 1;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X-1)
                    {
                        currentFrame.X = 0;
                    }
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                playerDirection = Direction.Up;
                Position.Y -= speed;
                if (Collision(Position, collisionLayer))
                {
                    Position.Y += speed;
                }
                if (timeSinceLastFrame > defaultMillisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    currentFrame.Y = 4;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X-1)
                    {
                        currentFrame.X = 0;
                    }
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                playerDirection = Direction.Down;
                Position.Y += speed;
                if (Collision(Position, collisionLayer))
                {
                    Position.Y -= speed;
                }
                if (timeSinceLastFrame > defaultMillisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    currentFrame.Y = 7;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X-1)
                    {
                        currentFrame.X = 0;
                    }
                }
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && collision == 1 && notreaded == 0)
            {
                
                doConversation = true;
                           }
            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && convset == 14)
            {
                notreaded = 1;
                doConversation = false;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Space) && OldKeyState.IsKeyUp(Keys.Space))
            {
                switch (playerDirection)
                {
                    case Direction.Down: projectileManager.AddKnife(new Vector2(playerBounds.Center.X, playerBounds.Center.Y), ProjectileManager.Knife.KnifeDirection.Down); break;
                    case Direction.Up: projectileManager.AddKnife(new Vector2(playerBounds.Center.X, playerBounds.Center.Y), ProjectileManager.Knife.KnifeDirection.Up); break;
                    case Direction.Left: projectileManager.AddKnife(new Vector2(playerBounds.Center.X, playerBounds.Center.Y), ProjectileManager.Knife.KnifeDirection.Left); break;
                    case Direction.Right: projectileManager.AddKnife(new Vector2(playerBounds.Center.X, playerBounds.Center.Y), ProjectileManager.Knife.KnifeDirection.Right); break;
                }
            }
            OldKeyState = Keyboard.GetState();

            foreach (ProjectileManager.Knife theKnife in projectileManager.knives)
            {
                switch (theKnife.dir)
                {
                    case ProjectileManager.Knife.KnifeDirection.Down: theKnife.Position.Y += theKnife.Speed; theKnife.kniferotaion += theKnife.Speed; break;
                    case ProjectileManager.Knife.KnifeDirection.Up: theKnife.Position.Y -= theKnife.Speed; theKnife.kniferotaion += theKnife.Speed; break;
                    case ProjectileManager.Knife.KnifeDirection.Right: theKnife.Position.X += theKnife.Speed; theKnife.kniferotaion += theKnife.Speed; break;
                    case ProjectileManager.Knife.KnifeDirection.Left: theKnife.Position.X -= theKnife.Speed; theKnife.kniferotaion += theKnife.Speed; break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 mapDimension, Vector2 windowDimension, Vector2 viewport)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (playerDirection == Direction.Left)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

             if (doConversation && convset == 4)
            {
              Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
              Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "BOY: You are to late!");
             
            }

             if (doConversation && convset == 5)
             {
                 Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
                 Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "YOU: What happend here?");
             }

            if (doConversation && convset == 6)
            {
                Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "BOY: The town folk, they are all dead!");
               
            }
            if (doConversation && convset == 7)
            {
                Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "YOU: What killed them?");
            }
            if (doConversation && convset == 8)
            {
                Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "BOY: An evil monster sent from the heavens to purge this land");
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 140, Game1.textconv, "The elders call him ... the garbage collecter, gods be true!");
               
            }
            if (doConversation && convset == 9)
            {
                Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "YOU: Let me guess, was he mumbling about mark-and-sweep?");
            }
            if (doConversation && convset == 10)
            {
                Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "BOY: What!? You knew of the monster");
               
            }
            if (doConversation && convset == 11)
            {
                Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "YOU: Yes, but I heard the town was under attack from Goblins?");
            }
            if (doConversation && convset == 12)
            {
                Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 115, Game1.textconv, "BOY: The monster took care of them aswell");
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 128, Game1.textconv, "... he said that the authors had not kept the reference");
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 141, Game1.textconv, "to the ArrayList<Goblins> and they all had to die!");
            }
            if (doConversation && convset == 13)
            {
                Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
                               Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "YOU: Neat bro, cya");
                               notreaded = 1;
            }

            spriteBatch.Draw(PlayerTexture, CalculateScreenPosition(mapDimension, windowDimension, viewport), new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
            foreach (ProjectileManager.Knife theKnife in projectileManager.knives)
            {
                //spriteBatch.Draw(theKnife.Texture, CalculateScreenPositionKnife(mapDimension, windowDimension, viewport, theKnife.Position), Color.White);
                spriteBatch.Draw(theKnife.Texture, CalculateScreenPositionKnife(mapDimension, windowDimension, viewport, theKnife.Position), null, Color.White, theKnife.kniferotaion, new Vector2(0,0),1f,SpriteEffects.None,0);
            }
        }

        private Vector2 CalculateScreenPosition(Vector2 mapDimension, Vector2 windowDimension, Vector2 viewport)
        {
            Vector2 realPosition = Vector2.Zero;
            if (mapDimension.X <= windowDimension.X && mapDimension.Y <= windowDimension.Y)
            {
                realPosition.X = Position.X;
                realPosition.Y = Position.Y;
            }
            else if (mapDimension.X > windowDimension.X && mapDimension.Y <= windowDimension.Y)
            {
                realPosition.X = Position.X - viewport.X;
                realPosition.Y = Position.Y;
            }
            else if (mapDimension.X <= windowDimension.X && mapDimension.Y > windowDimension.Y)
            {
                realPosition.X = Position.X;
                realPosition.Y = Position.Y - viewport.Y;
            }
            else if (mapDimension.X > windowDimension.X && mapDimension.Y > windowDimension.Y)
            {
                realPosition.X = Position.X - viewport.X;
                realPosition.Y = Position.Y - viewport.Y;
            }
            return realPosition;
        }

        private Vector2 CalculateScreenPositionKnife(Vector2 mapDimension, Vector2 windowDimension, Vector2 viewport, Vector2 KnifePosition)
        {
            Vector2 realPosition = Vector2.Zero;
            if (mapDimension.X <= windowDimension.X && mapDimension.Y <= windowDimension.Y)
            {
                realPosition.X = KnifePosition.X;
                realPosition.Y = KnifePosition.Y;
            }
            else if (mapDimension.X > windowDimension.X && mapDimension.Y <= windowDimension.Y)
            {
                realPosition.X = KnifePosition.X - viewport.X;
                realPosition.Y = KnifePosition.Y;
            }
            else if (mapDimension.X <= windowDimension.X && mapDimension.Y > windowDimension.Y)
            {
                realPosition.X = KnifePosition.X;
                realPosition.Y = KnifePosition.Y - viewport.Y;
            }
            else if (mapDimension.X > windowDimension.X && mapDimension.Y > windowDimension.Y)
            {
                realPosition.X = KnifePosition.X - viewport.X;
                realPosition.Y = KnifePosition.Y - viewport.Y;
            }
            return realPosition;
        }


        private bool Collision(Vector2 pos, Layer collisionLayer)
        {

            int leftTile = (int)Math.Floor((float)playerBounds.Left / 32);
            int rightTile = (int)Math.Ceiling(((float)playerBounds.Right / 32)) - 1;
            int topTile = (int)Math.Floor((float)playerBounds.Top / 32);
            int bottomTile = (int)Math.Ceiling(((float)playerBounds.Bottom / 32)) - 1;
            //Debug.Print("left: " + leftTile + " right: " + rightTile + " top: " + topTile + " bottom: " + bottomTile);
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    if ((x >= 0 && x < collisionLayer.LayerWidth) && (y >= 0 && y < collisionLayer.LayerHeight))
                    {
                        Tile tile = collisionLayer.Tiles[x, y];
                        if (tile != null && tile.TileIndex == 23)
                        {
                            
                            Debug.Print("Collision with tile at {" + x + "," + y + "}");
                            return true;
                        }
                          else if (tile != null && tile.TileIndex == 740)
                         {
                             Game1.map = Game1.inhousMapb;
                             Position = new Vector2(380, 284);
                         
                              return true;
                         }
                        else if (tile != null && tile.TileIndex == 332)
                        {
                            Game1.map = Game1.forestMap;
                            Position = new Vector2(1768, 300);


                            return true;
                        }

                        else if (tile != null && tile.TileIndex == 741)
                        {
                            Game1.map = Game1.inhousMapa;
                            Position = new Vector2(380, 284);

                            return true;
                        }
                       
                      
                        else if (tile != null && tile.TileIndex == 331)
                        {
                            Game1.map = Game1.forestMap;
                            Position = new Vector2(2020, 300);


                            return true;
                        }
                        else if (tile != null && tile.TileIndex == 742)
                        {
                            Game1.map = Game1.inhous2Mapa;
                            Position = new Vector2(380, 284);

                            return true;
                        }
                        else if (tile != null && tile.TileIndex == 351)
                        {
                            Game1.map = Game1.forestMap;
                            Position = new Vector2(2276, 300);


                            return true;
                        }

                        else if (tile != null && tile.TileIndex == 743)
                        {
                            Game1.map = Game1.inhous2Mapb;
                            Position = new Vector2(380, 284);

                            return true;
                        }
                        else if (tile != null && tile.TileIndex == 352)
                        {
                            Game1.map = Game1.forestMap;
                            Position = new Vector2(2528, 340);


                            return true;
                        }
                     
                        else if (tile != null && tile.TileIndex == 760)
                        {
                            Game1.map = Game1.second;
                            Position = new Vector2(32, 100);


                            return true;
                        }


                        else if (tile != null && tile.TileIndex == 942)
                        {
                           // Game1.map = Game1.second;
                            //Position = new Vector2(100, 100);
                            doConversation = true;
                         
                        
                           
                            return true;
                        }
                        else if (tile != null && tile.TileIndex == 1408)
                        {
                            // how many screens count to 1 -> for 2 screens, 2  -> for three screens etc..
                            Game1.gamestate = Game1.GameStates.End; 
                            // count = 7;
                            // convset = 4;
                            // doConversation = true;
                        }
                        
                        else if (tile != null && tile.TileIndex == 1901)
                        {
                            // how many screens count to 1 -> for 2 screens, 2  -> for three screens etc..
                            count = 14;
                            convset = 4;
                            collision = 1;
                        }
                        //template for tile conv
                        else if (tile != null && tile.TileIndex == 200000)
                        {

                            // how many screens count to 1 -> for 2 screens, 2  -> for three screens etc..
                                   count = 7;
                                   convset = 4;
                                   doConversation = true;
                        }
                        else if (tile != null && tile.TileIndex == 1881)
                        {

                            Game1.map = Game1.forestMap;
                            Position = new Vector2(3112, 226);
                        }

                       

                    }
                }
            }
            return false;
        }

    }
}