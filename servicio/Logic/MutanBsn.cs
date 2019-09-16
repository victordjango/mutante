using myMicroservice.Model;
using myMicroservice.Repostories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace myMicroservice.Logic
{
    /// <summary>
    /// Clase con la logica para detectar si una matriz de  adns es un mutante o no
    /// </summary>
    public class MutanBsn : IMutanBsn
    {
        
        private List<string> GetMutantsPatternsToCompare()
        {
            var patternList = new List<string>();

            foreach (var letter in DnaHelper.DNALetters.ToCharArray())
            {
                patternList.Add(letter.ToString().PadRight(4, letter));
            }
            return patternList;
        }

        public MessageResponse GetApiResultMessageResponse(bool isMutant)
        {
            var messageResponse = new MessageResponse();

            messageResponse.Success = isMutant;
            messageResponse.Message = isMutant ? "MUTANTE" : "NO MUTANTE";

            return messageResponse;
        }

        public MessageResponse GeneralValidation(MutantDnaRequest request)
        {
            return DnaValidator.GeneralValidation(request);
        }
        public bool IsMutant(List<string> dnas)
        {

            dnas = DnaHelper.UpperDna(dnas);

            var muttantPatterns = new List<string>();               
            var verticalDnas = new List<string>();
            var diagonalDnas = new List<string>();


            Parallel.Invoke(() => {
                            muttantPatterns = GetMutantsPatternsToCompare();
                                  }, 
                            () => {
                            verticalDnas = DnaHelper.GetVerticalDnas(dnas);
                                  },
                            () => {
                            diagonalDnas = DnaHelper.GetDiagonalDnas(dnas);
                }
            );


            int countRepeatedLetters = 0;

            //CON DETECTAR 2 O + REPETICIONES CANCELAMOS LA BÚSQUEDA DE LOS PATRONES
            for (int i = 0;  i < muttantPatterns.Count && countRepeatedLetters < 2;  i ++ )
            {
                    var pattern = muttantPatterns[i];
            
                    //ANALISIS EN HORIZONTAL DE LA MATRIZ
                    countRepeatedLetters += dnas
                        .Select(s => Regex.Matches(s, Regex.Escape(pattern)).Count)
                        .Sum();


                    if (countRepeatedLetters < 2)
                    {
                        //ANALISIS EN VERTICAL DE LA MATRIZ
                        countRepeatedLetters += verticalDnas
                                            .Select(s => Regex.Matches(s, Regex.Escape(pattern)).Count)
                                            .Sum();
                    }

                    if (countRepeatedLetters < 2)
                    {


                        //ANALISIS DE LAS DIAGONALES DE LA MATRIX
                        countRepeatedLetters += diagonalDnas
                                            .Select(s => Regex.Matches(s, Regex.Escape(pattern)).Count)
                                            .Sum();
                    }
            }
            return countRepeatedLetters > 1;
        }

        public void InsertDna(List<string> dnas, bool isMutant)
        {
            var dnaRepository = new DnaRepository();

            dnaRepository.InsertDna(dnas, isMutant);
        }

        public MutantStatistic GetMutanDnaStatistics()
        {
            var dnaRepository = new DnaRepository();

            return dnaRepository.GetMutantStatistic();
        }
    }
}
