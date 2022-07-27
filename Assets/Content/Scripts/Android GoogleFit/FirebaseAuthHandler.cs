using Proyecto26;
using UnityEngine;

/// <summary>
/// Handles authentication calls to Firebase
/// </summary>

public static class FirebaseAuthHandler
{
    private const string ApiKey = "AIzaSyD6MJFXoqe4OoqZZC17FQ5Xw5HFV3GFq7Q"; //TODO: Change [API_KEY] to your API_KEY

    /// <summary>
    /// Signs in a user with their Id Token
    /// </summary>
    /// <param name="token"> Id Token </param>
    /// <param name="providerId"> Provider Id </param>
    public static void SingInWithToken(string token, string providerId)
    {
        var payLoad =
            $"{{\"postBody\":\"id_token={token}&providerId={providerId}\",\"requestUri\":\"http://localhost\",\"returnIdpCredential\":true,\"returnSecureToken\":true}}";
        RestClient.Post($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={ApiKey}", payLoad).Then(
            response =>
            {
                // You now have the userId (localId) and the idToken of the user!
                Debug.Log(response.Text);
            }).Catch(Debug.Log);    
    }

    public static void GetStepsData(string token)
    {
        string url = "https://fitness.googleapis.com/fitness/v1/users/me/dataSources/derived:com.google.step_count.delta:com.google.android.gms:estimated_steps/dataPointChanges?access_token=" + token;
        //string url = "https://www.googleapis.com/fitness/v1/users/me/dataSources/derived:com.google.step_count.delta:com.google.android.gms:estimated_steps/datasets/1658320628-1658925563?access_token=" + token;
        Debug.Log("GetStepsData URL: " + url);
        RestClient.Get(url).Then(
            response =>
            {
                Debug.Log("GetStepsData: " + response.Text);
                UIManager.Instance.PopulateStepLogsJson(response.Text);
            }).Catch(Debug.Log);
    }
}
