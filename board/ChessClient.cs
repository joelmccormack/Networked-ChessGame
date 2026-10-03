using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ChessGame
{

    public class ChessClient
    {
    
        private UdpClient udpClient;
        static readonly int PORT = 55555;
        readonly IPEndPoint remoteIP = new IPEndPoint(IPAddress.Broadcast, PORT);
        private IPEndPoint serverIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
        public bool serverConnected = false;
        
        public ChessClient()
        {
            udpClient = new UdpClient();
        }
        internal void connected()
        {
            if (serverConnected == false)
            {
                Byte[] bytes = ASCIIEncoding.ASCII.GetBytes("finding-server");
                udpClient.Send(bytes, bytes.Length, remoteIP);

                if (udpClient.Available > 0)
                {
                    string message = ASCIIEncoding.ASCII.GetString(udpClient.Receive(ref serverIPEndPoint));
                    if (message == "chess-server")
                    {
                        serverConnected = true;
                    }
                }
            }
        }
 
        internal void SendMove(BoardState boardState) //need to work here
        {
            string data = (BoardstateToString(boardState));
            Byte[] bytes = Encoding.ASCII.GetBytes(data);
            udpClient.Send(bytes, bytes.Length, serverIPEndPoint);
        }
        public string BoardstateToString(BoardState boardstate) //need to work here
        {
            string board = "makemove;"; 
            Dictionary<Position, Piece> pieces = boardstate.allPieces;
            int size = pieces.Count;
            foreach (KeyValuePair<Position, Piece> entry in pieces)
            {
                Position pos = entry.Key;
                Piece piece = entry.Value;
                if (piece == null)
                {
                    continue;
                }
                int x = pos.getX();
                int y = pos.getY();
                int posValue = (y * 8) + x;
                string pieceValue = pieceToString(piece);
                board += ($"{posValue.ToString()}={pieceValue},");
            }
            board = board.Substring(0, board.Length-1);
            return board;
        }

        private string pieceToString(Piece piece)
        {
            string pieceValue = null;
            int value = piece.value;
            switch (value)
            {
                case (1):
                    {
                        pieceValue = "P";
                        break;
                    }    
                case (2):
                    {
                        pieceValue = "p";
                        break;
                    }
                case (3):
                    {
                        pieceValue = "R";
                        break;
                    }
                case (4):
                    {
                        pieceValue = "r";
                        break;
                    }
                case (5):
                    {
                        pieceValue = "H";
                        break;
                    }
                case (6):
                    {
                        pieceValue = "h";
                        break;
                    }
                case (7):
                    {
                        pieceValue = "B";
                        break;
                    }
                case (8):
                    {
                        pieceValue = "b";
                        break;
                    }
                case (9):
                    {
                        pieceValue = "Q";
                        break;
                    }
                case (10):
                    {
                        pieceValue = "q";
                        break;
                    }
                case (11):
                    {
                        pieceValue = "K";
                        break;
                    }
                case (12):
                    {
                        pieceValue = "k";
                        break;
                    }
                default: 
                    break;

            }
            return pieceValue;


        }

        public BoardState StringToBoardState(string board)
        {
            BoardState boardstate = new BoardState();
            boardstate.allPieces.Clear();
            string[] values = board.Split(";");
            string[] piecePos = values[1].Split(",");
            for (int i = 0; i < piecePos.Length; i++)
            {
                string[] pos = piecePos[i].Split("=");
                int num = Convert.ToInt32(pos[0]);
                int x = (num) % 8;
                int y = (num) / 8;
                Position position = new Position(x, y);
                Piece piece = parsePiece(position, boardstate, pos[1]);
                boardstate.addAt(position, piece);
            }
            return boardstate;
        }

        private Piece parsePiece(Position position, BoardState boardstate, string piece)
        {
            switch (piece)
            {
                case "P":
                    {
                        return new Pawn(position, true);
                    }
                case "p":
                    {
                        return new Pawn(position, false);

                    }
                case "R":
                    {
                        return new Rook(position, true);
                    }
                case "r":
                    {
                        return new Rook(position, false);
                    }
                case "H":
                    {
                        return new Knight(position, true);
                    }
                case "h":
                    {
                        return new Knight(position, false);
                    }
                case "B":
                    {
                        return new Bishop(position, true);
                    }
                case "b":
                    {
                        return new Bishop(position, false);
                    }
                case "Q":
                    {
                        return new Queen(position, true);
                    }
                case "q":
                    {
                        return new Queen(position, false);
                    }
                case "K":
                    {
                        return new King(position, true);
                    }
                case "k":
                    {
                        return new King(position, false);
                    }

                default:
                break;
            }
            return null;

               
        }

        public void newGame(string servername)
        {
            Byte[] bytes = Encoding.ASCII.GetBytes($"newgame;{servername}");
            udpClient.Send(bytes, bytes.Length, serverIPEndPoint);
            MessageBox.Show("waiting for player to join");
        }
        public void joinGame()
        {
            List<Button> buttons = new List<Button>();
            Byte[] bytes = Encoding.ASCII.GetBytes("joingame");
            udpClient.Send(bytes, bytes.Length, serverIPEndPoint);          
        }

        internal void Surrender() //code
        {
            
        }

        internal void join(Button? btn)
        {
            string message = ($"addplayer;{btn.Text}");
            Byte[] bytes = Encoding.ASCII.GetBytes(message);
            udpClient.Send(bytes, bytes.Length, serverIPEndPoint);
        }
        internal string CheckServerEvents()
        {
            if (udpClient.Available > 0)
            {
                Byte[] bytes = udpClient.Receive(ref serverIPEndPoint);
                string data = Encoding.ASCII.GetString(bytes);
                return data;
            }
            return null;
        }
    }
}
