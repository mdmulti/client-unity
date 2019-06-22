using System.Collections.Generic;
using UnityEngine.Networking;

using Debug = UnityEngine.Debug;

namespace MDMulti
{
    public class User
    {
        public static void Create(string displayName)
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("displayName=" + displayName));


            if (Mono.Main.InScene())
            {
                Mono.Main.Inst.StartCoroutine(Rest.Post("users/create", formData, res =>
                {
                    Debug.Log("RES TYPE: " + res.Type());
                    Debug.Log("RES CODE: " + res.ResponseCode());
                    Debug.Log("RES PROTO: " + res.ProtocolVersion());
                    Debug.Log("RES DATA: " + res.ResponseData());
                }));
            }
        }
    }
}