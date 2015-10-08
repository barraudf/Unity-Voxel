using UnityEngine;
using System;

[Serializable]
public struct GridPosition
{
	#region Fields
	[NonSerialized]
	public static readonly GridPosition Zero = new GridPosition(0, 0, 0);
	[NonSerialized]
	public static readonly GridPosition One = new GridPosition(1, 1, 1);
	[NonSerialized]
	public static readonly GridPosition UnitX = new GridPosition(1, 0, 0);
	[NonSerialized]
	public static readonly GridPosition UnitY = new GridPosition(0, 1, 0);
	[NonSerialized]
	public static readonly GridPosition UnitZ = new GridPosition(0, 0, 1);
	[NonSerialized]
	public static readonly GridPosition Up = new GridPosition(0, 1, 0);
	[NonSerialized]
	public static readonly GridPosition Down = new GridPosition(0, -1, 0);
	[NonSerialized]
	public static readonly GridPosition Right = new GridPosition(1, 0, 0);
	[NonSerialized]
	public static readonly GridPosition Left = new GridPosition(-1, 0, 0);
	[NonSerialized]
	public static readonly GridPosition Forward = new GridPosition(0, 0, 1);
	[NonSerialized]
	public static readonly GridPosition Backward = new GridPosition(0, 0, -1);

	public int x;
	public int y;
	public int z;
	#endregion Fields

	public GridPosition(int x, int y, int z)
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
		if (!(obj is GridPosition))
			return false;

		var other = (GridPosition)obj;
		bool ret = x == other.x && y == other.y && z == other.z;
		return ret;
	}

	/// <summary>
	/// Compares whether current instance is equal to specified <see cref="GridPosition"/>.
	/// </summary>
	/// <param name="other">The <see cref="GridPosition"/> to compare.</param>
	/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
	public bool Equals(GridPosition other)
	{
		bool ret = x == other.x && y == other.y && z == other.z;
		return ret;
	}

	/// <summary>
	/// Gets the hash code of this <see cref="GridPosition"/>.
	/// </summary>
	/// <returns>Hash code of this <see cref="GridPosition"/>.</returns>
	public override int GetHashCode()
	{
		int hash = x + y + z;
		return hash;
	}

	#region Operators
	public static GridPosition operator +(GridPosition value1, GridPosition value2)
	{
		value1.x += value2.x;
		value1.y += value2.y;
		value1.z += value2.z;
		return value1;
	}

	public static Vector3 operator +(GridPosition value1, Vector3 value2)
	{
		value2.x += value1.x;
		value2.y += value1.y;
		value2.z += value1.z;
		return value2;
	}

	public static GridPosition operator -(GridPosition value1, GridPosition value2)
	{
		value1.x -= value2.x;
		value1.y -= value2.y;
		value1.z -= value2.z;
		return value1;
	}

	public static GridPosition operator *(GridPosition value1, int scaleFactor)
	{
		value1.x *= scaleFactor;
		value1.y *= scaleFactor;
		value1.z *= scaleFactor;
		return value1;
	}

	public static Vector3 operator *(GridPosition value1, float scaleFactor)
	{
		Vector3 v = value1.ToVector3() * scaleFactor;
		return v;
	}

	public static GridPosition operator *(GridPosition value1, GridPosition value2)
	{
		value1.x *= value2.x;
		value1.y *= value2.y;
		value1.z *= value2.z;
		return value1;
	}

	public static GridPosition operator /(GridPosition value1, GridPosition value2)
	{
		Vector3 v2 = value2;
		value1.x = Mathf.FloorToInt(value1.x / v2.x);
		value1.y = Mathf.FloorToInt(value1.y / v2.y);
		value1.z = Mathf.FloorToInt(value1.z / v2.z);
		return value1;
	}
	#endregion Operators

	public static double DistanceSquared(GridPosition value1, GridPosition value2)
	{
		double result;
		DistanceSquared(ref value1, ref value2, out result);
		return result;
	}

	public static void DistanceSquared(ref GridPosition value1, ref GridPosition value2, out double result)
	{
		result = (value1.x - value2.x) * (value1.x - value2.x) +
				 (value1.y - value2.y) * (value1.y - value2.y) +
				 (value1.z - value2.z) * (value1.z - value2.z);
	}

	public override string ToString()
	{
		return string.Format("GridPosition({0},{1},{2})", x, y, z);
	}

	public static implicit operator GridPosition(Vector3 v)
	{
		return new GridPosition((int)v.x, (int)v.y, (int)v.z);
	}

	public static implicit operator Vector3(GridPosition pos)
	{
		return new Vector3(pos.x, pos.y, pos.z);
	}
}
