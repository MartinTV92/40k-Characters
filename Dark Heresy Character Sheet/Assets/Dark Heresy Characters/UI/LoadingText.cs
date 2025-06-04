using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    public TextMeshProUGUI dotText;
    public float updateInterval = 0.75f;
    float nextUpdate;
    int dotNum = 0;

	private void OnEnable()
	{
		dotText.text = "";
        dotNum = 0;
        nextUpdate = Time.time + updateInterval;
	}

	void Update()
    {
		UpdateLoadingDots();
	}

    void UpdateLoadingDots()
    {
        if(Time.time < nextUpdate)
            return;

        nextUpdate = Time.time + updateInterval;
        dotNum = (int) Mathf.Repeat(++dotNum, 4);
        dotText.text = new ('.', dotNum);
	}
}
