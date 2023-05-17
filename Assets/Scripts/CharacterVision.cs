using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVision : MonoBehaviour
{
    [SerializeField] private float _angle;
    [SerializeField] private float _radius;
    [SerializeField] private float _meshResolution;
    [SerializeField] private MeshFilter viewMeshFilter;
    [SerializeField] private LayerMask _obstructionMask;
    [SerializeField] private LayerMask _enemyMask;

    private Mesh _viewMesh;

    private void Start()
    {
        _viewMesh = new Mesh();
        _viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = _viewMesh;
    }

    private void LateUpdate()
    {
        DrawFieldOfViewMesh();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), _radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + DirectionFromAngle(transform.eulerAngles.z, _angle / 2) * _radius);
        Gizmos.DrawLine(transform.position, transform.position + DirectionFromAngle(transform.eulerAngles.z, -_angle / 2) * _radius);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(-Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
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
