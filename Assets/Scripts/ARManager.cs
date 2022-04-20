using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ARManager : MonoBehaviour {
    [SerializeField] PlaceDevice device;
    [SerializeField] GameObject[] stepsUI;
    int _index = 0;
    public void NextButton() {
        foreach(var obj in device.demoObjs)
            obj.SetActive(false);

        if(_index < stepsUI.Length - 1) {
            stepsUI[_index].SetActive(false);
            _index++;
            stepsUI[_index].SetActive(true);
        }

        if(_index == 2)
            device.demoObjs[0].SetActive(true);
        else if(_index == 5)
            device.demoObjs[1].SetActive(true);
    }
    public void BackButton() {
        foreach(var obj in device.demoObjs)
            obj.SetActive(false);

        if(_index > 0) {
            stepsUI[_index].SetActive(false);
            _index--;
            stepsUI[_index].SetActive(true);
        }

        if(_index == 2)
            device.demoObjs[0].SetActive(true);
        else if(_index == 5)
            device.demoObjs[1].SetActive(true);
    }
}
