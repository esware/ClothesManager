using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace EWGames.Dev.Scripts.Editor
{
#if UNITY_EDITOR
    public class ResetGame:EditorWindow
    {
        private List<MachineData> machineDatas = new List<MachineData>();
        
        [MenuItem("Tools/Reset Game")]
        public static void ShowWindow()
        {
            GetWindow<ResetGame>("Reset Game");
        }

        void OnGUI()
        {
            GUILayout.Label("This will clear all data. Are you sure?");
            GUILayout.Label("Select ScriptableObjects to delete:");

            foreach (var obj in GetAllMachineDataScriptableObjects())
            {
                machineDatas.Add(EditorGUILayout.ObjectField(obj, typeof(MachineData), false) as MachineData);
            }
            
            if (GUILayout.Button("Clear Data"))
            {
                PlayerPrefs.DeleteAll();
                foreach (var machine in machineDatas)
                {
                    machine.isLocked = true;
                }
                Debug.Log("Data cleared.");
            }
            
            if (GUILayout.Button("Unlock All Machines Data"))
            {
                PlayerPrefs.DeleteAll();
                foreach (var machine in machineDatas)
                {
                    machine.isLocked = false;
                }
                Debug.Log("Machines Unlocked.");
            }
            
            if (GUILayout.Button("Multiply the money earned by all your machines"))
            {
                PlayerPrefs.DeleteAll();
                foreach (var machine in machineDatas)
                {
                    machine.earnedMoney = 1200;
                }
                Debug.Log("Success.");
            }
        }
        private List<MachineData> GetAllMachineDataScriptableObjects()
        {
            string[] guids = AssetDatabase.FindAssets("t:MachineData");

            List<MachineData> machineDatas = new List<MachineData>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                MachineData machineData = AssetDatabase.LoadAssetAtPath<MachineData>(path);

                if (machineData != null)
                {
                    machineDatas.Add(machineData);
                }
            }

            return machineDatas;
        }
    }
#endif
}