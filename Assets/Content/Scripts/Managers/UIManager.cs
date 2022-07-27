using System;
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
    //public Text accelerationCounter;
    //public Text text;
    //public Text stepCounter;
    //public Text pedometer;
    //public Text flightStepCounter;
    //public Text latitudeValue;
    //public Text longitudeValue;
    //public Text sleepAnalysis;
    //public Text distance1;
    //public Text distance2;
    //public Text distance3;
    //public Text speedValue;
    //public Text stepsDataValue;
    //public Text walkDistanceDataValue;
    //public Text flightStairsDataValue;
    //public Text sleepAnalysisDataValue;
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

    public void PopulateStepLogsJson(string json)
    {
        GameObject log = Instantiate(stepLogPrefab, stepLogContainer);
        log.GetComponent<Text>().text = json;
        GoogleFitData googleFitData =  JsonUtility.FromJson<GoogleFitData>(json);
        for (int i = 0; i < googleFitData.insertedDataPoint.Length; i++)
        {
            DateTime now = DateTime.Now;
            DateTime aWeekAgo = now.AddDays(-1);
            DateTime start = convertNano(googleFitData.insertedDataPoint[i].startTimeNanos);
            if (DateTime.Compare(aWeekAgo, start) < 0)
            {
                DateTime end = convertNano(googleFitData.insertedDataPoint[i].endTimeNanos);
                GameObject stepLog = Instantiate(stepLogPrefab, stepLogContainer);
                string answer = string.Format("In {0} to {1}, you have made {2} steps", start, end, googleFitData.insertedDataPoint[i].value[0].intVal.ToString());
                stepLog.GetComponent<Text>().text = answer;
            }
        }
    }

    //public void setText(string data)
    //{
    //    text.text = data;
    //}

    public void OnClickGetGoogleCode()
    {
        GoogleAuthenticator.GetAuthCode();
    }

    #endregion

    #region Private Variables
    private DateTime convertNano(long nano)
    {
        DateTime epochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epochTime.AddTicks(nano / 100);
    }
    #endregion
}
