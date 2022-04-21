using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  static class NoiseFilterFactory
{
    // Start is called before the first frame update

    public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
    {
        switch (settings.filterType)
        {
            case NoiseSettings.FilterType.Simple:
                return new SimpleNoiseFilter(settings.simpleNoiseSettings);
            case NoiseSettings.FilterType.Rigid:
                return new RigidNoiseFilter(settings.rigidNoiseSettings);
            case NoiseSettings.FilterType.My:
                return new MyNoiseFilter(settings.myNoiseSettings);
            case NoiseSettings.FilterType.MyCos:
                return new MyCosNoiseFilter(settings.myCosNoiseSettings);
            case NoiseSettings.FilterType.Cool:
                return new CoolNoiseFilter(settings.coolNoiseSettings);
        }

        return new SimpleNoiseFilter(settings.simpleNoiseSettings);
    }
}
