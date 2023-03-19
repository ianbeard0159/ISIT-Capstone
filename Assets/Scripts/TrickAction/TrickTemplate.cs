using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickAction
{
   
    public class TrickTemplate : ScriptableObject
    {
        
        TrickFeature trickFeature;
        public bool liftZoneInput; // should be a boolean for true blow input at the right time or false no blow input/wrong time
        public int[] TrickInputs = new int[0];//to store all the user Trick Inputs after the lift zone 
        
    }
}
//can not make enums into an empty array must be strings or numbers
//there for will create an empty array, the inputs we receive will be in numbers that we 
//will compare to the vaules of our enums and depending on the order and the time between inputs
//you will get different kinds of tricks 

// This template goes to a trick script takes which control of the character's board and moves them with animations +camera direction