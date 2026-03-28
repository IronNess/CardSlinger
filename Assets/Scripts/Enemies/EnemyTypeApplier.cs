using UnityEngine;

public class EnemyTypeApplier : MonoBehaviour
{
    public EnemyType enemyType;

    void Start()
    {
        moveToPlayer mover = GetComponent<moveToPlayer>();
        if (mover != null && enemyType != null)
        {
            mover.ApplyStats(enemyType);
        }
    }
}