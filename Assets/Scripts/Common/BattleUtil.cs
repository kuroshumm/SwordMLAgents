using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleUtil : Singleton<BattleUtil>
{
    //---------------------------------
    // ŒöŠJ ŠÖ”
    //---------------------------------
    public void DestroyObj(GameObject obj)
    {
        Destroy(obj);
    }

    public float CalcDistanceXZ(Vector3 posA, Vector3 posB)
    {
        Vector2 posA2 = new Vector2(posA.x, posA.z);
        Vector2 posB2 = new Vector2(posB.x, posB.z);
        return Vector2.Distance(posA2, posB2);
    }

    public float CalcAngleXZ(Vector3 originVec, Vector3 targetVec)
    {
        Vector3 axis = Vector3.Cross(originVec, targetVec);
        float angle = Vector3.Angle(originVec, targetVec) * (axis.y > 0 ? 1 : -1);

        return angle;
    }

    public Mesh CreateFanMesh(Vector3[] vertices, int triangleCount = 12)
    {
        var mesh = new Mesh();

        var triangleIndexes = new List<int>(triangleCount * 3);
        for (int i = 0; i < triangleCount; ++i)
        {
            triangleIndexes.Add(0);
            triangleIndexes.Add(i + 1);
            triangleIndexes.Add(i + 2);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangleIndexes.ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }

    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                result = hit.position;
                //result.y = center.y;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public int[] StateToBinary(int stateType)
    {
        //int[] state = new int[2];
        //switch (stateType) 
        //{
        //    case 0:
        //        state[0] = 0;
        //        state[1] = 0;
        //        break;
        //    case 1:
        //        state[0] = 1;
        //        state[1] = 0;
        //        break;
        //    case 2:
        //        state[0] = 0;
        //        state[1] = 1;
        //        break;
        //}
        string binaryString = Convert.ToString(stateType, 2).PadLeft(2, '0'); // 2i”‚Ì•¶š—ñ‚ğæ“¾
        return binaryString.Select(c => c - '0').ToArray(); // Še•¶š‚ğ®”‚É•ÏŠ·‚µ
    }

    public int[] ConvertToOnehot(int[] array, int num)
    {
        int length = array.Length * num;
        int[] resultArray = new int[length];

        for(int i = 0, max = array.Length; i < max; i++)
        {
            int sect = array[i];
            for(int j = 0; j < num; j++)
            {
                if (sect != j) continue;
                resultArray[(i*num)+j] = 1;
            }
        }

        return resultArray;
    }
}
