using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterScript : MonoBehaviour
{
    // This is attached to every square in the game space, set up into a grid by the GameManager. Handles the info regarding what letter the box has storage wise as well as the visuals.

    [SerializeField] private char? Letter { get; set; } = null;
    [SerializeField] private StateHandler stateHandler { get; set; } = StateHandler.Default;

    public GameObject _default;
    public GameObject _close;
    public GameObject _correct;

    public Text LetterText = null;

    [SerializeField] private GameManager _gameManager;

    public void Awake()
    {
        LetterText = GetComponentInChildren<Text>();
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void Update()
    {
        switch (stateHandler)
        {
            case StateHandler.Default:
                _default.SetActive(true);
                _close.SetActive(false);
                _correct.SetActive(false);

                for (int i = 0; i < _gameManager.allButtons.Count; i++)
                {
                    if (_gameManager.allButtons[i]._letterChar.ToString().ToUpper() == LetterText.text)
                    {
                        _gameManager.allButtons[i].SetColor(0);
                    }
                }

                break;
            case StateHandler.WrongLocation:
                _default.SetActive(false);
                _close.SetActive(true);
                _correct.SetActive(false);

                for (int i = 0; i < _gameManager.allButtons.Count; i++)
                {
                    if (_gameManager.allButtons[i]._letterChar.ToString().ToUpper() == LetterText.text)
                    {
                        _gameManager.allButtons[i].SetColor(1);
                    }
                }

                break;

            case StateHandler.Correct:
                _default.SetActive(false);
                _close.SetActive(false);
                _correct.SetActive(true);

                for (int i = 0; i < _gameManager.allButtons.Count; i++)
                {
                    if (_gameManager.allButtons[i]._letterChar.ToString().ToUpper() == LetterText.text)
                    {
                        _gameManager.allButtons[i].SetColor(2);
                    }
                }

                break;
        }
    }

    public void InputLetter(char c)
    {
        Letter = c;
        LetterText.text = c.ToString().ToUpper();
    }

    public void DeleteLetter()
    {
        Letter = null;
        LetterText.text = null;
    }

    public void SetLetterState(StateHandler state)
    {
        stateHandler = state;
    }

    public void ClearAll()
    {
        stateHandler = StateHandler.Default;
        Letter = null;
        LetterText.text = null;
    }
}
