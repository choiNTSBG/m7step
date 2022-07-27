using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;

namespace BeliefEngine.HealthKit
{


	public partial class HealthKitDataTypesEditor : Editor
	{
		private bool clinicalDataSection = false;

		partial void DrawClinicalSupport() {
			clinicalDataSection = EditorGUILayout.Foldout(clinicalDataSection, "Clinical Data");
			if (clinicalDataSection) {
				DrawDataTypes(HKClinicalType.HKClinicalTypeIdentifierAllergyRecord, HKClinicalType.HKClinicalTypeIdentifierVitalSignRecord);
			}
		}
	}
}
