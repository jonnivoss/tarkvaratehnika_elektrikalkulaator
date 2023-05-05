using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;
using VecUCT = System.Collections.Generic.Dictionary<string, System.Tuple<double, double>>;

namespace AndmeSalvestaja
{
    class CSerializable : AndmeSalvestaja.ISerializable
    {
        public Dictionary<ASSetting, string> setMap { get; set; } = new Dictionary<ASSetting, string>
        {
            { ASSetting.tarbijaAndmed,    "" },  // tarbijaandmete CSV asukoht
            { ASSetting.paketiAndmed,     "" },  // paketiandmete CSV asukoht
            { ASSetting.suurendusLubatud, "0" }, // mäletab kas suurendus on lubatud
            { ASSetting.tumeTaust,        "0" }  // dark mode
        };
        public VecT marketData { get; set; } = new VecT { };
        // Default kasutusmallid
        public VecUCT useCases { get; } = new VecUCT
        {
            { "",             Tuple.Create(   0.0,  0.0)  },
            { "Röster",       Tuple.Create(1200.0,  10.0) },
            { "Tolmuimeja",   Tuple.Create(2000.0,  30.0) },
            { "Televiisor",   Tuple.Create(  90.0, 120.0) },
            { "Pesumasin",    Tuple.Create( 900.0, 180.0) },
            { "Veekeetja",    Tuple.Create(3000.0,  10.0) },
            { "Elektripliit", Tuple.Create(3000.0,  60.0) },
            { "Köögikombain", Tuple.Create( 300.0,  15.0) },
            { "Kohvimasin",   Tuple.Create(1500.0,   5.0) },
            { "Raadio",       Tuple.Create(  50.0, 120.0) },
            { "Munakeetja",   Tuple.Create(1000.0,   6.0) },
            { "Föön",         Tuple.Create(2000.0,  10.0) },
            { "Elektriauto",  Tuple.Create(7200.0, 420.0) }
        };
    }

    public class CAS : AndmeSalvestaja.IAS
    {
        private string path = "";
        private ISerializable data = new CSerializable();

        public CAS(string savepath)
        {
            // Jätab meelde sätetefaili asukoha
            this.path = savepath;
        }

        // FAILI LUGEMINE
        /* Kui faili asukoht on määratletud, siis programm avab ning loeb failist andmed sisse.
         * Väärtus tagastatakse olenevalt faili avamise/lugemise õnnestumisest/ebaõnnestumisest.
         *      
         * TAGASTUSVÄÄRTUSED:
         *      Tagastab boolean väärtuse
         */
        public bool loadFile()
        {
            // Kui sätetefaili asukohta pole valitud
            if (this.path.Length == 0)
            {
                return false;
            }

            try // Proovib lugeda avatud faili
            {
                var contents = File.ReadAllText(this.path);
                this.data = JsonConvert.DeserializeObject<CSerializable>(contents); // Deserialiseerib loetud failid
                return true;
            }
            catch (Exception) // Kui mingil põhjusel faili lugemine ebaõnnestub
            {
                return false;
            }
        }

        // FAILI SALVESTAMINE
        /* Kirjutab sisu faili ning salvestab selle valitud faili path'i.
         *         
         * TAGASTUSVÄÄRTUSED:
         *      Tagastab boolean väärtuse
         */
        public bool saveFile()
        {
            try // Proovib sisu faili salvestada
            {
                var contents = JsonConvert.SerializeObject(this.data, Formatting.Indented);
                File.WriteAllText(this.path, contents);
                return true;
            }
            catch (Exception) // Kui faili salvestamine ebaõnnestub
            {
                return false;
            }
            
        }

        // SÄTETE FAILI MUUTMINE
        /* Funktsiooni kirjeldus siia!
         * 
         * PARAMEETRID (SISEND):
         *      setting: sätte indeks
         *      value: säte
         *      
         * TAGASTUSVÄÄRTUSED:
         *      Tagstab boolean väärtuse  
         */
        public bool changeSetting(ASSetting setting, string value)
        {
            // Kui valitud sätte indeks on piirkonnast väljas
            if ((int)setting >= (int)ASSetting.size)
            {
                return false;
            }

            try
            {
                this.data.setMap[setting] = value;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // SÄTETE HANKIMINE
        /* Funktsioon tagastab sisestatud indeksiga soovitud sätte. 
         * 
         * PARAMEETRID (SISEND):
         *      setting: sätte indeks
         * 
         * TAGASTUSVÄÄRTUSED:
         *      Tagastab sätte
         */
        public string getSetting(ASSetting setting)
        {
            // Kui soovitud sätte indeks on piirkonnast väljas
            if ((int)setting >= (int)ASSetting.size)
            {
                return null;
            }

            return this.data.setMap[setting];
        }

        // BÖRSIANDMETE MÄÄRAMINE
        /* Funktsioon määrab börsiandmeteks sisse antud andmed. 
         * 
         * PARAMEETRID (SISEND):
         *      data: börsiandmed
         *  
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        public void setMarketData(VecT data)
        {
            this.data.marketData = data;
        }

        // BÖRSIANDMETE HANKIMINE
        /* Tagastab programmis seatud börsiandmed.
         *      
         * TAGASTUSVÄÄRTUSED:
         *      this.data.marketData: seatud börsiandmed
         */
        public VecT getMarketData()
        {
            return this.data.marketData;
        }

        // KASUTUSMALLIDE HANKIMINE
        /* Tagastab määratletud kasutusmallid.
         * 
         * TAGASTUSVÄÄRTUSED:
         *      this.data.useCases: eelnevalt määratud kasutusmallid
         */
        public VecUCT getUseCases()
        {
            return this.data.useCases;
        }
    }
}
