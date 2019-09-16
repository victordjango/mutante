using Microsoft.AspNetCore.Mvc;
using myMicroservice.Controllers;
using myMicroservice.Logic;
using System;
using System.Collections.Generic;
using Xunit;

namespace mutantetest
{
    public class MutantControllerTest
    {

        private MutantController _mutantController;

        private IMutanBsn _mutantBsn;

        public MutantControllerTest()
        {

            _mutantBsn = new MutanBsn();
            _mutantController = new MutantController(_mutantBsn);
        }

        [Fact]
        public void Mutant_ReturnsOkResult()
        {
            
            var dnas = new List<string>();

            dnas.Add("GAAT");
            dnas.Add("GGgt");
            dnas.Add("GGgt");
            dnas.Add("GAAt");

            var request = new myMicroservice.Model.MutantDnaRequest();
            request.Dna = dnas;

            var miresult =  _mutantController.Mutant(request);

            Assert.IsType<OkObjectResult>(miresult);

        }

        [Fact]
        public void Mutant_ReturnsForbidkResult()
        {

            var dnas = new List<string>();

            dnas.Add("AAAT");
            dnas.Add("GGgt");
            dnas.Add("GGgt");
            dnas.Add("GAAt");

            var request = new myMicroservice.Model.MutantDnaRequest();
            request.Dna = dnas;

            var miresult = _mutantController.Mutant(request);

            string statusCode = "";

            if ((miresult is ObjectResult))
            {
                var result = (ObjectResult)miresult;
                statusCode = result.StatusCode.GetValueOrDefault().ToString();
            }

            Assert.Equal("403", statusCode);

        }

    }
}
