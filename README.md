# Unity Game Jam Quest


## Game Overview
**Game Designer:** Connie Xu\
**Game Title:** Hoop Madness\
**GitLab Repository Link:** https://coursework.cs.duke.edu/computer-game-design-spring-2025/connie-xu/unitygamejam

### Elevator Pitch
Hoop Madness is a single-player basketball shooting game where each level introduces basketballs with unique power-ups that are activated by scoring. Players accumulate points over a given time frame and clear levels by meeting the target score.

### Theme Interpretation
The game aligns with the theme of "Strange Power-ups" by introducing a variety of unconventional basketballs in each level, each with its own effect. These power-ups include modifying the player's size or speed, inverting movement and controls, making the hoop temporarily invisible, and even a (slightly silly/cursed) bonus level (made for friends' entertainment, you don't have/need to play it) where the player’s appearance transforms. These mechanics playfully poke at the idea of "power-ups" in a basketball setting.

Each basketball is visually distinct because its color hints at the type of power it holds. While some power-ups may help players score more easily, others try to create fun or unexpected challenges—adding variety and unpredictability to the gameplay. A dedicated reference page lists all the special basketballs with subtle hints about their effects.

### Controls
List the controls for your game (e.g., arrow keys for movement, Z for action).

- Right/Left Arrow Keys: Move right/left.
- Up Arrow Key: Jump
- Q Key: Dribble the ball if it is within range
- T Key: Holding the key picks up and aims the basketball if it is within range. Releasing the key throws the basketball in the direction the player is aiming at.

## MDA Framework

### Aesthetics
The primary aesthetics of my game are Challenge and Discovery. 

The game consists of short, time-limited levels where the player must score a specific number of points—or, in the bonus level, score once with each unique basketball—within a strict time limit. This structure creates high-pressure scenarios that emphasize urgency and precision. The jumbotron serves as a real-time guide by displaying the remaining time, current score, and level-specific win condition to help players stay on track. Sound design reinforces this aesthetic: cheers reward successful shots, the jumbotron score updates to reflect progress, and a buzzer marks the end of each round with dramatic tension. Upon winning, celebratory marching band music plays, to give players a sense of achievement. The game begins with Level 0, a tutorial round featuring only normal basketballs, and each subsequent level must be unlocked by beating the previous one. This progression system challenges players to improve and unlock all levels through mastery of new powerups and mechanics.

Even though players are provided with information about controls and powerups, they must experiment to truly understand how to use them effectively. For instance, players discover that rapidly tapping the dribble key allows them to move while keeping control of the ball, and through trial and error, they learn the best throwing angles from different positions on the court. There’s also a hidden mechanic where scoring can count as a 2- or 3-pointer depending on the player's distance from the hoop. Each level revolves around powerups of a certain theme (such as speed, size, or inversion) and introduces new powerup balls that change gameplay in unexpected ways. While a reference page hints at what each ball does, the balls are visually distinct and intentionally not explained in full so that players can learn their effects through direct interaction. This promotes a strong sense of discovery as players begin to recognize each powerup and adapt their strategy accordingly.

### Dynamics
Outline the core interactions and player experiences that emerge from the mechanics. How does the game encourage specific behaviors, strategies, or engagement over time?

The game centers around rounds of dribbling, aiming, and scoring under time pressure. Players must master the basic mechanics (learning the keyboard controls and combining them) in earlier levels to move, control the ball, and shoot the ball effectively because later levels introduce powerups and not only have higher requirements for points but also have shorter times. There is only one basketball available at a time, so the player focuses on that singular basketball and how to get it into the hoop in order to make progress. Themed powerup balls motivate players to experiment and adapt. For example, players could try to take advantage of increased speed from a powerup or learn how to throw the ball in a way that rebounds it off the edges of the screen into the hoop. The scoring mechanic, where distance from the hoop determines whether a shot counts as two or three points, also rewards spatial awareness and experimentation. Over time, players can shift from trial-and-error to strategy after learning how to recognize and play with the effects from specific powerups. The game's objectives (score goals under time pressure) and powerups that spawn randomly in each round encourages replayability for players that wish improve their skills to score as many points as possible after satisfying the required amount. 

### Mechanics
The core mechanics of the game are movement, dribbling, aiming, and shooting, all under time pressure. The player controls a stick figure basketball player using keyboard inputs to move left and right, jump, dribble, pick up and aim the basketball, and throw it in a chosen direction. Aiming is handled through an oscillating visual arc that appears during aim mode, allowing the player to time their shot based on the arc’s angle. The ball can bounce off the backboard, rim, ground, and edges of the screen, and the points earned upon scoring the ball depends on the player's distance (2 pointer vs 3 pointer), which pushes the player to carefully consider positioning and shot angles. Only one basketball is active on the court at any given time, so the player dedicates all their focus to perform actions on it and progress through the level.

There are 6 total levels in the game (including the bonus). Difficulty slowly escalates through shorter time limits and higher score requirements. Each level introduces new themed basketballs with special powerups. Level 1 has powerups for speeding up and slowing down the player, Level 2 has powerups for growing and shrinking the player, Level 3 has powerups for reversing the movement or dribbling/throwing controls, Level 4 has a powerup for making the hoop invisible for a period of time, and Level 5 has powerups that change the player's appearance. These powerups are activated only when the corresponding basketball is scored, which prompts players to change their strategy in the level at a moment's notice.

Together, these mechanics create urgency, encourage experimentation, and keep each level interesting with the same fundamental controls.

## External Resources

### Assets
Audio:
- Crowd Cheering by storegraphic via Pixabay
    - Date: March 17, 2025
    - URL: https://pixabay.com/sound-effects/crowd-cheering-314920/
- Ball bounce by JakeEaton (Freesound) via Pixabay
    - Date: August 24, 2022
    - URL: https://pixabay.com/sound-effects/ball-bounce-94853/
- Marching Band by Humanoide_Media via Pixabay
    - Date: March 24, 2025
    - URL: https://pixabay.com/music/marching-band-marching-band-317665/

Art: All sprites were originally hand-drawn in Procreate and then digitally refined by ChatGPT. 

### Code
No specific external code was directly used, but here are some inspirations:
- The Unity Tutorial For Complete Beginners: watched the Unity tutorial listed on the Class Resources page to understand the Unity editor and the basics for creating a game. 
    - Author: Gamer Maker's Toolkit
    - Date: December 2, 2022
    - URL: https://www.youtube.com/watch?v=XtQMytORBmM
- Unity Manual: read official Unity documentation to learn about functions and answer any questions I had
- ChatGPT: discussed with ChatGPT about how to organize my game/code structure


## Code Documentation
You do not need to modify the Code Documentation section of the readme. This section serves as a reminder to make sure that your in-code documentation is clear and informative. Important sections such as function or files should be accompanied by comments describing their purpose and functionality.  