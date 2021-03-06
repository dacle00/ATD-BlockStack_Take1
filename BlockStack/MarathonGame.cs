﻿///////////////////////////////////////////////////////////////////////////////////////////
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
    /// <summary>
    /// 
    /// </summary>
    class MarathonGame
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        // global variables

        // Screen
        int boardWidth;
        int boardHeight;
        GameBoard theBoard;
        public const int offsetPlayFieldX = 20;  //pixels from window boarder to playfield
        public const int offsetPlayFieldY = 100; //pixels from window boarder to playfield
        Rectangle playFieldBounds = new Rectangle(offsetPlayFieldX, offsetPlayFieldY, 320, 640);
        Rectangle scoreFieldBounds = new Rectangle(20, 20, 500, 60);
        Rectangle nextPieceFieldBounds = new Rectangle(360, 100, 160, 160);

        // Game Pieces
        List<Tetromino> availablePieces;  // a list containing 1 of each possible unique tetromino piece
        Tetromino currentPiece;  // the piece in play, dropping down the gameboard
        Tetromino nextPiece;  // TODO:  make array[3] to see further ahead??
        Vector2 pieceStartingScreenPosition = new Vector2(offsetPlayFieldX + (3 * blockwidth), offsetPlayFieldY + (0 * blockwidth));  // PIXELS
        Vector2 pieceStartingBoardPosition = new Vector2(3, 0);                                                                       // BOARD GRID
        Vector2 nextPieceDisplayPosition = new Vector2(376f, 116f); //within the box, the 4x4 grid is displayed plus a 16 pixel padding on each side;
        const int blockwidth = 32;  //got rid of separate width/height vars as we're dealing with squares, right???
        BlockBag tetrisPieceFactory;  //gives another "random" tetris piece, used to get next pieces.
        CountdownTimer pieceLandedTimer;
        int pieceLandedPauseTime = 10000; // milliseconds  is

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
        bool clockwise = false;
        bool counterclockwise = true; // default rotation direction

        // Game State

        public enum GameState
        {
            running, paused, ended
        }
        GameState gameState;

        public enum PieceState
        {
            none, falling, landed
        }
        PieceState pieceState;


        public MarathonGame(int pLevel, ContentManager pContentManager)
        {
            boardWidth = 10;  // Standard board width (number of cells).
            boardHeight = 20; // Standard board height (number of cells).
            theBoard = new GameBoard(boardWidth, boardHeight, offsetPlayFieldX, offsetPlayFieldY);
            levelNumber = pLevel;
            stepTime = 0.5f - (0.05f * (levelNumber - 1));
            Load(pContentManager);
            tetrisPieceFactory = new BlockBag(availablePieces);

            lastTimeDropped = null;
            lastTimeUpdate = null;

            gameState = GameState.running;
            pieceState = PieceState.none;

            pieceLandedTimer = new CountdownTimer();
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
            if (gameState == GameState.running)
            {
                // game play is running

                if (pieceState == PieceState.falling)
                {
                    if (currentPiece != null)
                    {

                        // check user input
                        UpdateUserInput(gt);

                        // timer drops the piece
                        UpdatePieceDrop(gt);

                        // Collision Checks
                        CheckCurrentPieceLanded(gt);
                        CheckLineClear();  //TODO:  move this call to happen when piece is landed.
                    }
                    else
                    {
                        // Cycle to next piece.
                        CycleNextPiece();
                    }
                    // update Scores
                    UpdateScore(0);

                    //TODO:  Other MetaData to update?  piece tallies, time display, etc
                }
                else if (pieceState == PieceState.landed)
                {
                    /* TODO:
                     * check if a timer is currently running. (some sort of decrementing timer)
                     * * if there is no currently running timer, start timer until piece locks into place, leave time for a rotation or sideways movement.
                     * * if there is a running timer, check user input.
                     * when timer runs out, set it to 0 or null, not running. Also, lock piece into place and cycle to next piece.
                    */
                    if (!pieceLandedTimer.isActive)
                        pieceLandedTimer.setAndStart(gt, pieceLandedPauseTime);
                    else
                    {
                        UpdateUserInput(gt);
                        pieceLandedTimer.checkExpired(gt);
                    }

                    if (pieceLandedTimer.isComplete)
                    {
                        pieceLandedTimer.reset();
                        pieceState = PieceState.none;
                        //TODO:  check if piece is now un-landed (moved sideways off of whatever piece was below itself)
                    }


                }
                else if (pieceState == PieceState.none)
                {
                    if (currentPiece != null)
                        theBoard.AddLandedPiece(currentPiece);
                    CycleNextPiece();
                }
            }
            else if (gameState == GameState.paused)
            {
                // game play is paused
                //TODO: display pause menu, take user input for options.
            }
            else
            {
                //game play has ended
                //TODO: dispplay gameover menu. take user input for options.
            }
        }


        #region UpdateSupportFunctions


        private void UpdateUserInput(GameTime gt)
        {
            if (lastTimeUpdate != null)
            {
                if (gt.TotalGameTime >= lastTimeUpdate.TotalGameTime.Add(new TimeSpan(0, 0, 0, 0, 100)))
                {
                    #region Keyboard input
                    KeyboardState currentState = Keyboard.GetState();
                    if (currentState.IsKeyDown(Keys.Left) && !previousKeyState.IsKeyDown(Keys.Left))
                    {
                        //move piece left if, once moved, piece would remain inside GameBoard
                        if (CanMoveLeft())
                        {
                            // move piece left 
                            currentPiece.screenPosition.X -= blockwidth;
                            currentPiece.boardPosition.X -= 1;
                        }
                        else
                        {
                            // feedback?  User requested illegal move
                        }
                    }
                    if (currentState.IsKeyDown(Keys.Right) && !previousKeyState.IsKeyDown(Keys.Right))
                    {
                        if (CanMoveRight())
                        {
                            // move piece right 
                            currentPiece.screenPosition.X += blockwidth;
                            currentPiece.boardPosition.X += 1;
                        }
                        else
                        {
                            // feedback?  User requested illegal move
                        }
                    }
                    if (currentState.IsKeyDown(Keys.Down) && !previousKeyState.IsKeyDown(Keys.Down))
                    {
                        if (CanMoveDown())
                        {
                            // move piece down
                            currentPiece.screenPosition.Y += blockwidth;
                            currentPiece.boardPosition.Y += 1;
                            lastTimeDropped = new GameTime(gt.TotalGameTime, gt.ElapsedGameTime);
                        }
                        else
                        {
                            //collision is handled elsewhere
                        }
                    }

                    if (currentState.IsKeyDown(Keys.A) && !previousKeyState.IsKeyDown(Keys.A))
                    {
                        currentPiece.Rotate(clockwise);
                    }

                    if (currentState.IsKeyDown(Keys.S) && !previousKeyState.IsKeyDown(Keys.S))
                    {
                        currentPiece.Rotate(counterclockwise);
                    }

                    previousKeyState = currentState;
                    #endregion

                    #region PadInput
                    GamePadState currentPadState = GamePad.GetState(PlayerIndex.One);

                    if (currentPadState.DPad.Left == ButtonState.Pressed)
                    {
                        //move piece left if, once moved, piece would remain inside GameBoard
                        if (CanMoveLeft())
                        {
                            // move piece left 
                            currentPiece.screenPosition.X -= blockwidth;
                            currentPiece.boardPosition.X -= 1;
                        }
                        else
                        {
                            // feedback?  User requested illegal move
                        }
                    }

                    if (currentPadState.DPad.Right == ButtonState.Pressed)
                    {
                        //move piece right if, once moved, piece would remain inside GameBoard
                        if (CanMoveRight())
                        {
                            // move piece right 
                            currentPiece.screenPosition.X += blockwidth;
                            currentPiece.boardPosition.X += 1;
                        }
                        else
                        {
                            // feedback?  User requested illegal move
                        }
                    }

                    if (currentPadState.DPad.Down == ButtonState.Pressed)
                    {
                        if (currentPiece.screenPosition.Y + blockwidth < playFieldBounds.Bottom)
                        {
                            if (CanMoveDown())
                            {
                                // move piece down
                                currentPiece.screenPosition.Y += blockwidth;
                                currentPiece.boardPosition.Y += 1;
                                lastTimeDropped = new GameTime(gt.TotalGameTime, gt.ElapsedGameTime);
                            }
                            else
                            {
                                //collision is handled elsewhere
                            }
                        }
                    }

                    if (currentPadState.Buttons.A == ButtonState.Pressed && !(previousPadState.Buttons.A == ButtonState.Pressed))
                    {
                        currentPiece.Rotate(clockwise);
                    }

                    if (currentPadState.Buttons.B == ButtonState.Pressed && !(previousPadState.Buttons.A == ButtonState.Pressed))
                    {
                        currentPiece.Rotate(counterclockwise);
                    }

                    lastTimeUpdate = new GameTime(gt.TotalGameTime, gt.ElapsedGameTime);

                    if (currentPadState.Buttons.A == ButtonState.Pressed && !(previousPadState.Buttons.A == ButtonState.Pressed))
                    {
                        currentPiece.Rotate(clockwise);
                    }

                    if (currentPadState.Buttons.B == ButtonState.Pressed)
                    {
                        currentPiece.Rotate(counterclockwise);
                    }

                    previousPadState = currentPadState;
                    #endregion
                }
            }
            else
            {
                lastTimeUpdate = new GameTime(gt.TotalGameTime, gt.ElapsedGameTime);
            }
        }

        /// <summary>
        /// Handles when the user input specifically accelerates piece-drop.
        /// </summary>
        /// <param name="gt"></param>
        private void UpdatePieceDrop(GameTime gt)
        {
            if (lastTimeDropped != null)
            {
                if (gt.TotalGameTime >= lastTimeDropped.TotalGameTime.Add(new TimeSpan(0, 0, 0, 0, 500)))
                {
                    if (currentPiece.screenPosition.Y + blockwidth < playFieldBounds.Bottom)
                    {
                        if (CanMoveDown())
                        {
                            // move piece down
                            currentPiece.screenPosition.Y += blockwidth;
                            currentPiece.boardPosition.Y += 1;
                            lastTimeDropped = new GameTime(gt.TotalGameTime, gt.ElapsedGameTime);
                        }
                        else
                        {
                            //collision is handled elsewhere
                        }
                    }
                }
            }
            else
            {
                lastTimeDropped = new GameTime(gt.TotalGameTime, gt.ElapsedGameTime);
            }
        }


        /// <summary>
        /// using the bounds in the parameter, check each block of the piece to see if that block can move Left
        /// </summary>
        /// <returns></returns>
        public bool CanMoveLeft()
        {
            bool retVal = true;

            foreach (Block b in currentPiece.blockList)
            {
                //determine sub-block's position relative to tetrominos position
                int xPos = Convert.ToInt16(currentPiece.boardPosition.X + b.position.X);
                int yPos = Convert.ToInt16(currentPiece.boardPosition.Y + b.position.Y);

                //check bounds of gameboard
                if ((xPos - 1) < 0)
                    retVal = false;
                //check for other pieces
                else if (theBoard.pile[yPos].data[xPos - 1].isFilled)
                    retVal = false;
            }

            return retVal;
        }


        /// <summary>
        /// using the bounds in the parameter, check each block of the piece to see if that block can move Left
        /// </summary>
        /// <returns></returns>
        public bool CanMoveRight()
        {
            bool retVal = true;

            foreach (Block b in currentPiece.blockList)
            {
                //determine sub-block's position relative to tetrominos position
                int xPos = Convert.ToInt16(currentPiece.boardPosition.X + b.position.X);
                int yPos = Convert.ToInt16(currentPiece.boardPosition.Y + b.position.Y);

                //check bounds of gameboard
                if ((xPos + 1) > (boardWidth - 1))
                    retVal = false;
                //check for other pieces
                else if (theBoard.pile[yPos].data[xPos + 1].isFilled)
                    retVal = false;
            }

            return retVal;
        }


        /// <summary>
        /// using the bounds in the parameter, check each block of the piece to see if that block can move Left
        /// </summary>
        /// <returns></returns>
        public bool CanMoveDown()
        {
            bool retVal = true;

            foreach (Block b in currentPiece.blockList)
            {
                //determine sub-block's position relative to tetrominos position
                int xPos = Convert.ToInt16(currentPiece.boardPosition.X + b.position.X);
                int yPos = Convert.ToInt16(currentPiece.boardPosition.Y + b.position.Y);

                //check gameboard's bounds
                if ((yPos + 1) > (boardHeight - 1))
                    retVal = false;
                //check for other pieces
                else if (theBoard.pile[yPos + 1].data[xPos].isFilled)
                    retVal = false;

            }

            return retVal;
        }


        /// <summary>
        /// Checks below currentPiece for collision with bottom of board, or with another landed piece.
        /// </summary>
        public void CheckCurrentPieceLanded(GameTime gt)
        {
            //examine each sub-block within currentPiece
            foreach (Block b in currentPiece.blockList)
            {
                //determine sub-block's position relative to tetrominos position
                int xPos = Convert.ToInt16(currentPiece.boardPosition.X + b.position.X);
                int yPos = Convert.ToInt16(currentPiece.boardPosition.Y + b.position.Y);

                // case #1.  LANDED ON BOTTOM
                if (yPos >= boardHeight - 1)
                {
                    pieceState = PieceState.landed;
                    pieceLandedTimer.setAndStart(gt, pieceLandedPauseTime);
                }

                // case #2.  LANDED ON ANOTHER PIECE
                else if (theBoard.pile[yPos + 1].data[xPos].isFilled)
                {
                    pieceState = PieceState.landed;
                    pieceLandedTimer.setAndStart(gt, pieceLandedPauseTime);
                }
            }
        }


        public void CheckLineClear()
        {
        }


        public void CycleNextPiece()
        {
            if (nextPiece == null)
                nextPiece = tetrisPieceFactory.GetNextPiece();

            currentPiece = nextPiece;
            nextPiece = tetrisPieceFactory.GetNextPiece();
            nextPiece.screenPosition = nextPieceDisplayPosition;

            // assign location and speed for currentPiece, per levelNumber
            currentPiece.screenPosition = pieceStartingScreenPosition;  // pixels
            currentPiece.boardPosition = pieceStartingBoardPosition; //board grid

            pieceState = PieceState.falling;
        }


        public void UpdateScore(int scoreDelta)
        {
            score += scoreDelta;
        }


        #endregion

    }
}
