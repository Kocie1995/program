using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using BombsAway.Properties;
using WMPLib;

namespace BombsAway
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Player.SoundLocation = "smb_mariodie.wav";
            Wplay.URL = "background.mp3";
        }
       
        #region Vars

        public PictureBox[] Bombs1 { get; set; } = new PictureBox[10];
        public PictureBox[] Explosives1 { get; set; } = new PictureBox[10];
        public PictureBox[] WorldObjects1 { get; set; } = new PictureBox[10];
        public Control[] DebugMenu1 { get; set; } = new Control[9];
        public PictureBox[] Npc1 { get; set; } = new PictureBox[2];
        public Random Rng { get; set; } = new Random();
        public bool PlayerJump1 { get; set; }
        public bool PlayerLeft1 { get; set; }
        public bool PlayerRight1 { get; set; }
        public bool LastDirRight1 { get; set; } = true;
        public bool GameOn1 { get; set; }
        public bool GodMode1 { get; set; }
        public bool Debug1 { get; set; }
        public string DebugLog1 { get; set; } = $"STARTED: {DateTime.Now}\n";
        public int Gravity1 { get; set; } = 20;
        public int Anim1 { get; set; }
        public int Force1 { get; set; }
        public int BombSize1 { get; set; } = 16;
        public int SpeedMovement1 { get; set; } = 3;
        public int SpeedJump1 { get; set; } = 3;
        public int SpeedFall1 { get; set; } = 3;
        public int Score1 { get; set; }
        public SoundPlayer Player { get; } = new SoundPlayer();
        public WindowsMediaPlayer Wplay { get; } = new WindowsMediaPlayer();

        #endregion
        #region Boolean Functions, "Check functions"
        public bool InAirNoCollision(PictureBox tar)
        {
            //sprawdza czy picturebox jest poza ramką
            return !OutsideWorldFrame(tar) && WorldObjects1.Where(obj => !tar.Bounds.IntersectsWith(obj.Bounds)).Any(obj => tar.Location.Y < WorldFrame.Width);
        }

        public bool OutsideWorldFrame(PictureBox tar)
        {
            if (tar.Location.X < 0) //czy wychodzi z lewej lub prawej strony
                return true;
            if (tar.Location.X > WorldFrame.Width) 
                return true;
            return tar.Location.Y + tar.Height > WorldFrame.Height - 3 || WorldObjects1.Where(obj => obj != null).Any(obj => tar.Bounds.IntersectsWith(obj.Bounds));
        }
        public bool Collision_Top(PictureBox tar)
        {
            foreach (var ob in WorldObjects1)
            {
                if (ob == null) continue;
                var temp1 = new PictureBox
                {
                    Bounds = ob.Bounds
                }; //tworzenie pojedynczego pixela do sprawdzenia kolizji i dochodzenia do ścian
                temp1.SetBounds(temp1.Location.X - 3, temp1.Location.Y - 1, temp1.Width + 6, 1);
                if (tar.Bounds.IntersectsWith(temp1.Bounds))
                    return true;
            }
            return false;
        }

        public bool Collision_Bottom(PictureBox tar)
        {
            foreach (var ob in WorldObjects1)
            {
                if (ob == null) continue;
                var temp1 = new PictureBox {Bounds = ob.Bounds};
                temp1.SetBounds(temp1.Location.X, temp1.Location.Y + temp1.Height, temp1.Width, 1);
                if (tar.Bounds.IntersectsWith(temp1.Bounds))
                    return true;
            }
            return false;
        }

        public bool Collision_Left(PictureBox tar)
        {
            foreach (var ob in WorldObjects1)
            {
                if (ob == null) continue;
                var temp1 = new PictureBox {Bounds = ob.Bounds};
                temp1.SetBounds(temp1.Location.X - 1, temp1.Location.Y + 1, 1, temp1.Height - 1);
                if (tar.Bounds.IntersectsWith(temp1.Bounds))
                    return true;
            }
            return false;
        }
        public bool Collision_Right(PictureBox tar)
        {
            foreach (var ob in WorldObjects1)
            {
                if (ob == null) continue;
                var temp2 = new PictureBox {Bounds = ob.Bounds};
                temp2.SetBounds(temp2.Location.X + temp2.Width, temp2.Location.Y + 1, 1, temp2.Height - 1);
                if (tar.Bounds.IntersectsWith(temp2.Bounds))
                    return true;
            }
            return false;
        }
        #endregion
        #region Clicks
        private void OnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var debug in DebugMenu1)
            {
                debug.Visible = true;
            }
        }

        private void OffToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (var debug in DebugMenu1)
            {
                debug.Visible = false;
            }
        }

        private void Debug_Godmode_Click(object sender, EventArgs e)
        {
            if (!GodMode1)
            {
                GodMode1 = true;
                pb_Player.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                GodMode1 = false;
                pb_Player.BorderStyle = BorderStyle.None;
            }
        }

        private void Debug_NoBombs_Click(object sender, EventArgs e)
        {
            if (!Debug1)
            {
                Debug1 = true;
                timer_Randombomb.Enabled = false;
                timer_Sec.Enabled = false;
            }
            else
            {
                Debug1 = false;
                timer_Randombomb.Enabled = true;
                timer_Sec.Enabled = true;
            }
        }

        private void Debug_PGravity_Click(object sender, EventArgs e)
        {
            Gravity1++;
        }

        private void Debug_MGravity_Click(object sender, EventArgs e)
        {
            Gravity1--;
        }

        private void Debug_PJump_Click(object sender, EventArgs e)
        {
            SpeedJump1++;
        }

        private void Debug_MJump_Click(object sender, EventArgs e)
        {
            SpeedJump1--;
        }

        private void Debug_PSpeed_Click(object sender, EventArgs e)
        {
            SpeedMovement1++;
        }

        private void Debug_MSpeed_Click(object sender, EventArgs e)
        {
            SpeedMovement1--;
        }

        private void Debug_Log_Click(object sender, EventArgs e)
        {
            var sF = new SaveFileDialog {DefaultExt = "txt"};
            if (sF.ShowDialog() != DialogResult.OK) return;
            using (var file = new StreamWriter(sF.FileName))
            {
                file.WriteLine(DebugLog1);
            }
        }
        #endregion
        #region Voids
        public void Dead()
        {
            if (GodMode1) return;
            Wplay.controls.stop();
            pb_Player.Visible = false;
            label_Dead.Visible = true;
            GameOn1 = false;
            Player.Play();
        }

        public void RemovePictureBoxAt(int x, int y)
        {
            foreach (var boom in Explosives1)
            {
                if (boom?.Location.X == x && boom.Location.Y == y)
                {
                    boom.Dispose();
                }
            }
        }

        public void Reset()
        {   //Reset
            label_Dead.Visible = false;
            Wplay.controls.play();
            var x = 0;
            foreach (var bomb in Bombs1)
            {
                if (bomb != null)
                {   //usuwa bomby
                    bomb.Dispose();
                    Bombs1[x] = null;
                }
                x++;
            }
            var x2 = 0;
            foreach (var boom in Explosives1)
            {
                if (boom != null)
                {   //usuwa bomby
                    boom.Dispose();
                    Bombs1[x2] = null;
                }
                x2++;
            }
            pb_Player.Visible = true;   //gracz widoczny i ustawiony na pozycji startowej
            pb_Player.Location = new Point(167, WorldFrame.Size.Height - 10 - pb_Player.Height);
            pb_NPC1.Location = new Point(1, WorldFrame.Size.Height - 1 - pb_NPC1.Height);
            pb_NPC2.Location = new Point(WorldFrame.Width-10, WorldFrame.Size.Height - 1 - pb_NPC2.Height);
            pb_Player.Image = Character.stand_r;
            Score1 = 0;
            BombSize1 = 16;
            GameOn1 = true;
        }
        public void CreateBoom(int x, int y)
        {   //tworzenie picturebox dla eksplozji
            var boom = new PictureBox
            {
                Name = "Boom",
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(BombSize1, BombSize1),
                Image = World.Boom,
                Location = new Point(x, y)
            };
            WorldFrame.Controls.Add(boom);
            Explosives1[0] = boom;
        }


        public void PaintBox(int x, int y, int w, int h, Color c)
        {   //przeciwko problemom z wybuchem
            var temp = new PictureBox
            {
                BackColor = c,
                Size = new Size(w, h),
                Location = new Point(x, y)
            };
            WorldFrame.Controls.Add(temp);
        }

        public void CreatePipe(int x)
        {   //tworz Pipe
            var y = WorldFrame.Height - 45;
            var pipe = new PictureBox
            {
                Name = "Pipe",
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(35, 45),
                Image = World.Pipe,
                Location = new Point(x, y)
            };
            WorldFrame.Controls.Add(pipe);
        }
        #endregion
        #region Keyboard
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Z:
                    Score1 += 120;
                    break;
                case Keys.X:
                    pb_Player.Top+=3;
                    break;
                case Keys.P:
                    if (GameOn1)
                    {
                        GameOn1 = false;         //pauza
                        label_Dead.Text = @"Pause";
                        label_Dead.Visible = true;
                    }
                    else
                    {
                        GameOn1 = true;          //Game Over
                        label_Dead.Text = @"Game Over";
                        label_Dead.Visible = false;
                    }
                    break;
                case Keys.Left:                 // 
                    if (GameOn1)
                    {
                        LastDirRight1 = false;   //animacja
                        PlayerLeft1 = true;     //
                    }
                    break;
                case Keys.Right:                //
                    if (GameOn1)
                    {
                        LastDirRight1 = true;
                        PlayerRight1 = true;
                    }
                    break;
                case Keys.Space:    // skok
                    if (label_Dead.Visible && !label_Dead.Text.Contains("Paused"))
                    {               // jesli pauza i kilkana jest space
                        Reset();    //Reset
                    }
                    else
                    {
                        if (!PlayerJump1 && !InAirNoCollision(pb_Player))
                        {   //Anty multijump
                            pb_Player.Image = LastDirRight1 ? Character.jump_r : Character.jump_l;
                            pb_Player.Top -= SpeedJump1;     //lekko w górę
                            Force1 = Gravity1;        //Force - grawitacja dla skoku
                            PlayerJump1 = true;     //zmienne skaczące
                        }
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!GameOn1) return;
            switch (e.KeyCode)
            {
                case Keys.Left:                             //nie wciskam lewego przycisku już
                    pb_Player.Image = Character.stand_l;    //zmiana obrazu na stojący
                    LastDirRight1 = false;                   //w kierunku lewym
                    PlayerLeft1 = false;                    //stoi
                    break;
                case Keys.Right:
                    pb_Player.Image = Character.stand_r;
                    LastDirRight1 = true;
                    PlayerRight1 = false;
                    break;
            }
        }
        #endregion
        #region TimerTicks | VIP Region
        private void Timer_Jump_Tick(object sender, EventArgs e)
        {
            if (GameOn1)
            {
                if (PlayerRight1 && pb_Player.Right <= WorldFrame.Width - 3 && !Collision_Left(pb_Player))
                { //nie wyjdzie poza ekran
                    pb_Player.Left += SpeedMovement1; //ruch w prawo
                }
                if (PlayerLeft1 && pb_Player.Location.X >= 3 && !Collision_Right(pb_Player))
                { //nie wyjdzie poza ekran
                    pb_Player.Left -= SpeedMovement1; //ruch w lewo
                }
            }
            else
            {   //gra nie zaczęta zatrzym gracza
                PlayerRight1 = false;
                PlayerLeft1 = false;
            }

            if (Force1 > 0)
            {   //jeśli Force istnieje
                if (Collision_Bottom(pb_Player))
                {   //dopóki nie dotknie ściany
                    Force1 = 0;
                }
                else
                {   //gracz porusza się w górę
                    Force1--;
                    pb_Player.Top -= SpeedJump1;
                }
            }
            else
            {   //nie ma force nie ma skoku
                PlayerJump1 = false;
            }
        }

        private void Timer_Anim_Tick(object sender, EventArgs e)
        {
            
            Anim1++; //animacja
            label1.Text = $@"Bombs: {GetBombsNum(Bombs1)}"; //liczba bomb spadających
            label2.Text = $@"Highscore: {Settings.Default.Highscore}";
            if (PlayerRight1 && Anim1 % 15 == 0)
            {   //Animacja
                pb_Player.Image = Character.walk_r;
            }
            if (PlayerLeft1 && Anim1 % 15 == 0)
            {
                pb_Player.Image = Character.walk_l;
            }

            foreach (var bomb in Bombs1)
            {   //interakcja z bombą
                if (bomb == null) continue;
                //jeśli bomba nie istnieje
                if (!pb_Player.Bounds.IntersectsWith(bomb.Bounds)) continue;
                if (bomb.Name == "Coin")
                { //jeśli to Coin to punkty
                    Score1++;
                    bomb.Dispose();
                }
                else
                {   //rip
                    Dead();
                    bomb.Dispose();
                   
                }
            }

            #region NPC
            foreach (var npc in Npc1)
            {
                if (npc.Bounds.IntersectsWith(pb_Player.Bounds))
                {
                    Dead();
                   
                }
                else
                {
                    if (npc.Location.X > pb_Player.Location.X && npc.Location.X < WorldFrame.Width && !Collision_Right(npc) && GameOn1)
                    {
                        npc.Left--;
                        npc.Image = Enemy.Enemy_left;
                    }
                    if (npc.Location.X >= pb_Player.Location.X || npc.Location.X <= 0 || Collision_Left(npc) ||
                        !GameOn1) continue;
                    npc.Left++;
                    npc.Image = Enemy.Enemy_right;
                }
            }
        }

        public bool NoCollision(PictureBox tar)
        {
            return WorldObjects1.Where(obj => obj != null).Any(obj => tar.Bounds.IntersectsWith(obj.Bounds));
        }
            #endregion

        private void Timer_Gravity_Tick(object sender, EventArgs e)
        {
            if (!PlayerJump1 && pb_Player.Location.Y + pb_Player.Height < WorldFrame.Height - 2 && !Collision_Top(pb_Player))
            {   //jesli nie skacze lokalizacja jest ponad podloga
                pb_Player.Top += SpeedFall1; //gracz przyciagany do ziemii
            }

            if (!PlayerJump1 && pb_Player.Location.Y + pb_Player.Height > WorldFrame.Height - 1)
            {   //jesli gracz pod podloga wyciagnij na gore
                pb_Player.Top--;
            }

            var x = 0;
            if (!GameOn1) return;
            foreach (var bomb in Bombs1) //dla każdej bomby
            {
                if (bomb != null)               // jeśli bomby istnieją
                {
                    try
                    {
                        if (!OutsideWorldFrame(bomb))
                        {   //jeśli bomba jest nad ziemią
                            if (bomb.Name == "pb" || bomb.Name == "Coin")
                            {
                                bomb.Top += 3;      //bomby spadają
                            }
                            if (bomb.Name == "pbR")
                            {
                                bomb.Left += 3;     //rakieta w prawo
                            }
                            if (bomb.Name == "pbL")
                            {
                                bomb.Left -= 3;     //rakieta w lewo
                            }
                        }
                        else // jeśli bomba nie jest nad ziemią
                        {
                            if (OutsideWorldFrame(bomb))
                            {   //jeśli rakieta wyjdzie z prawej jest usuwana
                                Bombs1[x] = null;
                                bomb.Dispose();
                                DebugLog1 += $"{DateTime.Now}: Removed rocket at {x}\n";
                            }
                            if (OutsideWorldFrame(bomb))
                            {   //jeśli rakieta wyjdzie z lewej jest usuwana
                                Bombs1[x] = null;
                                bomb.Dispose();
                                DebugLog1 += $"{DateTime.Now}: Removed rocket at {x}\n";
                            }   //wszystkie rakiety sa usuwane po dotknieciu obiektu
                            Bombs1[x] = null;
                            bomb.Dispose();
                            DebugLog1 += $"{DateTime.Now}: Removed \"bomb\" at {x}\n";
                            if (Explosives1[0] != null)
                            {
                                Explosives1[0].Dispose();
                                Explosives1[0] = null;
                            }
                            CreateBoom(bomb.Location.X, bomb.Location.Y);
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                x++; //na jakiej bombie z kolei pracujemy
                if (x >= 10)
                {   //wraca do 0 po 10
                    x = 0;
                }
            }
        }
        private void Timer_Randombomb_Tick(object sender, EventArgs e)
        {
            var rng = new Random();
            if (!GameOn1 && (GameOn1 || label_Dead.Visible)) return;
            if (GetBombsNum(Bombs1) == 10)
            {
                timer_BombFailsafe.Enabled = true;
            }
            else
            {
                timer_BombFailsafe.Enabled = false;
                {
                    var r = 2;
                    int nextSpot;
                    NextBomb(Bombs1);
                    if (Score1 > 20 && Score1 < 40) r = 12;
                    if (Score1 > 40 && Score1 < 80) r = 13;
                    if (Score1 > 80) r = 14;
                    switch (rng.Next(1, r))
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                            nextSpot = NextBomb(Bombs1);
                            var pb = new PictureBox();
                            pb.Name = "pb";
                            pb.BackColor = Color.Transparent;
                            pb.SizeMode = PictureBoxSizeMode.StretchImage;
                            pb.Size = new Size(16, 16);
                            pb.Image = Enemy.Bomb;
                            pb.Location = Score1 > 120 ? new Point(rng.Next(pb_Player.Location.X - 10, pb_Player.Location.X + 10), 0) : new Point(rng.Next(0, WorldFrame.Width), 0);
                            WorldFrame.Controls.Add(pb);
                            Bombs1[NextBomb(Bombs1)] = pb;
                            DebugLog1 += $"{DateTime.Now}: Added bomb at {nextSpot}\n";
                            break;
                        case 9:
                        case 10:
                        case 11:
                            nextSpot = NextBomb(Bombs1);
                            var coin = new PictureBox
                            {
                                Name = "Coin",
                                BackColor = Color.Transparent,
                                SizeMode = PictureBoxSizeMode.Zoom,
                                Size = new Size(20, 29),
                                Image = World.Coin,
                                Location = new Point(rng.Next(0, WorldFrame.Width), 0)
                            };
                            WorldFrame.Controls.Add(coin);
                            Bombs1[NextBomb(Bombs1)] = coin;
                            DebugLog1 += $"{DateTime.Now}: Added coin at {nextSpot}\n";
                            break;
                        case 12:
                            nextSpot = NextBomb(Bombs1);
                            var pbR = new PictureBox
                            {
                                Name = "pbR",
                                BackColor = Color.Transparent,
                                SizeMode = PictureBoxSizeMode.StretchImage,
                                Size = new Size(30, 20),
                                Image = Enemy.Rocket_R
                            };
                            pbR.Location = rng.Next(1, 3) == 1 ? new Point(1, 205) : new Point(1, 124);
                            WorldFrame.Controls.Add(pbR);
                            Bombs1[NextBomb(Bombs1)] = pbR;
                            DebugLog1 += $"{DateTime.Now}: Added bomb at {nextSpot}\n";
                            break;
                        case 13:
                            nextSpot = NextBomb(Bombs1);
                            var pbL = new PictureBox();
                            pbL.Name = "pbL";
                            pbL.BackColor = Color.Transparent;
                            pbL.SizeMode = PictureBoxSizeMode.StretchImage;
                            pbL.Size = new Size(30, 20);
                            pbL.Image = Enemy.Rocket_L;
                            pbL.Location = rng.Next(1, 3) == 1 ? new Point(WorldFrame.Width + 30, 205) : new Point(WorldFrame.Width + 30, 151);
                            WorldFrame.Controls.Add(pbL);
                            Bombs1[NextBomb(Bombs1)] = pbL;
                            DebugLog1 += $"{DateTime.Now}: Added bomb at {nextSpot}\n";
                            break;
                    }
                }
            }
        }

        private void Timer_Sec_Tick(object sender, EventArgs e)
        {
            for (var i = 0; i < 10; i++)
            {
                if (Bombs1[i] != null && Bombs1[i].IsDisposed)
                {
                    Bombs1[i] = null;
                }
            }
            label_Score.Text = @"Score: " + Score1;
            if (label_Dead.Visible) return;
            Score1++;
            BombSize1++;
            if (timer_Randombomb.Interval > 1)
            {
                timer_Randombomb.Interval--;
            }
            if (Score1 <= Settings.Default.Highscore) return;
            Settings.Default.Highscore = Score1;
            Settings.Default.Save();
        }

        private void TimerBoomRemove_Tick(object sender, EventArgs e)
        {
            foreach (Control x in Controls)
            {
                if (!(x is PictureBox)) continue;
                if (x.Name == "Boom")
                {
                    x.Dispose();
                }
            }

            foreach (var boom in Explosives1)
            {
                boom?.Dispose();
            }
        }

        private void Timer_BombFailsafe_Tick(object sender, EventArgs e)
        {   // failsafe
            DebugLog1 += DateTime.Now + ": Bombs - " + BombDebug();
            for (var i = 0; i < 10; i++)
            {   // jesli bedzie 10 bomb przez 3 sek wszystko jest resetowane
                if (Explosives1[0] != null)
                {
                    Explosives1[0].Dispose();
                    Explosives1[0] = null;
                }
                if (Bombs1[i] == null) continue;
                Bombs1[i].Dispose();
                Bombs1[i] = null;
            }
            DebugLog1 += $"{DateTime.Now}: Had to hard Failsafe\n";
            DebugLog1 += $"{DateTime.Now}: Bombs - {BombDebug()}";
        }
        #endregion
        #region Other
        public string BombDebug()
        {
            var t = "";
            for (var i = 0; i < 10; i++)
            {
                if (Bombs1[i] == null)
                {
                    t += 0;
                }
                else
                {
                    t += 1;
                }
            }
            return t;
        }

        public int GetBombsNum(PictureBox[] arr)
        {
            return arr.Count(bomb => bomb != null);
        }

        public int NextBomb(PictureBox[] arr)
        {
            if (GetBombsNum(arr) < 10)
            {
                for (var i = 0; i < 10; i++)
                {   //zwraca pierwsze miejsce w tablicy
                    if (arr[i] == null)
                    {
                        return i;
                    }
                }
            }   //failsafe uruchomiony
            Bombs1[0] = null;    //pierwsza bomba jest usuwana
            return 0;
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            Reset();
            WorldObjects1[0] = pb_Pipe;
            WorldObjects1[1] = pb_Block1;
            WorldObjects1[2] = pb_Block2;
            DebugMenu1[0] = debug_Log;
            DebugMenu1[1] = debug_Godmode;
            DebugMenu1[2] = debug_NoBombs;
            DebugMenu1[3] = debug_PGravity;
            DebugMenu1[4] = debug_MGravity;
            DebugMenu1[5] = debug_PJump;
            DebugMenu1[6] = debug_MJump;
            DebugMenu1[7] = debug_PSpeed;
            DebugMenu1[8] = debug_MSpeed;
            Npc1[0] = pb_NPC1;
            Npc1[1] = pb_NPC2;
        }


    }
}
