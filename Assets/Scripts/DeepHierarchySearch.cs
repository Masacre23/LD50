using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeepHierarchySearch
{
  public static GameObject SearchAll(GameObject g, string n)
    {
        for (int i = 0; i < g.transform.childCount; i++)
        {

            if (g.transform.GetChild(i).transform.name == n)
            {

                return g.transform.GetChild(i).gameObject;

            }
            else if (g.transform.GetChild(i).childCount > 0)
            {
 
                GameObject aux = SearchAll(g.transform.GetChild(i).gameObject, n);
                if (aux != null)
                {
                    return aux;
                }
            }
        }
        return null;
    }


}
