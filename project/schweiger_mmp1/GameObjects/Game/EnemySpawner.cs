/*
 * Impressum
 * Studiengang: MultiMediaTechnology
 * Institution: Fachhochschule Salzburg (FHS)
 * Zweck: MultiMediaProjekt 1
 * Autor: Leopold Schweiger
 */

public class EnemySpawner : GameObject
{
    private float lastSpawn, spawnInterval;
    Random rand = new Random();

    public float bigSpawnRate = 2, afraidSpawnRate = 1.5f;

    public EnemySpawner(GameObjectHolder parent, float spawnInterval, int startAmount) : base(parent)
    {
        // return; // Debugging

        this.spawnInterval = spawnInterval;

        for (int i = 0; i < startAmount; i++)
        {
            SpawnEnemy(false);
        }
        // (scene as SceneGame)!.enemies.Add(new EnemyBig(scene)); // Debugging spawning special guys at the beginning
        // (scene as SceneGame)!.enemies.Add(new EnemyAfraid(scene)); // Debugging spawning special guys at the beginning
    }

    public override void Update(float deltatime)
    {
        if(InputManager.Instance.GetKeyDown(SFML.Window.Keyboard.Key.Num3)) SpawnEnemy(true, true);
        // return; // Debugging
        if (lastSpawn + spawnInterval < Game.gameTime)
        {
            lastSpawn = Game.gameTime;
            SpawnEnemy();
        }
    }

    public void SpawnEnemy(bool canSpawnSpecial = true, bool dontRespawn = false)
    {
        Enemy? enemyToSpawn = null;
        if (canSpawnSpecial)
        {
            if (rand.NextDouble() < bigSpawnRate / 100f) enemyToSpawn = new EnemyBig(scene);
            else if (rand.NextDouble() < afraidSpawnRate / 100f) enemyToSpawn = new EnemyAfraid(scene);
        }
        if (enemyToSpawn == null) enemyToSpawn = new Enemy(scene, dontRespawn);
        (scene as SceneGame)!.enemies.Add(enemyToSpawn);
    }
}