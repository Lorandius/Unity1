using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet _planet;
    private Editor shapeEditor;
    private Editor colourEditor;
    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                _planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            _planet.GeneratePlanet();
        }

        DrawSettingsEditor(_planet.ShapeSettings, _planet.OnShapeSettingsUpdated, 
            ref _planet.shapeEditorFoldout, ref shapeEditor);
        DrawSettingsEditor(_planet.ColorSettings, _planet.OnColourSettingsUpdated, 
            ref _planet.colourEditorFoldout, ref colourEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingUpdated,ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {


            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

            using (var check = new EditorGUI.ChangeCheckScope())
            {

                if (foldout)
                {

                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();
                    if (check.changed)
                    {
                        if (onSettingUpdated != null)
                        {
                            onSettingUpdated();
                        }
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        _planet = (Planet) target;
    }
}
