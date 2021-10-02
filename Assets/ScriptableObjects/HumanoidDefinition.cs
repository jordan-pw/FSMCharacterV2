using UnityEngine;
using Entity;

[CreateAssetMenu(fileName = "New HumanoidData", menuName = "Humanoid Data", order = 51)]
public class HumanoidDefinition : EntityDefinition
{
    public Stat crouchSpeed;
    public Stat sprintSpeed;

    public Stat maxAirAcceleration;
    public Stat jumpHeight;
}
