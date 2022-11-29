using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Text _letter;
    [SerializeField] private string _letterText;

    [SerializeField] private char[] _allChars;

    [SerializeField] public char _letterChar;

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private WordClass _wordClass;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _gameManager.allButtons.Add(this);
    }

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

    public void SetColor(int value)
    {
        switch (value)
        {
            case 0:
                gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                break;
            case 1:
                gameObject.GetComponent<Image>().color = new Color32(255, 220, 90, 255);
                break;
            case 2:
                gameObject.GetComponent<Image>().color = new Color32(80, 255, 70, 255);
                break;
        }
    }
}
