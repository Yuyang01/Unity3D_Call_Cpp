
using UnityEngine;

/*
Network agent and his children are used to synchronize data between thread
It will generate and read messages sent by netsynchronizer
By default synchronize the transform pos and orientation
*/
public class networkAgent : MonoBehaviour
{
    /*
     * Attributes
     */
    Vector3 _transformPosRef;
    Quaternion _transformOriRef;

    public int _idNetwork = 0;

    /*
     * Monos
     */

    /*
     * Interfaces
     */

    /// <summary>
    /// Interface of class
    /// call the right compute function
    /// </summary>
    /// <returns>"" if no message, message if one message, message1|message2|... |messageN if n messages </returns>
    public string getMessage()
    {
        return computeMessages();
    }

    /// <summary>
    /// call the right function to parse the message received
    /// </summary>
    /// <param name="message">a message is id_defMessage_val1_val2_..._valN</param>
    public void readMessage(string message)
    {
        // split messages in items separated by _ char
        string[] items = message.Split('_');
        if (items.Length == 0)
        {
            // 0 items = null message, return (should be verified yet normally)
            return;
        }

        // check if the id of message is equal to the id of the agent
        int id = int.Parse(items[0]);
        if (id != _idNetwork)
        {
            // if not, break
            return;
        }

        // This is the right id, the message is adressed to this agent
        // so parse it
        parseMessage(message);
    }

    /*
     * Internals
     */

    /// <summary>
    /// compute a list of message for the synchronisation
    /// Could be overiden in order to compute other kind of message
    /// </summary>
    /// <returns> if no message, message if one message, message1|message2|... |messageN if n messages </returns>
    protected virtual string computeMessages()
    {
        //return value
        string res = "";

        // compute transform messages
        Vector3 pos = transform.localPosition;
        if (pos != _transformPosRef)
        {
            _transformPosRef = pos;
            res += _idNetwork.ToString() + "_" + "POS" + "_" + _transformPosRef.x.ToString("F3") + "_"
                                                             + _transformPosRef.y.ToString("F3") + "_"
                                                             + _transformPosRef.z.ToString("F3");
        }

        Quaternion ori = transform.localRotation;
        if (ori != _transformOriRef)
        {
            if (res != "")
            {
                res += "|";
            }

            _transformOriRef = ori;
            res += _idNetwork.ToString() + "_" + "ORI" + "_" + _transformOriRef.x.ToString("F3") + "_"
                                                             + _transformOriRef.y.ToString("F3") + "_"
                                                             + _transformOriRef.z.ToString("F3") + "_"
                                                             + _transformOriRef.w.ToString("F3");
        }
        
        return res;
    }

    /// <summary>
    /// check what kind of message is and update gameObject
    /// </summary>
    /// <returns> if no message, message if one message, message1|message2|... |messageN if n messages </returns>
    protected virtual void parseMessage(string message)
    {
        string[] messageParts = message.Split('_');
        if (messageParts.Length < 2)
        {
            // The message is empty
        }
        else
        {
            if (messageParts[1] == "POS")
            {
                if (messageParts.Length > 4)
                {
                    Vector3 pos = new Vector3(float.Parse(messageParts[2]), float.Parse(messageParts[3]), float.Parse(messageParts[4]));
                    transform.localPosition = pos;
                }
            }
            else if (messageParts[1] == "ORI")
            {
                if (messageParts.Length > 5)
                {
                    Quaternion ori = new Quaternion(float.Parse(messageParts[2]), float.Parse(messageParts[3]), float.Parse(messageParts[4]), float.Parse(messageParts[5]));
                    transform.localRotation = ori;
                }
            }
        }
    }
}
