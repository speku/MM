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

        /// <summary>
        /// Returns the first "next" state with an even nim sum
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public static IEnumerable<int> NextState(ImmutableList<int> currentState)
        {
            return NextStates(currentState).Where(nextState => EvenNimSum(nextState)).FirstOrDefault();     // generate all possible next states and filter them for evenness of their nim sums. Then return the first acceptable state.
        }


        /// <summary>
        /// Generates all possible next states of the given state
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        private static IEnumerable<ImmutableList<int>> NextStates(ImmutableList<int> currentState)
        {
            return
                currentState.Zip(Enumerable.Range(0, currentState.Count()), (n, i) => new { n, i }).          // attach indices: [3,2,1] => [(3,0),(2,1),(1,2)] 
                Select(ni => new { i = ni.i, ns = Enumerable.Range(0, ni.n - 1) }).                           // for each heap, create all possible future heap sizes: [(0,[2,1,0]),(1,[1,0]),(2,[0])] the first element in the tuple is the index within the outer list
                SelectMany(ins => ins.ns.Select(_n => currentState.RemoveAt(ins.i).Insert(ins.i, _n)));       // for each possible future heap size of each heap, create the next state with only the size of the particular heap altered. Then, flatten the list to remove one level of nesting: [[2,2,1],[1,2,1],[0,2,1],[3,1,1],[3,0,1],[3,2,0]]

        }


        /// <summary>
        /// Determines whether a states' nim sum is even
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private static bool EvenNimSum(ImmutableList<int> state)
        {
            return 
                Transpose(ToBinary(state)).                 // transform a state to its binary representation and transpose the resulting matrix: [3,2,1] => [[1,1],[1,0],[0,1]] => [[1,1,0],[1,0,1]]
                Select(nimParts => nimParts.Sum()).         // calculate the nim sums for each transposed row: [[1,1,0],[1,0,1]] => [2,2]
                All(s => s % 2 == 0);                       // return true, if all sums are even
        }


        /// <summary>
        /// Converts a state to its binary representation (of its heap sizes)
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private static IEnumerable<IEnumerable<int>> ToBinary(ImmutableList<int> state)
        {
            return
                state.
                Select(heapSize =>
                {
                    var binaryString = Convert.ToString(heapSize, 2);                   // convert each heaps' size to its binary representation: 2 -> "10"
                    return
                        binaryString.PadLeft(32 - binaryString.Length).                 // normalize the afore-created binary strings to a length of 32 digits: "10" => "                              01" (haven't counted this)
                        Replace(' ', '0').                                              // replace the whitespace characters by 0: "                              01" => "00000000000000000000000000000010"
                        Select(binaryChar => int.Parse(binaryChar.ToString()));         // parse each character as an integer
                });
        }

        /// <summary>
        /// Transposes a matrix (changes up columns and rows)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private static IEnumerable<IEnumerable<int>> Transpose(IEnumerable<IEnumerable<int>> matrix)
        {
            return
                matrix.
                Select(binaryList => binaryList.Zip(Enumerable.Range(0, 32), (element, index) => Tuple.Create(index, element))).    // attach column-indeces to each element
                SelectMany(id => id).                                                                                               // flatten the list
                GroupBy(columnElement => columnElement.Item1).                                                                      // group items by their column-index
                Select(columnElementList => columnElementList.Select(columnElement => columnElement.Item2));                        // remove indeces
        }





    }
}
