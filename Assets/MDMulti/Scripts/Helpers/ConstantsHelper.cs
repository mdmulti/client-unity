using UnityEngine;
using MDMulti.Constants;

namespace MDMulti
{
    public class ConstantsHelper
    {
        /// <summary>
        /// Get the Constants Object.
        /// </summary>
        public static ConstantsObject Get()
        {
            string data = Resources.Load<TextAsset>("constants").text;
            return ConstantsObject.FromJson(data);
        }
    }
}