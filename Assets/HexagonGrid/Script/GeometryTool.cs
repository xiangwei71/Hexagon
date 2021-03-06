﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mytool
{
    public class GeometryTool
    {
        static public bool RayHitPlane(Vector3 from, Vector3 dir, Vector3 PlaneN, Vector3 PlaneC, out Vector3 hitPos)
        {
            //(F-C)。N + t (D。N) = 0
            // t  = (C-F)。N / (D。N)
            // t  = (A / (B)
            var B = Vector3.Dot(dir, PlaneN);
            var A = Vector3.Dot(PlaneC - from, PlaneN);

            var Epsilon = 0.0001f;
            if (Mathf.Abs(B) < Epsilon)
            {
                hitPos = Vector3.zero;
                return false;
            }
            var t = A / B;
            hitPos = from + t * dir;
            return true;
        }
        static float clickPointDistance = 10.0f;
        static void GetShootingRayPerspective(Camera camera, Vector3 mousePos, out Vector3 from, out Vector3 clickWorldPoint, out Vector3 normalDir)
        {
            mousePos.y = camera.pixelHeight - mousePos.y;//mousePos左上角是(0,0)
            from = camera.transform.position;
            //第3個參數的距離沿著camera forward的方向
            clickWorldPoint = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, clickPointDistance));

            normalDir = (clickWorldPoint - from).normalized;
        }

        //正交camera
        static void GetGetShootingRayOrthographic(Camera camera, Vector3 mousePos, out Vector3 from, out Vector3 clickWorldPoint, out Vector3 normalDir)
        {
            mousePos.y = camera.pixelHeight - mousePos.y;//mousePos左上角是(0,0)

            var halfWidth = 0.5f * camera.pixelWidth;
            var halfHeight = 0.5f * camera.pixelHeight;
            var nx = (mousePos.x - halfWidth) / halfWidth;
            var ny = (mousePos.y - halfHeight) / halfHeight;
            var ratio = (float)camera.pixelWidth / camera.pixelHeight;

            var offsetY = ny * camera.orthographicSize;
            var offsetX = ratio * nx * camera.orthographicSize;
            from = camera.transform.position +
                offsetY * camera.transform.up +
                offsetX * camera.transform.right;
            clickWorldPoint = from + camera.transform.forward * clickPointDistance;

            normalDir = camera.transform.forward;
        }

        // 用ScreenPointToRay就可以了
        public static void GetShootingRay(Camera camera, Vector3 mousePos, out Vector3 from, out Vector3 clickWorldPoint, out Vector3 normalDir)
        {
            if (camera.orthographic)
                GeometryTool.GetGetShootingRayOrthographic(camera, mousePos, out from, out clickWorldPoint, out normalDir);
            else
                GeometryTool.GetShootingRayPerspective(camera, mousePos, out from, out clickWorldPoint, out normalDir);
        }
    }
}
