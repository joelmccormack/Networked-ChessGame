using System.CodeDom;

namespace ChessGame
{
    public partial class ChessUI : Form
    {
        private BoardState boardState = new BoardState();
        private readonly Dictionary<Position, Button> allButtons = new Dictionary<Position, Button>(); // Dictionary 
        private ChessClient chessClient;
        private List<Position> nextpositions;
        Piece pieceToMove;
        public bool turn = true;
        public bool multiplayer;
        public bool player;
        public ChessUI(ChessClient chessClient)
        {
            InitializeComponent();
            this.chessClient = chessClient;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            btnMenu.Hide();
            BuildBoard();
            if (multiplayer)
            {
                if (player != turn)
                {
                    turn = player;
                    flipBoard();
                    DisableButtons();
                }
                btn_flip.Hide();
            }
        }

        public void BuildBoard() // method to make a 8x8 board of buttons
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Position pos = new Position(row, col);
                    Button square = new Button()
                    {
                        Height = 150,
                        Width = 150,
                        Location = new Point((row * 150) + 200, (col * 150) + 200),
                        BackColor = (pos.getY() + pos.getX()) % 2 == 0 ? Color.White : Color.LightGray,
                        Font = new Font("Arial", 40, FontStyle.Bold),
                        Tag = pos,
                    };
                    Controls.Add(square);
                    allButtons.Add(pos, square);
                    square.Click += Square_Click; //method for when square is clicked

                }
            }
            EnableButtons(turn);
            drawBoardState(boardState);
        }

        private void defaultBackColor() // method to reset the colour of the board
        {
            foreach (KeyValuePair<Position, Button> entry in allButtons) //reseting colour of each square 
            {
                Button b = entry.Value;
                Position p = entry.Key;
                b.BackColor = (p.getY() + p.getX()) % 2 == 0 ? Color.White : Color.LightGray;
            }
        }

        private void Square_Click(object? sender, EventArgs e)
        {
            EnableButtons(turn);
            defaultBackColor();
            bool moved = false;
            Button sendingButton = (Button)sender; //getting button clicked
            Position pos = (Position)sendingButton.Tag; //getting position of button clicked
            Piece piece = boardState.getPieceAt(pos); //trying to get piece on button clicked


            if (piece != null && piece.isWhite() == turn) //if clicked on a piece highlight it
            {
                sendingButton.BackColor = Color.Aquamarine;
            }

            if (pieceToMove != null) //if a piece was clicked on last time try to move it to the square clicked
            {
                moved = pieceToMove.movePiece(pos, boardState);
                if (moved) //if piece was successful in moving update current position of the piece, set the piece waiting to move to null, clear list of next positions and draw the new boardstate onto the buttons
                {
                    bool gameOver = boardState.inCheckMate(pieceToMove.isWhite());
                    if (gameOver)
                    {
                        btnMenu.Show();
                        if (piece.inCheck(boardState, !pieceToMove.isWhite()))
                        {
                            drawBoardState(boardState);
                            boardState.finishGame(false, pieceToMove.isWhite(), allButtons);
                            btn_flip.Enabled = false;
                            btnSurrender.Enabled = false;
                        }
                        else
                        {
                            boardState.StaleMate();
                        }
                    }
                    else
                    {
                        drawBoardState(boardState);
                        if (!multiplayer)
                        {
                            if (turn)
                            {
                                turn = false;
                            }
                            else
                            {
                                turn = true;
                            }
                        }
                        EnableButtons(turn);
                        pieceToMove = null;
                        nextpositions.Clear();
                    }

                }
            }
            if (piece != null && !moved && piece.isWhite() == turn) //if no piece has succesfully moved, and hae clicked on a square with a piece set this to the new piece to move and find all of its next positions and add to the list 
            {
                pieceToMove = piece;
                nextpositions = piece.nextPositions(boardState);
                EnableNextPosButton(nextpositions);
            }
            if (piece == null && !moved) //if no piece was clicked adn failed to move a piece set the piece waiting to move to null
            {
                pieceToMove = null;
            }
            if(multiplayer && moved)
            {
                chessClient.SendMove(boardState);
                DisableButtons();
            }
        }

        private void flipBoard()
        {
            //line 146 and 147, for converting a dictionary to 2 lists from stack overflow https://stackoverflow.com/questions/4038978/map-two-lists-into-a-dictionary-in-c-sharp
            List<Position> keys = allButtons.Select(kvp => kvp.Key).ToList();
            List<Button> Values = allButtons.Select(kvp => kvp.Value).ToList();
            Values.Reverse();
            for (int i = 0; i < keys.Count; i++)
            {
                allButtons[keys[i]] = Values[i];
            }
            foreach (KeyValuePair<Position, Button> entry in allButtons)
            {
                Position p = entry.Key;
                Button b = entry.Value;
                b.Tag = p;
            }
            EnableButtons(turn);
            defaultBackColor();
            drawBoardState(boardState);


        }

        private void drawBoardState(BoardState boardState) //displaying all pieces on corresponding buttons
        {
            foreach (KeyValuePair<Position, Button> entry in allButtons)
            {
                Position pos = entry.Key;
                Button button = entry.Value;
                Piece picece = boardState.getPieceAt(pos);
                if (picece != null)
                {
                    button.Text = picece.unicode();
                }
                else
                {
                    button.Text = null;
                }
            }
        }

        private void EnableButtons(bool turn) //enabling the buttons of the players pieces each turn
        {
            foreach (KeyValuePair<Position, Button> entry in allButtons)
            {
                Position pos = entry.Key;
                Button button = entry.Value;
                Piece piece = boardState.getPieceAt(pos);
                if (piece != null)
                {
                    if (piece.isWhite() != turn)
                    {
                        button.Enabled = false;
                    }
                    else if (piece.isWhite() == turn)
                    {
                        button.Enabled = true;
                    }
                }
            }
        }
        private void EnableNextPosButton(List<Position> nextpositions) //enabling buttons of a pieces next positions
        {
            foreach (KeyValuePair<Position, Button> entry in allButtons)
            {
                Position pos = entry.Key;
                Button button = entry.Value;
                if (nextpositions != null && nextpositions.Contains(pos))
                {
                    button.Enabled = true;
                }
            }
        }

        private void btn_flip_Click(object sender, EventArgs e) //when btn_flip is clicked run this
        {
            if (!multiplayer)
            {
                flipBoard();
            }
        }

        private void btnSurrender_Click(object sender, EventArgs e) //ending game if player surrenders
        {
            boardState.finishGame(true, !turn, allButtons);
            btnMenu.Show();
            btn_flip.Enabled = false;
            btnSurrender.Enabled = false;
            if (multiplayer)
            {
                chessClient.Surrender();
            }
        }

        private void DisableButtons()
        {
            foreach (KeyValuePair<Position, Button> entry in allButtons)
            {
                Button button = entry.Value;
                button.Enabled = false;
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            ChessMenu menu = new ChessMenu();
            menu.Show();
            this.Hide();
        }

        internal void setBoardState(BoardState boardState)
        {
            this.boardState = boardState;          
            drawBoardState(this.boardState);
            EnableButtons(turn);
        }
    }
}
