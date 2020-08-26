using UnityEngine;
using UnityEngine.UI;
 
public class JetBar : MonoBehaviour
{
    private static Image JetBarImage;
    private RectTransform transform2D;
    private float startingWidth;

    // SIKK HTML TAGS BRO
    /// <summary>
    /// Sets the health bar value
    /// </summary>
    /// <param name="value">should be between 0 to 1</param>
    public void SetJetBarValue(float value)
    {   
        JetBarImage.fillAmount = value;
        transform2D.sizeDelta = new Vector2(startingWidth*value, transform2D.rect.height);
    }

    public float GetJetBarValue()
    {
        //runtime error from this line idk why
        return JetBarImage.fillAmount;
        //return 0;
    }

    /// <summary>
    /// Sets the health bar color
    /// </summary>
    /// <param name="healthColor">Color </param>
    public static void SetHealthBarColor(Color healthColor)
    {
        JetBarImage.color = healthColor;
    }

    /// <summary>
    /// Initialize the variable
    /// </summary>
    private void Start()
    {   
        JetBarImage = GetComponent<Image>();
        transform2D = GetComponent<RectTransform>();
        startingWidth = transform2D.rect.width;
    }
}