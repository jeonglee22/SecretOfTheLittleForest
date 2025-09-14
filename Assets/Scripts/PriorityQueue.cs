using System;
using System.Collections.Generic;

public class PriorityQueue<TElement, TPriority>
{
	private (TElement element, TPriority priority)[] elements;
	private int depth = 3;

	private int count = 0;
	public int Count => count;
	public TPriority Priority => Count > 0 ? elements[0].priority : throw new InvalidOperationException();

	private IComparer<TPriority> comparer = Comparer<TPriority>.Default;

	public PriorityQueue()
	{
		elements = new (TElement, TPriority)[(1 << depth) - 1];
	}

	public PriorityQueue(IComparer<TPriority> comparer)
	{
		elements = new (TElement, TPriority)[(1 << depth) - 1];
		this.comparer = comparer;
	}

	public void Enqueue(TElement element, TPriority priority)
	{
		if (Count == elements.Length)
		{
			Array.Resize(ref elements, (1 << (++depth)) - 1);
		}
		elements[count] = (element, priority);

		MoveAbove();

		count++;
	}

	private void MoveAbove()
	{
		var childPos = count;
		var parentPos = (count - 1) / 2;
		while (parentPos >= 0)
		{
			if (comparer.Compare(elements[childPos].priority, elements[parentPos].priority) < 0)
			{
				var temp = elements[parentPos];
				elements[parentPos] = elements[childPos];
				elements[childPos] = temp;
			}
			else
			{
				break;
			}

			childPos = parentPos;
			parentPos = (parentPos - 1) / 2;
		}
	}

	public TElement Dequeue()
	{
		if (Count == 0)
			throw new InvalidOperationException();
		var result = elements[0];

		elements[0] = elements[--count];
		elements[count] = default;

		MoveDown();

		return result.element;
	}

	private void MoveDown()
	{
		var parentPos = 0;
		var childPos = 0;
		while (parentPos * 2 + 1 < count)
		{
			if (parentPos * 2 + 1 == count - 1)
				childPos = parentPos * 2 + 1;
			else
			{
				var compareValue = comparer.Compare(elements[parentPos * 2 + 1].priority, elements[parentPos * 2 + 2].priority);
				childPos = parentPos * 2 + (compareValue > 0 ? 2 : 1);
			}

			if (comparer.Compare(elements[parentPos].priority, elements[childPos].priority) <= 0)
				break;

			var temp = elements[parentPos];
			elements[parentPos] = elements[childPos];
			elements[childPos] = temp;

			parentPos = childPos;
		}
	}

	public TElement Peek()
	{
		if (Count == 0)
			throw new InvalidOperationException();

		return elements[0].element;
	}

	public bool TryDequeue(out TElement element, out TPriority priority)
	{
		element = default;
		priority = default;
		if (Count == 0)
			return false;

		var result = elements[0];
		element = result.element;
		priority = result.priority;

		elements[0] = elements[--count];
		elements[count] = default;

		MoveDown();

		return true;
	}

	public bool TryPeek(out TElement element, out TPriority priority)
	{
		element = default;
		priority = default;
		if (Count == 0)
			return false;

		element = elements[0].element;
		priority = elements[0].priority;

		return true;
	}

	public void Clear()
	{
		for (int i = 0; i < Count; i++)
		{
			elements[i] = default;
		}
		count = 0;
	}
}
