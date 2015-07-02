using UnityEngine;
using System;

[Serializable]
public struct Vector3i
{
    #region Fields
    [NonSerialized]
    public static readonly Vector3i Zero = new Vector3i(0, 0, 0);
    [NonSerialized]
    public static readonly Vector3i One = new Vector3i(1, 1, 1);
    [NonSerialized]
    public static readonly Vector3i UnitX = new Vector3i(1, 0, 0);
    [NonSerialized]
    public static readonly Vector3i UnitY = new Vector3i(0, 1, 0);
    [NonSerialized]
    public static readonly Vector3i UnitZ = new Vector3i(0, 0, 1);
    [NonSerialized]
    public static readonly Vector3i Up = new Vector3i(0, 1, 0);
    [NonSerialized]
    public static readonly Vector3i Down = new Vector3i(0, -1, 0);
    [NonSerialized]
    public static readonly Vector3i Right = new Vector3i(1, 0, 0);
    [NonSerialized]
    public static readonly Vector3i Left = new Vector3i(-1, 0, 0);
    [NonSerialized]
    public static readonly Vector3i Forward = new Vector3i(0, 0, -1);
    [NonSerialized]
    public static readonly Vector3i Backward = new Vector3i(0, 0, 1);

    [NonSerialized]
    public static readonly Vector3i ChunkSize = new Vector3i(World.CHUNK_SIZE_H, World.CHUNK_SIZE_V, World.CHUNK_SIZE_H);

    public int x;
    public int y;
    public int z;
    #endregion Fields

    public Vector3i(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Compares whether current instance is equal to specified <see cref="Object"/>.
    /// </summary>
    /// <param name="obj">The <see cref="Object"/> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
        if (!(obj is Vector3i))
            return false;

        var other = (Vector3i)obj;
        bool ret = x == other.x && y == other.y && z == other.z;
        return ret;
    }

    /// <summary>
    /// Compares whether current instance is equal to specified <see cref="Vector3i"/>.
    /// </summary>
    /// <param name="other">The <see cref="Vector3"/> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public bool Equals(Vector3i other)
    {
        bool ret =  x == other.x && y == other.y && z == other.z;
        return ret;
    }

    /// <summary>
    /// Gets the hash code of this <see cref="Vector3"/>.
    /// </summary>
    /// <returns>Hash code of this <see cref="Vector3"/>.</returns>
    public override int GetHashCode()
    {
        int hash = x + y + z;
        return hash;
    }

    #region Operators
    public static Vector3i operator +(Vector3i value1, Vector3i value2)
    {
        value1.x += value2.x;
        value1.y += value2.y;
        value1.z += value2.z;
        return value1;
    }

    public static Vector3 operator +(Vector3i value1, Vector3 value2)
    {
        value2.x += value1.x;
        value2.y += value1.y;
        value2.z += value1.z;
        return value2;
    }

    public static Vector3i operator -(Vector3i value1, Vector3i value2)
    {
        value1.x -= value2.x;
        value1.y -= value2.y;
        value1.z -= value2.z;
        return value1;
    }

    public static Vector3i operator *(Vector3i value1, int scaleFactor)
    {
        value1.x *= scaleFactor;
        value1.y *= scaleFactor;
        value1.z *= scaleFactor;
        return value1;
    }

    public static Vector3 operator *(Vector3i value1, float scaleFactor)
    {
        Vector3 v = value1.ToVector3() * scaleFactor;
        return v;
    }

    public static Vector3i operator *(Vector3i value1, Vector3i value2)
    {
        value1.x *= value2.x;
        value1.y *= value2.y;
        value1.z *= value2.z;
        return value1;
    }

    public static Vector3i operator /(Vector3i value1, Vector3i value2)
    {
        Vector3 v2 = value2;
        value1.x = Mathf.FloorToInt(value1.x / v2.x);
        value1.y = Mathf.FloorToInt(value1.y / v2.y);
        value1.z = Mathf.FloorToInt(value1.z / v2.z);
        return value1;
    }
    #endregion Operators

    public static double DistanceSquared(Vector3i value1, Vector3i value2)
    {
        double result;
        DistanceSquared(ref value1, ref value2, out result);
        return result;
    }

    public static void DistanceSquared(ref Vector3i value1, ref Vector3i value2, out double result)
    {
        result = (value1.x - value2.x) * (value1.x - value2.x) +
                 (value1.y - value2.y) * (value1.y - value2.y) +
                 (value1.z - value2.z) * (value1.z - value2.z);
    }

    public override string ToString()
    {
        return string.Format("Vector3i({0},{1},{2})", x, y, z);
    }

    public static implicit operator Vector3i(Vector3 v)
    {
        Vector3i blockPos = new Vector3i(
            Mathf.RoundToInt(v.x),
            Mathf.RoundToInt(v.y),
            Mathf.RoundToInt(v.z)
            );

        return blockPos;
    }

    public static implicit operator Vector3(Vector3i pos)
    {
        return new Vector3(pos.x, pos.y, pos.z);
    }
}
