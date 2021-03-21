using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dodge_Game
{
    public partial class Form1 : Form
    {
        #region Variables
        #region Player Variables
        int playerX = 10;
        int playerY = 10;

        int paddleHeight = 20;
        int paddleWidth = 20;
        int paddleSpeed = 6;
        #endregion
        #region Player Bools
        bool wDown = false;
        bool aDown = false;
        bool sDown = false;
        bool dDown = false;
        #endregion
        #region Obstacle Variables And Lists
        List<int> leftObstacleYList = new List<int>();
        List<int> rightObstacleYList = new List<int>();
        int obstacleHeight = 50;
        int obstacleWidth = 10;
        int obstacleSpeed = 6;

        int movingObstacleCounter = 0;
        int flashObstacleCounter = 0;
        #endregion
        #region Brushes
        SolidBrush pinkBrush = new SolidBrush(Color.Pink);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        #endregion
        int counter = 0;
        #endregion
        public Form1()
        {
            InitializeComponent();
            leftObstacleYList.Add(0);
            rightObstacleYList.Add(275);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                #region Player Controls
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                    #endregion
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                #region Player Controls
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                    #endregion
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            flashObstacleCounter++;
            movingObstacleCounter++;
            #region Player Movement
            if (wDown == true && playerY > 0)
            {
                playerY -= paddleSpeed;
            }
            if (sDown == true && playerY < this.Height)
            {
                playerY += paddleSpeed;
            }
            if (aDown == true && playerY > 0)
            {
                playerX -= paddleSpeed;
            }
            if (dDown == true && playerY < this.Width)
            {
                playerX += paddleSpeed;
            }
            #endregion
            #region Obstacles
            // spawn obstacles
            counter++;

            if (movingObstacleCounter == 40)
            {
                leftObstacleYList.Add(0);
                rightObstacleYList.Add(275);
                movingObstacleCounter = 0;
            }
            #endregion
            #region Obstacle Movement
            // obstacle movement

            for (int i = 0; i < leftObstacleYList.Count(); i++)
            {
                leftObstacleYList[i] += obstacleSpeed;
            }
            for (int i = 0; i < rightObstacleYList.Count(); i++)
            {
                rightObstacleYList[i] -= obstacleSpeed;
            }
            #endregion
            #region Obstacle deletetion
            //obstacle delete
            for (int i = 0; i < leftObstacleYList.Count(); i++)
            {
                if (leftObstacleYList[i] > this.Height)
                {
                    leftObstacleYList.RemoveAt(i);
                }
            }
            for (int i = 0; i < rightObstacleYList.Count(); i++)
            {
                if (rightObstacleYList[i] < 0)
                {
                    rightObstacleYList.RemoveAt(i);
                }
            }
            #endregion
            #region Player And Obstacle Collisions
            // player obstacle collisios
            Rectangle playerRec = new Rectangle(playerX, playerY, paddleWidth, paddleHeight);

            #region Obstacle 1
            for (int i = 0; i < leftObstacleYList.Count(); i++)
            {
                Rectangle leftObstacle = new Rectangle(100, leftObstacleYList[i], obstacleWidth, obstacleWidth);

                if (playerRec.IntersectsWith(leftObstacle))
                {
                    GameOver();
                }
            }
            #endregion
            #region Obstacle 2
            for (int i = 0; i < rightObstacleYList.Count; i++)
            {
                Rectangle rightObstacle = new Rectangle(275, rightObstacleYList[i], obstacleWidth, obstacleWidth);

                if (playerRec.IntersectsWith(rightObstacle))
                {
                    GameOver();
                }
            }
            #endregion
            #region Obstacle 3
            Rectangle flashBarRec = new Rectangle(375, 0, 10, this.Height);
            if (flashObstacleCounter >= 20 && flashObstacleCounter <= 40)
            {
                if (playerRec.IntersectsWith(flashBarRec))
                {
                    GameOver();
                }
                flashObstacleCounter = 0;
            }
            #endregion
            #endregion
            #region Player Win
            //player wins
            if (playerX >= this.Width - paddleHeight)
            {
                outputLabel.Text = "You Won";
                gameTimer.Enabled = false;
            }
            #endregion
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            #region Draw Player
            // Player
            e.Graphics.FillRectangle(pinkBrush, playerX, playerY, paddleWidth, paddleHeight);
            #endregion
            #region Draw Obstacles
            //Obstacles
            //Obstacle 1
            for (int i = 0; i < leftObstacleYList.Count; i++)
            {
                e.Graphics.FillRectangle(whiteBrush, 100, leftObstacleYList[i], obstacleWidth, obstacleHeight);
            }
            //Obstacle
            for(int i = 0; i < rightObstacleYList.Count; i++)
            {
                e.Graphics.FillRectangle(whiteBrush, 275, rightObstacleYList[i], obstacleWidth, obstacleHeight);
            }
            //Obstaacle 3
            if (flashObstacleCounter >= 20 && flashObstacleCounter <= 40)
            {
                e.Graphics.FillRectangle(blackBrush, 375, 0, 10, this.Height);
                flashObstacleCounter = 0;
            }
            else
            {
                e.Graphics.FillRectangle(whiteBrush, 375, 0, 10, this.Height);
            }
            #endregion
        }
        public void GameOver()
        {
            outputLabel.Text = "You Lost";
            gameTimer.Enabled = false;
        }
    }
}