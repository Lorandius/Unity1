using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings {
    public enum FilterType
    {
        Simple, Rigid, My, MyCos, Cool
    }
    public FilterType filterType;
    [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", 1)]
    public RigidNoiseSettings rigidNoiseSettings;
    [ConditionalHide("filterType", 2)]
    public MyNoiseSettings myNoiseSettings;
    [ConditionalHide("filterType", 3)]
    public MyCosNoiseSettings myCosNoiseSettings;
    [ConditionalHide("filterType", 4)]
    public CoolNoiseSettings coolNoiseSettings;
    [System.Serializable]
    public class SimpleNoiseSettings
    {


       
        public float strength = 1;
        [Range(1, 8)] public int numLayers = 1;
        public float baseRoughness = 1;
        public float roughness = 2;
        public float persistence = .5f;
        public Vector3 centre;
        public float minValue;
    }
    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultiplier = .8f;
    }
    [System.Serializable]
    public class MyNoiseSettings : SimpleNoiseSettings
    {
        
    }
    [System.Serializable]
    public class MyCosNoiseSettings : SimpleNoiseSettings
    {
        
    }
    [System.Serializable]
    public class CoolNoiseSettings : SimpleNoiseSettings
    {
        
    }
}