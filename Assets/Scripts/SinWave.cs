using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Shapes;

[ExecuteAlways] public class SinWave : ImmediateModeShapeDrawer
{
    public float discRadius ;
    public Color color;
    public float movementRadius;

    private float x = 0;

    public override void DrawShapes(Camera cam)
    {        
        using (Draw.Command(cam)) 
        {
            // Sets up all static parameters.
            // These are used for all following Draw.Line calls
            Draw.LineGeometry = LineGeometry.Volumetric3D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.Thickness = 4; // 4px wide
            // draw lines
            Draw.Line( Vector3.zero, Vector3.right, Color.red );
            Draw.Line( Vector3.zero, Vector3.up, Color.green );
            Draw.Line( Vector3.zero, Vector3.forward, Color.blue );
        }
    }
}