using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorConfig", menuName = "Config/GeneratorConfig")]
class GeneratorConfig : ScriptableObject
{
    [SerializeField]
    internal float MinSpeed;

    [SerializeField]
    internal float MaxSpeed;

    [SerializeField]
    internal float MinManeuverability;

    [SerializeField]
    internal float MaxManeuverability;

    [SerializeField]
    internal float MinStamina;

    [SerializeField]
    internal float MaxStamina;
}
