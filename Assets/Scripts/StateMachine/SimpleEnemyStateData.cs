using UnityEngine;

public class SimpleEnemyStateData : IBaseStateData {
    public EnemyBase enemyBaseRef;
    public PlayerManager playerManager;

    public SimpleEnemyStateData(EnemyBase enemyBase, PlayerManager playerMan) {
        enemyBaseRef = enemyBase;
        playerManager = playerMan;
    }
}