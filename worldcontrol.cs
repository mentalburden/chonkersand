using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine.UI;

public class WorldControl : MonoBehaviour
{
    public int boundarysize;
    public List<particletype> pt;
    public GameObject[,] particleobjects;
    public GameObject testobj;

    void Start()
    {
        pt = new List<particletype>();
        particleobjects = new GameObject[boundarysize, boundarysize];
        for (int i=0, x = 0; x <= boundarysize; x++)
        {
            for (int z = 0; z <= boundarysize; z++)
            {
                int coinflip = getrand(1, 2);
                if (coinflip == 1)
                {
                    pt.Add(new particletype(i, x, z, 0, true));
                }
                if (coinflip == 2)
                {
                    pt.Add(new particletype(i, x, z, 0, false));
                }
                i++;
                Debug.Log(i.ToString());
            }
        }
    }

    public particletype GetItem(int x, int z)
    {
        return pt.Find(chonk => chonk.px == x && chonk.pz == z);
    }

    public void applychild(int x, int z)
    {
        try
        {
            Vector3 thispos = new Vector3(x, 0, z);
            GameObject myobj = Instantiate(testobj, thispos, Quaternion.identity);
            particleobjects[x, z] = myobj;
        }
        catch
        {

        }
    }

    public void destroychild(int x, int z)
    {
        try
        {
            Destroy(particleobjects[x, z].gameObject);
            Debug.Log("destroyed object at " + x.ToString() + " " + z.ToString());
        }
        catch
        {

        }
    }    

    public static int getrand(int min, int max)
    {
        if (min >= max)
        {
            return min;
        }
        byte[] intBytes = new byte[4];
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetNonZeroBytes(intBytes);
        }
        return min + Math.Abs(BitConverter.ToInt32(intBytes, 0)) % (max - min + 1);
    }

    public void runsolidkernel(int x, int z)
    {
        particletype me = GetItem(x, z);
        particletype below = GetItem(x, z - 1);
        particletype belowleft = GetItem(x - 1, z - 1);
        particletype belowright = GetItem(x + 1, z - 1);
        if (me != null && below != null && belowleft != null && belowright != null)
        {
            if (below.pstate == false)
            {
                me.pz -= 1;
                below.pz += 1;
                //below.pstate = false;
                return;
            }
            else if (belowleft.pstate == false && belowright.pstate == true && below.pstate == true)
            {
                me.pz -= 1;
                me.px -= 1;
                belowleft.pz += 1;
                belowleft.px += 1;
                return;
            }
            else if (belowleft.pstate == true && belowright.pstate == false && below.pstate == true)
            {
                me.pz -= 1;
                me.px += 1;
                belowright.pz += 1;
                belowright.px -= 1;
                return;
            }
        }
        else if (x == 0)
        {
            me.pstate = false;
            return;
        }
    }

    void Update()
    {
        //calc
        for (int x = 0; x < boundarysize; x++)
        {
            for (int z = 0; z < boundarysize; z++)
            {                
                //run the neighbor operation and set the coords
                runsolidkernel(x, z);
                //run the draw or instantiate afterwards
                if (GetItem(x,z).pstate == true)
                {
                    applychild(x, z);
                }
                else if (GetItem(x, z).pstate == false)
                {
                    destroychild(x, z);
                }
            }
        }
    }
}



































//public int gatherneighbors(int x, int z)
//{
//    try
//    {
//        // 6 7 8
//        // 4 X 5
//        // 1 2 3
//        //---------------------------------------
//        // - - -
//        // 0 x 0  state 1
//        // 0 x 0
//        //---------------------------------------
//        // - - -
//        // 0 x 0  state 2
//        // x x x
//        //---------------------------------------
//        // - - -
//        // 0 x 0  state 3
//        // x x 0

//        bool n1 = GetItem(x - 1, z - 1).pstate;
//        bool n2 = GetItem(x, z - 1).pstate;
//        bool n3 = GetItem(x + 1, z - 1).pstate;
//        bool n4 = GetItem(x - 1, z).pstate;
//        bool n5 = GetItem(x + 1, z).pstate;
//        if (n2 == false)
//        {
//            return 0;
//        }
//        else if (n2 == true)
//        {
//            return 1;
//        }
//        else if (n1 == true && n2 == true && n3 == true)
//        {
//            return 2;
//        }
//        else if (n1 == true && n2 == true)
//        {
//            return 3;
//        }
//        else if (n2 == true && n3 == true)
//        {
//            return 4;
//        }
//        return 0;
//    }
//    catch
//    {
//        return 0;
//    }
//}
