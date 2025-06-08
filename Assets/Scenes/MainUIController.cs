using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.UI
{
    /// <summary>
    /// A simple controller for switching between UI panels.
    /// </summary>
    public class MainUIController : MonoBehaviour
    {
        public GameObject[] panels;

        public void SetActivePanel(int index)
        {
            panels[index].SetActive(true);

        }
        /*
        void OnEnable()
        {
            SetActivePanel(0);
            SetDeactivePanel
        }
        */
        public void SetDeactivePanel(int index)
        {
           
                    panels[index].SetActive(false);
            
        }
    }
}