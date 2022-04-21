using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNoiseFilter  : INoiseFilter{

    NoiseSettings.MyNoiseSettings settings;
    Noise noise = new Noise();

    public MyNoiseFilter(NoiseSettings.MyNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = Mathf.Sin(noise.Evaluate(point * frequency + settings.centre));
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        return noiseValue * settings.strength;
    }
}
