namespace NHibernateExperiments.Infrastructure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    namespace Ryc.Utilities
    {
        static public class ListUtil
        {
            /// <summary>
            /// Creates a new generic list with all unique items from the specified list.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="list"></param>
            /// <returns></returns>
            static public IList<T> MakeUnique<T>(IList list)
            {
                var dic = new Dictionary<T, object>();
                foreach (T n in list)
                {
                    dic[n] = null;
                }
                return new List<T>(dic.Keys);
            }

            /// <summary>
            /// Remove all duplicates from given list. No order is guaranteed.
            /// </summary>        
            static public void RemoveDuplicates<T>(IList<T> list)
            {
                var dic = new Dictionary<T, object>();
                foreach (T t in list)
                {
                    dic[t] = null;
                }
                list.Clear();
                foreach (T t in dic.Keys)
                {
                    list.Add(t);
                }
            }

            public static void RemoveAtIndexes<T>(IList<T> list, int[] idxs)
            {
                Array.Sort(idxs);
                Array.Reverse(idxs);

                for (var col = 0; col < idxs.Length; col++)
                {
                    list.RemoveAt(idxs[col]);
                }
            }

            public static void RemoveAllFromListExcept<T>(IList<T> list, IList<T> itemsToKeep)
            {
                var remove = new List<int>(list.Count);

                for (var i = 0; i < list.Count; i++)
                {
                    T item = list[i];
                    if (!itemsToKeep.Contains(item))
                    {
                        remove.Add(i);
                    }
                }

                RemoveAtIndexes(list, remove.ToArray());
            }

            public static void Sort<T>(IList<T> list, Comparison<T> comparer)
            {
                var sortList = new List<T>(list);
                sortList.Sort(comparer);
                list.Clear();
                foreach (T item in sortList)
                {
                    list.Add(item);
                }
            }

            public static void Sort<T>(IList<T> list, IComparer<T> comparer)
            {
                var sortList = new List<T>(list);
                sortList.Sort(comparer);
                list.Clear();
                foreach (T item in sortList)
                {
                    list.Add(item);
                }
            }

            public static void Sort<T>(IList<T> list)
            {
                var sortList = new List<T>(list);
                sortList.Sort();
                list.Clear();
                foreach (T item in sortList)
                {
                    list.Add(item);
                }
            }

            public static IList<IList<T>> Split<T>(IList<T> list, int maxSize)
            {
                var result = new List<IList<T>>();

                var index = 0;
                var current = new List<T>();
                while (index < list.Count)
                {
                    if (current.Count == maxSize)
                    {
                        result.Add(current);
                        current = new List<T>();
                    }
                    current.Add(list[index]);
                    index++;
                }
                if (current.Count > 0)
                {
                    result.Add(current);
                }

                return result;
            }

            public static IList<IList> Split(IList list, int maxSize)
            {
                var result = new List<IList>();

                var index = 0;
                var current = new ArrayList();
                while (index < list.Count)
                {
                    if (current.Count == maxSize)
                    {
                        result.Add(current);
                        current = new ArrayList();
                    }
                    current.Add(list[index]);
                    index++;
                }
                if (current.Count > 0)
                {
                    result.Add(current);
                }

                return result;
            }

            public static IList<T> GetDifference<T>(IList<T> items, IEnumerable<T> excludeItems)
            {
                var result = new List<T>(items);

                foreach (T t in excludeItems)
                {
                    result.Remove(t);
                }

                return result;
            }

            public static List<T> Create<T>(params T[] items)
            {
                return new List<T>(items);
            }           

            public static bool Swap<T>(IList<T> list, int i, int j)
            {
                var min = Math.Min(i, j);
                var max = Math.Max(i, j);
                if (min < 0 || max > list.Count - 1)
                {
                    return false;
                }

                var n = list[i];
                list[i] = list[j];
                list[j] = n;
                return true;
            }

            public static bool HasEqualItems<T>(IEnumerable<T> itemsA, IEnumerable<T> itemsB)
            {
                var setA = new HashSet<T>(itemsA);
                var setB = new HashSet<T>(itemsB);

                return setA.IsSubsetOf(setB) && setA.IsSupersetOf(setB);
            }

            #region Extension methods

            public static T GetElementAfter<T>(this IList<T> source, Func<T, bool> condition) where T : class
            {
                var element = source.FirstOrDefault(condition);
                if (element == null)
                {
                    return null;
                }

                var nextIndex = source.IndexOf(element) + 1;

                return nextIndex < source.Count
                    ? source[nextIndex]
                    : null;
            }

            public static T GetElementBefore<T>(this IList<T> source, Func<T, bool> condition) where T : class
            {
                var element = source.FirstOrDefault(condition);
                if (element == null)
                {
                    return null;
                }

                var prevIndex = source.IndexOf(element) - 1;

                return prevIndex >= 0
                    ? source[prevIndex]
                    : null;
            }

            public static bool IsNullOrEmpty<T>(this IList<T> source)
            {
                return source == null || source.Count == 0;
            }

            public static IList<T> GetAlternate<T>(this IList<T> source, bool startWithFirst = false)
            {
                return source.Where((element, index) => (index % 2 == 0) == startWithFirst).ToList();
            }

            #endregion
        }
    }

}