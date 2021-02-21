# Boss Battle
*A game focusing on AI by Jack Hunt.*  


## Workflow
Two branches: Main, LinterValidation.
As code was developed, it was pushed to the LinterValidation branch. Once there was reasonable progress, and Super Linter gave a pass status, the branch was merged into main.


## The Game
The player faces up against a big boss viking and his minions.

### The Player
* Moves around with WASD.
* Punches with left mouse.
* Launches an AOE attack with right mouse.

### The Boss
* Has a large pool of HP.
* Has a basic punching attack and ranged AOE attack.
* Has explosive minions that will chase after the player if they are too close.

### The Mechanics
* The player has a health and mana pool. Mana is depleted by using the AOE attack.
* Health and mana can be restored over time by standing on the two healing pads.
* Pressing F3 shows debug info, such as showing the agents pathing and their internal states.

## Current State of Project
The project has been finished and I have moved on to another. I copied the repository here to be public for use in my portfolio.

## Screen Shots

![A typical view of the scene.](https://github.com/JHuntsGHub/Boss-Battle/blob/main/.ScreenShots/BossBattle_1_Big.png)
A typical view of the scene.

![A typical view of the scene.](https://github.com/JHuntsGHub/Boss-Battle/blob/main/.ScreenShots/BossBattle_2_Big.png)
Here we can see the debug info for the agents.
