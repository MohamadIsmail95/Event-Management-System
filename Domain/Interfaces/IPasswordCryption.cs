using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPasswordCryption
    {
        string EncodePasswordToBase64(string password);
        string DecodeFrom64(string encodedData);
    }
}
