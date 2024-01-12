using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

class Character : MonoBehaviour
{
    [SerializeField]
    int _characterID;

    [SerializeField]
    CharacterConfig _charactersConfig;

    [SerializeField]
    InputActionAsset _actionAsset;

    [SerializeField]
    GeneratorConfig _generatorConfig;

    NavMeshAgent _navMeshAgent;
    Animator _characterAnimator;
    Camera _mainCamera;

    float _speed;
    float _maneuverability;
    float _stamina;
    float _currentStamina;
    float _staminaRegenTime;
    bool _isRunning;

    void OnEnable()
    {
        _actionAsset.Enable();
        _charactersConfig.LeaderID = 0;
        _charactersConfig.LeaderTransform = null;
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _mainCamera = Camera.main;
        _isRunning = false;
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
        _actionAsset.FindAction(_charactersConfig.MouseClickAction).performed += _ =>
        {
            if (_charactersConfig.LeaderID == 0)
            {
                return;
            }

            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) && hit.collider)
            {
                _charactersConfig.DestinationPoint = hit.point;
                _isRunning = true;
                _navMeshAgent.speed = _speed;                

                if (_characterID == _charactersConfig.LeaderID)
                {
                    _charactersConfig.LeaderTransform = transform;
                    _charactersConfig.IsLeaderAtDestination = false;
                }
            }
        };

        if (_characterID == _charactersConfig.LeaderID && _charactersConfig.LeaderTransform != null)
        {
            _characterAnimator.SetBool(_charactersConfig.AnimationParameterName, true);
            _navMeshAgent.destination = _charactersConfig.DestinationPoint;
            _charactersConfig.LeaderTransform = transform;
            _navMeshAgent.stoppingDistance = _charactersConfig.LeaderStoppingDistance;

        }
        else if (_characterID != _charactersConfig.LeaderID && _charactersConfig.LeaderTransform != null)
        {
            _characterAnimator.SetBool(_charactersConfig.AnimationParameterName, true);
            _navMeshAgent.destination = _charactersConfig.LeaderTransform.position;
            _navMeshAgent.stoppingDistance = _charactersConfig.CharacterStoppingDistance;
        }


        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _characterID == _charactersConfig.LeaderID)
        {
            _isRunning = false;
            _currentStamina = _stamina;
            _charactersConfig.IsLeaderAtDestination = true;
            _characterAnimator.SetBool(_charactersConfig.AnimationParameterName, false);
        }

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _characterID != _charactersConfig.LeaderID && _charactersConfig.IsLeaderAtDestination)
        {
            _isRunning = false;
            _currentStamina = _stamina;
            _characterAnimator.SetBool(_charactersConfig.AnimationParameterName, false);
        }
        else if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _characterID != _charactersConfig.LeaderID && !_charactersConfig.IsLeaderAtDestination)
        {
            _characterAnimator.SetBool(_charactersConfig.AnimationParameterName, false);
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
        if (_currentStamina >= 0 && _isRunning)
        {
            _currentStamina -= Time.deltaTime;
        }
        else if(_currentStamina <= 0)
        {
            if (_staminaRegenTime >= 0)
            {
                _staminaRegenTime -= Time.deltaTime;
                _navMeshAgent.speed = 0;
                _characterAnimator.SetBool(_charactersConfig.AnimationParameterName, false);
            }
            else
            {
                _currentStamina = _stamina;
                _navMeshAgent.speed = _speed;
                _staminaRegenTime = _charactersConfig.StaminaRegenTime;
                _characterAnimator.SetBool(_charactersConfig.AnimationParameterName, true);
            }
        }
    }

    void OnDisable()
    {
        _actionAsset.Disable();
    }
}