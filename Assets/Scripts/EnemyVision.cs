using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private float _angle;
    [SerializeField] private float _radius;
    [SerializeField] private float _meshResolution;
    [SerializeField] private float _rateOfFire;
    [SerializeField] private MeshFilter viewMeshFilter;
    [SerializeField] private Texture _fiewTexture;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstructionMask;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _gun;

    private GameObject _player;
    private bool _canSeePlayer;
    private float _shootTime;
    private Mesh _viewMesh;

    private void Start()
    {
        _canSeePlayer = false;
        _shootTime = 0;

        _viewMesh = new Mesh();
        _viewMesh.name = "View Mesh";
        //_viewMesh.SetColors(new Color[] { new Color(1, 1, 1, 0.2f) });
        viewMeshFilter.mesh = _viewMesh;
        viewMeshFilter.gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", _fiewTexture);
    }
    private void LateUpdate()
    {
        DrawFieldOfViewMesh();
    }
    void Update()
    {
        FieldOfViewCheck();

        if (_canSeePlayer)
        {
            ShootTarget();
        }
        _shootTime += Time.deltaTime;
    }

    private void ShootTarget()
    {
        Vector2 direction = ((Vector2)_player.transform.position - (Vector2)transform.position).normalized;

        transform.up = direction;

        if (_shootTime > _rateOfFire)
        {
            GameObject bullet = Instantiate(_bullet, _gun.position + _gun.up * 0.3f, _gun.rotation);
            bullet.GetComponent<BulletScript>().TargetLayer = _targetMask;
            bullet.transform.parent = null;
            _shootTime = 0;
        }
    }

    private void FieldOfViewCheck()
    {
        Collider2D rangeChecks = Physics2D.OverlapCircle(transform.position, _radius, _targetMask);

        if (rangeChecks != null)
        {
            Transform target = rangeChecks.transform;
            _player = target.gameObject;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < _angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask))
                {
                    _canSeePlayer = true;
                }
                else
                {
                    _canSeePlayer = false;
                }
            }
            else
                _canSeePlayer = false;
        }
        else if (_canSeePlayer)
            _canSeePlayer = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), _radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + DirectionFromAngle(transform.eulerAngles.z, _angle / 2) * _radius);
        Gizmos.DrawLine(transform.position, transform.position + DirectionFromAngle(transform.eulerAngles.z, -_angle / 2) * _radius);
        if (_canSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _player.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(-Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    public bool CanSeePlayer()
    {
        return _canSeePlayer;
    }

    private void DrawFieldOfViewMesh()
    {
        int stepCount = Mathf.RoundToInt(_angle * _meshResolution);
        float stepAngleSize = _angle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= stepCount; ++i)
        {
            float angle = transform.eulerAngles.y - _angle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] verticles = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        verticles[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; ++i)
        {
            verticles[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        _viewMesh.Clear();
        _viewMesh.vertices = verticles;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(transform.eulerAngles.z, globalAngle);
        RaycastHit2D hit;

        if (hit = Physics2D.Raycast(transform.position, dir, _radius, _obstructionMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * _radius, _radius, globalAngle);
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool hit, Vector3 point, float dst, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.dst = dst;
            this.angle = angle;
        }
    }
}
