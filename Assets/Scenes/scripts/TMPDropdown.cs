using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TMPDropdownWithValues : MonoBehaviour
{
    public TMP_Dropdown tmpDropdown; // Reference to the TMP_Dropdown component
    public TMP_Text OnlinePlayersText; // Reference to the TMP_Text component for displaying online players

    // A dictionary to associate each option text with a corresponding value
    private Dictionary<int, string> optionValues = new Dictionary<int, string>();

    void Start()
    {
        if (tmpDropdown != null)
        {
            // Clear existing options at the start
            tmpDropdown.ClearOptions();

            // Add listener for when the value changes
            tmpDropdown.onValueChanged.AddListener(delegate {
                TMPDropdownValueChanged(tmpDropdown);
            });
        }
    }

    public void UpdateOnlinePlayers(string playerList)
    {
        // Update the OnlinePlayersText with the raw list
        OnlinePlayersText.text = "Online Players:\n" + playerList;

        // Split the player list into an array of player names
        string[] players = playerList.Split(',');

        // Clear existing options in the dropdown
        tmpDropdown.ClearOptions();
        optionValues.Clear(); // Clear the existing dictionary to reset it

        // Convert the string array to a list of TMP_Dropdown options
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < players.Length; i++)
        {
            // Add each player as a dropdown option
            dropdownOptions.Add(new TMP_Dropdown.OptionData(players[i]));

            // Associate each player name with a corresponding value in the dictionary
            optionValues.Add(i, players[i]); // Using player name as both key and value
        }

        // Add the new options to the dropdown
        tmpDropdown.AddOptions(dropdownOptions);

        // Optionally set the default value (index-based)
        tmpDropdown.value = 0;

        // Refresh the shown value
        tmpDropdown.RefreshShownValue();
    }

    void TMPDropdownValueChanged(TMP_Dropdown change)
    {
        // Get the associated value from the dictionary
        string selectedValue;
        if (optionValues.TryGetValue(change.value, out selectedValue))
        {
            Debug.Log("Selected Text: " + change.options[change.value].text + ", Associated Value: " + selectedValue);
        }
    }
}
