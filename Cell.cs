using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Cellule
    {
        public bool visited = false; // A-t-elle été visitée pendant la génération du labyrinthe ?
        public bool visitedSolve = false; // A-t-elle été visitée pendant la génération de la solution ?
        public int key { get; private set; } // La clef de la cellule (de 0 à N)

        // Les coordonnées
        public int x { get; private set; }
        public int y;

        public bool[] border = new bool[] { true, true, true, true }; // Par défaut, une cellule a tous ses murs

        public Cellule(int x, int y, int k)
        {
            this.key = k;
            this.x = x;
            this.y = y;
        }
    }
}
