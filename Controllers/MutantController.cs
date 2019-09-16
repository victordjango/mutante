using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myMicroservice.Model;
using myMicroservice.Logic;
using myMicroservice.ApiErrors;

namespace myMicroservice.Controllers
{
    [Route("")]
    [ApiController]
    public class MutantController : ControllerBase
    {
        /// <summary>
        /// Metodo POST para determinar si un arreglo de Dnas representa el dna de un mutante o de una persona.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Mutant")]        
        public IActionResult Mutant(MutantDnaRequest request)
        {
            //VALIDACION DE ADNS
            var apiValidation = DnaValidator.GeneralValidation(request);

            if (apiValidation.Success == false)
            {
                return StatusCode(403, apiValidation.Message);
            }
            else
                {

                //OBTENER DNA
                var mutanBsn = new MutanBsn();

                var dnas = DnaHelper.UpperDna(request.Dna.ToList());
                var isMutant = mutanBsn.IsMutant(dnas);

                var apiResult = mutanBsn.GetApiResultMessageResponse(isMutant);

                //REGISTRAR (ASINCRONICAMENTE) => LA GRABACIÓN NO SE INVOLUCRA EN LA RESPUESTA
                var task =Task.Run(() => mutanBsn.InsertDna(dnas, isMutant));  

                if (apiResult.Success)
                    return Ok(apiResult);
                else
                    return StatusCode(403, apiResult.Message);
            }           
           
        } 

        /// <summary>
        /// Metodo GET para recuperar una estadistica de los dnas informados.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Stats")]
        public ActionResult<MutantStatistic> GetStats()
        {
            var mutantBsn = new MutanBsn();
            return mutantBsn.GetMutanDnaStatistics();            
        }

    }
}