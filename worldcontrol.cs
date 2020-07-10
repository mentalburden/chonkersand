using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldControl : MonoBehaviour
{
    public int boundarysize;
    public int epochpage;
    public List<particletype> pt;
    public GameObject[,] particleobjects;
    public GameObject testobj;
    public RawImage uiimage;
    public Texture2D testimage;
    Color[] testpixels;
    

    void Start()
    {

        testimage = new Texture2D(boundarysize, boundarysize, TextureFormat.ARGB32, false);
        testpixels = new Color[boundarysize * boundarysize];        
        pt = new List<particletype>();
        particleobjects = new GameObject[boundarysize, boundarysize];
        for (int i=0, x = 0; x <= boundarysize; x++)
        {
            for (int z = 0; z <= boundarysize; z++)
            {
                int coinflip = getrand(0, 3);
                if (coinflip == 0)
                {
                    pt.Add(new particletype(i, x, z, 0, true, 0));
                }
                if (coinflip == 1)
                {
                    pt.Add(new particletype(i, x, z, 1, false, 0));
                }
                if (coinflip == 2)
                {
                    pt.Add(new particletype(i, x, z, 2, false, 0));
                }
                if (coinflip == 3)
                {
                    pt.Add(new particletype(i, x, z, 3, false, 0));
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
        if (me.page >= epochpage)
        {
            me.ptype = 0;
            me.pstate = false;
            me.page -= epochpage;
            return;
        }
        if (me != null && below != null && belowleft != null && belowright != null)
        {
            if (below.pstate == false)
            {
                me.pz -= 1;
                below.pz += 1;
                me.page -= epochpage;
                //below.pstate = true;
                return;
            }
            else if (belowleft.pstate == false && belowright.pstate == true && below.pstate == true)
            {
                me.pz -= 1;
                me.px -= 1;
                belowleft.pz += 1;
                belowleft.px += 1;
                me.page -= epochpage;
                return;
            }
            else if (belowleft.pstate == true && belowright.pstate == false && below.pstate == true)
            {
                me.pz -= 1;
                me.px += 1;
                belowright.pz += 1;
                belowright.px -= 1;
                me.page -= epochpage;
                return;
            }
            else if (belowleft.pstate == true && belowright.pstate == true && below.pstate == true)
            {
                me.page = me.page + 1;
                //me.pz -= 1;
                //me.px += 1;
                //belowright.pz += 1;
                //belowright.px -= 1;
                return;
            }
        }
    }

    public void restartscene()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        //calc
        for (int i = 0, x = 0; x < boundarysize; x++)
        {
            for (int z = 0; z < boundarysize; z++)
            {
                int thistype = GetItem(x, z).ptype;
                //run the neighbor operation and set the coords

                //run the draw or instantiate afterwards
                if (thistype == 0)
                {
                    testpixels[i] = Color.blue;
                    //testpixels[(int)x * testimage.width + (int)z] = Color.white;
                    //applychild(x, z);
                }
                else if (thistype == 1)
                {
                    testpixels[i] = Color.white;
                    //testpixels[(int)x * testimage.width + (int)z] = Color.black;
                    //applychild(x, z);
                }
                else if (thistype == 2)
                {
                    testpixels[i] = Color.white;
                    //testpixels[(int)x * testimage.width + (int)z] = Color.green;
                    //applychild(x, z);
                }
                else if (thistype == 3)
                {
                    testpixels[i] = Color.white;
                    //testpixels[(int)x * testimage.width + (int)z] = Color.red;
                    //applychild(x, z);
                }
                i++;
                runsolidkernel(x, z);
            }
        }        
        testimage.SetPixels(testpixels);
        testimage.Apply();
        uiimage.texture = testimage;
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
