using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastRender
{
    /// <summary>
    /// return the distance of the first crossed renderer shape
    /// </summary>
    /// <param name="origin">the start of the ray</param>
    /// <param name="dir">the direction of the ray</param>
    /// <param name="length">the length of the ray</param>
    /// <returns></returns>
    public static float rayCastRenderer(Vector3 origin, Vector3 dir, float length)
    {
        float computedDist = 1000.0f;
        Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();
        Ray ray = new Ray(origin, dir);
        for (int i = 0; i < renderers.Length; i++)
        {
            float dist = 0.0f;
            if( renderers[i].bounds.IntersectRay(ray,out dist) )
            {
                
            }
        }
        return computedDist;
    }
}
