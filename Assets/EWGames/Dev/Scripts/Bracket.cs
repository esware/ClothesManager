using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace EWGames.Dev.Scripts
{
    public class Bracket : MonoBehaviour
    {
        public List<GameObject> ropeList;
        public List<Vector3> ropePositionList;
        private int ropeCount = 3;
        private Vector3 _lastRopePosition;
        public float ropeCreateTime = 1f;
        
        private void Start()
        {
            for (int i = 0; i < ropeCount; i++)
            {
                var rope = ObjectPool.Instance.GetObjectFromPool();
                rope.transform.position = new Vector3(-0.3f+0.3f*i,-0.5f,0);
                ropePositionList.Add(rope.transform.position);
                ropeList.Add(rope);
                
            }
            SignUpEvents();
            
        }
        private void SignUpEvents()
        {
            DragAndDrop.OnRopeLocated += CreateRope;
        }

        void CreateRope(SewingMachine sewingMachine)
        {
            StartCoroutine(WaitAndCreateRope());
        }


        IEnumerator WaitAndCreateRope()
        {
            yield return new WaitForSecondsRealtime(ropeCreateTime);
            DragAndDrop dragAndDrop = null;
            
            for (int i = 0; i < ropeList.Count; i++)
            {
                var r = ropeList[i];
                if (!r.gameObject.activeInHierarchy)
                {
                    ropeList.Remove(r);
                    dragAndDrop = r.GetComponent<DragAndDrop>();
                    break;
                }
            }
            
            var rope = ObjectPool.Instance.GetObjectFromPool();
            ropeList.Add(rope);
            rope.transform.position =dragAndDrop.startPosition;
        }
    }
}