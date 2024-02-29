using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace HexagonStealthGame {
    public partial class StealthGame : Form {

        public int radius = 10;
        public int size = 45;
        public const float ratio = 0.86602540378f; //rt3/2
        public Random rn = new Random ();

        public SolidBrush[,] grid;
        public List<int[]> changed = new List<int[]> ();
        List<Enemy> enemies = new List<Enemy> ();

        bool editMode = false;

        int playerX = -1;
        int playerY = -1;
        List<int[]> markedPath;

        public StealthGame () {
            InitializeComponent ();
            LoadMap ();
        }

        void Tick () {
            DateTime start = DateTime.Now;
            foreach(Enemy e in enemies) {
                e.TakeTurn ();
            }

            DrawGrid ();

            System.Threading.Thread.Sleep (Math.Max(100 - (int) Math.Round ((DateTime.Now - start).TotalMilliseconds), 0));
        }

        void AddEnemies (bool vision = true) {
            foreach (Enemy e in enemies)
                if (vision == true)
                    foreach (int[] n in FindVision (new List<int[]> (), e.vision, e.x, e.y, e.direction))
                        if (chk_vision.Checked == true && grid[n[0], n[1]].Color.ToArgb() != Color.Blue.ToArgb()) {
                            grid[n[0], n[1]] = new SolidBrush (Color.LightSlateGray);

                            bool present = false;
                            foreach (int[] c in changed)
                                if (c[0] == n[0] && c[1] == n[1])
                                    present = true;
                            if (present == false)
                                changed.Add (new int[] { n[0], n[1] });
                        }

            foreach (Enemy e in enemies) {
                grid[e.x, e.y] = new SolidBrush (Color.Red);
                changed.Add (new int[] { e.x, e.y });
            }
        }

        void SaveMap (string file = "default.txt") {
            StreamWriter write = new StreamWriter (file);
            write.WriteLine (radius);
            write.WriteLine (size);

            for (int x = 0; x < size; x++) {
                string line = "";
                for (int y = 0; y < size; y++) {
                    if (grid[x, y].Color.ToArgb () == Color.LightSlateGray.ToArgb () || grid[x, y].Color.ToArgb () == Color.Red.ToArgb ())
                        line += Color.White.ToArgb ().ToString() + ';';
                    else
                        line += grid[x, y].Color.ToArgb ().ToString () + ';';
                }
                write.WriteLine (line);
            }

            write.WriteLine (enemies.Count);

            foreach(Enemy e in enemies) {
                write.WriteLine (e.x.ToString() + ';' + e.y.ToString () + ';' + e.vision.ToString () + ';' + e.direction.ToString ());
            }

            write.WriteLine (playerX.ToString() + ';' + playerY.ToString());

            write.Close ();
        }

        void LoadMap (string file = "default.txt") {
            enemies.Clear ();
            List<string> lines;
            string[] line;
            try {
                lines = new List<string> (File.ReadAllLines (file));
            } catch {
                return;
            }

            radius = int.Parse (lines[0]);
            lines.RemoveAt (0);
            size = int.Parse (lines[0]);
            lines.RemoveAt (0);
            grid = new SolidBrush[size, size];
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    changed.Add (new int[] { x, y });
                }
            }

            for (int x = 0; x < size; x++) {
                line = lines[0].Split (';');
                for (int y = 0; y < size; y++) {
                    grid[x, y] = new SolidBrush (Color.FromArgb (int.Parse (line[y])));
                }
                lines.RemoveAt (0);
            }

            int numEnemies = int.Parse (lines[0]);
            lines.RemoveAt (0);
            for (int x = 0; x < numEnemies; x++) {
                line = lines[0].Split (';');
                Enemy e = new Enemy (int.Parse (line[0]), int.Parse (line[1]), int.Parse (line[2]), int.Parse (line[3]), this);
                enemies.Add (e);
                lines.RemoveAt (0);
            }

            line = lines[0].Split (';');
            playerX = int.Parse (line[0]);
            playerY = int.Parse (line[1]);
        }

        List<int[]> FindVision (List<int[]> range, int i, int x, int y, int d) {
            if (x < 0 || x >= size || y < 0 || y >= size)
                return range;
            if (i < 0 || grid[x,y].Color.ToArgb() == Color.DimGray.ToArgb())
                return range;

            range.Add (new int[] { x, y });

            switch (d) {
                case 0:
                    range = FindVision (range, i - 1, x - 1, y + (x % 2 == 0 ? 0 : -1), d);
                    range = FindVision (range, i - 1, x, y - 1, d);
                    range = FindVision (range, i - 1, x + 1, y + (x % 2 == 0 ? 0 : -1), d);
                    break;
                case 1:
                    range = FindVision (range, i - 1, x, y - 1, d);
                    range = FindVision (range, i - 1, x + 1, y, d);
                    range = FindVision (range, i - 1, x + 1, y + (x % 2 == 0 ? 1 : -1), d);
                    break;
                case 2:
                    range = FindVision (range, i - 1, x + 1, y + (x % 2 == 0 ? 0 : -1), d);
                    range = FindVision (range, i - 1, x + 1, y + (x % 2 == 0 ? 1 : 0), d);
                    range = FindVision (range, i - 1, x, y + 1, d);
                    break;
                case 3:
                    range = FindVision (range, i - 1, x + 1, y + (x % 2 == 0 ? 1 : 0), d);
                    range = FindVision (range, i - 1, x, y + 1, d);
                    range = FindVision (range, i - 1, x - 1, y + (x % 2 == 0 ? 1 : 0), d);
                    break;
                case 4:
                    range = FindVision (range, i - 1, x, y + 1, d);
                    range = FindVision (range, i - 1, x - 1, y + (x % 2 == 0 ? 0 : -1), d);
                    range = FindVision (range, i - 1, x - 1, y + (x % 2 == 0 ? 1 : 0), d);
                    break;
                case 5:
                    range = FindVision (range, i - 1, x - 1, y + (x % 2 == 0 ? 0 : -1), d);
                    range = FindVision (range, i - 1, x - 1, y + (x % 2 == 0 ? 1 : 0), d);
                    range = FindVision (range, i - 1, x, y - 1, d);
                    break;
            }

            return range;
        }

        void DrawGrid () {
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    if (grid[x, y].Color.ToArgb () == Color.LightSlateGray.ToArgb () || grid[x, y].Color.ToArgb () == Color.Orange.ToArgb ()) {
                        grid[x, y] = new SolidBrush (Color.White);
                        changed.Add (new int[] { x, y });
                    }
                }
            }

            AddEnemies ();

            if (markedPath != null)
                foreach (int[] n in markedPath) {
                    if (grid[n[0], n[1]].Color.ToArgb () == Color.Red.ToArgb () || grid[n[0], n[1]].Color.ToArgb () == Color.Blue.ToArgb ())
                        continue;
                    grid[n[0], n[1]].Color = Color.Orange;
                    changed.Add (new int[] { n[0], n[1] });
                }

            float offset;
            Pen black = new Pen (Color.Black, 3);
            Graphics g = CreateGraphics ();

            foreach (int[] c in changed) {
                offset = (c[0] + 1) % 2 * radius * ratio;
                float r = (c[0] * radius * 1.5f) + 20f;
                float n = (c[1] * radius * ratio * 2f) + 20f;
                g.FillPolygon (grid[c[0], c[1]], new PointF[6] {
                            new PointF (r + radius,    n + offset),
                            new PointF (r + radius/2,  n + ratio*radius + offset),
                            new PointF (r - radius/2,  n + ratio*radius + offset),
                            new PointF (r - radius,    n + offset),
                            new PointF (r - radius/2,  n - ratio*radius + offset),
                            new PointF (r + radius/2,  n - ratio*radius + offset)
                        });
                g.DrawPolygon (black, new PointF[6] {
                            new PointF (r + radius,    n + offset),
                            new PointF (r + radius/2,  n + ratio*radius + offset),
                            new PointF (r - radius/2,  n + ratio*radius + offset),
                            new PointF (r - radius,    n + offset),
                            new PointF (r - radius/2,  n - ratio*radius + offset),
                            new PointF (r + radius/2,  n - ratio*radius + offset)
                        });
            }

            changed.Clear ();

            if (playerX == -1 || playerY == -1)
                return;

            g.DrawString ("Q", new Font (FontFamily.GenericSerif, 10f, FontStyle.Bold), new SolidBrush (Color.Black), ((playerX - 1) * radius * 1.5f) + 13f, (playerY * radius * ratio * 2f) + ((playerX + 1) % 2 * radius * ratio) - 1f);
            g.DrawString ("W", new Font (FontFamily.GenericSerif, 10f, FontStyle.Bold), new SolidBrush (Color.Black), (playerX * radius * 1.5f) + 13f, ((playerY-1) * radius * ratio * 2f) + ((playerX + 1) % 2 * radius * ratio) + 15f);
            g.DrawString ("E", new Font (FontFamily.GenericSerif, 10f, FontStyle.Bold), new SolidBrush (Color.Black), ((playerX + 1) * radius * 1.5f) + 13f, ((playerY) * radius * ratio * 2f) + ((playerX + 1) % 2 * radius * ratio) - 1f);
            g.DrawString ("D", new Font (FontFamily.GenericSerif, 10f, FontStyle.Bold), new SolidBrush (Color.Black), ((playerX + 1) * radius * 1.5f) + 13f, ((playerY+1) * radius * ratio * 2f) + ((playerX + 1) % 2 * radius * ratio) - 1f);
            g.DrawString ("S", new Font (FontFamily.GenericSerif, 10f, FontStyle.Bold), new SolidBrush (Color.Black), (playerX * radius * 1.5f) + 13f, ((playerY+1) * radius * ratio * 2f) + ((playerX + 1) % 2 * radius * ratio) + 15f);
            g.DrawString ("A", new Font (FontFamily.GenericSerif, 10f, FontStyle.Bold), new SolidBrush (Color.Black), ((playerX - 1) * radius * 1.5f) + 13f, ((playerY+1) * radius * ratio * 2f) + ((playerX + 1) % 2 * radius * ratio) - 1f);
            changed.Add (new int[] { playerX - 1, playerY + 1 });
            changed.Add (new int[] { playerX - 1, playerY });
            changed.Add (new int[] { playerX - 1, playerY - 1 });
            changed.Add (new int[] { playerX, playerY + 1 });
            changed.Add (new int[] { playerX + 1, playerY + 1 });
            changed.Add (new int[] { playerX + 1, playerY });
            changed.Add (new int[] { playerX + 1, playerY - 1 });
            changed.Add (new int[] { playerX, playerY - 1 });
        }

        public List<int[]> AStarPath (int startX, int startY, int endX, int endY) {
            List<Node> openNodes = new List<Node> ();
            Node[,] nodes = new Node[size, size];

            float temp = FindDistance (startX, startY, endX, endY);
            Node start = new Node (startX, startY, 0, temp, temp);
            nodes[startX, startY] = start;
            openNodes.Add (start);

            if (grid[endX, endY].Color.ToArgb () == Color.DimGray.ToArgb ())
                return null;

            while (openNodes.Count != 0) {
                Node curr = openNodes[0];
                foreach (Node x in openNodes) {
                    if (x.f < curr.f)
                        curr = x;
                }

                if (curr.x == endX && curr.y == endY) {
                    List<int[]> path = new List<int[]> ();
                    while (curr != null) {
                        path.Add (new int[2] { curr.x, curr.y });
                        curr = curr.parent;
                    }
                    path.Reverse ();
                    return path;
                }

                openNodes.Remove (curr);

                foreach (int[] pos in new int[][] { new int[2] { curr.x+1, curr.y},
                                                    new int[2] { curr.x+1, curr.y + ((curr.x) % 2 == 0 ? 1 : -1)},
                                                    new int[2] { curr.x, curr.y+1},
                                                    new int[2] { curr.x, curr.y-1},
                                                    new int[2] { curr.x-1, curr.y},
                                                    new int[2] { curr.x-1, curr.y + ((curr.x) % 2 == 0 ? 1 : -1)} }) {
                    if (pos[0] < 0 || pos[1] < 0 || pos[0] >= size || pos[1] >= size)
                        continue;
                    if (grid[pos[0], pos[1]].Color.ToArgb () == Color.DimGray.ToArgb ())
                        continue;

                    Node neighbour = nodes[pos[0], pos[1]];
                    if (neighbour == null) {
                        neighbour = new Node (pos[0], pos[1], float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
                        nodes[pos[0], pos[1]] = neighbour;
                    }

                    float tempg = curr.g;
                    tempg++;

                    if (tempg < neighbour.g) {
                        neighbour.g = tempg;
                        neighbour.parent = curr;
                        if (neighbour.h == float.PositiveInfinity)
                            neighbour.h = FindDistance (neighbour.x, neighbour.y, endX, endY);
                        neighbour.f = neighbour.g + neighbour.h;
                        if (openNodes.Contains (neighbour) == false)
                            openNodes.Add (neighbour);
                    }
                }
            }
            return null;
        }

        float FindDistance (int x1, int y1, int x2, int y2) {
            return (float) Math.Sqrt (Math.Pow (x1 - x2, 2) + Math.Pow (y1 + (((x1 + 1) % 2) * radius * ratio) - y2 - (((x2 + 1) % 2) * radius * ratio), 2));
        }

        class Node {
            public int x;
            public int y;
            public float f;
            public float g;
            public float h;
            public Node parent;
            public float min;

            public Node (int x, int y, float g, float f, float h, Node parent = null) {
                this.x = x;
                this.y = y;
                this.f = f;
                this.h = h;
                this.g = g;
                this.parent = parent;
            }
        }

    private void Btn_draw_Click (object sender, EventArgs e) {
            DrawGrid ();
        }

        private void Btn_save_Click (object sender, EventArgs e) {
            if (txt_file.Text == "")
                SaveMap ();
            else
                SaveMap (txt_file.Text + ".txt");
        }

        private void Btn_load_Click (object sender, EventArgs e) {
            if (txt_file.Text == "")
                LoadMap ();
            else
                LoadMap (txt_file.Text + ".txt");
            DrawGrid ();
        }

        private void Btn_clear_Click (object sender, EventArgs e) {
            Graphics g = CreateGraphics ();
            g.Clear (Color.Gray);
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    changed.Add (new int[] { x, y });
                }
            }
        }

        private void StealthGame_MouseDown (object sender, MouseEventArgs e) {
            if (chk_edit.Checked == false)
                return;

            editMode = true;

            StealthGame_MouseMove (sender, e);

            if (e.Button == MouseButtons.Middle)
                editMode = false;
        }

        private void StealthGame_MouseUp (object sender, MouseEventArgs e) {
            if (chk_edit.Checked == false) {
                int x = (int) Math.Round ((MousePosition.X - this.Location.X - 5) / (radius * 1.5f)) - 1;
                int y = (int) Math.Round ((MousePosition.Y - this.Location.Y - 30) / (radius * ratio * 2f)) - 1;

                if (x < 0 || x >= size || y < 0 || y >= size)
                    return;

                markedPath = AStarPath (playerX, playerY, x, y);
                DrawGrid ();
            } else
                editMode = false;
        }

        private void StealthGame_MouseMove (object sender, MouseEventArgs e) {
            if (editMode == false)
                return;

            Color c = Color.White;
            if (e.Button == MouseButtons.Left)
                c = Color.DimGray;
            else if (e.Button == MouseButtons.Middle)
                c = Color.Red;

            int x = (int) Math.Round ((MousePosition.X - this.Location.X - 5) / (radius * 1.5f)) - 1;
            int y = (int) Math.Round ((MousePosition.Y - this.Location.Y - 30) / (radius * ratio * 2f)) - 1;

            if (x < 0 || x >= size || y < 0 || y >= size)
                return;

            if (grid[x, y].Color.ToArgb () == Color.Red.ToArgb ())
                for (int i = 0; i < enemies.Count; i++)
                    if (enemies[i].x == x && enemies[i].y == y) { 
                        enemies.RemoveAt (i);
                        break;
                    }

            if (c.ToArgb () == Color.Red.ToArgb ())
                if (grid[x, y].Color.ToArgb () != Color.Red.ToArgb ())
                    enemies.Add (new Enemy (x, y, 5, rn.Next (0, 6), this));
                else {
                    playerX = x;
                    playerY = y;
                    c = Color.Blue;
                }


            grid[x, y] = new SolidBrush (c);
            changed.Add (new int[] { x, y });

            DrawGrid ();
        }

        private void Btn_tick_Click (object sender, EventArgs e) {
            Tick ();
        }

        private void StealthGame_KeyDown (object sender, KeyEventArgs e) {
            if (playerX == -1 || playerY == -1)
                return;

            switch (e.KeyCode) {
                case Keys.W:
                    grid[playerX, playerY] = new SolidBrush (Color.White);
                    changed.Add (new int[] { playerX, playerY });
                    if (grid[playerX, playerY - 1].Color.ToArgb () != Color.DimGray.ToArgb ())
                        playerY--;
                    break;
                case Keys.E:
                    grid[playerX, playerY] = new SolidBrush (Color.White);
                    changed.Add (new int[] { playerX, playerY });
                    if (grid[playerX + 1, playerY + (playerX % 2 == 0 ? 0 : -1)].Color.ToArgb () != Color.DimGray.ToArgb ()) {
                        playerX++;
                        playerY += (playerX % 2 == 0 ? -1 : 0);
                    }
                    break;
                case Keys.D:
                    grid[playerX, playerY] = new SolidBrush (Color.White);
                    changed.Add (new int[] { playerX, playerY });
                    if (grid[playerX + 1, playerY + (playerX % 2 == 0 ? 1 : 0)].Color.ToArgb () != Color.DimGray.ToArgb ()) {
                        playerX++;
                        playerY += (playerX % 2 == 0 ? 0 : 1);
                    }
                    break;
                case Keys.S:
                    grid[playerX, playerY] = new SolidBrush (Color.White);
                    changed.Add (new int[] { playerX, playerY });
                    if (grid[playerX, playerY + 1].Color.ToArgb () != Color.DimGray.ToArgb ())
                        playerY++;
                    break;
                case Keys.A:
                    grid[playerX, playerY] = new SolidBrush (Color.White);
                    changed.Add (new int[] { playerX, playerY });
                    if (grid[playerX - 1, playerY + (playerX % 2 == 0 ? 1 : 0)].Color.ToArgb () != Color.DimGray.ToArgb ()) {
                        playerX--;
                        playerY += (playerX % 2 == 0 ? 0 : 1);
                    }
                    break;
                case Keys.Q:
                    grid[playerX, playerY] = new SolidBrush (Color.White);
                    changed.Add (new int[] { playerX, playerY });
                    if (grid[playerX - 1, playerY + (playerX % 2 == 0 ? 0 :-1)].Color.ToArgb () != Color.DimGray.ToArgb ()) {
                        playerX--;
                        playerY += (playerX % 2 == 0 ? -1 : 0);
                    }
                    break;
                case Keys.F:
                    if (markedPath == null)
                        return;
                    grid[playerX, playerY] = new SolidBrush (Color.White);
                    changed.Add (new int[] { playerX, playerY });
                    playerX = markedPath[1][0];
                    playerY = markedPath[1][1];
                    break;
                default:
                    return;
            }
            grid[playerX, playerY] = new SolidBrush (Color.Blue);
            changed.Add (new int[] { playerX, playerY });

            if (markedPath != null) {
                if (playerX == markedPath[markedPath.Count - 1][0] && playerY == markedPath[markedPath.Count - 1][1])
                    markedPath = null;
                else
                    markedPath = AStarPath (playerX, playerY, markedPath[markedPath.Count - 1][0], markedPath[markedPath.Count - 1][1]);
            }
            DrawGrid ();

            Tick ();
        }
    }

    public class Enemy {
        public int x;
        public int y;
        public int vision;
        public int direction;
        public List<int[]> path = null;
        public StealthGame game;

        public Enemy (int x, int y, int vision, int direction, StealthGame game) {
            this.x = x;
            this.y = y;
            this.vision = vision;
            this.direction = direction;
            this.game = game;
        }

        public void TakeTurn () {
            //Options:
            //0 - Move to destination
            //1 - Find Destination
            //2 - Remain Idle
            List<int> options = new List<int> ();
            if (path != null)
                options.Add (0);
            else {
                options.Add (1);
                options.Add (2);
                options.Add (2);
                options.Add (2);
            }

            switch(options[game.rn.Next(0, options.Count)]) {
                case 0:
                    FollowPath ();
                    break;
                case 1:
                    do {
                        int newX = x + game.rn.Next (-5, 6);
                        int newY = y + game.rn.Next (-5, 6);
                        if (newX >= 0 && newX < game.size && newY >= 0 && newY < game.size)
                            path = game.AStarPath (x, y, newX, newY);
                    } while (path == null);
                    break;
            }
        }

        void FollowPath () {
            int newX = path[0][0];
            int newY = path[0][1];
            int reqDirection = 0;

            if (x == newX && y - 1 == newY)
                reqDirection = 0;
            else if (x + 1 == newX && y + (x % 2 == 0 ? 0 : -1) == newY)
                reqDirection = 1;
            else if (x + 1 == newX && y + (x % 2 == 0 ? 1 : 0) == newY)
                reqDirection = 2;
            else if (x == newX && y + 1 == newY)
                reqDirection = 3;
            else if (x - 1 == newX && y + (x % 2 == 0 ? 1 : 0) == newY)
                reqDirection = 4;
            else if (x - 1 == newX && y + (x % 2 == 0 ? 0 : -1) == newY)
                reqDirection = 5;

            if (direction == reqDirection) {
                game.grid[x, y] = new SolidBrush (Color.White);
                game.changed.Add (new int[] { x, y });
                x = newX;
                y = newY;
                path.RemoveAt (0);

                if (path.Count == 0)
                    path = null;
            } else {
                if ((reqDirection - direction + 6) % 6 <= 3)
                    direction = (direction + 1) % 6;
                else
                    direction = (direction + 5) % 6;
            }
        }
    }
}