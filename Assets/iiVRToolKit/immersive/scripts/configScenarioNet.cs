
using UnityEngine;

public class configScenarioNet : networkAgent
{
    Vector3 _oldHeadPos = new Vector3();

    protected override string computeMessages()
    {
        string res = "";

        Vector3 offset = GetComponent<configScenario>().getHeadValue();

        if (_oldHeadPos != offset)
        {
            res += "_iiCFGHEADOFFSET_" + offset.x.ToString("F4") + "_" + offset.y.ToString("F4") + "_" + offset.z.ToString("F4");
            _oldHeadPos = offset;
        }

        if (res != "")
        {
            res = _idNetwork.ToString("F0") + res;
        }

        return res;
    }

    protected override void parseMessage(string message)
    {
        string[] items = message.Split('_');
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == "iiCFGHEADOFFSET")
            {
                float valX = 0.0f;
                float valY = 0.0f;
                float valZ = 0.0f;

                i++;
                if (i < items.Length)
                {
                    valX = float.Parse(items[i]);
                }

                i++;
                if (i < items.Length)
                {
                    valY = float.Parse(items[i]);
                }

                i++;
                if (i < items.Length)
                {
                    valZ = float.Parse(items[i]);
                }

                GetComponent<configScenario>().setHeadValue(new Vector3(valX,valY,valZ));
            }
        }
    }
}
