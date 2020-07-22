using System;
// ReSharper disable MemberCanBePrivate.Global

namespace CurlNet
{
	public sealed partial class Curl
	{
		public readonly struct Progress : IEquatable<Progress>
		{
			public readonly int Done;
			public readonly int Total;

			public Progress(int done, int total)
			{
				Done = done;
				Total = total;
			}

			#region IEquatable
			public bool Equals(Progress other)
			{
				return Done == other.Done && Total == other.Total;
			}

			public override bool Equals(object obj)
			{
				return obj is Progress other && Equals(other);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					return (Done * 397) ^ Total;
				}
			}

			public static bool operator ==(Progress left, Progress right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(Progress left, Progress right)
			{
				return !left.Equals(right);
			}
			#endregion

			public override string ToString()
			{
				// ReSharper disable RedundantToStringCallForValueType
				return Done.ToString() + "/" + Total.ToString();
				// ReSharper restore RedundantToStringCallForValueType
			}
		}
	}
}