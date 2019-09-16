using myMicroservice.Framework;
using myMicroservice.Model;
using System.Linq;

namespace myMicroservice.Caching
{
    public class InvalidDnaCacheRapper
    {

        private SimpleMemoryCache<MutantDnaRequest> _cache;
        private static InvalidDnaCacheRapper _instance;

        private InvalidDnaCacheRapper()
        {
            _cache = new SimpleMemoryCache<MutantDnaRequest>();
        }

        public static InvalidDnaCacheRapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InvalidDnaCacheRapper();
                }
                return _instance;
            }
        }

        public bool IsInDnaInvalidCache(object key)
        {
            return  _cache.IsInCache(key);
        }

        public void AddToDnaInvalidCAche(MutantDnaRequest dnaRequest)
        {
            _cache.GetOrCreate(dnaRequest.Dna.ToList().GetDeterministicHashCode(), () => dnaRequest);
        }
    }
}
