using UnityEngine;
using System.Text;

public class String_Generator
{
    public static string generateRandomString()
    {
        //隨機產生數字，是我們等等字串repeat的次數。
        int repeatNumber = Random.Range(1, 20);
        //StringBuilder可以將字串重複。
        return new StringBuilder("Text".Length * repeatNumber).Insert(0, "Text", repeatNumber).ToString(); ;
    }
}
