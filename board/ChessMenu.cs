using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessGame
{
    public partial class ChessMenu : Form
    {
        ChessClient chessClient = new ChessClient();
        ChessUI currentgame;
        public ChessMenu()
        {

            InitializeComponent();
            TimerConnect.Interval = 1000;
            TimerConnect.Tick += TimerConnect_Tick;
            TimerConnect.Start();
            TimerCheckEvents.Interval = 100;
            TimerCheckEvents.Tick += TimerCheckEvents_Tick;
        }

        private void ChessMenu_Load(object sender, EventArgs e)
        {
            btnNewGame.Hide();
            btnJoinGame.Hide();
            TBServerName.Hide();
            btnEnter.Hide();
        }

        private void btnOnline_Click(object sender, EventArgs e)
        {
            if (chessClient.serverConnected)
            {
                btnOnline.Hide();
                btnPass.Hide();
                btnNewGame.Show();
                btnJoinGame.Show();
                TimerCheckEvents.Start();
            }
            else
            {
                MessageBox.Show("Server not online or have not been able to connect");
            }
        }

        private void btnPass_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChessUI game = new ChessUI(chessClient);
            game.multiplayer = false;
            game.Show();
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            btnJoinGame.Hide();
            TBServerName.Show();
            btnNewGame.Enabled = false;
            btnEnter.Show();
        }

        private void btnJoinGame_Click(object sender, EventArgs e)
        {
            btnNewGame.Hide();
            btnJoinGame.Hide();
            chessClient.joinGame();
        }

        private void button_Click(object? sender, EventArgs e)
        {
            Button btn = (Button)sender;
            chessClient.join(btn);
            startGame(false);
        }

        //need to delete
        private void btnEnter_Click(object sender, EventArgs e)
        {
            btnEnter.Enabled = false;
            TBServerName.Enabled = false;
            string serverName = TBServerName.Text.Trim();
            chessClient.newGame(serverName);
        }

        private void TimerConnect_Tick(object sender, EventArgs e)
        {
            chessClient.connected();
            if (chessClient.serverConnected)
            {
                TimerConnect.Stop();
            }
        }

        private void TimerCheckEvents_Tick(object? sender, EventArgs e)
        {
            string data = chessClient.CheckServerEvents();
            if (data != null) 
            { 
                string[] strings = data.Split(";");
                switch (strings[0])
                {
                    case "playerJoined":
                        {
                            startGame(true);
                            break;
                        }
                    case "gameid":
                        {
                            int pos = 0;
                            string[] games = strings[1].Split(",");
                            foreach (string game in games)
                            {
                                Button btnGame = new Button()
                                {
                                    Height = 100,
                                    Width = 200,
                                    Location = new Point(1300, 1000 + pos),
                                    Text = game,
                                    Tag = game
                                };
                                Controls.Add(btnGame);
                                btnGame.Click += button_Click;
                                pos += 100;
                            }
                            break;
                        }
                    case "makemove":
                        {
                            BoardState boardState = chessClient.StringToBoardState(data);
                            this.currentgame.setBoardState(boardState);
                            break;
                        }
                        default: { break; }
                       
                }

            }

        }
        private void startGame(bool player)
        { 
            this.Hide();
            currentgame = new ChessUI(chessClient);
            currentgame.multiplayer = true;
            currentgame.player = player;
            currentgame.Show();
        }
    }
}
