
using UnityEngine;

public class vrConvTools
{
    public static Vector3 ARTToUnity(Vector3 toConv)
    {
        Vector3 res = new Vector3(toConv.x, toConv.z, toConv.y);
        return res;
    }

    public static Quaternion ARTToUnity(Quaternion toConv)
    {
        Quaternion res = new Quaternion(-toConv.x, -toConv.z, -toConv.y, toConv.w);
        return res;
    }
}

