# Networked Chess Game

A multiplayer chess game built with C# and .NET, featuring real-time networking capabilities and a Windows Forms UI.

## Features

- **Multiplayer Support** — Play chess against other players over a network via UDP
- **Local Pass-and-Play** — Play against a friend on the same machine
- **Game-Session Discovery** — Automatically discover and join active game sessions
- **Move Validation** — Strict piece movement rules and checkmate detection
- **Synchronized Board State** — Real-time synchronization across all connected clients
- **Windows Forms UI** — Clean desktop application with intuitive board interface

## Tech Stack

- **Language:** C#
- **Framework:** .NET 6.0
- **UI:** Windows Forms
- **Networking:** UDP-based client-server architecture
- **Build System:** Visual Studio / MSBuild

## Project Structure

```
Networked-ChessGame/
├── board/                    # Chess game UI and game logic
│   ├── ChessGame.cs         # Main game controller
│   ├── ChessUI.cs           # Windows Forms interface
│   ├── ChessMenu.cs         # Menu and session management
│   ├── BoardState.cs        # Game state management
│   ├── Position.cs          # Board position tracking
│   └── [Piece Classes]      # Individual piece logic
│       ├── Pawn.cs
│       ├── Rook.cs
│       ├── Knight.cs
│       ├── Bishop.cs
│       ├── Queen.cs
│       └── King.cs
├── networking/              # Server and networking logic
│   ├── GameServer.cs        # UDP multiplayer server
│   └── Game.cs              # Game session management
└── board.sln               # Visual Studio solution
```

## Getting Started

### Prerequisites
- .NET 6.0 SDK or later
- Visual Studio 2022 (or VS Code with C# extension)

### Installation & Running

1. Clone this repository
   ```bash
   git clone https://github.com/joelmccormack/Networked-ChessGame.git
   cd Networked-ChessGame
   ```

2. Open `board.sln` in Visual Studio

3. Build the solution
   ```bash
   dotnet build
   ```

4. Run the game
   ```bash
   dotnet run --project board/ChessGame.csproj
   ```

5. For multiplayer, start the server in a separate terminal
   ```bash
   dotnet run --project networking/
   ```

## How to Play

- **Start Local Game** — Play against a friend on the same computer
- **Host Multiplayer Game** — Start a server and share your IP
- **Join Multiplayer Game** — Connect to a friend's game session
- Click pieces to select and move them on the board
- Follow standard chess rules

## What I Learned

This project demonstrates:
- **Game Logic** — Implementing complex piece movement validation and game rules
- **Networking** — UDP-based client-server architecture for real-time multiplayer
- **Windows Forms Development** — Building desktop UI with event-driven programming
- **Object-Oriented Design** — Inheritance patterns (base Piece class with specialized implementations)
- **State Management** — Synchronizing game state across multiple clients

## License

[Add your preferred license here]
