using System;
using System.Threading.Tasks;

namespace FireAuth
{
    public interface IAuth
    {
        bool SignUpWithEmailPassword(string email, string password);   
        Task<string> LoginWithEmailPassword(string email, string password);
    }
}