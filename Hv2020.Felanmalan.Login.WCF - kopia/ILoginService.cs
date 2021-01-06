using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hv2020.Felanmalan.Login.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ILoginService
    {

        [OperationContract]
        List<LoginData> GetAllLogins();

        [OperationContract]
        LoginData GetLogin(string anvandarnamn, string password);

        [OperationContract]
        void AddLogin(string anvandarnamn, string password);

        [OperationContract]
        void RemoveLogin(int id);

        [OperationContract]
        void UpdateLogin(int id, string anvandarnamn, string salt, string HashPassword);
        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]

    public class LoginData
    {
        public LoginData()
        {

        }
        public LoginData(int loginID, string anvandarnamn, string salt, string losenord)
        {
            this.Anvandarnamn = anvandarnamn;
            this.LoginID = loginID;
            this.Salt = salt;
            this.Losenord = losenord;
        }
        [DataMember]
        public int LoginID { get; set; }
        [DataMember]
        public string Anvandarnamn { get; set; }
        [DataMember]
        public string Salt { get; set; }
        [DataMember]
        public string Losenord { get; set; }
    }
    
}


