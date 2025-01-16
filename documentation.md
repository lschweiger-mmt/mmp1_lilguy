 
# Documentation

![Scripts](<media/scripts graph.png>)

## Script Hierarchy

### Main Scripts

- `Program.cs` instantiates the Game.
- `Game.cs` manages Managers, Scenes, and includes some functions that are used universally (Wait, View, Window, ...).

### Managers

- `Settings.cs` manages settings like window size, volume, username, and saves them in `lilguy.settings`.
- `AssetManager.cs` manages assets.
- `InputManager.cs` manages user input.
- `MusicManager.cs` manages music.
- `ScoreManager.cs` manages scoring.
- `SoundManager.cs` manages sound effects.
- `TimeManager.cs` manages time manipulation (slow motion).
- `ParticleManager.cs` manages particles that are not assigned to any object.

### Utilities

- `Utilities.cs` contains helpful methods, such as vector length or random string generation.
- `DrawStuff.cs` offloads the creation of common shapes into methods.
- `Waiter.cs` is used to call actions with a delay (e.g., a one-second delay before the scene loads).

### Shapes

- `RoundedRectangleShape.cs` inherits from `CircleShape`.

### Database

- `PostgresAccess.cs` makes calls to the leaderboard database and the share integration for Luca Troger's social media web project.
- `Config.cs` contains database credentials (gitignore).
- `ConfigSample.txt` here you can write your credentials.

### Scenes

- `Scene.cs` contains the game objects and UI of the scene.
- `SceneCredits.cs`, `SceneDeath.cs`, `SceneGame.cs`, `SceneLeaderboard.cs`, `SceneMenu.cs` are children of `Scene.cs` and contain the logic for their respective scenes.

### GameObjects

- `GameObject.cs` is a transformable and drawable object that can carry components that give it functionality.
- `SpriteObject.cs`, `SpritesheetObject.cs` are children of `GameObject.cs`, which have built-in functionality for sprites/spritesheets. The same can theoretically be achieved with components on a regular game object, but since it's used so often, it became a separate class.
- `Enemy.cs` contains logic for a simple enemy.
- `EnemySpawner.cs` spawns enemies.
- `Player.cs` contains logic for the player. It has components like `RigidBody` or `Top-Down-Movement`.
- `Sword.cs` contains logic for the sword.
- `SetupSwordAnimations.cs` contains all the values for the sword's animation.

#### UI

- `UIElement.cs` is the base class for UI elements. They can be positioned relatively or absolutely like in CSS, transform into child hierarchies, and animate smoothly.
- `UIButton.cs`, `UIInputField.cs`, `UIPanel.cs`, `UIProgressBar.cs`, `UISprite.cs`, `UIText.cs`, `UIUnderline.cs` are children of `UIElement.cs`.

### HUD

- `Hud.cs` is the parent for all HUDs. It has methods for creating common UI elements (buttons, text).
- `Credits`, `Death`, `Game`, `Leaderboard`, `Menu` are the HUDs for the respective scenes and contain the UI elements.

### Components

- `Collider.cs` handles collisions. There are `CircleCollider` and `BoxCollider`. Collisions have been optimized using Sweep-and-Prune.
- `Movement.cs` handles the movement of enemies and the player. It is physics-based, so with acceleration.
- `TopDownMovement.cs` handles player movement with input.
- `RigidBodyCircle.cs` ensures that enemies and the player cannot move through each other.
- `SpriteRenderer.cs` handles the rendering of sprites.
- `SpriteSheet.cs` handles the rendering of spritesheet animations.
- `Health.cs`, `HealthEnemy.cs`, `HealthPlayer.cs` handle the health points of enemies and players and trigger damage and death events.
- `Tween.cs` handles movement (position, rotation, scale).
- `Animation.cs` handles multiple tweens as an animation.
- `Animator.cs` handles multiple animations and plays them.

### Shaders

- `background.hlsl` shader for the UI background.
- `circle.hlsl` shader for the circle vignette for scene transitions.
