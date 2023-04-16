using UnityEngine;
using System.Collections;

using Player;
public class KingOfTheHill : Ability
{
    [SerializeField] private KingOfTheHillStats _kingOfTheHillContext;

    
    private GameObject _spawnedZone;

    private GameObject _player;
    private PlayerHealth _playerHealth;
    private PlayerAttack _playerAttack;
    
    private KingOfTheHillZone _kingOfTheHillZoneScript;
    private GameObject _banner;

    private void Awake()
    {
        ConvertContext(_kingOfTheHillContext);
    }

    
    public override void Activate(GameObject go)
    {
        base.Activate(go);
        _player = go;

        _kingOfTheHillContext.Zone.transform.localScale =
            new Vector3(_kingOfTheHillContext.ZoneRadius, 0.1f, _kingOfTheHillContext.ZoneRadius);

       // Vector3 vect = new Vector3(0.5f, _player.transform.position.y , 0.5f);
        _spawnedZone = Instantiate(_kingOfTheHillContext.Zone,
            _player.transform.position /*- vect*/, 
            Quaternion.Euler(0, 0, 0));
        
         _kingOfTheHillContext.Banner.transform.localScale =
            new Vector3(0.05f, 0.05f, 0.05f);
         
         _banner =Instantiate(_kingOfTheHillContext.Banner, 
             _player.transform.position /*- vect*/,
             Quaternion.Euler(0, 0, 0));

        _kingOfTheHillZoneScript = _spawnedZone.GetComponent<KingOfTheHillZone>();
        
        _kingOfTheHillZoneScript.onInsideZone += InsideZone;
        _kingOfTheHillZoneScript.onOutsideZone += OutOfZone;

        _playerAttack = _player.GetComponent<PlayerAttack>();
        _playerHealth = _player.GetComponent<PlayerHealth>();

        StartCoroutine(KOTHRegenerationEffect());
    }

    IEnumerator KOTHRegenerationEffect()
    {
        for (int i = 0; i < _kingOfTheHillContext.TickCount; ++i)
        {
            if (_playerHealth != null)
            {
                _playerHealth.AddHealth(_kingOfTheHillContext.Regeneration);
            }

            yield return new WaitForSeconds( _kingOfTheHillContext.WaitTime);
        }
    }

    void OutOfZone()
    {
        _playerAttack.SetAttackDamage(_playerAttack.GetAttackDamage() - _kingOfTheHillContext.AdditionalDamage);

        _playerAttack = null;
        _playerHealth = null;
    }

    void InsideZone()
    {
        _playerAttack = _player.GetComponent<PlayerAttack>();
        _playerHealth = _player.GetComponent<PlayerHealth>();
        
        _playerAttack.SetAttackDamage(_playerAttack.GetAttackDamage() + _kingOfTheHillContext.AdditionalDamage);
    }
    
    public override void Cooldown()
    {
        Destroy(_spawnedZone);
        Destroy(_banner);
    }
}