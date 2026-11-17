using Core.DAO;

Console.WriteLine("Sajam knjiga");
PosetilacDAO posetilacDAO = new PosetilacDAO();

Console.WriteLine("Broj posetilaca: " + posetilacDAO.GetAll().Count);
