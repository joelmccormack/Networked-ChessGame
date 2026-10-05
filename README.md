# Networked Chess Game

## Overview

Networked Chess Game is a multiplayer chess desktop application built with C# and .NET 6.0, combining traditional chess logic with modern networking capabilities. The project demonstrates how to coordinate game state across networked players using UDP communication while maintaining a responsive Windows Forms interface. It supports both local pass-and-play and networked multiplayer modes, with features including piece-specific movement validation, real-time board-state synchronization, and game-session management. This project showcases the integration of game logic, networking, state management, and event-driven GUI development in a cohesive desktop application.

## Key Features

- **Multiplayer Networking** — Play chess against another player over UDP network using a dedicated game server
- **Local Pass-and-Play** — Play against a friend on the same machine without networking
- **Game-Session Management** — Create named game sessions and discover available sessions on the network
- **UDP-Based Communication** — Real-time game-state synchronization between networked players using UDP sockets
- **Interactive Windows Forms Interface** — Drag-and-drop board interaction with visual feedback
- **Piece Selection and Movement** — Click pieces to select, with valid move destinations highlighted
- **Strict Move Validation** — Individual piece classes enforce chess movement rules
- **All Chess Pieces Implemented** — Pawn, Rook, Knight, Bishop, Queen, King with piece-specific movement rules
- **Board State Synchronization** — Networked players stay synchronized as moves are exchanged
- **Turn Management** — Alternating turns between players (local or networked)

## How the Game Works

**Local Mode:**
1. Launch the game and select "Pass and Play"
2. White starts first; click a piece to select it
3. Valid destination squares highlight automatically
4. Click a destination to move the piece
5. Turns alternate between white and black
6. Use the "Flip" button to rotate the board for the other player

**Networked Mode:**
1. Start the GameServer in a terminal (listens on port 55555)
2. Launch the game client; it auto-discovers the running server
3. Host creates a new named game session or join an existing one
4. Second player joins the same session
5. Moves are exchanged in real-time; board updates on both clients
6. Turns alternate between networked players

## Multiplayer Networking

The networking layer uses **UDP (User Datagram Protocol)** for low-latency game-state updates:

**Architecture:**
- **Dedicated Server** — GameServer listens on port 55555 for client connections
- **Discovery Mechanism** — Clients broadcast "finding-server" to discover active server; server responds with "chess-server"
- **Session Management** — GameServer maintains a list of active Game sessions; each session pairs two players
- **Message Protocol** — Clients send messages prefixed with message type (e.g., "newgame;...", "joingame;...", "makemove;...")
- **State Synchronization** — Board state is serialized to a string format (piece positions and types) and transmitted between players

**Key Classes:**
- **GameServer** — Listens for incoming connections and message requests; manages the list of active game sessions
- **Game** — Represents a single game session with two player endpoints; tracks player1 and player2 network addresses
- **ChessClient** — Handles UDP communication; connects to the server, creates or joins games, sends/receives board updates

**Flow:**
1. Client sends "finding-server" broadcast → Server responds with "chess-server"
2. Client sends "newgame;GameName" or "joingame" → Server creates or joins session
3. Players exchange moves via "makemove;positionData" messages
4. Board state serialized as position-to-piece mappings; reconstructed on receiving end

## Chess Logic

The chess implementation combines a shared base architecture with piece-specific movement rules:

**Board Representation:**
- 8×8 chessboard represented as an `int x int` grid (0-7)
- **BoardState** class maintains a Dictionary<Position, Piece> mapping each position to its occupant
- **Position** struct (immutable) represents x, y coordinates with helper methods (getX, getY, addX, addY)
- Pieces initialized in standard chess starting positions: pawns on rows 1 and 6, major pieces on rows 0 and 7

**Piece Hierarchy:**
- **Piece** (abstract base class) — Defines shared properties (position, color, moved flag, value) and protected movement helper methods
  - `nextPositions(BoardState)` — Abstract method; returns list of legal moves for this piece
  - `horizontalmoves()` — Scans left/right for Rook-like pieces; stops at own piece or captures opponent's
  - `verticalmoves()` — Scans up/down for Rook-like pieces
  - `diagonalmoves()` — Scans all diagonals for Bishop-like pieces (can be inferred from hierarchy)
  
**Piece Implementations:**
- **Pawn** — Moves forward 1 square (2 on first move); captures diagonally forward only
- **Rook** — Moves horizontally and vertically any distance until blocked
- **Bishop** — Moves diagonally any distance until blocked
- **Knight** — Moves in an L-shape (2+1 squares); ignores board obstacles
- **Queen** — Combines Rook and Bishop movement (horizontal, vertical, diagonal)
- **King** — Moves one square in any direction

**Move Validation:**
- Each piece calculates its legal moves by calling `nextPositions(BoardState)`
- Moves must be within board bounds and cannot move to squares occupied by friendly pieces
- Captures are allowed: moving to a square with an opponent's piece removes it
- Piece tracking: `moved` flag used to distinguish first pawn moves and potential castling logic (if implemented)

**Turn Control:**
- `ChessUI.turn` boolean tracks whose turn it is (true = white, false = black)
- Buttons disabled for non-active player; pieces cannot be moved out of turn
- After move execution, turn flips and board state updates

**Special Considerations:**
- Check detection: Not explicitly shown in provided code excerpts; to be verified in full implementation
- Pawn movement uses `addY(-1)` for white (moving "up") and `addY(1)` for black (moving "down")
- Move serialization for networking: Piece value codes (1=white pawn, 2=black pawn, etc.) identify piece type during transmission

## Object-Oriented Design

The project demonstrates clean object-oriented principles through its component separation:

**Inheritance & Polymorphism:**
- **Piece** abstract base class defines the contract for all chess pieces
- Each piece type (Pawn, Rook, Knight, Bishop, Queen, King) inherits from Piece and overrides `nextPositions()` with piece-specific logic
- Protected helper methods (`horizontalmoves()`, `verticalmoves()`, `diagonalmoves()`) in Piece are reused by multiple piece types, reducing code duplication
- Unicode symbols (e.g., ♟ for pawn, ♜ for rook) are piece-specific; each piece overrides `unicode()` method

**Encapsulation:**
- **BoardState** encapsulates the board's internal state; all piece access goes through `getPieceAt(Position)`
- **ChessClient** hides network communication details; external code calls high-level methods like `SendMove()` without understanding UDP serialization
- **Position** struct provides controlled access to x/y coordinates via getX(), getY()

**Separation of Responsibilities:**
- **Game Logic** — Piece classes define movement rules; BoardState tracks positions
- **UI** — ChessUI renders the board, handles clicks, updates display based on BoardState
- **Networking** — ChessClient handles UDP sockets, serialization, and server communication
- **Session Management** — ChessMenu coordinates game mode selection; Game/GameServer manage networked sessions

**Architecture Flow:**
```
Windows Forms UI (ChessUI/ChessMenu)
         ↓
Chess Logic (Piece classes / BoardState)
         ↕
Networking Layer (ChessClient)
         ↕
GameServer & Game Sessions
```

When a player makes a move in ChessUI:
1. ChessUI.Square_Click() → calls `piece.nextPositions(boardState)` to validate
2. If valid, updates BoardState locally
3. ChessUI calls `chessClient.SendMove(boardState)` → serializes board state → transmits via UDP
4. Remote player's ChessClient receives message → reconstructs BoardState → ChessUI renders update

## Windows Forms Interface

The graphical interface is built using **Windows Forms** with an event-driven programming model:

**Key Classes:**
- **ChessUI** — The main game board window (inherits from Form)
  - Creates an 8×8 grid of Button controls representing board squares (150×150 pixels each)
  - Each button stores a Position tag and responds to Click events
  - Displays pieces using Unicode symbols; recolors squares to show valid moves
  - Methods: BuildBoard(), EnableButtons(), drawBoardState(), flipBoard(), Square_Click()

- **ChessMenu** — The startup menu (inherits from Form)
  - Presents options: "Pass and Play" (local) or "Online" (networked)
  - Uses Timers to periodically check server connectivity and incoming game session announcements
  - Shows available game sessions as clickable buttons
  - Transitions to ChessUI when game mode is selected

**Event-Driven Features:**
- **Button.Click** — Triggered when a board square is clicked; calls Square_Click() to select pieces or move
- **Timer.Tick** — Periodically checks for server availability and game session updates
- **Form.Load** — Initializes the board and UI state on startup
- **Piece Selection Visual Feedback** — Selected piece's square highlights; valid destinations highlight in different color

**Interactive Flow:**
1. Player clicks a piece → ChessUI calculates valid moves → highlights destination squares
2. Player clicks destination → BoardState updates → board redraws with new piece positions
3. In networked mode, move serialized and sent; opponent's board updates when message received

## Main Classes

| Class | Responsibility |
|-------|-----------------|
| **ChessUI** | Windows Forms game window; renders 8×8 board; handles user input; updates display after moves |
| **ChessMenu** | Startup menu; game mode selection (local/online); displays available game sessions |
| **ChessClient** | UDP networking; server discovery; game creation/joining; move transmission and receipt |
| **BoardState** | Manages board state; stores piece positions in Dictionary; initializes pieces; provides piece lookup |
| **Position** | Immutable coordinate struct; represents a square on the board (x: 0-7, y: 0-7) |
| **Piece** | Abstract base class for all chess pieces; defines movement interface and shared helper methods |
| **Pawn** | Pawn-specific movement (forward 1-2 squares, diagonal captures) |
| **Rook** | Rook-specific movement (horizontal and vertical lines) |
| **Knight** | Knight-specific movement (L-shaped jumps; bypasses obstacles) |
| **Bishop** | Bishop-specific movement (diagonal lines) |
| **Queen** | Queen-specific movement (combines rook and bishop) |
| **King** | King-specific movement (one square in any direction) |
| **GameServer** | UDP server; listens for client connections; manages active game sessions |
| **Game** | Represents a single game session; tracks player1 and player2 network endpoints |

## Project Structure

```
Networked-ChessGame/
├── board/                          # Chess game logic and UI
│   ├── Program.cs                  # Application entry point
│   ├── ChessUI.cs                  # Windows Forms board interface
│   ├── ChessUI.Designer.cs         # UI layout (auto-generated)
│   ├── ChessMenu.cs                # Menu and session selection
│   ├── ChessMenu.Designer.cs       # Menu layout (auto-generated)
│   ├── ChessClient.cs              # UDP networking client
│   ├── BoardState.cs               # Board state management
│   ├── Position.cs                 # Board coordinate struct
│   ├── Piece.cs                    # Abstract base piece class
│   ├── Pawn.cs                     # Pawn-specific movement
│   ├── Rook.cs                     # Rook-specific movement
│   ├── Knight.cs                   # Knight-specific movement
│   ├── Bishop.cs                   # Bishop-specific movement
│   ├── Queen.cs                    # Queen-specific movement
│   ├── King.cs                     # King-specific movement
│   └── ChessGame.csproj            # Project configuration (net6.0-windows)
├── networking/                     # Server and networking
│   ├── GameServer.cs               # UDP server listening on port 55555
│   ├── Game.cs                     # Game session representation
│   └── networking.csproj           # Project configuration (net6.0)
└── board.sln                       # Visual Studio solution file
```

## Getting Started

### Prerequisites
- **.NET 6.0 SDK** or later
- **Visual Studio 2022** (or VS Code with C# extension) — optional but recommended

### Installation & Running

**1. Clone the repository:**
```bash
git clone https://github.com/joelmccormack/Networked-ChessGame.git
cd Networked-ChessGame
```

**2. Build the solution:**
```bash
dotnet build
```

**3. Run the game (local mode requires no setup):**
```bash
dotnet run --project board
```

**4. For networked multiplayer, start the server first (in a separate terminal):**
```bash
dotnet run --project networking
```
The server will output: `Listening on port: 55555`

### Running a Multiplayer Game

1. **On Host Machine:**
   - Terminal 1: Start the server
     ```bash
     dotnet run --project networking
     ```
   - Terminal 2: Start the game client
     ```bash
     dotnet run --project board
     ```
   - In the game menu, click "Online"
   - Click "New Game" and enter a game name

2. **On Remote Machine (on same network):**
   - Start the game client:
     ```bash
     dotnet run --project board
     ```
   - In the game menu, click "Online"
   - Available game sessions appear as buttons; click to join

3. **Game begins** — White (host) moves first; turns alternate

**Notes:**
- Both machines must be on the same network for UDP discovery to work
- Server listens on port 55555; ensure no firewall blocks it
- Game discovery is automatic; no need to manually enter IP addresses

## Technical Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Windows Forms UI                          │
│  (ChessUI / ChessMenu with event-driven Button controls)    │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ↓
┌─────────────────────────────────────────────────────────────┐
│              Chess Game Logic & State                        │
│  (Piece classes with movement rules / BoardState)           │
└────────────────┬──────────────────────────────┬─────────────┘
                 │                              │
    (Updates)    ↓                              ↓ (Queries)
                 │                              │
┌────────────────────────────────────────────────────────────┐
│                   Networking Layer                          │
│     (ChessClient / UDP sockets / Game serialization)        │
└────────────┬────────────────────────────────────────────────┘
             │
     (Over Network)
             ↓
┌────────────────────────────────────────────────────────────┐
│                  Remote Chess Client                        │
│     (Receives board state / Updates UI)                     │
└────────────────────────────────────────────────────────────┘
```

The separation of networking, game logic, and UI improves maintainability: each component can be tested and modified independently. Game logic is decoupled from networking; the same BoardState and Piece classes work identically in local or networked modes.

## Testing

No automated unit tests are included in the repository. Testing is performed manually through gameplay in both local and networked modes.

## Technical Skills Demonstrated

- **C# & .NET** — Built a desktop application using modern C# language features and the .NET 6.0 framework
- **Windows Forms** — Created an interactive GUI using event-driven programming; managed 64 button controls and dynamic visual updates
- **Object-Oriented Design** — Implemented inheritance hierarchies (Piece base class with 6 concrete pieces); used polymorphism for move calculation
- **Networking** — Implemented UDP client-server communication; game-session discovery via broadcast; serialized complex game state for transmission
- **Game Logic** — Developed piece-specific movement validation; board state management; turn alternation
- **State Management** — Synchronized board state between networked players; maintained consistency across distributed game sessions
- **Software Architecture** — Separated concerns (UI, logic, networking) for clean, maintainable code
- **Problem Solving** — Managed real-time multiplayer challenges including latency, state synchronization, and session discovery
