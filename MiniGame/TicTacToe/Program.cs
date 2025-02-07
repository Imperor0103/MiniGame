using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
namespace TicTacToe
{
    internal class Program
    {
        static int[] num = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        static bool isFinished = false;

        static string playerMark = "X ";
        static string computerMark = "O ";
        static bool isPlayerWon = false;
        static bool isComWon = false;

        static int turnCount = 0;   // 현재 턴

        static string[,] board = new string[11, 11];
        //{      0    1    2    3   4    5    6    7   8    9    10 
        // 0   {"  ","  ","  ","|","  ","  ","  ","|","  ","  ","  " },
        // 1   {"  ","(1,1)==1","  ","|","  ","(1,5)==2","  ","|","  ","(1,9)==3","  " },
        // 2   {"  ","  ","  ","|","  ","  ","  ","|","  ","  ","  " },
        // 3   {"ㅡ","ㅡ","ㅡ","+","ㅡ","ㅡ","ㅡ","+","ㅡ","ㅡ","ㅡ" },
        // 4   {"  ","  ","  ","|","  ","  ","  ","|","  ","  ","  " },
        // 5   {"  ","(5,1)==4","  ","|","  ","(5,5)==5","  ","|","  ","(5,9)==6","  " },
        // 6   {"  ","  ","  ","|","  ","  ","  ","|","  ","  ","  " },
        // 7   {"ㅡ","ㅡ","ㅡ","+","ㅡ","ㅡ","ㅡ","+","ㅡ","ㅡ","ㅡ" },
        // 8   {"  ","  ","  ","|","  ","  ","  ","|","  ","  ","  " },
        // 9   {"  ","(9,1)==7","  ","|","  ","(9,5)==8","  ","|","  ","(9,9)==9","  " },
        // 10   {"  ","  ","  ","|","  ","  ","  ","|","  ","  ","  " },
        //};

        // board 초기화
        static void InitBoard()
        {
            // 위의 내용을 for문을 이용하여 초기화
            for (int i = 0; i < board.GetLength(0); i++) // 행
            {
                for (int j = 0; j < board.GetLength(1); j++)  // 열
                {
                    // 3행, 7행: ㅡ
                    if (i % 4 == 3)
                    {
                        // 3행3열, 3행7열, 7행3열, 7행7열: +
                        if (j % 4 == 3)
                        {
                            board[i, j] = "+";
                        }
                        else
                        {
                            board[i, j] = "ㅡ";
                        }
                    }
                    else if (i % 4 == 1)
                    {
                        // 3열, 7열: |
                        if (j % 4 == 3)
                        {
                            board[i, j] = "|";
                        }
                        else if (j % 4 == 1)
                        {
                            /// 숫자를 배치
                            int numberIndex = (i / 4) * 3 + (j / 4);
                            board[i, j] = $"{num[numberIndex]} ";

                            // 처음에 이랬다가 틀렸다...
                            //board[i, j] = num[3 * ((i - 1) / 4) /*+ 3 * ((j - 1) / 4)*/].ToString();
                        }
                        else
                        {
                            board[i, j] = "  ";
                        }
                    }
                    else
                    {
                        // 3열, 7열: |
                        if (j % 4 == 3)
                        {
                            board[i, j] = "|";
                        }
                        else
                        {
                            board[i, j] = "  ";
                        }
                    }
                }
            }
        }
        // 진행상황 보여주기
        static void ShowBoard()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write(board[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void TicTacToe()
        {
            turnCount++;
            // 화면청소
            Console.Clear();
            ShowBoard();
            Console.WriteLine($"현재 턴: {turnCount}");
            Console.WriteLine("플레이어:X, 컴퓨터:O");
            Console.WriteLine("플레이어의 차례입니다. 1~9 중에서 남은 숫자를 입력하세요");
            // 플레이어 차례
            while (true)
            {
                // 1.기존 방식
                //int input = int.Parse(Console.ReadLine());
                //if (input >= 1 && input <= 9)

                // 2.새로운 방식 시도
                int input;  // 대입하는건 1부터 9다
                if (int.TryParse(Console.ReadLine(), out input) && input >= 1 && input <= 9)
                {
                    // 행(row), 열(col)
                    // 1부터 9를 대입하니까 input-1로 인덱스 관리하는거 당연하다
                    int row = 4 * ((input - 1) / 3) + 1;
                    int col = 4 * ((input - 1) % 3) + 1;
                    // 중복입력방지
                    //if (num[input] == 0)
                    if (board[row, col] == playerMark || board[row, col] == computerMark)
                    {
                        Console.WriteLine("이미 사용된 자리입니다. 다시 입력하십시오");
                        continue;
                    }
                    else
                    {
                        int ascii = (int)playerMark[0]; // X의 ASCII: 88
                        num[input - 1] = ascii; // 매직넘버(ASCII로 바꾸어 숫자대입한다)
                        board[row, col] = playerMark;
                        Console.Clear();
                        ShowBoard();
                        Console.WriteLine($"현재 턴: {turnCount}");
                        Console.WriteLine($"플레이어의 입력값:{input}");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("다시 입력하십시오");
                    continue;
                }
            }
            Check();

            // 컴퓨터 차례
            while (true)
            {
                // 화면청소
                Console.Clear();
                ShowBoard();
                Console.WriteLine($"현재 턴: {turnCount}");
                Console.WriteLine("플레이어:X, 컴퓨터:O");
                Console.WriteLine("컴퓨터의 차례 진행...");

                // 컴퓨터는 바르게 찾은 것만 출력한다
                int computerChoice = new Random().Next(0, num.Length);  // 인덱스로 접근
                                                                        // 이미 선택한건 제외하고 출력하자


                // 행(row), 열(col)
                // 컴퓨터는 0부터 8중에 선택하니까 computerChoice에서 1을 빼면 안된다. 이미 인덱스로 접근하고 있기 때문
                int row = 4 * ((computerChoice) / 3) + 1;
                int col = 4 * ((computerChoice) % 3) + 1;
                // 중복입력방지
                //if (num[input] == 0)
                if (board[row, col] == playerMark || board[row, col] == computerMark)
                {
                    //Console.WriteLine("이미 사용된 자리입니다. 다시 입력하십시오");
                    continue;
                }
                else
                {
                    int ascii = (int)computerMark[0]; // O의 ASCII: 79
                    num[computerChoice] = ascii; // 매직넘버(ASCII로 바꾸어 숫자대입한다)
                    board[row, col] = computerMark;
                    Console.Clear();
                    ShowBoard();
                    Console.WriteLine($"현재 턴: {turnCount}");
                    Console.WriteLine($"컴퓨터의 입력값:{computerChoice}");
                    break;
                }
            }
            Check();
        }
        static void Check()
        {
            int row = 3;
            int rightdownDiagonal = 4;   // 우하향 대각은 index 4 차이
            int rightupDiagonal = 2;    // 우상향 대각은 index 2 차이
                                        // 가로, 세로, 대각선이 모두 같으면 승리
            for (int i = 0; i < row; i++)
            {
                // 플레이어가 선택: 88로 갱신
                // 컴퓨터가 선택: 79로 갱신

                // num을 통해 비교할 수 있으므로 굳이, board를 비교하지 않아도 된다
                // 가로비교
                if (num[i * row] == num[i * row + 1] && num[i * row] == num[i * row + 2])
                {
                    if (num[i * row] == 88)
                    {
                        isPlayerWon = true;
                        break;
                    }
                    else if (num[i * row] == 79)
                    {
                        isPlayerWon = false;
                        break;
                    }
                }
                // 세로비교
                else if (num[i] == num[i + row * 1] && num[i] == num[i + row * 2])
                {
                    if (num[i] == 88)
                    {
                        isPlayerWon = true;
                        break;
                    }
                    else if (num[i] == 79)
                    {
                        isPlayerWon = false;
                        break;
                    }
                }
            }
            // 대각비교
            if (num[0] == num[0 + rightdownDiagonal * 1] && num[0] == num[0 + rightdownDiagonal * 2])
            {
                // 우하향 대각
                if (num[0] == 88)
                {
                    isPlayerWon = true;
                }
                else if (num[0] == 79)
                {
                    isPlayerWon = false;
                }

            }
            else if (num[2] == num[2 + rightupDiagonal * 1] && num[2] == num[2 + rightupDiagonal * 2])
            {
                // 우상향 대각
                if (num[2] == 88)
                {
                    isPlayerWon = true;
                }
                else if (num[2] == 79)
                {
                    isPlayerWon = false;
                }
            }

            if (isPlayerWon)
            {
                isFinished = true;
                Console.WriteLine("플레이어 승리");
            }
            else if (isComWon)
            {
                isFinished = true;
                Console.WriteLine("컴퓨터 승리");
            }
        }

        static void Main(string[] args)
        {
            InitBoard();    // 보드 초기화 (1번)
            while (!isFinished)
            {
                TicTacToe();
            }
        }
    }
}

