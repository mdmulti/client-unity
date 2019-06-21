﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using Debug = UnityEngine.Debug;

namespace MDMulti
{
    /// <summary>REST class for getting data from the server.</summary>
    public class Rest
    {
        static readonly string ServerUrl = "http://localhost:3000";
        static readonly int ProtocolVersion = 1;

        static string ConstructUrl(string end)
        {
            return ServerUrl + "/" + end;
        }

        public static IEnumerator Get(string path, Action<RequestResponse> onComplete)
        {
            UnityWebRequest www = UnityWebRequest.Get(ConstructUrl(path));
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                onComplete(new RequestResponse(ResponseTypes.ERR, www));
            }
            else
            {
                onComplete(new RequestResponse(ResponseTypes.GET, www));
            }
        }

        public static IEnumerator Post(string path, List<IMultipartFormSection> formData, Action<RequestResponse> onComplete)
        {
            UnityWebRequest www = UnityWebRequest.Post(ConstructUrl(path), formData);
            www.timeout = 10;
            Debug.Log(www.timeout);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("ERROR");
                onComplete(new RequestResponse(ResponseTypes.ERR, www));
            }
            else
            {
                onComplete(new RequestResponse(ResponseTypes.POST, www));
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