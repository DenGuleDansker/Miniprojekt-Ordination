namespace ordination_test;

using shared.Model;

[TestClass]
public class PNTest
{

    [TestMethod]
    public void GivDosisTesting()
    {
        //Vores gyldige data, alle Testcase skal vise have en bool som viser true, hvis den er givet indenfor den gyldige periode.
        //Vores TC's er fra starten, midten og slutningen af den gyldige periode.

        //TC1 tester starten af perioden
        PN TC1 = new PN(new DateTime(2023, 11, 01), new DateTime(2023, 11, 10), 123, new Laegemiddel("Paracematol", 0.5, 0.5, 0.5, "Styk"));

        bool Dosis_T1 = TC1.givDosis(new Dato { dato = new DateTime(2023, 11, 01).Date });

        Assert.AreEqual(true, Dosis_T1);

        //TC2 tester midten af perioden
        PN TC2 = new PN(new DateTime(2023, 11, 01), new DateTime(2023, 11, 10), 123, new Laegemiddel("Paracematol", 0.5, 0.5, 0.5, "Styk"));

        bool Dosis_T2 = TC2.givDosis(new Dato { dato = new DateTime(2023, 11, 05).Date });

        Assert.AreEqual(true, Dosis_T2);

        //TC3 tester dagen inden slutningen af perioden
        PN TC3 = new PN(new DateTime(2023, 11, 01), new DateTime(2023, 11, 10), 123, new Laegemiddel("Paracematol", 0.5, 0.5, 0.025, "Styk"));

        bool Dosis_T3 = TC3.givDosis(new Dato { dato = new DateTime(2023, 11, 10).Date });

        Assert.AreEqual(true, Dosis_T3);

        //Vores ugyldige data, disse testcase skal have en bool, som viser false, når den bliver givet datoer udenfor den gyldige periode.

        //TC4 - tester om dosis kan gives inden starten af perioden.
        PN TC4 = new PN(new DateTime(2023, 11, 01), new DateTime(2023, 11, 10), 123, new Laegemiddel("Paracematol", 0.5, 0.5, 0.025, "Styk"));

        bool Dosis_T4 = TC4.givDosis(new Dato { dato = new DateTime(2023, 10, 31).Date });

        Assert.AreEqual(false, Dosis_T4);

        //TC4 - tester om dosis kan gives inden starten af perioden.
        PN TC5 = new PN(new DateTime(2023, 11, 01), new DateTime(2023, 11, 10), 123, new Laegemiddel("Paracematol", 0.5, 0.5, 0.025, "Styk"));

        bool Dosis_T5 = TC5.givDosis(new Dato { dato = new DateTime(2023, 10, 31).Date });

        Assert.AreEqual(false, Dosis_T4);
    }
}