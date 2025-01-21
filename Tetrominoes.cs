using System;

private readonly int[][][] Tetrominoes = new int[][][]
{
    new int[][] { new int[] { 1, 1, 1, 1 } }, // I
    new int[][] { new int[] { 1, 1 }, new int[] { 1, 1 } }, // O
    new int[][] { new int[] { 0, 1, 0 }, new int[] { 1, 1, 1 } }, // T
    new int[][] { new int[] { 1, 1, 0 }, new int[] { 0, 1, 1 } }, // S
    new int[][] { new int[] { 0, 1, 1 }, new int[] { 1, 1, 0 } }, // Z
    new int[][] { new int[] { 1, 0, 0 }, new int[] { 1, 1, 1 } }, // L
    new int[][] { new int[] { 0, 0, 1 }, new int[] { 1, 1, 1 } }  // J
};
