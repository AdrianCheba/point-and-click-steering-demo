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
    
    internal Vector3 DestinationPoint;
}
