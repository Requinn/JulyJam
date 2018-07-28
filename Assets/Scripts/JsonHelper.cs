using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used to assist in serializing lists and arrays
/// Source: https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
/// </summary>
public static class JsonHelper {
    [Serializable]
    private class Wrapper<T>{
        public List<T> items;
    }

    /// <summary>
    /// Read a string from a json file into the original type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static List<T> FromJson<T>(string json){
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }

    /// <summary>
    /// Takes list and wraps into a jason
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static string ToJson<T>(List<T> array){
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper);
    }

}
