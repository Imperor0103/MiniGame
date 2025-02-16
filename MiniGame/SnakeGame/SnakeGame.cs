namespace SnakeGame
{
    internal class SnakeGame
    {
        static int x, y;    // 커서의 위치, 캐릭터의 위치 저장에 쓴다
        static int dir;  // 이동방향 저장
        static int key;  // 입력받은 키 저장
        static int speed = 200;

        const string sign_Head = "Ｏ";
        const string sign_Body = "ㅇ";

        static void Title()
        {
            // 키 입력이 있으면 비운다(키 입력이 없는 상태로 만듦)
            while (Console.KeyAvailable)    // 키 입력이 있으면 true
            {
                Console.ReadKey(true);  // 키 버퍼 비우기
            }

            PrintTo(15, 5, "+--------------------------+");
            PrintTo(15, 6, "         S N A K E         ");
            PrintTo(15, 7, "+--------------------------+");

            while (true)
            {
                if (Console.KeyAvailable)   // 키 입력 버퍼에 키 입력을 받으면 true, 아무 입력이 없으면 false
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);    // true: 입력된 키를 화면에 표시하지 않음,
                                                                       // .Key: 어떤 키가 눌렸는지 반환된다(왼쪽 화살표 누르면 ConsoleKey.LeftArrow 반환)
                    key = (int)keyInfo.Key;
                    break;
                }
                PrintTo(15, 10, " < PRESS ANY KEY TO START > ");
                Thread.Sleep(400);
                PrintTo(15, 10, "                            ");
                Thread.Sleep(400);
            }
            Console.Clear();
            Console.CursorVisible = false;  // 커서를 안보이게 
            // 머리가 생성될 위치(출력은 PrintTo)
            x = 15;
            y = 8;
        }
        static void PrintTo(int x, int y, string s)
        {
            Console.SetCursorPosition(x * 2, y);    // 콘솔창에서 커서의 위치를 x*2, y로 이동
            Console.Write(s);
            //Console.Out.Flush(); // 즉시 출력 반영
        }

        static void Move(int dir)
        {
            Console.Clear();
            if (dir == (int)ConsoleKey.LeftArrow) --x;
            if (dir == (int)ConsoleKey.RightArrow) ++x;
            if (dir == (int)ConsoleKey.UpArrow) --y;
            if (dir == (int)ConsoleKey.DownArrow) ++y;

            PrintTo(x, y, sign_Head);
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
