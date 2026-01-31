using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "PowerUps/Database")]
public class PowerUpDatabaseSO : ScriptableObject
{
    public List<PowerUpSO> powerUps;

    public PowerUpSO Get(PowerUpType type)
    {
        return powerUps.Find(p => p.type == type);
    }
}
