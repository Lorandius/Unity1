using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain
{
    private ShapeGenerator _shapeGenerator;
    private Mesh _mesh;
    private int _resolution;
    private Vector3 _localUp;
    private Vector3 _AxisA;
    private bool isSphere = true;
    public Terrain(ShapeGenerator shapeGenerator,Mesh mesh, int resolution, Vector3 localUp, bool isSphere)
    {
        this._shapeGenerator = shapeGenerator;
        this._mesh = mesh;
        this._resolution = resolution;
        this._localUp = localUp;
        this.isSphere = isSphere;
        _AxisA = new Vector3(localUp.y, _localUp.z, _localUp.x);
        _AxisB = Vector3.Cross(_localUp,_AxisA);
    }

    public Terrain(Mesh mesh, int resolution, Vector3 localUp,bool isSphere)
    {
        _mesh = mesh;
        _resolution = resolution;
        _localUp = localUp;
        this.isSphere = isSphere;
        _AxisA = new Vector3(localUp.y, _localUp.z, _localUp.x);
        _AxisB = Vector3.Cross(_localUp,_AxisA);
    }

    private Vector3 _AxisB;
    private Vector3[] vertices;
    private int[] triangles ;
    public void ConstructorMesh()
    {
        int triIndex = 0;
        vertices = new Vector3[_resolution * _resolution];
    triangles = new int[(_resolution - 1) * (_resolution - 1) * 6];
    Vector2[] uv = _mesh.uv;
        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                int i = x + y * _resolution;
                Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                Vector3 pointOnUnitCube = _localUp + (percent.x - .5f) * 2 * _AxisA + (percent.y - .5f) * 2 *_AxisB;
                if (isSphere)
                {


                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    vertices[i] = _shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);
                }
                else
                {

                    vertices[i] = _shapeGenerator.CalculatePointOnPlanet(pointOnUnitCube);
                }

                if (x != _resolution - 1 && y != _resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + _resolution + 1;
                    triangles[triIndex + 2] = i + _resolution;
                    
                    triangles[triIndex + 3] = i ;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + _resolution + 1;
                    triIndex += 6;
                }
            }
        }
    
        
        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
        _mesh.uv = uv;
    }

    public void deformMesh(float coeff)
    {
        
        for (int i =0; i < vertices.Length; i++)
        {
            vertices[i] = vertices[i] + (Vector3.up * coeff);
        }
        _mesh.RecalculateNormals();
    }
    public void UpdateUVs(ColourGenerator colourGenerator)
    {
        Vector2[] uv = new Vector2[_resolution * _resolution];

        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                int i = x + y * _resolution;
                Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                Vector3 pointOnUnitCube = _localUp + (percent.x - .5f) * 2 * _AxisA + (percent.y - .5f) * 2 * _AxisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                uv[i] = new Vector2(colourGenerator.BiomePercentFromPoint(pointOnUnitSphere),0);
            }
        }
        _mesh.uv = uv;
    }

}
