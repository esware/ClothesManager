namespace EWGames.Dev.Scripts.Serialization
{
    using UnityEngine;
    using System.Collections.Generic;

    public class StringListManager : MonoBehaviour
    {
        public List<string> saveNameList = new List<string>();

        private void OnEnable()
        {
            LoadSaveNameList();
        }

        private void OnDisable()
        {
            SaveSaveNameList();
        }

        private void SaveSaveNameList()
        {
            string saveNames = string.Join(",", saveNameList.ToArray());
            PlayerPrefs.SetString("SaveNames", saveNames);
        }

        private void LoadSaveNameList()
        {
            if (PlayerPrefs.HasKey("SaveNames"))
            {
                string saveNames = PlayerPrefs.GetString("SaveNames");
                saveNameList = new List<string>(saveNames.Split(','));
            }
        }
    }

}