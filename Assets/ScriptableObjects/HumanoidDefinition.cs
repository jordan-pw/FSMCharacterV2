using UnityEngine;
using Entity;

[CreateAssetMenu(fileName = "New HumanoidData", menuName = "Humanoid Data", order = 51)]
public class HumanoidDefinition : EntityDefinition
{
    public Stat baseStamina;
    public Stat staminaRechargeRate;
    public Stat staminaRechargeDelay;

    public Stat crouchSpeed;
    public Stat sprintSpeed;

    public Stat maxAirAcceleration;
    public Stat jumpHeight;

    public Stat dashSpeed;
    public FloatStat dashTime;
    public Stat dashAcceleration;
    public Stat dashCost;
}
