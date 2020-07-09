using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particletype
{
    public int pid;
    public int px;
    public int pz;
    public int ptype;
    public bool pstate;
    //public Vector3 ppos;
        //ptypes 
        // 0 - nothing
        // 1 - dirt
        // 2 - water
        // 3 - fire
        // 4 - smoke

    public particletype(int pid, int px, int pz, int ptype, bool pstate) //Vector3 ppos
    {
        this.pid = pid;
        this.px = px;
        this.pz = pz;
        this.ptype = ptype;
        this.pstate = pstate;
        //this.ppos = ppos;

    }
    public particletype(particletype pt)
    {
        this.pid = pt.pid;
        this.px = pt.px;
        this.pz = pt.pz;
        this.ptype = pt.ptype;
        this.pstate = pt.pstate;
        //this.ppos = pt.ppos;
    }
}
