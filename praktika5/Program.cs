using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serialize_People
{
  [Serializable]
  class Person : IDeserializationCallback
  {
    public string name;
    public DateTime dateOfBirth;
    [NonSerialized] public int age;

    public Person(string _name, DateTime _dateOfBirth)
    {
      name = _name;
      dateOfBirth = _dateOfBirth;
      CalculateAge();
    }

    public Person()
    {
    }

    public override string ToString()
    {
      return name + " was born on " + dateOfBirth.ToShortDateString() + " and is " + age.ToString() + " years old.";
    }

    private void CalculateAge()
    {
      age = DateTime.Now.Year - dateOfBirth.Year;

      // If they haven't had their birthday this year, 
      // subtract a year from their age
      if (dateOfBirth.AddYears(DateTime.Now.Year - dateOfBirth.Year) > DateTime.Now)
      {
        age--;
      }
    }
    // C#
    void IDeserializationCallback.OnDeserialization(Object sender)
    {
      CalculateAge();
      Console.WriteLine("calc");
    }

  }

}

namespace Serialize_People
{
  // A simple program that accepts a name, year, month date,
  // creates a Person object from that information, 
  // and then displays that person's age on the console.
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length == 0)
      {
        // If they provide no arguments, display the last person
        Person p = Deserialize();
        Console.WriteLine(p.ToString());
      }
      else
      {
        try
        {
          if (args.Length != 4)
          {
            throw new ArgumentException("You must provide four arguments.");
          }

          DateTime dob = new DateTime(Int32.Parse(args[1]), Int32.Parse(args[2]), Int32.Parse(args[3]));
          Person p = new Person(args[0], dob);
          Console.WriteLine(p.ToString());

          Serialize(p);
        }
        catch (Exception ex)
        {
          DisplayUsageInformation(ex.Message);
        }
      }
    }

    private static void DisplayUsageInformation(string message)
    {
      Console.WriteLine("\nERROR: Invalid parameters. " + message);
      Console.WriteLine("\nSerialize_People \"Name\" Year Month Date");
      Console.WriteLine("\nFor example:\nSerialize_People \"Tony\" 1922 11 22");
      Console.WriteLine("\nOr, run the command with no arguments to display that previous person.");
    }

    private static void Serialize(Person sp)
    {
      // TODO: Serialize sp object
      // Create file to save the data to
      FileStream fs = new FileStream("Person.Dat", FileMode.Create);

      // Create a BinaryFormatter object to perform the serialization
      BinaryFormatter bf = new BinaryFormatter();

      // Use the BinaryFormatter object to serialize the data to the file
      bf.Serialize(fs, sp);

      // Close the file
      fs.Close();

    }

    private static Person Deserialize()
    {
      Person dsp = new Person();
      return dsp;

    }
  }
}
