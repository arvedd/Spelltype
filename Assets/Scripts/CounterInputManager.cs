using UnityEngine;

public class CounterInputManager : MonoBehaviour
{
    void Update()
    {
        if (PauseManager.IsPaused) return;

        if (Input.anyKeyDown)
        {
            string input = Input.inputString.ToLower();

            // ignore non-letter keys
            if (string.IsNullOrEmpty(input) || !char.IsLetter(input[0]))
                return;

            char typedChar = input[0];

            // send typed char to all active enemy attacks
            foreach (var attack in FindObjectsByType<Attack>(FindObjectsSortMode.None))
            {
                if (attack.caster == Caster.Enemy)
                {
                    attack.TryCounterProgressive(typedChar);
                }
            }
        }
    }
}