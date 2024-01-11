using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Config/CharacterConfig")]
class CharacterConfig : ScriptableObject
{
    [SerializeField]
    internal int LeaderID;

    [SerializeField]
    internal Camera MainCamera;
    
    [SerializeField]
    internal bool IsLeaderAtDestination;

    [SerializeField]
    internal Transform LeaderTransform;

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
