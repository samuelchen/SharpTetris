
//=======================================================================
// <copyright file="PlayPanel.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : A control to play game on.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

using Net.SamuelChen.Tetris.Blocks;
using Net.SamuelChen.Tetris.Service;
using Net.SamuelChen.Tetris.Controller;
using Net.SamuelChen.Tetris.Skin;
using System.Collections.Generic;

namespace Net.SamuelChen.Tetris.Game {

    /// <summary>
    /// The panel of playing field.
    /// </summary>
    public class PlayPanel : System.Windows.Forms.PictureBox {

        /// <summary>
        /// ctor()
        /// </summary>
        public PlayPanel() {

            m_minBlocks = m_maxBlocks = Shape.DEFAULT_BLOCK_NUM;
            this.Columns = 17;
            this.Rows = 22;
            this.BlockWidth = this.BlockHeight = 16;

            Initialize();
        }

        public void Initialize() {

            m_Cells = new Block[Rows, Columns];
            for (int i = 0; i < Rows; i++) {
                for (int j = 0; j < Columns; j++) {
                    m_Cells[i, j] = new Block(j, i, EnumBlockType.Blank);
                    if (i == 0 || i == Rows - 1 || j == 0 || j == Columns - 1) {
                        m_Cells[i, j].Type = EnumBlockType.Wall;
                    }
                }
            }

            for (int i = 2; i < Columns - 1; i++) {
                m_Cells[Rows - 2, i].Type = EnumBlockType.RoadBlock;
            }

            this.Size = new Size(BlockWidth * Columns, BlockHeight * Rows);
            this.TabStop = false;
            this.m_curShape = null;
            this.m_nextShape = null;
            this.Status = EnumGameStatus.Initialized;
            this.RePaint();

            //m_Game = theGame;
            CreateNextShape();
        }

        #region Properties

        /// <summary>
        /// How many blocks a row in play panel. including the left and right ones as the walls.
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        /// How many blocks a cloumn in play panel. including the top and bottom ones as the walls.
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// The block width in pixel
        /// </summary>
        public int BlockWidth = 16;

        /// <summary>
        /// The block height in pixel
        /// </summary>
        public int BlockHeight = 16;


        /// <summary>
        /// The game status of current play panel.
        /// </summary>
        public EnumGameStatus Status { get; set; }

        /// <summary>
        /// Min block number of a shape. Between Shape.MIN_BLOCK_NUM and Shape.MAX_BLOCK_NUM
        /// </summary>
        public int MinShapeBlockNumber {
            set {

                if (value < Shape.MIN_BLOCK_NUM || value > Shape.MAX_BLOCK_NUM || value > this.MaxShapeBlockNumber)
                    throw (new ArgumentException("The min block number of a shape is incorrect."));
            }
            get {
                return this.m_minBlocks;
            }
        }

        /// <summary>
        /// Max block number of a shape. Between Shape.MIN_BLOCK_NUM and Shape.MAX_BLOCK_NUM
        /// </summary>
        public int MaxShapeBlockNumber {
            set {

                if (value < Shape.MIN_BLOCK_NUM || value > Shape.MAX_BLOCK_NUM || value < this.MinShapeBlockNumber)
                    throw (new ArgumentException("The max block number of a shape is incorrect."));
            }
            get {
                return this.m_maxBlocks;
            }
        }

        #endregion


        #region Game Methods

        public void Go() {
            MoveCurShape(EnumMoving.Down);
        }

        public void Go(object act) {
            EnumMoving move = (EnumMoving) act;
            if (move == EnumMoving.Pause) {
                if (this.Status == EnumGameStatus.Paused)
                    this.Status = EnumGameStatus.Running;
                else if (this.Status == EnumGameStatus.Running)
                    this.Status = EnumGameStatus.Paused;
            }
            MoveCurShape(move);
        }

        /// <summary>
        /// Show the play panel on specified place
        /// </summary>
        /// <param name="x">top</param>
        /// <param name="y">left</param>
        public void Show(int x, int y) {
            System.Windows.Forms.Form form = (System.Windows.Forms.Form)this.Parent;
            if (null == form)
                return;

            this.Location = new Point(x, y);
            this.Visible = true;

            base.Show();
        }

        /// <summary>
        /// Check the game filed status line by line.
        /// </summary>
        protected void CheckLines() {
            bool bFull;
            List<int> lines = new List<int>();

            // check from top to bottom
            for (int i = 1; i < Rows - 1; i++) {
                bFull = true;
                for (int j = 1; j < Columns - 1; j++) {
                    if (EnumBlockType.Blank == m_Cells[i, j].Type) {
                        bFull = false;
                        break;
                    }
                }
                if (true == bFull) {
                    lines.Add(i);
                }
            }

            DestroyLine(lines);

            // score
            //m_Counter.ScoreAdd(nLines);
            //m_InfoPanel.Invalidate();
        }

        /// <summary>
        /// Destory a line.
        /// </summary>
        /// <param name="nLine"></param>
        protected void DestroyLine(int nLine) {
            // destory effective drawing
            for (int j = 1; j < Columns - 1; j++) {
                m_Cells[nLine, j].Type = EnumBlockType.Destory;
            }
            RePaint();
            System.Threading.Thread.Sleep(120);
            for (int j = 1; j < Columns - 1; j++) {
                m_Cells[nLine, j].Type = EnumBlockType.RoadBlock;
            }
            RePaint();

            // move the upper row down
            for (int i = nLine; i > 1; i--) {
                for (int j = 1; j < Columns - 1; j++) {
                    m_Cells[i, j].Type = m_Cells[i - 1, j].Type;
                }
            }

            // first row
            for (int j = 1; j < Columns - 1; j++) {
                m_Cells[1, j].Type = EnumBlockType.Blank;
            }

            RePaint();
        }

        protected void DestroyLine(IList<int> lines) {
            if (null == lines || lines.Count < 1)
                return;

            // destory effective drawing
            for (int i = 0; i < lines.Count; i++) {
                for (int j = 1; j < Columns - 1; j++) {
                    m_Cells[lines[i], j].Type = EnumBlockType.Destory;
                }
            }
            RePaint();
            System.Threading.Thread.Sleep(120);
            for (int i = 0; i < lines.Count; i++) {
                for (int j = 1; j < Columns - 1; j++) {
                    m_Cells[lines[i], j].Type = EnumBlockType.RoadBlock;
                }
            }
            RePaint();

            // move the upper rows down
            for (int l = 0; l < lines.Count; l++) {
                for (int i = lines[l]; i > 1; i--) {
                    for (int j = 1; j < Columns - 1; j++) {
                        m_Cells[i, j].Type = m_Cells[i - 1, j].Type;
                    }
                }

                // first row
                for (int j = 1; j < Columns - 1; j++) {
                    m_Cells[1, j].Type = EnumBlockType.Blank;
                }
            }

            RePaint();
        }

        /// <summary>
        /// Check whther current game on this play panel is defeated.
        /// </summary>
        /// <returns></returns>
        protected bool Defeated() {
            for (int j = 1; j < Columns - 1; j++) {
                if (m_Cells[1, j].Type != EnumBlockType.Blank)
                    return true;
            }
            return false;
        }

        #endregion

        #region Shape
        /// <summary>
        /// Create a new shape as next shape and place current next shape to field.
        /// </summary>
        public void CreateNextShape() {
            int num = ShapeFactory.GetRandomBlocksNumber(this.MinShapeBlockNumber, this.MaxShapeBlockNumber);
            ShapeFactory factory = ShapeFactory.CreateInstance(num);
            if (null != m_curShape)
                m_curShape.Moved -= new ShapeMovingHandler(OnCurShape_Moved);
            m_curShape = m_nextShape;
            if (null != m_curShape)
                m_curShape.Moved += new ShapeMovingHandler(OnCurShape_Moved);

            m_nextShape = factory.CreateRandomShape();

            //if (null != m_nextShape && null != m_InfoPanel) {
            //    m_InfoPanel.Block = m_nextShape;
            //}
        }

        /// <summary>
        /// move current shape.
        /// </summary>
        /// <param name="theDirection">the moving direction</param>
        public bool MoveCurShape(EnumMoving theDirection) {
            if (this.Status != EnumGameStatus.Running)
                return false;

            if (EnumMoving.DirectDown == theDirection) {
                while (this.MoveCurShape(EnumMoving.Down)) {
                }
                return true;
            }

            if (this.CanMove(theDirection)) {
                m_curShape.Move(theDirection);
            }else if (EnumMoving.Down == theDirection) {
                // can not be moving down
                if (null != m_curShape) {
                    DestroyCurShape();
                    CheckLines();
                }
                if (Defeated()) {
                    this.Status = EnumGameStatus.Defeated;
                    RePaint();
                } else
                    CreateNextShape();
                return false;
            }
                

            return true;
        }


        /// <summary>
        /// Destory current shape. Current block becomes roadblocks
        /// </summary>
        protected void DestroyCurShape() {
            if (null == m_curShape)
                return;

            Block[] blocks = m_curShape.Blocks;
            for (int i = 0; i < blocks.Length; i++) {
                m_Cells[blocks[i].Y, blocks[i].X] = blocks[i];
                m_Cells[blocks[i].Y, blocks[i].X].Type = EnumBlockType.RoadBlock;
            }
            m_curShape = null;
        }

        /// <summary>
        /// Check whether current shape can move to speicified direction
        /// </summary>
        /// <param name="theDirection">the specified direct</param>
        /// <returns>ture if can, otherwise false</returns>
        protected bool CanMove(EnumMoving theDirection) {
            if (null == m_curShape)
                return false;

            Block[] blocks = m_curShape.Blocks;
            if (null == blocks)
                return false;

            switch (theDirection) {
                case EnumMoving.Left:
                    for (int i = 0; i < blocks.Length; i++) {
                        if (EnumBlockType.Blank != m_Cells[blocks[i].Y, blocks[i].X - 1].Type)
                            return false;
                    }
                    break;
                case EnumMoving.Right:
                    for (int i = 0; i < blocks.Length; i++) {
                        if (EnumBlockType.Blank != m_Cells[blocks[i].Y, blocks[i].X + 1].Type)
                            return false;
                    }
                    break;
                case EnumMoving.Down:
                    for (int i = 0; i < blocks.Length; i++) {
                        if (EnumBlockType.Blank != m_Cells[blocks[i].Y + 1, blocks[i].X].Type)
                            return false;
                    }
                    break;
                case EnumMoving.Rotate:
                    return CanRotate();
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// Check whether current block can rotate.
        /// </summary>
        /// <returns>true if can, otherwise false</returns>
        protected bool CanRotate() {
            if (null == m_curShape)
                return false;

            // the blocks of current shape
            Block[] oriBlocks = m_curShape.Blocks;
            if (oriBlocks.Length <= 0)
                return false;

            // temp blocks
            Block[] tarBlocks = new Block[oriBlocks.Length];
            Block origin = oriBlocks[(int)(oriBlocks.Length + 1) / 2];

            // rotated temp blocks
            int x, y;
            for (int i = 0; i < tarBlocks.Length; i++) {
                x = oriBlocks[i].Y - origin.Y + origin.X;
                y = origin.X - oriBlocks[i].X + origin.Y;
                tarBlocks[i] = new Block(x, y);
            }

            // check if it's valid.
            for (int i = 0; i < tarBlocks.Length; i++) {
                if (EnumBlockType.Blank != m_Cells[tarBlocks[i].Y, tarBlocks[i].X].Type)
                    return false;
            }

            return true;
        }

        #endregion


        #region Drawing

        /// <summary>
        /// repaint the play panel
        /// </summary>
        public void RePaint() {
            this.Invalidate();
        }

        /// <summary>
        /// OnPaint event
        /// </summary>
        /// <param name="e">the paint event argument</param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
            base.OnPaint(e);
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            DrawBackground(e.Graphics);
            DrawCurShape(e.Graphics);
            string sStatus = string.Empty;
            switch (this.Status) {
                case EnumGameStatus.Initialized:
                    sStatus = m_skin.GetString("ready!");
                    break;
                case EnumGameStatus.Running:
                    break;
                case EnumGameStatus.Paused:
                    sStatus = m_skin.GetString("pause!");
                    break;
                case EnumGameStatus.Over:
                    sStatus = m_skin.GetString("game_over!");
                    break;
                case EnumGameStatus.Defeated:
                    sStatus = m_skin.GetString("defeated!");
                    break;
                default:
                    break;
            }
            if (sStatus.Length > 0) {
                e.Graphics.DrawString(sStatus, new Font("tahoma", 20, FontStyle.Bold),
                    Brushes.Teal, 61, 151);
                e.Graphics.DrawString(sStatus, new Font("tahoma", 20, FontStyle.Bold),
                    Brushes.Red, 60, 150);
            }
#if DEBUG
            e.Graphics.DrawString(sDebug, new Font("tahoma", 8), Brushes.Blue, 10, 20);
#endif
        }

        /// <summary>
        /// Draw background (the roadbloacks)
        /// </summary>
        /// <param name="graph">the gdi+ graphics object</param>
        protected void DrawBackground(Graphics graph) {
            for (int i = 0; i < Rows; i++) {
                for (int j = 0; j < Columns; j++) {
                    DrawBlock(graph, m_Cells[i, j]);
                }
            }
        }

        /// <summary>
        /// draw current shape
        /// </summary>
        /// <param name="graph"></param>
        protected void DrawCurShape(Graphics graph) {
            if (null == m_curShape) {
                return;
            }

            for (int i = 0; i < m_curShape.Blocks.Length; i++) {
                DrawBlock(graph, m_curShape.Blocks[i]);
            }
        }

        /// <summary>
        /// Draw a block
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="theDiamond">block instance</param>
        protected void DrawBlock(Graphics graph, Block block) {
            Image blockImage;
            string sResName;
            //if (block.Type == EnumBlockType.Blank)
            //    return;

            sResName = block.Type.ToString().ToLower();

            blockImage = m_skin.GetImage(sResName);
            if (null != blockImage)
                graph.DrawImage(blockImage, block.X * BlockWidth, block.Y * BlockHeight);
        }

        #endregion

        #region Events
        /// <summary>
        /// Controller pressed.
        /// </summary>
        /// <param name="sender">the controller</param>
        /// <param name="e"></param>
        //protected void OnController_Pressed(object sender, ControllerPressedEventArgs e) {
        //    if (this.Status != EnumGameStatus.Running)
        //        return;

        //    EnumMoving theDirection;
        //    switch (e.key) {
        //        case enumControllerKey.adKeyLeft:
        //            theDirection = EnumMoving.Left;
        //            break;
        //        case enumControllerKey.adKeyRight:
        //            theDirection = EnumMoving.Right;
        //            break;
        //        case enumControllerKey.adKeyDown:
        //            theDirection = EnumMoving.Down;
        //            break;
        //        case enumControllerKey.adKeyDirectDown :
        //            theDirection = EnumMoving.DownDirectly;
        //            break;
        //        case enumControllerKey.adKeyRotate:
        //            theDirection = EnumMoving.Rotate;
        //            break;
        //        default:
        //            theDirection = EnumMoving.None;
        //            break;
        //    }

        //    if (EnumMoving.DirectDown == theDirection) {
        //        // go down to bottom directly
        //        while (CanMove(EnumMoving.Down)) {
        //            m_curShape.Move(EnumMoving.Down);
        //        }
        //        MoveCurBlock(EnumMoving.Down);
        //    }else
        //        MoveCurBlock(theDirection);
        //}

        /// <summary>
        /// Fired when a shape moved
        /// </summary>
        /// <param name="sender">the moving shape</param>
        /// <param name="e"></param>
        protected void OnCurShape_Moved(object sender, ShapeMovingEventArgs e) {
            RePaint();
        }

        #endregion


        #region Fields

        //protected Game			m_Game;					// the game instance
        protected IController m_Controller;			// the controller instance
        protected Player m_Player;				// the player
        //protected CCounter		m_Counter;				// counter
        //protected CInfomationPanel m_InfoPanel;			// information panel

        protected Block[,] m_Cells;				// background cells
        protected Shape m_curShape;				// current shape
        protected Shape m_nextShape;			// the next shape
        protected int m_minBlocks;            // Min block number of a shape
        protected int m_maxBlocks;            // Max block number of a shape

        //protected Setting m_setting = Setting.Instance;
        protected Skins m_skin = Skins.Instance;
        public string sDebug = string.Empty;

        #endregion
    }
}
