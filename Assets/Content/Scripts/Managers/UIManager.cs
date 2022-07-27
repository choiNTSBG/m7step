using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Public Variables
    public Text Title;
    //public Text EfficiencyValue;
    ////public Text LuckValue;
    //public Text GSTEarningValue;
    public Text accelerationCounter;
    public Text text;
    public Text stepCounter;
    public Text pedometer;
    public Text flightStepCounter;
    public Text latitudeValue;
    public Text longitudeValue;
    public Text sleepAnalysis;
    public Text distance1;
    public Text distance2;
    public Text distance3;
    public Text speedValue;
    public Text stepsDataValue;
    public Text walkDistanceDataValue;
    public Text flightStairsDataValue;
    public Text sleepAnalysisDataValue;
    public Transform stepLogContainer;
    public GameObject stepLogPrefab;
    #endregion

    #region Private Variables
    #endregion

    #region Public Functions
    public void PopulateStepLogs(List<string> stepLogs)
    {
        for (int i = 0; i < stepLogs.Count; i++)
        {
            GameObject stepLog = Instantiate(stepLogPrefab, stepLogContainer);
            stepLog.GetComponent<Text>().text = stepLogs[i];
        }
    }

    public void PopulateStepLogs(string stepLogs)
    {
        GameObject stepLog = Instantiate(stepLogPrefab, stepLogContainer);
        stepLog.GetComponent<Text>().text = stepLogs;
    }

    public void setText(string data)
    {
        text.text = data;
    }

    public void OnClickGetGoogleCode()
    {
        GoogleAuthenticator.GetAuthCode();
    }
    #endregion

    #region Private Variables
    #endregion
}
