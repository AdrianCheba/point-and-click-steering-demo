using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorConfig", menuName = "Config/GeneratorConfig")]
public class GeneratorConfig : ScriptableObject
{
    public float MinSpeed;
    public float MaxSpeed;
    public float MinManeuverability;
    public float MaxManeuverability;
    public float MinStamina;
    public float MaxStamina;
}
