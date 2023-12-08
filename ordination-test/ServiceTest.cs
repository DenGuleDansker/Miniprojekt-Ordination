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
        optionsBuilder.UseInMemoryDatabase(databaseName: $"test-database-{DateTime.UtcNow.Ticks}");
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
    [ExpectedException(typeof(ArgumentNullException))]
    public void OpretDagligeFastException()
    {
        service.OpretDagligFast(0, 1, 2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void OpretDagligeSkævDatoException()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId,
           new Dosis[]
       {
            new Dosis(DateTime.Now, 0.5),
        new Dosis(DateTime.Now.AddHours(1), 1),
        new Dosis(DateTime.Now.AddHours(2), 2.5),
        new Dosis(DateTime.Now.AddHours(3), 3),
   }, new DateTime(2023, 12, 3), new DateTime(2023, 12, 1));
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

        Assert.AreEqual(4, service.GetPNs().Count());

        service.OpretPN(patient.PatientId, lm.LaegemiddelId,
                       0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(5, service.GetPNs().Count());
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
    public void TestOpretPN_PatientID()
    {
        // Arrange
        Laegemiddel lm = service.GetLaegemidler().First();

        // Act
        PN createdPN = service.OpretPN(1, lm.LaegemiddelId, 0, DateTime.Now, DateTime.Now.AddDays(3));

        // Assert
        Assert.IsNotNull(createdPN);
        
    }


    [TestMethod]
    public void TestOpretPN_Lægemiddel()
    {
        // Arrange
        Patient patient = service.GetPatienter().First();

        // Act
        PN createdPN = service.OpretPN(patient.PatientId, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        // Assert
        Assert.IsNotNull(createdPN);

    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestOpretPN_MinusAntal()
    {
        // Arrange
        Laegemiddel lm = service.GetLaegemidler().First();
        Patient patient = service.GetPatienter().First();

        // Act
        PN createdPN = service.OpretPN(patient.PatientId, lm.LaegemiddelId, -1, DateTime.Now, DateTime.Now.AddDays(3));

        // Assert
        Assert.AreEqual(new ArgumentNullException(), createdPN.antalEnheder);

    }

    [TestMethod]
    public void TestOpretPN_PlusAntal()
    {
        // Arrange
        Laegemiddel lm = service.GetLaegemidler().First();
        Patient patient = service.GetPatienter().First();

        // Act
        PN createdPN = service.OpretPN(patient.PatientId, lm.LaegemiddelId, 1, DateTime.Now, DateTime.Now.AddDays(3));

        // Assert
        Assert.IsNotNull(createdPN);

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


    [TestMethod]
    public void TestDoegnDosisDagligskaev()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId,
                       new Dosis[]
     {
        new Dosis(DateTime.Now, 0.5),
        new Dosis(DateTime.Now.AddHours(1), 1),
        new Dosis(DateTime.Now.AddHours(2), 2.5),
        new Dosis(DateTime.Now.AddHours(3), 3),
     }, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(7, service.GetDagligSkæve().First().doegnDosis());
    }

    [TestMethod]
    public void TestOpretDagligSkaev_PatientID()
    {
        // Arrange
        Laegemiddel lm = service.GetLaegemidler().First();

        // Act
        DagligSkæv createdDagliSkaev = service.OpretDagligSkaev(1, lm.LaegemiddelId,
                       new Dosis[]
     {
            new Dosis(DateTime.Now, 0.5),
        new Dosis(DateTime.Now.AddHours(1), 1),
        new Dosis(DateTime.Now.AddHours(2), 2.5),
        new Dosis(DateTime.Now.AddHours(3), 3),
     }, DateTime.Now, DateTime.Now.AddDays(3));

        // Assert
        Assert.IsNotNull(createdDagliSkaev);

    }

    [TestMethod]
    public void TestOpretSkæv_Lægemiddel()
    {
        // Arrange
        Patient patient = service.GetPatienter().First();

        // Act
        DagligSkæv createdDagliSkaev = service.OpretDagligSkaev(patient.PatientId, 1,
                       new Dosis[]
     {
            new Dosis(DateTime.Now, 0.5),
        new Dosis(DateTime.Now.AddHours(1), 1),
        new Dosis(DateTime.Now.AddHours(2), 2.5),
        new Dosis(DateTime.Now.AddHours(3), 3),
     }, DateTime.Now, DateTime.Now.AddDays(3));

        // Assert
        Assert.IsNotNull(createdDagliSkaev);

    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestNegativDosisDagligskaev()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId,
                       new Dosis[]
     {
        new Dosis(DateTime.Now, -1),
        new Dosis(DateTime.Now.AddHours(1), 0),
        new Dosis(DateTime.Now.AddHours(2), 0),
        new Dosis(DateTime.Now.AddHours(3), 0),
     }, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(new ArgumentNullException(), service.GetDagligSkæve().First().doegnDosis());
    }







    [TestMethod]
    public void AntalDageOrdinationTest()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        var test = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId, 2, 2, 1, 1, new DateTime(2023, 12, 1), new DateTime(2023, 12, 10));

        Assert.AreEqual(10, test.antalDage());
        
    }

    [TestMethod]
    public void TestOpretDagligFast_PatientID()
    {
        // Arrange
        Laegemiddel lm = service.GetLaegemidler().First();

        // Act
        DagligFast createdFast = service.OpretDagligFast(1, lm.LaegemiddelId, 0, 0, 0, 0, DateTime.Now, DateTime.Now.AddDays(3));

        // Assert
        Assert.IsNotNull(createdFast);

    }

    [TestMethod]
    public void TestDagligFast_Lægemiddel()
    {
        // Arrange
        Patient patient = service.GetPatienter().First();

        // Act
        DagligFast createdFast = service.OpretDagligFast(patient.PatientId, 1, 0, 0, 0, 0, DateTime.Now, DateTime.Now.AddDays(3));

        // Assert
        Assert.IsNotNull(createdFast);

    }

    [TestMethod]
    public void TestDagligFast_MinusAntal()
    {
        // Arrange
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        // Act
        DagligFast createdFast = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId, -1, -1, -1, -1, DateTime.Now, DateTime.Now.AddDays(3));

        // Assert
        Assert.IsNull(createdFast);

    }

    [TestMethod]
    public void TestDagligFast_PlusAntal()
    {
        // Arrange
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        // Act
        DagligFast createdFast = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId, 1, 1, 1, 1, DateTime.Now, DateTime.Now.AddDays(3));

        // Assert
        Assert.IsNotNull(createdFast);

    }
}







