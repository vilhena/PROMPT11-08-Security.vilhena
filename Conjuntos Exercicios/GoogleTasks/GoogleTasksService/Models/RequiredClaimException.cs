using System;

namespace GoogleTasksService.Models
{
    class RequiredClaimException : Exception
    {
        public string Claim { get; set; }
        public RequiredClaimException(string claim):base()
        {
            Claim = claim;
        }
    }
}
