using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private AudioSource _walkAudioSource;

    private PathManager _pathManager;
    private Transform[] _patrolPath;
    private Transform[] _playerFollowPath;
    private Rigidbody2D _rigidbody;
    private EnemyVision _vision;
    private SpriteRenderer _sprite;
    private Animator _animator;
    private int _target;
    private int _hp;
    private static bool _knowAboutPlayer;

    public RoomManager CurrentRoom;

    public int HP 
    { 
        get => _hp; 
        set
        {
            if (_hp > value) 
            {
                StartCoroutine(HitVisualization()); 
            }
            _hp = value;
            if (_hp <= 0)
            {
                transform.parent.GetComponent<EnemyManager>().KillEnemy(gameObject);
            }
        } 
    } 


    private void Start()
    {
        HP = 50; 
        _target = 0;
        _knowAboutPlayer = false;
        Physics2D.gravity = Vector2.zero;
        _pathManager = GameObject.Find("PathManager").GetComponent<PathManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _vision = GetComponent<EnemyVision>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();

        InvokeRepeating("GetPlayerPosition", 0.1f, 3);
    }

    private void FixedUpdate()
    {
        if (!_vision.CanSeePlayer())
        {
            if (!_walkAudioSource.isPlaying) _walkAudioSource.Play();
            if (_knowAboutPlayer && _playerFollowPath is not null)
            {
                FollowPaths(_playerFollowPath, _speed * 2f, false);
            }
            else
            {
                if (_patrolPath is not null) 
                    FollowPaths(_patrolPath, _speed, true);
            }
        }
        else
        {
            if (_walkAudioSource.isPlaying) _walkAudioSource.Stop();
            if (!_knowAboutPlayer)
            {
                _knowAboutPlayer = true;
                GetPlayerPosition();
            }
            _rigidbody.velocity = Vector2.zero;
        }
        if (_rigidbody.velocity.sqrMagnitude == 0) 
            _animator.SetBool("Walk", false);
        else
            _animator.SetBool("Walk", true);
    }

    private void FollowPaths(Transform[] path, float speed, bool isLooped)
    {
        if(_target >= path.Length) _target = 0;
        if (Vector2.Distance(transform.position, path[_target].position) < 0.2f)
        {
            if (_target == path.Length - 1 && isLooped) _target = 0;

            if (_target != path.Length - 1) _target++;
        }
        if (Vector2.Distance(transform.position, path[path.Length - 1].position) >= 0.2f || isLooped && path.Length - 1 != _target)
        {
            Vector3 norTar = (path[_target].position - transform.position).normalized;
            float angle = Mathf.Atan2(norTar.y, norTar.x) * Mathf.Rad2Deg;

            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = new Vector3(0, 0, angle - 90);
            transform.rotation = rotation;

            _rigidbody.velocity = transform.TransformDirection(Vector2.up * speed);
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0;
            transform.rotation = path[path.Length - 1].rotation;
        }
    }

    public void GetNewPatrolPath()
    {
        _patrolPath = _pathManager.GetPatrolPaths(CurrentRoom);
        _target = 0;
    }

    public void RiseAlarm()
    {
        _target = 0;
        GetPlayerPosition();
        _knowAboutPlayer = true;
    }

    private void GetPlayerPosition()
    {
        Transform[] path = _pathManager.FindPath(CurrentRoom, _pathManager.FindPlayer());
        if (path is null) return;
        if (_knowAboutPlayer)
        {
            if (_playerFollowPath is null || _target >= _playerFollowPath.Length || path.Length == 1)
            {
                _target = 0;
            }
            else
            {
                if (_playerFollowPath[_target] == path[1] && _playerFollowPath.Length > 1 && path.Length > 1) _target = 1;
                else _target = 0;
            }
        }
        _playerFollowPath = path;
    }

    private IEnumerator HitVisualization()
    {
        float gb = _sprite.color.g;
        while (gb > 0.4f)
        {
            gb = Mathf.Lerp(gb,0,0.5f);
            _sprite.color = new Color(1,gb,gb);
            yield return new WaitForFixedUpdate();
        }
        while (gb < 0.95f)
        {
            gb = Mathf.Lerp(gb, 1, 0.5f);
            _sprite.color = new Color(1, gb, gb);
            yield return new WaitForFixedUpdate();
        }
    }
}
