using System.Collections;
using System.Collections.Generic;

namespace GarbageCollection.WebApi.WebApiController
{
    public class ReturnValue
    {
        public bool Status { get; set; }
        public List<BuisnessObject.ViewModel.MainViewModel.CollectionEntry> Data { get; set; }
        public List<BuisnessObject.ViewModel.MainViewModel.CollectionVerificationEntry> ColVerData { get; set; }
    }
}