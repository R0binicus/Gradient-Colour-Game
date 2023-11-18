using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // GameObject References
    private Player _player;

    // Internal Data
    List<Enemy> _enemies; // List of Enemies in the scene

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _enemies = new List<Enemy>();
        EventManager.EventInitialise(EventType.LOSE);
    }

    private void OnEnable()
    {
        EventManager.EventSubscribe(EventType.ADD_ENEMY, AddEnemy);
    }

    private void OnDisable()
    {
        EventManager.EventUnsubscribe(EventType.ADD_ENEMY, AddEnemy);
    }

    // Receives Enemies that are instantiated within the level to keep track of for win condition
    private void AddEnemy(object data)
    {
        // Make sure enemies are being passed in as data
        Enemy enemy = data as Enemy;
        if (enemy == null) return;

        // Assign EnemyManager and Player to enemy then add to list of enemies
        enemy.EnemyManager = this;
        enemy.Player = _player;
        _enemies.Add(enemy);
    }

    public void PlayerCaught()
    {
        EventManager.EventTrigger(EventType.LOSE, null);
    }

    // Deactivate Enemy that was attacked then check to see how many enemies are left
    public void PlayerAttacked(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        CheckEnemiesLeft();
    }

    // Checks to see how many enemies are left
    private void CheckEnemiesLeft()
    {
        foreach (Enemy enemy in _enemies)
        {
            // If an enemy is still active, do not end game
            if (enemy.gameObject.activeInHierarchy == true)
            {
                return;
            }
        }

        // Signal game won
        EventManager.EventTrigger(EventType.WIN, null);
    }
}