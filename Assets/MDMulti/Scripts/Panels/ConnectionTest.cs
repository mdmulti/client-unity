using UnityEngine;
using UnityEngine.UI;

namespace MDMulti.Panels
{
    public class ConnectionTest : MonoBehaviour
    {
        public Image display;

        public GameObject animatedLoadingCircle;

        //public Image incompleteGraphic;
        public Sprite successfulGraphic;
        public Sprite failedGraphic;

        async void Start()
        {
            await new WaitForSeconds(1);
            bool active = await RestHelper.ConnectionTest();

            // Hide the loading circle
            animatedLoadingCircle.SetActive(false);

            // Enable the White Circle Image / Mask
            GetComponent<Image>().enabled = true;

            // Enable the result image
            display.enabled = true;

            // Set the result image
            display.sprite = (active == true ? successfulGraphic : failedGraphic);

            // Enable the built-in button to restart
            GetComponent<Button>().enabled = true;
        }

        public void Restart()
        {
            // Disable the White Circle Image / Mask
            GetComponent<Image>().enabled = false;

            // Disable the result image
            display.enabled = false;

            // Activate the loading circle
            animatedLoadingCircle.SetActive(true);

            // Disable the restart button
            GetComponent<Button>().enabled = false;

            // Run the start script
            Start();
        }
    }

}