﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    /// <summary>
    /// Класс-контейнер настроек точек для спавна деревьев.
    /// Благодаря нему можно легко настроить площадь изначального 
    /// спавна растительности
    /// </summary>
    [Serializable]
    public class GridData
    {
        [field: SerializeField] public Transform spawnCenter { get; private set; }
        [field: SerializeField] public List<Transform> Obstacles { get; private set; }
        [field: SerializeField] public float CellSize { get; private set; } = 2f;
        [field: SerializeField, Min(0), Space] public int GridSizeX { get; private set; } = 10;
        [field: SerializeField, Min(0)] public int GridSizeY { get; private set; } = 10;
        [field: SerializeField, Min(0), Space] public float EmptyCenterRadius { get; private set; } = 4f;

        /// <summary>
        /// Возвращает левую нижнюю точку поля для удобных расчётов 
        /// </summary>
        public Vector3 GetStartPos()
        {
            var leftDownPos = spawnCenter.position
                - spawnCenter.forward * (GridSizeY / 2f) * CellSize
                - spawnCenter.right * (GridSizeX / 2f) * CellSize;

            return leftDownPos;
        }

        /// <summary>
        /// Возвращает все доступные точки для спавна с учётом проверки
        /// расстояния для указанных препятствий
        /// </summary>
        public List<Vector3> GetSpawnPoints()
        {
            var points = new List<Vector3>();
            var startPos = GetStartPos();

            for(var x = 0; x < GridSizeX; x++)
            {
                for(var y = 0; y < GridSizeY; y++)
                {
                    var pos = startPos
                        + spawnCenter.right * x * CellSize
                        + spawnCenter.forward * y * CellSize;
                    
                    if (IsPosNearObstacle(pos))
                    {
                        continue;
                    }

                    points.Add(pos);
                }
            }

            return points;
        }

        /// <summary>
        /// Проверка на присутствие рядом препятствия для точки
        /// </summary>
        private bool IsPosNearObstacle(Vector3 pos)
        {
            foreach(var obstacle in Obstacles)
            {
                if (!obstacle)
                {
                    continue;
                }

                if (Vector3.Distance(pos, obstacle.position) <= EmptyCenterRadius)
                {
                    return true;
                }
            }
            return false;
        }
    }
}