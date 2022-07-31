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
            var sw = new System.Diagnostics.Stopwatch ();
            sw.Start ();
            m.IsingModelEvolve();
            sw.Stop ();
            Debug.Log ($"Generated evolution history ({m.numIsingSteps} iterations; {sw.ElapsedMilliseconds}ms)");
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
