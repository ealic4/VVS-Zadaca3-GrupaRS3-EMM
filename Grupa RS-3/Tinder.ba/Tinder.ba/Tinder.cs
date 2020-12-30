using System;
using System.Collections.Generic;
using System.Text;

namespace Tinder.ba
{
    public class Tinder
    {
        #region Atributi

        List<Korisnik> korisnici;
        List<Chat> razgovori;

        #endregion

        #region Properties

        public List<Korisnik> Korisnici
        {
            get => korisnici;
        }

        public List<Chat> Razgovori
        {
            get => razgovori;
        }

        #endregion

        #region Konstruktor

        public Tinder()
        {
            korisnici = new List<Korisnik>();
            razgovori = new List<Chat>();
        }

        #endregion

        #region Metode

        public void RadSaKorisnikom(Korisnik k, int opcija)
        {
            if (opcija == 0)
            {
                Korisnik postojeci = korisnici.Find(korisnik => korisnik.Ime == k.Ime);
                if (postojeci != null)
                    throw new InvalidOperationException("Korisnik već postoji!");

                korisnici.Add(k);
            }
            else if (opcija == 1)
            {
                Korisnik postojeci = korisnici.Find(korisnik => korisnik.Ime == k.Ime);
                if (postojeci == null)
                    throw new InvalidOperationException("Korisnik ne postoji!");

                korisnici.Remove(k);

                List<Chat> razgovoriZaBrisanje = new List<Chat>();
                foreach (Chat c in razgovori)
                {
                    if (c.Korisnici.Find(korisnik => korisnik.Ime == k.Ime) != null)
                        razgovoriZaBrisanje.Add(c);
                }

                foreach (Chat brisanje in razgovoriZaBrisanje)
                    razgovori.Remove(brisanje);
            }
        }

        public void DodavanjeRazgovora(List<Korisnik> korisnici, bool grupniChat)
        {
            if (korisnici == null || korisnici.Count < 2 || (!grupniChat && korisnici.Count > 2))
                throw new ArgumentException("Nemoguće dodati razgovor!");

            if (grupniChat)
                razgovori.Add(new GrupniChat(korisnici));

            else
                razgovori.Add(new Chat(korisnici[0], korisnici[1]));
        }

        /// <summary>
        /// Metoda u kojoj se određuju i vraćaju svi kompatibilni korisnici u listi parova.
        /// Parovi ne smiju imati duplikate - dva para ne smiju imati ista dva korisnika.
        /// Ukoliko nema nijednog korisnika, baca se izuzetak.
        /// Korisnici su kompatibilni ako lokacija k1 odgovara željenoj lokaciji k2 i obrnuto
        /// i ukoliko se godine k1 nalaze između minimalnih i maksimalnih željenih godina k2 i obrnuto.
        /// </summary>
        /// <returns></returns>
        public List<Tuple<Korisnik, Korisnik>> DajSveKompatibilneKorisnike()
        {
            if (Korisnici.Count < 1)
                throw new Exception("Nema korisnika!");

            List<Tuple<Korisnik, Korisnik>> parovi = new List<Tuple<Korisnik, Korisnik>>();
            
            foreach(Korisnik prvi in korisnici)
            {
                foreach(Korisnik drugi in korisnici)
                {
                    bool postoji = false;
                    if (prvi == drugi)
                        postoji = true;
                    foreach (Tuple<Korisnik, Korisnik> par in parovi)
                    {
                        if (par.Item1 == prvi && par.Item2 == drugi)
                            postoji = true;
                        else if (par.Item2 == prvi && par.Item1 == drugi)
                            postoji = true;
                    }
                    if (!postoji)
                    {
                        if (prvi.ZeljenaLokacija == drugi.Lokacija && prvi.Lokacija == drugi.ZeljenaLokacija
                            && prvi.Godine >= drugi.ZeljeniMinGodina && prvi.Godine <= drugi.ZeljeniMaxGodina
                            && drugi.Godine >= prvi.ZeljeniMinGodina && drugi.Godine <= prvi.ZeljeniMaxGodina)
                            parovi.Add(new Tuple<Korisnik, Korisnik>(prvi, drugi));
                    }
                }
            }
            List<Tuple<Korisnik, Korisnik>> paroviZaBrisanje = new List<Tuple<Korisnik, Korisnik>>();
            foreach (Tuple<Korisnik, Korisnik> par in parovi)
            {
                if (par.Item1 == par.Item2)
                    paroviZaBrisanje.Add(par);
            }

            return parovi;
        }

        public bool DaLiJeSpajanjeUspjesno(Chat c, IRecenzija r)
        {
            if (c is GrupniChat)
                throw new InvalidOperationException("Grupni chatovi nisu podržani!");

            if (c.Poruke.Find(poruka => poruka.IzračunajPotencijalPoruke() == 100) != null
                && r.DajUtisak() == "Pozitivan")
                return true;

            else
                return false;
        }

        public int PotencijalChata(Chat c)
        {
            if (c is GrupniChat)
                throw new InvalidOperationException("Grupni chatovi nisu podržani!");

            int potencijal = 0;
            foreach(Poruka p in c.Poruke)
            {
                potencijal += p.IzračunajPotencijalPoruke();
            }

            return potencijal;
        }
        #endregion
    }
}
