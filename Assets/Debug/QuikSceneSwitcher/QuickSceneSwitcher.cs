﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace MDMulti_DEBUG
{
    public class QuickSceneSwitcher : MonoBehaviour
    {
        private int getCurrent()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

        private void openIfValid(int scene)
        {
            if ((uint)scene <= SceneManager.sceneCountInBuildSettings - 1) {
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }

        }

        public void Next()
        {
            openIfValid(getCurrent() + 1);
        }

        public void Prev()
        {
            openIfValid(getCurrent() - 1);
        }


    }
}
