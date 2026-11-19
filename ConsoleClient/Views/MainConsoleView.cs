
using Core.DAO;
using Core.Services;

namespace ConsoleClient.Views;

public class MainConsoleView
{
    private readonly AdresaConsoleView _adresaView;
    private readonly AutorConsoleView _autorView;
    private readonly KnjigaConsoleView _knjigaView;
    private readonly PosetilacConsoleView _posetilacView;
    private readonly KupovinaConsoleView _kupovinaView;
    private readonly IzdavacConsoleView _izdavacView;
    private readonly KnjigaAutorConsoleView _knjigaAutorView;
    private readonly StavkaListeZeljaConsoleView _listaZeljaView;

    public MainConsoleView()
    {
        AdresaDAO adresaDao = new AdresaDAO();
        AutorDAO autorDao = new AutorDAO();
        KnjigaDAO knjigaDao = new KnjigaDAO();
        PosetilacDAO posetilacDao = new PosetilacDAO();
        KupovinaDAO kupovinaDao = new KupovinaDAO();
        IzdavacDAO izdavacDao = new IzdavacDAO();
        KnjigaAutorDAO knjigaAutorDao = new KnjigaAutorDAO();
        StavkaListeZeljaDAO stavkaListeZeljaDao = new StavkaListeZeljaDAO();

        LinkService linkService = new LinkService(adresaDao, autorDao, knjigaDao, posetilacDao,
            kupovinaDao, izdavacDao, knjigaAutorDao, stavkaListeZeljaDao);
        linkService.LinkAll();

        _adresaView = new AdresaConsoleView(adresaDao, posetilacDao, autorDao);
        _autorView = new AutorConsoleView(autorDao, adresaDao, knjigaAutorDao, izdavacDao, knjigaDao, linkService);
        _knjigaView = new KnjigaConsoleView(knjigaDao, izdavacDao, kupovinaDao, knjigaAutorDao, stavkaListeZeljaDao, linkService);
        _posetilacView = new PosetilacConsoleView(posetilacDao, adresaDao, kupovinaDao, stavkaListeZeljaDao, knjigaDao, linkService);
        _kupovinaView = new KupovinaConsoleView(kupovinaDao, posetilacDao, knjigaDao, linkService);
        _izdavacView = new IzdavacConsoleView(new PublisherService(izdavacDao, autorDao, knjigaDao), linkService, autorDao);
        _knjigaAutorView = new KnjigaAutorConsoleView(knjigaAutorDao, knjigaDao, autorDao, linkService);
        _listaZeljaView = new StavkaListeZeljaConsoleView(stavkaListeZeljaDao, posetilacDao, knjigaDao, linkService);
    }

    public void RunMenu()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== SAJAM KNJIGA ===");
            Console.WriteLine("1 - Adrese");
            Console.WriteLine("2 - Autori");
            Console.WriteLine("3 - Knjige");
            Console.WriteLine("4 - Posetioci");
            Console.WriteLine("5 - Kupovine");
            Console.WriteLine("6 - Izdavaci");
            Console.WriteLine("7 - Knjiga - autor");
            Console.WriteLine("8 - Lista zelja");
            Console.WriteLine("0 - Izlaz");
            Console.Write("Izbor: ");
            string izbor = Console.ReadLine() ?? "0";
            if (izbor == "0") return;
            ObradiIzbor(izbor);
        }
    }

    private void ObradiIzbor(string izbor)
    {
        switch (izbor)
        {
            case "1": _adresaView.RunMenu(); break;
            case "2": _autorView.RunMenu(); break;
            case "3": _knjigaView.RunMenu(); break;
            case "4": _posetilacView.RunMenu(); break;
            case "5": _kupovinaView.RunMenu(); break;
            case "6": _izdavacView.RunMenu(); break;
            case "7": _knjigaAutorView.RunMenu(); break;
            case "8": _listaZeljaView.RunMenu(); break;
            default: Console.WriteLine("Nepoznata opcija."); break;
        }
    }
}
