
using UnityEngine;

public class bezier
{
    /*
    return the resutl of a bezier operation
    xValue should be between 0.0f and 1.0f
    p1X, p1Y, p2X, p2Y, input values for curves
    */
    public static float cubic_bezier(float xValue, float p1X, float p1Y, float p2X, float p2Y)
    {
        /*
        To compute an animation speed curve with bezier formule
        http://cubic-bezier.com/ to see preview
        */

        //float p0X = 0.0f;
        float p0Y = 0.0f;
        //float p3X = 1.0f;
        float p3Y = 1.0f;

        float A = Mathf.Pow(1.0f - xValue, 3) * p0Y;
        float B = 3.0f * Mathf.Pow(1.0f - xValue, 2) * xValue * p1Y;
        float C = 3.0f * (1.0f - xValue) * Mathf.Pow(xValue, 2) * p2Y;
        float D = Mathf.Pow(xValue, 3) * p3Y;

        return A + B + C + D;
    }

    /*
    start slow end slow
    */
    public static float bezier_easeIn_easeOut(float xValue)
    {
        return cubic_bezier(xValue,0.95f,0.05f, 0.05f, 0.95f);
    }

    public static float bezier_parabole(float xValue)
    {
        return cubic_bezier(xValue, 0.75f, 0.25f, 0.75f, 0.25f);
    }
}
