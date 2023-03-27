// See https://aka.ms/new-console-template for more information
using SudokuSolver.Strategies;
using SudokuSolver.Workers;

//Console.WriteLine("Hello, World!");
//SudokuFileReader sudokuFileReader = new SudokuFileReader();

//var sudoku = sudokuFileReader.ReadFile("SudokuEasy.txt");

//SudokuBoardDisplayer sudokuBoardDisplayer = new SudokuBoardDisplayer();
//sudokuBoardDisplayer.Display("Sudoku Easy", sudoku);

try
{
    SudokuMapper sudokuMapper = new SudokuMapper();
    SudokuBoardStateManager sudokuBoardStateManager = new SudokuBoardStateManager();
    SudokuSolverEngine sudokuSolverEngine = new SudokuSolverEngine(sudokuBoardStateManager, sudokuMapper);
    SudokuFileReader sudokuFileReader = new SudokuFileReader(); 
    SudokuBoardDisplayer sudokuBoardDisplayer = new SudokuBoardDisplayer();

    Console.WriteLine("Please enter the filename containing the Sudoku Puzzle");
    var filename = Console.ReadLine();

    var sudokuBoard = sudokuFileReader.ReadFile(filename);
    sudokuBoardDisplayer.Display("Initial State", sudokuBoard);

    bool isSudokuSolved = sudokuSolverEngine.Solve(sudokuBoard);
    sudokuBoardDisplayer.Display("Final State", sudokuBoard);
    Console.WriteLine(isSudokuSolved 
        ? "You have successfull solved this Sudoku Puzzle"
        : "Unfortunately current algorithm(s) were not enough to solve the current Sudoku Puzzle!");
}
catch (Exception ex)
{
    Console.WriteLine("Sdoku Puzzle cannot be solved because there was an error : ", ex.Message);
}