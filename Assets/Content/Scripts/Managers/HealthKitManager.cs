using BeliefEngine.HealthKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKitManager : Singleton<HealthKitManager>
{
	#region Public Variables
	public HealthKitDataTypes types;
	#endregion

	#region Private 
	private HealthStore healthStore;
	private int days = 0;
	private int currentDay = 0;
	#endregion

	#region Public Functions
	public void ReadSteps()
	{
		DateTimeOffset end = DateTimeOffset.UtcNow;
		DateTimeOffset start = end.AddDays(-7);

		//healthStore.ReadSteps(start, end, delegate (double steps, Error error) {
		//	//UIManager.Instance.stepsDataValue.text = steps.ToString();
		//	//string answer = string.Format("You have made {0} steps since Yesterday", steps);
		//	string answer = string.Format("In {0} to {1}, you have made {2} steps", start.ToLocalTime(), end.ToLocalTime().ToString("h:mm:ss tt"), steps.ToString());
		//	//UIManager.Instance.stepsDataValue.text = answer;
		//	List<String> stepsLog = new List<string>() { };
		//	stepsLog.Add(answer);
		//	UIManager.Instance.PopulateStepLogs(answer);
		//});

		this.healthStore.ReadQuantitySamples(HKDataType.HKQuantityTypeIdentifierStepCount, start, end, delegate (List<QuantitySample> samples, Error error) {
			if (samples.Count > 0)
			{
				Debug.Log("found " + samples.Count + " samples");
				bool cumulative = (samples[0].quantityType == QuantityType.cumulative);
				string text = "";
				double sum = 0;
				foreach (QuantitySample sample in samples)
				{
					Debug.LogFormat("   - {0} : {1}", sample, sample.quantity.doubleValue);
					if (cumulative)
					{
						sum += Convert.ToInt32(sample.quantity.doubleValue);
						Debug.LogFormat("       - sum:{0}", sample.sumQuantity);
					}
					else
					{
						text = text + "- " + sample + "\n";
						Debug.LogFormat("       - min:{0} / max:{1} / avg:{2}", sample.minimumQuantity, sample.maximumQuantity, sample.averageQuantity);
					}
				}

				if (cumulative)
				{
					Debug.LogFormat("       - sum:{0}", sum);
				}
				else
				{
					Debug.LogFormat("{0}", text);
				}
			}
			else
			{
				Debug.Log("found no samples");
			}
		});
	}

	//public void ReadFlights()
	//{
	//	DateTimeOffset end = DateTimeOffset.UtcNow;
	//	//DateTimeOffset start = end.AddMinutes(-10);
	//	DateTimeOffset start = end.AddDays(-1);

	//	int steps = 0;
	//	this.healthStore.ReadQuantitySamples(HKDataType.HKQuantityTypeIdentifierFlightsClimbed, start, end, delegate (List<QuantitySample> samples, Error error) {
	//		Debug.Log("found " + samples.Count + " flights samples");
	//		foreach (QuantitySample sample in samples)
	//		{
	//			Debug.Log("   - " + sample.quantity.doubleValue + " from " + sample.startDate + " to " + sample.endDate);
	//			steps += Convert.ToInt32(sample.quantity.doubleValue);
	//		}
	//		//this.resultsLabel.text += "No flights found.";
	//		//UIManager.Instance.flightStepCounter.text = "no data for the past 10 minutes yet";
	//		//UIManager.Instance.flightStairsDataValue.text = "no data for the past 10 minutes yet";
	//		UIManager.Instance.flightStairsDataValue.text = string.Format("You have done {0} Flight of Stairs since Yesterday", steps);
	//	});
	//}

	//public void ReadSleep()
	//{
	//	DateTimeOffset end = DateTimeOffset.UtcNow;
	//	//DateTimeOffset start = end.AddMinutes(-10);
	//	DateTimeOffset start = end.AddDays(-1);

	//	Debug.Log("reading sleep from " + start + " to " + end);
	//	this.healthStore.ReadCategorySamples(HKDataType.HKCategoryTypeIdentifierSleepAnalysis, start, end, delegate (List<CategorySample> samples, Error error) {
	//		//string text = "";
	//		//String str;
	//		double hrs = 0;
	//		foreach (CategorySample sample in samples)
	//		{
	//			string valueString = ((SleepAnalysis)sample.value == SleepAnalysis.Asleep) ? "Sleeping" : "In Bed";
	//			//str = string.Format("- {0} from {1} to {2}", valueString, sample.startDate, sample.endDate);
	//			//Debug.Log(str);
	//			//text = text + str + "\n";
	//			TimeSpan ts = sample.endDate - sample.startDate;
	//			hrs += ts.TotalHours;
	//		}
	//		//UIManager.Instance.sleepAnalysis.text = text;
	//		//UIManager.Instance.sleepAnalysisDataValue.text = text;
	//		UIManager.Instance.sleepAnalysisDataValue.text = string.Format("You have slept {0} Hours since Yesterday", hrs.ToString("#.##"));
	//		//this.resultsLabel.text = text;
	//	});
	//}

	//public void ReadPedometer() {
	//	DateTimeOffset start = DateTimeOffset.UtcNow;

	//	//if (!reading) {
	//	//	int steps = 0;
	//	//	this.healthStore.BeginReadingPedometerData(start, delegate(List<PedometerData> data, Error error) {
	//	//		foreach (PedometerData sample in data) {
	//	//			steps += sample.numberOfSteps;
	//	//		}
	//	//		this.resultsLabel.text = string.Format("{0}", steps);
	//	//	});
	//	//	buttonLabel.text = "Stop reading";
	//	//	reading = true;
	//	//} else {
	//	//	this.healthStore.StopReadingPedometerData();
	//	//	buttonLabel.text = "Start reading";
	//	//	reading = false;
	//	//}

	//	//buttonLabel.text = "Stop reading";
	//	//reading = true;
	//	int steps = 0;
	//	this.healthStore.BeginReadingPedometerData(start, delegate (List<PedometerData> data, Error error) {
	//		steps = 0;
	//		foreach (PedometerData sample in data)
	//		{
	//			steps += sample.numberOfSteps;
	//		}
	//		//this.resultsLabel.text = string.Format("{0}", steps);
	//		//UIManager.Instance.pedometer.text = string.Format("{0}", steps);
	//		UIManager.Instance.pedometer.text = string.Format("You have done {0} steps", steps);
	//	});
	//}

	//public void ReadWalkDistance()
	//{
	//	DateTimeOffset end = DateTimeOffset.UtcNow;
	//	//DateTimeOffset start = end.AddMinutes(-10);
	//	DateTimeOffset start = end.AddDays(-1);

	//	int steps = 0;
	//	this.healthStore.ReadQuantitySamples(HKDataType.HKQuantityTypeIdentifierDistanceWalkingRunning, start, end, delegate (List<QuantitySample> samples, Error error) {
	//		Debug.Log("found " + samples.Count + " flights samples");
	//		foreach (QuantitySample sample in samples)
	//		{
	//			Debug.Log("   - " + sample.quantity.doubleValue + " from " + sample.startDate + " to " + sample.endDate);
	//			steps += Convert.ToInt32(sample.quantity.doubleValue);
	//		}

	//		UIManager.Instance.walkDistanceDataValue.text = string.Format("You have traveled {0} Km. since Yesterday", steps.ToString("#.##"));
	//	});
	//}

	public void readAllSteps()
    {
		days = 7;
		currentDay = 0;
		loopSteps();
	}
	#endregion

	#region Private Functions
	void Start()
	{
		Debug.Log("---------- START ----------");
		healthStore = this.GetComponent<HealthStore>();
		healthStore.Authorize(this.types);
		//InvokeRepeating("ReadDatas", 0.5f, 5f);
		StartCoroutine(readData());
	}

	private IEnumerator readData()
    {
		yield return new WaitForSeconds(3.0f);
		readDatas();
    }

	private void readDatas()
    {
        //ReadPedometer();
        ReadSteps();
        //      ReadWalkDistance();
        //ReadFlights();
        //ReadSleep();
        //      readAllSteps();
    }

	private void loopSteps()
	{
		//List<String> stepLogs = new List<string>() { };

		DateTimeOffset now = DateTimeOffset.UtcNow;
		DateTimeOffset end = now.AddDays((currentDay) * -1);
		DateTimeOffset start = end.AddDays(-1);
		Debug.Log("start: " + start.ToString());
		Debug.Log("end: " + end.ToString());

		healthStore.ReadSteps(start, end, delegate (double steps, Error error)
		{
			String date = start.Day.ToString() + "/" + start.Month.ToString();
			String answer = string.Format("In {0}, you have made {1} steps", date, steps);
			//Debug.Log("answer: " + answer);
			//stepLogs.Add(answer);
			UIManager.Instance.PopulateStepLogs(answer);
			if(currentDay != days)
            {
				currentDay++;
				loopSteps();
            }
		});
	}
	#endregion
}
