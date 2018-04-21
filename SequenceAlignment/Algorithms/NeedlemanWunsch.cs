using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceAlignment.Algorithms
{
    class NeedlemanWunsch
    {
        public string sequenceX { set; get; }
        public string sequenceY { set; get; }

        public int gap = -1;
        public int mismatch = -1;
        public int match = 1;

        private int[,] matrix;
        private char[,] tracebackMatrix;

        public int[,] CreateMatrix()
        {
            tracebackMatrix = new char[sequenceX.Length + 1, sequenceY.Length + 1];
            matrix = new int[sequenceX.Length + 1, sequenceY.Length + 1];

            // x-sequence every first row
            for (int x = 1; x <= sequenceX.Length; x++)
            {
                matrix[x, 0] = matrix[x - 1, 0] + gap;
                tracebackMatrix[x, 0] = 'L';
            }
            // y-sequence every first column
            for (int y = 1; y <= sequenceY.Length; y++)
            {
                matrix[0, y] = matrix[0, y - 1] + gap;
                tracebackMatrix[0, y] = 'U';
            }

            for (int x = 1; x <= sequenceX.Length; x++)
            {
                for (int y = 1; y <= sequenceY.Length; y++)
                {
                    int diagonal = matrix[x - 1, y - 1] + (sequenceX.ToCharArray()[(x - 1)] == sequenceY.ToCharArray()[(y - 1)] ? match : mismatch);
                    int left = matrix[x - 1, y] + gap;
                    int up = matrix[x, y-1] + gap;
                    matrix[x, y] = Math.Max(Math.Max(diagonal, left), up);

                    if (matrix[x, y] == diagonal && x > 0 && y > 0)
                    {
                        tracebackMatrix[x, y] = 'D';
                    }
                    else
                        tracebackMatrix[x, y] = (matrix[x, y] == left ? 'L' : 'U');
                }
            }
            return matrix;
        }

        public void TraceBack(out string resultS1, out string resultS2)
        {
            resultS1 = "";
            resultS2 = "";
            int x = tracebackMatrix.GetLength(0) - 1;
            int y = tracebackMatrix.GetLength(1) - 1;
            StringBuilder alignedSeqX = new StringBuilder();
            StringBuilder alignedSeqY = new StringBuilder();

            while (tracebackMatrix[x,y] != 0)
            {
                switch (tracebackMatrix[x, y])
                {
                    case 'D':
                        alignedSeqX.Append(sequenceX[x - 1]);
                        alignedSeqY.Append(sequenceY[y - 1]);
                        x--;
                        y--;
                        break;
                    case 'U':
                        alignedSeqX.Append("-");
                        alignedSeqY.Append(sequenceY[y - 1]);
                        y--;
                        break;
                    case 'L':
                        alignedSeqX.Append(sequenceX[x - 1]);
                        alignedSeqY.Append("-");
                        x--;
                        break;
                }
            }
            resultS1 = new string(alignedSeqX.ToString().Reverse().ToArray());
            resultS2 = new string(alignedSeqY.ToString().Reverse().ToArray());
        }
    }
}
