using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Text _letter;
    [SerializeField] private string _letterText;

    void Start()
    {
        _letter = GetComponentInChildren<Text>();

        if (_letterText != null)
        {
            _letter.text = _letterText;
        }
    }

    void Update()
    {
        
    }
}
