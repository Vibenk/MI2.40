using System.Linq;
using System.Reflection;
using System.Text;
/// <summary>
/// Minden állapot osztály őse.
/// </summary>
abstract class AbsztraktÁllapot : ICloneable
{
    // Megvizsgálja, hogy a belső állapot állapot-e.
    // Ha igen, akkor igazat ad vissza, egyébként hamisat.
    public abstract bool ÁllapotE();
    // Megvizsgálja, hogy a belső állapot célállapot-e.
    // Ha igen, akkor igazat ad vissza, egyébként hamisat.
    public abstract bool CélÁllapotE();
    // Visszaadja az alapoperátorok számát.
    public abstract int OperátorokSzáma();
    // A szuper operátoron keresztül lehet elérni az összes operátort.
    // Igazat ad vissza, ha az i.dik alap operátor alkalmazható a belső állapotra.
    // for ciklusból kell hívni 0-tól kezdve az alap operátorok számig. Pl. így:
    // for (int i = 0; i < állapot.GetNumberOfOps(); i++)
    // {
    // AbsztraktÁllapot klón=(AbsztraktÁllapot)állapot.Clone();
    // if (klón.SzuperOperátor(i))
    // {
    // Console.WriteLine("Az {0} állapotra az {1}.dik " +
    // "operátor alkalmazható", állapot, i);
    // }
    // }
    public abstract bool SzuperOperátor(int i);
    // Klónoz. Azért van rá szükség, mert némelyik operátor hatását vissza kell vonnunk.
    // A legegyszerűbb, hogy az állapotot leklónozom. Arra hívom az operátort.
    // Ha gond van, akkor visszatérek az eredeti állapothoz.
    // Ha nincs gond, akkor a klón lesz az állapot, amiből folytatom a keresést.
    // Ez sekély klónozást alkalmaz. Ha elég a sekély klónozás, akkor nem kell felülírni a gyermek osztályban.
    // Ha mély klónozás kell, akkor mindenképp felülírandó.
    public virtual object Clone() { return MemberwiseClone(); }
    // Csak akkor kell felülírni, ha emlékezetes backtracket akarunk használni, vagy mélységi keresést.
    // Egyébként maradhat ez az alap implementáció.
    // Programozás technikailag ez egy kampó metódus, amit az OCP megszegése nélkül írhatok felül.
    public override bool Equals(Object a) { return false; }
    // Ha két példány egyenlő, akkor a hasítókódjuk is egyenlő.
    // Ezért, ha valaki felülírja az Equals metódust, ezt is illik felülírni.
    public override int GetHashCode() { return base.GetHashCode(); }
}





// 2.40
class DiscFlipper : AbsztraktÁllapot
{

    private string discs;

    public DiscFlipper(string discOrder)
    {
        // Kezdetben minden korong piros oldala van felfelé, kivéve a jelölt korongot a 13. pozícióban.
        // @isti: itt egy "collection expression"-el hoztam létre a tömböt, ez tetszeni fog a tanárnak, mert olyan metodika,
        // amit nem ismer, cserébe átlátható :)
        discs = discOrder;
        Console.WriteLine("Kezdeti állapot: " + this.ToString());
    }

    public override bool ÁllapotE()
    {
        // Nincsenek különösebb feltételek megadva, illetve az operátor művelet is úgy van megírva, hogy ne lépjen ki az állapottérből, így egy alapvizsgálat
        // lett megadva, igaz, ha összesen 13 korongunk van
        return discs.Length == 13;
    }
    public override bool CélÁllapotE()
    {
        // A célállapot az, hogy minden korong piros oldala van felfelé, kivéve a korongot az 1. pozícióban

        return discs == "KPPPPPPPPPPPP";
    }

    private bool PreOp(int startPosition)
    {
        // Nincs a feladatban különösebb bemenő/előzetes feltétel, így csak azt vizsgálom, hogy a startposition a tömb határain belül van-e
        return startPosition < discs.Length;
    }

    private char flipString(char Color)
    {
        if (Color == char.Parse("P"))
        {
            return char.Parse("K");
        }

        return char.Parse("P");
    }
    private bool op(int startPosition)
    {
        if (!PreOp(startPosition)) return false;
        DiscFlipper mentes = (DiscFlipper)Clone();

        Console.WriteLine();
        Console.WriteLine("Kiinduló helyzet:        " + this.ToString());
        Console.WriteLine("Fordítás pozíciója:      " + (startPosition+1));
        for (int i = 0; i < 4; i++)
        {
            // A korongok megfordítása, az óramutató járásával megegyező irányban figyelve arra, hogy a sor végére érve az elejéről kezdjük
            // a körkörös elrendezést szimulálva
            StringBuilder stringBuilder = new StringBuilder(discs);
            char discAtPosition = discs[(startPosition + i) % 13];
            char flippedDisc = flipString(discAtPosition);
            stringBuilder[(startPosition + i) % 13] = flippedDisc;
            discs = stringBuilder.ToString();
        }
        Console.WriteLine("Fordítás utáni helyzet:  " + this.ToString());
        Console.WriteLine();

        
        if (ÁllapotE())
        {
            return true;
        }

        this.discs = mentes.discs;
        return false;

    }
    public override int OperátorokSzáma()
    {
        return 13;
    }

    public override bool SzuperOperátor(int i)
    {
        switch (i)
        {
            case 0: return op(0);
            case 1: return op(1);
            case 2: return op(2);
            case 3: return op(3);
            case 4: return op(4);
            case 5: return op(5);
            case 6: return op(6);
            case 7: return op(7);
            case 8: return op(8);
            case 9: return op(9);
            case 10: return op(10);
            case 11: return op(11);
            case 12: return op(12);
            //case 13: return op(13); // a 13-as index az a 0-s index. Nem 14 operátorod van, hanem csak 13
            default: return false;
        }
    }
    public override string ToString()
    {
        return discs;
    }

    public override bool Equals(object a)
    {
        DiscFlipper aa = (DiscFlipper)a;
        return aa.discs == discs;
    }

    public override int GetHashCode()
    {
        return discs.GetHashCode();
    }
}


/// <summary>
/// A csúcs tartalmaz egy állapotot, a csúcs mélységét, és a csúcs szülőjét.
/// Így egy csúcs egy egész utat reprezentál a start csúcsig.
/// </summary>
class Csúcs
{
    // A csúcs tartalmaz egy állapotot, a mélységét és a szülőjét
    AbsztraktÁllapot állapot;
    int mélység;
    Csúcs szülő; // A szülőkön felfelé haladva a start csúcsig jutok.
                 // Konstruktor:
                 // A belső állapotot beállítja a start csúcsra.
                 // A hívó felelőssége, hogy a kezdő állapottal hívja meg.
                 // A start csúcs mélysége 0, szülője nincs.
    public Csúcs(AbsztraktÁllapot kezdőÁllapot)
    {
        állapot = kezdőÁllapot;
        mélység = 0;
        szülő = null;
    }
    // Egy új gyermek csúcsot készít.
    // Erre még meg kell hívni egy alkalmazható operátor is, csak azután lesz kész.
    public Csúcs(Csúcs szülő)
    {
        állapot = (AbsztraktÁllapot)szülő.állapot.Clone();
        mélység = szülő.mélység + 1;
        this.szülő = szülő;
    }
    public Csúcs GetSzülő() { return szülő; }
    public int GetMélység() { return mélység; }
    public bool TerminálisCsúcsE() { return állapot.CélÁllapotE(); }
    public int OperátorokSzáma() { return állapot.OperátorokSzáma(); }
    public bool SzuperOperátor(int i) { return állapot.SzuperOperátor(i); }
    public override bool Equals(Object obj)
    {
        Csúcs cs = (Csúcs)obj;
        return állapot.Equals(cs.állapot);
    }
    public override int GetHashCode() { return állapot.GetHashCode(); }
    public override String ToString() { return "-> " + állapot.ToString(); }
    // Alkalmazza az összes alkalmazható operátort.
    // Visszaadja az így előálló új csúcsokat.
    public List<Csúcs> Kiterjesztes()
    {
        List<Csúcs> újCsúcsok = new List<Csúcs>();
        for (int i = 0; i < OperátorokSzáma(); i++)
        {
            // Új gyermek csúcsot készítek.
            Csúcs újCsúcs = new Csúcs(this);
            // Kiprobálom az i.dik alapoperátort. Alkalmazható?
            if (újCsúcs.SzuperOperátor(i))
            {
                // Ha igen, hozzáadom az újakhoz.
                újCsúcsok.Add(újCsúcs);
            }
        }
        return újCsúcsok;
    }
}

/// <summary>
/// Minden gráfkereső algoritmus őse.
/// A gráfkeresőknek csak a Keresés metódust kell megvalósítaniuk.
/// Ez visszaad egy terminális csúcsot, ha talált megoldást, egyébként null értékkel tér vissza.
/// A terminális csúcsból a szülő referenciákon felfelé haladva áll elő a megoldás.
/// </summary>
abstract class GráfKereső
{
    private Csúcs startCsúcs; // A start csúcs csúcs.
                              // Minden gráfkereső a start csúcsból kezd el keresni.
    public GráfKereső(Csúcs startCsúcs)
    {
        this.startCsúcs = startCsúcs;
    }
    // Jobb, ha a start csúcs privát, de a gyermek osztályok lekérhetik.
    protected Csúcs GetStartCsúcs() { return startCsúcs; }
    /// Ha van megoldás, azaz van olyan út az állapottér gráfban,
    /// ami a start csúcsból egy terminális csúcsba vezet,
    /// akkor visszaad egy megoldást, egyébként null.
    /// A megoldást egy terminális csúcsként adja vissza.
    /// Ezen csúcs szülő referenciáin felfelé haladva adódik a megoldás fordított sorrendben.
    public abstract Csúcs Keresés();
    /// <summary>
    /// Kiíratja a megoldást egy terminális csúcs alapján.
    /// Feltételezi, hogy a terminális csúcs szülő referenciáján felfelé haladva eljutunk a start csúcshoz.
    /// A csúcsok sorrendjét megfordítja, hogy helyesen tudja kiírni a megoldást.
    /// Ha a csúcs null, akkor kiírja, hogy nincs megoldás.
    /// </summary>
    /// <param name="egyTerminálisCsúcs">
    /// A megoldást képviselő terminális csúcs vagy null.
    /// </param>
    public void megoldásKiírása(Csúcs egyTerminálisCsúcs)
    {
        if (egyTerminálisCsúcs == null)
        {
            Console.WriteLine("Nincs megoldás");
            return;
        }
        // Meg kell fordítani a csúcsok sorrendjét.
        Stack<Csúcs> megoldás = new Stack<Csúcs>();
        Csúcs aktCsúcs = egyTerminálisCsúcs;
        while (aktCsúcs != null)
        {
            megoldás.Push(aktCsúcs);
            aktCsúcs = aktCsúcs.GetSzülő();
        }
        // Megfordítottuk, lehet kiírni.
        foreach (Csúcs akt in megoldás) Console.WriteLine(akt);
    }
}

/// <summary>
/// A backtrack gráfkereső algoritmust megvalósító osztály.
/// A három alap backtrack algoritmust egyben tartalmazza. Ezek
/// - az alap backtrack
/// - mélységi korlátos backtrack
/// - emlékezetes backtrack
/// Az ág-korlátos backtrack nincs megvalósítva.
/// </summary>
class BackTrack : GráfKereső
{
    int korlát; // Ha nem nulla, akkor mélységi korlátos kereső.
    bool emlékezetes; // Ha igaz, emlékezetes kereső.
    public BackTrack(Csúcs startCsúcs, int korlát, bool emlékezetes) : base(startCsúcs)
    {
        this.korlát = korlát;
        this.emlékezetes = emlékezetes;
    }
    // nincs mélységi korlát, se emlékezet
    public BackTrack(Csúcs startCsúcs) : this(startCsúcs, 0, false) { }
    // mélységi korlátos kereső
    public BackTrack(Csúcs startCsúcs, int korlát) : this(startCsúcs, korlát, false) { }
    // emlékezetes kereső
    public BackTrack(Csúcs startCsúcs, bool emlékezetes) : this(startCsúcs, 0, emlékezetes) { }
    // A keresés a start csúcsból indul.
    // Egy terminális csúcsot ad vissza. A start csúcsból el lehet jutni ebbe a terminális csúcsba.
    // Ha nincs ilyen, akkor null értéket ad vissza.
    public override Csúcs Keresés() { return Keresés(GetStartCsúcs()); }
    // A kereső algoritmus rekurzív megvalósítása.
    // Mivel rekurzív, ezért a visszalépésnek a "return null" felel meg.
    private Csúcs Keresés(Csúcs aktCsúcs)
    {
        int mélység = aktCsúcs.GetMélység();
        // mélységi korlát vizsgálata
        if (korlát > 0 && mélység >= korlát) return null;
        // emlékezet használata kör kiszűréséhez
        Csúcs aktSzülő = null;
        if (emlékezetes) aktSzülő = aktCsúcs.GetSzülő();
        while (aktSzülő != null)
        {
            // Ellenőrzöm, hogy jártam-e ebben az állapotban. Ha igen, akkor visszalépés.
            if (aktCsúcs.Equals(aktSzülő)) return null;
            // Visszafelé haladás a szülői láncon.
            aktSzülő = aktSzülő.GetSzülő();
        }
        if (aktCsúcs.TerminálisCsúcsE())
        {
            // Megvan a megoldás, vissza kell adni a terminális csúcsot.
            return aktCsúcs;
        }
        // Itt hívogatom az alapoperátorokat a szuper operátoron
        // keresztül. Ha valamelyik alkalmazható, akkor új csúcsot
        // készítek, és meghívom önmagamat rekurzívan.
        for (int i = 0; i < aktCsúcs.OperátorokSzáma(); i++)
        {
            // Elkészítem az új gyermek csúcsot.
            // Ez csak akkor lesz kész, ha alkalmazok rá egy alkalmazható operátort is.
            Csúcs újCsúcs = new Csúcs(aktCsúcs);
            // Kipróbálom az i.dik alapoperátort. Alkalmazható?
            if (újCsúcs.SzuperOperátor(i))
            {
                // Ha igen, rekurzívan meghívni önmagam az új csúcsra.
                // Ha nem null értéket ad vissza, akkor megvan a megoldás.
                // Ha null értéket, akkor ki kell próbálni a következő alapoperátort.
                Csúcs terminális = Keresés(újCsúcs);
                if (terminális != null)
                {
                    // Visszaadom a megoldást képviselő terminális csúcsot.
                    return terminális;
                }
                // Az else ágon kellene visszavonni az operátort.
                // Erre akkor van szükség, ha az új gyermeket létrehozásában nem lenne klónozást.
                // Mivel klónoztam, ezért ez a rész üres.
            }
        }
        // Ha kipróbáltam az összes operátort és egyik se vezetett megoldásra, akkor visszalépés.
        // A visszalépés hatására eggyel feljebb a következő alapoperátor kerül sorra.
        return null;
    }
}

/// <summary>
/// Mélységi keresést megvalósító gráfkereső osztály.
/// Ez a megvalósítása a mélységi keresésnek felteszi, hogy a start csúcs nem terminális csúcs.
/// A nyílt csúcsokat veremben tárolja.
/// </summary>
class MélységiKeresés : GráfKereső
{
    // Mélységi keresésnél érdemes a nyílt csúcsokat veremben tárolni,
    // mert így mindig a legnagyobb mélységű csúcs lesz a verem tetején.
    // Így nem kell külön keresni a legnagyobb mélységű nyílt csúcsot, amit ki kell terjeszteni.
    Stack<Csúcs> Nyilt; // Nílt csúcsok halmaza.
    List<Csúcs> Zárt; // Zárt csúcsok halmaza.
    bool körFigyelés; // Ha hamis, végtelen ciklusba eshet.
    public MélységiKeresés(Csúcs startCsúcs, bool körFigyelés) :
    base(startCsúcs)
    {
        Nyilt = new Stack<Csúcs>();
        Nyilt.Push(startCsúcs); // kezdetben csak a start csúcs nyílt
        Zárt = new List<Csúcs>(); // kezdetben a zárt csúcsok halmaza üres
        this.körFigyelés = körFigyelés;
    }
    // A körfigyelés alapértelmezett értéke igaz.
    public MélységiKeresés(Csúcs startCsúcs) : this(startCsúcs, true) { }
    // A megoldás keresés itt indul.
    public override Csúcs Keresés()
    {
        // Ha nem kell körfigyelés, akkor sokkal gyorsabb az algoritmus.
        if (körFigyelés) return TerminálisCsúcsKeresés();
        return TerminálisCsúcsKeresésGyorsan();
    }
    private Csúcs TerminálisCsúcsKeresés()
    {
        // Amíg a nyílt csúcsok halmaza nem nem üres.
        while (Nyilt.Count != 0)
        {
            // Ez a legnagyobb mélységű nyílt csúcs.
            Csúcs C = Nyilt.Pop();
            // Ezt kiterjesztem.
            List<Csúcs> újCsucsok = C.Kiterjesztes();
            foreach (Csúcs D in újCsucsok)
            {
                // Ha megtaláltam a terminális csúcsot, akkor kész vagyok.
                if (D.TerminálisCsúcsE()) return D;
                // Csak azokat az új csúcsokat veszem fel a nyíltak közé,
                // amik nem szerepeltek még sem a zárt, sem a nyílt csúcsok halmazában.
                // A Contains a Csúcs osztályban megírt Equals metódust hívja.
                if (!Zárt.Contains(D) && !Nyilt.Contains(D)) Nyilt.Push(D);
            }
            // A kiterjesztett csúcsot átminősítem zárttá.
            Zárt.Add(C);
        }
        return null;
    }
    // Ezt csak akkor szabad használni, ha biztos, hogy az állapottér gráfban nincs kör!
    // Különben valószínűleg végtelen ciklust okoz.
    private Csúcs TerminálisCsúcsKeresésGyorsan()
    {
        while (Nyilt.Count != 0)
        {
            Csúcs C = Nyilt.Pop();
            List<Csúcs> ujCsucsok = C.Kiterjesztes();
            foreach (Csúcs D in ujCsucsok)
            {
                if (D.TerminálisCsúcsE()) return D;
                // Ha nincs kör, akkor felesleges megnézni, hogy D volt-e már nyíltak vagy a zártak közt.
                Nyilt.Push(D);
            }
            // Ha nincs kör, akkor felesleges C-t zárttá minősíteni.
        }
        return null;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Csúcs startCsúcs;
        GráfKereső kereső;

        Console.WriteLine("Korongforgató 2.40-es feladat megoldása");
        startCsúcs = new Csúcs(new DiscFlipper("PPPPPPPPPPPPK"));
        Console.WriteLine("A kereső egy mélységi keresés körfigyeléssel.");
        kereső = new MélységiKeresés(startCsúcs, true);
        kereső.megoldásKiírása(kereső.Keresés());
        Console.ReadLine();
    }
}