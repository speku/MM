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

        // Please take note, that the below queries are implemented by means of lazy evaluation.
        // So even though the computations might seem incredibly inefficient at first glance, with multiple intermediary lists created,
        // this is actually not the case as long as the result is passed along as an IEnumerable abstraction.
        // In order to modularize the code and make it more digestible, I've introduced a few helper functions that might force evaluation.
        // The initial attempt was all written in one big query, where such effects were minimized.
        // Coming from the Haskell programming language, I've tried to maintain a functional approach - so the below style is deliberate.


        /// <summary>
        /// Returns the first "next" state with an even nim sum
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public static ImmutableList<int> NextState(ImmutableList<int> currentState)
        {
            return NextStates(currentState).Where(nextState => EvenNimSum(nextState)).FirstOrDefault() ?? NonWinningState(currentState);  // generate all possible next states and filter them for evenness of their nim sums. Then return the first acceptable state. If no winning position can be achieved, return a state with a non-empty heap removed.
        }


        /// <summary>
        /// Produces a potentially non-winning state
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        private static ImmutableList<int> NonWinningState(ImmutableList<int> currentState)
        {
            var iNonEmptyHeap = currentState.FindIndex(n => n > 0);                                            // find first non-empty heap
            return currentState.RemoveAt(iNonEmptyHeap).Insert(iNonEmptyHeap, 0);               // remove the heap (replace its heap size with 0)
        }


        /// <summary>
        /// Generates all possible next states of the given state
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        private static IEnumerable<ImmutableList<int>> NextStates(ImmutableList<int> currentState)
        {
            return
                currentState.Zip(Enumerable.Range(0, currentState.Count()), (heapSize, index) => new { heapSize, index }).                                                                              // attach indices: [3,2,1] => [(3,0),(2,1),(1,2)] 
                Where(heapSizeIndex => heapSizeIndex.heapSize != 0).                                                                                                                                    // filter out empty heaps since they can't be modified in order to create a new state
                Select(heapSizeIndex => new { index = heapSizeIndex.index, nextSizes = Enumerable.Range(0, heapSizeIndex.heapSize)}).                                                                   // for each heap, create all possible future heap sizes: [(0,[2,1,0]),(1,[1,0]),(2,[0])] the first element in the tuple is the index within the outer list
                SelectMany(indecesHeapSizes => indecesHeapSizes.nextSizes.Select(nextSize => currentState.RemoveAt(indecesHeapSizes.index).Insert(indecesHeapSizes.index, nextSize)));                  // for each possible future heap size of each heap, create the next state with only the size of the particular heap altered. Then, flatten the list to remove one level of nesting: [[2,2,1],[1,2,1],[0,2,1],[3,1,1],[3,0,1],[3,2,0]]
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
                All(nimSum => nimSum % 2 == 0);             // return true, if all sums are even
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
                        binaryString.PadLeft(32 - binaryString.Length).                 // normalize the afore-created binary strings to a length of 32 digits: "10" => "                              01"
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


        /// <summary>
        /// Wrapper for the Prove function
        /// </summary>
        /// <param name="heapSizes"></param>
        /// <returns></returns>
        public static bool Prove(params int[] heapSizes)
        {
            return ProveCore(Enumerable.Repeat(heapSizes.ToImmutableList(), 1));
        }


        /// <summary>
        /// Determines wheter a state generated by NextState is a winning position of the computer.
        /// The game is assumed to be won by the player that removed the last heap.
        /// </summary>
        /// <param name="states">states generated by the previous player</param>
        /// <param name="player">the player that generated the states which are passed as the first argument
        /// 0 = computer, 1 = human</param>
        /// <returns></returns>
        public static bool ProveCore(IEnumerable<ImmutableList<int>> states, int player = 0)
        {
            return states.Any(state => state.All(x => x == 0)) ? player == 1 : player == 0 ? ProveCore(states.SelectMany(state => NextStates(state)), 1) : ProveCore(states.Select(state => NextState(state)), 0);  

            // If a human has generated the passed in states, the player parameter is bound to the argument 1
            // and for each possible player state a corresponding (and potentially winning) next state for the computer is generated.
            // If the player parameter is bound to 0 however, the ProveCore function assumes the role of the human player and 
            // generates all possible next states for each passed in state.
            // Once a state with all empty heaps is passed, the procedure comes to an halt, and returns true if this all-zero-state has been generated by the computer - and false otherwise.
            // There's another way to play the game, where the player that removes the last heap loses - however this is not assumed here.
            // If such an alteration of the algorithm is required, I'd be happy to adapt it.
        }



    }
}
