# Final Year Project-CS3IP16

## Tank Tactics: Top-Down Online Multiplayer Shooter

Welcome to the Unity project repository for *Tank Tactics*, a top-down online multiplayer tank battle game developed for the CS3IP16 dissertation project at the University of Reading.

## Project Overview

*Tank Tactics* is a top-down online multiplayer tank battle game. Developed using Unity, the game leverages modern multiplayer technologies such as Netcode for GameObjects (NGO) and Unity Gaming Services (UGS), allowing seamless multiplayer experiences without the need for complex network configurations like port forwarding or IP address sharing.

**Note:** For UGS and multiplayer functionality I have **disabled** this feature so it does not currently support online matchmaking or multiplayer gameplay.

## Features

- **Real-Time Multiplayer:** Host or join games effortlessly using Unity's Relay & Lobby services.
- **Intuitive Controls:** Simple yet responsive tank movement and shooting mechanics.
- **Dynamic Gameplay Elements:**
  - **Leaderboard:** Tracks top players in real-time.
  - **Mini-Map:** Provides players with situational awareness of the battlefield.
  - **Healing Zones:** Special areas where players can replenish health.
  - **Bounty Coins:** Collect coins dropped by opponents for strategic advantage.

## Technologies Used

- **Unity Game Engine:** Powered by Unity, allowing flexible and powerful game development.
- **Netcode for GameObjects (NGO):** Simplifies multiplayer networking (disabled in this version).
- **Unity Gaming Services (UGS):** Utilizes Relay & Lobby for easy game hosting and matchmaking (disabled in this version).

## Installation and Setup

1. Clone this repository:
    ```bash
    git clone https://github.com/InaJaweed/Individual_Project-CS3IP16
    ```
2. Open the project in Unity.

3. Install necessary packages via the Unity Package Manager, including Netcode for GameObjects and UGS SDK.

4. Customize settings such as player count or map size if needed.

**Important:** Even if you clone the repository and set up the project, the game cannot be played in multiplayer mode as the multiplayer features (including matchmaking, Relay, and Lobby services) are currently **disabled**.

## How to Play

- **Host a Game:**  One player can act as a host to set up a lobby for other players.
- **Join a Game:** Players can join an active lobby and jump straight into the action.
- Collect coins, defeat enemies, and use healing zones strategically to stay ahead on the leaderboard.

## Future Improvements

- Enhanced matchmaking systems.
- Performance optimizations for larger matches.

## Documentation
You can read the full dissertation [here](./Dissertation_CS3IP16.pdf).
