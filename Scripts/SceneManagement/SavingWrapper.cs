using UnityEngine;
using RR.Saving;

namespace RR.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
    }
}