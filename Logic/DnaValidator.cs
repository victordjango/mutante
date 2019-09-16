using System.Collections.Generic;
using System.Linq;
using myMicroservice.Model;
using myMicroservice.Caching;
using myMicroservice.Framework;

namespace myMicroservice.Logic
{
    /// <summary>
    /// Clase con validaciones de ingreso de DNAs
    /// </summary>
    public static class DnaValidator
    {
        /// <summary>
        /// Clase para validar un ingreso correcto de Dnas
        /// </summary>
        /// <param name="dnas">Lista de Dnas</param>
        /// <returns>Respuesta con mensaje de validación</returns>
        public static MessageResponse ValidateDna(List<string> dnas)
        {
            var response = new MessageResponse();
            response.Success = true;

            int count = 0;

            dnas.ForEach(subdna =>
            {
                var otherLetters = subdna.Except(DnaHelper.DNALetters);
                count += otherLetters.Count();
            });

            if (count != 0)
            {
                response.Success = false;
                response.Message = "CARACTERS INCORRECTOS";
                return response;
            }
            else
            {
                var isSquare = IsSquareDnaMatrix(dnas);
                if (!isSquare)
                {
                    response.Success = false;
                    response.Message = "LA MATRIX DE DNA NO ES CUADRADA";
                }
            }

            return response;
        }
        /// <summary>
        /// Clase que verifica si una cadena de DNAs es cuadrada
        /// </summary>
        /// <param name="dnas"></param>
        /// <returns></returns>
        public static bool IsSquareDnaMatrix(List<string> dnas)
        {
            var rows = dnas.Count;

            var verifyRows = dnas.Select(subdna => subdna)
                .Where(subdna => subdna.Length == rows)
                .Count();

            return rows == verifyRows;

        }


        public static MessageResponse GeneralValidation(MutantDnaRequest request)
        {
            var apiValidation = new MessageResponse();

            var cache = InvalidDnaCacheRapper.Instance;

            if (cache.IsInDnaInvalidCache(request.Dna.ToList().GetDeterministicHashCode()))
            {
                apiValidation.Success = false;
                apiValidation.Message = "ADN INVALIDO";
                return apiValidation;
            }
            else
            {
                //NORMALIZACION DE DNAS
                var upperDna = DnaHelper.UpperDna(request.Dna.ToList());

                //VALIDACION DE DNAS
                 apiValidation = DnaValidator.ValidateDna(upperDna);

                if (apiValidation.Success == false)
                {
                    cache.AddToDnaInvalidCAche(request);
                }

                return apiValidation;
            }
            
        }

    }
}
