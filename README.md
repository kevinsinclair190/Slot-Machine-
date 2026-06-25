# Slot Machine Game

A fully playable 2D Slot Machine game built in Unity for WebGL, developed as part of a game development internship assignment at Rural Games.

---

##  Play Now
👉 [Play WebGL Build](https://kevinsinclair190.github.io/Slot-Machine-/)

---

## 📌 Project Overview
This project is a complete slot machine game built using Unity 2D and deployed as a WebGL build. It features a fully functional RNG system, payout logic, balance management, sound effects, and a polished UI.

---

##  Gameplay Features
- 3 independent reels with 4 unique symbols
- Lever pull to spin the reels
- Player balance system starting at $5,000
- Bet selection: $100 / $500 / $1,000
- Win detection for 2 matching and 3 matching symbols
- Jackpot for 3x Seven (777)
- Result popup showing win amount or try again
- Payout table displayed on screen at all times
- Sound effects for lever pull, win, lose, and jackpot

---

##  Win Conditions

### Two Matching Symbols
| Match | Multiplier |
|-------|-----------|
| 2x Cherry | x1 |
| 2x Bell | x1.5 |
| 2x Diamond | x2 |
| 2x Seven | x3 |

### Three Matching Symbols
| Match | Multiplier |
|-------|-----------|
| 3x Cherry | x5 |
| 3x Bell | x10 |
| 3x Diamond | x25 |
| 3x Seven JACKPOT | x50 |

---

## RNG System
The slot machine uses a pre-determined outcome system for fair and realistic results. Outcomes are decided before the reel animation plays, mirroring how real slot machines operate.

Outcome probability:
- Loss: 50%
- Two match: 25%
- 3x Cherry: 12%
- 3x Bell: 8%
- 3x Diamond: 3%
- 3x Seven Jackpot: 2%

---

## Sound Effects
- Lever pull sound
- Win sound
- Lose sound
- Jackpot sound

---

##  How To Run Project
1. Clone this repository
2. Open Unity Hub
3. Click Add Project and select the cloned folder
4. Open with Unity 6
5. Open Assets/Scenes/SampleScene
6. Press Play in the Unity Editor

---

## How To Run WebGL Build

### Option 1 - Play Online
Visit: https://kevinsinclair190.github.io/Slot-Machine-/

### Option 2 - Run Locally
1. Clone the repository
2. Navigate to the WebGL-Build folder
3. Run a local server using VS Code Live Server
4. Open index.html in your browser

---

## Architecture Overview

| Script | Responsibility |
|--------|---------------|
| SlotMachine.cs | Main game controller, spin flow, win/loss logic, balance |
| Reel.cs | Individual reel animation and symbol display |
| BetManager.cs | Bet selection and cycling logic |
| SymbolData.cs | ScriptableObject defining each symbol properties |

---

## 📁 Folder Structure
Assets/
├── Scripts/
├── Prefabs/
├── Animations/
├── UI/
├── Sounds/
├── Materials/
├── Sprites/
├── ScriptableObjects/
└── Resources/

---

## Development Approach
- UI layout built first for clear visual structure
- RNG outcome pre-determined before animation for fairness
- Modular scripts with single responsibilities
- WebGL optimized canvas scaling at 1920x1080
- Weighted probability system to balance gameplay

---

## Developer
Kevin Sinclair
Game Development Intern — Rural Games
Unity | C# | WebGL
