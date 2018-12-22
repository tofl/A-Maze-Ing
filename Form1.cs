using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        public int l;
        public int h;
        public int dim = 15;
        public int marginTop = 40;

        Maze maze;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void DrawMaze()
        {
            System.Drawing.Graphics graphicsObj;

            graphicsObj = this.CreateGraphics();
            
            this.maze.BuildMaze(l, h);
            
            // On initialise les crayons (grid = les bordures des cellules, border = la bordure du labyrinthe)
            Pen grid = new Pen(System.Drawing.Color.Black, 1);
            Pen border = new Pen(System.Drawing.Color.Black, 3);

            // Le labyrinthe sera placé au dessus d'une zone blanche des mêmes dimensions
            SolidBrush canvas = new SolidBrush(System.Drawing.Color.White);
            Graphics gr;
            gr = this.CreateGraphics();
            Rectangle rCanvas = new Rectangle(0, 0 + this.marginTop, this.l * this.dim, this.h * this.dim);
            gr.FillRectangle(canvas, rCanvas);
            canvas.Dispose();
            gr.Dispose();

            // Les bordures du labyrinthe
            graphicsObj.DrawRectangle(border, 0, 0 + this.marginTop, this.l * this.dim, this.h * this.dim);

            

            int line = 0; // La ligne actuellement parcourue
            int cell = 0; // La colone actuellement parcourue

            // On parcourt chacune des cellules du labyrinthe
            for (int i = 1; i < this.maze.totalCells; i++)
            {

                // On dessine successivement chacune des bordures de la cellule courante
                Cellule currentCell = this.maze.cells[i - 1];
                if (currentCell.border[0])
                {
                    graphicsObj.DrawLine(grid, cell * this.dim, line * this.dim + this.marginTop, cell * this.dim + this.dim, line * this.dim + this.marginTop);
                }
                if (currentCell.border[1])
                {
                    graphicsObj.DrawLine(grid, cell * this.dim + this.dim, line * this.dim + this.marginTop, cell * this.dim + this.dim, line * this.dim + this.dim + this.marginTop);
                }
                if (currentCell.border[2])
                {
                    graphicsObj.DrawLine(grid, cell * this.dim, line * this.dim + this.dim + this.marginTop, cell * this.dim, line * this.dim + this.dim + this.marginTop);
                }
                if (currentCell.border[3])
                {
                    graphicsObj.DrawLine(grid, cell * this.dim, line * this.dim + this.marginTop, cell * this.dim, line * this.dim + this.dim + this.marginTop);
                }
                
                // Si la cellule courante se trouve être le point d'entrée, on dessine un carré vert
                if (this.maze.start == i-1)
                {
                    SolidBrush start = new SolidBrush(System.Drawing.Color.Green);
                    Graphics formGraphics;
                    formGraphics = this.CreateGraphics();
                    Rectangle r = new Rectangle(cell * this.dim + 2, line * this.dim + 2 + this.marginTop, this.dim - 3, this.dim - 3);
                    formGraphics.FillRectangle(start, r);
                    start.Dispose();
                    formGraphics.Dispose();
                }

                // Si la cellule courante se trouve être le point d'entrée, on dessine un carré rouge dedans
                if (this.maze.finish == i-1)
                {
                    SolidBrush finish = new SolidBrush(System.Drawing.Color.Red);
                    Graphics formGraphics;
                    formGraphics = this.CreateGraphics();
                    Rectangle r = new Rectangle(cell * this.dim + 2, line * this.dim + 2 + this.marginTop, this.dim - 3, this.dim - 3);
                    formGraphics.FillRectangle(finish, r);
                    finish.Dispose();
                    formGraphics.Dispose();
                }

                cell++; // Fin de l'itération, on passe à la colone suivante

                if (i % this.l == 0) // Si on arrive à la fin de la ligne
                {
                    line++;
                    cell = 0;
                }
            }

        }

        
        // Résoud le labyrinthe et génère l'affichage du chemin.
        private void solveMazePath()
        {
            // Initialisation des variables
            Maze maze = this.maze;
            List<int> solution;
            solution = this.maze.solve();
            solution.RemoveAt(0); // On retire la première case du chemin, sinon la case d'entrée du labyrinthe est masqué

            int line = 0;
            int cell = 0;

            // On parcourt toutes les cases du labyrinthe
            for (int i = 0; i < this.maze.totalCells; i++)
            {
                // Si la case actuellement parcourue est comprise dans les cases du chemin...
                if (solution.Contains(i - 1) && this.maze.cells[i - 1].key != this.maze.start)
                {
                    SolidBrush solutionPath = new SolidBrush(System.Drawing.Color.Pink);
                    Graphics formGraphics;
                    formGraphics = this.CreateGraphics();
                    Rectangle r = new Rectangle(cell * this.dim + 2, (line - 1) * this.dim + 2 + this.marginTop, this.dim - 3, this.dim - 3);
                    formGraphics.FillRectangle(solutionPath, r);
                    solutionPath.Dispose();
                    formGraphics.Dispose();
                }

                cell++;

                // Si on arrive à la fin de la ligne, on réinitialise cell (on revient au début) et on passe à la ligne suivante
                if (i % this.l == 0)
                {
                    line++;
                    cell = 0;
                }
            }
            
        }

        // Bouton de génération du labyrinthe
        private void button1_Click(object sender, EventArgs e)
        {
            this.Refresh(); // On efface tous les autres dessins
            this.l = 20; // Longueur & hauteur par défaut
            this.h = 20;

            // Si l'utilisateur spécifie des dimensions
            if (textBox1.Text != "")
            {
                this.l = Int32.Parse(textBox1.Text);
            }
            if (textBox2.Text != "")
            {
                this.h = Int32.Parse(textBox2.Text);
            }
            
            this.maze = new Maze();

            this.DrawMaze();

            // Le labyrinthe est généré, on autorise donc l'utilisateur à afficher une solution si il le souhaite
            button2.Enabled = true;
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // Le bouton pour afficher la solution du labyrinthe
        private void button2_Click(object sender, EventArgs e)
        {
            this.solveMazePath(); // Dessine la solution
            button2.Enabled = false; // Le chemin a été affiché, l'utilisateur ne peut plus le redessiner.
        }
    }
}
