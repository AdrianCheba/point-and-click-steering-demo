using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetLeaderUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _liderIDText;

    [SerializeField]
    CharacterConfig _characterConfig;

    public void SetLeader()
    {
        Int32.TryParse(_liderIDText.text, out _characterConfig.LiderID);
    }
}
