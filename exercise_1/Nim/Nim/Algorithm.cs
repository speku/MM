using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace Nim
{
    static class Algorithm
    {
        public static Tuple<int,int> NextState(IImmutableList<int> currentState)
        {
            return (currentState.
                Zip(Enumerable.Range(0, currentState.Count()), (n, i) => new { n, i }).
                Select(ni => new { i = ni.i, ns = Enumerable.Range(0, ni.n - 1) }).
                SelectMany(ins => ins.ns.Select(_n => currentState.RemoveAt(ins.i).Insert(ins.i, _n))).
                Where(ns => ns.
                    Select(_ns =>
                    {
                        var s = Convert.ToString(_ns, 2); return s.PadLeft(32 - s.Length).Replace(' ', '0').Select(b => int.Parse(b.ToString())).
                        Zip(Enumerable.Range(0, 32), (b, i) => new { i, b });
                    }).
                    SelectMany(i => i).
                    GroupBy(ib => ib.i).
                    Select(ibs => ibs.Sum(_ib => _ib.b)).
                    All(s => s % 2 == 0)).
                    FirstOrDefault() ?? currentState.Skip(1)).
                    Zip(currentState, (ns, cs) => cs - ns).
                    Zip(Enumerable.Range(0, int.MaxValue), (n, i) => new { n, i }).
                    Where(ni => ni.n > 0).
                    Select(_ni => Tuple.Create(_ni.i, _ni.n)).
                    First();


        }
    }
}
