using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Tinder.ba.Tinder t = new Tinder.ba.Tinder();
            Tinder.ba.Korisnik korisnik = new Tinder.ba.Korisnik("name", "*+Sifra123", Tinder.ba.Lokacija.Sarajevo, Tinder.ba.Lokacija.Mostar, 30, true);
  
            
            for(int i=0; i<100000; i++)
            {
                korisnik.Ime = i.ToString();
                t.RadSaKorisnikom(korisnik, 0);
            }
            t.DajSveKompatibilneKorisnike();
        }
    }
}
