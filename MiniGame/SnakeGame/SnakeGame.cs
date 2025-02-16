namespace SnakeGame
{
    internal class SnakeGame
    {
        const int MAP_X = 0;
        const int MAP_Y = 0;
        const int MAP_XSIZE = 30;
        const int MAP_YSIZE = 20;

        //static int x, y;    // 커서의 위치, 캐릭터의 위치 저장에 쓴다
        // 뱀이 길어질 것이므로, 배열로 바꾼다
        static int[] x = new int[100], y = new int[100];  // x, y 좌표값을 저장 총 100개

        static int dir;  // 이동방향 저장
        static int key;  // 입력받은 키 저장
        static int speed = 200;
        static int length = 5;  // 길이(일단 5 고정)

        const string sign_Head = "Ｏ";
        const string sign_Body = "ㅇ";

        static void Title()
        {
            // 키 입력이 있으면 비운다(키 입력이 없는 상태로 만듦)
            while (Console.KeyAvailable)    // 키 입력이 있으면 true
            {
                Console.ReadKey(true);  // 키 버퍼 비우기
            }
            DrawMap(); // 맵 테두리 그리기
            for (int i = MAP_Y + 1; i < MAP_Y + MAP_YSIZE - 1; i++)
            {
                for (int j = MAP_X + 1; j < MAP_X + MAP_XSIZE - 1; j++)
                    PrintTo(j, i, "  "); // 맵 테두리 안쪽을 빈칸으로 채움
            }
            PrintTo(MAP_X + (MAP_XSIZE / 2) - 7, MAP_Y + 5, "+--------------------------+");
            PrintTo(MAP_X + (MAP_XSIZE / 2) - 7, MAP_Y + 6, "         S N A K E         ");
            PrintTo(MAP_X + (MAP_XSIZE / 2) - 7, MAP_Y + 7, "+--------------------------+");
            PrintTo(MAP_X + (MAP_XSIZE / 2) - 7, MAP_Y + 9, " < PRESS ANY KEY TO START > ");
            PrintTo(MAP_X + (MAP_XSIZE / 2) - 7, MAP_Y + 11, "   ◇ ←,→,↑,↓ : Move    ");
            PrintTo(MAP_X + (MAP_XSIZE / 2) - 7, MAP_Y + 12, "   ◇ P : Pause         ");
            PrintTo(MAP_X + (MAP_XSIZE / 2) - 7, MAP_Y + 13, "   ◇ ESC : Quit        ");
            while (true)
            {
                if (Console.KeyAvailable)   // 키 입력 버퍼에 키 입력을 받으면 true, 아무 입력이 없으면 false
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);    // true: 입력된 키를 화면에 표시하지 않음,
                                                                       // .Key: 어떤 키가 눌렸는지 반환된다(왼쪽 화살표 누르면 ConsoleKey.LeftArrow 반환)
                    key = (int)keyInfo.Key;
                    break;
                }
                PrintTo(MAP_X + (MAP_XSIZE / 2) - 7, MAP_Y + 9, " < PRESS ANY KEY TO START > ");
                Thread.Sleep(400);
                PrintTo(MAP_X + (MAP_XSIZE / 2) - 7, MAP_Y + 9, "                            ");
                Thread.Sleep(400);
            }
            Reset(); // 초기화
        }
        static void PrintTo(int x, int y, string s)
        {
            Console.SetCursorPosition(x * 2, y);    // 콘솔창에서 커서의 위치를 x*2, y로 이동
            Console.Write(s);
            //Console.Out.Flush(); // 즉시 출력 반영
        }

        // 맵 테두리 그리는 메서드
        static void DrawMap()
        {
            for (int i = 0; i < MAP_XSIZE; i++)
            {
                PrintTo(MAP_X + i, MAP_Y, "■");
            }
            for (int i = MAP_Y + 1; i < MAP_Y + MAP_YSIZE - 1; i++)
            {
                PrintTo(MAP_X, i, "■");
                PrintTo(MAP_X + MAP_XSIZE - 1, i, "■");
            }
            for (int i = 0; i < MAP_XSIZE; i++)
            {
                PrintTo(MAP_X + i, MAP_Y + MAP_YSIZE - 1, "■");
            }
        }

        // 이동
        static void Move(int dir)
        {
            int i;

            PrintTo(MAP_X + x[length - 1], MAP_Y + y[length - 1], "  "); // 몸통 마지막을 지움
            for (i = length - 1; i > 0; i--) // 몸통 좌표를 한칸씩 옮김
            {
                x[i] = x[i - 1];
                y[i] = y[i - 1];
            }
            PrintTo(MAP_X + x[0], MAP_Y + y[0], sign_Body); // 머리가 있던 곳을 몸통으로 고침
            if (dir == (int)ConsoleKey.LeftArrow) --x[0]; // 방향에 따라 새로운 머리좌표(x[0], y[0])값을 변경
            if (dir == (int)ConsoleKey.RightArrow) ++x[0];
            if (dir == (int)ConsoleKey.UpArrow) --y[0];
            if (dir == (int)ConsoleKey.DownArrow) ++y[0];
            PrintTo(MAP_X + x[i], MAP_Y + y[i], sign_Head); // 새로운 머리좌표값에 머리를 그림
        }

        // 초기화
        static void Reset()
        {
            Console.Clear(); // 화면을 지움

            DrawMap();

            while (Console.KeyAvailable) Console.ReadKey(true); // 버퍼에 있는 키값을 버림
            dir = (int)ConsoleKey.LeftArrow; // 방향 초기화
            speed = 200; // 속도 초기화

            for (int i = 0; i < length; i++)
            {
                x[i] = MAP_XSIZE / 2 + i;
                y[i] = MAP_YSIZE / 2;
                PrintTo(x[i], y[i], sign_Body); // 뱀 몸통값 입력
            }
            PrintTo(x[0], y[0], sign_Head); // 뱀 머리 그림
        }



        static void Main(string[] args)
        {
            Title();

            while (true)
            {
                if (Console.KeyAvailable)   // 키 입력 버퍼에 키 입력을 받으면 true, 아무 입력이 없으면 false
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);    // true: 입력된 키를 화면에 표시하지 않음,
                                                                       // .Key: 어떤 키가 눌렸는지 반환된다(왼쪽 화살표 누르면 ConsoleKey.LeftArrow 반환)
                    key = (int)keyInfo.Key;
                }
                Thread.Sleep(speed);

                switch (key)
                {
                    case (int)ConsoleKey.LeftArrow:
                        if (dir != (int)ConsoleKey.RightArrow) dir = (int)ConsoleKey.LeftArrow;
                        break;
                    case (int)ConsoleKey.RightArrow:
                        if (dir != (int)ConsoleKey.LeftArrow) dir = (int)ConsoleKey.RightArrow;
                        break;
                    case (int)ConsoleKey.UpArrow:
                        if (dir != (int)ConsoleKey.DownArrow) dir = (int)ConsoleKey.UpArrow;
                        break;
                    case (int)ConsoleKey.DownArrow:
                        if (dir != (int)ConsoleKey.UpArrow) dir = (int)ConsoleKey.DownArrow;
                        break;
                    case (int)ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }
                key = 0;    // 입력된 키 값을 초기화

                Move(dir);
            }
        }
    }
}
