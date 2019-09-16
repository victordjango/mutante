using myMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myMicroservice.Logic
{
    public interface IMutanBsn
    {
        MessageResponse GetApiResultMessageResponse(bool isMutant);
        bool IsMutant(List<string> dnas);
        void InsertDna(List<string> dnas, bool isMutant);
        MutantStatistic GetMutanDnaStatistics();
        MessageResponse GeneralValidation(MutantDnaRequest request);
    }
}
