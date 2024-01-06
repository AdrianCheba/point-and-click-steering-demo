using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Config/CharacterConfig")]
public class CharacterConfig : ScriptableObject
{
    public int LiderID;

    public Camera MainCamera;

    public Transform LiderTransform;
}
