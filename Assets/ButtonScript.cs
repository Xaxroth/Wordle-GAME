using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Text _letter;
    [SerializeField] private string _letterText;

    [SerializeField] public char[] _allChars;

    [SerializeField] public char _letterChar;

    [SerializeField] private GameManager _gameManager;

    void Start()
    {
        _letter = GetComponentInChildren<Text>();

        if (_letterText != null)
        {
            _letter.text = _letterText;
        }

        _allChars = _letterText.ToCharArray();

        _letterChar = _allChars[0];
    }

    public void ManualCharacterInput()
    {
        _gameManager.EnterKey(_letterChar);
    }

}
