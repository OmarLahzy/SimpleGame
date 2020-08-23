using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace PCMAN
{
    class Program
    {
        class Game
        {
            private int _rows;
            private int _cols;
            private player _player;
            private Enemy _enemy;
            public object[,] items;
            private bool _Gameover = false;

            public Game(int rows, int cols, player player, Enemy enemy)
            {
                this._rows = rows;
                this._cols = cols;
                this._player = player;
                this._enemy = enemy;
                items = new object[_rows, _cols];
            }
            public void LoadGame()
            {
                var jsonText = File.ReadAllText("..//..//..//Gamemap (2).json");
                var obj = JsonConvert.DeserializeObject<JObject>(jsonText);

                foreach (var i in obj)
                {
                    switch (i.Key)
                    {
                        case "GoldCheast":
                            items[(int)obj[i.Key]["Cellrow"], (int)obj[i.Key]["Cellcol"]] = "Goldcheast";
                            break;
                        case "silverCheast":
                            items[(int)obj[i.Key]["Cellrow"], (int)obj[i.Key]["Cellcol"]] = "silvercheast";
                            break;
                        case "bronzeCheast":
                            items[(int)obj[i.Key]["Cellrow"], (int)obj[i.Key]["Cellcol"]] = "bronzecheast";
                            break;
                        case "bronzekey":
                            items[(int)obj[i.Key]["Cellrow"], (int)obj[i.Key]["Cellcol"]] = 3;
                            break;
                        case "goldkey":
                            items[(int)obj[i.Key]["Cellrow"], (int)obj[i.Key]["Cellcol"]] = 1;
                            break;
                        case "silverkey":
                            items[(int)obj[i.Key]["Cellrow"], (int)obj[i.Key]["Cellcol"]] = 2;
                            break;
                        case "Enemy":
                            items[(int)obj[i.Key]["Cellrow"], (int)obj[i.Key]["Cellcol"]] = "Enemy";
                            break;
                        case "diamond":
                            items[(int)obj[i.Key]["Cellrow"], (int)obj[i.Key]["Cellcol"]] = "diamond";
                            break;
                        default:
                            break;
                    }
                }



            }
            public void Update()
            {
                show_map();

                Console.Write("Enetr your move (up/down/right/left) : ");
                var ch = Console.ReadKey().Key;

                switch (ch)
                {
                    case ConsoleKey.UpArrow:
                        if (_player.postionrow == 0)
                        {

                            Console.WriteLine("You hit a wall");
                        }
                        else
                        {
                            _player.postionrow -= 1;
                            Collect_object();
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (_player.postionrow == _rows - 1)
                        {
                            Console.WriteLine("You hit a wall");
                        }
                        else
                        {
                            _player.postionrow += 1;
                            Collect_object();
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (_player.postioncol == _cols - 1)
                        {
                            Console.WriteLine("You hit a wall");
                        }
                        else
                        {
                            _player.postioncol += 1;
                            Collect_object();
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (_player.postioncol == 0)
                        {
                            Console.WriteLine("You hit a wall");
                        }
                        else
                        {
                            _player.postioncol -= 1;
                            Collect_object();
                        }
                        break;
                    default:
                        Console.WriteLine("You Hit the Wrong Key...");
                        break;
                }
                
            }

            private void Collect_object()
            {
                if (items[_player.postionrow, _player.postioncol] != null)
                {
                    if (items[_player.postionrow, _player.postioncol].ToString().Contains("Goldcheast"))
                    {
                        Goldcheast g = new Goldcheast();
                        _player.upgrade(g);

                        if (_player._keys.Contains(g.key_type))
                        {
                            items[_player.postionrow, _player.postioncol] = null;
                        }
                    }
                    else if (items[_player.postionrow, _player.postioncol].ToString().Contains("bronzecheast"))
                    {
                        bronzecheast b = new bronzecheast();
                        _player.upgrade(b);

                        if (_player._keys.Contains(b.key_type))
                        {
                            items[_player.postionrow, _player.postioncol] = null;
                        }
                    }
                    else if (items[_player.postionrow, _player.postioncol].ToString().Contains("silvercheast"))
                    {
                        silvercheast s = new silvercheast();
                        _player.upgrade(s);

                        if (_player._keys.Contains(s.key_type))
                        {
                            items[_player.postionrow, _player.postioncol] = null;
                        }
                       
                    }
                    else if (items[_player.postionrow, _player.postioncol].ToString().Contains("silvercheast"))
                    {
                        _player.upgrade(new silvercheast());
                    }
                    else if (items[_player.postionrow, _player.postioncol].ToString().Contains("1"))
                    {
                        Console.WriteLine("you collected Gold Key ...");
                        items[_player.postionrow, _player.postioncol] = null;
                        _player.Add_key(1);

                    }
                    else if (items[_player.postionrow, _player.postioncol].ToString().Contains("2"))
                    {
                        Console.WriteLine("you collected bronze Key ...");
                        items[_player.postionrow, _player.postioncol] = null;
                        _player.Add_key(2);
                    }
                    else if (items[_player.postionrow, _player.postioncol].ToString().Contains("3"))
                    {
                        Console.WriteLine("you collected Silver Key ...");
                        items[_player.postionrow, _player.postioncol] = null;
                        _player.Add_key(3);
                    }
                    else if (items[_player.postionrow, _player.postioncol].ToString().Contains("Enemy"))
                    {
                        Console.WriteLine("Enemy attcked ...");
                        items[_player.postionrow, _player.postioncol] = null;
                        _player.takedamge(_enemy);
                    }
                    else if (items[_player.postionrow, _player.postioncol].ToString().Contains("diamond"))
                    {
                        Console.WriteLine("You Collected The Hidden diamond You Won ...");
                        items[_player.postionrow, _player.postioncol] = null;
                        _Gameover = true;
                        
                    }
                }
                else
                {
                    Console.WriteLine("Nothig Found ....");
                }
            }

            private void show_map()
            {
                Console.Write("\n-----------------------------------------------------------------------------------------------\n");

                for (int i = 0; i < _rows; i++)
                {
                    for (int j = 0; j < _cols; j++)
                    {
                        Console.Write("|\t");
                       if(i==_player.postionrow && j == _player.postioncol)
                       {
                            Console.Write("PLAYER");
                        }
                        Console.Write(items[i, j]+"\t");
                    }
                    Console.Write("\n-----------------------------------------------------------------------------------------------\n");
                }
                
            }

            public bool Game_over()
            {
                return _Gameover;
            }

            
        }
        class status
        {
            public int hitpoints;
            public int weponepoints;
        }
        abstract class cheastkeys : status
        {
            public int key_type;
            public String cheastname;
        }
        class player : status
        {
            public List<int> _keys = new List<int>();
            private cheastkeys _cheast;
            public int postionrow = 0;
            public int postioncol = 0;

            public player(int hitpoints ,int weponepoints)
            {
                this.hitpoints = hitpoints;
                this.weponepoints = weponepoints;
            }
            public void Add_key(int key)
            {
                this._keys.Add(key);
            }

            public void upgrade(cheastkeys cheast)
            {
                this._cheast = cheast;

                if (_keys.Contains(_cheast.key_type))
                {
                     Console.WriteLine("You Collected {0} Cheast", _cheast.cheastname);
                    
                    this.hitpoints += _cheast.hitpoints;
                    this.weponepoints += _cheast.weponepoints;
                }
                else
                {
                    Console.WriteLine("You Dont have a {0} Key",_cheast.cheastname);
                }
            }

            public void takedamge(Enemy enemy)
            {
                hitpoints -= enemy.weponepoints;
                Console.WriteLine("Player Taken Damge hitpoints becom {0} ", hitpoints);
            }
        }
        class Enemy : status
        {
            public Enemy(int hitpoints, int weponepoints)
            {
                this.hitpoints = hitpoints;
                this.weponepoints = weponepoints;
            }
        }
        class Goldcheast:cheastkeys
            {
                public Goldcheast()
                {
                    this.cheastname = "Gold";
                    this.hitpoints = 50;
                    this.weponepoints = 10;
                    this.key_type = 1;
                }

            }
        class bronzecheast : cheastkeys
            {
                public bronzecheast()
                {
                    this.cheastname = "Bronze";
                    this.hitpoints = 20;
                    this.weponepoints = 0;
                    this.key_type = 2;
                }

            }
        class silvercheast : cheastkeys
            {
                public silvercheast()
                {
                    this.cheastname = "Silver";
                    this.hitpoints = 40;
                    this.weponepoints = 10;
                    this.key_type = 3;
                }

            }

        static void Main(string[] args)
        {
            player p = new player(40,40);
            Enemy e = new Enemy(50, 10);
            Game g = new Game(6,6, p,e);
            g.LoadGame();

            Console.WriteLine("Player Hitpoints is {0} and wepone is {1} ", p.hitpoints, p.weponepoints);

            do
            {
                g.Update();

            }while(!g.Game_over());

            
        }
    }
}