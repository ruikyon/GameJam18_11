using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class UdpReceiver
{
    private const int LOCAL_PORT = 22222;
    static UdpClient udp;
    Thread thread;

    private static Action<float, float, float> AccelCallBack;

    public UdpReceiver(Action<float, float, float> action)
    {
        AccelCallBack += action;
    }

    public void UdpStart()
    {
        udp = new UdpClient(LOCAL_PORT);
        udp.Client.ReceiveTimeout = 3000;
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();
    }

    private static void ThreadMethod()
    {
        while (true)
        {
            byte[] data;
            try
            {
                IPEndPoint remoteEp = null;
                data = udp.Receive(ref remoteEp);

                string text = Encoding.ASCII.GetString(data);

                JsonNode jsonNode = JsonNode.Parse(text);

                double ax = jsonNode["sensordata"]["accel"]["x"].Get<double>();
                double ay = jsonNode["sensordata"]["accel"]["y"].Get<double>();
                double az = jsonNode["sensordata"]["accel"]["z"].Get<double>();

                AccelCallBack((float)ax, (float)ay, (float)az);
                //Debug.Log("(x, y, z): "+ax);
            }
            catch (SocketException se)
            {
                udp.Close();
                udp = new UdpClient(LOCAL_PORT);
                udp.Client.ReceiveTimeout = 3000;
                Debug.Log(se);
            }
            catch (NullReferenceException nre)
            {
                Debug.Log(nre);
            }
            catch (InvalidCastException ice)
            {
                Debug.Log(ice);
            }
        }
    }

    void OnApplicationQuit()
    {
        thread.Abort();
        udp.Close();
    }
}
