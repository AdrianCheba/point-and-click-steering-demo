using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

class Character : MonoBehaviour
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
    Animator _characterAnimator;

    float _speed;
    float _maneuverability;
    float _stamina;
    float _currentStamina;
    float _staminaRegenTime;
    bool _isRuning;

    readonly string _mouseClickAction = "MouseClick";
    readonly string _animationParameterName = "IsWalking";

    void OnEnable()
    {
        _actionAsset.Enable();
        _charactersConfig.LiderID = 0;
        _charactersConfig.LiderTransform = null;
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _charactersConfig.MainCamera = Camera.main;
        _isRuning = false;
        _staminaRegenTime = _charactersConfig.StaminaRegenTime;
        _characterAnimator = GetComponent<Animator>();

        GenerateTraits();

        _currentStamina = _stamina;
    }

    void Update()
    {
        MoveTo();
        StaminaManager();
    }

    void MoveTo()
    {
        _actionAsset.FindAction(_mouseClickAction).performed += _ =>
        {
            if (_charactersConfig.LiderID == 0)
            {
                Debug.Log("Select Character");
                return;
            }

            Ray ray = _charactersConfig.MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) && hit.collider)
            {
                _charactersConfig.DestinationPoint = hit.point;
                _isRuning = true;
                _navMeshAgent.speed = _speed;

                if (_characterID == _charactersConfig.LiderID)
                {
                    _charactersConfig.LiderTransform = transform;
                }
            }
        };

        if (_characterID == _charactersConfig.LiderID && _charactersConfig.LiderTransform != null)
        {
            _characterAnimator.SetBool(_animationParameterName, true);
            _navMeshAgent.destination = _charactersConfig.DestinationPoint;
            _charactersConfig.LiderTransform = transform;
            _navMeshAgent.stoppingDistance = _charactersConfig.LeaderStoppingDistance;

        }
        else if (_characterID != _charactersConfig.LiderID && _charactersConfig.LiderTransform != null)
        {
            _characterAnimator.SetBool(_animationParameterName, true);
            _navMeshAgent.destination = _charactersConfig.LiderTransform.position;
            _navMeshAgent.stoppingDistance = _charactersConfig.CharacterStoppingDistance;
        }
    }

    void GenerateTraits() 
    { 
        _speed = Random.Range(_generatorConfig.MinSpeed, _generatorConfig.MaxSpeed);
        _maneuverability = Random.Range(_generatorConfig.MinManeuverability, _generatorConfig.MaxManeuverability);
        _stamina = Random.Range(_generatorConfig.MinStamina, _generatorConfig.MaxStamina);

        _navMeshAgent.speed = _speed;
        _navMeshAgent.angularSpeed = _maneuverability;
    }

    void StaminaManager()
    {
        if (_currentStamina >= 0 && _isRuning)
        {
            _currentStamina -= Time.deltaTime;
        }
        else if(_currentStamina <= 0)
        {
            if (_staminaRegenTime >= 0)
            {
                _staminaRegenTime -= Time.deltaTime;
                _navMeshAgent.speed = 0;
                _characterAnimator.SetBool(_animationParameterName, false);
            }
            else
            {
                _currentStamina = _stamina;
                _navMeshAgent.speed = _speed;
                _staminaRegenTime = _charactersConfig.StaminaRegenTime;
                _characterAnimator.SetBool(_animationParameterName, true);
            }

        }
        
        if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _isRuning)
        {
            _isRuning = false;
            _currentStamina = _stamina;
            _navMeshAgent.speed = _speed;
            _characterAnimator.SetBool(_animationParameterName, false); 

        }
    }

    void OnDisable()
    {
        _actionAsset.Disable();
    }
}