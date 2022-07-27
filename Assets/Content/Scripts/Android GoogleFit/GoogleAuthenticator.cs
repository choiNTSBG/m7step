using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Proyecto26;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Handles calls to the Google provider for authentication
/// </summary>

public static class GoogleAuthenticator
{
    private const string ClientId = "234054440659-bdg0bbhg1cf8mva1svg0i67jonercjeg.apps.googleusercontent.com"; //TODO: Change [CLIENT_ID] to your CLIENT_ID
    private const string ClientSecret = "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"; //TODO: Change [CLIENT_SECRET] to your CLIENT_SECRET
    
    private const int Port = 8080;
    private static string RedirectUri = $"http://127.0.0.1:{Port}";

    private static readonly string scopeUri = "https://www.googleapis.com/auth/fitness.activity.read";


    private static readonly HttpCodeListener codeListener = new HttpCodeListener(Port);

    /// <summary>
    /// Opens a webpage that prompts the user to sign in and copy the auth code 
    /// </summary>
    public static void GetAuthCode()
    {
        string url = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={ClientId}&redirect_uri={RedirectUri}&response_type=code&scope={scopeUri}&flowName=GeneralOAuthFlow&prompt=consent";
        Debug.Log("url: " + url);
        Application.OpenURL(url);

        codeListener.StartListening(code =>
        {
            Debug.Log("code: " + code);
            ExchangeAuthCodeWithAccessToken(code, idToken =>
            {
                Debug.Log("idToken: " + idToken);
                //FirebaseAuthHandler.SingInWithToken(idToken, "google.com");
                FirebaseAuthHandler.GetStepsData(idToken);
            });
            
            codeListener.StopListening();
        });
    }
    
    /// <summary>
    /// Exchanges the Auth Code with the user's Id Token
    /// </summary>
    /// <param name="code"> Auth Code </param>
    /// <param name="callback"> What to do after this is successfully executed </param>
    public static void ExchangeAuthCodeWithIdToken(string code, Action<string> callback)
    {
        try
        {
            RestClient.Request(new RequestHelper
            {
                Method = "POST",
                Uri = "https://oauth2.googleapis.com/token",
                Params = new Dictionary<string, string>
                {
                    {"code", code},
                    {"client_id", ClientId},
                    {"client_secret", ClientSecret},
                    {"redirect_uri", RedirectUri},
                    {"grant_type", "authorization_code"}
                }
            }).Then(
                response =>
                {
                    var data =
                        StringSerializationAPI.Deserialize(typeof(GoogleIdTokenResponse), response.Text) as
                            GoogleIdTokenResponse;
                    //callback(data.id_token);
                }).Catch(Debug.Log);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    /// <summary>
    /// Exchanges the Auth Code with the user's access Token
    /// </summary>
    /// <param name="code"> Auth Code </param>
    /// <param name="callback"> What to do after this is successfully executed </param>
    public static void ExchangeAuthCodeWithAccessToken(string code, Action<string> callback)
    {
        try
        {
            RestClient.Request(new RequestHelper
            {
                Method = "POST",
                Uri = "https://oauth2.googleapis.com/token",
                Params = new Dictionary<string, string>
                {
                    {"code", code},
                    {"client_id", ClientId},
                    {"client_secret", ClientSecret},
                    {"redirect_uri", RedirectUri},
                    {"grant_type", "authorization_code"}
                }
            }).Then(
                response =>
                {
                    var data =
                        StringSerializationAPI.Deserialize(typeof(GoogleIdTokenResponse), response.Text) as
                            GoogleIdTokenResponse;
                    callback(data.access_token);
                }).Catch(Debug.Log);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
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
