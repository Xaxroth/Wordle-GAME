using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] const int _numberOfLettersPerWord = 5;
    [SerializeField] private int _numberOfRows = 5;
    [SerializeField] private int _currentRow = 1;

    [SerializeField] private int _letterNumber;

    [SerializeField] private int index = 0;
    [SerializeField] private int currentRow = 0;

    [SerializeField] private LetterScript _letterBoxPrefab;
    [SerializeField] private GridLayoutGroup _grid = null;
    [SerializeField] private WordClass _wordClass;

    List<LetterScript> allLetters = null;

    char?[] wordGuess = new char?[_numberOfLettersPerWord];

    [SerializeField] private bool _allLettersCorrect;
    [SerializeField] private bool _gameOver;

    [SerializeField] private AudioSource _managerAudioSource;

    [SerializeField] private AudioClip _wrongWord;
    [SerializeField] private AudioClip _victory;
    [SerializeField] private AudioClip _insufficientLetters;
    [SerializeField] private AudioClip _lose;

    [SerializeField] private GameObject ErrorMessageWordTooShort;
    [SerializeField] private GameObject VictoryScreen;
    [SerializeField] private GameObject LoseScreen;

    void Awake()
    {
        InitializeGame();
        ErrorMessageWordTooShort.SetActive(false);
        VictoryScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.anyKeyDown && !_gameOver)
        {
            TranslateInput(Input.inputString);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResetGame();
        }
    }

    public void TranslateInput(string keyInput)
    {
        foreach(char c in keyInput)
        {
            if (c == '\b') // Backspace deletes the letter in the current slot
            {
                DeleteKey();
            }
            else
            {
                if (c != '\b' && c != '\n' && c != '\r') // As long as the input is not Enter, Return or Newline, we enter the key pressed
                {
                    EnterKey(c);
                }
                else
                {
                    if (index == _numberOfLettersPerWord) // If the index is at the last slot of the word (the word's total length), it will be a valid guess.
                    {
                        Guess();
                    }
                    else
                    {
                        StartCoroutine(ErrorMessage());
                        _managerAudioSource.PlayOneShot(_insufficientLetters);
                    }
                }
            }
        }
    }

    public void ManualTranslateInput()
    {
        if (index == _numberOfLettersPerWord) // If the index is at the last slot of the word (the word's total length), it will be a valid guess.
        {
            Guess();
        }
        else
        {
            StartCoroutine(ErrorMessage());
            _managerAudioSource.PlayOneShot(_insufficientLetters);
        }
    }

    private void InitializeGame()
    {
        if (allLetters == null)
        {
            allLetters = new List<LetterScript>();
        }

        for (int i = 0; i < _numberOfRows; i++) // Initilalizes rows, and then for each row it makes letter boxes according to the total length of the words.
        {
            for (int j = 0; j < _numberOfLettersPerWord; j++)
            {
                LetterScript letterBox = Instantiate<LetterScript>(_letterBoxPrefab);
                letterBox.transform.SetParent(_grid.transform);
                allLetters.Add(letterBox);
            }
        }
    }

    public void EnterKey(char c)
    {
        if (index < _numberOfLettersPerWord && !_gameOver) // As long as there are not 5 characters present, the player can input letters. For each letter entered it moves forward in the current row index.
        {
            c = char.ToUpper(c);

            allLetters[(currentRow * _numberOfLettersPerWord) + index].InputLetter(c);
            wordGuess[index] = c;
            index++;
        }
    }

    public void DeleteKey()
    {
        if (index > 0)
        {
            index--;
            allLetters[(currentRow * _numberOfLettersPerWord) + index].DeleteLetter();
            wordGuess[index] = null;
        }
    }

    public void Guess()
    {

        StringBuilder word = new StringBuilder();

        for (int i = 0; i < _numberOfLettersPerWord; i++)
        {
            word.Append(wordGuess[i]);
        }

        for (int i = 0; i < _numberOfLettersPerWord; i++)
        {
            bool correct = wordGuess[i] == _wordClass.wordToBeGuessed[i];

            if (!correct)
            {
                bool letterExists = false;
 
                for (int j = 0; j < _numberOfLettersPerWord; j++)
                {
                    letterExists = wordGuess[i] == _wordClass.wordToBeGuessed[j];

                    if (letterExists)
                    {
                        allLetters[i + (currentRow * _numberOfLettersPerWord)].SetLetterState(StateHandler.WrongLocation);
                        break;
                    }
                }
            }
            else
            {
                allLetters[i + (currentRow * _numberOfLettersPerWord)].SetLetterState(StateHandler.Correct);
            }
        }

        if (word.ToString().Equals(_wordClass.wordToBeGuessed) && _wordClass.wordToBeGuessed != null)
        {
            _managerAudioSource.PlayOneShot(_victory);
            VictoryScreen.SetActive(true);
            _gameOver = true;
        }
        else
        {
            _managerAudioSource.PlayOneShot(_wrongWord);
        }

        if (currentRow < _numberOfRows)
        {
            currentRow++;
            index = 0;
        }
        else
        {
            _managerAudioSource.PlayOneShot(_victory);
            return;
        }

        if (currentRow == _numberOfRows && !_gameOver)
        {
            _managerAudioSource.PlayOneShot(_lose);
            LoseScreen.SetActive(true);
            _currentRow = 0;
            _gameOver = true;
        }
    }

    private IEnumerator ErrorMessage()
    {
        ErrorMessageWordTooShort.SetActive(true);
        yield return new WaitForSeconds(2);
        ErrorMessageWordTooShort.SetActive(false);
    }

    public void ResetGame()
    {
        LoseScreen.SetActive(false);
        VictoryScreen.SetActive(false);

        _wordClass.Retry();

        for (int i = 0; i < allLetters.Count; i++) // Initilalizes rows, and then for each row it makes letter boxes according to the total length of the words.
        {
            allLetters[i].ClearAll();
            DeleteKey();
        }

        currentRow = 0;

        _gameOver = false;
    }
}
