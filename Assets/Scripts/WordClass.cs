using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordClass : MonoBehaviour
{
    public static WordClass Instance { get; private set; }
    [SerializeField] private TextAsset _allWords;
    [SerializeField] private TextMeshProUGUI _wordSpoiler;

    public List<string> _wordList;
    public string wordToBeGuessed;

    void Start()
    {
        //Removes empty entries in the attached Text File.
        _wordList = new List<string>(_allWords.text.Split(new char[] { ',', ' ', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries));

        Retry();

        wordToBeGuessed = "TROPE";
    }

    public void Update()
    {
        _wordSpoiler.text = wordToBeGuessed;
    }

    public void Retry()
    {
        wordToBeGuessed = GenerateNewWord();
    }

    public string GenerateNewWord()
    {
        return _wordList[Random.Range(0, _wordList.Count)];
    }

    public bool CheckWord(string word)
    {
        return _wordList.Contains(word);
    }

}
