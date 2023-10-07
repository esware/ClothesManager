using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace EWGames.Dev.Scripts
{
    public class RopeSpawn : MonoBehaviour
    {
        private readonly Dictionary<RopeControl, Vector3> _positions = new Dictionary<RopeControl, Vector3>();
        public float ropeCreateTime = 2f;
        public Vector3 defaultPosition;
        public GameObject ropePrefab;

        private void Start()
        {
            StartCoroutine(WaitAndCreateRope());
        }

        IEnumerator WaitAndCreateRope()
        {
            while (true)
            {
                var ropeClone = Instantiate(ropePrefab, transform);
                ropeClone.transform.localScale=Vector3.zero;
                ropeClone.transform.DOScale(Vector3.one, 1f);
                ropeClone.transform.localPosition = ObjectsToPlace(ropeClone.GetComponent<RopeControl>());
                yield return new WaitForSecondsRealtime(ropeCreateTime);
            }
        }
        
        

        Vector3 ObjectsToPlace(RopeControl c)
        {
            var p = defaultPosition;
            for (int i = 0; i < _positions.Count; i++)
            {
                if (!_positions.ContainsValue(p))
                {
                    return p;
                }
                p.x -= 0.3f;
            }
            _positions.Add(c,p);
            
            return p;
        }

        public void RemovePositionFromList(RopeControl ropeControl)
        {
            List<RopeControl> ropeControls = new List<RopeControl>();
            
            _positions.Remove(ropeControl);
            foreach (var p in _positions)
            {
                ropeControls.Add( p.Key);
            }
            _positions.Clear();

            foreach (var p in ropeControls)
            {
                p.transform.DOLocalMove(ObjectsToPlace(p),1f);
            }
        }

    }
}