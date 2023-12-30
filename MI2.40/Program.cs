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

/// <summary>
/// A VakÁllapot csak a szemléltetés kedvért van itt.
/// Megmutatja, hogy kell az operátorokat megírni és bekötni a szuper operátorba.
/// </summary>
abstract class VakÁllapot : AbsztraktÁllapot
{
    // Itt kell megadni azokat a mezőket, amelyek tartalmazzák a belső állapotot.
    // Az operátorok belső állapot átmenetet hajtanak végre.
    // Először az alapoperátorokat kell megírni.
    // Minden operátornak van előfeltétele.
    // Minden operátor utófeltétele megegyezik az ÁllapotE predikátummal.
    // Az operátor igazat ad vissza, ha alkalmazható, hamisat, ha nem alkalmazható.
    // Egy operátor alkalmazható, ha a belső állapotra igaz
    // az előfeltétele és az állapotátmenet után igaz az utófeltétele.
    // Ez az első alapoperátor.
    private bool op1()
    {
        // Ha az előfeltétel hamis, akkor az operátor nem alkalmazható.
        if (!preOp1()) return false;
        // állapot átmenet
        //
        // TODO: Itt kell kiegészíteni a kódot!
        //
        // Utófeltétel vizsgálata, ha igaz, akkor alkalmazható az operátor.
        if (ÁllapotE()) return true;
        // Egyébként vissza kell vonni a belső állapot átmenetet,
        //
        // TODO: Itt kell kiegészíteni a kódot!
        //
        // és vissza kell adni, hogy nem alkalmazható az operátor.
        return false;
    }
    // Az első alapoperátor előfeltétele. Az előfeltétel neve általában ez: pre+operátor neve.
    // Ez segíti a kód megértését, de nyugodtan eltérhetünk ettől.
    private bool preOp1()
    {
        // Ha igaz az előfeltétel, akkor igazat ad vissza.
        return true;
    }
    // Egy másik operátor.
    private bool op2()
    {
        if (!preOp2()) return false;
        // Állapot átmenet:
        // TODO: Itt kell kiegészíteni a kódot!
        if (ÁllapotE()) return true;
        // Egyébként vissza kell vonni a belső állapot átmenetet:
        // TODO: Itt kell kiegészíteni a kódot!
        return false;
    }
    private bool preOp2()
    {
        // Ha igaz az előfeltétel, akkor igazat ad vissza.
        return true;
    }
    // Nézzük, mi a helyzet, ha az operátornak van paramétere.
    // Ilyenkor egy operátor több alapoperátornak felel meg.
    private bool op3(int i)
    {
        // Az előfeltételt ugyanazokkal a pereméterekkel kell hívni, mint magát az operátort.
        if (!preOp3(i)) return false;
        // Állapot átmenet:
        // TODO: Itt kell kiegészíteni a kódot!
        if (ÁllapotE()) return true;
        // egyébként vissza kell vonni a belső állapot átmenetet
        // TODO: Itt kell kiegészíteni a kódot!
        return false;
    }
    // Az előfeltétel paraméterlistája megegyezik az operátor paraméterlistájával.
    private bool preOp3(int i)
    {
        // Ha igaz az előfeltétel, akkor igazat ad vissza. Az előfeltétel függ a paraméterektől.
        return true;
    }
    // Ez a szuper operátor. Ezen keresztül lehet hívni az alapoperátorokat.
    // Az i paraméterrel mondjuk meg, hanyadik operátort akarjuk hívni.
    // Általában egy for ciklusból hívjuk, ami 0-tól az OperátorokSzáma()-ig fut.
    public override bool SzuperOperátor(int i)
    {
        switch (i)
        {
            // Itt kell felsorolnom az összes alapoperátort.
            // Ha egy új operátort veszek fel, akkor ide is fel kell venni.
            case 0: return op1();
            case 1: return op2();
            // A paraméteres operátorok több alap operátornak megfelelnek, ezért itt több sor is tartozik hozzájuk.
            // Hogy hány sor, az feladat függő.
            case 3: return op3(0);
            case 4: return op3(1);
            case 5: return op3(2);
            default: return false;
        }
    }
    // Visszaadja az alap operátorok számát.
    public override int OperátorokSzáma()
    {
        // Az utolsó case számát kell itt visszaadni.
        // Ha bővítjük az operátorok számát, ezt a számot is növelni kell.
        return 5;
    }
}


/// <summary>
/// Ez a “éhes huszár” probléma állapottér reprezentációja.
/// A huszárnak az állomás helyétől, a bal felső sarokból,
/// el kell jutnia a kantinba, ami a jobb alsó sarokban van.
/// A táblát egy (N+4)*(N+4) mátrixszal ábrázolom.
/// A külső 2 széles rész margó, a belső rész a tábla.
/// A margó használatával sokkal könnyebb megírni az ÁllapotE predikátumot.
/// A 0 jelentése üres. Az 1 jelentése, itt van a ló.
/// 3*3-mas tábla esetén a kezdő állapot:
/// 0,0,0,0,0,0,0
/// 0,0,0,0,0,0,0
/// 0,0,1,0,0,0,0
/// 0,0,0,0,0,0,0
/// 0,0,0,0,0,0,0
/// 0,0,0,0,0,0,0
/// 0,0,0,0,0,0,0
/// A fenti reprezentációból látszik, hogy elég csak a ló helyét nyilvántartani,
/// mert a táblán csak a ló van. Így a kezdő állpot (bal felső sarokból indulunk):
/// x = 2
/// y = 2
/// A célállapot (jobb alsó sarokba megyek):
/// x = N+1
/// y = N+1
/// Operátorok:
/// A lehetséges 8 ló lépés.
/// </summary>
class ÉhesHuszárÁllapot : AbsztraktÁllapot
{
    // Alapértelmezetten egy 3*3-as sakktáblán fut.
    static int N = 3;
    // A belső állapotot leíró mezők.
    int x, y;
    // Beállítja a kezdő állapotra a belső állapotot.
    public ÉhesHuszárÁllapot()
    {
        x = 2; // A bal felső sarokból indulunk, ami a margó
        y = 2; // miatt a (2,2) koordinátán van.
    }
    // Beállítja a kezdő állapotra a belső állapotot.
    // Itt lehet megadni a tábla méretét is.
    public ÉhesHuszárÁllapot(int n)
    {
        x = 2;
        y = 2;
        N = n;
    }
    public override bool CélÁllapotE()
    {
        // A jobb alsó sarok a margó miatt a (N+1,N+1) helyen van.
        return x == N + 1 && y == N + 1;
    }
    public override bool ÁllapotE()
    {
        // a ló nem a margon van
        return x >= 2 && y >= 2 && x <= N + 1 && y <= N + 1;
    }
    private bool preLóLépés(int x, int y)
    {
        // jó lólépés-e, ha nem akkor return false
        if (!(x * y == 2 || x * y == -2)) return false;
        return true;
    }
    /* Ez az operátorom, igaz ad vissza, ha alakalmazható, 
    * egyébként hamisat.
    * Paraméterek: 
    * x: x irányú elmozdulás
    * y: y irányú elmozdulás
    * Az előfeltétel ellenőrzi, hogy az elmozdulás lólépés-e.
    * Az utófeltétel ellenőrzi, hogy a táblán maradtunk-e.
    * Példa:
    * lóLépés(1,-2) jelentése felfelé 2-öt jobbra 1-et.
    */
    private bool lóLépés(int x, int y)
    {
        if (!preLóLépés(x, y)) return false;
        // Ha az előfeltétel igaz, akkor megcsinálom az
        // állapot átmenetet.
        this.x += x;
        this.y += y;
        // Az utófeltétel mindig megegyezik az ÁllapotE-vel.
        if (ÁllapotE()) return true;
        // Ha az utófeltétel nem igaz, akkor vissza kell csinálni.
        this.x -= x;
        this.y -= y;
        return false;
    }
    public override bool SzuperOperátor(int i)
    {
        switch (i)
        { // itt sorolom fel a lehetséges 8 lólépést
            case 0: return lóLépés(1, 2);
            case 1: return lóLépés(1, -2);
            case 2: return lóLépés(-1, 2);
            case 3: return lóLépés(-1, -2);
            case 4: return lóLépés(2, 1);
            case 5: return lóLépés(2, -1);
            case 6: return lóLépés(-2, 1);
            case 7: return lóLépés(-2, -1);
            default: return false;
        }
    }
    public override int OperátorokSzáma() { return 8; }
    // A kiíratásnál kivonom x-ből és y-ból a margó szélességét.
    public override string ToString() { return (x - 2) + " : " + (y - 2); }
    public override bool Equals(Object a)
    {
        ÉhesHuszárÁllapot aa = (ÉhesHuszárÁllapot)a;
        return aa.x == x && aa.y == y;
    }
    // Ha két példány egyenlő, akkor a hasítókódjuk is egyenlő.
    public override int GetHashCode()
    {
        return x.GetHashCode() + y.GetHashCode();
    }
}


/// <summary>
/// A "3 szerzetes és 3 kannibál" probléma állapottér reprezentációja.
/// Illetve általánosítása akárhány szerzetesre és kannibálra.
/// Probléma: 3 szerzet és 3 kannibál van a folyó bal partján.
/// Át kell juttatni az összes embert a másik partra.
/// Ehhez rendelkezésre áll egy két személyes csónak.
/// Egy ember is elég az átjutáshoz, de kettőnél több ember nem fér el.
/// Ha valaki átmegy a másik oldalra, akkor ki is kell szállni, nem maradhat a csónakban.
/// A gond ott van, hogy ha valamelyik parton több kannibál van,
/// mint szerzetes, akkor a kannibálok megeszik a szerzeteseket.
/// Kezdő állapot:
/// 3 szerzetes a bal oldalon.
/// 3 kannibál a bal oldalon.
/// A csónak a bal parton van.
/// 0 szerzetes a jobb oldalon.
/// 0 kannibál a jobb oldalon.
/// Ezt az állapotot ezzel a rendezett 5-össel írjuk le:
/// (3,3,'B',0,0)
/// A célállapot:
/// (0,0,'J',3,3)
/// Operátor:
/// Op(int sz, int k):
/// sz darab szerzetes átmegy a másik oldalra és
/// k darab kannibál átmegy a másik oldalra.
/// Lehetséges paraméterezése:
/// Op(1,0): 1 szerzetes átmegy a másik oldalra.
/// Op(2,0): 2 szerzetes átmegy a másik oldalra.
/// Op(1,1): 1 szerzetes és 1 kannibál átmegy a másik oldalra.
/// Op(0,1): 1 kannibál átmegy a másik oldalra.
/// Op(0,2): 2 kanibál átmegy a másik oldalra.
/// </summary>
class SzerzetesekÉsKannibálokÁllapot : AbsztraktÁllapot
{
    int sz; // ennyi szerzetes van összesen
    int k; // ennyi kannibál van összesen
    int szb; // szerzetesek száma a bal oldalon
    int kb; // kannibálok száma a bal oldalon
    char cs; // Hol a csónak? Értéke vagy 'B' vagy 'J'.
    int szj; // szerzetesek száma a jobb oldalon
    int kj; // kannibálok száma a jobb oldalon
    public SzerzetesekÉsKannibálokÁllapot(int sz, int k) // beállítja a kezdő állapotot
    {
        this.sz = sz;
        this.k = k;
        szb = sz;
        kb = k;
        cs = 'B';
        szj = 0;
        kj = 0;
    }
    public override bool ÁllapotE()
    { // igaz, ha a szerzetesek biztonságban vannak
        return ((szb >= kb) || (szb == 0)) &&
        ((szj >= kj) || (szj == 0));
    }
    public override bool CélÁllapotE()
    {
        // igaz, ha mindenki átért a jobb oldalra
        return szj == sz && kj == k;
    }
    private bool preOp(int sz, int k)
    {
        // A csónak 2 személyes, legalább egy ember kell az evezéshez.
        // Ezt végül is felesleges vizsgálni, mert a szuper operátor csak ennek megfelelően hívja.
        if (sz + k > 2 || sz + k < 0 || sz < 0 || k < 0) return false;
        // Csak akkor lehet átvinni sz szerzetest és
        // k kannibált, ha a csónak oldalán van is legalább ennyi.
        if (cs == 'B')
            return szb >= sz && kb >= k;
        else
            return szj >= sz && kj >= k;
    }
    // Átvisz a másik oldalra sz darab szerzetes és k darab kannibált.
    private bool op(int sz, int k)
    {
        if (!preOp(sz, k)) return false;
        SzerzetesekÉsKannibálokÁllapot mentes = (SzerzetesekÉsKannibálokÁllapot)Clone();
        if (cs == 'B')
        {
            szb -= sz;
            kb -= k;
            cs = 'J';
            szj += sz;
            kj += k;
        }
        else
        {
            szb += sz;
            kb += k;
            cs = 'B';
            szj -= sz;
            kj -= k;
        }
        if (ÁllapotE()) return true;
        szb = mentes.szb;
        kb = mentes.kb;
        cs = mentes.cs;
        szj = mentes.szj;
        kj = mentes.kj;
        return false;
    }
    public override int OperátorokSzáma() { return 5; }
    public override bool SzuperOperátor(int i)
    {
        switch (i)
        {
            case 0: return op(0, 1);
            case 1: return op(0, 2);
            case 2: return op(1, 1);
            case 3: return op(1, 0);
            case 4: return op(2, 0);
            default: return false;
        }
    }
    public override string ToString()
    {
        return szb + "," + kb + "," + cs + "," + szj + "," + kj;
    }
    public override bool Equals(Object a)
    {
        SzerzetesekÉsKannibálokÁllapot aa = (SzerzetesekÉsKannibálokÁllapot)a;
        // szj és kj számítható, ezért nem kell vizsgálni
        return aa.szb == szb && aa.kb == kb && aa.cs == cs;
    }
    // Ha két példány egyenlő, akkor a hasítókódjuk is egyenlő.
    public override int GetHashCode()
    {
        return szb.GetHashCode() + kb.GetHashCode() + cs.GetHashCode();
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
    public override String ToString() { return állapot.ToString(); }
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
        Console.WriteLine("Az éhes huszár problémát megoldjuk 4x4-es táblán.");
        startCsúcs = new Csúcs(new ÉhesHuszárÁllapot(4));
        Console.WriteLine("A kereső egy 10 mélységi korlátos és emlékezetes backtrack.");
        kereső = new BackTrack(startCsúcs, 10, true);
        kereső.megoldásKiírása(kereső.Keresés());
        Console.WriteLine("A kereső egy mélységi keresés körfigyeléssel.");
        kereső = new MélységiKeresés(startCsúcs, true);
        kereső.megoldásKiírása(kereső.Keresés());
        Console.WriteLine("A 3 szerzetes 3 kanibál problémát oldjuk meg.");
        startCsúcs = new Csúcs(new SzerzetesekÉsKannibálokÁllapot(3, 3));
        Console.WriteLine("A kereső egy 15 mélységi korlátos és emlékezetes backtrack.");
        kereső = new BackTrack(startCsúcs, 15, true);
        kereső.megoldásKiírása(kereső.Keresés());
        Console.WriteLine("A kereső egy mélységi keresés körfigyeléssel.");
        kereső = new MélységiKeresés(startCsúcs, true);
        kereső.megoldásKiírása(kereső.Keresés());
        Console.ReadLine();
    }
}
