using UnityEngine;
using Entity;

[CreateAssetMenu(fileName = "New EntityData", menuName = "Entity Data", order = 51)]
public class EntityDefinition : ScriptableObject
{
    public string entityName;

    public Stat speed;

    public Stat maxAcceleration;

    public Stat baseArmor;
    public Stat baseDamage;
}
