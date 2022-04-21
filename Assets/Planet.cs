using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public ColorSettings ColorSettings;
    public ShapeSettings ShapeSettings;
    [Range(3,256)]
    public int resolution = 10;
    [SerializeField, HideInInspector]
    private MeshFilter[] _meshFilters;
    public bool isSphere;
    [HideInInspector]
    public bool shapeEditorFoldout;
    [HideInInspector]
    public bool colourEditorFoldout;

    public bool autoUpdate;
    private ShapeGenerator _shapeGenerator = new ShapeGenerator();

    private ColourGenerator _colourGenerator = new ColourGenerator();
  //  [Range(1.5f, 5f)]
   // public float defStrenght = 2f;
  //  [Range(0.5f, 5f)]
   // public float smoothValue = 2f;

   // public float radius = 2f;
   // public float smothingFactor = 2f;
    //private Mesh[] _defMesh = new Mesh[6];
    private Vector3[] _vertices, _modifiedVerts;
   // private RaycastHit _hit;
   // private Ray _ray;
    private Terrain[] _terrains;
    // Start is called before the first frame update
    void Initialize()
    {
        _shapeGenerator.UpdateSettings(ShapeSettings);
        _colourGenerator.UpdateSettings(ColorSettings);
        if (_meshFilters == null || _meshFilters.Length == 0)
        {


            _meshFilters = new MeshFilter[6];
        }

        _terrains = new Terrain[6];
        Vector3[] directions =
        {
            Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward,
            Vector3.back,
        };
        for (int i = 0; i < 6; i++)
        {
            if (_meshFilters[i] == null)
            {


                GameObject gameObj = new GameObject("mesh");
                gameObj.transform.parent = transform;

                gameObj.AddComponent<MeshRenderer>();
                _meshFilters[i] = gameObj.AddComponent<MeshFilter>();
                _meshFilters[i].sharedMesh = new Mesh();
            }

            _meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = ColorSettings.planetMaterial;

            _terrains[i] = new Terrain(_shapeGenerator,_meshFilters[i].sharedMesh, resolution, directions[i], isSphere);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColour();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {


            Initialize();
            GenerateMesh();
        }
    }
    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {


            Initialize();
            GenerateColour();
        }
    }
    void GenerateMesh()
    {
        foreach (Terrain terrain in _terrains)
        {
            terrain.ConstructorMesh();
        }
        _colourGenerator.UpdateElevation(_shapeGenerator.elevationMinMax);
    }

    void GenerateColour()
    {
       _colourGenerator.UpdateColours();
       for (int i = 0; i < 6; i++)
       {
           if (_meshFilters[i].gameObject.activeSelf)
           {
               _terrains[i].UpdateUVs(_colourGenerator);
           }
       }
    }
  //  private void OnValidate()
 //   {
 //      GeneratePlanet();        
 //   }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
