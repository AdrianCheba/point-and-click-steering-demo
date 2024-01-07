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

    [SerializeField]
    InputActionAsset _actionAsset;

    [SerializeField]
    GeneratorConfig _generatorConfig;

    NavMeshAgent _navMeshAgent;
    RaycastHit _hit;

    float _speed;
    float _maneuverability;
    float _stamina;
    bool _isRuning;

    void OnEnable()
    {
        _actionAsset.Enable();
        _charactersConfig.LiderID = 0;
        _charactersConfig.LiderTransform = null;
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _charactersConfig.MainCamera = Camera.main;
        _isRuning = false;

        GenerateAttributes();
    }

    void Update()
    {
        MoveTo();
        StaminaManager();
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
            if (Physics.Raycast(ray: ray, hitInfo: out _hit) && _hit.collider)
            {
                if (_characterID == _charactersConfig.LiderID)
                {
                    _charactersConfig.LiderTransform = transform;
                    _navMeshAgent.destination = _hit.point;
                    _navMeshAgent.stoppingDistance = 0;
                }

                _isRuning = true;
            }
        };

        if (_characterID != _charactersConfig.LiderID && _charactersConfig.LiderTransform != null)
        {
            _navMeshAgent.destination = _charactersConfig.LiderTransform.position;
            _navMeshAgent.stoppingDistance = 1;
        }
    }

    void GenerateAttributes() 
    { 
        _speed = Random.Range(_generatorConfig.MinSpeed, _generatorConfig.MaxSpeed);
        _maneuverability = Random.Range(_generatorConfig.MinManeuverability, _generatorConfig.MaxManeuverability);
        _stamina = Random.Range(_generatorConfig.MinStamina, _generatorConfig.MaxStamina);

        _navMeshAgent.speed = _speed;
        _navMeshAgent.angularSpeed = _maneuverability;
    }

    void StaminaManager()
    {
        if (_stamina >= 0 && _isRuning)
        {
            _stamina -= Time.deltaTime;
            Debug.Log(_stamina);
        }
    }

    void OnDisable()
    {
        _actionAsset.Disable();
    }
}