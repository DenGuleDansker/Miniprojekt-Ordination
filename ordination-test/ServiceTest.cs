namespace ordination_test;

using Microsoft.EntityFrameworkCore;

using Service;
using Data;
using shared.Model;

[TestClass]
public class ServiceTest
{
    private DataService service;

    [TestInitialize]
    public void SetupBeforeEachTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: "test-database");
        var context = new OrdinationContext(optionsBuilder.Options);
        service = new DataService(context);
        service.SeedData();
    }

    [TestMethod]
    public void PatientsExist()
    {
        Assert.IsNotNull(service.GetPatienter());
    }

    [TestMethod]
    public void OpretDagligFast()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(1, service.GetDagligFaste().Count());

        service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligFaste().Count());
    }

    [TestMethod]
    public void OpretDagligSkaev()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();


        Assert.AreEqual(1, service.GetDagligSkæve().Count());

        service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId,
            new Dosis[]
        {
            new Dosis(DateTime.Now, 0.5),
        new Dosis(DateTime.Now.AddHours(1), 1),
        new Dosis(DateTime.Now.AddHours(2), 2.5),
        new Dosis(DateTime.Now.AddHours(3), 3),
    }, DateTime.Now, DateTime.Now.AddDays(3));
        Assert.AreEqual(2, service.GetDagligSkæve().Count());
 

    }

    [TestMethod]

    public void TestOpretPN()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(1, service.GetPNs().Count());

        service.OpretPN(patient.PatientId, lm.LaegemiddelId,
                       0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetPNs().Count());
    }   




    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestAtKodenSmiderEnException()
    {
        // Herunder skal man så kalde noget kode,
        // der smider en exception.


        service.OpretPN(-1, 2,
        0, DateTime.Now, DateTime.Now.AddDays(3));


        // Hvis koden _ikke_ smider en exception,
        // så fejler testen.

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestAtLægemiddelSmiderNull()
    {
        // Herunder skal man så kalde noget kode,
        // der smider en exception.


        service.OpretPN(1, -1, 0, DateTime.Now, DateTime.Now.AddDays(3));


        // Hvis koden _ikke_ smider en exception,
        // så fejler testen.

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }
}