// Solver Class
// Holds the methods that solve the sudoku puzzle
// Created by: Ethan Cockrell

using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SudokuSolverTest
{
    public class Solver
    {
        public static string startPuzzle1 = "070000043040009610800634900094052000358460020000800530080070091902100005007040802"; // easy
        public static string startPuzzle2 = "005020006430000007008730500040000001600143005500000090004098100200000058300010900"; // medium - level 1
        public static string startPuzzle3 = "000105300030008000567000900000000090806370410005004700000040000000000020423007000"; // medium - level 1 - phantom row, column, group & hidden pair row
        public static string startPuzzle4 = "800000000003600000070090200050007000000045700000100030001000068008500010090000400"; // expert - level 3 - guess and check & brute force
        public static string startPuzzle5 = "000000760800706000007900000042130000000004050000050009000000401038000000060005093"; // medium - level 1 - phantom column, group & hidden pair row, column
        public static string startPuzzle6 = "007809031080004000094000080020000000600030042001050600002000000000010908000003570"; // hard --- level 2 - phantom row, group & naked pair column, group & naked triple column
        public static string startPuzzle7 = "000000000409000010050607008200000807000140000800000540008700002000060100300890000"; // medium - level 1 - phantom row, column, group & naked pair row, group
        public static string startPuzzle8 = "000001000310000902009360100000080057607000000020100600000504003008900040270000000"; // medium - level 1 - phantom row, column, group & naked pair column
        public static string startPuzzle9 = "070040050000020010301009800029800600000000000040060008004010000100902040000000003"; // medium - level 1 - phantom row, column, group & naked pair row, column, group
        public static string startPuzzle10 = "600070009003004080020000005000081040000000800065000020001740000200065000080010030"; // expert - level 3 - phantom row, column & naked pair row, column, group & hidden pair group & guess and check
        public static string startPuzzle11 = "000000000000789000789000000400500600500600400600400500000000700000000800000000900"; // naked triple row, group test
        public static string startPuzzle12 = "007456000008000000009000000070564000080000000090000000000645789000000000000000000"; // naked triple column test
        public static string startPuzzle13 = "000000000000123000000000123000000000000000000000000000000000000000000000000000000"; // hidden triple row, group test
        public static string startPuzzle14 = "000000000000000000000000000010000000020000000030000000001000000002000000003000000"; // hidden triple column test
        public static string startPuzzle15 = "000800400030001000001000953003002009004000080000790200000006801500000600026007000"; // expert - level 3 - phantom row, group & hidden pair row, column & X wing row & guess and check
        public static string startPuzzle16 = "000000000567000000890000000000000500000000600000000700000000800000000900000000000"; // naked quad row test
        public static string startPuzzle17 = "058000000069000000070000000000000000000000000000000000000567890000000000000000000"; // naked quad column test
        public static string startPuzzle18 = "567000000000000000000000000000508000000609000000700000000800000000900000000000000"; // naked quad group test
        public static string startPuzzle19 = "003910700000003400100040006060700000002109600000002010700080003008200000005071900"; // hard - level 2 - phantom group & hidden pair column & X Wing column
        public static string startPuzzle20 = "091700050700801000008469000073000000000396000000000280000684500000902001020007940"; // hard - level 2 - phantom row & naked pair column & Y Wing row-column & Y Wing column-group
        public static string startPuzzle21 = "000000000060003020070620400900000003000040700300701000501000200600800100000000649"; // expert - level 3 - phantom row, column, group & hidden pair column & X Wing row & Y Wing row-group & guess and check
        public static string startPuzzle22 = "000400000002000410010036020600700030007050000509000002020040070090005000008010309"; // expert - level 3 - phantom row, column & naked pair row & Y Wing column-group & guess and check
        public static string startPuzzle23 = "091030000050006407000000500007080001200609000000000060040910000000003852060000000"; // expert - level 3 - phantom row, column, group & naked pair row, column & hidden pair row & naked triple row & guess and check
        public static string startPuzzle24 = "800000000003600000070090200050007000000045700000100030001000068008500010090000400"; // expert - level 3 - phantom row, group & hidden pair group & guess and check & bruteForce
        public static string startPuzzle25 = "800030200000080000030009000090700020001006507070040008000071000500000084004020009"; // expert - level 3 - phantom row, column, group & naked pair row, column & hidden pair row & hidden triple column & guess and check
        public static string startPuzzle26 = "070010906000000307100600800084000001005090000000502003600830000030050070018000000"; // expert - level 3 - phantom column & hidden pair row, column & naked triple row & guess and check
        public static string startPuzzle27 = "005000007900800000010023000007109000850040000004000509300004600000000034000006280"; // hard - level 2 - phantom row, column & X Wing row, column & Y Wing row-group, column-group
        public static string startPuzzle28 = "108000006060900700050000210000600000000009803004000027000582004300000050000010070"; // expert - level 3 - phantom row, column, group & naked pair column & hidden pair row & Y Wing column-group & guess and check
        public static string startPuzzle29 = "000000010400900503058400000300000206790006300000830001040000008007604000003020000"; // expert - level 3 - phantom row, column, group & hidden pair row & Y Wing row-group, column-group & guess and check
        public static string startPuzzle30 = "800700000043600000075090200050007000000045700080100030521070368008500910090000400"; // guess and check test

        public static int[,] solvePuzzleInt = new int[9, 9];
        public static int[,,] solvePossible = new int[9, 9, 9];
        public static bool[,] solveRows = new bool[9, 9];
        public static bool[,] solveColumns = new bool[9, 9];
        public static bool[,] solveGroups = new bool[9, 9];
        public static bool solved = false;
        public static bool bruteForceSolved = false;

        public static int difficultyScore = 0;

        private static int[] ROW_START = { 0, 0, 0, 3, 3, 3, 6, 6, 6 };
        private static int[] ROW_END = { 2, 2, 2, 5, 5, 5, 8, 8, 8 };
        private static int[] COLUMN_START = { 0, 3, 6, 0, 3, 6, 0, 3, 6 };
        private static int[] COLUMN_END = { 2, 5, 8, 2, 5, 8, 2, 5, 8 };

        public static void Main(string[] args)
        {
            solvePuzzleInt = ConvertPuzzleToInt(startPuzzle24);
            solveRows = InitializeRows(solvePuzzleInt);
            solveColumns = InitializeColumns(solvePuzzleInt);
            solveGroups = InitializeGroups(solvePuzzleInt);
            solvePossible = InitializePossible(solvePuzzleInt, solveRows, solveColumns, solveGroups);

            if (Check(solvePuzzleInt))
            {
                LevelZeroMethods(solvePuzzleInt, solvePossible, solveRows, solveColumns, solveGroups, false, false);

                if (!solved)
                {
                    LevelOneMethods(solvePuzzleInt, solvePossible, solveRows, solveColumns, solveGroups, false, false);

                    if(!solved)
                    {
                        LevelTwoMethods(solvePuzzleInt, solvePossible, solveRows, solveColumns, solveGroups, false, false);
                        if(!solved)
                        {
                            LevelThreeMethods(solvePuzzleInt, solvePossible, solveRows, solveColumns, solveGroups);
                        }
                    }
                }

                Console.WriteLine($"Difficulty Score: {difficultyScore}");

            } else
            {
                Console.WriteLine("Puzzle isn't valid...");
            }
            
            Console.WriteLine($"Solved: {solved}");

            PrintPuzzle(solvePuzzleInt);

            if (!solved)
            {
                PrintPossible(solvePuzzleInt, solvePossible);
            }
            

        }

        public static int[,] ConvertPuzzleToInt(string stringPuzzle)
        {
            int[,] intPuzzle = new int[9, 9];

            for (int i = 0; i < 81; i++) // index for stringPuzzle
            {
                if (int.TryParse(stringPuzzle.Substring(i,1), out int num))
                {
                    int row = i / 9;
                    int col = i % 9;
                    intPuzzle[row, col] = num;
                } else
                {
                    Console.WriteLine("error");
                }
                

            }

            return intPuzzle;
        }

        public static void UpdateShadowPossible(int number, int row, int column, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups)
        {
            Rows[row, number - 1] = true;
            Columns[column, number - 1] = true;
            int group = DetermineGroup(row, column);
            Groups[group, number - 1] = true;

            // Removes all options from the cell
            for (int k = 0; k < 9; k++) // number 1-9
            { 
                if (possible[row, column, k] == k + 1)
                {
                    possible[row, column, k] = 0;
                }
            }

            // Removes the number as an option for the column
            for (int i = 0; i < 9; i++) // row number
            { 
                if (possible[i, column, number - 1] == number)
                {
                    possible[i, column, number - 1] = 0;
                }
            }

            // Removes the number as an option for the row
            for (int j = 0; j < 9; j++) // column number
            { 
                if (possible[row, j, number - 1] == number)
                {
                    possible[row, j, number - 1] = 0;
                }
            }

            // Removes the number as an option for the group
            for (int i = ROW_START[group]; i <= ROW_END[group]; i++) // row number
            { 
                for (int j = COLUMN_START[group]; j <= COLUMN_END[group]; j++) // column number
                { 
                    if (possible[i, j, number - 1] == number)
                    {
                        possible[i, j, number - 1] = 0;
                    }
                }
            }
        }

        public static void UpdateSolved(int[,] intPuzzle)
        {
            int count = 0;
            for (int i = 0; i < 9; i++) // row number
            {
                for (int j = 0; j < 9; j++) // column number
                {
                    if (intPuzzle[i, j] != 0)
                    {
                        count++;
                    }
                }
            }
            solved = (count == 81 && Check(intPuzzle));

        }

        public static bool Check(int[,] intPuzzle)
        {
            int count = 0;

            // Check to make sure there are at least 17 given digits
            for (int i = 0; i < 9; i++) // row number
            {
                for (int j = 0; j < 9; j++) // column number
                {
                    if (intPuzzle[i, j] != 0)
                    {
                        count++;
                    }
                }
            }
            if (count < 17)
            {
                return false;
            }

            // Checks each row for duplicates
            for (int i = 0; i < 9; i++) // row number
            { 
                for (int k = 0; k < 9; k++) // number 1-9
                { 
                    count = 0;
                    for (int j = 0; j < 9; j++) // column number
                    { 
                        if (intPuzzle[i, j] == k + 1)
                            count++;
                    }
                    if (count > 1)
                        return false;
                }
            }

            // Checks each column for duplicates
            for (int j = 0; j < 9; j++) // column number
            { 
                for (int k = 0; k < 9; k++) // number 1-9
                { 
                    count = 0;
                    for (int i = 0; i < 9; i++) // row number
                    { 
                        if (intPuzzle[i, j] == k + 1)
                            count++;
                    }
                    if (count > 1)
                        return false;
                }
            }

            // Checks each group for duplicates
            for (int l = 0; l < 9; l++) // group number
            { 
                for (int k = 0; k < 9; k++) // number 1-9
                { 
                    count = 0;
                    for (int i = ROW_START[l]; i <= ROW_END[l]; i++)
                    {
                        for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++)
                        {
                            if (intPuzzle[i, j] == k + 1)
                                count++;
                        }
                    }
                    if (count > 1)
                        return false;
                }
            }

            return true;
        }

        public static bool[,] InitializeRows(int[,] intPuzzle)
        {
            bool[,] Rows = new bool[9, 9];

            for (int i = 0; i < 9; i++) // row number
            { 
                for (int j = 0; j < 9; j++) // column number
                { 
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (intPuzzle[i,j] == k + 1)
                        {
                            Rows[i,k] = true;
                        }
                    }
                }
            }
            return Rows;
        }

        public static bool[,] InitializeColumns(int[,] intPuzzle)
        {
            bool[,] Columns = new bool[9,9];

            for (int j = 0; j < 9; j++) // column number
            { 
                for (int i = 0; i < 9; i++) // row number
                { 
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (intPuzzle[i,j] == k + 1)
                        {
                            Columns[j,k] = true;
                        }
                    }
                }
            }
            return Columns;
        }

        public static bool[,] InitializeGroups(int[,] intPuzzle)
        {
            bool[,] Groups = new bool[9,9];
            int group;

            for (int i = 0; i < 9; i++) // row number
            { 
                for (int j = 0; j < 9; j++) // column number
                { 
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (intPuzzle[i,j] == k + 1)
                        {
                            group = DetermineGroup(i, j);
                            Groups[group,k] = true;
                        }
                    }
                }
            }
            return Groups;
        }

        public static int DetermineGroup(int row, int column)
        {
            int group;
            if (row < 3 && column < 3)
            {
                group = 0;
            }
            else if (row < 3 && column < 6)
            {
                group = 1;
            }
            else if (row < 3)
            {
                group = 2;
            }
            else if (row < 6 && column < 3)
            {
                group = 3;
            }
            else if (row < 6 && column < 6)
            {
                group = 4;
            }
            else if (row < 6)
            {
                group = 5;
            }
            else if (column < 3)
            {
                group = 6;
            }
            else if (column < 6)
            {
                group = 7;
            }
            else
            {
                group = 8;
            }
            return group;
        }

        public static int[,,] InitializePossible(int[,] intPuzzle, bool[,] Rows, bool[,] Columns, bool[,] Groups)
        {
            int[,,] possible = new int[9,9,9];
            int group;

            for (int i = 0; i < 9; i++) // row number
            { 
                for (int j = 0; j < 9; j++) // column number
                { 
                    if (intPuzzle[i,j] == 0)
                    {
                        group = DetermineGroup(i, j);
                        for (int k = 0; k < 9; k++) // number 1-9
                        { 
                            if (!Rows[i,k] && !Columns[j,k] && !Groups[group,k])
                            {
                                possible[i,j,k] = k + 1;
                            }
                        }
                    }
                }
            }

            return possible;
        }

        public static int LevelZeroMethods(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes;
            int tempLevelZeroChanges = 0;
            do
            {
                changes = 0;
                changes += OneInARowPossibleCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += OneInAColumnPossibleCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += OneInAGroupPossibleCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += OneInACellPossibleCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                tempLevelZeroChanges += changes;
            } while (changes != 0);
            return tempLevelZeroChanges;
        }

        public static int OneInARowPossibleCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int works;
            int worksColumn = 0;
            int changes = 0;
            int tempChanges;

            do
            {
                tempChanges = 0;
                for (int i = 0; i < 9; i++) // row number
                { 
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        works = 0;
                        for (int j = 0; j < 9; j++) // column number
                        { 
                            if (intPuzzle[i,j] == 0)
                            {
                                if (possible[i,j,k] == k + 1)
                                {
                                    works++;
                                    worksColumn = j;
                                }
                            }
                        }

                        if (works == 1)
                        {
                            intPuzzle[i,worksColumn] = k + 1;
                            if (!isGuessAndCheck && !isBruteForce)
                            {
                                UpdateSolved(intPuzzle);
                            }
                            UpdateShadowPossible((k + 1), i, worksColumn, possible, Rows, Columns, Groups);
                            tempChanges++;
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0);

            return changes;
        }

        public static int OneInAColumnPossibleCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int works;
            int worksRow = 0;
            int changes = 0;
            int tempChanges;

            do
            {
                tempChanges = 0;
                for (int j = 0; j < 9; j++) // column number
                { 
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        works = 0;
                        for (int i = 0; i < 9; i++) // row number
                        { 
                            if (intPuzzle[i, j] == 0)
                            {
                                if (possible[i, j, k] == k + 1)
                                {
                                    works++;
                                    worksRow = i;
                                }
                            }
                        }

                        if (works == 1)
                        {
                            intPuzzle[worksRow, j] = k + 1;
                            if (!isGuessAndCheck && !isBruteForce)
                            {
                                UpdateSolved(intPuzzle);
                            }
                            UpdateShadowPossible((k + 1), worksRow, j, possible, Rows, Columns, Groups);
                            tempChanges++;
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0);

            return changes;
        }

        public static int OneInAGroupPossibleCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int works;
            int worksRow = 0;
            int worksColumn = 0;
            int changes = 0;
            int tempChanges;

            do
            {
                tempChanges = 0;
                for (int l = 0; l < 9; l++) // group number
                { 
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        works = 0;
                        for (int i = ROW_START[l]; i <= ROW_END[l]; i++) // row number
                        { 
                            for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++) // column number
                            { 
                                if (intPuzzle[i, j] == 0)
                                {
                                    if (possible[i, j, k] == k + 1)
                                    {
                                        works++;
                                        worksRow = i;
                                        worksColumn = j;
                                    }
                                }
                            }
                        }

                        if (works == 1)
                        {
                            intPuzzle[worksRow, worksColumn] = k + 1;
                            if (!isGuessAndCheck && !isBruteForce)
                            {
                                UpdateSolved(intPuzzle);
                            }
                            UpdateShadowPossible((k + 1), worksRow, worksColumn, possible, Rows, Columns, Groups);
                            tempChanges++;
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0);
            return changes;
        }

        public static int OneInACellPossibleCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int works = 0;
            int count;
            int changes = 0;
            int tempChanges;

            do
            {
                tempChanges = 0;
                for (int i = 0; i < 9; i++) // row number
                { 
                    for (int j = 0; j < 9; j++) // column number
                    { 
                        count = 0;
                        for (int k = 0; k < 9; k++) // number 1-9
                        { 
                            if (intPuzzle[i, j] == 0)
                            {
                                if (possible[i, j, k] == k + 1)
                                {
                                    works = k + 1;
                                    count++;
                                }
                            }
                        }

                        if (count == 1)
                        {
                            intPuzzle[i, j] = works;
                            if (!isGuessAndCheck && !isBruteForce)
                            {
                                UpdateSolved(intPuzzle);
                            }
                            UpdateShadowPossible(works, i, j, possible, Rows, Columns, Groups);
                            tempChanges++;
                        }
                    }
                }
            } while (tempChanges != 0);

            return changes;
        }

        public static int LevelOneMethods(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes;
            int tempLevelOneChanges = 0;

            do
            {
                changes = 0;
                changes += PhantomChecks(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce); 
                changes += NakedPairChecks(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce); 
                changes += HiddenPairChecks(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce); 
                tempLevelOneChanges += changes;
            } while (changes != 0 && !solved);

            return tempLevelOneChanges;
        }

        public static int PhantomChecks(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes;
            int tempPhantomChanges = 0;

            do
            {
                changes = 0;
                changes += RowPhantomCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += ColumnPhantomCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += GroupPhantomRowCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += GroupPhantomColumnCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                tempPhantomChanges += changes;
            } while (changes != 0 && !solved);

            return tempPhantomChanges;
        }

        public static int RowPhantomCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            bool flag;
            int count;
            int rowWork = 0;
            int columnsChangedCount;

            do
            {
                tempChanges = 0;
                for (int l = 0; l < 9 && !solved; l++) // group number
                { 
                    for (int k = 0; k < 9 && !solved; k++) // number 1-9
                    { 
                        count = 0;
                        columnsChangedCount = 0;

                        for (int i = ROW_START[l]; i <= ROW_END[l]; i++) // row number
                        { 
                            flag = false;
                            for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++) // column number
                            { 
                                if (intPuzzle[i, j] == 0)
                                {
                                    if (possible[i, j, k] == k + 1 && flag == false)
                                    {
                                        rowWork = i;
                                        count++;
                                        flag = true;
                                    }
                                }
                            }
                        }

                        if (count == 1)
                        {
                            for (int j = 0; j < 9; j++) // column number
                            { 
                                if (intPuzzle[rowWork, j] == 0)
                                {
                                    if (j < COLUMN_START[l] || j > COLUMN_END[l])
                                    {
                                        if (possible[rowWork, j, k] != 0)
                                        {
                                            possible[rowWork, j, k] = 0;
                                            columnsChangedCount++;
                                        }
                                    }
                                }
                            }

                            if (columnsChangedCount > 0)
                            {
                                tempChanges++;

                                // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                if (!isGuessAndCheck && !isBruteForce)
                                {
                                    Console.WriteLine("Phantom Row found something!");
                                    difficultyScore++;
                                }

                                // Run previous methods to see if the puzzle can be solved
                                tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int ColumnPhantomCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            bool flag;
            int count;
            int columnWork = 0;
            int rowsChangedCount;

            do
            {
                tempChanges = 0;
                for (int l = 0; l < 9 && !solved; l++) // group number
                { 
                    for (int k = 0; k < 9 && !solved; k++) // number 1-9
                    { 
                        if (!Groups[l, k])
                        {
                            count = 0;
                            rowsChangedCount = 0;
                            for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++) // column number
                            { 
                                flag = false;
                                for (int i = ROW_START[l]; i <= ROW_END[l]; i++) // row number
                                { 
                                    if (intPuzzle[i, j] == 0)
                                    {
                                        if (possible[i, j, k] == k + 1 && flag == false)
                                        {
                                            columnWork = j;
                                            count++;
                                            flag = true;
                                        }
                                    }
                                }
                            }

                            if (count == 1)
                            {
                                for (int i = 0; i < 9; i++) // row number
                                { 
                                    if (intPuzzle[i, columnWork] == 0)
                                    {
                                        if (i < ROW_START[l] || i > ROW_END[l])
                                        {
                                            if (possible[i, columnWork, k] != 0)
                                            {
                                                possible[i, columnWork, k] = 0;
                                                rowsChangedCount++;
                                            }
                                        }
                                    }
                                }

                                // Save data in change log
                                if (rowsChangedCount > 0)
                                {
                                    tempChanges++;

                                    // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                    if (!isGuessAndCheck && !isBruteForce)
                                    {
                                        Console.WriteLine("Phantom Column found something!");
                                        difficultyScore++;
                                    }

                                    // Run previous methods to see if the puzzle can be solved
                                    tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int GroupPhantomRowCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int groupCount;
            int groupWork;
            int tempGroup;
            int count;

            do
            {
                tempChanges = 0;
                for (int i = 0; i < 9 && !solved; i++) // row number
                { 
                    for (int k = 0; k < 9 && !solved; k++) // number 1-9
                    { 
                        if (!Rows[i, k])
                        {
                            groupCount = 0;
                            groupWork = 10;
                            for (int j = 0; j < 9; j++) // column number
                            { 
                                if (intPuzzle[i, j] == 0)
                                {
                                    if (possible[i, j, k] == k + 1)
                                    {
                                        tempGroup = DetermineGroup(i, j);
                                        if (tempGroup != groupWork)
                                        {
                                            groupWork = tempGroup;
                                            groupCount++;
                                        }
                                    }
                                }
                            }

                            if (groupCount == 1)
                            {
                                count = 0;
                                for (int row = ROW_START[groupWork]; row <= ROW_END[groupWork]; row++)
                                {
                                    for (int col = COLUMN_START[groupWork]; col <= COLUMN_END[groupWork]; col++)
                                    {
                                        if (intPuzzle[row, col] == 0 && possible[row, col, k] == k + 1 && row != i)
                                        {
                                            possible[row, col, k] = 0;
                                            count++;
                                        }
                                    }
                                }

                                if (count > 0)
                                {
                                    tempChanges++;

                                    // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                    if (!isGuessAndCheck && !isBruteForce)
                                    {
                                        Console.WriteLine("Phantom Group-Row found something!");
                                        difficultyScore++;
                                    }

                                    // Run previous methods to see if the puzzle can be solved
                                    tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int GroupPhantomColumnCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int groupCount;
            int groupWork;
            int tempGroup;
            int count;

            do
            {
                tempChanges = 0;
                for (int j = 0; j < 9 && !solved; j++) // column number
                { 
                    for (int k = 0; k < 9 && !solved; k++) // number 1-9
                    { 
                        if (!Columns[j, k])
                        {
                            groupCount = 0;
                            groupWork = 10;
                            for (int i = 0; i < 9; i++) // row number
                            { 
                                if (intPuzzle[i, j] == 0)
                                {
                                    if (possible[i, j, k] == k + 1)
                                    {
                                        tempGroup = DetermineGroup(i, j);
                                        if (tempGroup != groupWork)
                                        {
                                            groupWork = tempGroup;
                                            groupCount++;
                                        }
                                    }
                                }
                            }

                            if (groupCount == 1)
                            {
                                count = 0;
                                for (int row = ROW_START[groupWork]; row <= ROW_END[groupWork]; row++)
                                {
                                    for (int col = COLUMN_START[groupWork]; col <= COLUMN_END[groupWork]; col++)
                                    {
                                        if (intPuzzle[row, col] == 0 && possible[row, col, k] == k + 1 && col != j)
                                        {
                                            possible[row, col, k] = 0;
                                            count++;
                                        }
                                    }
                                }

                                if (count > 0)
                                {
                                    tempChanges++;

                                    // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                    if (!isGuessAndCheck && !isBruteForce)
                                    {
                                        Console.WriteLine("Phantom Group-Column found something!");
                                        difficultyScore++;
                                    }

                                    // Run previous methods to see if the puzzle can be solved
                                    tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int HiddenPairChecks(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes;
            int tempHiddenPairChanges = 0;

            do
            {
                changes = 0;
                changes += HiddenPairRowCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += HiddenPairColumnCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += HiddenPairGroupCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                tempHiddenPairChanges += changes;
            } while (changes != 0 && !solved);

            return tempHiddenPairChanges;
        }

        public static int HiddenPairRowCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges = 0;
            int[,] columns = new int[9, 2];
            int[] columnsCount = new int[9];
            int[] pairNumbers = new int[9];
            int pairCount = 0;
            int number1 = 0;
            int number2 = 0;
            int column1 = 0;
            int column2 = 0;
            bool[] twoOptionCell = new bool[9];
            int tempCount = 0;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int i = 0; i < 9 && !solved; i++) // row number
                { 
                    for (int z = 0; z < 9; z++)
                    {
                        columns[z, 0] = 0;
                        columns[z, 1] = 0;
                        columnsCount[z] = 0;
                        twoOptionCell[z] = false;
                    }
                    pairCount = 0;
                    column1 = 0;
                    column2 = 0;

                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (!Rows[i, k])
                        {
                            for (int j = 0; j < 9; j++) // column number
                            { 
                                if (intPuzzle[i, j] == 0 && possible[i, j, k] == k + 1)
                                {
                                    if (columnsCount[k] == 0)
                                    {
                                        columns[k, 0] = j;
                                    }
                                    else if (columnsCount[k] == 1)
                                    {
                                        columns[k, 1] = j;
                                    }
                                    columnsCount[k]++;
                                }
                            }
                        }
                    }

                    // Counts the number of numbers 1-9 that appear in only 2 columns
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (columnsCount[k] == 2)
                        {
                            pairNumbers[pairCount] = k;
                            pairCount++;
                        }
                    }

                    // Counts the number of cells with only 2 options
                    for (int j = 0; j < 9; j++) // column number
                    { 
                        if (intPuzzle[i, j] == 0)
                        {
                            tempCount = 0;
                            for (int k = 0; k < 9; k++) // number 1-9
                            { 
                                if (possible[i, j, k] == k + 1)
                                {
                                    tempCount++;
                                }
                            }
                            if (tempCount == 2)
                            {
                                twoOptionCell[j] = true;
                            }
                        }
                    }

                    // Checks to see if any of the numbers match any other numbers, forming a hidden pair
                    // Then, if a hidden pair is found, it updates the possible values
                    if (pairCount > 1)
                    {
                        for (int m = 0; m < pairCount; m++)
                        {
                            number1 = pairNumbers[m];
                            column1 = columns[number1, 0];
                            column2 = columns[number1, 1];

                            for (int n = m + 1; n < pairCount; n++)
                            {
                                number2 = pairNumbers[n];

                                if (column1 == columns[number2, 0] && column2 == columns[number2, 1])
                                {
                                    if (!(twoOptionCell[column1] && twoOptionCell[column2]))
                                    {
                                        // number1 and number2 form a hidden pair in column1 and column2
                                        totalCount = 0;

                                        // Remove all possible options other than number1 and number2 in column1 and column2
                                        for (int k = 0; k < 9; k++)
                                        {
                                            if (k != number1 && k != number2)
                                            {
                                                if (possible[i, column1, k] == k + 1)
                                                {
                                                    possible[i, column1, k] = 0;
                                                    totalCount++;
                                                }
                                                if (possible[i, column2, k] == k + 1)
                                                {
                                                    possible[i, column2, k] = 0;
                                                    totalCount++;
                                                }
                                            }
                                        }

                                        if (totalCount > 0)
                                        {
                                            tempChanges++;

                                            // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                            if (!isGuessAndCheck && !isBruteForce)
                                            {
                                                Console.WriteLine("Hidden Pair Row found something!");
                                                difficultyScore++;
                                            }

                                            // Run previous methods to see if the puzzle can be solved
                                            tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);
            return changes;
        }

        public static int HiddenPairColumnCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[,] rows = new int[9, 2];
            int[] rowsCount = new int[9];
            int[] pairNumbers = new int[9];
            int pairCount = 0;
            int number1 = 0;
            int number2 = 0;
            int row1 = 0;
            int row2 = 0;
            bool[] twoOptionCell = new bool[9];
            int tempCount = 0;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int j = 0; j < 9 && !solved; j++) // column number
                { 
                    for (int z = 0; z < 9; z++)
                    {
                        rows[z, 0] = 0;
                        rows[z, 1] = 0;
                        rowsCount[z] = 0;
                        twoOptionCell[z] = false;
                    }
                    pairCount = 0;
                    row1 = 0;
                    row2 = 0;

                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (!Columns[j, k])
                        {
                            for (int i = 0; i < 9; i++) // row number
                            { 
                                if (intPuzzle[i, j] == 0 && possible[i, j, k] == k + 1)
                                {
                                    if (rowsCount[k] == 0)
                                    {
                                        rows[k, 0] = i;
                                    }
                                    else if (rowsCount[k] == 1)
                                    {
                                        rows[k, 1] = i;
                                    }
                                    rowsCount[k]++;
                                }
                            }
                        }
                    }

                    // Counts the number of numbers 1-9 that appear in only 2 rows
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (rowsCount[k] == 2)
                        {
                            pairNumbers[pairCount] = k;
                            pairCount++;
                        }
                    }

                    // Counts the number of cells with only 2 options
                    for (int i = 0; i < 9; i++) // row number
                    { 
                        if (intPuzzle[i, j] == 0)
                        {
                            tempCount = 0;
                            for (int k = 0; k < 9; k++) // number 1-9
                            { 
                                if (possible[i, j, k] == k + 1)
                                {
                                    tempCount++;
                                }
                            }
                            if (tempCount == 2)
                            {
                                twoOptionCell[i] = true;
                            }
                        }
                    }

                    // Checks to see if any of the numbers match any other numbers, forming a hidden pair
                    // Then, if a hidden pair is found, it updates the possible values
                    if (pairCount > 1)
                    {
                        for (int m = 0; m < pairCount; m++)
                        {
                            number1 = pairNumbers[m];
                            row1 = rows[number1, 0];
                            row2 = rows[number1, 1];

                            for (int n = m + 1; n < pairCount; n++)
                            {
                                number2 = pairNumbers[n];

                                if (row1 == rows[number2, 0] && row2 == rows[number2, 1])
                                {
                                    if (!(twoOptionCell[row1] && twoOptionCell[row2]))
                                    {
                                        // number1 and number2 form a hidden pair in row1 and row2
                                        totalCount = 0;

                                        // Remove all possible options other than number1 and number2 in row1 and row2
                                        for (int k = 0; k < 9; k++)
                                        {
                                            if (k != number1 && k != number2)
                                            {
                                                if (possible[row1, j, k] == k + 1)
                                                {
                                                    possible[row1, j, k] = 0;
                                                    totalCount++;
                                                }
                                                if (possible[row2, j, k] == k + 1)
                                                {
                                                    possible[row2, j, k] = 0;
                                                    totalCount++;
                                                }
                                            }
                                        }

                                        if (totalCount > 0)
                                        {
                                            tempChanges++;

                                            // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                            if (!isGuessAndCheck && !isBruteForce)
                                            {
                                                Console.WriteLine("Hidden Pair Column found something!");
                                                difficultyScore++;
                                            }

                                            // Run previous methods to see if the puzzle can be solved
                                            tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int HiddenPairGroupCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[] cellCount = new int[9];
            int[,] rows = new int[9, 2];
            int[,] columns = new int[9, 2];
            int[] pairNumbers = new int[9];
            int pairCount = 0;
            bool[,] twoOptionCells = new bool[9, 9];
            int tempCount = 0;
            int number1 = 0;
            int number2 = 0;
            int row1 = 0;
            int row2 = 0;
            int column1 = 0;
            int column2 = 0;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int l = 0; l < 9 && !solved; l++) // group number
                { 
                    pairCount = 0;
                    for (int z = 0; z < 9; z++)
                    {
                        for (int y = 0; y < 9; y++)
                        {
                            twoOptionCells[z, y] = false;
                        }
                        cellCount[z] = 0;
                        pairNumbers[z] = 0;
                        rows[z, 0] = 0;
                        columns[z, 0] = 0;
                        rows[z, 1] = 0;
                        columns[z, 1] = 0;
                    }

                    // Counts how many times each number appears in the group
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (!Groups[l, k])
                        {
                            for (int i = ROW_START[l]; i <= ROW_END[l]; i++) // row number
                            { 
                                for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++) // column number
                                { 
                                    if (intPuzzle[i, j] == 0 && possible[i, j, k] == k + 1)
                                    {
                                        if (cellCount[k] == 0)
                                        {
                                            rows[k, 0] = i;
                                            columns[k, 0] = j;
                                        }
                                        else if (cellCount[k] == 1)
                                        {
                                            rows[k, 1] = i;
                                            columns[k, 1] = j;
                                        }
                                        cellCount[k]++;
                                    }
                                }
                            }
                        }
                    }

                    // Counts the number of numbers 1-9 that appear in only 2 rows
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (cellCount[k] == 2)
                        {
                            pairNumbers[pairCount] = k;
                            pairCount++;
                        }
                    }

                    // Counts the number of cells with only 2 options
                    for (int i = ROW_START[l]; i <= ROW_END[l]; i++) // row number
                    { 
                        for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++) // column number
                        { 
                            tempCount = 0;
                            if (intPuzzle[i, j] == 0)
                            {
                                for (int k = 0; k < 9; k++) // number 1-9
                                { 
                                    if (possible[i, j, k] == k + 1)
                                    {
                                        tempCount++;
                                    }
                                }
                                if (tempCount == 2)
                                {
                                    twoOptionCells[i, j] = true;
                                }
                            }
                        }
                    }

                    // Checks to see if any of the numbers match any other numbers, forming a hidden pair
                    // Then, if a hidden pair is found, it updates the possible values
                    if (pairCount > 1)
                    {
                        for (int m = 0; m < pairCount; m++)
                        {
                            number1 = pairNumbers[m];
                            row1 = rows[number1, 0];
                            column1 = columns[number1, 0];
                            row2 = rows[number1, 1];
                            column2 = columns[number1, 1];

                            for (int n = m + 1; n < pairCount; n++)
                            {
                                number2 = pairNumbers[n];

                                if (row1 == rows[number2, 0] && row2 == rows[number2, 1] &&
                                        column1 == columns[number2, 0] && column2 == columns[number2, 1])
                                {
                                    if (!(twoOptionCells[row1, column1] && twoOptionCells[row2, column2]))
                                    {
                                        // number1 and number2 form a hidden pair in row1, column1 and row2, column2
                                        totalCount = 0;

                                        // Remove all possible options other than number1 and number2 in row1, column1 and row2, column2
                                        for (int k = 0; k < 9; k++)
                                        {
                                            if (k != number1 && k != number2)
                                            {
                                                if (possible[row1, column1, k] == k + 1)
                                                {
                                                    possible[row1, column1, k] = 0;
                                                    totalCount++;
                                                }
                                                if (possible[row2, column2, k] == k + 1)
                                                {
                                                    possible[row2, column2, k] = 0;
                                                    totalCount++;
                                                }
                                            }
                                        }

                                        if (totalCount > 0)
                                        {
                                            tempChanges++;

                                            // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                            if (!isGuessAndCheck && !isBruteForce)
                                            {
                                                Console.WriteLine("Hidden Pair Group found something!");
                                                difficultyScore++;
                                            }

                                            // Run previous methods to see if the puzzle can be solved
                                            tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);
            return changes;
        }

        public static int NakedPairChecks(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes;
            int tempNakedPairChanges = 0;

            do
            {
                changes = 0;
                changes += NakedPairRowCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += NakedPairColumnCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += NakedPairGroupCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                tempNakedPairChanges += changes;
            } while (changes != 0 && !solved);

            return tempNakedPairChanges;
        }

        public static int NakedPairRowCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[] numberCount = new int[9];
            int[,] numbers = new int[9, 2];
            int[] pairColumns = new int[9];
            int pairCount = 0;
            int number1 = 0;
            int number2 = 0;
            int column1 = 0;
            int column2 = 0;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int i = 0; i < 9 && !solved; i++) // row number
                { 
                    for (int z = 0; z < 9; z++)
                    {
                        numbers[z, 0] = 0;
                        numbers[z, 1] = 0;
                        numberCount[z] = 0;
                        pairColumns[z] = 0;

                    }
                    pairCount = 0;

                    // Goes down the row and counts the number of options in each cell and stores the first two
                    for (int j = 0; j < 9; j++) // column number
                    { 
                        if (intPuzzle[i, j] == 0)
                        {
                            for (int k = 0; k < 9; k++) // number 1-9
                            { 
                                if (possible[i, j, k] == k + 1)
                                {
                                    if (numberCount[j] == 0)
                                    {
                                        numbers[j, 0] = k;
                                    }
                                    else if (numberCount[j] == 1)
                                    {
                                        numbers[j, 1] = k;
                                    }
                                    numberCount[j]++;
                                }
                            }
                        }
                    }

                    // Counts the number of cells that have only 2 options
                    for (int j = 0; j < 9; j++) // column number
                    { 
                        if (numberCount[j] == 2)
                        {
                            pairColumns[pairCount] = j;
                            pairCount++;
                        }
                    }

                    // Checks to see if any of the two option cells have the same numbers, forming a naked pair
                    // Then, if a naked pair is found, it updates the possible values
                    if (pairCount > 1)
                    {
                        for (int m = 0; m < pairCount; m++)
                        {
                            column1 = pairColumns[m];
                            number1 = numbers[column1, 0];
                            number2 = numbers[column1, 1];

                            for (int n = m + 1; n < pairCount; n++)
                            {
                                column2 = pairColumns[n];

                                if (number1 == numbers[column2, 0] && number2 == numbers[column2, 1])
                                {
                                    // number1 and number2 form a naked pair in column1 and column2
                                    totalCount = 0;

                                    // Remove number1 and number2 from every cell in row other than column1 and column2
                                    for (int j = 0; j < 9; j++)
                                    {
                                        if (intPuzzle[i, j] == 0 && j != column1 && j != column2)
                                        {
                                            if (possible[i, j, number1] == number1 + 1)
                                            {
                                                possible[i, j, number1] = 0;
                                                totalCount++;
                                            }
                                            if (possible[i, j, number2] == number2 + 1)
                                            {
                                                possible[i, j, number2] = 0;
                                                totalCount++;
                                            }
                                        }
                                    }

                                    if (totalCount > 0)
                                    {
                                        tempChanges++;

                                        // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                        if (!isGuessAndCheck && !isBruteForce)
                                        {
                                            Console.WriteLine("Naked Pair Row found something!");
                                            difficultyScore++;
                                        }

                                        // Run previous methods to see if the puzzle can be solved
                                        tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);
            return changes;
        }

        public static int NakedPairColumnCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[] numberCount = new int[9];
            int[,] numbers = new int[9, 2];
            int[] pairRows = new int[9];
            int pairCount = 0;
            int number1 = 0;
            int number2 = 0;
            int row1 = 0;
            int row2 = 0;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int j = 0; j < 9 && !solved; j++) // column number
                { 
                    for (int z = 0; z < 9; z++)
                    {
                        numbers[z, 0] = 0;
                        numbers[z, 1] = 0;
                        numberCount[z] = 0;
                        pairRows[z] = 0;
                    }
                    pairCount = 0;

                    // Goes down the column and counts the number of options in each cell and stores the first two
                    for (int i = 0; i < 9; i++) // row number
                    { 
                        if (intPuzzle[i, j] == 0)
                        {
                            for (int k = 0; k < 9; k++) // number 1-9
                            { 
                                if (possible[i, j, k] == k + 1)
                                {
                                    if (numberCount[i] == 0)
                                    {
                                        numbers[i, 0] = k;
                                    }
                                    else if (numberCount[i] == 1)
                                    {
                                        numbers[i, 1] = k;
                                    }
                                    numberCount[i]++;
                                }
                            }
                        }
                    }

                    // Counts the number of cells that have only 2 options
                    for (int i = 0; i < 9; i++) // row number
                    { 
                        if (numberCount[i] == 2)
                        {
                            pairRows[pairCount] = i;
                            pairCount++;
                        }
                    }

                    // Checks to see if any of the two option cells have the same numbers, forming a naked pair
                    // Then, if a naked pair is found, it updates the possible values
                    if (pairCount > 1)
                    {
                        for (int m = 0; m < pairCount; m++)
                        {
                            row1 = pairRows[m];
                            number1 = numbers[row1, 0];
                            number2 = numbers[row1, 1];

                            for (int n = m + 1; n < pairCount; n++)
                            {
                                row2 = pairRows[n];

                                if (number1 == numbers[row2, 0] && number2 == numbers[row2, 1])
                                {
                                    // number1 and number2 form a naked pair in row1 and row2
                                    totalCount = 0;

                                    // Remove number1 and number2 from every cell in column other than column1 and column2
                                    for (int i = 0; i < 9; i++)
                                    {
                                        if (intPuzzle[i, j] == 0 && i != row1 && i != row2)
                                        {
                                            if (possible[i, j, number1] == number1 + 1)
                                            {
                                                possible[i, j, number1] = 0;
                                                totalCount++;
                                            }
                                            if (possible[i, j, number2] == number2 + 1)
                                            {
                                                possible[i, j, number2] = 0;
                                                totalCount++;
                                            }
                                        }
                                    }

                                    if (totalCount > 0)
                                    {
                                        tempChanges++;

                                        // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                        if (!isGuessAndCheck && !isBruteForce)
                                        {
                                            Console.WriteLine("Naked Pair Column found something!");
                                            difficultyScore++;
                                        }

                                        // Run previous methods to see if the puzzle can be solved
                                        tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int NakedPairGroupCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[,] numberCount = new int[9, 9];
            int[,,] numbers = new int[9, 9, 2];
            int pairCount = 0;
            int[] pairRows = new int[9];
            int[] pairColumns = new int[9];
            int row1 = 0;
            int column1 = 0;
            int number1 = 0;
            int row2 = 0;
            int column2 = 0;
            int number2 = 0;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int l = 0; l < 9 && !solved; l++) // group number// group number
                { 
                    for (int x = 0; x < 9; x++)
                    {
                        for (int y = 0; y < 9; y++)
                        {
                            numberCount[x, y] = 0;
                            numbers[x, y, 0] = 0;
                            numbers[x, y, 1] = 0;
                        }
                    }
                    pairCount = 0;

                    // Goes through the group and counts the number of options in each cell and stores the first two
                    for (int i = ROW_START[l]; i <= ROW_END[l]; i++) // row number
                    { 
                        for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++) // column number
                        { 
                            if (intPuzzle[i, j] == 0)
                            {
                                for (int k = 0; k < 9; k++) // number 1-9
                                { 
                                    if (possible[i, j, k] == k + 1)
                                    {
                                        if (numberCount[i, j] == 0)
                                        {
                                            numbers[i, j, 0] = k;
                                        }
                                        else if (numberCount[i, j] == 1)
                                        {
                                            numbers[i, j, 1] = k;
                                        }
                                        numberCount[i, j]++;
                                    }
                                }
                            }
                        }
                    }

                    // Counts the number of cells that have only 2 options
                    for (int i = ROW_START[l]; i <= ROW_END[l]; i++) // row number
                    { 
                        for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++) // column number
                        { 
                            if (numberCount[i, j] == 2)
                            {
                                pairRows[pairCount] = i;
                                pairColumns[pairCount] = j;
                                pairCount++;
                            }
                        }
                    }

                    // Checks to see if any of the two option cells have the same numbers, forming a naked pair
                    // Then, if a naked pair is found, it updates the possible values
                    if (pairCount > 1)
                    {
                        for (int m = 0; m < pairCount; m++)
                        {
                            row1 = pairRows[m];
                            column1 = pairColumns[m];
                            number1 = numbers[row1, column1, 0];
                            number2 = numbers[row1, column1, 1];

                            for (int n = m + 1; n < pairCount; n++)
                            {
                                row2 = pairRows[n];
                                column2 = pairColumns[n];

                                if (number1 == numbers[row2, column2, 0] && number2 == numbers[row2, column2, 1])
                                {
                                    // number1 and number2 form a naked pair in row1, column1 and row2, column2
                                    totalCount = 0;

                                    // Remove number1 and number2 from every cell in group other than row1, column1 and row2, column2
                                    for (int i = ROW_START[l]; i <= ROW_END[l]; i++) // row number
                                    { 
                                        for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++) // column number
                                        { 
                                            if (intPuzzle[i, j] == 0 && !(i == row1 && j == column1) && !(i == row2 && j == column2))
                                            {
                                                if (possible[i, j, number1] == number1 + 1)
                                                {
                                                    possible[i, j, number1] = 0;
                                                    totalCount++;
                                                }
                                                if (possible[i, j, number2] == number2 + 1)
                                                {
                                                    possible[i, j, number2] = 0;
                                                    totalCount++;
                                                }
                                            }
                                        }
                                    }

                                    if (totalCount > 0)
                                    {
                                        tempChanges++;

                                        // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                        if (!isGuessAndCheck && !isBruteForce)
                                        {
                                            Console.WriteLine("Naked Pair Group found something!");
                                            difficultyScore++;
                                        }

                                        // Run previous methods to see if the puzzle can be solved
                                        tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int LevelTwoMethods(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes;
            int tempLevelTwoChanges = 0;

            do
            {
                changes = 0;
                changes += NakedTripleChecks(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += HiddenTripleChecks(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += NakedQuadChecks(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += XWingChecks(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += YWingChecks(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                tempLevelTwoChanges += changes;
            } while (changes != 0 && !solved);

            return tempLevelTwoChanges;
        }

        public static int NakedTripleChecks(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes;
            int tempNakedTripleChanges = 0;

            do
            {
                changes = 0;
                changes += NakedTripleRowCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += NakedTripleColumnCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += NakedTripleGroupCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                tempNakedTripleChanges += changes;
            } while (changes != 0 && !solved);

            return tempNakedTripleChanges;
        }

        public static int NakedTripleRowCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[] numberCount = new int[9];
            int[,] numbers = new int[9, 3];
            int[] tripleColumns = new int[9];
            int tripleCount = 0;
            int column1 = 0;
            int column2 = 0;
            int column3 = 0;
            int[] diffNumbers = new int[3];
            int[] countDiffNumbers = new int[3];
            int tempCount = 0;
            int tempNumber = 0;
            bool sameFlag = false;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int i = 0; i < 9 && !solved; i++) // row number
                { 
                    for (int z = 0; z < 9; z++)
                    {
                        numbers[z, 0] = 0;
                        numbers[z, 1] = 0;
                        numbers[z, 2] = 0;
                        numberCount[z] = 0;
                        tripleColumns[z] = 0;
                    }
                    tripleCount = 0;

                    // Goes down the row and counts the number of options in each cell and stores the first three
                    for (int j = 0; j < 9; j++) // column number
                    { 
                        if (intPuzzle[i, j] == 0)
                        {
                            for (int k = 0; k < 9; k++) // number 1-9
                            { 
                                if (possible[i, j, k] == k + 1)
                                {
                                    if (numberCount[j] < 3)
                                    {
                                        numbers[j, numberCount[j]] = k;
                                    }
                                    numberCount[j]++;
                                }
                            }
                        }
                    }

                    // Counts number of cells with 3 or less options
                    for (int j = 0; j < 9; j++) // column number
                    { 
                        if (numberCount[j] == 2 || numberCount[j] == 3)
                        {
                            tripleColumns[tripleCount] = j;
                            tripleCount++;
                        }
                    }

                    // Checks to see if any of the two or three option cells have the same numbers, forming a naked triple
                    // Then, if a naked triple is found, it updates the possible values
                    if (tripleCount > 2)
                    {
                        // Stage 1
                        for (int a = 0; a <= tripleCount - 3 && !solved; a++)
                        {
                            for (int z = 0; z < 3; z++)
                            {
                                diffNumbers[z] = 0;
                                countDiffNumbers[z] = 0;
                            }
                            column1 = tripleColumns[a];
                            diffNumbers[0] = numbers[column1, 0];
                            diffNumbers[1] = numbers[column1, 1];
                            if (numberCount[column1] == 2)
                            {
                                countDiffNumbers[0] = 2;
                            }
                            if (numberCount[column1] == 3)
                            {
                                diffNumbers[2] = numbers[column1, 2];
                                countDiffNumbers[0] = 3;
                            }

                            // Stage 2
                            for (int b = a + 1; b <= tripleCount - 2 && !solved; b++)
                            {
                                column2 = tripleColumns[b];
                                tempCount = countDiffNumbers[0];

                                for (int x = 0; x < numberCount[column2]; x++)
                                {
                                    sameFlag = false;
                                    tempNumber = numbers[column2, x];
                                    for (int y = 0; y < countDiffNumbers[0]; y++)
                                    {
                                        if (tempNumber == diffNumbers[y])
                                        {
                                            sameFlag = true;
                                        }
                                    }
                                    if (sameFlag == false)
                                    {
                                        if (tempCount < 3)
                                        {
                                            diffNumbers[tempCount] = tempNumber;
                                        }
                                        tempCount++;
                                    }
                                }
                                countDiffNumbers[1] = tempCount;

                                // Stage 3
                                if (countDiffNumbers[1] <= 3) // There still may be a naked triple
                                { 
                                    for (int c = b + 1; c <= tripleCount - 1 && !solved; c++)
                                    {
                                        column3 = tripleColumns[c];
                                        tempCount = countDiffNumbers[1];

                                        for (int x = 0; x < numberCount[column3]; x++)
                                        {
                                            sameFlag = false;
                                            tempNumber = numbers[column3, x];
                                            for (int y = 0; y < countDiffNumbers[1]; y++)
                                            {
                                                if (tempNumber == diffNumbers[y])
                                                {
                                                    sameFlag = true;
                                                }
                                            }
                                            if (sameFlag == false)
                                            {
                                                if (tempCount < 3)
                                                {
                                                    diffNumbers[tempCount] = tempNumber;
                                                }
                                                tempCount++;
                                            }
                                        }
                                        countDiffNumbers[2] = tempCount;

                                        if (countDiffNumbers[2] == 3)
                                        {
                                            // The 3 numbers stored in diffNumbers form a naked triple in columns 1, 2, & 3
                                            totalCount = 0;

                                            // Remove number1, number2, and number3 from every cell 
                                            // in row other than column1, column2, and column4
                                            for (int j = 0; j < 9; j++)
                                            {
                                                if (intPuzzle[i, j] == 0 && j != column1 && j != column2 && j != column3)
                                                {
                                                    if (possible[i, j, diffNumbers[0]] == diffNumbers[0] + 1)
                                                    {
                                                        possible[i, j, diffNumbers[0]] = 0;
                                                        totalCount++;
                                                    }
                                                    if (possible[i, j, diffNumbers[1]] == diffNumbers[1] + 1)
                                                    {
                                                        possible[i, j, diffNumbers[1]] = 0;
                                                        totalCount++;
                                                    }
                                                    if (possible[i, j, diffNumbers[2]] == diffNumbers[2] + 1)
                                                    {
                                                        possible[i, j, diffNumbers[2]] = 0;
                                                        totalCount++;
                                                    }
                                                }
                                            }

                                            if (totalCount > 0)
                                            {
                                                tempChanges++;
                                                
                                                // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                if (!isGuessAndCheck && !isBruteForce)
                                                {
                                                    Console.WriteLine("Naked Triple Row found something!");
                                                    difficultyScore += 100;
                                                }
                                                
                                                // Run previous methods to see if the puzzle can be solved
                                                tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                if (!solved)
                                                {
                                                    tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int NakedTripleColumnCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[] numberCount = new int[9];
            int[,] numbers = new int[9, 3];
            int[] tripleRows = new int[9];
            int tripleCount = 0;
            int row1 = 0;
            int row2 = 0;
            int row3 = 0;
            int[] diffNumbers = new int[3];
            int[] countDiffNumbers = new int[3];
            int tempCount = 0;
            int tempNumber = 0;
            bool sameFlag = false;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int j = 0; j < 9 && !solved; j++) // column number
                { 
                    for (int z = 0; z < 9; z++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            numbers[z, y] = 0;
                        }
                        numberCount[z] = 0;
                        tripleRows[z] = 0;
                    }
                    tripleCount = 0;

                    // Goes down the column and counts the number of options in each cell and stores the first three
                    for (int i = 0; i < 9; i++) // row number
                    { 
                        if (intPuzzle[i, j] == 0)
                        {
                            for (int k = 0; k < 9; k++) // number 1-9
                            { 
                                if (possible[i, j, k] == k + 1)
                                {
                                    if (numberCount[i] < 3)
                                    {
                                        numbers[i, numberCount[i]] = k;
                                    }
                                    numberCount[i]++;
                                }
                            }
                        }
                    }

                    // Counts number of cells with 3 or less options
                    for (int i = 0; i < 9; i++) // row number
                    { 
                        if (numberCount[i] == 2 || numberCount[i] == 3)
                        {
                            tripleRows[tripleCount] = i;
                            tripleCount++;
                        }
                    }

                    // Checks to see if any of the two or three option cells have the same numbers, forming a naked triple
                    // Then, if a naked triple is found, it updates the possible values
                    if (tripleCount > 2)
                    {
                        // Stage 1
                        for (int a = 0; a <= tripleCount - 3 && !solved; a++)
                        {
                            for (int z = 0; z < 3; z++)
                            {
                                diffNumbers[z] = 0;
                                countDiffNumbers[z] = 0;
                            }
                            row1 = tripleRows[a];
                            diffNumbers[0] = numbers[row1, 0];
                            diffNumbers[1] = numbers[row1, 1];
                            if (numberCount[row1] == 2)
                            {
                                countDiffNumbers[0] = 2;
                            }
                            if (numberCount[row1] == 3)
                            {
                                diffNumbers[2] = numbers[row1, 2];
                                countDiffNumbers[0] = 3;
                            }

                            // Stage 2
                            for (int b = a + 1; b <= tripleCount - 2 && !solved; b++)
                            {
                                row2 = tripleRows[b];
                                tempCount = countDiffNumbers[0];

                                for (int x = 0; x < numberCount[row2]; x++)
                                {
                                    sameFlag = false;
                                    tempNumber = numbers[row2, x];
                                    for (int y = 0; y < countDiffNumbers[0]; y++)
                                    {
                                        if (tempNumber == diffNumbers[y])
                                        {
                                            sameFlag = true;
                                        }
                                    }
                                    if (sameFlag == false)
                                    {
                                        if (tempCount < 3)
                                        {
                                            diffNumbers[tempCount] = tempNumber;
                                        }
                                        tempCount++;
                                    }
                                }
                                countDiffNumbers[1] = tempCount;

                                // Stage 3
                                if (countDiffNumbers[1] <= 3) // There still may be a naked triple
                                { 
                                    for (int c = b + 1; c <= tripleCount - 1 && !solved; c++)
                                    {
                                        row3 = tripleRows[c];
                                        tempCount = countDiffNumbers[1];

                                        for (int x = 0; x < numberCount[row3]; x++)
                                        {
                                            sameFlag = false;
                                            tempNumber = numbers[row3, x];
                                            for (int y = 0; y < countDiffNumbers[1]; y++)
                                            {
                                                if (tempNumber == diffNumbers[y])
                                                {
                                                    sameFlag = true;
                                                }
                                            }
                                            if (sameFlag == false)
                                            {
                                                if (tempCount < 3)
                                                {
                                                    diffNumbers[tempCount] = tempNumber;
                                                }
                                                tempCount++;
                                            }
                                        }
                                        countDiffNumbers[2] = tempCount;

                                        if (countDiffNumbers[2] == 3)
                                        {
                                            // The 3 numbers stored in diffNumbers form a naked triple in rows 1, 2, & 3
                                            totalCount = 0;

                                            // Remove number1, number2, and number3 from every cell 
                                            // in column other than row1, row2, and row4
                                            for (int i = 0; i < 9; i++)
                                            {
                                                if (intPuzzle[i, j] == 0 && i != row1 && i != row2 && i != row3)
                                                {
                                                    if (possible[i, j, diffNumbers[0]] == diffNumbers[0] + 1)
                                                    {
                                                        possible[i, j, diffNumbers[0]] = 0;
                                                        totalCount++;
                                                    }
                                                    if (possible[i, j, diffNumbers[1]] == diffNumbers[1] + 1)
                                                    {
                                                        possible[i, j, diffNumbers[1]] = 0;
                                                        totalCount++;
                                                    }
                                                    if (possible[i, j, diffNumbers[2]] == diffNumbers[2] + 1)
                                                    {
                                                        possible[i, j, diffNumbers[2]] = 0;
                                                        totalCount++;
                                                    }
                                                }
                                            }

                                            if (totalCount > 0)
                                            {
                                                tempChanges++;

                                                // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                if (!isGuessAndCheck && !isBruteForce)
                                                {
                                                    Console.WriteLine("Naked Triple Column found something!");
                                                    difficultyScore += 100;
                                                }

                                                // Run previous methods to see if the puzzle can be solved
                                                tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                if (!solved)
                                                {
                                                    tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int NakedTripleGroupCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[,] numberCount = new int[9, 9];
            int[,,] numbers = new int[9, 9, 3];
            int[] tripleRows = new int[9];
            int[] tripleColumns = new int[9];
            int tripleCount = 0;
            int row1 = 0;
            int row2 = 0;
            int row3 = 0;
            int column1 = 0;
            int column2 = 0;
            int column3 = 0;
            int[] diffNumbers = new int[3];
            int[] countDiffNumbers = new int[3];
            int tempCount = 0;
            int tempNumber = 0;
            bool sameFlag = false;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int l = 0; l < 9 && !solved; l++) // group number
                { 
                    for (int x = 0; x < 9; x++)
                    {
                        for (int y = 0; y < 9; y++)
                        {
                            numberCount[x, y] = 0;
                            for (int z = 0; z < 3; z++)
                            {
                                numbers[x, y, z] = 0;
                            }
                        }
                        tripleRows[x] = 0;
                        tripleColumns[x] = 0;
                    }
                    tripleCount = 0;

                    // Goes down the group and counts the number of options in each cell and stores the first three
                    for (int i = ROW_START[l]; i <= ROW_END[l]; i++) // row number
                    { 
                        for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++) // column number
                        { 
                            if (intPuzzle[i, j] == 0)
                            {
                                for (int k = 0; k < 9; k++) // number 1-9
                                { 
                                    if (possible[i, j, k] == k + 1)
                                    {
                                        if (numberCount[i, j] < 3)
                                        {
                                            numbers[i, j, numberCount[i, j]] = k;
                                        }
                                        numberCount[i, j]++;
                                    }
                                }
                            }
                        }
                    }

                    // Counts number of cells with 3 or less options
                    for (int i = ROW_START[l]; i <= ROW_END[l]; i++) // row number
                    { 
                        for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++) // column number
                        { 
                            if (numberCount[i, j] == 2 || numberCount[i, j] == 3)
                            {
                                tripleRows[tripleCount] = i;
                                tripleColumns[tripleCount] = j;
                                tripleCount++;
                            }
                        }
                    }

                    // Checks to see if any of the two or three option cells have the same numbers, forming a naked triple
                    // Then, if a naked triple is found, it updates the possible values
                    if (tripleCount > 2)
                    {
                        // Stage 1
                        for (int a = 0; a <= tripleCount - 3 && !solved; a++)
                        {
                            for (int z = 0; z < 3; z++)
                            {
                                diffNumbers[z] = 0;
                                countDiffNumbers[z] = 0;
                            }
                            row1 = tripleRows[a];
                            column1 = tripleColumns[a];
                            diffNumbers[0] = numbers[row1, column1, 0];
                            diffNumbers[1] = numbers[row1, column1, 1];
                            if (numberCount[row1, column1] == 2)
                            {
                                countDiffNumbers[0] = 2;
                            }
                            if (numberCount[row1, column1] == 3)
                            {
                                diffNumbers[2] = numbers[row1, column1, 2];
                                countDiffNumbers[0] = 3;
                            }

                            // Stage 2
                            for (int b = a + 1; b <= tripleCount - 2 && !solved; b++)
                            {
                                row2 = tripleRows[b];
                                column2 = tripleColumns[b];
                                tempCount = countDiffNumbers[0];

                                for (int x = 0; x < numberCount[row2, column2]; x++)
                                {
                                    sameFlag = false;
                                    tempNumber = numbers[row2, column2, x];
                                    for (int y = 0; y < countDiffNumbers[0]; y++)
                                    {
                                        if (tempNumber == diffNumbers[y])
                                        {
                                            sameFlag = true;
                                        }
                                    }
                                    if (sameFlag == false)
                                    {
                                        if (tempCount < 3)
                                        {
                                            diffNumbers[tempCount] = tempNumber;
                                        }
                                        tempCount++;
                                    }
                                }
                                countDiffNumbers[1] = tempCount;

                                // Stage 3
                                if (countDiffNumbers[1] <= 3) // There still may be a naked triple
                                { 
                                    for (int c = b + 1; c <= tripleCount - 1 && !solved; c++)
                                    {
                                        row3 = tripleRows[c];
                                        column3 = tripleColumns[c];
                                        tempCount = countDiffNumbers[1];

                                        for (int x = 0; x < numberCount[row3, column3]; x++)
                                        {
                                            sameFlag = false;
                                            tempNumber = numbers[row3, column3, x];
                                            for (int y = 0; y < countDiffNumbers[1]; y++)
                                            {
                                                if (tempNumber == diffNumbers[y])
                                                {
                                                    sameFlag = true;
                                                }
                                            }
                                            if (sameFlag == false)
                                            {
                                                if (tempCount < 3)
                                                {
                                                    diffNumbers[tempCount] = tempNumber;
                                                }
                                                tempCount++;
                                            }
                                        }
                                        countDiffNumbers[2] = tempCount;

                                        if (countDiffNumbers[2] == 3)
                                        {
                                            // The 3 numbers stored in diffNumbers form a naked triple in cells 1, 2, & 3
                                            totalCount = 0;

                                            // Remove number1, number2, and number3 from every cell 
                                            // in group other than cells 1, 2, and 4
                                            for (int i = ROW_START[l]; i <= ROW_END[l]; i++)
                                            {
                                                for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++)
                                                {
                                                    if (intPuzzle[i, j] == 0 && !(i == row1 && j == column1) && !(i == row2 && j == column2) &&
                                                            !(i == row3 && j == column3))
                                                    {
                                                        if (possible[i, j, diffNumbers[0]] == diffNumbers[0] + 1)
                                                        {
                                                            possible[i, j, diffNumbers[0]] = 0;
                                                            totalCount++;
                                                        }
                                                        if (possible[i, j, diffNumbers[1]] == diffNumbers[1] + 1)
                                                        {
                                                            possible[i, j, diffNumbers[1]] = 0;
                                                            totalCount++;
                                                        }
                                                        if (possible[i, j, diffNumbers[2]] == diffNumbers[2] + 1)
                                                        {
                                                            possible[i, j, diffNumbers[2]] = 0;
                                                            totalCount++;
                                                        }
                                                    }
                                                }
                                            }

                                            if (totalCount > 0)
                                            {
                                                
                                                tempChanges++;

                                                // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                if (!isGuessAndCheck && !isBruteForce)
                                                {
                                                    Console.WriteLine("Naked Triple Group found something!");
                                                    difficultyScore += 100;
                                                }

                                                // Run previous methods to see if the puzzle can be solved
                                                tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                if (!solved)
                                                {
                                                    tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int HiddenTripleChecks(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes;
            int tempHiddenTripleChanges = 0;

            do
            {
                changes = 0;
                changes += HiddenTripleRowCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += HiddenTripleColumnCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += HiddenTripleGroupCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                tempHiddenTripleChanges += changes;
            } while (changes != 0);

            return tempHiddenTripleChanges;
        }

        public static int HiddenTripleRowCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[,] columns = new int[9, 3];
            int[] columnsCount = new int[9];
            int[] tripleNumbers = new int[9];
            int tripleCount = 0;
            int number1 = 0;
            int number2 = 0;
            int number3 = 0;
            int[] diffColumns = new int[3];
            int[] countDiffColumns = new int[3];
            int tempCount = 0;
            int tempColumn = 0;
            bool sameFlag = false;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int i = 0; i < 9 && !solved; i++) // row number
                { 
                    for (int z = 0; z < 9; z++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            columns[z, y] = 0;
                        }
                        columnsCount[z] = 0;
                        tripleNumbers[z] = 0;
                    }
                    tripleCount = 0;

                    // Goes down the row, counts how many columns each number appears in, then stores the first three
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (!Rows[i, k])
                        {
                            for (int j = 0; j < 9; j++) // column number
                            { 
                                if (intPuzzle[i, j] == 0 && possible[i, j, k] == k + 1)
                                {
                                    if (columnsCount[k] < 3)
                                    {
                                        columns[k, columnsCount[k]] = j;
                                    }
                                    columnsCount[k]++;
                                }
                            }
                        }
                    }

                    // Counts the number of numbers 1-9 that appear in only 2 or 3 columns
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (columnsCount[k] == 2 || columnsCount[k] == 3)
                        {
                            tripleNumbers[tripleCount] = k;
                            tripleCount++;
                        }
                    }

                    // Checks to see if any of the numbers match any other numbers, forming a hidden triple
                    // Then, if a hidden triple is found, it updates the possible values
                    if (tripleCount > 2)
                    {
                        // Stage 1
                        for (int a = 0; a <= tripleCount - 3 && !solved; a++)
                        {
                            for (int z = 0; z < 3; z++)
                            {
                                diffColumns[z] = 0;
                                countDiffColumns[z] = 0;
                            }
                            number1 = tripleNumbers[a];
                            diffColumns[0] = columns[number1, 0];
                            diffColumns[1] = columns[number1, 1];
                            if (columnsCount[number1] == 2)
                            {
                                countDiffColumns[0] = 2;
                            }
                            if (columnsCount[number1] == 3)
                            {
                                diffColumns[2] = columns[number1, 2];
                                countDiffColumns[0] = 3;
                            }

                            // Stage 2
                            for (int b = a + 1; b <= tripleCount - 2 && !solved; b++)
                            {
                                number2 = tripleNumbers[b];
                                tempCount = countDiffColumns[0];

                                for (int x = 0; x < columnsCount[number2]; x++)
                                {
                                    sameFlag = false;
                                    tempColumn = columns[number2, x];
                                    for (int y = 0; y < countDiffColumns[0]; y++)
                                    {
                                        if (tempColumn == diffColumns[y])
                                        {
                                            sameFlag = true;
                                        }
                                    }
                                    if (sameFlag == false)
                                    {
                                        if (tempCount < 3)
                                        {
                                            diffColumns[tempCount] = tempColumn;
                                        }
                                        tempCount++;
                                    }
                                }
                                countDiffColumns[1] = tempCount;

                                // Stage 3
                                if (countDiffColumns[1] <= 3) // There still may be a hidden triple
                                { 
                                    for (int c = b + 1; c <= tripleCount - 1 && !solved; c++)
                                    {
                                        number3 = tripleNumbers[c];
                                        tempCount = countDiffColumns[1];

                                        for (int x = 0; x < columnsCount[number3]; x++)
                                        {
                                            sameFlag = false;
                                            tempColumn = columns[number3, x];
                                            for (int y = 0; y < countDiffColumns[1]; y++)
                                            {
                                                if (tempColumn == diffColumns[y])
                                                {
                                                    sameFlag = true;
                                                }
                                            }
                                            if (sameFlag == false)
                                            {
                                                if (tempCount < 3)
                                                {
                                                    diffColumns[tempCount] = tempColumn;
                                                }
                                                tempCount++;
                                            }
                                        }
                                        countDiffColumns[2] = tempCount;

                                        if (countDiffColumns[2] == 3)
                                        {
                                            // number1, number2 and number3 form a hidden triple in column1, column2 and column3
                                            totalCount = 0;

                                            // Remove all numbers except number1, number2, number3 from 
                                            // all columns except column1, column2, and column3
                                            for (int k = 0; k < 9; k++)
                                            {
                                                if (k != number1 && k != number2 && k != number3)
                                                {
                                                    if (possible[i, diffColumns[0], k] == k + 1)
                                                    {
                                                        possible[i, diffColumns[0], k] = 0;
                                                        totalCount++;
                                                    }
                                                    if (possible[i, diffColumns[1], k] == k + 1)
                                                    {
                                                        possible[i, diffColumns[1], k] = 0;
                                                        totalCount++;
                                                    }
                                                    if (possible[i, diffColumns[2], k] == k + 1)
                                                    {
                                                        possible[i, diffColumns[2], k] = 0;
                                                        totalCount++;
                                                    }
                                                }
                                            }

                                            if (totalCount > 0)
                                            {
                                                tempChanges++;

                                                // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                if (!isGuessAndCheck && !isBruteForce)
                                                {
                                                    Console.WriteLine("Hidden Triple Row found something!");
                                                    difficultyScore += 100;
                                                }

                                                // Run previous methods to see if the puzzle can be solved
                                                tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                if (!solved)
                                                {
                                                    tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int HiddenTripleColumnCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[,] rows = new int[9, 3];
            int[] rowsCount = new int[9];
            int[] tripleNumbers = new int[9];
            int tripleCount = 0;
            int number1 = 0;
            int number2 = 0;
            int number3 = 0;
            int[] diffRows = new int[3];
            int[] countDiffRows = new int[3];
            int tempCount = 0;
            int tempRow = 0;
            bool sameFlag = false;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int j = 0; j < 9 && !solved; j++) // column number
                { 
                    for (int z = 0; z < 9; z++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            rows[z, y] = 0;
                        }
                        rowsCount[z] = 0;
                        tripleNumbers[z] = 0;
                    }
                    tripleCount = 0;

                    // Goes down the column, counts how many rows each number appears in, then stores the first three
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (!Columns[j, k])
                        {
                            for (int i = 0; i < 9; i++) // row number
                            { 
                                if (intPuzzle[i, j] == 0 && possible[i, j, k] == k + 1)
                                {
                                    if (rowsCount[k] < 3)
                                    {
                                        rows[k, rowsCount[k]] = i;
                                    }
                                    rowsCount[k]++;
                                }
                            }
                        }
                    }

                    // Counts the number of numbers 1-9 that appear in only 2 or 3 rows
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (rowsCount[k] == 2 || rowsCount[k] == 3)
                        {
                            tripleNumbers[tripleCount] = k;
                            tripleCount++;
                        }
                    }

                    // Checks to see if any of the numbers match any other numbers, forming a hidden triple
                    // Then, if a hidden triple is found, it updates the possible values
                    if (tripleCount > 2)
                    {
                        // Stage 1
                        for (int a = 0; a <= tripleCount - 3 && !solved; a++)
                        {
                            for (int z = 0; z < 3; z++)
                            {
                                diffRows[z] = 0;
                                countDiffRows[z] = 0;
                            }
                            number1 = tripleNumbers[a];
                            diffRows[0] = rows[number1, 0];
                            diffRows[1] = rows[number1, 1];
                            if (rowsCount[number1] == 2)
                            {
                                countDiffRows[0] = 2;
                            }
                            if (rowsCount[number1] == 3)
                            {
                                diffRows[2] = rows[number1, 2];
                                countDiffRows[0] = 3;
                            }

                            // Stage 2
                            for (int b = a + 1; b <= tripleCount - 2 && !solved; b++)
                            {
                                number2 = tripleNumbers[b];
                                tempCount = countDiffRows[0];

                                for (int x = 0; x < rowsCount[number2]; x++)
                                {
                                    sameFlag = false;
                                    tempRow = rows[number2, x];
                                    for (int y = 0; y < countDiffRows[0]; y++)
                                    {
                                        if (tempRow == diffRows[y])
                                        {
                                            sameFlag = true;
                                        }
                                    }
                                    if (sameFlag == false)
                                    {
                                        if (tempCount < 3)
                                        {
                                            diffRows[tempCount] = tempRow;
                                        }
                                        tempCount++;
                                    }
                                }
                                countDiffRows[1] = tempCount;

                                // Stage 3
                                if (countDiffRows[1] <= 3) // There still may be a hidden triple
                                {
                                    for (int c = b + 1; c <= tripleCount - 1 && !solved; c++)
                                    {
                                        number3 = tripleNumbers[c];
                                        tempCount = countDiffRows[1];

                                        for (int x = 0; x < rowsCount[number3]; x++)
                                        {
                                            sameFlag = false;
                                            tempRow = rows[number3, x];
                                            for (int y = 0; y < countDiffRows[1]; y++)
                                            {
                                                if (tempRow == diffRows[y])
                                                {
                                                    sameFlag = true;
                                                }
                                            }
                                            if (sameFlag == false)
                                            {
                                                if (tempCount < 3)
                                                {
                                                    diffRows[tempCount] = tempRow;
                                                }
                                                tempCount++;
                                            }
                                        }
                                        countDiffRows[2] = tempCount;

                                        if (countDiffRows[2] == 3)
                                        {
                                            // number1, number2 and number3 form a hidden triple in row1, row2 and row3
                                            totalCount = 0;

                                            // Remove all numbers except number1, number2, number3 from 
                                            // all rows except row1, row2, and row3
                                            for (int k = 0; k < 9; k++)
                                            {
                                                if (k != number1 && k != number2 && k != number3)
                                                {
                                                    if (possible[diffRows[0], j, k] == k + 1)
                                                    {
                                                        possible[diffRows[0], j, k] = 0;
                                                        totalCount++;
                                                    }
                                                    if (possible[diffRows[1], j, k] == k + 1)
                                                    {
                                                        possible[diffRows[1], j, k] = 0;
                                                        totalCount++;
                                                    }
                                                    if (possible[diffRows[2], j, k] == k + 1)
                                                    {
                                                        possible[diffRows[2], j, k] = 0;
                                                        totalCount++;
                                                    }
                                                }
                                            }

                                            if (totalCount > 0)
                                            {
                                                tempChanges++;

                                                // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                if (!isGuessAndCheck && !isBruteForce)
                                                {
                                                    Console.WriteLine("Hidden Triple Column found something!");
                                                    difficultyScore += 100;
                                                }

                                                // Run previous methods to see if the puzzle can be solved
                                                tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                if (!solved)
                                                {
                                                    tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int HiddenTripleGroupCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[,] rows = new int[9, 3];
            int[,] columns = new int[9, 3];
            int[] cellCount = new int[9];
            int[] tripleNumbers = new int[9];
            int tripleCount = 0;
            int number1 = 0;
            int number2 = 0;
            int number3 = 0;
            int[] diffRows = new int[3];
            int[] diffColumns = new int[3];
            int[] countDiffCells = new int[3];
            int tempCount = 0;
            int tempRow = 0;
            int tempColumn = 0;
            bool sameFlag = false;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int l = 0; l < 9 && !solved; l++) // group number
                { 
                    for (int z = 0; z < 9; z++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            rows[z, y] = 0;
                            columns[z, y] = 0;
                        }
                        cellCount[z] = 0;
                        tripleNumbers[z] = 0;
                    }
                    tripleCount = 0;

                    // Goes down the group, counts how many cells each number appears in, then stores the first three
                    for (int k = 0; k < 9; k++) // number 1-9
                    { 
                        if (!Groups[l, k])
                        {
                            for (int i = ROW_START[l]; i <= ROW_END[l]; i++) // row number
                            { 
                                for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++) // column number
                                { 
                                    if (intPuzzle[i, j] == 0 && possible[i, j, k] == k + 1)
                                    {
                                        if (cellCount[k] < 3)
                                        {
                                            rows[k, cellCount[k]] = i;
                                            columns[k, cellCount[k]] = j;
                                        }
                                        cellCount[k]++;
                                    }
                                }
                            }
                        }
                    }

                    // Counts the number of numbers 1-9 that appear in only 2 or 3 cells
                    for (int k = 0; k < 9; k++)  // number 1-9
                    { 
                        if (cellCount[k] == 2 || cellCount[k] == 3)
                        {
                            tripleNumbers[tripleCount] = k;
                            tripleCount++;
                        }
                    }

                    // Checks to see if any of the numbers match any other numbers, forming a hidden triple
                    // Then, if a hidden triple is found, it updates the possible values
                    if (tripleCount > 2)
                    {
                        // Stage 1
                        for (int a = 0; a <= tripleCount - 3 && !solved; a++)
                        {
                            for (int z = 0; z < 3; z++)
                            {
                                diffRows[z] = 0;
                                diffColumns[z] = 0;
                                countDiffCells[z] = 0;
                            }
                            number1 = tripleNumbers[a];
                            diffRows[0] = rows[number1, 0];
                            diffColumns[0] = columns[number1, 0];
                            diffRows[1] = rows[number1, 1];
                            diffColumns[1] = columns[number1, 1];
                            if (cellCount[number1] == 2)
                            {
                                countDiffCells[0] = 2;
                            }
                            if (cellCount[number1] == 3)
                            {
                                diffRows[2] = rows[number1, 2];
                                diffColumns[2] = columns[number1, 2];
                                countDiffCells[0] = 3;
                            }

                            // Stage 2
                            for (int b = a + 1; b <= tripleCount - 2 && !solved; b++)
                            {
                                number2 = tripleNumbers[b];
                                tempCount = countDiffCells[0];

                                for (int x = 0; x < cellCount[number2]; x++)
                                {
                                    sameFlag = false;
                                    tempRow = rows[number2, x];
                                    tempColumn = columns[number2, x];
                                    for (int y = 0; y < countDiffCells[0]; y++)
                                    {
                                        if (tempRow == diffRows[y] && tempColumn == diffColumns[y])
                                        {
                                            sameFlag = true;
                                        }
                                    }
                                    if (sameFlag == false)
                                    {
                                        if (tempCount < 3)
                                        {
                                            diffRows[tempCount] = tempRow;
                                            diffColumns[tempCount] = tempColumn;
                                        }
                                        tempCount++;
                                    }
                                }
                                countDiffCells[1] = tempCount;

                                // Stage 3
                                if (countDiffCells[1] <= 3) // There still may be a hidden triple
                                { 
                                    for (int c = b + 1; c <= tripleCount - 1 && !solved; c++)
                                    {
                                        number3 = tripleNumbers[c];
                                        tempCount = countDiffCells[1];

                                        for (int x = 0; x < cellCount[number3]; x++)
                                        {
                                            sameFlag = false;
                                            tempRow = rows[number3, x];
                                            tempColumn = columns[number3, x];
                                            for (int y = 0; y < countDiffCells[1]; y++)
                                            {
                                                if (tempRow == diffRows[y] && tempColumn == diffColumns[y])
                                                {
                                                    sameFlag = true;
                                                }
                                            }
                                            if (sameFlag == false)
                                            {
                                                if (tempCount < 3)
                                                {
                                                    diffRows[tempCount] = tempRow;
                                                    diffColumns[tempCount] = tempColumn;
                                                }
                                                tempCount++;
                                            }
                                        }
                                        countDiffCells[2] = tempCount;

                                        if (countDiffCells[2] == 3)
                                        {
                                            // number1, number2 and number3 form a hidden triple in cells 1, 2, and 3
                                            totalCount = 0;

                                            // Remove all numbers except number1, number2, number3 from 
                                            // all cells except cells 1, 2, and 3
                                            for (int k = 0; k < 9; k++)
                                            {
                                                if (k != number1 && k != number2 && k != number3)
                                                {
                                                    if (possible[diffRows[0], diffColumns[0], k] == k + 1)
                                                    {
                                                        possible[diffRows[0], diffColumns[0], k] = 0;
                                                        totalCount++;
                                                    }
                                                    if (possible[diffRows[1], diffColumns[1], k] == k + 1)
                                                    {
                                                        possible[diffRows[1], diffColumns[1], k] = 0;
                                                        totalCount++;
                                                    }
                                                    if (possible[diffRows[2], diffColumns[2], k] == k + 1)
                                                    {
                                                        possible[diffRows[2], diffColumns[2], k] = 0;
                                                        totalCount++;
                                                    }
                                                }
                                            }

                                            if (totalCount > 0)
                                            {
                                                tempChanges++;

                                                // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                if (!isGuessAndCheck && !isBruteForce)
                                                {
                                                    Console.WriteLine("Hidden Triple Group found something!");
                                                    difficultyScore += 100;
                                                }

                                                // Run previous methods to see if the puzzle can be solved
                                                tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                if (!solved)
                                                {
                                                    tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int NakedQuadChecks(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes;
            int tempNakedQuadChanges = 0;

            do
            {
                changes = 0;
                changes += NakedQuadRowCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += NakedQuadColumnCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += NakedQuadGroupCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce); 
                tempNakedQuadChanges += changes;
            } while (changes != 0 && !solved);

            return tempNakedQuadChanges;
        }

        public static int NakedQuadRowCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[] numberCount = new int[9];
            int[,] numbers = new int[9, 4];
            int[] quadColumns = new int[9];
            int quadCount = 0;
            int column1 = 0;
            int column2 = 0;
            int column3 = 0;
            int column4 = 0;
            int[] diffNumbers = new int[4];
            int[] countDiffNumbers = new int[4];
            int tempCount = 0;
            int tempNumber = 0;
            bool sameFlag = false;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int i = 0; i < 9 && !solved; i++) // row number
                { 
                    for (int z = 0; z < 9; z++)
                    {
                        for (int y = 0; y < 4; y++)
                        {
                            numbers[z, y] = 0;
                        }
                        numberCount[z] = 0;
                        quadColumns[z] = 0;
                    }
                    quadCount = 0;

                    // Goes down the row and counts the number of options in each cell and stores the first four
                    for (int j = 0; j < 9; j++) // column number
                    { 
                        if (intPuzzle[i, j] == 0)
                        {
                            for (int k = 0; k < 9; k++) // number 1-9
                            { 
                                if (possible[i, j, k] == k + 1)
                                {
                                    if (numberCount[j] < 4)
                                    {
                                        numbers[j, numberCount[j]] = k;
                                    }
                                    numberCount[j]++;
                                }
                            }
                        }
                    }

                    // Counts number of cells with 4 or less options
                    for (int j = 0; j < 9; j++) // column number
                    { 
                        if (numberCount[j] == 2 || numberCount[j] == 3 || numberCount[j] == 4)
                        {
                            quadColumns[quadCount] = j;
                            quadCount++;
                        }
                    }

                    // Checks to see if any of the two, three or four option cells have the same numbers, forming a naked quad
                    // Then, if a naked quad is found, it updates the possible values
                    if (quadCount > 3)
                    {
                        // Stage 1
                        for (int a = 0; a <= quadCount - 4 && !solved; a++)
                        {
                            for (int z = 0; z < 4; z++)
                            {
                                diffNumbers[z] = 0;
                                countDiffNumbers[z] = 0;
                            }
                            column1 = quadColumns[a];
                            diffNumbers[0] = numbers[column1, 0];
                            diffNumbers[1] = numbers[column1, 1];
                            if (numberCount[column1] == 2)
                            {
                                countDiffNumbers[0] = 2;
                            }
                            if (numberCount[column1] == 3)
                            {
                                diffNumbers[2] = numbers[column1, 2];
                                countDiffNumbers[0] = 3;
                            }
                            if (numberCount[column1] == 4)
                            {
                                diffNumbers[2] = numbers[column1, 2];
                                diffNumbers[3] = numbers[column1, 3];
                                countDiffNumbers[0] = 4;
                            }

                            // Stage 2
                            for (int b = a + 1; b <= quadCount - 3 && !solved; b++)
                            {
                                column2 = quadColumns[b];
                                tempCount = countDiffNumbers[0];

                                for (int x = 0; x < numberCount[column2]; x++)
                                {
                                    sameFlag = false;
                                    tempNumber = numbers[column2, x];
                                    for (int y = 0; y < countDiffNumbers[0]; y++)
                                    {
                                        if (tempNumber == diffNumbers[y])
                                        {
                                            sameFlag = true;
                                        }
                                    }
                                    if (sameFlag == false)
                                    {
                                        if (tempCount < 4)
                                        {
                                            diffNumbers[tempCount] = tempNumber;
                                        }
                                        tempCount++;
                                    }
                                }
                                countDiffNumbers[1] = tempCount;

                                // Stage 3
                                if (countDiffNumbers[1] <= 4) // There still may be a naked quad
                                { 
                                    for (int c = b + 1; c <= quadCount - 2 && !solved; c++)
                                    {
                                        column3 = quadColumns[c];
                                        tempCount = countDiffNumbers[1];

                                        for (int x = 0; x < numberCount[column3]; x++)
                                        {
                                            sameFlag = false;
                                            tempNumber = numbers[column3, x];
                                            for (int y = 0; y < countDiffNumbers[1]; y++)
                                            {
                                                if (tempNumber == diffNumbers[y])
                                                {
                                                    sameFlag = true;
                                                }
                                            }
                                            if (sameFlag == false)
                                            {
                                                if (tempCount < 4)
                                                {
                                                    diffNumbers[tempCount] = tempNumber;
                                                }
                                                tempCount++;
                                            }
                                        }
                                        countDiffNumbers[2] = tempCount;

                                        // Stage 4
                                        if (countDiffNumbers[2] == 4) // There still may be a naked quad
                                        { 
                                            for (int d = c + 1; d <= quadCount - 1 && !solved; d++)
                                            {
                                                column4 = quadColumns[d];
                                                tempCount = countDiffNumbers[2];

                                                for (int x = 0; x < numberCount[column4]; x++)
                                                {
                                                    sameFlag = false;
                                                    tempNumber = numbers[column4, x];
                                                    for (int y = 0; y < countDiffNumbers[2]; y++)
                                                    {
                                                        if (tempNumber == diffNumbers[y])
                                                        {
                                                            sameFlag = true;
                                                        }
                                                    }
                                                    if (sameFlag == false)
                                                    {
                                                        if (tempCount < 4)
                                                        {
                                                            diffNumbers[tempCount] = tempNumber;
                                                        }
                                                        tempCount++;
                                                    }
                                                }
                                                countDiffNumbers[3] = tempCount;

                                                if (countDiffNumbers[3] == 4)
                                                {
                                                    // The 4 numbers stored in diffNumbers form a naked quad in columns 1, 2, 3, & 4
                                                    totalCount = 0;

                                                    // Remove number1, number2, number3, and number4 from every cell 
                                                    // in row other than column1, column2, column3, and column4
                                                    for (int j = 0; j < 9; j++)
                                                    {
                                                        if (intPuzzle[i, j] == 0 && j != column1 && j != column2 && j != column3 && j != column4)
                                                        {
                                                            if (possible[i, j, diffNumbers[0]] == diffNumbers[0] + 1)
                                                            {
                                                                possible[i, j, diffNumbers[0]] = 0;
                                                                totalCount++;
                                                            }
                                                            if (possible[i, j, diffNumbers[1]] == diffNumbers[1] + 1)
                                                            {
                                                                possible[i, j, diffNumbers[1]] = 0;
                                                                totalCount++;
                                                            }
                                                            if (possible[i, j, diffNumbers[2]] == diffNumbers[2] + 1)
                                                            {
                                                                possible[i, j, diffNumbers[2]] = 0;
                                                                totalCount++;
                                                            }
                                                            if (possible[i, j, diffNumbers[3]] == diffNumbers[3] + 1)
                                                            {
                                                                possible[i, j, diffNumbers[3]] = 0;
                                                                totalCount++;
                                                            }
                                                        }
                                                    }

                                                    if (totalCount > 0)
                                                    {
                                                        tempChanges++;

                                                        // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                        if (!isGuessAndCheck && !isBruteForce)
                                                        {
                                                            Console.WriteLine("Naked Quad Row found something!");
                                                            difficultyScore += 100;
                                                        }

                                                        // Run previous methods to see if the puzzle can be solved
                                                        tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                        if (!solved)
                                                        {
                                                            tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int NakedQuadColumnCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[] numberCount = new int[9];
            int[,] numbers = new int[9, 4];
            int[] quadRows = new int[9];
            int quadCount = 0;
            int row1 = 0;
            int row2 = 0;
            int row3 = 0;
            int row4 = 0;
            int[] diffNumbers = new int[4];
            int[] countDiffNumbers = new int[4];
            int tempCount = 0;
            int tempNumber = 0;
            bool sameFlag = false;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int j = 0; j < 9 && !solved; j++) // column number
                { 
                    for (int z = 0; z < 9; z++)
                    {
                        for (int y = 0; y < 4; y++)
                        {
                            numbers[z, y] = 0;
                        }
                        numberCount[z] = 0;
                        quadRows[z] = 0;
                    }
                    quadCount = 0;

                    // Goes down the column and counts the number of options in each cell and stores the first four
                    for (int i = 0; i < 9; i++) // row number
                    { 
                        if (intPuzzle[i, j] == 0)
                        {
                            for (int k = 0; k < 9; k++) // number 1-9
                            { 
                                if (possible[i, j, k] == k + 1)
                                {
                                    if (numberCount[i] < 4)
                                    {
                                        numbers[i, numberCount[i]] = k;
                                    }
                                    numberCount[i]++;
                                }
                            }
                        }
                    }

                    // Counts number of cells with 4 or less options
                    for (int i = 0; i < 9; i++) // row number
                    { 
                        if (numberCount[i] == 2 || numberCount[i] == 3 || numberCount[i] == 4)
                        {
                            quadRows[quadCount] = i;
                            quadCount++;
                        }
                    }

                    // Checks to see if any of the two, three or four option cells have the same numbers, forming a naked quad
                    // Then, if a naked quad is found, it updates the possible values
                    if (quadCount > 3)
                    {
                        // Stage 1
                        for (int a = 0; a <= quadCount - 4 && !solved; a++)
                        {
                            for (int z = 0; z < 4; z++)
                            {
                                diffNumbers[z] = 0;
                                countDiffNumbers[z] = 0;
                            }
                            row1 = quadRows[a];
                            diffNumbers[0] = numbers[row1, 0];
                            diffNumbers[1] = numbers[row1, 1];
                            if (numberCount[row1] == 2)
                            {
                                countDiffNumbers[0] = 2;
                            }
                            if (numberCount[row1] == 3)
                            {
                                diffNumbers[2] = numbers[row1, 2];
                                countDiffNumbers[0] = 3;
                            }
                            if (numberCount[row1] == 4)
                            {
                                diffNumbers[2] = numbers[row1, 2];
                                diffNumbers[3] = numbers[row1, 3];
                                countDiffNumbers[0] = 4;
                            }

                            // Stage 2
                            for (int b = a + 1; b <= quadCount - 3 && !solved; b++)
                            {
                                row2 = quadRows[b];
                                tempCount = countDiffNumbers[0];

                                for (int x = 0; x < numberCount[row2]; x++)
                                {
                                    sameFlag = false;
                                    tempNumber = numbers[row2, x];
                                    for (int y = 0; y < countDiffNumbers[0]; y++)
                                    {
                                        if (tempNumber == diffNumbers[y])
                                        {
                                            sameFlag = true;
                                        }
                                    }
                                    if (sameFlag == false)
                                    {
                                        if (tempCount < 4)
                                        {
                                            diffNumbers[tempCount] = tempNumber;
                                        }
                                        tempCount++;
                                    }
                                }
                                countDiffNumbers[1] = tempCount;

                                // Stage 3
                                if (countDiffNumbers[1] <= 4) // There still may be a naked quad
                                { 
                                    for (int c = b + 1; c <= quadCount - 2 && !solved; c++)
                                    {
                                        row3 = quadRows[c];
                                        tempCount = countDiffNumbers[1];

                                        for (int x = 0; x < numberCount[row3]; x++)
                                        {
                                            sameFlag = false;
                                            tempNumber = numbers[row3, x];
                                            for (int y = 0; y < countDiffNumbers[1]; y++)
                                            {
                                                if (tempNumber == diffNumbers[y])
                                                {
                                                    sameFlag = true;
                                                }
                                            }
                                            if (sameFlag == false)
                                            {
                                                if (tempCount < 4)
                                                {
                                                    diffNumbers[tempCount] = tempNumber;
                                                }
                                                tempCount++;
                                            }
                                        }
                                        countDiffNumbers[2] = tempCount;

                                        // Stage 4
                                        if (countDiffNumbers[2] == 4) // There still may be a naked quad
                                        { 
                                            for (int d = c + 1; d <= quadCount - 1 && !solved; d++)
                                            {
                                                row4 = quadRows[d];
                                                tempCount = countDiffNumbers[2];

                                                for (int x = 0; x < numberCount[row4]; x++)
                                                {
                                                    sameFlag = false;
                                                    tempNumber = numbers[row4, x];
                                                    for (int y = 0; y < countDiffNumbers[2]; y++)
                                                    {
                                                        if (tempNumber == diffNumbers[y])
                                                        {
                                                            sameFlag = true;
                                                        }
                                                    }
                                                    if (sameFlag == false)
                                                    {
                                                        if (tempCount < 4)
                                                        {
                                                            diffNumbers[tempCount] = tempNumber;
                                                        }
                                                        tempCount++;
                                                    }
                                                }
                                                countDiffNumbers[3] = tempCount;

                                                if (countDiffNumbers[3] == 4)
                                                {
                                                    // The 4 numbers stored in diffNumbers form a naked quad in rows 1, 2, 3, & 4
                                                    totalCount = 0;

                                                    // Remove number1, number2, number3, and number4 from every cell 
                                                    // in column other than row1, row2, row3, and row4
                                                    for (int i = 0; i < 9; i++)
                                                    {
                                                        if (intPuzzle[i, j] == 0 && i != row1 && i != row2 && i != row3 && i != row4)
                                                        {
                                                            if (possible[i, j, diffNumbers[0]] == diffNumbers[0] + 1)
                                                            {
                                                                possible[i, j, diffNumbers[0]] = 0;
                                                                totalCount++;
                                                            }
                                                            if (possible[i, j, diffNumbers[1]] == diffNumbers[1] + 1)
                                                            {
                                                                possible[i, j, diffNumbers[1]] = 0;
                                                                totalCount++;
                                                            }
                                                            if (possible[i, j, diffNumbers[2]] == diffNumbers[2] + 1)
                                                            {
                                                                possible[i, j, diffNumbers[2]] = 0;
                                                                totalCount++;
                                                            }
                                                            if (possible[i, j, diffNumbers[3]] == diffNumbers[3] + 1)
                                                            {
                                                                possible[i, j, diffNumbers[3]] = 0;
                                                                totalCount++;
                                                            }
                                                        }
                                                    }

                                                    if (totalCount > 0)
                                                    {
                                                        tempChanges++;

                                                        // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                        if (!isGuessAndCheck && !isBruteForce)
                                                        {
                                                            Console.WriteLine("Naked Quad Column found something!");
                                                            difficultyScore += 100;
                                                        }

                                                        // Run previous methods to see if the puzzle can be solved
                                                        tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                        if (!solved)
                                                        {
                                                            tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int NakedQuadGroupCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[,] numberCount = new int[9, 9];
            int[,,] numbers = new int[9, 9, 4];
            int[] quadRows = new int[9];
            int[] quadColumns = new int[9];
            int quadCount = 0;
            int row1 = 0;
            int row2 = 0;
            int row3 = 0;
            int row4 = 0;
            int column1 = 0;
            int column2 = 0;
            int column3 = 0;
            int column4 = 0;
            int[] diffNumbers = new int[4];
            int[] countDiffNumbers = new int[4];
            int tempCount = 0;
            int tempNumber = 0;
            bool sameFlag = false;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int l = 0; l < 9 && !solved; l++) // group number
                {
                    for (int x = 0; x < 9; x++)
                    {
                        for (int y = 0; y < 9; y++)
                        {
                            numberCount[x, y] = 0;
                            for (int z = 0; z < 4; z++)
                            {
                                numbers[x, y, z] = 0;
                            }
                        }
                        quadRows[x] = 0;
                        quadColumns[x] = 0;
                    }
                    quadCount = 0;

                    // Goes down the group and counts the number of options in each cell and stores the first four
                    for (int i = ROW_START[l]; i <= ROW_END[l]; i++) // row number
                    { 
                        for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++) // column number
                        { 
                            if (intPuzzle[i, j] == 0)
                            {
                                for (int k = 0; k < 9; k++) // number 1-9
                                { 
                                    if (possible[i, j, k] == k + 1)
                                    {
                                        if (numberCount[i, j] < 4)
                                        {
                                            numbers[i, j, numberCount[i, j]] = k;
                                        }
                                        numberCount[i, j]++;
                                    }
                                }
                            }
                        }
                    }

                    // Counts number of cells with 4 or less options
                    for (int i = ROW_START[l]; i <= ROW_END[l]; i++) // row number
                    { 
                        for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++) // column number
                        { 
                            if (numberCount[i, j] == 2 || numberCount[i, j] == 3 || numberCount[i, j] == 4)
                            {
                                quadRows[quadCount] = i;
                                quadColumns[quadCount] = j;
                                quadCount++;
                            }
                        }
                    }

                    // Checks to see if any of the two, three or four option cells have the same numbers, forming a naked quad
                    // Then, if a naked quad is found, it updates the possible values
                    if (quadCount > 3)
                    {
                        // Stage 1
                        for (int a = 0; a <= quadCount - 4 && !solved; a++)
                        {
                            for (int z = 0; z < 4;  z++)
                            {
                                diffNumbers[z] = 0;
                                countDiffNumbers[z] = 0;
                            }
                            row1 = quadRows[a];
                            column1 = quadColumns[a];
                            diffNumbers[0] = numbers[row1, column1, 0];
                            diffNumbers[1] = numbers[row1, column1, 1];
                            if (numberCount[row1, column1] == 2)
                            {
                                countDiffNumbers[0] = 2;
                            }
                            if (numberCount[row1, column1] == 3)
                            {
                                diffNumbers[2] = numbers[row1, column1, 2];
                                countDiffNumbers[0] = 3;
                            }
                            if (numberCount[row1, column1] == 4)
                            {
                                diffNumbers[2] = numbers[row1, column1, 2];
                                diffNumbers[3] = numbers[row1, column1, 3];
                                countDiffNumbers[0] = 4;
                            }

                            // Stage 2
                            for (int b = a + 1; b <= quadCount - 3 && !solved; b++)
                            {
                                row2 = quadRows[b];
                                column2 = quadColumns[b];
                                tempCount = countDiffNumbers[0];

                                for (int x = 0; x < numberCount[row2, column2]; x++)
                                {
                                    sameFlag = false;
                                    tempNumber = numbers[row2, column2, x];
                                    for (int y = 0; y < countDiffNumbers[0]; y++)
                                    {
                                        if (tempNumber == diffNumbers[y])
                                        {
                                            sameFlag = true;
                                        }
                                    }
                                    if (sameFlag == false)
                                    {
                                        if (tempCount < 4)
                                        {
                                            diffNumbers[tempCount] = tempNumber;
                                        }
                                        tempCount++;
                                    }
                                }
                                countDiffNumbers[1] = tempCount;

                                // Stage 3
                                if (countDiffNumbers[1] <= 4) // There still may be a naked quad
                                { 
                                    for (int c = b + 1; c <= quadCount - 2 && !solved; c++)
                                    {
                                        row3 = quadRows[c];
                                        column3 = quadColumns[c];
                                        tempCount = countDiffNumbers[1];

                                        for (int x = 0; x < numberCount[row3, column3]; x++)
                                        {
                                            sameFlag = false;
                                            tempNumber = numbers[row3, column3, x];
                                            for (int y = 0; y < countDiffNumbers[1]; y++)
                                            {
                                                if (tempNumber == diffNumbers[y])
                                                {
                                                    sameFlag = true;
                                                }
                                            }
                                            if (sameFlag == false)
                                            {
                                                if (tempCount < 4)
                                                {
                                                    diffNumbers[tempCount] = tempNumber;
                                                }
                                                tempCount++;
                                            }
                                        }
                                        countDiffNumbers[2] = tempCount;

                                        // Stage 4
                                        if (countDiffNumbers[2] == 4) // There still may be a naked quad
                                        { 
                                            for (int d = c + 1; d <= quadCount - 1 && !solved; d++)
                                            {
                                                row4 = quadRows[d];
                                                column4 = quadColumns[d];
                                                tempCount = countDiffNumbers[2];

                                                for (int x = 0; x < numberCount[row4, column4]; x++)
                                                {
                                                    sameFlag = false;
                                                    tempNumber = numbers[row4, column4, x];
                                                    for (int y = 0; y < countDiffNumbers[2]; y++)
                                                    {
                                                        if (tempNumber == diffNumbers[y])
                                                        {
                                                            sameFlag = true;
                                                        }
                                                    }
                                                    if (sameFlag == false)
                                                    {
                                                        if (tempCount < 4)
                                                        {
                                                            diffNumbers[tempCount] = tempNumber;
                                                        }
                                                        tempCount++;
                                                    }
                                                }
                                                countDiffNumbers[3] = tempCount;

                                                if (countDiffNumbers[3] == 4)
                                                {
                                                    // The 4 numbers stored in diffNumbers form a naked quad in cells 1, 2, 3, & 4
                                                    totalCount = 0;

                                                    // Remove number1, number2, number3, and number4 from every cell 
                                                    // in group other than cells 1, 2, 3, and 4
                                                    for (int i = ROW_START[l]; i <= ROW_END[l]; i++)
                                                    {
                                                        for (int j = COLUMN_START[l]; j <= COLUMN_END[l]; j++)
                                                        {
                                                            if (intPuzzle[i, j] == 0 && !(i == row1 && j == column1) && !(i == row2 && j == column2) &&
                                                                    !(i == row3 && j == column3) && !(i == row4 && j == column4))
                                                            {
                                                                if (possible[i, j, diffNumbers[0]] == diffNumbers[0] + 1)
                                                                {
                                                                    possible[i, j, diffNumbers[0]] = 0;
                                                                    totalCount++;
                                                                }
                                                                if (possible[i, j, diffNumbers[1]] == diffNumbers[1] + 1)
                                                                {
                                                                    possible[i, j, diffNumbers[1]] = 0;
                                                                    totalCount++;
                                                                }
                                                                if (possible[i, j, diffNumbers[2]] == diffNumbers[2] + 1)
                                                                {
                                                                    possible[i, j, diffNumbers[2]] = 0;
                                                                    totalCount++;
                                                                }
                                                                if (possible[i, j, diffNumbers[3]] == diffNumbers[3] + 1)
                                                                {
                                                                    possible[i, j, diffNumbers[3]] = 0;
                                                                    totalCount++;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (totalCount > 0)
                                                    {
                                                        tempChanges++;

                                                        // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                        if (!isGuessAndCheck && !isBruteForce)
                                                        {
                                                            Console.WriteLine("Naked Quad Group found something!");
                                                            difficultyScore += 100;
                                                        }

                                                        // Run previous methods to see if the puzzle can be solved
                                                        tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                        if (!solved)
                                                        {
                                                            tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int XWingChecks(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes;
            int tempXWingChanges = 0;

            do
            {
                changes = 0;
                changes += XWingRowCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += XWingColumnCheck(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                tempXWingChanges += changes;
            } while (changes != 0 && !solved);

            return tempXWingChanges;
        }

        public static int XWingRowCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[] count = new int[9];
            int[,] columns = new int[9, 2];
            int[] doubleRows = new int[9];
            int doubleCount = 0;
            int row1 = 0;
            int row2 = 0;
            int column1 = 0;
            int column2 = 0;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int k = 0; k < 9 && !solved; k++) // number 1-9
                { 
                    for (int z = 0; z < 9; z++)
                    {
                        count[z] = 0;
                        doubleRows[z] = 0;
                    }
                    doubleCount = 0;

                    // Goes down each row, counts the number of columns the number k appears in,
                    // and then stores the first two columns
                    for (int i = 0; i < 9; i++) // row number
                    { 
                        if (!Rows[i, k])
                        {
                            for (int j = 0; j < 9; j++) // column number
                            { 
                                if (intPuzzle[i, j] == 0 && possible[i, j, k] == k + 1)
                                {
                                    if (count[i] < 2)
                                    {
                                        columns[i, count[i]] = j;
                                    }
                                    count[i]++;
                                }
                            }
                        }
                    }

                    // Counts the number of rows the number appears twice in and stores them
                    for (int i = 0; i < 9; i++) // row number
                    { 
                        if (count[i] == 2)
                        {
                            doubleRows[doubleCount] = i;
                            doubleCount++;
                        }
                    }

                    if (doubleCount > 1)
                    {
                        // Stage 1
                        for (int a = 0; a <= doubleCount - 2 && !solved; a++)
                        {
                            row1 = doubleRows[a];
                            column1 = columns[row1, 0];
                            column2 = columns[row1, 1];
                            // Stage 2
                            for (int b = a + 1; b <= doubleCount - 1 && !solved; b++)
                            {
                                row2 = doubleRows[b];

                                if (columns[row2, 0] == column1 && columns[row2, 1] == column2)
                                {
                                    // There is an X Wing in row1, row2 and column1, column2
                                    totalCount = 0;

                                    // Remove number1 and number2 from every cell 
                                    // in column1 and column2 other than row1 and row2
                                    for (int i = 0; i < 9; i++) // row number
                                    { 
                                        if (i != row1 && i != row2)
                                        {
                                            if (intPuzzle[i, column1] == 0 && possible[i, column1, k] == k + 1)
                                            {
                                                possible[i, column1, k] = 0;
                                                totalCount++;
                                            }
                                            if (intPuzzle[i, column2] == 0 && possible[i, column2, k] == k + 1)
                                            {
                                                possible[i, column2, k] = 0;
                                                totalCount++;
                                            }
                                        }
                                    }

                                    if (totalCount > 0)
                                    {
                                        tempChanges++;

                                        // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                        if (!isGuessAndCheck && !isBruteForce)
                                        {
                                            Console.WriteLine("X Wing Row found something!");
                                            difficultyScore += 100;
                                        }

                                        // Run previous methods to see if the puzzle can be solved
                                        tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                        if (!solved)
                                        {
                                            tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int XWingColumnCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int[] count = new int[9];
            int[,] rows = new int[9, 2];
            int[] doubleColumns = new int[9];
            int doubleCount = 0;
            int row1 = 0;
            int row2 = 0;
            int column1 = 0;
            int column2 = 0;
            int totalCount = 0;

            do
            {
                tempChanges = 0;
                for (int k = 0; k < 9 && !solved; k++) // number 1-9
                {
                    for (int z = 0; z < 9; z++)
                    {
                        count[z] = 0;
                        doubleColumns[z] = 0;
                    }
                    doubleCount = 0;

                    // Goes down each column, counts the number of rows the number k appears in,
                    // and then stores the first two rows
                    for (int j = 0; j < 9; j++) // column number
                    {
                        if (!Columns[j, k])
                        {
                            for (int i = 0; i < 9; i++)  // row number
                            {
                                if (intPuzzle[i, j] == 0 && possible[i, j, k] == k + 1)
                                {
                                    if (count[j] < 2)
                                    {
                                        rows[j, count[j]] = i;
                                    }
                                    count[j]++;
                                }
                            }
                        }
                    }

                    // Counts the number of columns the number appears twice in and stores them
                    for (int j = 0; j < 9; j++) // column number
                    {
                        if (count[j] == 2)
                        {
                            doubleColumns[doubleCount] = j;
                            doubleCount++;
                        }
                    }

                    if (doubleCount > 1)
                    {
                        // Stage 1
                        for (int a = 0; a <= doubleCount - 2 && !solved; a++)
                        {
                            column1 = doubleColumns[a];
                            row1 = rows[column1, 0];
                            row2 = rows[column1, 1];
                            // Stage 2
                            for (int b = a + 1; b <= doubleCount - 1 && !solved; b++)
                            {
                                column2 = doubleColumns[b];

                                if (rows[column2, 0] == row1 && rows[column2, 1] == row2)
                                {
                                    // There is an X Wing in row1, row2 and column1, column2
                                    totalCount = 0;

                                    // Remove number1 and number2 from every cell 
                                    // in row1 and row2 other than column1 and column2
                                    for (int j = 0; j < 9; j++) // column number
                                    {
                                        if (j != column1 && j != column2)
                                        {
                                            if (intPuzzle[row1, j] == 0 && possible[row1, j, k] == k + 1)
                                            {
                                                possible[row1, j, k] = 0;
                                                totalCount++;
                                            }
                                            if (intPuzzle[row2, j] == 0 && possible[row2, j, k] == k + 1)
                                            {
                                                possible[row2, j, k] = 0;
                                                totalCount++;
                                            }
                                        }
                                    }

                                    if (totalCount > 0)
                                    {
                                        tempChanges++;

                                        // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                        if (!isGuessAndCheck && !isBruteForce)
                                        {
                                            Console.WriteLine("X Wing Column found something!");
                                            difficultyScore += 100;
                                        }

                                        // Run previous methods to see if the puzzle can be solved
                                        tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                        if (!solved)
                                        {
                                            tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int YWingChecks(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempYWingChanges = 0;
            bool[,] isTwoOption = new bool[9, 9];
            int[,,] numbers = new int[9, 9, 2];

            do
            {
                changes = 0;
                changes += RowColumnYWingCheck(intPuzzle, possible, numbers, isTwoOption, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += RowGroupYWingCheck(intPuzzle, possible, numbers, isTwoOption, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                changes += ColumnGroupYWingCheck(intPuzzle, possible, numbers, isTwoOption, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
            } while (changes != 0 && !solved);

            return tempYWingChanges;
        }

        public static int RowColumnYWingCheck(int[,] intPuzzle, int[,,] possible, int[,,] numbers, bool[,] isTwoOption, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges = 0;
            int A;
            int B;
            int C;
            int rowWingColumn;
            bool isRowWing_AC;
            bool isRowWing_BC;
            int centerWingGroup;
            int columnWingRow;
            bool yWingChangesMade;

            do
            {
                tempChanges = 0;
                UpdateTwoOption(intPuzzle, possible, numbers, isTwoOption);
                yWingChangesMade = false;

                for (int i = 0; i < 9 && !yWingChangesMade && !solved; i++) // row number
                {
                    for (int j = 0; j < 9 && !yWingChangesMade && !solved; j++) // column number
                    {
                        A = 0;
                        B = 0;
                        C = 0;

                        if (isTwoOption[i, j] == true)
                        {
                            A = numbers[i, j, 0];
                            B = numbers[i, j, 1];
                            centerWingGroup = DetermineGroup(i, j);

                            // go down the row to see if there is another two option cell
                            // that can be a wing cell
                            for (int q = 0; q < 9 && !yWingChangesMade && !solved; q++) // column number
                            {
                                rowWingColumn = -1;
                                int tempGroup = DetermineGroup(i, q);
                                isRowWing_AC = false;
                                isRowWing_BC = false;

                                if (isTwoOption[i, q] == true && q != j && tempGroup != centerWingGroup)
                                {
                                    if (numbers[i, q, 0] == A && numbers[i, q, 1] != B)
                                    {
                                        rowWingColumn = q;
                                        C = numbers[i, q, 1];
                                        isRowWing_AC = true;
                                    }
                                    else if (numbers[i, q, 0] == B && numbers[i, q, 1] != A)
                                    {
                                        rowWingColumn = q;
                                        C = numbers[i, q, 1];
                                        isRowWing_BC = true;
                                    }
                                    else if (numbers[i, q, 0] != A && numbers[i, q, 1] == B)
                                    {
                                        rowWingColumn = q;
                                        C = numbers[i, q, 0];
                                        isRowWing_BC = true;
                                    }
                                    else if (numbers[i, q, 0] != B && numbers[i, q, 1] == A)
                                    {
                                        rowWingColumn = q;
                                        C = numbers[i, q, 0];
                                        isRowWing_AC = true;
                                    }

                                    // if a row wing has been found, check for a column wing
                                    if (rowWingColumn != -1)
                                    {
                                        columnWingRow = -1;

                                        for (int r = 0; r < 9 && !yWingChangesMade && !solved; r++)  // row number
                                        {
                                            tempGroup = DetermineGroup(r, j);

                                            if (isTwoOption[r, j] == true && r != i && tempGroup != centerWingGroup)
                                            {
                                                // check to see if the column wing is equal to the opposite config of the row wing
                                                // case 1) Row Wing = AC, is Column Wing = BC?
                                                // case 2) Row Wing = BC, is Column Wing = AC?

                                                if ((isRowWing_AC == true && ((numbers[r, j, 0] == B && numbers[r, j, 1] == C) ||
                                                        (numbers[r, j, 1] == B && numbers[r, j, 0] == C))) ||
                                                        (isRowWing_BC == true && ((numbers[r, j, 0] == A && numbers[r, j, 1] == C) ||
                                                        (numbers[r, j, 1] == A && numbers[r, j, 0] == C))))
                                                {
                                                    // Y Wing exists!!
                                                    columnWingRow = r;

                                                    // Remove C from the cell at (columnWingRow, rowWingColumn), if it's an option
                                                    if (intPuzzle[columnWingRow, rowWingColumn] == 0 && possible[columnWingRow, rowWingColumn, C - 1] == C)
                                                    {
                                                        // Remove C and record the change
                                                        possible[columnWingRow, rowWingColumn, C - 1] = 0;
                                                        yWingChangesMade = true;
                                                    }
                                                }

                                                if (yWingChangesMade)
                                                {
                                                    tempChanges++;

                                                    // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                    if (!isGuessAndCheck && !isBruteForce)
                                                    {
                                                        Console.WriteLine("Y Wing Row-Column found something!");
                                                        difficultyScore += 100;
                                                    }

                                                    // Run previous methods to see if the puzzle can be solved
                                                    tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                    if (!solved)
                                                    {
                                                        tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int RowGroupYWingCheck(int[,] intPuzzle, int[,,] possible, int[,,] numbers, bool[,] isTwoOption, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges = 0;
            int A;
            int B;
            int C;
            int rowWingColumn;
            int rowWingGroup;
            bool isRowWing_AC;
            bool isRowWing_BC;
            int centerWingGroup;
            int groupWingColumn;
            int groupWingRow;
            bool yWingChangesMade;

            do
            {
                tempChanges = 0;
                UpdateTwoOption(intPuzzle, possible, numbers, isTwoOption);
                yWingChangesMade = false;

                for (int i = 0; i < 9 && !yWingChangesMade && !solved; i++) // row number
                {
                    for (int j = 0; j < 9 && !yWingChangesMade && !solved; j++) // column number
                    {
                        A = 0;
                        B = 0;
                        C = 0;

                        if (isTwoOption[i, j] == true)
                        {
                            A = numbers[i, j, 0];
                            B = numbers[i, j, 1];
                            centerWingGroup = DetermineGroup(i, j);

                            // go down the row to see if there is another two option cell
                            // that can be a wing cell
                            for (int q = 0; q < 9 && !yWingChangesMade && !solved; q++) // column number
                            {
                                rowWingColumn = -1;
                                rowWingGroup = -1;
                                int tempGroup = DetermineGroup(i, q);
                                isRowWing_AC = false;
                                isRowWing_BC = false;

                                if (isTwoOption[i, q] == true && q != j && tempGroup != centerWingGroup)
                                {
                                    if (numbers[i, q, 0] == A && numbers[i, q, 1] != B)
                                    {
                                        rowWingColumn = q;
                                        rowWingGroup = DetermineGroup(i, q);
                                        C = numbers[i, q, 1];
                                        isRowWing_AC = true;
                                    }
                                    else if (numbers[i, q, 0] == B && numbers[i, q, 1] != A)
                                    {
                                        rowWingColumn = q;
                                        rowWingGroup = DetermineGroup(i, q);
                                        C = numbers[i, q, 1];
                                        isRowWing_BC = true;
                                    }
                                    else if (numbers[i, q, 0] != A && numbers[i, q, 1] == B)
                                    {
                                        rowWingColumn = q;
                                        rowWingGroup = DetermineGroup(i, q);
                                        C = numbers[i, q, 0];
                                        isRowWing_BC = true;
                                    }
                                    else if (numbers[i, q, 0] != B && numbers[i, q, 1] == A)
                                    {
                                        rowWingColumn = q;
                                        rowWingGroup = DetermineGroup(i, q);
                                        C = numbers[i, q, 0];
                                        isRowWing_AC = true;
                                    }

                                    // if a row wing has been found, check for a group wing
                                    if (rowWingColumn != -1)
                                    {
                                        groupWingColumn = -1;
                                        groupWingRow = -1;

                                        for (int r = ROW_START[centerWingGroup]; r <= ROW_END[centerWingGroup] && !yWingChangesMade && !solved; r++) // row number
                                        {
                                            for (int s = COLUMN_START[centerWingGroup]; s <= COLUMN_END[centerWingGroup] && !yWingChangesMade && !solved; s++) // column number
                                            {
                                                if (isTwoOption[r, s] == true && r != i)
                                                {
                                                    // check to see if the group wing is equal to the opposite config of the row wing
                                                    // case 1) Row Wing = AC, is Group Wing = BC?
                                                    // case 2) Row Wing = BC, is Group Wing = AC?

                                                    if ((isRowWing_AC == true && ((numbers[r, s, 0] == B && numbers[r, s, 1] == C) ||
                                                            (numbers[r, s, 1] == B && numbers[r, s, 0] == C))) ||
                                                            (isRowWing_BC == true && ((numbers[r, s, 0] == A && numbers[r, s, 1] == C) ||
                                                            (numbers[r, s, 1] == A && numbers[r, s, 0] == C))))
                                                    {
                                                        // Y Wing exists!!
                                                        groupWingRow = r;
                                                        groupWingColumn = s;

                                                        // Remove C from all cells that are:
                                                        // 1) in both the Group Wing's row & the Row Wing's group OR
                                                        // 2) in the Center group & current row
                                                        for (int t = COLUMN_START[rowWingGroup]; t <= COLUMN_END[rowWingGroup]; t++)
                                                        {
                                                            if (intPuzzle[groupWingRow, t] == 0 && possible[groupWingRow, t, C - 1] == C && t != groupWingColumn)
                                                            {
                                                                // Remove C and record the change
                                                                possible[groupWingRow, t, C - 1] = 0;
                                                                yWingChangesMade = true;
                                                            }
                                                        }

                                                        for (int t = COLUMN_START[centerWingGroup]; t <= COLUMN_END[centerWingGroup]; t++)
                                                        {
                                                            if (intPuzzle[i, t] == 0 && possible[i, t, C - 1] == C && t != j)
                                                            {
                                                                // Remove C and record the change
                                                                possible[i, t, C - 1] = 0;
                                                                yWingChangesMade = true;
                                                            }
                                                        }

                                                        if (yWingChangesMade)
                                                        {
                                                            tempChanges++;

                                                            // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                            if (!isGuessAndCheck && !isBruteForce)
                                                            {
                                                                Console.WriteLine("Y Wing Row-Group found something!");
                                                                difficultyScore += 100;
                                                            }

                                                            // Run previous methods to see if the puzzle can be solved
                                                            tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                            if (!solved)
                                                            {
                                                                tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static int ColumnGroupYWingCheck(int[,] intPuzzle, int[,,] possible, int[,,] numbers, bool[,] isTwoOption, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isGuessAndCheck, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges = 0;
            int A;
            int B;
            int C;
            int columnWingRow;
            int columnWingGroup;
            bool isColumnWing_AC;
            bool isColumnWing_BC;
            int centerWingGroup;
            int groupWingColumn;
            int groupWingRow;
            bool yWingChangesMade;

            do
            {
                tempChanges = 0;
                UpdateTwoOption(intPuzzle, possible, numbers, isTwoOption);
                yWingChangesMade = false;

                for (int i = 0; i < 9 && !yWingChangesMade && !solved; i++) // row number
                {
                    for (int j = 0; j < 9 && !yWingChangesMade && !solved; j++) // column number
                    {
                        A = 0;
                        B = 0;
                        C = 0;

                        if (isTwoOption[i, j] == true)
                        {
                            A = numbers[i, j, 0];
                            B = numbers[i, j, 1];
                            centerWingGroup = DetermineGroup(i, j);

                            // go down the column to see if there is another two option cell
                            // that can be a wing cell
                            for (int q = 0; q < 9 && !yWingChangesMade && !solved; q++) // row number
                            {
                                columnWingRow = -1;
                                columnWingGroup = -1;
                                int tempGroup = DetermineGroup(q, j);
                                isColumnWing_AC = false;
                                isColumnWing_BC = false;

                                if (isTwoOption[q, j] == true && q != i && tempGroup != centerWingGroup)
                                {
                                    if (numbers[q, j, 0] == A && numbers[q, j, 1] != B)
                                    {
                                        columnWingRow = q;
                                        columnWingGroup = DetermineGroup(q, j);
                                        C = numbers[q, j, 1];
                                        isColumnWing_AC = true;
                                    }
                                    else if (numbers[q, j, 0] == B && numbers[q, j, 1] != A)
                                    {
                                        columnWingRow = q;
                                        columnWingGroup = DetermineGroup(q, j);
                                        C = numbers[q, j, 1];
                                        isColumnWing_BC = true;
                                    }
                                    else if (numbers[q, j, 0] != A && numbers[q, j, 1] == B)
                                    {
                                        columnWingRow = q;
                                        columnWingGroup = DetermineGroup(q, j);
                                        C = numbers[q, j, 0];
                                        isColumnWing_BC = true;
                                    }
                                    else if (numbers[q, j, 0] != B && numbers[q, j, 1] == A)
                                    {
                                        columnWingRow = q;
                                        columnWingGroup = DetermineGroup(q, j);
                                        C = numbers[q, j, 0];
                                        isColumnWing_AC = true;
                                    }

                                    // if a column wing has been found, check for a group wing
                                    if (columnWingRow != -1)
                                    {
                                        groupWingColumn = -1;
                                        groupWingRow = -1;

                                        for (int r = ROW_START[centerWingGroup]; r <= ROW_END[centerWingGroup] && !yWingChangesMade && !solved; r++) // row number
                                        {
                                            for (int s = COLUMN_START[centerWingGroup]; s <= COLUMN_END[centerWingGroup] && !yWingChangesMade && !solved; s++) // column number
                                            {
                                                if (isTwoOption[r, s] == true && s != j)
                                                {
                                                    // check to see if the group wing is equal to the opposite config of the column wing
                                                    // case 1) Column Wing = AC, is Group Wing = BC?
                                                    // case 2) Column Wing = BC, is Group Wing = AC?

                                                    if ((isColumnWing_AC == true && ((numbers[r, s, 0] == B && numbers[r, s, 1] == C) ||
                                                            (numbers[r, s, 1] == B && numbers[r, s, 0] == C))) ||
                                                            (isColumnWing_BC == true && ((numbers[r, s, 0] == A && numbers[r, s, 1] == C) ||
                                                            (numbers[r, s, 1] == A && numbers[r, s, 0] == C))))
                                                    {
                                                        // Y Wing exists!!
                                                        groupWingRow = r;
                                                        groupWingColumn = s;

                                                        // Remove C from all cells that are:
                                                        // 1) in both the Group Wing's column & the Column Wing's group OR
                                                        // 2) in the Center's group & current column
                                                        for (int t = ROW_START[columnWingGroup]; t <= ROW_END[columnWingGroup]; t++)
                                                        {
                                                            if (intPuzzle[t, groupWingColumn] == 0 && possible[t, groupWingColumn, C - 1] == C && t != groupWingRow)
                                                            {
                                                                // Remove C and record the change
                                                                possible[t, groupWingColumn, C - 1] = 0;
                                                                yWingChangesMade = true;
                                                            }
                                                        }

                                                        for (int t = ROW_START[centerWingGroup]; t <= ROW_END[centerWingGroup]; t++)
                                                        {
                                                            if (intPuzzle[t, j] == 0 && possible[t, j, C - 1] == C && t != i)
                                                            {
                                                                // Remove C and record the change
                                                                possible[t, j, C - 1] = 0;
                                                                yWingChangesMade = true;
                                                            }
                                                        }

                                                        if (yWingChangesMade)
                                                        {
                                                            tempChanges++;

                                                            // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                            if (!isGuessAndCheck && !isBruteForce)
                                                            {
                                                                Console.WriteLine("Y Wing Column-Group found something!");
                                                                difficultyScore += 100;
                                                            }

                                                            // Run previous methods to see if the puzzle can be solved
                                                            tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                            if (!solved)
                                                            {
                                                                tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, isGuessAndCheck, isBruteForce);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static void UpdateTwoOption(int[,] intPuzzle, int[,,] possible, int[,,] numbers, bool[,] isTwoOption)
        {
            int cellCount;

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    isTwoOption[x, y] = false;
                    for (int z = 0; z < 2; z++)
                    {
                        numbers[x, y, z] = 0;
                    }
                }
            }

            for (int i = 0; i < 9; i++)  // row number
            {
                for (int j = 0; j < 9; j++)  // column number
                {
                    cellCount = 0;
                    if (intPuzzle[i, j] == 0)
                    {
                        for (int k = 0; k < 9; k++) // number 1-9
                        {
                            if (possible[i, j, k] == k + 1)
                            {
                                if (cellCount == 0)
                                {
                                    numbers[i, j, 0] = k + 1;
                                }
                                else if (cellCount == 1)
                                {
                                    numbers[i, j, 1] = k + 1;
                                }
                                cellCount++;
                            }
                        }
                    }

                    if (cellCount == 2)
                    {
                        isTwoOption[i, j] = true;
                    }
                }
            }
        }

        public static int LevelThreeMethods(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups)
        {
            int changes;
            int tempLevelThreeChanges = 0;

            do
            {
                changes = 0;
                changes += GuessAndCheck(intPuzzle, possible, Rows, Columns, Groups, false);
                tempLevelThreeChanges += changes;
            } while (changes != 0 && !solved);

            // implement bruteForce only once if the puzzle isn't solved yet
            if (!solved)
            {
                // clone intPuzzle, possible, Rows, Columns, and Groups here and feed them into bruteForce below
                int[,] clonePuzzle = new int[9, 9];
                int[,,] clonePossible = new int[9, 9, 9];
                bool[,] cloneRows = new bool[9, 9];
                bool[,] cloneColumns = new bool[9, 9];
                bool[,] cloneGroups = new bool[9, 9];
                SetPuzzleEqual(intPuzzle, clonePuzzle);
                SetPossibleEqual(possible, clonePossible);
                SetBoolEqual(Rows, cloneRows);
                SetBoolEqual(Columns, cloneColumns);
                SetBoolEqual(Groups, cloneGroups);

                BruteForce(clonePuzzle, clonePossible, cloneRows, cloneColumns, cloneGroups);

                // update possible so the puzzle will solve by calling previous methods again
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (intPuzzle[i, j] != clonePuzzle[i, j])
                        {
                            for (int k = 0; k < 9; k++)
                            {
                                if (possible[i, j, k] != 0 && possible[i, j, k] != clonePuzzle[i, j])
                                {
                                    possible[i, j, k] = 0;
                                }
                            }
                        }
                    }
                }

                // call previous methods again to solve the puzzle
                LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, false, false);
                if (!solved)
                {
                    LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, false, false);
                    if (!solved)
                    {
                        LevelTwoMethods(intPuzzle, possible, Rows, Columns, Groups, false, false);
                        if (!solved)
                        {
                            GuessAndCheck(intPuzzle, possible, Rows, Columns, Groups, false);
                        }
                    }
                }
            }
            
            return tempLevelThreeChanges;
        }

        public static int GuessAndCheck(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups, bool isBruteForce)
        {
            int changes = 0;
            int tempChanges;
            int tempCount;
            int[] tempOptions = new int[9];
            int[,,] clonePuzzles = new int[9, 9, 9]; // first index: clone puzzle number, second index: row, third index: column
            int[,,] clonePossible = new int[9, 9, 9];
            bool[,] cloneRows = new bool[9, 9];
            bool[,] cloneColumns = new bool[9, 9];
            bool[,] cloneGroups = new bool[9, 9];
            bool contradictionFound;
            bool sameNumFound;
            bool tempSolved;

            do
            {
                tempChanges = 0;
                contradictionFound = false;
                sameNumFound = false;
                tempSolved = false;

                for (int optionCount = 2; optionCount < 10 && !contradictionFound && !sameNumFound && !tempSolved; optionCount++)
                {
                    for (int i = 0; i < 9 && !contradictionFound && !sameNumFound && !tempSolved; i++)
                    {
                        for (int j = 0; j < 9 && !contradictionFound && !sameNumFound && !tempSolved; j++)
                        {
                            if (intPuzzle[i, j] == 0)
                            {
                                // count how many options are in this cell and store them
                                tempCount = 0;
                                for (int k = 0; k < 9; k++)
                                {
                                    if (possible[i, j, k] == k + 1)
                                    {
                                        tempOptions[tempCount] = k + 1;
                                        tempCount++;
                                    }
                                }

                                if (tempCount == optionCount)
                                {
                                    // set cloneGames to zero
                                    ResetCloneGames(clonePuzzles);

                                    for (int currentOption = 0; currentOption < optionCount && !contradictionFound && !tempSolved; currentOption++)
                                    {
                                        // clone the current game
                                        SetClonePuzzleEqualToIntPuzzle(intPuzzle, clonePuzzles, currentOption);
                                        SetPossibleEqual(possible, clonePossible);
                                        SetBoolEqual(Rows, cloneRows);
                                        SetBoolEqual(Columns, cloneColumns);
                                        SetBoolEqual(Groups, cloneGroups);

                                        // set this cell in clone game to the current option
                                        clonePuzzles[currentOption, i, j] = tempOptions[currentOption];
                                        UpdateShadowPossible(tempOptions[currentOption], i, j, clonePossible, cloneRows, cloneColumns, cloneGroups);

                                        int[,] tempClonePuzzle = GetClonePuzzleAtIndex(clonePuzzles, currentOption);

                                        // solve the puzzle as much as possible using all previous methods
                                        LevelTwoMethods(tempClonePuzzle, clonePossible, cloneRows, cloneColumns, cloneGroups, true, isBruteForce);

                                        SetClonePuzzleEqualToIntPuzzle(tempClonePuzzle, clonePuzzles, currentOption);

                                        // check to see if this guess solved the puzzle
                                        if (IsSolved(GetClonePuzzleAtIndex(clonePuzzles, currentOption)))
                                        {
                                            tempSolved = true;
                                            for (int k = 0; k < 9; k++)
                                            {
                                                if (possible[i, j, k] != tempOptions[currentOption] && possible[i, j, k] != 0)
                                                {
                                                    possible[i, j, k] = 0;
                                                    tempChanges++;

                                                    // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                    if (!isBruteForce)
                                                    {
                                                        Console.WriteLine("Guess and Check found something! - it solved the puzzle");
                                                        difficultyScore += 10000;
                                                    }
                                                }
                                            }
                                        }

                                        // check to see if there are any cells with zero options
                                        if (!tempSolved)
                                        {
                                            for (int x = 0; x < 9 && !contradictionFound; x++)
                                            {
                                                for (int y = 0; y < 9 && !contradictionFound; y++)
                                                {
                                                    if (clonePuzzles[currentOption, x, y] == 0)
                                                    {
                                                        if (CountPossibleOptions(x, y, clonePossible) == 0)
                                                        {
                                                            // a contradiction has occured that made a cell unfillable
                                                            // therefore, the current option is not possible and needs to be removed
                                                            contradictionFound = true;
                                                        }
                                                    }
                                                }
                                            }

                                            // if a contradiction was found, remove the current option as a possible option
                                            if (contradictionFound)
                                            {
                                                int tempOptionRemoved = possible[i, j, tempOptions[currentOption] - 1];
                                                possible[i, j, tempOptions[currentOption] - 1] = 0;
                                                tempChanges++;

                                                // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                if (!isBruteForce)
                                                {
                                                    Console.WriteLine("Guess and Check found something! - it found a contradiction");
                                                    difficultyScore += 10000;
                                                }
                                            }
                                        }
                                    }

                                    // check to see if any cells resulted in the same number with all guesses
                                    if (!contradictionFound && !tempSolved)
                                    {
                                        for (int x = 0; x < 9; x++)
                                        {
                                            for (int y = 0; y < 9; y++)
                                            {
                                                if (intPuzzle[x, y] == 0)
                                                {
                                                    int num = clonePuzzles[0, x, y];
                                                    bool diffFlag = false;
                                                    for (int currentOption = 1; currentOption < optionCount; currentOption++)
                                                    {
                                                        if (clonePuzzles[currentOption, x, y] != num)
                                                        {
                                                            diffFlag = true;
                                                        }
                                                    }
                                                    if (!diffFlag && num != 0)
                                                    {
                                                        // the value at row x column y must be num
                                                        // in order to let the level zero methods place the number in the grid, remove all possible options except num
                                                        sameNumFound = true;
                                                        for (int k = 0; k < 9; k++)
                                                        {
                                                            if (possible[x, y, k] != num && possible[x, y, k] != 0)
                                                            {
                                                                int tempOptionRemoved = possible[x, y, k];
                                                                possible[x, y, k] = 0;
                                                                tempChanges++;

                                                                // only update the score and print to the console when a change is made to the actual puzzle, not a clone
                                                                if (!isBruteForce)
                                                                {
                                                                    Console.WriteLine("Guess and Check found something! - a cell was the same in multiple guesses");
                                                                    difficultyScore += 10000;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                // Run previous methods to see if the puzzle can be solved
                tempChanges += LevelZeroMethods(intPuzzle, possible, Rows, Columns, Groups, false, isBruteForce);
                if (!IsSolved(intPuzzle))
                {
                    tempChanges += LevelOneMethods(intPuzzle, possible, Rows, Columns, Groups, false, isBruteForce);
                    if (!IsSolved(intPuzzle))
                    {
                        tempChanges += LevelTwoMethods(intPuzzle, possible, Rows, Columns, Groups, false, isBruteForce);
                    }
                }
                changes += tempChanges;
            } while (tempChanges != 0 && !solved);

            return changes;
        }

        public static void BruteForce(int[,] intPuzzle, int[,,] possible, bool[,] Rows, bool[,] Columns, bool[,] Groups)
        {
            int[,] clonePuzzle = new int[9, 9];
            int[,,] clonePossible = new int[9, 9, 9];

            for (int optionCount = 2; optionCount < 10; optionCount++)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (intPuzzle[i, j] == 0)
                        {
                            // count how many options are in this cell and store them
                            int tempCount = 0;
                            int[] tempOptions = new int[9];
                            for (int k = 0; k < 9; k++)
                            {
                                if (possible[i, j, k] == k + 1)
                                {
                                    tempOptions[tempCount] = k + 1;
                                    tempCount++;
                                }
                            }
                            if (tempCount == optionCount)
                            {
                                for (int currentOption = 0; currentOption < optionCount; currentOption++)
                                {
                                    SetPuzzleEqual(intPuzzle, clonePuzzle);
                                    SetPossibleEqual(possible, clonePossible);
                                    intPuzzle[i, j] = tempOptions[currentOption];
                                    UpdateShadowPossible(tempOptions[currentOption], i, j, possible, Rows, Columns, Groups);

                                    GuessAndCheck(intPuzzle, possible, Rows, Columns, Groups, true);

                                    // check if the puzzle is solved
                                    if (IsSolved(intPuzzle))
                                    {
                                        bruteForceSolved = true;
                                        // reset the puzzle to only have the digits needed to solve the puzzle with previous methods, and return
                                        SetPuzzleEqual(clonePuzzle, intPuzzle);
                                        intPuzzle[i, j] = tempOptions[currentOption];
                                        return;
                                    }

                                    // if the puzzle wasn't solved, check if it's still valid
                                    if (IsValid(intPuzzle, possible))
                                    {
                                        // puzzle is still valid, continue with the recursion
                                        BruteForce(intPuzzle, possible, Rows, Columns, Groups);
                                        // return if the subsequent recursions have solved the puzzle
                                        if (bruteForceSolved)
                                        {
                                            Console.WriteLine("Brute Force solved the puzzle!");
                                            difficultyScore += 10000;
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        // puzzle is invalid, reset intPuzzle and possible, try the next option
                                        SetPuzzleEqual(clonePuzzle, intPuzzle);
                                        SetPossibleEqual(clonePossible, possible);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void ResetCloneGames(int[,,] cloneGames)
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    for (int z = 0; z < 9; z++)
                    {
                        cloneGames[x, y, z] = 0;
                    }
                }
            }
        }

        public static int[,] GetClonePuzzleAtIndex(int[,,] clonePuzzles, int index)
        {
            int[,] clonePuzzle = new int[9, 9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    clonePuzzle[i, j] = clonePuzzles[index, i, j];
                }
            }
            
            return clonePuzzle;
        }

        public static void SetClonePuzzleEqualToIntPuzzle(int[,] intPuzzle, int[,,] clonePuzzles, int index)
        {
            for (int i = 0; i < 9; i++) // row number
            {
                for (int j = 0; j < 9; j++) // column number
                {
                    clonePuzzles[index, i, j] = intPuzzle[i, j];
                }
            }
        }

        public static void SetPuzzleEqual(int[,] intPuzzle, int[,] clonePuzzle)
        {
            for (int i = 0; i < 9; i++) // row number
            {
                for (int j = 0; j < 9; j++) // column number
                { 
                    clonePuzzle[i, j] = intPuzzle[i, j];
                }
            }
        }

        public static void SetBoolEqual(bool[,] boolean, bool[,] newBoolean)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    newBoolean[i, j] = boolean[i, j];
                }
            }
        }

        public static void SetPossibleEqual(int[,,] possible, int[,,] newPossible)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        newPossible[i, j, k] = possible[i, j, k];
                    }
                }
            }
        }

        public static bool IsSolved(int[,] intPuzzle)
        {
            int count = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (intPuzzle[i, j] != 0)
                    {
                        count++;
                    }
                }
            }
            return count == 81 && Check(intPuzzle);
        }

        public static bool IsValid(int[,] intPuzzle, int[,,] possible)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (intPuzzle[i, j] == 0)
                    {
                        if (CountPossibleOptions(i, j, possible) == 0)
                        {
                            // a contradiction has occured that made a cell unfillable
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static int CountPossibleOptions(int row, int column, int[,,] possible)
        {
            int options = 0;

            for (int k = 0; k < 9; k++)
            {
                if (possible[row, column, k] == k + 1)
                {
                    options++;
                }
            }

            return options;
        }


        // Helper functions that print variables to the console
        public static void PrintPuzzle(int[,] puzzle)
        {
            for (int i = 0; i < 9; i++) // row number
            {
                for (int j = 0; j < 9; j++)  // column number
                {
                    Console.Write(puzzle[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public static void PrintRows(bool[,] Rows)
        {
            Console.WriteLine("                           Number                        ");
            Console.WriteLine("Row     1     2     3     4     5     6     7     8     9");
            for (int i = 0; i < 9; i++) // row number
            {
                Console.Write("  " + i + "   ");
                for (int k = 0; k < 9; k++) // number 1-9
                {
                    if (Rows[i,k] == false)
                    {
                        Console.Write(Rows[i,k] + " ");
                    }
                    else
                    {
                        Console.Write(Rows[i,k] + "  ");
                    }
                }
                Console.WriteLine();
            }
        }

        public static void PrintColumns(bool[,] Columns)
        {
            Console.WriteLine("                           Number                        ");
            Console.WriteLine("Column     1     2     3     4     5     6     7     8     9");
            for (int j = 0; j < 9; j++) // column number
            { 
                Console.Write("     " + j + "   ");
                for (int k = 0; k < 9; k++) // number 1-9
                { 
                    if (Columns[j,k] == false)
                    {
                        Console.Write(Columns[j,k] + " ");
                    }
                    else
                    {
                        Console.Write(Columns[j,k] + "  ");
                    }
                }
                Console.WriteLine();
            }
        }

        public static void PrintGroups(bool[,] Groups)
        {
            Console.WriteLine("Group     1     2     3     4     5     6     7     8     9");
            for (int group = 0; group < 9; group++) // group number
            {
                Console.Write("    " + group + "   ");
                for (int k = 0; k < 9; k++) // number 1-9
                {
                    if (Groups[group, k] == false)
                    {
                        Console.Write(Groups[group, k] + " ");
                    }
                    else
                    {
                        Console.Write(Groups[group, k] + "  ");
                    }
                }
                Console.WriteLine();
            }
        }

        public static void PrintPossible(int[,] intPuzzle, int[,,] possible)
        {
            Console.WriteLine("Possible: ");
            for (int i = 0; i < 9; i++) // row number
            {
                Console.WriteLine("Row " + i + ":");
                for (int j = 0; j < 9; j++) // column number
                {
                    if (intPuzzle[i,j] == 0)
                    {
                        for (int k = 0; k < 9; k++) // number 1-9
                        {
                            if (possible[i,j,k] == k + 1)
                            {
                                Console.Write(possible[i,j,k] + " ");
                            }
                        }
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
            }
        }
    }
    
}

