using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterScript : MonoBehaviour
{

    // This is attached to every square in the game space, set up into a grid by the GameManager. Handles the info regarding what letter the box has storage wise as well as the visuals.

    public char? Letter { get; private set; } = null;
    public StateHandler stateHandler { get; private set; } = StateHandler.Default;

    public GameObject _default;
    public GameObject _close;
    public GameObject _correct;

    public Text LetterText = null;

    public void Update()
    {

    }

    public void Awake()
    {
        LetterText = GetComponentInChildren<Text>();
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
        LetterText = null;
    }
}
