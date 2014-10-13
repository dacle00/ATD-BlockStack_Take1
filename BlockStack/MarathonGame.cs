using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BlockStack
{ 
    class MarathonGame
    {
        // global variables
        int levelNumber;
        float stepTime;
        int boardWidth;
        int boardHeight;
        GameBoard theBoard;

        List<Tetromino> availablePieces;
        KeyboardState previousState;
        GamePadState previousPadState;
        Tetromino currentPiece;
        Tetromino nextPiece;  // TODO:  make array[3] ??
        Vector2 pieceStartingPosition;
        Vector2 nextPieceDisplayPosition;
        const int blockWidth = 32;
        const int blockHeight = 32;
        BlockBag tetrisPieceFactory;
        GameTime lastTimeDropped;
        GameTime lastTimeUpdate;

        Rectangle playFieldBounds = new Rectangle(20, 100, 320, 640);
        Rectangle scoreFieldBounds = new Rectangle(20, 20, 500, 60);
        Rectangle nextPieceFieldBounds = new Rectangle(360, 100, 160, 160);

        int score = 4000;
        SpriteFont scoreFont = null;
        Vector2 scoreFontPosition = new Vector2(22, 22);
        string scoreString;


        public MarathonGame(int pLevel, ContentManager pContentManager)
        {
            boardWidth = 10;  // Standard board width.
            boardHeight = 20; // Standard board height.
            theBoard = new GameBoard(boardWidth, boardHeight);
            levelNumber = pLevel;
            stepTime = 0.5f - (0.05f * (levelNumber - 1));
            Load(pContentManager);
            tetrisPieceFactory = new BlockBag(availablePieces);

            lastTimeDropped = null;
            lastTimeUpdate = null;
        }

        public virtual void Load( ContentManager pContentManager)
        {
            Texture2D blockTexture = pContentManager.Load<Texture2D>("CustomImages\\aviBlock1");
            Tetromino tmpShape;
            pieceStartingPosition = new Vector2(5f, 0f);
            nextPieceDisplayPosition = new Vector2(376f, 116f);
            availablePieces = new List<Tetromino>();

            // SHAPE I
            tmpShape = new ShapeI();
            tmpShape.constructPiece(Color.LightGray, blockWidth, blockHeight, blockTexture);
            availablePieces.Add(tmpShape);

            // SHAPE O 
            tmpShape = new ShapeO();
            tmpShape.constructPiece(Color.Yellow, blockWidth, blockHeight, blockTexture);
            availablePieces.Add(tmpShape);

            // SHAPE Z
            tmpShape = new ShapeZ();
            tmpShape.constructPiece(Color.Red, blockWidth, blockHeight, blockTexture);
            availablePieces.Add(tmpShape);

            // SHAPE T 
            tmpShape = new ShapeT();
            tmpShape.constructPiece(Color.Purple, blockWidth, blockHeight, blockTexture);
            availablePieces.Add(tmpShape);

            // SHAPE J 
            tmpShape = new ShapeJ();
            tmpShape.constructPiece(Color.LightBlue, blockWidth, blockHeight, blockTexture);
            availablePieces.Add(tmpShape);

            // SHAPE S 
            tmpShape = new ShapeS();
            tmpShape.constructPiece(Color.Green, blockWidth, blockHeight, blockTexture);
            availablePieces.Add(tmpShape);

            // SHAPE L 
            tmpShape = new ShapeL();
            tmpShape.constructPiece(Color.Orange, blockWidth, blockHeight, blockTexture);
            availablePieces.Add(tmpShape);

            // Start the RandomBag of Pieces, supplying a complete List of all potentialy available pieces for this game:
            tetrisPieceFactory = new BlockBag(availablePieces);

            // get game's first pieces.
            currentPiece = tetrisPieceFactory.GetNextPiece();
            currentPiece.position = pieceStartingPosition;
            nextPiece = tetrisPieceFactory.GetNextPiece();
            nextPiece.position = nextPieceDisplayPosition;

            scoreFont = pContentManager.Load<SpriteFont>("ScoreFont");
        }


        public void Draw(GameTime gt, SpriteBatch sb)
        {
            // draw background and gameboard, etc.
            sb.Draw(Game1.dummyTexture, playFieldBounds, Color.Beige);
            sb.Draw(Game1.dummyTexture, scoreFieldBounds, Color.Azure);
            sb.Draw(Game1.dummyTexture, nextPieceFieldBounds, Color.Azure);

            // draw GameBoard's contents
            theBoard.Draw(gt, sb);

            // draw each piece to screen
            if (currentPiece != null)
                currentPiece.Draw(gt, sb);
            if (nextPiece != null)
                nextPiece.Draw(gt, sb);

            // draw score
            scoreString = "Your score is OVER: " + score;
            sb.DrawString(scoreFont, scoreString, scoreFontPosition, Color.Teal);
        }


        public void Update(GameTime gt)
        {
            if (currentPiece != null)
            {
                // Movement Requests
                UpdateUserInput(gt);
                UpdatePieceDrop(gt);

                // Collision Checks

            }
            else
            {
                // Cycle to next piece.
                CycleNextPiece();
            }


            // update Scores and Meta



        }


        #region UpdateSupportFunctions

        private void UpdateUserInput(GameTime gt)
        {
            GamePadState currentPadState = GamePad.GetState(PlayerIndex.One);

            //pieces.GetNextPiece().Draw(pGameTime, pSpriteBatch);
            if (lastTimeUpdate != null)
            {
                

                if (gt.TotalGameTime >= lastTimeUpdate.TotalGameTime.Add(new TimeSpan(0, 0, 0, 0, 100)))
                {
                    #region Keyboard input
                    KeyboardState currentState = Keyboard.GetState();
                    if (currentState.IsKeyDown(Keys.Left) && !previousState.IsKeyDown(Keys.Left))
                    {
                        //move piece left, if no collision
                        if (currentPiece.position.X > 1)
                        {
                            // move piece left 
                            currentPiece.position.X -= blockWidth;
                        }
                        else
                        {
                            // feedback?  User requested illegal move
                        }
                    }
                    if (currentState.IsKeyDown(Keys.Right) && !previousState.IsKeyDown(Keys.Right))
                    {
                        if (currentPiece.position.X < 10)
                        {
                            // move piece left 
                            currentPiece.position.X += blockWidth;
                        }
                        else
                        {
                            // feedback?  User requested illegal move
                        }
                    }
                    if (currentState.IsKeyDown(Keys.Down) && !previousState.IsKeyDown(Keys.Down))
                    {
                        if (currentPiece.position.Y + blockHeight < playFieldBounds.Bottom)
                        {
                            currentPiece.position.Y += blockHeight;
                        }
                    }

                    if (currentState.IsKeyDown(Keys.A) && !previousState.IsKeyDown(Keys.A))
                    {
                        currentPiece.Rotate(false);
                    }

                    if (currentState.IsKeyDown(Keys.S) && !previousState.IsKeyDown(Keys.S))
                    {
                        currentPiece.Rotate(true);
                    }

                    previousState = currentState;
                    #endregion

                    #region PadInput
                    

                    if (currentPadState.DPad.Down == ButtonState.Pressed)
                    {
                        if (currentPiece.position.Y + blockHeight < playFieldBounds.Bottom)
                        {
                            currentPiece.position.Y += blockHeight;
                        }
                    }

                    if (currentPadState.DPad.Left == ButtonState.Pressed)
                    {
                        currentPiece.position.X -= blockWidth;
                    }

                    if (currentPadState.DPad.Right == ButtonState.Pressed)
                    {
                        currentPiece.position.X += blockWidth;
                    }

                    if (currentPadState.Buttons.A == ButtonState.Pressed && !(previousPadState.Buttons.A == ButtonState.Pressed))
                    {
                        currentPiece.Rotate(false);
                    }

                    if (currentPadState.Buttons.B == ButtonState.Pressed && !(previousPadState.Buttons.A == ButtonState.Pressed))
                    {
                        currentPiece.Rotate(true);
                    }
                    #endregion

                    lastTimeUpdate = new GameTime(gt.TotalGameTime, gt.ElapsedGameTime);
                    
                }

               
            }
            else
            {
                lastTimeUpdate = new GameTime(gt.TotalGameTime, gt.ElapsedGameTime);
            }

            if (currentPadState.Buttons.A == ButtonState.Pressed && !(previousPadState.Buttons.A == ButtonState.Pressed))
            {
                currentPiece.Rotate(false);
            }

            if (currentPadState.Buttons.B == ButtonState.Pressed)
            {
                currentPiece.Rotate(true);
            }

            previousPadState = currentPadState;
        }


        private void UpdatePieceDrop(GameTime gt)
        {
            if (lastTimeDropped != null)
            {
                if (gt.TotalGameTime >= lastTimeDropped.TotalGameTime.Add(new TimeSpan(0, 0, 0, 0, 500)))
                {
                    if (currentPiece.position.Y + blockHeight < playFieldBounds.Bottom)
                    {
                        currentPiece.position.Y += blockHeight;
                        lastTimeDropped = new GameTime(gt.TotalGameTime, gt.ElapsedGameTime);
                    }
                }
            }
            else
            {
                lastTimeDropped = new GameTime(gt.TotalGameTime, gt.ElapsedGameTime);
            }

            //currenttime => lasttime.totalseconds++
            //if (lastTimeDropped != null)
            //{
            //    if (pGameTime.TotalGameTime >= lastTimeDropped.TotalGameTime.Add(new TimeSpan(0,0,0,0,500)))
            //    {
                    
            //        lastTimeDropped = new GameTime(pGameTime.TotalGameTime, pGameTime.ElapsedGameTime);
            //    }
            //}
            //else
            //{
            //    lastTimeDropped = new GameTime(pGameTime.TotalGameTime, pGameTime.ElapsedGameTime);
            //}

            //currentPiece.position.Y += blockHeight;

        }


        public void CycleNextPiece()
        {
            currentPiece = nextPiece;
            nextPiece = tetrisPieceFactory.GetNextPiece();
        }


        #endregion

    }
}
