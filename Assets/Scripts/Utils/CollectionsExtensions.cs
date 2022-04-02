using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionsExtensions
{
    public static void RemoveAll(this ArrayList collection, ICollection toRemove)
    {
        foreach (object element in toRemove)
        {
            collection.Remove(element);
        }
    }

    public static bool NotContainsKey<T, R>(this Dictionary<T, R> self, T key) { return !self.ContainsKey(key); }

    public static T Random<T>(this List<T> self)
    {
        return self[UnityEngine.Random.Range(0, self.Count - 1)];
    }

    public static List<T> Filter<T>(this List<T> self, Func<T, bool> condition)
    {
        List<T> ret = new List<T>();
        foreach (T element in self)
        {
            if (condition(element))
            {
                ret.Add(element);
            }
        }

        return ret;
    }

    public static T[] Filter<T>(this T[] self, Func<T, bool> condition)
    {
        T[] ret = new T[self.Length];
        foreach (T element in self)
        {
            if (condition(element))
            {
                ret[Array.IndexOf(self, element)] = element;
            }
        }

        return ret;
    }

    public static List<T> RemoveAll<T>(this List<T> self, Func<T, bool> condition)
    {
        List<T> ret = self;
        foreach (T element in self)
        {
            if (condition(element))
            {
                ret.Remove(element);
            }
        }
        self = ret;
        return self;
    }


    public static T MinBy<T>(this List<T> self, Func<T, float> condition)
    {
        T ret = default;
        if (self.IsNotEmpty()) ret = self.First();
        foreach (T element in self)
        {
            if (condition(element) < condition(ret))
            {
                ret = element;
            }
        }

        return ret;
    }

    public static T First<T>(this T[] self, Func<T, bool> condition) { return self.Filter(condition).First(); }
    public static T First<T>(this List<T> self, Func<T, bool> condition) { return self.Filter(condition).First(); }

    public static bool IsEmpty<T>(this T[] self)
    {
        return self.Length == 0;
    }
    public static bool IsNotEmpty<T>(this T[] self)
    {
        return !self.IsEmpty();
    }

    public static bool IsEmpty<T>(this List<T> self)
    {
        return self.Count == 0;
    }
    public static bool IsNotEmpty<T>(this List<T> self)
    {
        return !self.IsEmpty();
    }

    public static bool Contains<T>(this List<T> self, T elementToContain)
    {

        foreach (T element in self)
        {
            if (element.Equals(elementToContain))
                return true;
        }

        return false;
    }

    public static bool NotContains<T>(this List<T> self, T elementToContain)
    {
        return !self.Contains(elementToContain);
    }

    public static T First<T>(this T[] self) { return self[0]; }
    public static T First<T>(this List<T> self) { return self[0]; }
    public static T Second<T>(this List<T> self) { return self[1]; }
    public static T Third<T>(this List<T> self) { return self[2]; }

    public static T Last<T>(this List<T> self) { return self[self.Count - 1]; }

    public static void RemoveFirst<T>(this List<T> self) { self.RemoveAt(0); }

    public static void AddAll<T>(this List<T> self, List<T> list)
    {
        foreach (T element in list)
        {
            self.Add(element);
        }
    }

    public static void ReplaceAll<T>(this List<T> self, List<T> list)
    {
        self.Clear();
        self.AddAll(list);
    }

    public static void ForeachIndexed<T>(this T[] self, Action<int, T> action)
    {
        int index = 0;
        foreach (T element in self)
        {
            action(index, element);
            index++;
        }
    }

    public static void ForeachIndexed<T>(this List<T> self, Action<int, T> action)
    {
        int index = 0;
        foreach (T element in self)
        {
            action(index, element);
            index++;
        }
    }

    public static bool Any<T>(this List<T> self, Func<T, bool> condition)
    {
        foreach (var element in self)
        {
            if (condition(element))
                return true;
        }
        return false;
    }
}
