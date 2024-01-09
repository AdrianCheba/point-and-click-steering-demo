using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Config/CharacterConfig")]
class CharacterConfig : ScriptableObject
{
    [SerializeField]
    internal int LiderID;

    [SerializeField]
    internal Camera MainCamera;

    [SerializeField]
    internal Transform LiderTransform;

    [SerializeField]
    [Range(0.1f, 5f)]
    internal float StaminaRegenTime;

    [SerializeField]
    [Range(0.1f, 3f)]
    internal float LeaderStoppingDistance; 
    
    [SerializeField]
    [Range(0.1f, 3f)]
    internal float CharacterStoppingDistance;

    internal Vector3 DestinationPoint;
}
