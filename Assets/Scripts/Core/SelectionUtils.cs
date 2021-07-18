using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectionUtils
{
    public delegate void DetectionEvent(RaycastHit2D hit);

    public class ObjectSelector2D
    {
        public static void CheckForObjectDetection(DetectionEvent onObjectDetect, float range)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(camRay.origin, camRay.direction, range);

            if(hit)
                onObjectDetect(hit);
        }
    }
}
