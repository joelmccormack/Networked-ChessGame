using System.Net;

namespace networking
{
    internal class Game
    {
        internal readonly string gameID;
        readonly IPEndPoint player1;
        IPEndPoint player2;

        public Game(string gameID, IPEndPoint player1)
        {
            this.gameID = gameID;
            this.player1 = player1;
            this.player2 = null;
        }
        internal void AddP2(IPEndPoint ipEndpoint)
        {
            player2 = ipEndpoint;
        }
        internal IPEndPoint getplayer1()
        {
            return player1;
        }
        internal IPEndPoint getplayer2()
        {
            return player2;
        }
        internal bool isFull()
        {
            return player2 != null;
        }


    }
}