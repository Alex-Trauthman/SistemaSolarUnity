using UnityEngine;
using UnityEngine.UI;

public class PlanetInfo : MonoBehaviour
{
    // Este é o texto único para este planeta
    public string planetDescription;

    // Opcional: método para exibir as informações diretamente
    public void DisplayInfo(Text infoText)
    {
        infoText.text = planetDescription;
    }
}
