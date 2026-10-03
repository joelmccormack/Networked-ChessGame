using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace networking
{
    public class GameServer
    {
        private static readonly int PORT = 55555;
        private static UdpClient chessServerClient;
        private static UdpClient broadcastClient;
        private static List<Game> gameList = new List<Game>();
        static void Main(string[] args)
        {
            chessServerClient = new UdpClient();
            broadcastClient = new UdpClient(PORT);
            Console.WriteLine($"Listening on port: {PORT}");
            do
            {
                ListenForClient();
                Listening();               
            }
            while (true);
        }

        private static void ListenForClient()
        { 
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, PORT);
            if (broadcastClient.Available > 0)
            {
               
                string message = Encoding.ASCII.GetString(broadcastClient.Receive(ref iPEndPoint));
                Console.WriteLine("received message " + message);
                if (message.Trim() == "finding-server")
                {
                    Byte[] bytes = Encoding.ASCII.GetBytes("chess-server");
                    chessServerClient.Send(bytes, bytes.Length, iPEndPoint);
                }
            }
        }

        private static void Listening()
        {      
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            if (chessServerClient.Available <= 0)
            {
                return;
            }
            Byte[] bytes = chessServerClient.Receive(ref iPEndPoint);
            string data = Encoding.ASCII.GetString(bytes).Trim();
            String[] values = data.Split(";");
            string msgType = values[0];
            switch (msgType)
            {
                case "newgame":
                    {
                        Console.WriteLine("recieved");
                        string gameID = values[1].Trim();
                        Game game = new Game(gameID, iPEndPoint);
                        gameList.Add(game);
                        Console.WriteLine($"new server created with gameID: {gameID}");
                        break;
                    }
                case "joingame":
                    {
                        Console.WriteLine("recieved");
                        if (gameList.Count == 0)
                        {
                            Byte[] replyBytes1 = Encoding.ASCII.GetBytes("no-servers-available");
                            chessServerClient.Send(replyBytes1, replyBytes1.Length, iPEndPoint);
                            break;
                        }
                        string reply = "gameid;";
                        foreach (Game game in gameList)
                        {
                            if (game.isFull())
                            {
                                continue;
                            }
                            reply += game.gameID + ",";
                        }
                        reply = reply.Substring(0, reply.Length - 1);
                        Console.WriteLine("Sending" + reply);
                        Byte[] replyBytes2 = Encoding.ASCII.GetBytes(reply);
                        chessServerClient.Send(replyBytes2, replyBytes2.Length, iPEndPoint);
                        break;
                    }
                case "addplayer":
                    {
                        Console.WriteLine("recieved");
                        string gameID = values[1].Trim();
                        foreach (Game game in gameList)
                            if (gameID == game.gameID)
                            {
                                game.AddP2(iPEndPoint);
                                Console.WriteLine($"New Server: gameID = {game.gameID} player 1 ip = {game.getplayer1().ToString()} player 2 = {game.getplayer2().ToString()}");

                                Byte[] startbytes = Encoding.ASCII.GetBytes("playerJoined");
                                chessServerClient.Send(startbytes, startbytes.Length, game.getplayer1());
                            }
                        break;
                    }
                case "makemove":
                    {
                        Console.WriteLine("recieved");
                        foreach (Game game in gameList)
                        {
                            if (iPEndPoint.ToString() == game.getplayer1().ToString())
                            {
                                Console.WriteLine($"sending {data} to {game.getplayer2().ToString()}");
                                chessServerClient.Send(bytes, bytes.Length, game.getplayer2());
                            }
                            else if (iPEndPoint.ToString() == game.getplayer2().ToString())
                            {
                                chessServerClient.Send(bytes, bytes.Length, game.getplayer1());
                                Console.WriteLine($"sending {data} to {game.getplayer2().ToString()}");
                            }
                        }
                        break;
                    }
                case "deletegame":
                    {
                        Console.WriteLine("recieved");
                        string gameID = values[1].Trim();
                        foreach (Game game in gameList)
                        {
                            if(gameID == game.gameID)
                            {
                                gameList.Remove(game);
                                break;
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
            
        }
    }
}
