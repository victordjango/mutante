using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myMicroservice.Logic
{
    /// <summary>
    /// Clases con funcionalidad para el manejo de los DNAS (Horizontales, Veritales , Diagonales)
    /// </summary>
    public static class DnaHelper
    {
        public const string DNALetters = "ATCG";

        /// <summary>
        /// Metodo que recupera la lista vertial de los Dnas
        /// </summary>
        /// <param name="dnas"></param>
        /// <returns>Lista en vertial de los dnas</returns>
        public static List<string> GetVerticalDnas(List<string> dnas)
        {
            var verticalDnas = new List<string>();
            var rows = dnas.Count;
            for (int i = 0; i < rows; i++)
            {
                var verticalString = string.Join("", dnas.Select(dna => dna[i]));
                verticalDnas.Add(verticalString);
            }
            return verticalDnas;
        }
        /// <summary>
        /// Metodo que recupera la lista de Dnas en Diagonal
        /// </summary>
        /// <param name="dnas"></param>
        /// <returns></returns>
        public static List<string> GetDiagonalDnas(List<string> dnas)
        {
            var N = dnas.Count;

            var diagonalDnas = new List<string>();

            for (int x = 0; x <= N - 4; x++) //ITERO POR FILA
            {
                for (int y = 0; y <= N - 4; y++)  //ITERA POR COLUMNA
                {
                    string diagonal = "";

                    for (int i = 0; i < 4; i++) // DIAGONAL
                    {
                        diagonal += dnas[x + i][y + i].ToString();
                    }
                    diagonalDnas.Add(diagonal);
                }
            }

            for (int x = N - 1; x >= 3; x--) //ITERO POR FILA DESCENDENTE HASTA LA FILA INDICE 3 (4 FILA)
            {
                for (int y = 0; y <= N - 4; y++)  //ITERACIÓN POR COLUMNA
                {
                    string diagonal = "";

                    for (int i = 0; i < 4; i++) // DIAGONAL
                    {
                        diagonal += dnas[x - i][y + i].ToString();
                    }
                    diagonalDnas.Add(diagonal);
                }
            }

            return diagonalDnas;
        }

        /// <summary>
        /// Metodo para normalizar los dnas en UPPERCASE
        /// </summary>
        /// <param name="dnas"></param>
        /// <returns></returns>
        public static List<string> UpperDna(List<string> dnas)
        {
            return dnas
                .Select(subdna => subdna.ToUpper().Trim())
                .ToList();
        }




    }
}
