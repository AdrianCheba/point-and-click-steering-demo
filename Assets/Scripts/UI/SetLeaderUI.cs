using System;
using TMPro;
using UnityEngine;

class SetLeaderUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _leaderIDText;   
    
    [SerializeField]
    TextMeshProUGUI _informationPanelText;

    [SerializeField]
    CharacterConfig _characterConfig;

    public void SetLeader()
    {
        Int32.TryParse(_leaderIDText.text, out _characterConfig.LeaderID);
        _informationPanelText.text = "Current Leader : " + _characterConfig.LeaderID;
    }
}
