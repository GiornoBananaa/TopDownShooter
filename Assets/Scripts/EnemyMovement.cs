using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1;

    private PathManager _pathManager;
    private Transform[] _patrolPath;
    private Transform[] _playerFollowPath;
    private Rigidbody2D _rigidbody;
    private EnemyVision _vision;
    private Animator _animator;
    private int _target;
    private bool _goBack;
    private bool _stayOnPosition;
    private static bool _knowAboutPlayer;

    public RoomManager CurrentRoom;


    private void Start()
    {
        _goBack = false;
        _target = 0;
        _knowAboutPlayer = false;
        Physics2D.gravity = Vector2.zero;
        _pathManager = GameObject.Find("PathManager").GetComponent<PathManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _vision = GetComponent<EnemyVision>();
        _animator = GetComponent<Animator>();

        InvokeRepeating("GetNewPatrolPath", 0.1f, 6);
        InvokeRepeating("GetPlayerPosition", 0.1f, 3);
    }

    private void FixedUpdate()
    {
        if (!_vision.CanSeePlayer())
        {
            if (_knowAboutPlayer)
            {
                FollowPaths(_playerFollowPath, _speed * 1.5f, false);
            }
            else
            {
                if (_patrolPath is not null) FollowPaths(_patrolPath, _speed, true);
            }
        }
        else
        {
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
        if (Vector2.Distance(transform.position, path[_target].position) < 0.2f)
        {
            if (_target == path.Length - 1) _goBack = true;
            else if (_target == 0) _goBack = false;

            if (_goBack && isLooped) _target--;
            else if(!_goBack) _target++;
        }
        if (!(!isLooped && Vector2.Distance(transform.position, path[path.Length - 1].position) < 0.2f))
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
        }
    }

    public void GetNewPatrolPath()
    {
        _patrolPath = _pathManager.GetPatrolPaths(CurrentRoom);
    }

    public void GetPlayerPosition()
    {
        Transform[] path = _pathManager.FindPath(CurrentRoom, _pathManager.FindPlayer());
        if (_knowAboutPlayer)
        {
            if (_target >= _playerFollowPath.Length || path.Length == 1)
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
}
