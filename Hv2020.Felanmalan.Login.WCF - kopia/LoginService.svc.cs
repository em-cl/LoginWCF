using System;
using System.Collections.Generic;
using System.Linq;
using Hv2020.Felanmalan.Login.WCF.Utils;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hv2020.Felanmalan.Login.WCF
{
    public class LoginService : ILoginService
    {
        public void AddLogin(string anvandarnamn, string password)
        {
            LoginData loginData = new LoginData();
            DataProcessing start = new DataProcessing();
            anvandarnamn=start.CleanInput(anvandarnamn);
            password=start.CleanInput(password);

            //exempel på ytterligare kontroll av användar inmatning
            if(anvandarnamn!=null&&password!=null) {
            string salt = start.CreateSalt();
                loginData.Anvandarnamn = anvandarnamn;
                loginData.Salt = salt;
                loginData.Losenord = start.PasswordBuilder(salt, password);
                
                //spara och överför med objekt. salt attributet är med för tydlighetens skull
                using(LoginModel db = new LoginModel()) {
                    Login login = new Login();
                    login.Anvandarnamn = loginData.Anvandarnamn;
                    login.Salt = loginData.Salt;
                    login.Losenord = loginData.Losenord;
                    db.Login.Add(login);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Skapa lista med logins sorterat efter användarnamn
        /// </summary>
        /// <returns></returns>
        public List<LoginData> GetAllLogins()
        {
            List<LoginData> resultat = new List<LoginData>();
            using (LoginModel db = new LoginModel()) {
                var LoginLista = db.Login.OrderBy(e=>e.Anvandarnamn).ToList();

                LoginLista.ForEach(login => resultat.Add(new LoginData(login.LoginID, login.Anvandarnamn, login.Salt, login.Losenord)));
            }
            return resultat;
        }
        /// <summary>
        /// 
        /// öppna databaskoppling
        /// tvätta användarnamn inmatning
        /// 
        /// ifall användarnamn finns i databas
        ///     hämta login
        ///     gör salt från databas och tvättad imatning password till hash
        ///  
        ///     ifall hash == password i databas
        ///         sätt värden på attribut till objekt som ska till kontroller
        /// 
        /// skicka objekt
        ///     
        /// </summary>
        /// <param name="anvandarnamn"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public LoginData GetLogin(string anvandarnamn, string password)
        {
            LoginData resultat = new LoginData();
            DataProcessing start = new DataProcessing();
            List<LoginData> tempLogins = new List<LoginData> ();

            using (LoginModel db = new LoginModel()) {
                anvandarnamn = start.CleanInput(anvandarnamn);
                var login = db.Login.Where(e => e.Anvandarnamn == anvandarnamn).Select(e => e).FirstOrDefault();
                Console.WriteLine(login.LoginID.ToString()+" "+ login.Anvandarnamn +" "+ login.Salt +" "+login.Losenord);
                if (login != null) {

                    if (login.Losenord == start.PasswordBuilder(login.Salt, start.CleanInput(password))) {
                        resultat = new LoginData(login.LoginID, login.Anvandarnamn, login.Salt, login.Losenord);
                    }

                }
                return resultat;
                //db.SaveChanges();
            }

        }
        /// <summary>
        /// ta bort login
        /// </summary>
        /// <param name="id"></param>
        public void RemoveLogin(int id)
        {
            //öppna databas
            using (LoginModel db = new LoginModel()) {
                //hitta med id från hiddenfield
                var login = db.Login.Find(id);
                //tabort rad i databas
                db.Login.Remove(login);
                //spara
                db.SaveChanges();
            }//stäng databas
               
        }

        public void UpdateLogin(int id, string anvandarnamn, string salt, string HashPassword)
        {
            DataProcessing start = new DataProcessing();
            //öppna databas
            using (LoginModel db = new LoginModel()) {
                //hitta med id från hiddenfield
                Login login = db.Login.Find(id);
                db.Login.Attach(login);
                if (anvandarnamn != login.Anvandarnamn) {
                    //eftersom användarinmatningar går genom whitelist
                    login.Anvandarnamn = start.CleanInput(anvandarnamn);
                }
                if (salt != login.Salt) {
                    login.Salt = salt;
                }
                if (HashPassword != login.Losenord) {
                    login.Losenord = HashPassword;
                }
                //spara
                db.SaveChanges();
            }//stäng databas
        }
    }
}
