using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LaunchARCamera : MonoBehaviour
{
    public GameObject welcomePanel;
    public GameObject phoneMovePanel;
    public Button arLaunch;

    // Start is called before the first frame update
    public void Awake()
    {
        phoneMovePanel.SetActive(false);
        arLaunch.onClick.AddListener(Dismiss);
        arLaunch.onClick.AddListener(Intro);
    }

    private void Dismiss() => welcomePanel.SetActive(false);
    private void Intro() => phoneMovePanel.SetActive(true);

    // Update is called once per frame
    void Update()
    {
        
    }
}
