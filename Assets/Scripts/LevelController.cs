using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Transform _roomSpawnPoint;

    [SerializeField] private float _safeRoomChance = 0.3f;

    [Range(0.0f, 1.0f)]
    [Tooltip("Chance to spawn shop, forge spawn = 1 - _shopChance")]
    [SerializeField] private float _shopChance = 0.5f;

    #region Room Prefabs

    [SerializeField] private GameObject _startRoom;
    [SerializeField] private List<GameObject> _roomList = new List<GameObject>();
    // [SerializeField] private List<GameObject> _firstRooms = new List<GameObject>();
    // [SerializeField] private List<GameObject> _secondRooms = new List<GameObject>();
    // [SerializeField] private List<GameObject> _thirdRooms = new List<GameObject>();
    // [SerializeField] private List<GameObject> _fourthRooms = new List<GameObject>();
    [SerializeField] private GameObject _bossRoom;
    [SerializeField] private GameObject _shopRoom;
    [SerializeField] private GameObject _forgeRoom;

    #endregion

    private int _currentLevel;

    private int _minLevelWithoutSafe = 1;

    private GameObject _spawnedRoom;

    private Animator _animatior;
    
    private bool _isLastSafeRoom;

    [Inject] private PlayerMovement player;

    private void Awake()
    {
        
        _animatior = GetComponent<Animator>();
        PlayerPrefs.SetInt("CurrentLevel", -1);
        _currentLevel = PlayerPrefs.GetInt("CurrentLevel", -1);

        if (_currentLevel == -1)
        {
            _currentLevel = 0;
        }

        _isLastSafeRoom = false;
        OnFadeComplete();
    }

    void InitRoom()
    {
        if (_currentLevel == 0)
        {
            _spawnedRoom = Instantiate(_startRoom, _roomSpawnPoint);
        }
        else if(_currentLevel <= 4){
            int rnd = Random.Range(0, _roomList.Count);
            _spawnedRoom = Instantiate(_roomList[rnd], _roomSpawnPoint);
            _roomList.RemoveAt(rnd);
        }
        else
        {
            _spawnedRoom = Instantiate(_bossRoom, _roomSpawnPoint);
        }
        //
        // switch (_currentLevel)
        // {
        //     case 0:
        //         _spawnedRoom = Instantiate(_startRoom, _roomSpawnPoint);
        //         break;
        //     case 1:
        //         _spawnedRoom = Instantiate(_firstRooms[Random.Range(0, _firstRooms.Count)], _roomSpawnPoint);
        //         break;
        //     case 2:
        //         _spawnedRoom = Instantiate(_secondRooms[Random.Range(0, _secondRooms.Count)], _roomSpawnPoint);
        //         break;
        //     case 3:
        //         _spawnedRoom = Instantiate(_thirdRooms[Random.Range(0, _thirdRooms.Count)], _roomSpawnPoint);
        //         break;
        //     case 4:
        //         _spawnedRoom = Instantiate(_fourthRooms[Random.Range(0, _fourthRooms.Count)], _roomSpawnPoint);
        //         break;
        //     case 5:
        //         _spawnedRoom = Instantiate(_bossRoom, _roomSpawnPoint);
        //         break;
        // }
    }

    void InitSafeRoom()
    {
        if (_shopChance < Random.Range(0.0f, 1.0f))
        {
            _spawnedRoom = Instantiate(_shopRoom, _roomSpawnPoint);
        }
        else
        {
            _spawnedRoom = Instantiate(_forgeRoom, _roomSpawnPoint);
        }

        --_currentLevel;
    }
    
    void InitNewRoom()
    {
        if(_spawnedRoom)
            Destroy(_spawnedRoom);
        
        if (_currentLevel > _minLevelWithoutSafe)
        {
            if (_safeRoomChance < Random.Range(0.0f, 1.0f) && !_isLastSafeRoom)
            {
                _isLastSafeRoom = true;
                InitSafeRoom();
            }
            else InitRoom();
        }
        else InitRoom();

        _isLastSafeRoom = false;
        
        _spawnedRoom.GetComponent<Room>().OnRoomCompleted += StartLoadNewRoom;
    }
    
    private void ReposPlayer()
    {
        Transform spawnPoint = _spawnedRoom.GetComponent<Room>().GetSpawnPlayerPoint();
        player.gameObject.transform.position = spawnPoint.position;
        player.gameObject.transform.rotation = spawnPoint.rotation;
    }

    //ивент в анимации 
    private void OnFadeComplete()
    {
        InitNewRoom();
        ReposPlayer();
        player.GetComponent<PlayerInput>().enabled = true;
        Debug.Log(player.GetComponent<PlayerInput>().enabled);
        _animatior.SetTrigger("FadeInTrigger");
    }
    private void StartLoadNewRoom() 
    {
        StartCoroutine(LoadNewRoom());
    }

    IEnumerator LoadNewRoom()
    {
        player.GetComponent<PlayerInput>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        LoadRoom();
        yield return new WaitForSeconds(1.0f);
    }

    void LoadRoom()
    {
        _spawnedRoom.GetComponent<Room>().OnRoomCompleted -= StartLoadNewRoom;
        ++_currentLevel;
        PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
        _animatior.SetTrigger("FadeOutTrigger");
        Debug.Log("xer");
        if (_currentLevel <= 5) ;

    }
}
