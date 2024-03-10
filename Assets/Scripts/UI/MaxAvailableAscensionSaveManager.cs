using System.IO;
using UnityEngine;

namespace FroguesFramework
{
    public class MaxAvailableAscensionSaveManager : MonoBehaviour
    {
        private string _dataPath = Application.dataPath + "/save.txt";

        public void TryToLoadInfo()
        {
            if(!File.Exists(_dataPath)) 
            {
                File.WriteAllText(_dataPath, 0.ToString());
            }

            var fileInfo = File.ReadAllLines(_dataPath);
            int maxAvailableAscensionInFile = int.Parse(fileInfo[0]);

            if(maxAvailableAscensionInFile > MaxAvailavleAscension.indexOfMaxAbailableAscension)
            {
                MaxAvailavleAscension.indexOfMaxAbailableAscension = maxAvailableAscensionInFile;
            }
        }

        public void TryToSaveInfo()
        {
            var fileInfo = File.ReadAllLines(_dataPath);
            int maxAvailableAscensionInFile = int.Parse(fileInfo[0]);

            if (maxAvailableAscensionInFile > CurrentAscention.ascensionSetup.index)
                return;

            int newMaxAscention = Mathf.Clamp(CurrentAscention.ascensionSetup.index + 1, 0, 8);
            File.WriteAllText(_dataPath, (newMaxAscention).ToString());
        }
    }
}