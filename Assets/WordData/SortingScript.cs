using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingScript : MonoBehaviour
{
    public int[] integers;

    private void Start()
    {
        for (int i = 0; i < integers.Length; i++)
        {
            integers[i] = Random.Range(0, 100);
        }

        BubbleSort(integers);
    }

    [ContextMenu("Sort")]
    public void Sort(int[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            int min = i;
            Debug.Log("Checked i");

            for (int j = i + 1; j < input.Length; j++)
            {
                if (input[j] < input[i])
                {
                    Debug.Log("Input i greater than input j");
                    min = j;
                }
            }

            int temp = input[i];

            input[i] = input[min];
            input[min] = temp;
        }
    }

    public void BubbleSort(int[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input.Length - i - 1; j++)
            {
                if (input[j] > input[j + 1])
                {
                    int temp = input[j]; // grabs the leftwise element
                    input[j] = input[j + 1]; // sets the leftwise element to be equal to the next entry in the array
                    input[j + 1] = temp; // sets the entry after that to be the bigger value
                }
            }
        }


    }
}
