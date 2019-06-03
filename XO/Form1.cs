using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace XO
{
    public partial class Form1 : Form
    {
        //entries
        int Person = 0;
        int Computer = 0;

        PictureBox[] playingField = new PictureBox[9];

        int[] moves =
        {
            0, 0, 0,
            0, 0, 0,
            0, 0, 0
        };

        int _heighP = 100;
        int _widthP = 100;

        Label colon;
        Label crossСounter;
        Label zeroCounter;

        Label playerOfCross;
        Label playerOfZero;

        Label results;

        PictureBox cross;
        PictureBox zero;

        Button btnRetry;
        Button btnMenu;

        internal string[] pictureDictionary =
        {
            "black.png", "xx.png","null.png"
        };

        public Form1()
        {
            InitializeComponent();
        }
        
        void MainField()
        {
            int X = 105;
            int Y = 160;

            int PictureCount = 0;

            string nameCell = "Cell_";
            for (int YY = 0; YY < 3; YY++)
            {
                for (int XX = 0; XX < 3; XX++)
                {
                    playingField[PictureCount] = new PictureBox()
                    {
                        Width = _widthP,
                        Height = _heighP,
                        Name = nameCell + PictureCount,

                        Image = Image.FromFile("black.png"),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Location = new Point(X, Y)
                    };
                    this.Controls.Add(playingField[PictureCount]);

                    //event
                    playingField[PictureCount].Click += Person_Step;

                    PictureCount++;
                    X += _widthP + 10;
                }

                Y += _heighP + 10;
                X = 105;
            }
        }

        bool Win(int player)
        {

            int[,] WinningCombination =
            {
                {
                    1,1,1,
                    0,0,0,
                    0,0,0
                },
                {
                    0,0,0,
                    1,1,1,
                    0,0,0
                },
                {
                    0,0,0,
                    0,0,0,
                    1,1,1
                },
                {
                    1,0,0,
                    1,0,0,
                    1,0,0
                },
                {
                    0,1,0,
                    0,1,0,
                    0,1,0
                },
                {
                    0,0,1,
                    0,0,1,
                    0,0,1
                },
                {
                    1,0,0,
                    0,1,0,
                    0,0,1
                },
                {
                    0,0,1,
                    0,1,0,
                    1,0,0
                }

            };

            int[] playerMoves = new int[moves.Length];
            for (int i = 0; i < playerMoves.Length; i++)
            {
                if (moves[i] == player)
                    playerMoves[i] = 1;
            }

            int winningSteps = 0;

            for (int winningCombinationIndex = 0;
                winningCombinationIndex < WinningCombination.GetUpperBound(0);
                winningCombinationIndex++)
            {
                for (int playerMovesIndex = 0;
                    playerMovesIndex < playerMoves.Length;
                    playerMovesIndex++)
                {
                    if (1 == WinningCombination[winningCombinationIndex, playerMovesIndex])
                    {
                        if (WinningCombination[winningCombinationIndex, playerMovesIndex]
                            == playerMoves[playerMovesIndex])
                            winningSteps++;
                    }
                }

                if (winningSteps == 3)
                {
                    Counter(player);
                    return true;
                }
                else
                    winningSteps = 0;
            }
            return false;
        }

        void LockField()
        {
            foreach (PictureBox p in playingField)
                p.Enabled = false;
        }

        void UnlockField()
        {
            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i] == 0)
                    playingField[i].Enabled = true;
            }

        }

        bool CanIStep()
        {
            foreach (int s in moves)
            {
                if (s == 0)
                    return true;
            }
            
            if (Win(Person))
            {
                LockField();
                PartlyHideForm2();
                LoadForm3();
                results.Text = "YOU WON!";

                //YOU WIN
                return false;
            }

            if (Win(Computer))
            {
                LockField();
                PartlyHideForm2();
                LoadForm3();
                results.Text = "YOU LOSE";
                //YOU LOSE
                return false;
            }

            LockField();
            PartlyHideForm2();
            LoadForm3();
            results.Text = "NOBODY WON";
            //NOBODY WON
            return false;
        }

        void Person_Step(object sender, EventArgs e)
        {
            if (CanIStep())
            {
                PictureBox selectedPictureClick = sender as PictureBox;
                int indexSelectedPicture = Convert.ToInt32(Regex.Replace(selectedPictureClick.Name, @"[^\d]+", ""));

                playingField[indexSelectedPicture].Image = Image.FromFile(pictureDictionary[Person]);
                moves[indexSelectedPicture] = Person;

                if (!Win(Person))
                {
                    LockField();
                    Computer_Step();
                    UnlockField();
                }
                else
                {
                    LockField();
                    PartlyHideForm2();
                    LoadForm3();
                    results.Text = "YOU WON!";

                }
            }
        }

        void Computer_Step()
        {
            Random r = new Random();

            if (CanIStep())
            {
            GenerationStart:
                int indexComputerStep = r.Next(0, 8);
                if (moves[indexComputerStep] == 0)
                {
                    playingField[indexComputerStep].Image = Image.FromFile(pictureDictionary[Computer]);
                    moves[indexComputerStep] = Computer;
                }
                else goto GenerationStart;

                if (Win(Computer))
                {
                    LockField();
                    PartlyHideForm2();
                    LoadForm3();
                    results.Text = "YOU LOSE";
                }
            }
        }

        void LoadForm2()
        {
            MainField();

            cross = new PictureBox()
            {
                Width = _widthP,
                Height = _heighP,
                Name = "Result_Cross",
                Location = new Point(x: 35, y: 15),
                Image = Image.FromFile(pictureDictionary[1]),
                SizeMode = PictureBoxSizeMode.StretchImage

            };
            this.Controls.Add(cross);

            zero = new PictureBox()
            {
                Width = _widthP,
                Height = _heighP,
                Name = "Result_Zero",
                Location = new Point(x: 400, y: 15),
                Image = Image.FromFile(pictureDictionary[2]),
                SizeMode = PictureBoxSizeMode.StretchImage

            };
            this.Controls.Add(zero);

            colon = new Label()
            {
                Width = 50,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(x: 245, y: 55),
                Text = ":",
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold),
            };
            this.Controls.Add(colon);

            crossСounter = new Label()
            {
                Width = 50,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(x: 185, y: 55),
                Text = "0",
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold),
            };
            this.Controls.Add(crossСounter);

            zeroCounter = new Label()
            {
                Width = 50,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(x: 305, y: 55),
                Text = "0",
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold),
            };
            this.Controls.Add(zeroCounter);

            playerOfCross = new Label()
            {
                Width = 170,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(x: 0, y: 116),
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold),
            };
            this.Controls.Add(playerOfCross);

            playerOfZero = new Label()
            {
                Width = 170,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(x: 370, y: 116),
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold),
            };
            this.Controls.Add(playerOfZero);

        }

        void HideForm1()
        {
            this.label1.Visible = false;
            this.pictureBox1.Visible = false;
            this.pictureBox2.Visible = false;
        }

        void LoadForm3()
        {
            results = new Label()
            {
                Width = 270,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(x: 135, y: 220),
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold),
            };
            this.Controls.Add(results);

            btnRetry = new Button()
            {
                Location = new Point(x: 70, y: 300),
                Text = "RETRY",
                Size = new Size(400, 60),
                BackColor = Color.Yellow,
                ForeColor = Color.Black,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold),
            };
            this.Controls.Add(btnRetry);
            btnRetry.Click += Retry;

            btnMenu = new Button()
            {
                Location = new Point(x: 70, y: 375),
                Text = "RETURN TO MENU",
                Size = new Size(400, 60),
                BackColor = Color.Yellow,
                ForeColor = Color.Black,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold),

            };
            this.Controls.Add(btnMenu);
            btnMenu.Click += BackToMenu;
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Person = 1;
            Computer = 2;

            HideForm1();
            LoadForm2();
            FillLabelWithWhoPlaysWhat();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Person = 2;
            Computer = 1;

            HideForm1();
            LoadForm2();
            FillLabelWithWhoPlaysWhat();
        }

        void FillLabelWithWhoPlaysWhat()
        {
            playerOfCross.Text = Person == 1 ? "PLAYER" : "COMPUTER";
            playerOfZero.Text = Person == 2 ? "PLAYER" : "COMPUTER";
        }

        void Retry(object sender, EventArgs e)
        {
            PartlyHideForm3();

            MainField();
            moves = new int[]
            {0,0,0,
                0,0,0,
                0,0,0
            };

            foreach (PictureBox p in playingField)
                p.Image = Image.FromFile("black.png");
            UnlockField();

        }
        void PartlyHideForm2()
        {
            //hide only mainfield
            foreach (PictureBox p in playingField)
                p.Visible = false;
        }

        
        void PartlyHideForm3()
        {
            this.results.Visible = false;
            this.btnRetry.Visible = false;
            this.btnMenu.Visible = false;

        }

        void FullyHideForm3()
        {
            PartlyHideForm3();
            HideHeadline();
        }

        void HideHeadline()
        {
            this.playerOfCross.Visible = false;
            this.playerOfZero.Visible = false;

            this.cross.Visible = false;
            this.zero.Visible = false;
            this.colon.Visible = false;
            this.crossСounter.Visible = false;
            this.zeroCounter.Visible = false;
        }

        void BackToMenu(object sender, EventArgs e)
        {

            HideHeadline();
            FullyHideForm3();
            
            LoadForm1();

            Person = 0;
            Computer = 0;

            moves = new int[]
            {0,0,0,
                0,0,0,
                0,0,0
            };

            foreach (PictureBox p in playingField)
                p.Image = Image.FromFile("black.png");
            UnlockField();

        }

        void LoadForm1()
        {
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            label1.Visible = true;
        }

        void Counter(int player)
        {

            if (player == 1)
            {
                crossСounter.Text = (int.Parse(crossСounter.Text) + 1).ToString();
            }

            if (player == 2)
            {
                zeroCounter.Text = (int.Parse(zeroCounter.Text) + 1).ToString();
            }

        }
    }
}
