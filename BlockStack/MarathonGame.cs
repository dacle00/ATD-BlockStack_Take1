///////////////////////////////////////////////////////////////////////////////////////////
// MarathonGame class represents the tetris game mode: "Marathon" and all inheirent rules.
// In Marathon game mode, player continues raising score through each level (increasing the
// speed of play) until screen is filled (Game Over).
//
// AUTHORS: F1tZ, DoubleMintBen, CptSpaceToaster, Dacle
// COMPANY: AfterThough Digital
// STARTED: October, 2014
//

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
        ///////////////////////////////////////////////////////////////////////////////////////////
        // global variables

        // Screen
        int boardWidth;
        int boardHeight;
        GameBoard theBoard;
        public const int offsetPlayFieldX = 20;
        public const int offsetPlayFieldY = 100;
        Rectangle playFieldBounds = new Rectangle(offsetPlayFieldX, offsetPlayFieldY, 320, 640);
        Rectangle scoreFieldBounds = new Rectangle(20, 20, 500, 60);
        Rectangle nextPieceFieldBounds = new Rectangle(360, 100, 160, 160);

        // Game Pieces
        List<Tetromino> availablePieces;  // a list containing 1 of each possible unique tetromino piece
        Tetromino currentPiece;  // the piece in play, dropping down the gameboard
        Tetromino nextPiece;  // TODO:  make array[3] to see further ahead??
        Vector2 pieceStartingPosition = new Vector2(offsetPlayFieldX + (5 * blockwidth), offsetPlayFieldY + (0 * blockwidth));
        Vector2 nextPieceDisplayPosition = new Vector2(376f, 116f); //within the box, the 4x4 grid is displayed plus a 16 pixel padding on each side;
        const int blockwidth = 32;  //got rid of separate width/height vars as we're dealing with squares, right???
        BlockBag tetrisPieceFactory;  //gives another "random" tetris piece, used to get next pieces.

        // Leveling/Score
        int levelNumber;
        float stepTime;
        int score = 1123;
        SpriteFont scoreFont = null;
        Vector2 scoreFontPosition = new Vector2(22, 22);
        string scoreString;

        // User Input
        KeyboardState previousKeyState;
        GamePadState previousPadState;
        GameTime lastTimeDropped;
        GameTime lastTimeUpdate;

        ///////////////////////////////////////////////////////////////////////////////////////////
        // New MarathonGame constructor - 

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
            availablePieces = new List<Tetromino>();

            // SHAPE I
            Tetromino tmpShape = new ShapeI();
            tmpShape.constructPiece(Color.LightGray, blockwidth, blockTexture);
            availablePieces.Add(tmpShape);

            // SHAPE O 
            tmpShape = new ShapeO();
            tmpShape.constructPiece(Color.Yellow, blockwidth, blockTexture);
            availablePieces.Add(tmpShape);

            // SHAPE Z
            tmpShape = new ShapeZ();
            tmpShape.constructPiece(Color.Red, blockwidth, blockTexture);
            availablePieces.Add(tmpShape);

            // SHAPE T 
            tmpShape = new ShapeT();
            tmpShape.constructPiece(Color.Purple, blockwidth, blockTexture);
            availablePieces.Add(tmpShape);

            // SHAPE J 
            tmpShape = new ShapeJ();
            tmpShape.constructPiece(Color.LightBlue, blockwidth, blockTexture);
            availablePieces.Add(tmpShape);

            // SHAPE S 
            tmpShape = new ShapeS();
            tmpShape.constructPiece(Color.Green, blockwidth, blockTexture);
            availablePieces.Add(tmpShape);

            // SHAPE L 
            tmpShape = new ShapeL();
            tmpShape.constructPiece(Color.Orange, blockwidth, blockTexture);
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
            scoreString = "Score:  " + score;
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
                    if (currentState.IsKeyDown(Keys.Left) && !previousKeyState.IsKeyDown(Keys.Left))
                    {
                        //move piece left, if no collision
                        if (currentPiece.position.X > 1)
                        {
                            // move piece left 
                            currentPiece.position.X -= blockwidth;
                        }
                        else
                        {
                            // feedback?  User requested illegal move
                        }
                    }
                    if (currentState.IsKeyDown(Keys.Right) && !previousKeyState.IsKeyDown(Keys.Right))
                    {
                        if (currentPiece.position.X < 10)
                        {
                            // move piece left 
                            currentPiece.position.X += blockwidth;
                        }
                        else
                        {
                            // feedback?  User requested illegal move
                        }
                    }
                    if (currentState.IsKeyDown(Keys.Down) && !previousKeyState.IsKeyDown(Keys.Down))
                    {
                        if (currentPiece.position.Y + blockwidth < playFieldBounds.Bottom)
                        {
                            currentPiece.position.Y += blockwidth;
                        }
                    }

                    if (currentState.IsKeyDown(Keys.A) && !previousKeyState.IsKeyDown(Keys.A))
                    {
                        currentPiece.Rotate(false);
                    }

                    if (currentState.IsKeyDown(Keys.S) && !previousKeyState.IsKeyDown(Keys.S))
                    {
                        currentPiece.Rotate(true);
                    }

                    previousKeyState = currentState;
                    #endregion

                    #region PadInput
                    

                    if (currentPadState.DPad.Down == ButtonState.Pressed)
                    {
                        if (currentPiece.position.Y + blockwidth < playFieldBounds.Bottom)
                        {
                            currentPiece.position.Y += blockwidth;
                        }
                    }

                    if (currentPadState.DPad.Left == ButtonState.Pressed)
                    {
                        currentPiece.position.X -= blockwidth;
                    }

                    if (currentPadState.DPad.Right == ButtonState.Pressed)
                    {
                        currentPiece.position.X += blockwidth;
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
                    if (currentPiece.position.Y + blockwidth < playFieldBounds.Bottom)
                    {
                        currentPiece.position.Y += blockwidth;
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

            // assign location and speed for currentPiece, per levelNumber
            currentPiece.position.X = offsetPlayFieldX + (5 * blockwidth);
            currentPiece.position.Y = offsetPlayFieldY + (0 * blockwidth);



        }

        #endregion

    }
}
