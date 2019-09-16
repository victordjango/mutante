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

        private readonly IMutanBsn _mutantBsn;

        public MutantController(IMutanBsn mutantBsn)
        {
            _mutantBsn = mutantBsn;
        }


        /// <summary>
        /// Metodo POST para determinar si un arreglo de Dnas representa el dna de un mutante o de una persona.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Mutant")]        
        public IActionResult Mutant(MutantDnaRequest request)
        {
            //VALIDACION DE ADNS
            var apiValidation = _mutantBsn.GeneralValidation(request);

            if (apiValidation.Success == false)
            {
                return StatusCode(403, apiValidation.Message);
            }
            else
                {

                var dnas = request.Dna.ToList();

                //OBTENER DNA                              
                var isMutant = _mutantBsn.IsMutant(dnas);
                var apiResult = _mutantBsn.GetApiResultMessageResponse(isMutant);

                //REGISTRAR (ASINCRONICAMENTE) => LA GRABACIÓN NO SE INVOLUCRA EN LA RESPUESTA
                var task =Task.Run(() => _mutantBsn.InsertDna(dnas, isMutant));  

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