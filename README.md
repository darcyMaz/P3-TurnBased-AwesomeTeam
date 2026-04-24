## TEAM: 
Darcy Mazloum (student ID # 2545876), Felis MacLellan (student ID #2245053)
## GAME DESCRIPTION: 
This is a simple turn based game where the player battles slew of enemies, one at a time.
## PLAYER ACTIONS: 
The player has four move options. Attack, Shield, Heal, and Psychic. See the intro of the game for an explanation of the move system.
## AI LOGIC: 
The AI logic is very simple. If its health is full, it attacks. If it has more than half its health left, then it attacks half the time, shield a quarter of the time, or heals a quarter of the time. If it has less than half its health left, it attacks a quarter of the time, shield half the time, and heals a quarter of the time.
## Turn States:
There are 6 turn states. Importantly, the PlayerTurn and EnemyTurn swap back and forth as they attack each other and complete their moves. The game loads into the StartUp state and moves into the PlayerTurn state when the player has pressed next for the final time on the Startup Menu. If an enemy dies, the usual flow is interupted and the PlayerTurn state is entered after a new enemy spawns in. The player always starts against a new enemy. If the player dies or the enemies are defeated, then the game enters either the PlayerDeath state or LevelCompleted states.
The PlayerTurn and EnemyTurn states swap only after a timer has elapsed for the animations to be shown uninterupted.
## KNOWN ISSUES/LIMITATIONS: 
The Psychic move is not tested and seems to be a bit off. Other than the game being unbalanced, it seems to work as intended!
