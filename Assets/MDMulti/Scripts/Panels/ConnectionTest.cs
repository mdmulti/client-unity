using UnityEngine;
using UnityEngine.UI;

namespace MDMulti.Panels
{
    public class ConnectionTest : MonoBehaviour
    {
        public Image display;

        //public Image incompleteGraphic;
        public Sprite successfulGraphic;
        public Sprite failedGraphic;

        async void Start()
        {
            display.sprite = ((await RestHelper.ConnectionTest()) == true ? successfulGraphic : failedGraphic);
        }
    }

}