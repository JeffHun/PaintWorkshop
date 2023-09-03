using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHeatMap : MonoBehaviour
{
    public List<GameObject> HeatMaps;
    public List<GameObject> Stickers;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bumper"))
        {
            HeatMaps[0].SetActive(true);
            Stickers[0].SetActive(true);
            Stickers[1].SetActive(false);
            Stickers[2].SetActive(false);
        }
        if (other.gameObject.CompareTag("Hood"))
        {
            HeatMaps[1].SetActive(true);
            Stickers[0].SetActive(false);
            Stickers[1].SetActive(true);
            Stickers[2].SetActive(false);
        }
        if (other.gameObject.CompareTag("Door"))
        {
            HeatMaps[2].SetActive(true);
            Stickers[0].SetActive(false);
            Stickers[1].SetActive(false);
            Stickers[2].SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HeatMaps[0].SetActive(false);
        HeatMaps[1].SetActive(false);
        HeatMaps[2].SetActive(false);
    }
}
