using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Maze
    {

        public Cellule[] cells; // Contient toutes les cellules
        public int totalCells;

        // Points d'entrée et de sortie
        public int start;
        public int finish;

        // Dimensions du labyrinthe
        public int l;
        public int h;

        public void BuildMaze(int l, int h)
        {
            this.l = l;
            this.h = h;

            this.totalCells = l * h;
            this.cells = new Cellule[totalCells];

            Stack<Cellule> cellsStack = new Stack<Cellule>(); // Le stack des cellules

            int x = 0;
            int y = 0;
            for (int i = 0; i < this.totalCells; i++) // Ajoute les cellules dans la liste de toutes les cellules (pas le stack)
            {
                this.cells[i] = new Cellule(x, y, i);
                x++;
                if (x == l)
                {
                    x = 0;
                    y++;
                }
            }


            Random rnd = new Random();
            int rndKey = rnd.Next(0, this.totalCells);
            Cellule currentCell = this.cells[rndKey]; // Cellule aléatoire pour initialiser la génération du labyrinthe

            int visitedCells = 1; // Nombre de cellules visitées au début de la génération


            // Tant qu'on n'a pas visité toutes les cellules
            while (visitedCells < this.totalCells)
            {

                currentCell.visited = true;

                // neighbours va contenir toutes les cellules voisines visitables
                List<Cellule> neighbours = new List<Cellule>();
                if (currentCell.x > 0) // Si on n'est pas sur le bord gauche
                {
                    if (!(this.cells[currentCell.key - 1]).visited) // Si la cellule sur la gauche n'a pas déjà été visitée
                    {
                        neighbours.Add(this.cells[currentCell.key - 1]); // On ajoute la cellule de gauche à la liste des voisins visitables
                    }
                }
                if (currentCell.x + 1 < l) // Si on n'est pas sur le bord droit
                {
                    if (!(this.cells[currentCell.key + 1]).visited) // Si la cellule à droite n'a pas déjà été visitée
                    {
                        neighbours.Add(this.cells[currentCell.key + 1]);
                    }
                }
                if (currentCell.y > 0) // Si on n'est pas sur le bord du haut
                {
                    if (!(this.cells[currentCell.key - l]).visited) // Si la cellule juste en haut n'a pas déjà été visitée
                    {
                        neighbours.Add(this.cells[currentCell.key - l]);
                    }
                }
                if (currentCell.y + 1 < h) // Si on n'est pas sur le bord du bas
                {
                    if (!this.cells[currentCell.key + l].visited) // Si la cellule juste en bas n'a pas déjà été visitée
                    {
                        neighbours.Add(this.cells[currentCell.key + l]);
                    }
                }

                // Si on n'est pas bloqué :
                if (neighbours.Count > 0)
                {
                    // Sélectionner une cellule au hasard
                    int r = rnd.Next(neighbours.Count);
                    Cellule newCell = neighbours[r];

                    // Abbattre les murs
                    if (newCell.key == currentCell.key - 1)
                    {
                        currentCell.border[3] = false; // On abbat la bordure ouest de la cellule courante
                        newCell.border[1] = false; // On abbat la bordure est de la cellule voisine de gauche
                    }
                    else if (newCell.key == currentCell.key + 1) // Pareil qu'au dessus mais pour la cellule à droite
                    {
                        currentCell.border[1] = false;
                        newCell.border[3] = false;
                    }
                    else if (newCell.key == currentCell.key - l) // Pour la cellule en haut
                    {
                        currentCell.border[0] = false;
                        newCell.border[2] = false;

                    }
                    else if (newCell.key == currentCell.key + l) // Pour la cellule en bas
                    {
                        currentCell.border[2] = false;
                        newCell.border[0] = false;

                    }

                    // Ajout de currentCell dans le stack
                    cellsStack.Push(currentCell);

                    currentCell = newCell;
                    visitedCells++;
                }
                else // Si on se retrouve bloqué...
                {
                    currentCell = cellsStack.Pop(); // ... On dépile
                }
            }

            // Points d'entrée et de sortie aléatoires
            this.start = rnd.Next(0, this.totalCells);
            this.finish = rnd.Next(0, this.totalCells);

        }


        // Pour résoudre le labyrinthe
        public List<int> solve()
        {
            // Le stack contenant toutes les cellules visitées.
            // A la fin de l'exécution, ne contiendra que les cases faisant partie du chemin
            Stack<Cellule> visitedCells = new Stack<Cellule>();
            
            Cellule currentCell = cells[this.start];

            visitedCells.Push(currentCell); // On commence par placer la cellule de départ dans le stack

            List<Cellule> neighbours = new List<Cellule>(); // Contiendra tous les voisins parcourables au fur et à mesure du parcours des cellules

            while (currentCell.key != this.finish) // Tant qu'on est pas arrivé à la sortie...
            {

                currentCell.visitedSolve = true; // On note qu'on a visité la cellule courante

                // NORTH ... Si la cellule nord peut être visitée...
                if (!currentCell.border[0] && !this.cells[currentCell.key - l].border[2] && !this.cells[currentCell.key - l].visitedSolve)
                {
                    neighbours.Add(this.cells[currentCell.key - l]); // On la rajoute à neighbours
                }

                // Et ainsi de suite
                
                // EST
                if (!currentCell.border[1] && !this.cells[currentCell.key + 1].border[3] && !this.cells[currentCell.key + 1].visitedSolve)
                {
                    neighbours.Add(this.cells[currentCell.key + 1]);
                }


                // SOUTH
                if (!currentCell.border[2] && !this.cells[currentCell.key + l].border[0] && !this.cells[currentCell.key + l].visitedSolve)
                {
                    neighbours.Add(this.cells[currentCell.key + l]);
                }


                // WEST
                if (!currentCell.border[3] && !this.cells[currentCell.key - 1].border[1] && !this.cells[currentCell.key - 1].visitedSolve)
                {
                    neighbours.Add(this.cells[currentCell.key - 1]);
                }


                Random rnd = new Random();
                if (neighbours.Count > 0) // Si neighbours contient des cellules...
                {
                    visitedCells.Push(currentCell);
                    currentCell = neighbours[rnd.Next(0, neighbours.Count)]; // On en sélectionne une au hasard
                }
                else
                {
                    currentCell = visitedCells.Pop(); // Sinon on fait demi tour car on est bloqué
                }
                neighbours.Clear(); // Réinitialisation pour le tour suivant
                

            }

            List<int> solution = new List<int>();
            
            // On place les indexes de toutes les cellules faisant partie du chemin final dans une liste
            foreach (var current in visitedCells)
            {
                solution.Insert(0, current.key);
            }
            
            return solution;
            
        }

    }
}
