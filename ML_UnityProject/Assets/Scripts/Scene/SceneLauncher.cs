using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{    
    public class SceneLauncher : MonoBehaviour
    {
        public static SceneLauncher instance;

        public GameObject menuPanel;
        public GameObject scenePanel;

        void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(this);
                instance = this;

                scenePanel.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #region Button callbacks

        public void Quit()
        {
            Application.Quit();
        }

        public void LoadScene(Transform self)
        {
            LoadScene(self.name);
            menuPanel.SetActive(false);
            scenePanel.SetActive(true);
        }

        public void LoadMenu()
        {
            LoadScene(0);
            scenePanel.SetActive(false);
            menuPanel.SetActive(true);
        }

        public void LoadScene(int id) { SceneManager.LoadScene(id); }
        public void LoadScene(string id) { SceneManager.LoadScene(id); }

        #endregion
    }
}