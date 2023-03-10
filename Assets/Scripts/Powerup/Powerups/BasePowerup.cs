using UnityEngine;

public enum PlayerRole
{
    Driver = 0,
    Gunner = 1
}

public abstract class BasePowerup : ScriptableObject
{
    public int MinimumPosition;
    public string PowerupName;
    public PlayerRole TargetTeamMember;

    public abstract void Pickup();
}