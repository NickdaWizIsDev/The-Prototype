using UnityEngine;

[CreateAssetMenu(fileName ="PersistentData", menuName = "Persistence")]

public class PersistentData : ScriptableObject
{
    public int level;
    public bool[] levelChecks = new[]{ true, false, false, false, false };

    public int playerMaxMana;
    public int playerMaxHealth;
}
