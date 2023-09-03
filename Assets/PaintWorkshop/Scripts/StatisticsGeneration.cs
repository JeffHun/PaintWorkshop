using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatisticsGeneration : MonoBehaviour
{
    public GameObject Panel;

    public TextMeshProUGUI CoveredTxt;
    public Slider CoveredSld;

    public TextMeshProUGUI CorrectTxt;
    public Slider CorrectSld;

    public TextMeshProUGUI InorrectTxt;
    public Slider IncorrectSld;

    public TextMeshProUGUI QuantityTxt;
    public List<RawImage> Drops = new List<RawImage>();

    public TextMeshProUGUI TimeTxt;
    public List<RawImage> Times = new List<RawImage>();

    public TextMeshProUGUI ScoreTxt;

    public float QuantityPaintSprayedPerSec;

    GameObject _currentPart = null;
    Texture2D _heatMapTexture;
    bool _isSprayActive;
    float _timer;

    public List<GameObject> HeatMaps;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bumper"))
        {
            _currentPart = other.gameObject;
            _heatMapTexture = HeatMaps[0].transform.GetComponent<Renderer>().material.mainTexture as Texture2D;
        }

        if (other.gameObject.CompareTag("Hood"))
        {
            _currentPart = other.gameObject;
            _heatMapTexture = HeatMaps[1].transform.GetComponent<Renderer>().material.mainTexture as Texture2D;
        }

        if (other.gameObject.CompareTag("Door"))
        {
            _currentPart = other.gameObject;
            _heatMapTexture = HeatMaps[2].transform.GetComponent<Renderer>().material.mainTexture as Texture2D;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bumper") || other.gameObject.CompareTag("Hood") || other.gameObject.CompareTag("Door"))
        {
            _currentPart = null;
            Panel.SetActive(true);
        }
    }

    public void StatisticalGeneration()
    {
        if(_currentPart != null)
        {
            Panel.SetActive(false);
            _currentPart.transform.parent.GetChild(0).gameObject.SetActive(false);
            _currentPart.transform.parent.GetChild(2).gameObject.SetActive(true);

            float surface = 0;
            float colored = 0;
            float green = 0;
            float red = 0;

            for (int j = 0; j < _heatMapTexture.width; j++)
            {
                for (int k = 0; k < _heatMapTexture.height; k++)
                {
                    if (_heatMapTexture.GetPixel(j, k) != Color.black)
                    {
                        surface++;
                        if (_heatMapTexture.GetPixel(j, k).b < 0.99)
                        {
                            colored++;
                        }
                        if (_heatMapTexture.GetPixel(j, k).b < .75 && _heatMapTexture.GetPixel(j, k).b >= 0 && _heatMapTexture.GetPixel(j, k).g >= 0.99)
                        {
                            green++;
                        }
                        if (_heatMapTexture.GetPixel(j, k).g < 0.99)
                        {
                            red++;
                        }
                    }
                }
            }
            if(colored == 0)
            {
                SetCoveredUIValue(colored / surface);
                SetCorrectUIdValue(0);
                SetIncorrectUIValue(0);
                SetQuantityUIValue(_timer * QuantityPaintSprayedPerSec);
                SetTimeUIValue(_timer);
                SetScorreUIValue(colored / surface, 0, 0, 0f, 0f);
            }
            else
            {
                SetCoveredUIValue(colored / surface);
                SetCorrectUIdValue(green / colored);
                SetIncorrectUIValue(red / colored);
                SetQuantityUIValue(_timer * QuantityPaintSprayedPerSec);
                SetTimeUIValue(_timer);
                SetScorreUIValue(colored / surface, green / colored, red / colored, _timer * QuantityPaintSprayedPerSec, _timer);
            }
            _timer = 0;
        }
    }

    public void SetSprayState(bool state)
    {
        _isSprayActive = state;
    }

    private void Update()
    {
        if (_isSprayActive)
            _timer += Time.deltaTime;
    }

    void SetCoveredUIValue(float value)
    {
        CoveredTxt.text = Mathf.RoundToInt(value*100).ToString() + "%";
        CoveredSld.value = value;
    }

    void SetCorrectUIdValue(float value)
    {
        CorrectTxt.text = Mathf.RoundToInt(value*100).ToString() + "%";
        CorrectSld.value = value;
    }

    void SetIncorrectUIValue(float value)
    {
        InorrectTxt.text = Mathf.RoundToInt(value*100).ToString() + "%";
        IncorrectSld.value = value;
    }

    void SetQuantityUIValue(float value)
    {
        QuantityTxt.text = value.ToString() + "g";
        for(int i = 1; i < Drops.Count+1; i++)
        {
            if(value / i > 10)
                Drops[i-1].GetComponent<RawImage>().color = new Color32(255, 255, 255, 255);
            else
                Drops[i-1].GetComponent<RawImage>().color = new Color32(255, 255, 255, 20);
        }
    }

    void SetTimeUIValue(float value)
    {
        TimeTxt.text = ((int)value/60).ToString()+":"+((int)value%60).ToString();
        for (int i = 1; i < Drops.Count+1; i++)
        {
            if (((int)value / i) > 30)
                Times[i-1].GetComponent<RawImage>().color = new Color32(255, 255, 255, 255);
            else
                Times[i-1].GetComponent<RawImage>().color = new Color32(255, 255, 255, 20);
        }
    }

    void SetScorreUIValue(float coveredValue, float correctValue, float incorrectValue, float quantityValue, float timeValue)
    {
        float score = ((coveredValue*100) * 3) + ((correctValue*100) * 2) - ((incorrectValue*100) * 3) - (quantityValue * 0.001f) - (timeValue * 0.75f);

        if (score >= 400)
        {
            ScoreTxt.text = "A";
            ScoreTxt.color = new Color32(0, 183, 0, 255);
            return;
        }
        if (score >= 300)
        {
            ScoreTxt.text = "B";
            ScoreTxt.color = new Color32(0, 183, 0, 255);
            return;
        }
        if (score >= 200)
        {
            ScoreTxt.text = "C";
            ScoreTxt.color = new Color32(183, 100, 0, 255);
            return;
        }
        if (score >= 100)
        {
            ScoreTxt.text = "D";
            ScoreTxt.color = new Color32(183, 100, 0, 255);
            return;
        }
        if (score < 100)
        {
            ScoreTxt.text = "E";
            ScoreTxt.color = new Color32(183, 0, 0, 255);
            return;
        }
    }
}
