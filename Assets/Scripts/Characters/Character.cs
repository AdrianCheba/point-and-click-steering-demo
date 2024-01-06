using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [SerializeField]
    CharacterConfig _charactersConfig;

    [SerializeField]
    int _characterID;

    NavMeshAgent _navMeshAgent;

    [SerializeField]
    InputActionAsset _actionAsset;

    void OnEnable()
    {
        _actionAsset.Enable();
        _charactersConfig.LiderID = 0;
        _charactersConfig.LiderTransform = null;
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _charactersConfig.MainCamera = Camera.main;
    }

    void Update()
    {
        MoveTo();
    }

    void MoveTo()
    {
        _actionAsset.FindAction("MouseClick").performed += _ =>
        {
            if (_charactersConfig.LiderID == 0)
            {
                Debug.Log("Select Character");
                return;
            }

            Ray ray = _charactersConfig.MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) && hit.collider)
            {
                if (_characterID == _charactersConfig.LiderID)
                {
                    _charactersConfig.LiderTransform = transform;
                    _navMeshAgent.destination = hit.point;

                }
            }
        };

        if (_characterID != _charactersConfig.LiderID && _charactersConfig.LiderTransform != null)
        {
            _navMeshAgent.destination = _charactersConfig.LiderTransform.position;
        }
    }
}