using UnityEngine;
using System;

[Serializable]
public struct Vector3i
{
    #region Fields
    [NonSerialized]
    public static readonly Vector3i zero = new Vector3i(0, 0, 0);
    [NonSerialized]
    public static readonly Vector3i one = new Vector3i(1, 1, 1);
    [NonSerialized]
    public static readonly Vector3i unitX = new Vector3i(1, 0, 0);
    [NonSerialized]
    public static readonly Vector3i unitY = new Vector3i(0, 1, 0);
    [NonSerialized]
    public static readonly Vector3i unitZ = new Vector3i(0, 0, 1);
    [NonSerialized]
    public static readonly Vector3i up = new Vector3i(0, 1, 0);
    [NonSerialized]
    public static readonly Vector3i down = new Vector3i(0, -1, 0);
    [NonSerialized]
    public static readonly Vector3i right = new Vector3i(1, 0, 0);
    [NonSerialized]
    public static readonly Vector3i left = new Vector3i(-1, 0, 0);
    [NonSerialized]
    public static readonly Vector3i forward = new Vector3i(0, 0, -1);
    [NonSerialized]
    public static readonly Vector3i backward = new Vector3i(0, 0, 1);

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
        return x == other.x && y == other.y && z == other.z;
    }

    /// <summary>
    /// Compares whether current instance is equal to specified <see cref="Vector3i"/>.
    /// </summary>
    /// <param name="other">The <see cref="Vector3"/> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public bool Equals(Vector3i other)
    {
        return x == other.x && y == other.y && z == other.z;
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
    public static Vector3i operator +(Vector3i v1, Vector3i v2)
    {
        return new Vector3i(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }

    public static Vector3 operator +(Vector3i v1, Vector3 v2)
    {
        return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }

    public static Vector3i operator -(Vector3i v1, Vector3i v2)
    {
        return new Vector3i(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    }

    public static Vector3i operator *(Vector3i v1, int i)
    {
        return new Vector3i(v1.x * i, v1.y * i, v1.z * i);
    }
    #endregion Operators

    public override string ToString()
    {
        return string.Format("Vector3i({0},{1},{2})", x, y, z);
    }
}
