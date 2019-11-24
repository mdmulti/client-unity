using MDMulti.Net;
using System;
using System.Text;
using UnityEngine;

namespace MDMulti_DEBUG.DebugScene
{
    public class SHA2 : MonoBehaviour
    {
        string dataToHash = "This is a SHA256 test.";
        byte[] dataToHashB { get { return Encoding.UTF8.GetBytes(dataToHash); } }

        string expectedHash = "4816482f8b4149f687a1a33d61a0de6b611364ec0fb7adffa59ff2af672f7232".ToUpper();

        public void Test()
        {
            Debug.LogWarning("----- Starting SHA2 Test -----");


            // Test Phase 1: Creation and validation
            Debug.LogWarning("----- PHASE 1: CREATION + VALIDATION -----");

            Debug.Log("Hashing string '" + dataToHash + "'");

            byte[] combined = Core.GenerateAndAddHash(dataToHashB);

            string gotHash = Core.GetHash(combined);

            Debug.Log("Got hash " + gotHash);
            Debug.Log("Expected hash " + expectedHash);

            if (gotHash == expectedHash)
            {
                Debug.Log("Hashes match!");
            } else
            {
                Debug.LogError("Hashes do not match!");
            }


            // Test Phase 2: Extraction
            Debug.LogWarning("----- PHASE 2: EXTRACTION -----");

            Debug.Log("Extracting string and hash...");

            Tuple<byte[], string> split = Core.SplitHashAndMessage(combined);

            string extractedMessage = Encoding.UTF8.GetString(split.Item1);

            Debug.Log("Extracted message '" + extractedMessage + "'");
            Debug.Log("Extracted hash " + split.Item2);

            if (dataToHash == extractedMessage)
            {
                Debug.Log("Messages match!");
            } else
            {
                Debug.LogError("Extracted message did not match original!");
            }

            if (expectedHash == split.Item2)
            {
                Debug.Log("Hashes match!");
            } else
            {
                Debug.LogError("Extracted hash did not match original!");
            }
            

            // End of test
            Debug.LogWarning("----- SHA2 Test Finished! -----\nPlease check logs for possible errors.");
        }
    }
}