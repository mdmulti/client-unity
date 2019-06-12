using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using Debug = UnityEngine.Debug;

namespace MDMulti
{
    public class Core
    {
        static string ServerUrl = "http://localhost:3000";
        static int ProtocolVersion = 1;

        static string ConstructUrl(string end)
        {
            return ServerUrl + "/" + end;
        }

        public static RequestResponse Get(string path)
        {
            UnityWebRequest www = UnityWebRequest.Get(ConstructUrl(path));
            www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                return new RequestResponse(ResponseTypes.ERR, www);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);

                return new RequestResponse(ResponseTypes.GET, www);
            }
        }

        public static RequestResponse Post(string path, List<IMultipartFormSection> formData)
        {
            UnityWebRequest www = UnityWebRequest.Post(ConstructUrl(path), formData);
            www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                return new RequestResponse(ResponseTypes.ERR, www);
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log(www.isDone);
                return new RequestResponse(ResponseTypes.POST, www);
            }
        }

        public enum ResponseTypes { GET, POST, ERR }

        public class RequestResponse
        {
            private UnityWebRequest request;
            private ResponseTypes type;

            public RequestResponse(ResponseTypes type, UnityWebRequest request)
            {
                this.type = type;
                this.request = request;
            }

            public ResponseTypes Type()
            {
                return type;
            }

            public long ResponseCode()
            {
                // error type checking
                if (type == ResponseTypes.ERR) return 0;

                return request.responseCode;
            }

            public string ResponseData()
            {
                // error type checking
                if (type == ResponseTypes.ERR) return null;

                return request.downloadHandler.text;
            }

            public int ProtocolVersion()
            {
                // error type checking
                if (type == ResponseTypes.ERR) return 0;

                string strProto = request.GetResponseHeader("X-MDM-Protocol-Version");
                int intProto = 0;
                int.TryParse(strProto, out intProto);
                return intProto;
            }

            public string ContentType()
            {
                // error type checking
                if (type == ResponseTypes.ERR) return null;

                return request.GetResponseHeader("Content-Type");
            }
        }
    }
}