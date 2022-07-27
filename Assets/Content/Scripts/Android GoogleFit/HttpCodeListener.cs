using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class HttpCodeListener
{
    private HttpListener listener;
    private Thread listenerThread;
    private Action<string> onCodeFetched;

    private const string responseHtml = "Success, you can return to the app now!"; // TODO: Change this to a successful html response

    public HttpCodeListener(int port)
    {
        listener = new HttpListener();
        //listener.Prefixes.Add($"https://www.google.com/");
        listener.Prefixes.Add($"http://localhost:{port}/");
        //listener.Prefixes.Add($"http://{LocalIPAddress()}:{port}/");
        listener.Prefixes.Add($"http://127.0.0.1:{port}/");
        ////listener.Prefixes.Add("https://staging.murasaki7.com/");
        ////listener.Prefixes.Add("https://fit-b741a.firebaseapp.com/");
        //listener.Prefixes.Add($"https://fit-b741a.firebaseapp.com:{port}/");
        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
    }

    public void StartListening(Action<string> callback)
    {
        onCodeFetched = callback;

        listener.Start();

        listenerThread = new Thread(ListeningThread);
        listenerThread.Start();
    }

    public void StopListening()
    {
        listener.Stop();
        onCodeFetched = null;
    }

    private void ListeningThread()
    {
        while (true)
        {
            var result = listener.BeginGetContext(ListenerCallback, listener);
            result.AsyncWaitHandle.WaitOne();
        }
    }

    private void ListenerCallback(IAsyncResult result)
    {
        var context = listener.EndGetContext(result);
        Debug.Log("context.Request: " + context.Request.Url);
        //if (!context.Request.QueryString.AllKeys.Contains("code")) return;
        //UnityMainThreadDispatcher.Instance().Enqueue(() => onCodeFetched?.Invoke(context.Request.QueryString.Get("code")));
        string[] strings = context.Request.Url.ToString().Split('=');
        string code = strings[1].Replace("&scope","");
        UnityMainThreadDispatcher.Instance().Enqueue(() => onCodeFetched?.Invoke(code));

        var buffer = Encoding.UTF8.GetBytes(responseHtml);

        context.Response.ContentLength64 = buffer.Length;
        var st = context.Response.OutputStream;
        st.Write(buffer, 0, buffer.Length);

        context.Response.Close();
    }
    public static string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "0.0.0.0";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}