using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGen : MonoBehaviour {

    public static string GenerateName(int len) {
        string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
        string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
        string Name = "";
        Name += consonants[Random.Range(0, (consonants.Length))].ToUpper();
        Name += vowels[Random.Range(0, (vowels.Length))];
        int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
        while (b < len) {
            Name += consonants[Random.Range(0, (consonants.Length))];
            b++;
            Name += vowels[Random.Range(0, (vowels.Length))];
            b++;
        }

        return Name;
    }

}
