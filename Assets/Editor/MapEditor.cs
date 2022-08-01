using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor{
    MapGenerator m;
    public override void OnInspectorGUI()
    {
        if (DrawDefaultInspector())
        {
            if (m.autoUpdate)
            {
                m.DrawMapInEditor();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            m.DrawMapInEditor();
        }

        if (GUILayout.Button("Evolve"))
        {
            m.evolutionHistory = new Dictionary<int, float[,]>(m.numIsingSteps);
            var sw = new System.Diagnostics.Stopwatch ();
            sw.Start ();
            m.IsingModelEvolve();
            sw.Stop ();
            Debug.Log ($"{sw.ElapsedMilliseconds}ms)");
        }
    }
    void OnEnable () {
        m = (MapGenerator) target;
        Tools.hidden = true;
    }

    void OnDisable () {
        Tools.hidden = false;
    }
}
