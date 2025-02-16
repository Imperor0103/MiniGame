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

        static int food_x, food_y;	//food의 좌표값을 저장 

        static int score;          //점수 저장: reset함수에 의해 초기화됨
        static int best_score = 0; //최고 점수 저장: reset함수에 의해 초기화 되지 않음 
        static int last_score = 0; //마지막 점수 저장: reset함수에 의해 초기화 되지 않음


        static int dir;  // 이동방향 저장
        static int key;  // 입력받은 키 저장
        static int speed = 200;
        static int length;  // 길이

        const string sign_Head = "Ｏ";
        const string sign_Body = "ㅇ";
        const string sign_Food = "☆";   // 먹이

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

        // 랜덤위치에 음식 추가
        static void Food()
        {
            bool food_crush_on = false; // food가 뱀 몸통 좌표에 생길 경우 true
            PrintTo(MAP_X, MAP_Y + MAP_YSIZE, " ");		//점수표시 
            Console.WriteLine($"  Score : {score}  Last Score : {last_score}  Best Score : {best_score}");

            Random rand = new Random();

            while (true)
            {
                food_crush_on = false;
                food_x = rand.Next(1, MAP_XSIZE - 1); // 난수를 좌표값에 넣음
                food_y = rand.Next(1, MAP_YSIZE - 1);

                for (int i = 0; i < length; i++) // food가 뱀 몸통과 겹치는지 확인
                {
                    if (food_x == x[i] && food_y == y[i])
                    {
                        food_crush_on = true; // 겹치면 food_crush_on을 true로 설정
                        break;
                    }
                }

                if (food_crush_on)
                {
                    continue; // 겹쳤을 경우 while문을 다시 시작
                }

                PrintTo(MAP_X + food_x, MAP_Y + food_y, sign_Food); // 안 겹쳤을 경우 좌표값에 food를 찍고
                speed = Math.Max(50, speed - 3); // 속도 증가 (최소 50 이하로 내려가지 않도록)
                break;
            }
        }


        // 이동
        static void Move(int dir)
        {
            // 먹이
            if (x[0] == food_x && y[0] == food_y)
            {
                score += 10;
                Food();
                length++;
                x[length - 1] = x[length - 2];
                y[length - 1] = y[length - 2];
            }
            // 벽과 충돌
            if (x[0] == 0 || x[0] == MAP_XSIZE - 1 || y[0] == 0 || y[0] == MAP_YSIZE - 1)
            {
                GameOver();
                return;
            }
            // 몸통과 충돌
            for (int i = 1; i < length; i++)
            {
                if (x[0] == x[i] && y[0] == y[i])
                {
                    GameOver();
                    return;
                }
            }
            // 충돌하지 않았다면 
            // 몸통부터 1칸씩 옮긴다
            PrintTo(MAP_X + x[length - 1], MAP_Y + y[length - 1], "  "); // 몸통 마지막을 지움
            for (int i = length - 1; i > 0; i--) /// 몸통 좌표를 한칸씩 옮김
            {
                x[i] = x[i - 1];
                y[i] = y[i - 1];
            }
            PrintTo(MAP_X + x[0], MAP_Y + y[0], sign_Body); /// 머리가 있던 곳을 몸통으로 고침
            // 머리를 1칸 옮긴다
            /// 기본 방향은 왼쪽으로 설정되어있다
            if (dir == (int)ConsoleKey.LeftArrow) --x[0]; // 방향에 따라 새로운 머리좌표(x[0], y[0])값을 변경
            if (dir == (int)ConsoleKey.RightArrow) ++x[0];
            if (dir == (int)ConsoleKey.UpArrow) --y[0];
            if (dir == (int)ConsoleKey.DownArrow) ++y[0];
            PrintTo(MAP_X + x[0], MAP_Y + y[0], sign_Head); // 새로운 머리좌표값에 머리를 그림
        }

        static void GameOver()
        {
            PrintTo(MAP_X + (MAP_XSIZE / 2) - 6, MAP_Y + 5, "+----------------------+");
            PrintTo(MAP_X + (MAP_XSIZE / 2) - 6, MAP_Y + 6, "       GAME OVER        ");
            PrintTo(MAP_X + (MAP_XSIZE / 2) - 6, MAP_Y + 7, "+----------------------+");
            PrintTo(MAP_X + (MAP_XSIZE / 2) - 6, MAP_Y + 8, $" YOUR SCORE : {score}");

            if (score > best_score)
            {
                best_score = score;
                PrintTo(MAP_X + (MAP_XSIZE / 2) - 4, MAP_Y + 10, " BEST SCORE ");
            }

            Thread.Sleep(500);
            Console.ReadKey(true);
            Title();
        }

        // 초기화
        static void Reset()
        {
            Console.Clear(); // 화면을 지움

            DrawMap();

            while (Console.KeyAvailable) Console.ReadKey(true); // 버퍼에 있는 키값을 버림
            dir = (int)ConsoleKey.LeftArrow; /// 방향 초기화(기본값: 왼쪽방향)
            speed = 200;    // 속도 초기화 
            length = 5;     // 뱀 길이 초기화 
            score = 0;		// 점수 초기화 

            for (int i = 0; i < length; i++)
            {
                x[i] = MAP_XSIZE / 2 + i;
                y[i] = MAP_YSIZE / 2;
                PrintTo(x[i], y[i], sign_Body); // 뱀 몸통값 입력
            }
            PrintTo(x[0], y[0], sign_Head); // 뱀 머리 그림
            Food();
        }

        static void Pause()
        {
            while (true)
            {
                if (key == (int)ConsoleKey.P)
                {
                    PrintTo(MAP_X + (MAP_XSIZE / 2) - 9, MAP_Y, "< PAUSE : PRESS ANY KEY TO RESUME > ");
                    Thread.Sleep(400);
                    PrintTo(MAP_X + (MAP_XSIZE / 2) - 9, MAP_Y, "                                    ");
                    Thread.Sleep(400);
                }
                else
                {
                    // 아무 키나 누르면 다시 게임이 재개된다
                    DrawMap();
                    return;
                }
                if (Console.KeyAvailable)
                {
                    key = (int)Console.ReadKey(true).Key;
                }
            }
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
                    case (int)ConsoleKey.P:  // 'P' 키를 눌렀을 때 (일시정지)
                        Pause();
                        break;
                }
                key = 0;    // 입력된 키 값을 초기화

                Move(dir);
            }
        }
    }
}
